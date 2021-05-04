using System;
using System.Collections.Generic;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelShift : IEntity
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int ShiftId { get; set; }
        public DateTime DateAssigned { get; set; }
        public DeleteState DeleteState { get; set; }

        public virtual Personnel Personnel { get; set; }
        public virtual Shift Shift { get; set; }
        public ICollection<PersonnelShiftAssignment> PersonnelShiftAssignments { get; set; }
    }
}
