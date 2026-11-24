using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using HallenSoder.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace HallenSoder.Client.Data;

public record Session(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAt, string? Email);

public class TokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = "";
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    [JsonPropertyName("user")] public UserInfo? User { get; set; }
}

public class UserInfo
{
    [JsonPropertyName("email")] public string? Email { get; set; }
}

/// <summary>
/// Minimal klient mot Supabase (inloggning + events-tabellen) via REST.
/// Url och nyckel läses från wwwroot/appsettings.json. Nyckeln är den publika
/// "anon"/"publishable"-nyckeln – aldrig service_role.
/// </summary>
public class SupabaseClient(HttpClient http, IConfiguration config, IJSRuntime js)
{
    private const string StorageKey = "hallen.session";
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private Session? session;
    private bool restored;

    private string Url => (config["Supabase:Url"] ?? "").TrimEnd('/');
    private string Key => config["Supabase:Key"] ?? "";

    public bool IsConfigured => Url.Length > 0 && Key.Length > 0;
    public bool IsLoggedIn => session is not null;
    public string? Email => session?.Email;

    // ---------- Inloggning ----------

    /// <summary>Återställer en tidigare inloggning från webbläsaren (om den finns).</summary>
    public async Task RestoreAsync()
    {
        if (restored) return;
        restored = true;
        try
        {
            var raw = await js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(raw))
                session = JsonSerializer.Deserialize<Session>(raw, Json);
        }
        catch (Exception) { session = null; }

