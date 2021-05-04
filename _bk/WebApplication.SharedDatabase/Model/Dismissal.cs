using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class Dismissal
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DismissalType DismissalType { get; set; }
        public DismissalSystemType DismissalSystemType { get; set; }
        public DismissalExcessiveReaction DismissalExcessiveReaction { get; set; }
        public int? ActionLimitDays { get; set; }
        public DeleteState DeleteState { get; set; }

        public ICollection<DismissalApproval> DismissalApprovals { get; set; }
        public ICollection<PersonnelDismissal> DismissalPersonnel { get; set; }
    }
}
