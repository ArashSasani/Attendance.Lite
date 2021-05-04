using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class DismissalApproval : IEntity
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DismissalId { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public virtual Dismissal Dismissal { get; set; }
    }
}
