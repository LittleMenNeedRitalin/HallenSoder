namespace HallenSoder.Client.Models;

/// <summary>Ett event i sändningsschemat. Datum är svensk tid (Europe/Stockholm).</summary>
public class SiteEvent
{
    public Guid? Id { get; set; }
    public string Type { get; set; } = "Annat";
    public string Title { get; set; } = "";
    public DateTime Date { get; set; }
    public string? League { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool Bookable { get; set; } = true;
    public bool Featured { get; set; }

    public string Icon => Type switch
    {
        "Match" => "⚽",
        "Quiz" => "🧠",
        "Live-musik" => "🎸",
        "Afterwork" => "🍻",
        _ => "⭐"
    };
}
