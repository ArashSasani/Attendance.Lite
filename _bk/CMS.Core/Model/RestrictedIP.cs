using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Core.Model
{
    public class RestrictedIP : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string IP { get; set; }
        public DeleteState DeleteState { get; set; }
    }
}
