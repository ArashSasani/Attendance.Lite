using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class HourlyShift
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
