namespace HallenSoder.Client.Data;

public record ContentField(string Key, string Label, string Group, string Default, bool Multiline = false);

/// <summary>
/// Alla texter som går att redigera i admin, med standardvärden. Saknas ett värde i databasen
/// används standardvärdet, så sajten fungerar även om Supabase inte svarar.
/// </summary>
public static class ContentDefaults
{
    public static readonly ContentField[] Fields =
    [
        // ---------- Meddelande ----------
        new("banner.text", "Meddelande högst upp på sajten (tomt = inget visas)", "Meddelande", "", false),

        // ---------- Startsidan ----------
        new("hero.eyebrow", "Liten text ovanför rubriken", "Startsidan", "Sportsbar · Södermalm · Stockholm"),
        new("hero.title1", "Rubrik, rad 1", "Startsidan", "Söders"),
        new("hero.title2", "Rubrik, rad 2 (kopparfärg)", "Startsidan", "sportbar"),
        new("hero.lead", "Text under rubriken", "Startsidan",
            "Välkommen till Hallen Södermalm – den ultimata platsen för dig som älskar sport, god mat och trevlig atmosfär.", true),
        new("strip.address", "Infofält: adress", "Startsidan", "Götgatan 85A, Södermalm"),
        new("strip.sport", "Infofält: sport", "Startsidan", "All sport, live"),
        new("strip.hours", "Infofält: öppettider", "Startsidan", "Mån–tors 13–01 · Fre–sön 11–01"),
        new("strip.lunch_label", "Infofält: lunch, rubrik", "Startsidan", "Lunch fredagar 11–14"),
        new("strip.lunch", "Infofält: lunch, text", "Startsidan", "119 kr inkl. kaffe"),
        new("about.eyebrow", "Presentation: liten text", "Startsidan", "Hallen Södermalm"),
        new("about.title", "Presentation: rubrik", "Startsidan", "Din moderna sportsbar som sänder all sport"),
        new("about.lead", "Presentation: text", "Startsidan",
            "Upplev spänningen med live-matcher på storbildsskärm. Njut av din favoritsport tillsammans med vänner i en pulserande atmosfär – mitt i hjärtat av Stockholm.", true),
        new("card1.title", "Kort 1: rubrik", "Startsidan", "God & variation"),
        new("card1.text", "Kort 1: text", "Startsidan", "Från saftiga hamburgare till läckra vegetariska alternativ – något för varje smak.", true),
        new("card2.title", "Kort 2: rubrik", "Startsidan", "Fräscha drycker"),
        new("card2.text", "Kort 2: text", "Startsidan", "Klassiska ölsorter, local craft beer och läckra alkoholfria alternativ.", true),
        new("card3.title", "Kort 3: rubrik", "Startsidan", "Live-matcher"),
        new("card3.text", "Kort 3: text", "Startsidan", "Storbildsskärmar och full stämning. Se sändningsschemat och boka bord.", true),
        new("afterwork.eyebrow", "Afterwork: liten text", "Startsidan", "Afterwork"),
        new("afterwork.title", "Afterwork: rubrik", "Startsidan", "Boka er afterwork hos oss"),
        new("afterwork.lead", "Afterwork: text", "Startsidan", "Samla jobbet, laget eller kompisgänget. Mat, dryck och sport i en och samma lokal.", true),
        new("afterwork.happy", "Afterwork: Happy Hour-rad (tomt = ingen)", "Startsidan", "Happy Hour · Mån–tors 13:00–18:00 · Fredag 11:00–18:00"),

        // ---------- Öppettider ----------
        new("hours.1.label", "Rad 1: rubrik", "Öppettider", "Måndag–torsdag"),
        new("hours.1.value", "Rad 1: tid", "Öppettider", "13:00–01:00"),
        new("hours.2.label", "Rad 2: rubrik", "Öppettider", "Fredag–söndag"),
        new("hours.2.value", "Rad 2: tid", "Öppettider", "11:00–01:00"),
        new("hours.3.label", "Rad 3: rubrik", "Öppettider", "After Work / Happy Hour"),
        new("hours.3.value", "Rad 3: tid", "Öppettider", "Mån–tors 13:00–18:00\nFredag 11:00–18:00", true),
        new("hours.4.label", "Rad 4: rubrik", "Öppettider", "Lunch"),
        new("hours.4.value", "Rad 4: tid", "Öppettider", "Fredag 11:00–14:00"),

        // ---------- Meny ----------
        new("menu.title", "Rubrik", "Menysidan", "Vår meny"),
        new("menu.intro", "Text under rubriken", "Menysidan",
            "Husmanskost, burgare och barmat med hög nivå. Lunch serveras för 119 kr inklusive kaffe.", true),
        new("menu.footer", "Text under menyn", "Menysidan",
            "Har du allergier eller specialkost? Säg till din servitör. Priser i SEK. Vill du ha dryckesmenyn? Fråga i baren eller ring 08-828 265.", true),

        // ---------- Event ----------
        new("events.intro", "Text under rubriken", "Eventsidan",
            "Matcher på storbild, quizkvällar och andra event. Boka bord så du får din plats.", true),

        // ---------- Boka bord ----------
        new("booking.intro", "Text under rubriken", "Boka bord", "Säkra ditt bord inför matchen, lunchen eller afterworken.", true),
        new("booking.notice", "Viktig notis ovanför bokningen", "Boka bord", "bokning av bord gäller enbart för ätande gäster.", true),
        new("booking.side_title", "Sidorutan: rubrik", "Boka bord", "Hellre ringa?"),
        new("booking.side_text", "Sidorutan: text", "Boka bord", "Större sällskap eller afterwork? Hör av dig så fixar vi det.", true),

        // ---------- Kontakt ----------
        new("contact.intro", "Text under rubriken", "Kontakt", "Vi finns på Götgatan på Södermalm.", true),
        new("contact.address", "Adress", "Kontakt", "Götgatan 85A\n116 62 Stockholm", true),
        new("contact.phone", "Telefon", "Kontakt", "08-828 265"),
        new("contact.email", "E-post", "Kontakt", "info@hallen.one"),

        // ---------- Sidfot ----------
        new("footer.about", "Beskrivning i sidfoten", "Sidfot", "Din moderna sportsbar på Södermalm – live-sport, god mat och fräscha drycker.", true),
        new("footer.copy", "Text längst ned (tomt = ingen)", "Sidfot", ""),
    ];

    private static readonly Dictionary<string, string> Map = Fields.ToDictionary(f => f.Key, f => f.Default);

    public static string Default(string key) => Map.TryGetValue(key, out var v) ? v : "";
}
