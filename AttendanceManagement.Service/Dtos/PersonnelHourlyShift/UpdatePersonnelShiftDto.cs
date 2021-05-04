using System.Collections.Generic;

namespace AttendanceManagement.Service.Dtos.PersonnelHourlyShift
{
    public class UpdatePersonnelHourlyShiftDto
    {
        public int Id { get; set; }
        public List<int> PersonnelIdList { get; set; } = new List<int>();
        public int HourlyShiftId { get; set; }
    }
}
