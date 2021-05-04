using System;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelDailyDuty : PersonnelDuty
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
