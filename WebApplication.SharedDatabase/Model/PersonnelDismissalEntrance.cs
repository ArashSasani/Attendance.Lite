using System;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelDismissalEntrance
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan Start { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? End { get; set; }
        public bool IsCompleted { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
    }
}
