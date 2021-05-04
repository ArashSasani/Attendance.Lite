using System;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelHourlyShift
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int HourlyShiftId { get; set; }
        public DateTime DateAssigned { get; set; }
        public DeleteState DeleteState { get; set; }

        public virtual Personnel Personnel { get; set; }
        public virtual HourlyShift HourlyShift { get; set; }
    }
}
