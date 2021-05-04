namespace WebApplication.SharedKernel.Enums
{
    public enum RequestType
    {
        Dismissal = 1,
        Duty = 2,
        ShiftReplacement = 3
    }

    public enum RequestAction
    {
        Unknown = 0,
        Accept = 1,
        PartialAccept = 2,
        Reject = 3
    }

    public enum RequestDuration
    {
        Daily = 1,
        Hourly = 2
    }
}
