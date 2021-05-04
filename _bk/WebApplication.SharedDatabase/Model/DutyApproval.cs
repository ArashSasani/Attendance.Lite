using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class DutyApproval
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DutyId { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public Duty Duty { get; set; }
    }
}
