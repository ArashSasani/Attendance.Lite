using System;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance
{
    public class PartialUpdatePersonnelDismissalEntranceDto
    {
        public Core.Model.PersonnelDismissalEntrance PersonnelDismissalEntranceEntity { get; set; }
        public PersonnelDismissalEntrancePatchDto PatchDto { get; set; }
    }

    public class PersonnelDismissalEntrancePatchDto
    {
        public TimeSpan Start { get; set; }
        public TimeSpan? End { get; set; }
    }
}
