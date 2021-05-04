namespace WebApplication.SharedKernel.Enums
{
    public enum Education
    {
        BeforeDiploma = 0,
        Diploma = 1,
        AdvancedDiploma = 2,
        Bachelor = 3,
        Master = 4,
        Phd = 5,
        PostDoc = 6
    }

    public enum Gender
    {
        Female = 1,
        Male = 2
    }

    public enum GenderBasedAccess
    {
        All = 0, //default
        Female = 1,
        Male = 2
    }

    public enum MaritalStatus
    {
        Single = 0,
        Married = 1
    }

    public enum MilitaryServiceStatus
    {
        Completed = 0,
        Included = 1,
        Exempt = 2
    }
}
