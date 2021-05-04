using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace CMS.Service.Dtos.Message
{
    public class CreateMessageDto
    {
        [Required(ErrorMessage = "*")]
        public string ReceiverId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        [MinLength(10, ErrorMessage = "min length should be 10 characters")]
        public string Content { get; set; }
        public MessageType MessageType { get; set; }

        public Request Request { get; set; }
    }

    public class Request
    {
        public int RequestId { get; set; }
        public RequestType RequestType { get; set; }
        public int? ParentApprovalProcId { get; set; }
        public RequestAction RequestAction { get; set; }
    }
}
