using System.Text.Json.Serialization;

namespace HallenSoder.Client.Models;

/// <summary>En rad i Supabase-tabellen "events".</summary>
public class EventRow
{
    [JsonPropertyName("id")] public Guid? Id { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = "Annat";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("starts_at")] public DateTimeOffset StartsAt { get; set; }
    [JsonPropertyName("league")] public string? League { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("image_url")] public string? ImageUrl { get; set; }
    [JsonPropertyName("bookable")] public bool Bookable { get; set; } = true;
    [JsonPropertyName("featured")] public bool Featured { get; set; }
}
