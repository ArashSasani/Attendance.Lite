using System;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelDailyDismissal : PersonnelDismissal
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
