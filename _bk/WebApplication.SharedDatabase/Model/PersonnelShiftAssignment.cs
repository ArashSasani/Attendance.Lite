using System;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class PersonnelShiftAssignment
    {
        public int Id { get; set; }
        public int PersonnelShiftId { get; set; }
        public DateTime Date { get; set; }
        public DeleteState DeleteState { get; set; }

        public PersonnelShift PersonnelShift { get; set; }
    }
}
