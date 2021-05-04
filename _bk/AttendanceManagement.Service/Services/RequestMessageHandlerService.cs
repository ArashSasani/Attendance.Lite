using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.ApprovalProc;
using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Interfaces;
using CMS.Service.Dtos.Message;
using CMS.Service.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class RequestMessageHandlerService : IRequestMessageHandlerService
    {
        private readonly IRepository<PersonnelApprovalProc> _personnelApprovalProcRepository;
        private readonly IRepository<PersonnelDismissal> _personnelDismissalRepository;
        private readonly IRepository<PersonnelDuty> _personnelDutyRepository;
        private readonly IRepository<PersonnelShiftReplacement> _personnelShiftReplacementRepository;

        private readonly IPersonnelService _personnelService;
        private readonly IApprovalProcService _approvalProcService;
        private readonly IMessageService _messageService;

        public RequestMessageHandlerService
            (IRepository<PersonnelApprovalProc> personnelApprovalProcRepositry
            , IRepository<PersonnelDismissal> personnelDismissalRepository
            , IRepository<PersonnelDuty> personnelDutyRepository
            , IRepository<PersonnelShiftReplacement> personnelShiftReplacementRepository
            , IPersonnelService personnelService, IApprovalProcService approvalProcService
            , IMessageService messageService)
        {
            _personnelApprovalProcRepository = personnelApprovalProcRepositry;
            _personnelDismissalRepository = personnelDismissalRepository;
            _personnelDutyRepository = personnelDutyRepository;
            _personnelShiftReplacementRepository = personnelShiftReplacementRepository;

            _personnelService = personnelService;
            _approvalProcService = approvalProcService;
            _messageService = messageService;
        }

        public async Task<string> HandleDismisalRequest(int personnelDismissalId
            , RequestDuration dismissalDuration, DateTime from, DateTime to, string senderUsername)
        {
            var personnel = _personnelService.GetByCode(senderUsername);

            var personnelApprovalProc = _personnelApprovalProcRepository
                .Get(q => q.Id == personnel.Id, includeProperties: "DismissalApprovalProc")
                .SingleOrDefault();

            string notificationReceiverId = null;
            if (personnelApprovalProc != null)
            {
                var receiverInfo = _approvalProcService
                    .GetNextReceiverId(personnelApprovalProc.DismissalApprovalProc);
                if (receiverInfo.ReceiverId != null)
                {
                    var personnelDismissal = _personnelDismissalRepository
                        .Get(q => q.Id == personnelDismissalId, includeProperties: "Dismissal")
                        .SingleOrDefault();

                    notificationReceiverId = await _messageService.Create(new CreateMessageDto
                    {
                        ReceiverId = receiverInfo.ReceiverId,
                        MessageType = MessageType.Request,
                        Title = GenerateDismissalRequestTitle(personnelDismissal.Dismissal),
                        Content = GenerateDismissalRequestContent(personnel, personnelDismissal.Dismissal
                            , dismissalDuration, from, to),
                        Request = new Request
                        {
                            RequestId = personnelDismissalId,
                            RequestType = RequestType.Dismissal,
                            ParentApprovalProcId = receiverInfo.ParentApprovalProcId
                        }
                    }, senderUsername);
                }
            }

            return notificationReceiverId;
        }

        public async Task<string> HandleDutyRequest(int personnelDutyId
            , RequestDuration dutyDuration, DateTime from, DateTime to, string senderUsername)
        {
            var personnel = _personnelService.GetByCode(senderUsername);

            var personnelApprovalProc = _personnelApprovalProcRepository
                    .Get(q => q.Id == personnel.Id, includeProperties: "DutyApprovalProc")
                    .SingleOrDefault();

            string notificationReceiverId = null;
            if (personnelApprovalProc != null)
            {
                var receiverInfo = _approvalProcService
                    .GetNextReceiverId(personnelApprovalProc.DutyApprovalProc);

                if (receiverInfo.ReceiverId != null)
                {
                    var personnelDuty = _personnelDutyRepository
                        .Get(q => q.Id == personnelDutyId, includeProperties: "Duty")
                        .SingleOrDefault();

                    notificationReceiverId = await _messageService.Create(new CreateMessageDto
                    {
                        ReceiverId = receiverInfo.ReceiverId,
                        MessageType = MessageType.Request,
                        Title = GenerateDutyRequestTitle(personnelDuty.Duty),
                        Content = GenerateDutyRequestContent(personnel, personnelDuty.Duty
                            , dutyDuration, from, to),
                        Request = new Request
                        {
                            RequestId = personnelDutyId,
                            RequestType = RequestType.Duty,
                            ParentApprovalProcId = receiverInfo.ParentApprovalProcId
                        }
                    }, senderUsername);
                }
            }

            return notificationReceiverId;
        }

        public async Task<string> HandleShiftReplacementRequest(int personnelShiftReplacementId
            , string senderUsername)
        {
            var shiftReplacement = _personnelShiftReplacementRepository
                .Get(q => q.Id == personnelShiftReplacementId
                , includeProperties: "ReplacedPersonnel,WorkingHour,ReplacedWorkingHour")
                .SingleOrDefault();

            var personnel = _personnelService.GetByCode(senderUsername);
            var replacedPersonnel = _personnelService.GetById(shiftReplacement.ReplacedPersonnelId);
            var shift = shiftReplacement.WorkingHour.Shift;
            var workingHour = shiftReplacement.WorkingHour;
            var replacedWorkingHour = shiftReplacement.ReplacedWorkingHour;

            var personnelApprovalProc = _personnelApprovalProcRepository
                .Get(q => q.Id == personnel.Id, includeProperties: "ShiftReplacementProc")
                .SingleOrDefault();

            string notificationReceiverId = null;
            if (personnelApprovalProc != null)
            {
                var receiverInfo = _approvalProcService
                    .GetNextReceiverId(personnelApprovalProc.ShiftReplacementProc);
                if (receiverInfo.ReceiverId != null)
                {
                    notificationReceiverId = await _messageService.Create(new CreateMessageDto
                    {
                        ReceiverId = receiverInfo.ReceiverId,
                        MessageType = MessageType.Request,
                        Title = "shift replacement reuqest",
                        Content = GenerateShiftReplacementRequestContent(personnel, replacedPersonnel
                            , shift.Title, workingHour.Title, replacedWorkingHour.Title
                            , shiftReplacement.ReplacementDate),
                        Request = new Request
                        {
                            RequestId = personnelShiftReplacementId,
                            RequestType = RequestType.ShiftReplacement,
                            ParentApprovalProcId = receiverInfo.ParentApprovalProcId
                        }
                    }, senderUsername);
                }
            }

            return notificationReceiverId;
        }

        public async Task<string> SubmitMessage(int messageId, ReceiverInfoDto receiverInfo
            , int requestId, RequestType requestType, RequestAction requestAction, string senderUsername)
        {
            var message = _messageService.GetById(messageId);

            return await _messageService.Create(new CreateMessageDto
            {
                ReceiverId = receiverInfo.ReceiverId,
                MessageType = MessageType.Request,
                Title = message != null ? message.Title : "",
                Content = message != null ? message.Content : "",
                Request = new Request
                {
                    RequestId = requestId,
                    RequestType = requestType,
                    ParentApprovalProcId = receiverInfo.ParentApprovalProcId,
                    RequestAction = requestAction
                }
            }, senderUsername);
        }

        public void UpdateRequestAction(int messageId, RequestAction requestAction)
        {
            _messageService.UpdateRequest(messageId, requestAction);
        }

        private string GenerateDismissalRequestTitle(Dismissal dismissal)
        {
            return "dismissal reuqest" + " " + dismissal.Title;
        }

        private string GenerateDutyRequestTitle(Duty duty)
        {
            return "duty reuqest" + " " + duty.Title;
        }

        private string GenerateDismissalRequestContent(PersonnelDto personnel
            , Dismissal dismissal, RequestDuration duration, DateTime from, DateTime to)
        {
            return "request type : "
                    + " " + (duration == RequestDuration.Daily ? "daily" : "hourly")
                    + "dismissal" + " " + dismissal.Title
                    + ", requested by: " + (personnel.Gender == Gender.Male ? "Mr." : "Mrs.")
                    + "" + personnel.FullName
                    + $", from date: {from.ToShortDateString()} to date:" +
                    $" {to.ToShortDateString()}.";
        }

        private string GenerateDutyRequestContent(PersonnelDto personnel
            , Duty duty, RequestDuration duration, DateTime from, DateTime to)
        {
            return "request type : "
                    + " " + (duration == RequestDuration.Daily ? "daily" : "hourly")
                    + "duty" + " " + duty.Title
                    + ", requested by: " + (personnel.Gender == Gender.Male ? "Mr." : "Mrs.")
                    + "" + personnel.FullName
                    + $", from date: {from.ToShortDateString()} to date:" +
                    $" {to.ToShortDateString()}.";
        }

        private string GenerateShiftReplacementRequestContent(PersonnelDto personnel
            , PersonnelDto replacedPersonnel, string shiftTitle
            , string workingHourTitle, string replacedWorkingHourTitle, DateTime date)
        {
            return "request type: shift replacement"
                + " " + ", requested by: "
                + (personnel.Gender == Gender.Male ? "Mr." : "Mrs.")
                + "" + personnel.FullName + "replace with" + " "
                + (replacedPersonnel.Gender == Gender.Male ? "Mr." : "Mrs.")
                + "" + replacedPersonnel.FullName
                + $" shift title: {shiftTitle}, working hour title: {workingHourTitle}"
                + " " + "replace with" + $" working hour title: {replacedWorkingHourTitle}"
                + $", requested date: {date.ToShortDateString()}.";
        }
    }
}
