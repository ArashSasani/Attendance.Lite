namespace AttendanceManagement.Core.Model
{
    public class MarriageDismissal : Dismissal
    {
        public int MarriageAllowanceInTotal { get; set; }
        public int MarriageCountInTotal { get; set; }
        public bool MarriageConsiderWithoutSalary { get; set; }
        public bool MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }
}
