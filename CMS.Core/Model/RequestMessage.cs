using WebApplication.SharedKernel.Enums;

namespace CMS.Core.Model
{
    public class RequestMessage : Message
    {
        public int RequestId { get; set; }
        public RequestType RequestType { get; set; }
        public int? ParentApprovalProcId { get; set; }
        public RequestAction RequestAction { get; set; }
    }
}
