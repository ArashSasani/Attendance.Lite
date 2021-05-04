namespace WebApplication.SharedDatabase.Model
{
    public class ChildBirthDismissal : Dismissal
    {
        public int ChildBirthAllowanceInTotal { get; set; }
        public bool ChildBirthConsiderWithoutSalary { get; set; }
        public bool ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }
}
