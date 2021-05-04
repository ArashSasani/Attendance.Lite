using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Service.Dtos.PersonnelProfile
{
    public class UpdatePersonnelProfileDto
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool IsPresent { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "confirm password is incorrect")]
        public string ConfirmPassword { get; set; }
    }
}
