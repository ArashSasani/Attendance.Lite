using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class WorkUnit : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DeleteState DeleteState { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}
