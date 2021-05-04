using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class EmployeementType
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DeleteState DeleteState { get; set; }

        public ICollection<Personnel> Personnel { get; set; }
    }
}
