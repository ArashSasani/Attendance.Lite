using WebApplication.SharedKernel.Enums;

namespace CMS.Service.Dtos.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverId { get; set; }
        public string SenderFullName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public string IsReadTitle { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageTypeTitle { get; set; }

        #region RequestMessage
        public int RequestId { get; set; }
        public RequestType RequestType { get; set; }
        public string RequestTypeTitle { get; set; }
        public int? ParentApprovalProcId { get; set; }
        public RequestAction RequestAction { get; set; }
        public string RequestActionTitle { get; set; }
        #endregion
    }

    public class NotificationDto
    {
        public MessageType MessageType { get; set; }
    }
}
