namespace WorkWeekPlanner.Api.Infrastructure;

public static class IsoWeekUtils
{
    // Returns (Year, WeekNumber) for a given date as per ISO8601
    public static (int year, int week) GetIso8601WeekOfYear(DateTime time)
    {
        var week = System.Globalization.ISOWeek.GetWeekOfYear(time);
        var year = System.Globalization.ISOWeek.GetYear(time);
        return (year, week);
    }

    // Returns the first Monday of the week/year
    public static DateTime FirstDateOfWeekIso8601(int year, int weekOfYear)
    {
        return System.Globalization.ISOWeek.ToDateTime(year, weekOfYear, DayOfWeek.Monday);
    }
}