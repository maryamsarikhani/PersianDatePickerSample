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

    public static class HolidayHelper
    {
        // 📅 تعطیلات شمسی - ثابت
        private static readonly HashSet<string> Holidays = new()
        {
        // 📅 تعطیلات شمسی (ثابت)
        "01/01",  // نوروز (1 فروردین)
        "01/02",  // نوروز (2 فروردین)
        "01/03",  // نوروز (3 فروردین)
        "01/04",  // نوروز (4 فروردین)
        "01/12",  // روز جمهوری اسلامی (12 فروردین)
        "01/13",  // روز طبیعت (13 فروردین)
        "03/14",  // رحلت امام خمینی (14 خرداد)
        "03/15",  // قیام 15 خرداد (15 خرداد)
        "11/22",  // پیروزی انقلاب اسلامی (22 بهمن)
        "12/29",   // ملی شدن صنعت نفت (29 اسفند)
        };

        // 🕌 تعطیلات مذهبی - قمری
        private static readonly HashSet<string> HijriHolidays = new()
        {
        "01/11",
        "02/04",
        "03/24",
        "04/14",
        "04/15",
        "05/23",
        "06/02",
        "06/10",
        "06/19",
        "09/03",
        "10/13",
        "10/27",
        "11/15",
        "12/20",
        };

        public static int GetPersianDayOfWeek(DateTime date)
        {
            var pc = new PersianCalendar();
            return (int)pc.GetDayOfWeek(date); // 5 == جمعه
        }

        public static List<DateTime> GetAllHolidays(int persianYear)
        {
            var holidays = new List<DateTime>();
            var persian = new PersianCalendar();
            var hijri = new HijriCalendar();

            if (persianYear == persian.GetYear(DateTime.Now))
            {
                // 📌 تعطیلات قمری
                foreach (var entry in HijriHolidays)
                {
                    var parts = entry.Split('/');
                    int month = int.Parse(parts[0]);
                    int day = int.Parse(parts[1]);

                    try
                    {
                        DateTime holiday = persian.ToDateTime(persianYear, month, day, 0, 0, 0, 0);
                        holidays.Add(holiday);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // در صورتی که تاریخ معتبر نباشد (مثلاً برای سال‌های کبیسه)
                    }
                }
            }

            // 📌 تعطیلات شمسی
            foreach (var entry in Holidays)
            {
                var parts = entry.Split('/');
                int month = int.Parse(parts[0]);
                int day = int.Parse(parts[1]);

                try
                {
                    DateTime holiday = persian.ToDateTime(persianYear, month, day, 0, 0, 0, 0);
                    holidays.Add(holiday);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // در صورتی که تاریخ معتبر نباشد (مثلاً برای سال های کبیسه)
                }
            }

            return holidays.Distinct().OrderBy(d => d).ToList();
        }

        public static bool IsHoliday(DateTime date, int persianYear)
        {
            var holidays = GetAllHolidays(persianYear);
            return holidays.Any(d => d.Date == date.Date);
        }


        //public static class HolidayHelper
        //{
        //    // 📅 تعطیلات شمسی (ثابت)
        //    private static readonly HashSet<string> FixedSolarHolidays = new()
        //{
        //    "01/01", "01/02", "01/03", "01/04", "01/12", "01/13",
        //    "03/14", "03/15", "11/22", "12/29"
        //};

        //    // 🕌 تعطیلات مذهبی (قمری)
        //    private static readonly List<(int Month, int Day)> HijriHolidays = new()
        //{
        //    (1, 9), (1, 10), (2, 20), (2, 28), (2, 30),
        //    (3, 8), (3, 17), (5, 3), (7, 13), (7, 27),
        //    (8, 15), (9, 21), (10, 1), (10, 2), (10, 25),
        //    (12, 10), (12, 18)
        //};

        //    // کش برای ذخیره نتایج
        //    private static readonly ConcurrentDictionary<int, List<DateTime>> _holidaysCache = new();

        //    public static List<DateTime> GetAllHolidays(int persianYear)
        //    {
        //        // اگر نتیجه در کش وجود دارد، از کش برگردانید
        //        if (_holidaysCache.TryGetValue(persianYear, out var cachedResult))
        //            return cachedResult;

        //        var holidays = new List<DateTime>();
        //        var persian = new PersianCalendar();
        //        var hijri = new HijriCalendar();

        //        // 1. اضافه کردن تعطیلات ثابت شمسی
        //        foreach (var entry in FixedSolarHolidays)
        //        {
        //            var parts = entry.Split('/');
        //            int month = int.Parse(parts[0]);
        //            int day = int.Parse(parts[1]);

        //            try
        //            {
        //                var date = persian.ToDateTime(persianYear, month, day, 0, 0, 0, 0);
        //                holidays.Add(date.Date);
        //            }
        //            catch { /* ignore invalid dates */ }
        //        }

        //        // 2. اضافه کردن تعطیلات مذهبی فقط برای سال جاری
        //        if (persianYear == persian.GetYear(DateTime.Now))
        //        {
        //            // 2. اضافه کردن تعطیلات مذهبی برای سال مورد نظر
        //            foreach (var (hMonth, hDay) in HijriHolidays)
        //            {
        //                try
        //                {
        //                    // محاسبه محدوده سال قمری ممکن برای سال شمسی مورد نظر
        //                    int minHijriYear = hijri.GetYear(persian.ToDateTime(persianYear, 1, 1, 0, 0, 0, 0)) - 1;
        //                    int maxHijriYear = hijri.GetYear(persian.ToDateTime(persianYear, 12, 29, 0, 0, 0, 0)) + 1;

        //                    for (int hYear = minHijriYear; hYear <= maxHijriYear; hYear++)
        //                    {
        //                        hijri.HijriAdjustment = hMonth switch
        //                        {
        //                            // ماه‌های 29 روزه: -1
        //                            3 or 5 or 7 or 9 or 11 or 10 => -1,  // ربیع الاول, جمادی الاول, رجب, رمضان, ذی القعده

        //                            // ماه‌های 30 روزه: -2
        //                            1 or 2 or 4 or 6 or 8  or 12 => -2,  // محرم, صفر, ربیع الثانی, جمادی الثانی, شعبان, شوال, ذی الحجه

        //                            1 or 2 or 4 or 6 or 8 or 12 => -2,
        //                            _ => 0  // حالت پیش‌فرض (نباید اتفاق بیفتد)
        //                        };

        //                        var gDate = hijri.ToDateTime(hYear, hMonth, hDay, 0, 0, 0, 0);

        //                        // فقط اگر تاریخ در سال شمسی مورد نظر باشد اضافه شود
        //                        if (persian.GetYear(gDate) == persianYear)
        //                        {
        //                            holidays.Add(gDate.Date);
        //                        }
        //                    }
        //                }
        //                catch { /* ignore conversion errors */ }
        //            }
        //        }

        //        // 3. اضافه کردن جمعه‌ها برای سال مورد نظر
        //        var startDate = persian.ToDateTime(persianYear, 1, 1, 0, 0, 0, 0);
        //        var endDate = persian.ToDateTime(persianYear, 12, 29, 0, 0, 0, 0);

        //        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        //        {
        //            if (date.DayOfWeek == DayOfWeek.Friday)
        //                holidays.Add(date.Date);
        //        }

        //        // حذف تاریخ‌های تکراری و مرتب‌سازی
        //        var result = holidays.Distinct().OrderBy(d => d).ToList();

        //        // ذخیره در کش
        //        _holidaysCache.TryAdd(persianYear, result);

        //        return result;
        //    }

        //    public static bool IsHoliday(DateTime date, int persianYear)
        //    {
        //        var holidays = GetAllHolidays(persianYear);
        //        return holidays.Any(d => d.Date == date.Date);
        //    }
        //}
    }
}