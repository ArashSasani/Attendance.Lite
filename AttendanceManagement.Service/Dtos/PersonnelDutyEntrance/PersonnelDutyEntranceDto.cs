namespace AttendanceManagement.Service.Dtos.PersonnelDutyEntrance
{
    public class PersonnelDutyEntranceDto
    {
        public int Id { get; set; }
        public string PersonnelCode { get; set; }
        public string PersonnelFullName { get; set; }
        public string StartDate { get; set; }
        public string Start { get; set; }
        public string EndDate { get; set; }
        public string End { get; set; }
        public long RowNumber { get; set; }
    }
}
