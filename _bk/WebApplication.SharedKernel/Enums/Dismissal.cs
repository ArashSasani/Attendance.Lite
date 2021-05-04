namespace WebApplication.SharedKernel.Enums
{
    public enum DismissalType
    {
        Demanded = 1,
        Sickness = 2,
        WithoutSalary = 3,
        Encouragement = 4,
        Marriage = 5,
        ChildBirth = 6,
        BreastFeeding = 7,
        DeathOfRelatives = 8
    }

    public enum DismissalSystemType
    {
        Default = 1,
        Customized = 2
    }

    public enum DismissalSystemTypeAccess
    {
        Default = 1,
        Customized = 2,
        All = 3
    }

    public enum DismissalExcessiveReaction
    {
        Nothing = 0,
        Alarm = 1,
        Forbid = 2
    }
}
