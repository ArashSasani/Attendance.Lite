using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class Position : IEntity
    {
        public int Id { get; set; }
        public int WorkUnitId { get; set; }
        [Required]
        public string Title { get; set; }
        public DeleteState DeleteState { get; set; }

        public virtual WorkUnit WorkUnit { get; set; }
        public ICollection<Personnel> Personnel { get; set; }
    }
}
