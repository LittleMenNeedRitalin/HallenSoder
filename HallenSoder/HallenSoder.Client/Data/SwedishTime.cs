namespace HallenSoder.Client.Data;

/// <summary>Konverterar mellan lagrad tid (med tidszon) och svensk lokal tid.</summary>
public static class SwedishTime
{
    private static readonly TimeZoneInfo Zone = Find();

    private static TimeZoneInfo Find()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm"); }
        catch (Exception) { return TimeZoneInfo.Local; }
    }

    public static DateTime ToLocal(DateTimeOffset value) => TimeZoneInfo.ConvertTime(value, Zone).DateTime;

    public static DateTimeOffset ToOffset(DateTime local)
    {
        var unspecified = DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
        return new DateTimeOffset(unspecified, Zone.GetUtcOffset(unspecified));
    }

    public static DateTime Now => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, Zone).DateTime;
}
