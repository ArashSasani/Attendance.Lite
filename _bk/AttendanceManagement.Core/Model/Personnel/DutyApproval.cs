using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class DutyApproval : IEntity
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DutyId { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public virtual Duty Duty { get; set; }
    }
}
