using System;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelShiftReplacement
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int ReplacedPersonnelId { get; set; }
        public int WorkingHourId { get; set; }
        public int ReplacedWorkingHourId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ReplacementDate { get; set; }

        #region confirmation or rejection
        public DateTime? ActionDate { get; set; }
        public RequestAction RequestAction { get; set; }
        public string ActionDescription { get; set; }
        #endregion

        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public Personnel ReplacedPersonnel { get; set; }
        public WorkingHour WorkingHour { get; set; }
        public WorkingHour ReplacedWorkingHour { get; set; }
    }
}
