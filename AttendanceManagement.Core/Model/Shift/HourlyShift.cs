using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class HourlyShift : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int? HoursShouldWorkInDay { get; set; }
        public int? HoursShouldWorkInWeek { get; set; }
        public int? HoursShouldWorkInMonth { get; set; }
        public DeleteState DeleteState { get; set; }
    }
}
