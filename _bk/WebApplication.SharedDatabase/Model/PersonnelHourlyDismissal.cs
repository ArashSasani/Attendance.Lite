using System;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelHourlyDismissal : PersonnelDismissal
    {
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
