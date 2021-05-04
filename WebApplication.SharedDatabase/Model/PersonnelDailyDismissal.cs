using System;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelDailyDismissal : PersonnelDismissal
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
