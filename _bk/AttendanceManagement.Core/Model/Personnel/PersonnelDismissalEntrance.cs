using System;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelDismissalEntrance : IEntity
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
