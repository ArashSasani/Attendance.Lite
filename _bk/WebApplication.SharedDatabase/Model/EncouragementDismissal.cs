using System;

namespace WebApplication.SharedDatabase.Model
{
    public class EncouragementDismissal : Dismissal
    {
        public DateTime EncouragementFromDate { get; set; }
        public DateTime EncouragementToDate { get; set; }
        public bool EncouragementConsiderWithoutSalary { get; set; }
    }
}
