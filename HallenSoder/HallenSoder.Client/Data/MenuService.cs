using HallenSoder.Client.Models;

namespace HallenSoder.Client.Data;

/// <summary>Läser och redigerar menyn i Supabase (menu_sections och menu_items).</summary>
public class MenuService(SupabaseClient supabase)
{
    /// <summary>Menyn sorterad. Null vid fel. Dolda rätter tas bort om includeHidden är false.</summary>
    public async Task<List<MenuSectionRow>?> GetAsync(bool includeHidden)
    {
        var rows = await supabase.SelectAsync<MenuSectionRow>(
            "rest/v1/menu_sections?select=*,menu_items(*)&order=sort.asc");
        if (rows is null) return null;

        foreach (var s in rows)
        {
            s.Items = s.Items.OrderBy(i => i.Sort).ToList();
            if (!includeHidden) s.Items = s.Items.Where(i => !i.Hidden).ToList();
        }
        return includeHidden ? rows : rows.Where(s => s.Items.Count > 0).ToList();
    }

    public static string Slugify(string text)
    {
        var t = text.Trim().ToLowerInvariant()
            .Replace('å', 'a').Replace('ä', 'a').Replace('ö', 'o').Replace('é', 'e');
        var chars = t.Select(c => char.IsLetterOrDigit(c) && c < 128 ? c : '-').ToArray();
        var slug = string.Join('-', new string(chars).Split('-', StringSplitOptions.RemoveEmptyEntries));
        return slug.Length == 0 ? "kategori" : slug;
    }

    // ---------- Kategorier ----------

    public Task<string?> SaveSectionAsync(MenuSectionRow s)
    {
        var body = new Dictionary<string, object?>
        {
            ["slug"] = string.IsNullOrWhiteSpace(s.Slug) ? Slugify(s.Title) : s.Slug,
            ["title"] = s.Title.Trim(),
            ["note"] = string.IsNullOrWhiteSpace(s.Note) ? null : s.Note.Trim(),
            ["sort"] = s.Sort
        };
        return s.Id is { } id
            ? supabase.WriteAsync(new HttpMethod("PATCH"), $"rest/v1/menu_sections?id=eq.{id}", body)
            : supabase.WriteAsync(HttpMethod.Post, "rest/v1/menu_sections", body);
    }

    public Task<string?> DeleteSectionAsync(Guid id) =>
        supabase.WriteAsync(HttpMethod.Delete, $"rest/v1/menu_sections?id=eq.{id}");

    public Task<string?> SetSectionSortAsync(Guid id, int sort) =>
        supabase.WriteAsync(new HttpMethod("PATCH"), $"rest/v1/menu_sections?id=eq.{id}",
            new Dictionary<string, object?> { ["sort"] = sort });

    // ---------- Rätter ----------

    public Task<string?> SaveItemAsync(MenuItemRow i)
    {
        var body = new Dictionary<string, object?>
        {
            ["section_id"] = i.SectionId,
            ["name"] = i.Name.Trim(),
            ["description"] = string.IsNullOrWhiteSpace(i.Description) ? null : i.Description.Trim(),
            ["price"] = i.Price,
            ["sort"] = i.Sort,
            ["hidden"] = i.Hidden
        };
        return i.Id is { } id
            ? supabase.WriteAsync(new HttpMethod("PATCH"), $"rest/v1/menu_items?id=eq.{id}", body)
            : supabase.WriteAsync(HttpMethod.Post, "rest/v1/menu_items", body);
    }

    public Task<string?> DeleteItemAsync(Guid id) =>
        supabase.WriteAsync(HttpMethod.Delete, $"rest/v1/menu_items?id=eq.{id}");

    public Task<string?> SetItemSortAsync(Guid id, int sort) =>
        supabase.WriteAsync(new HttpMethod("PATCH"), $"rest/v1/menu_items?id=eq.{id}",
            new Dictionary<string, object?> { ["sort"] = sort });

    // ---------- Import ----------

    /// <summary>Lägger in standardmenyn i databasen (används när menyn är tom).</summary>
    public async Task<string?> ImportDefaultsAsync()
    {
        var rows = MenuData.ToRows();

        var sections = rows.Select(s => new Dictionary<string, object?>
        {
            ["id"] = s.Id, ["slug"] = s.Slug, ["title"] = s.Title, ["note"] = s.Note, ["sort"] = s.Sort
        }).ToList();
        var error = await supabase.WriteAsync(HttpMethod.Post, "rest/v1/menu_sections", sections);
        if (error is not null) return error;

        var items = rows.SelectMany(s => s.Items).Select(i => new Dictionary<string, object?>
        {
            ["id"] = i.Id, ["section_id"] = i.SectionId, ["name"] = i.Name, ["description"] = i.Description,
            ["price"] = i.Price, ["sort"] = i.Sort, ["hidden"] = false
        }).ToList();
        return await supabase.WriteAsync(HttpMethod.Post, "rest/v1/menu_items", items);
    }
}
