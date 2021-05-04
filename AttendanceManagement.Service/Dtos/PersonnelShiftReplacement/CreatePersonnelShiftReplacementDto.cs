using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.PersonnelShiftReplacement
{
    public class CreatePersonnelShiftReplacementDto
    {
        public int ReplacedPersonnelId { get; set; }
        public int ShiftId { get; set; }
        public int WorkingHourId { get; set; }
        public int ReplacedShiftId { get; set; }
        public int ReplacedWorkingHourId { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime ReplacementDate { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }
    }
}
