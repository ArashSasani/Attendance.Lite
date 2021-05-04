namespace AttendanceManagement.Core.Model
{
    public class WithoutSalaryDismissal : Dismissal
    {
        public int WithoutSalaryAllowanceInMonth { get; set; }
        public int WithoutSalaryCountInMonth { get; set; }
    }
}
