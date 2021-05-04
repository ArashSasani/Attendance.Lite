using AttendanceManagement.Service.Dtos.ApprovalProc;
using System;
using System.Threading.Tasks;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IRequestMessageHandlerService
    {
        Task<string> SubmitMessage(int messageId, ReceiverInfoDto receiverInfo, int requestId
            , RequestType requestType, RequestAction requestAction, string senderUsername);
        Task<string> HandleDutyRequest(int personnelDutyId, RequestDuration dutyDuration
            , DateTime from, DateTime to, string senderUsername);
        Task<string> HandleDismisalRequest(int personnelDismissalId, RequestDuration dismissalDuration
            , DateTime from, DateTime to, string senderUsername);
        Task<string> HandleShiftReplacementRequest(int personnelShiftReplacementId
            , string senderUsername);
        void UpdateRequestAction(int messageId, RequestAction requestAction);
    }
}
