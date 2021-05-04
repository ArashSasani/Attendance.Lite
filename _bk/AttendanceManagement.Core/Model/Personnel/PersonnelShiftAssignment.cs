using System;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelShiftAssignment : IEntity
    {
        public int Id { get; set; }
        public int PersonnelShiftId { get; set; }
        public DateTime Date { get; set; }
        public DeleteState DeleteState { get; set; }

        public PersonnelShift PersonnelShift { get; set; }
    }
}
