using System.Globalization;

namespace PersianDatePickerSample.Components.PersianDatePickerComponent;

public static class PersianDateHelper
{
    private static PersianCalendar pc = new();
    public static string[] WeekNames => new[] { "ش", "ی", "د", "س", "چ", "پ", "ج" }; // Fixed syntax for array initialization.

    public static int GetWeekSpan(this DayOfWeek week)
    {
        return week switch
        {
            DayOfWeek.Saturday => 0,
            DayOfWeek.Sunday => 1,
            DayOfWeek.Monday => 2,
            DayOfWeek.Tuesday => 3,
            DayOfWeek.Wednesday => 4,
            DayOfWeek.Thursday => 5,
            DayOfWeek.Friday => 6,
            _ => 0 // Default value remains the same.
        };
    }

    public static string GetMonthName(this int month) =>
        month switch
        {
            1 => "فروردین",
            2 => "اردیبهشت",
            3 => "خرداد",
            4 => "تیر",
            5 => "مرداد",
            6 => "شهریور",
            7 => "مهر",
            8 => "آبان",
            9 => "آذر",
            10 => "دی",
            11 => "بهمن",
            12 => "اسفند",
            _ => "نامشخص", // Default value for invalid month numbers.
        };

    public static (int MonthNumber, string MonthName)[] GetMonths()
    {
        return new[] // Corrected syntax for array initialization.
        {
            (1, "فروردین"),
            (2, "اردیبهشت"),
            (3, "خرداد"),
            (4, "تیر"),
            (5, "مرداد"),
            (6, "شهریور"),
            (7, "مهر"),
            (8, "آبان"),
            (9, "آذر"),
            (10, "دی"),
            (11, "بهمن"),
            (12, "اسفند")
        };
    }

    public static string ToPersianDate(this DateTime date)
    {
        var year = pc.GetYear(date);
        var month = pc.GetMonth(date);
        var day = pc.GetDayOfMonth(date);
        return $"{year}/{month:D2}/{day:D2}"; // Simplified formatting.
    }
}