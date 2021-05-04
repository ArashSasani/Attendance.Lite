using System;
using System.Collections.Generic;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelShift
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int ShiftId { get; set; }
        public DateTime DateAssigned { get; set; }
        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public Shift Shift { get; set; }
        public ICollection<PersonnelShiftAssignment> PersonnelShiftAssignments { get; set; }
    }
}
