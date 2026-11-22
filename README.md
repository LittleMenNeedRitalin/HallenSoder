# Hallen Södermalm – ny hemsida (prototyp)

Blazor WebAssembly (standalone, .NET 10) som publiceras som statiska filer på GitHub Pages.
Allt ligger i `HallenSoder/HallenSoder.Client`.

## Struktur
- `Pages/` – Start, Meny, Evenemang, BokaBord, Kontakt, NotFound
- `Layout/MainLayout.razor` – header och footer
- `Components/EventCard.razor` – event-kort
- `Data/MenuData.cs` – menyn · `Data/EventService.cs` – läser sändningsschemat
- `wwwroot/` – css, bilder, `data/events.json`, admin (`/admin`)

## Köra lokalt
Öppna `HallenSoder.slnx` i Visual Studio och tryck F5, eller:

    dotnet run --project HallenSoder/HallenSoder.Client

## Publicera på GitHub Pages
1. Pusha till ett GitHub-repo (branch `main`).
2. Settings → Pages → Source: **GitHub Actions**.
3. Workflowen `.github/workflows/deploy.yml` bygger och publicerar vid varje push.
4. Sajten hamnar på `https://<användare>.github.io/<repo>/`.
   Egen domän: lägg en `CNAME`-fil i `wwwroot/` så byter workflowen automatiskt till rotadress.

## Admin för event och sändningsschema
Gå till `/admin/` på sajten och logga in med GitHub. Där läggs matcher, quiz och andra event till
via ett formulär. Ändringen sparas som en commit i `wwwroot/data/events.json` och sajten byggs om automatiskt.

Innan det fungerar: byt `repo:` i `wwwroot/admin/config.yml` till ert repo (`användare/repo`).
Admin behöver ett GitHub-konto med skrivrätt till repot.
