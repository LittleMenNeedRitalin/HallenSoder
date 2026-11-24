using HallenSoder.Client.Models;

namespace HallenSoder.Client.Data;

/// <summary>
/// Sajtens redigerbara texter. Värden från Supabase (tabellen site_content) går före
/// standardvärdena i <see cref="ContentDefaults"/>.
/// </summary>
public class SiteContent(SupabaseClient supabase)
{
    private Dictionary<string, string> overrides = new();

    public async Task LoadAsync(CancellationToken ct = default)
    {
        var rows = await supabase.SelectAsync<ContentRow>("rest/v1/site_content?select=key,value", ct);
        if (rows is not null)
            overrides = rows.ToDictionary(r => r.Key, r => r.Value ?? "");
    }

    public string this[string key] =>
        overrides.TryGetValue(key, out var value) ? value : ContentDefaults.Default(key);

    public bool IsOverridden(string key) => overrides.ContainsKey(key);

    /// <summary>Sparar en text. Är den lika med standardtexten tas den egna versionen bort.</summary>
    public async Task<string?> SaveAsync(string key, string value)
    {
        if (value == ContentDefaults.Default(key)) return await ResetAsync(key);

        var error = await supabase.WriteAsync(HttpMethod.Post, "rest/v1/site_content?on_conflict=key",
            new Dictionary<string, object?>
            {
                ["key"] = key,
                ["value"] = value,
                ["updated_at"] = DateTimeOffset.UtcNow
            },
            "resolution=merge-duplicates,return=minimal");
        if (error is null) overrides[key] = value;
        return error;
    }

    public async Task<string?> ResetAsync(string key)
    {
        if (!overrides.ContainsKey(key)) return null;
        var error = await supabase.WriteAsync(HttpMethod.Delete, $"rest/v1/site_content?key=eq.{Uri.EscapeDataString(key)}");
        if (error is null) overrides.Remove(key);
        return error;
    }

    /// <summary>Endast siffror, för tel:-länkar.</summary>
    public static string Digits(string value) => new(value.Where(char.IsDigit).ToArray());
}
