namespace AttendanceManagement.Core.Model
{
    public class BreastFeedingDismissal : Dismissal
    {
        public int BreastFeedingAllowanceInTotal { get; set; }
        public int BreastFeedingAllowanceInDay { get; set; }
        public int BreastFeedingCountInDay { get; set; }
        public bool BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }
}
