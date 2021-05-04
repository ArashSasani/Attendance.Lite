using System;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelDailyDuty : PersonnelDuty
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
