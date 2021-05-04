using System;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelShiftReplacement
{
    public class PersonnelShiftReplacementDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelFullName { get; set; }
        public int ReplacedPersonnelId { get; set; }
        public string ReplacedPersonnelFullName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftTitle { get; set; }
        public int WorkingHourId { get; set; }
        public string WorkingHourTitle { get; set; }
        public int ReplacedShiftId { get; set; }
        public string ReplacedShiftTitle { get; set; }
        public int ReplacedWorkingHourId { get; set; }
        public string ReplacedWorkingHourTitle { get; set; }
        public string RequestedDate { get; set; }
        public string ReplacementDate { get; set; }
        public string ActionDate { get; set; }
        public RequestAction RequestAction { get; set; }
        public string RequestActionTitle { get; set; }
        public string ActionDescription { get; set; }
    }

    public class ReplaceShiftDto
    {
        public string ShiftTitle { get; set; }
        public string WorkingHourTitle { get; set; }
        public DateTime Date { get; set; }
    }
}
