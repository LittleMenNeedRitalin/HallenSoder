using HallenSoder.Client.Models;

namespace HallenSoder.Client.Data;

public class EventService(SupabaseClient supabase)
{
    /// <summary>Alla event (även gamla), sorterade på starttid. Används av admin.</summary>
    public async Task<List<SiteEvent>> GetAllAsync() =>
        (await supabase.GetEventsAsync()).Select(ToSite).ToList();

    /// <summary>Kommande event. Pågående event visas i 3 timmar efter start.</summary>
    public async Task<List<SiteEvent>> GetUpcomingAsync()
    {
        var cutoff = DateTimeOffset.UtcNow.AddHours(-3);
        return (await supabase.GetEventsAsync())
            .Where(r => r.StartsAt >= cutoff)
            .OrderBy(r => r.StartsAt)
            .Select(ToSite)
            .ToList();
    }

    public Task<string?> SaveAsync(SiteEvent e) => supabase.SaveEventAsync(new EventRow
    {
        Id = e.Id,
        Type = e.Type,
        Title = e.Title.Trim(),
        StartsAt = SwedishTime.ToOffset(e.Date),
        League = string.IsNullOrWhiteSpace(e.League) ? null : e.League.Trim(),
        Description = string.IsNullOrWhiteSpace(e.Description) ? null : e.Description.Trim(),
        ImageUrl = string.IsNullOrWhiteSpace(e.ImageUrl) ? null : e.ImageUrl,
        Bookable = e.Bookable,
        Featured = e.Featured
    });

    /// <summary>Tar bort eventet och dess bild.</summary>
    public async Task<string?> DeleteAsync(SiteEvent e)
    {
        if (e.Id is null) return null;
        var error = await supabase.DeleteEventAsync(e.Id.Value);
        if (error is null) await supabase.DeleteImageAsync(e.ImageUrl);
        return error;
    }

    public Task<(string? Url, string? Error)> UploadImageAsync(byte[] bytes) => supabase.UploadImageAsync(bytes);

    private static SiteEvent ToSite(EventRow r) => new()
    {
        Id = r.Id,
        Type = r.Type,
        Title = r.Title,
        Date = SwedishTime.ToLocal(r.StartsAt),
        League = r.League,
        Description = r.Description,
        ImageUrl = r.ImageUrl,
        Bookable = r.Bookable,
        Featured = r.Featured
    };
}
