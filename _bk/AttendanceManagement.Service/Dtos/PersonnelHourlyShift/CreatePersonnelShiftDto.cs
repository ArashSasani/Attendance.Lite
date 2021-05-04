using System.Collections.Generic;

namespace AttendanceManagement.Service.Dtos.PersonnelHourlyShift
{
    public class CreatePersonnelHourlyShiftDto
    {
        public List<int> PersonnelIdList { get; set; } = new List<int>();
        public int HourlyShiftId { get; set; }
    }
}
