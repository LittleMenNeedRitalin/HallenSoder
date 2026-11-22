using System.Net.Http.Json;
using System.Text.Json;
using HallenSoder.Client.Models;

namespace HallenSoder.Client.Data;

public class EventService(HttpClient http)
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    /// <summary>Kommande event sorterade på datum. Pågående event visas i 3 timmar efter start.</summary>
    public async Task<List<SiteEvent>> GetUpcomingAsync()
    {
        try
        {
            // Cache-bust så att nya event syns direkt efter att admin sparat.
            var file = await http.GetFromJsonAsync<EventFile>($"data/events.json?v={DateTime.UtcNow.Ticks / TimeSpan.TicksPerMinute}", Options);
            var cutoff = DateTime.Now.AddHours(-3);
            return (file?.Events ?? new())
                .Where(e => e.Date >= cutoff)
                .OrderBy(e => e.Date)
                .ToList();
        }
        catch (Exception)
        {
            return new();
        }
    }
}
