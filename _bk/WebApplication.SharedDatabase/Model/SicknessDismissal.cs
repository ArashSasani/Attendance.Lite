namespace WebApplication.SharedDatabase.Model
{
    public class SicknessDismissal : Dismissal
    {
        public int SicknessAllowanceInYear { get; set; }
        public int SicknessCountInYear { get; set; }
        public bool SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }
}
