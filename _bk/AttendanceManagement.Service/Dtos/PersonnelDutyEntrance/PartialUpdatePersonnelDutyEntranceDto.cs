using System;

namespace AttendanceManagement.Service.Dtos.PersonnelDutyEntrance
{
    public class PartialUpdatePersonnelDutyEntranceDto
    {
        public Core.Model.PersonnelDutyEntrance PersonnelDutyEntranceEntity { get; set; }
        public PersonnelDutyEntrancePatchDto PatchDto { get; set; }
    }

    public class PersonnelDutyEntrancePatchDto
    {
        public TimeSpan Start { get; set; }
        public TimeSpan? End { get; set; }
    }
}
