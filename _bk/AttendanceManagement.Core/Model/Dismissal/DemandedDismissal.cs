namespace AttendanceManagement.Core.Model
{
    public class DemandedDismissal : Dismissal
    {
        /// <summary>
        /// calculates in seconds
        /// </summary>
        public int DemandedAllowanceInMonth { get; set; }
        public int DemandedCountInMonth { get; set; }
        public bool DemandedIsTransferableToNextMonth { get; set; }
        public int DemandedTransferableAllowanceToNextMonth { get; set; }
        public int DemandedAllowanceInYear { get; set; }
        public int DemandedCountInYear { get; set; }
        public bool DemandedIsTransferableToNextYear { get; set; }
        public int DemandedTransferableAllowanceToNextYear { get; set; }
        public bool DemandedIsAllowedToSave { get; set; }
        public int DemandedAllowanceToSave { get; set; }
        public bool DemandedMealTimeIsIncluded { get; set; }
        public bool DemandedDoesDismissalMeansExtraWork { get; set; }
        public bool DemandedIsNationalHolidysConsideredInDismissal { get; set; }
        public bool DemandedIsFridaysConsideredInDismissal { get; set; }
        public int DemandedAmountOfHoursConsideredDailyDismissal { get; set; }
    }
}
