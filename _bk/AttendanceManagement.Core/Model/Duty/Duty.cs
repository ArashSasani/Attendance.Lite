using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class Duty : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int? ActionLimitDays { get; set; }
        public DeleteState DeleteState { get; set; }

        public ICollection<DutyApproval> DutyApprovals { get; set; }
        public ICollection<PersonnelDuty> PersonnelDuties { get; set; }
    }
}
