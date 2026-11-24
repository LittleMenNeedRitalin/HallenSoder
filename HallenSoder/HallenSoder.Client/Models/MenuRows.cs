using System.Text.Json.Serialization;

namespace HallenSoder.Client.Models;

/// <summary>En rätt i Supabase-tabellen "menu_items".</summary>
public class MenuItemRow
{
    [JsonPropertyName("id")] public Guid? Id { get; set; }
    [JsonPropertyName("section_id")] public Guid SectionId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("price")] public int? Price { get; set; }
    [JsonPropertyName("sort")] public int Sort { get; set; }
    [JsonPropertyName("hidden")] public bool Hidden { get; set; }
}

/// <summary>En kategori i Supabase-tabellen "menu_sections", med sina rätter.</summary>
public class MenuSectionRow
{
    [JsonPropertyName("id")] public Guid? Id { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("note")] public string? Note { get; set; }
    [JsonPropertyName("sort")] public int Sort { get; set; }
    [JsonPropertyName("menu_items")] public List<MenuItemRow> Items { get; set; } = new();
}

/// <summary>En redigerbar text i tabellen "site_content".</summary>
public class ContentRow
{
    [JsonPropertyName("key")] public string Key { get; set; } = "";
    [JsonPropertyName("value")] public string? Value { get; set; }
}
