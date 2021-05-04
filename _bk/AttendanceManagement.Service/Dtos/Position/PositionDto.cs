namespace AttendanceManagement.Service.Dtos.Position
{
    public class PositionDto
    {
        public int Id { get; set; }
        public int WorkUnitId { get; set; }
        public string Title { get; set; }
        public string WorkUnitTitle { get; set; }
        public long RowNumber { get; set; }
    }

    public class PositionDDLDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
