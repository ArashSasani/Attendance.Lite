using System;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelHourlyShift : IEntity
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
