using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class Device
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        [Required]
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public ActiveState ActiveState { get; set; }
        public DeleteState DeleteState { get; set; }
    }
}
