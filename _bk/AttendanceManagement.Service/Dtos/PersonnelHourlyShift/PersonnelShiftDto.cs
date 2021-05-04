namespace AttendanceManagement.Service.Dtos.PersonnelHourlyShift
{
    public class PersonnelHourlyShiftDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelFullName { get; set; }
        public int HourlyShiftId { get; set; }
        public string HourlyShiftTitle { get; set; }
        public string DateAssigned { get; set; }
    }
}