        await EnsureFreshAsync();
    }

    /// <summary>Returnerar ett felmeddelande, eller null om inloggningen lyckades.</summary>
    public async Task<string?> SignInAsync(string email, string password)
    {
        if (!IsConfigured) return "Supabase är inte konfigurerat.";
        try
        {
            var res = await SendAsync(HttpMethod.Post, "auth/v1/token?grant_type=password", new { email, password });
            if (!res.IsSuccessStatusCode)
                return res.StatusCode == System.Net.HttpStatusCode.BadRequest
                    ? "Fel e-postadress eller lösenord."
                    : $"Inloggningen misslyckades ({(int)res.StatusCode}).";

            var token = await res.Content.ReadFromJsonAsync<TokenResponse>(Json);
            if (token is null) return "Oväntat svar från servern.";
            await StoreAsync(token, token.User?.Email ?? email);
            return null;
        }
        catch (Exception)
        {
            return "Kunde inte nå servern. Kontrollera uppkopplingen.";
        }
    }

    public async Task SignOutAsync()
    {
        session = null;
        await js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }

    private async Task StoreAsync(TokenResponse token, string? email)
    {
        session = new Session(token.AccessToken, token.RefreshToken, DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn), email);
        await js.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(session, Json));
    }

    private async Task EnsureFreshAsync()
    {
        if (session is null || session.ExpiresAt > DateTimeOffset.UtcNow.AddSeconds(60)) return;
        try
        {
            var res = await SendAsync(HttpMethod.Post, "auth/v1/token?grant_type=refresh_token", new { refresh_token = session.RefreshToken });
            if (!res.IsSuccessStatusCode) { await SignOutAsync(); return; }
            var token = await res.Content.ReadFromJsonAsync<TokenResponse>(Json);
            if (token is null) { await SignOutAsync(); return; }
            await StoreAsync(token, session.Email);
        }
        catch (Exception)
        {
            await SignOutAsync();
        }
    }

    // ---------- Events ----------

    /// <summary>Alla event sorterade på starttid. Publik läsning – kräver ingen inloggning.</summary>
    public async Task<List<EventRow>> GetEventsAsync()
    {
        if (!IsConfigured) return new();
        try
        {
            await EnsureFreshAsync();
            var res = await SendAsync(HttpMethod.Get, "rest/v1/events?select=*&order=starts_at.asc", useUserToken: true);
            if (!res.IsSuccessStatusCode) return new();
            return await res.Content.ReadFromJsonAsync<List<EventRow>>(Json) ?? new();
        }
        catch (Exception)
        {
            return new();
        }
    }

    /// <summary>Skapar (utan Id) eller uppdaterar (med Id) ett event. Returnerar felmeddelande eller null.</summary>
    public async Task<string?> SaveEventAsync(EventRow row)
    {
        await EnsureFreshAsync();
        if (session is null) return "Du är utloggad. Logga in igen.";
        try
        {
            HttpResponseMessage res;
            if (row.Id is { } id)
            {
                // Ordbok så att tomma fält (null) faktiskt skrivs över och kan rensas.
                var update = new Dictionary<string, object?>
                {
                    ["type"] = row.Type,
                    ["title"] = row.Title,
                    ["starts_at"] = row.StartsAt,
                    ["league"] = row.League,
                    ["description"] = row.Description,
                    ["image_url"] = row.ImageUrl,
                    ["bookable"] = row.Bookable,
                    ["featured"] = row.Featured
                };
                res = await SendAsync(new HttpMethod("PATCH"), $"rest/v1/events?id=eq.{id}", update, true, "return=minimal");
            }
            else
            {
                res = await SendAsync(HttpMethod.Post, "rest/v1/events", row, true, "return=minimal");
            }
            return await ErrorOrNullAsync(res);
        }
        catch (Exception)
        {
            return "Kunde inte nå servern.";
        }
    }

    public async Task<string?> DeleteEventAsync(Guid id)
    {
        await EnsureFreshAsync();
        if (session is null) return "Du är utloggad. Logga in igen.";
        try
        {
            var res = await SendAsync(HttpMethod.Delete, $"rest/v1/events?id=eq.{id}", useUserToken: true);
            return await ErrorOrNullAsync(res);
        }
        catch (Exception)
        {
            return "Kunde inte nå servern.";
        }
    }

    // ---------- Bilder (Supabase Storage, bucket "event-images") ----------

    private const string Bucket = "event-images";
    private string PublicPrefix => $"{Url}/storage/v1/object/public/{Bucket}/";

    /// <summary>Laddar upp en bild. Returnerar (publik adress, null) eller (null, felmeddelande).</summary>
    public async Task<(string? Url, string? Error)> UploadImageAsync(byte[] bytes, string contentType = "image/jpeg")
    {
        await EnsureFreshAsync();
        if (session is null) return (null, "Du är utloggad. Logga in igen.");
        try
        {
            var name = $"{Guid.NewGuid():N}.jpg";
            var req = new HttpRequestMessage(HttpMethod.Post, $"{Url}/storage/v1/object/{Bucket}/{name}");
            req.Headers.Add("apikey", Key);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
            req.Content = new ByteArrayContent(bytes);
            req.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            var res = await http.SendAsync(req);
            var error = await ErrorOrNullAsync(res);
            return error is null ? (PublicPrefix + name, null) : (null, error);
        }
        catch (Exception)
        {
            return (null, "Kunde inte ladda upp bilden.");
        }
    }

    /// <summary>Tar bort en uppladdad bild (om adressen är en av våra). Fel ignoreras.</summary>
    public async Task DeleteImageAsync(string? imageUrl)
    {
        if (session is null || string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith(PublicPrefix)) return;
        try
        {
            var name = imageUrl[PublicPrefix.Length..];
            await SendAsync(HttpMethod.Delete, $"storage/v1/object/{Bucket}/{name}", useUserToken: true);
        }
        catch (Exception) { /* bästa försök */ }
    }

    // ---------- Hjälpmetoder ----------

    private static async Task<string?> ErrorOrNullAsync(HttpResponseMessage res)
    {
        if (res.IsSuccessStatusCode) return null;
        var body = (await res.Content.ReadAsStringAsync()).Trim();
        if (body.Length > 300) body = body[..300];
        if (res.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden)
            return $"Saknar behörighet ({(int)res.StatusCode}). Databasen svarade: {body}";
        return $"Något gick fel ({(int)res.StatusCode}). {body}";
    }

    /// <summary>Hämtar en lista via REST (publik läsning). Returnerar null vid fel.</summary>
    public async Task<List<T>?> SelectAsync<T>(string path, CancellationToken ct = default)
    {
        if (!IsConfigured) return null;
        try
        {
            await EnsureFreshAsync();
            var res = await SendAsync(HttpMethod.Get, path, useUserToken: true, ct: ct);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<List<T>>(Json, ct);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>Skriver (POST/PATCH/DELETE) som inloggad. Returnerar felmeddelande eller null.</summary>
    public async Task<string?> WriteAsync(HttpMethod method, string path, object? body = null, string? prefer = "return=minimal")
    {
        await EnsureFreshAsync();
        if (session is null) return "Du är utloggad. Logga in igen.";
        try
        {
            var res = await SendAsync(method, path, body, true, prefer);
            return await ErrorOrNullAsync(res);
        }
        catch (Exception)
        {
            return "Kunde inte nå servern.";
        }
    }

    private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, object? body = null,
        bool useUserToken = false, string? prefer = null, CancellationToken ct = default)
    {
        var req = new HttpRequestMessage(method, $"{Url}/{path}");
        req.Headers.Add("apikey", Key);
        if (useUserToken && session is not null)
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
        if (prefer is not null) req.Headers.Add("Prefer", prefer);
        if (body is not null) req.Content = JsonContent.Create(body, body.GetType(), options: Json);
        return await http.SendAsync(req, ct);
    }
}
