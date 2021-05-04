using AttendanceManagement.Service.Dtos.ApprovalProc;
using System.Collections.Generic;

namespace AttendanceManagement.Service.Dtos.PersonnelProfile
{
    public class PersonnelProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool IsPresent { get; set; }
        public string IsPresentTitle { get; set; }

        #region Display personnel approval procs
        public List<ApprovalProcDtoForProfile> DismissalApprovalProcDto { get; set; }
        public List<ApprovalProcDtoForProfile> DutyApprovalProcDto { get; set; }
        public List<ApprovalProcDtoForProfile> ShiftReplacementApprovalProcDto { get; set; }
        #endregion
    }


}
