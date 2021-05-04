using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class Position
    {
        public int Id { get; set; }
        public int WorkUnitId { get; set; }
        [Required]
        public string Title { get; set; }
        public DeleteState DeleteState { get; set; }

        public WorkUnit WorkUnit { get; set; }
        public ICollection<Personnel> Personnel { get; set; }
    }
}
