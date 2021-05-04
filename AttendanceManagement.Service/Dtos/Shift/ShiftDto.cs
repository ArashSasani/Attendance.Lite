namespace AttendanceManagement.Service.Dtos.Shift
{
    public class ShiftDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long RowNumber { get; set; }
    }

    public class ShiftDDLDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
