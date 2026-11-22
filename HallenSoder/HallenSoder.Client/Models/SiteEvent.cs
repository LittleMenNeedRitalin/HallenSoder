namespace HallenSoder.Client.Models;

/// <summary>Ett event i sändningsschemat. Läses från wwwroot/data/events.json (redigeras via /admin).</summary>
public class SiteEvent
{
    public string Type { get; set; } = "Annat";
    public string Title { get; set; } = "";
    public DateTime Date { get; set; }
    public string? League { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
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

public class EventFile
{
    public List<SiteEvent> Events { get; set; } = new();
}
