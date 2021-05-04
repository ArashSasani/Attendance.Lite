using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class DismissalApproval
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DismissalId { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public Dismissal Dismissal { get; set; }
    }
}
