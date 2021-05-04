using System;

namespace AttendanceManagement.Core.Model
{
    /// <summary>
    /// This dismissal is not available for request by personnel
    /// </summary>
    public class EncouragementDismissal : Dismissal
    {
        public DateTime EncouragementFromDate { get; set; }
        public DateTime EncouragementToDate { get; set; }
        public bool EncouragementConsiderWithoutSalary { get; set; }
    }
}
