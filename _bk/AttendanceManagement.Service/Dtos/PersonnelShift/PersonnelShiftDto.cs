using AttendanceManagement.Service.Dtos.PersonnelShiftAssignment;
using System.Collections.Generic;

namespace AttendanceManagement.Service.Dtos.PersonnelShift
{
    public class PersonnelShiftDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelFullName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftTitle { get; set; }
        public string DateAssigned { get; set; }

        public List<PersonnelShiftAssignmentDisplayDto> ShiftAssignments { get; set; }
            = new List<PersonnelShiftAssignmentDisplayDto>();
    }
}
