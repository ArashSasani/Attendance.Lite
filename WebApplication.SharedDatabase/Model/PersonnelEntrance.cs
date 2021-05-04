using System;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelEntrance
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int? WorkingHourId { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan Enter { get; set; }
        public bool AutoEnter { get; set; }
        public DateTime? ExitDate { get; set; }
        public TimeSpan? Exit { get; set; }
        public bool AutoExit { get; set; }
        public bool InProcess { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditDate { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public WorkingHour WorkingHour { get; set; }
    }
}
