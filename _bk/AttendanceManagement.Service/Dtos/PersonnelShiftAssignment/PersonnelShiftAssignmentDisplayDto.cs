using System;

namespace AttendanceManagement.Service.Dtos.PersonnelShiftAssignment
{
    public class PersonnelShiftAssignmentDisplayDto
    {
        public int Id { get; set; }
        /// <summary>
        /// shift assigned date on calendar
        /// </summary>
        public DateTime Date { get; set; }
        public string FormattedDate { get; set; }
        public string DayOfWeek { get; set; }
        public string Label { get; set; }
        /// <summary>
        /// working hour duration
        /// </summary>
        public string Length { get; set; }
    }
}
