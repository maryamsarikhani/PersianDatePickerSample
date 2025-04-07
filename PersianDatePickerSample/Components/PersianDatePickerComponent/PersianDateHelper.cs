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

    public static int GetPersianDayOfWeek(DateTime date)
    {
        var pc = new PersianCalendar();
        return (int)pc.GetDayOfWeek(date); // 5 == جمعه
    }


    //public static class HolidayHelper
    //{
    //    // 📅 تعطیلات شمسی (ثابت)
    //    private static readonly HashSet<string> FixedSolarHolidays = new()
    //    {
    //    "01/01",  // نوروز (1 فروردین)
    //    "01/02",  // نوروز (2 فروردین)
    //    "01/03",  // نوروز (3 فروردین)
    //    "01/04",  // نوروز (4 فروردین)
    //    "01/12",  // روز جمهوری اسلامی (12 فروردین)
    //    "01/13",  // روز طبیعت (13 فروردین)
    //    "03/14",  // رحلت امام خمینی (14 خرداد)
    //    "03/15",  // قیام 15 خرداد (15 خرداد)
    //    "11/22",  // پیروزی انقلاب اسلامی (22 بهمن)
    //    "12/29"   // ملی شدن صنعت نفت (29 اسفند)
    //    };

    //    // 🕌 تعطیلات مذهبی (قمری)
    //    private static readonly List<(int Month, int Day)> HijriHolidays = new()
    //    {
    //        (1, 9),    // تاسوعا (9 محرم)
    //        (1, 10),   // عاشورا (10 محرم)
    //        (2, 20),   // اربعین (20 صفر)
    //        (2, 28),   // رحلت پیامبر (ص) و امام حسن مجتبی (ع) (28 صفر)
    //        (2, 29),   // شهادت امام رضا (ع) (29 یا 30 صفر)
    //        (2, 30),   // شهادت امام رضا (ع) (30 صفر)
    //        (3, 8),    // شهادت امام حسن عسگری (ع) (8 ربیع الاول)
    //        (3, 17),   // ولادت پیامبر و امام جعفر صادق (ع) (17 ربیع الاول)
    //        (5, 3),    // شهادت حضرت فاطمه الزهرا (ص) (3 جمادی الثانی)
    //        (7, 13),   // روز پدر و تولد امام علی (ع) (13 رجب)
    //        (7, 27),   // مبعث (27 رجب)
    //        (8, 15),   // ولادت حضرت مهدی (عج) (15 شعبان)
    //        (9, 21),   // شهادت امام علی (ع) (21 رمضان)
    //        (10, 1),   // عید فطر (اول شوال)
    //        (10, 2),   // تعطیلی به مناسبت عید فطر (دوم شوال)
    //        (10, 25),  // شهادت امام جعفر صادق (ع) (25 شوال)
    //        (12, 10),  // عید قربان (10 ذیحجه)
    //        (12, 18)   // عید غدیر (18 ذیحجه)
    //    };


    //public static List<DateTime> GetAllHolidays(int persianYear)
    //{
    //    var holidays = new List<DateTime>();
    //    var persian = new PersianCalendar();
    //    var hijri = new HijriCalendar();

    //    // 📌 تعطیلات شمسی
    //    foreach (var entry in FixedSolarHolidays)
    //    {
    //        var parts = entry.Split('/');
    //        int month = int.Parse(parts[0]);
    //        int day = int.Parse(parts[1]);

    //        try
    //        {
    //            DateTime holiday = persian.ToDateTime(persianYear, month, day, 0, 0, 0, 0);
    //            holidays.Add(holiday);
    //        }
    //        catch (ArgumentOutOfRangeException)
    //        {
    //            // در صورتی که تاریخ معتبر نباشد (مثلاً برای سال‌های کبیسه)
    //        }
    //    }

    //    // 🕌 تعطیلات قمری
    //    foreach (var (hMonth, hDay) in HijriHolidays)
    //    {
    //        // پوشش سه سال قمری برای تطابق با تاریخ شمسی
    //        for (int i = 0; i <= 1; i++)
    //        {
    //            int approxHijriYear = hijri.GetYear(DateTime.Now) + i;

    //            try
    //            {
    //                var gDate = hijri.ToDateTime(approxHijriYear, hMonth, hDay, 0, 0, 0, 0);
    //                int persianYearOfHoliday = persian.GetYear(gDate);

    //                // فقط تاریخ‌هایی که در سال شمسی مورد نظر قرار دارند را اضافه کنیم
    //                if (persianYearOfHoliday == persianYear)
    //                    holidays.Add(gDate.Date);
    //            }
    //            catch
    //            {
    //                // در صورتی که تبدیل تاریخ به اشتباه باشد
    //            }
    //        }
    //    }

    //    return holidays.Distinct().OrderBy(d => d).ToList();
    //}

    //public static bool IsHoliday(DateTime date, int persianYear)
    //{
    //    var holidays = GetAllHolidays(persianYear);
    //    return holidays.Any(d => d.Date == date.Date);
    //}

}
