namespace PersianDatePickerSample.Components.PersianDatePickerComponent
{
    public class DateCellModel
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public bool Show => Day > 0;
        public String PersianDateFormat => PersianDate();
        string PersianDate()
        {
            if (Day <= 0)
                return string.Empty;

            return Date.ToPersianDate();
        }
    }
}
