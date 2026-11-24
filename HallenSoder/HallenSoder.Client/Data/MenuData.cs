namespace HallenSoder.Client.Data;

public record Dish(string Name, string Description, int? Price = null);

public record MenuSection(string Id, string Title, string? Note, Dish[] Dishes);

/// <summary>Menyn, hämtad från hallen.one/meny.</summary>
public static class MenuData
{
    public static readonly MenuSection[] Sections =
    [
        new("lunch", "Lunch", "119 kr inkl. kaffe · fredagar 11–14",
        [
            new("Smashburger ost", "160g högrev – cheddarost, tomat, picklad rödlök, sallad, pommes & dipp"),
            new("Kycklingburgare", "Dubbelpanerad kycklingfilé – chili- & vitlöksmajo, tomat, rödlök, sallad, Grana Padano, pommes & dipp"),
            new("Pytt i panna", "Serveras med stekt ägg, rödbetor & saltgurka"),
            new("Raggmunk", "Serveras med stekt fläsk & råröda lingon"),
            new("Halloumiburgare", "Grillad halloumi – sirachamajo, tomat, sallad, rödlök, pommes & dipp"),
            new("Pasta pollo", "Krämig kyckling-pennepasta – champinjoner, lök, paprika, chili, vitlök, toppas med Grana Padano"),
        ]),
        new("smaratter", "Smårätter", null,
        [
            new("Nachos", "Paprika, rödlök, jalapeño, Grana Padano & chilimajo", 99),
            new("Toast Skagen", "Färska handskalade räkor, rödlök, citron, pepparrot & dill", 139),
            new("Kronärtskocka", "Friterad kronärtskocka med citronmajo & picklad rödlök", 139),
            new("Meaty fries", "Nötfärs, rödlök, paprika, jalapeño & chilimajo", 159),
            new("Meaty nachos deluxe", "Nötfärs, paprika, rödlök, jalapeño, Grana Padano & chilimajo", 169),
            new("Sportplanka", "4 st chili cheese, 4 st onion rings, 4 st mozzarella sticks & 4 st buffalo wings med dipp", 169),
            new("Hallens bricka", "2 olika ostar & 3 olika salami med oliver & grissini", 189),
            new("Pommes", "Serveras med dipp", 79),
            new("Sötpommes", "Serveras med flingsalt & dipp", 89),
            new("Vitlöksbröd", "Stekt vitlöksbröd med marinerade oliver", 89),
            new("Mozzarella sticks", "9 st, serveras med dipp", 89),
            new("Onion rings", "9 st, serveras med dipp", 89),
            new("Chili cheese", "9 st, serveras med dipp", 89),
            new("Buffalo wings", "9 st med paprika, rödlök, jalapeños, BBQ-sås & dipp", 99),
        ]),
        new("varmratter", "Varmrätter", null,
        [
            new("Fläskkarré", "Marinerad fläskkarré – rödvinssås, bearnaise & pommes", 179),
            new("Club sandwich", "Grillad kyckling med bacon; sirachamajo, tomat, picklad rödlök, sallad, pommes & dipp", 189),
            new("Smashburger ost", "160g högrev; dubbel cheddar, tomat, picklad rödlök, sallad, pommes & dipp (bacon +10 kr)", 179),
            new("Svamppasta", "Krämig pasta med champinjoner, kantareller, portobello, silverlök, vitlök, paprika & vitvin", 179),
            new("Biffpasta", "Krämig biffpasta; champinjoner, lök, paprika, vitlök & konjak", 199),
            new("Souvlakispett", "Kyckling – serveras med pommes, tzatziki & grekisk sallad", 179),
            new("Rödspätta", "Panerad rödspätta med pommes, remouladsås & citron", 179),
            new("Kycklingburgare", "Dubbelpanerad kycklingfilé; chili, vitlöksmajo, tomat, rödlök, sallad, Grana Padano, pommes & dipp", 189),
            new("Pytt i panna", "Serveras med stekt ägg, rödbetor & saltgurka", 179),
            new("Halloumiburgare", "Grillad halloumi; sirachamajo, tomat, sallad, rödlök, pommes & dipp", 179),
            new("Pasta pollo", "Krämig kyckling-pennepasta; champinjoner, lök, paprika, chili, vitlök & Grana Padano", 179),
            new("Black Angus ryggbiff", "Rosmarin- & vitlöksmarinerad. Serveras med stekpotatis/pommes/sidsallad, rödvinssås & bearnaise", 219),
            new("Söder sallad", "Grillad kyckling & halloumi. Grönsaker med Grana Padano, krutonger & Hallens dressing", 199),
            new("Schnitzel", "Panerat fläsk. Serveras med stekpotatis, rödvinssås, bearnaise, citron & kapris", 189),
        ]),
        new("efterratter", "Efterrätter", null,
        [
            new("Chokladfondant", "Serveras med vaniljglass", 99),
            new("Vaniljglass", "3 kulor, serveras med choklad- eller colasås", 79),
        ]),
        new("dessertdrinkar", "Dessertdrinkar", null,
        [
            new("Espresso martini", "Vodka, Kahlúa, kaffe", 149),
            new("Kaffe Karlsson", "Baileys, apelsinlikör, kaffe, grädde", 149),
            new("Irish coffee", "Whisky, kaffe, farinsocker, grädde", 149),
            new("Rosa pantern", "Vodka, Likör 43, mjölk, grenadin", 149),
        ]),
    ];

    /// <summary>Standardmenyn som databasrader (används som reserv och vid första importen).</summary>
    public static List<HallenSoder.Client.Models.MenuSectionRow> ToRows() =>
        Sections.Select((s, si) =>
        {
            var sectionId = Guid.NewGuid();
            return new HallenSoder.Client.Models.MenuSectionRow
            {
                Id = sectionId,
                Slug = s.Id,
                Title = s.Title,
                Note = s.Note,
                Sort = (si + 1) * 10,
                Items = s.Dishes.Select((d, di) => new HallenSoder.Client.Models.MenuItemRow
                {
                    Id = Guid.NewGuid(),
                    SectionId = sectionId,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    Sort = (di + 1) * 10
                }).ToList()
            };
        }).ToList();
}
