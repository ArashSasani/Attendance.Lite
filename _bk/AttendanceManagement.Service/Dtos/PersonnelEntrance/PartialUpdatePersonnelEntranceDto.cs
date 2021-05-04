using System;

namespace AttendanceManagement.Service.Dtos.PersonnelEntrance
{
    public class PartialUpdatePersonnelEntranceDto
    {
        public Core.Model.PersonnelEntrance PersonnelEntranceEntity { get; set; }
        public PersonnelEntrancePatchDto PatchDto { get; set; }
    }

    public class PersonnelEntrancePatchDto
    {
        public TimeSpan Enter { get; set; }
        public TimeSpan? Exit { get; set; }
    }
}
