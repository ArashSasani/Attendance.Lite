using AutoMapper;
using CMS.Core.Model;
using CMS.Service.Dtos.Message;
using CMS.Service.Dtos.RestrictedAccessTime;
using CMS.Service.Dtos.RestrictedIP;
using CMS.Service.Dtos.UserLog;
using WebApplication.Infrastructure.Security;
using WebApplication.SharedKernel.Enums;

namespace CMS.Service.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region UserLog
            CreateMap<UserLog, UserLogDto>()
                .ForMember(dest => dest.UserName
                    , opt => opt.MapFrom(src => src.User.UserName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Date
                    , opt => opt.MapFrom(src => src.Date.ToShortDateString()));

            #endregion

            #region RestrictedIP
            CreateMap<RestrictedIP, RestrictedIPDto>();
            #endregion

            #region RestrictedAccessTime
            CreateMap<RestrictedAccessTime, RestrictedAccessTimeDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.HasValue
                        ? src.Date.Value.ToShortDateString() : "no restriction"))
                .ForMember(dest => dest.FromTime,
                    opt => opt.MapFrom(src => src.FromTime.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.ToTime,
                    opt => opt.MapFrom(src => src.ToTime.ToString("hh\\:mm\\:ss")));
            #endregion

            #region Message
            CreateMap<Message, MessageDtoForPaging>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.ToShortDateString()))
                .ForMember(dest => dest.SenderUsername,
                    opt => opt.MapFrom(src => src.Sender.UserName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SenderFullName,
                    opt => opt.MapFrom(src =>
                        src.Sender.Name.GetSafeHtmlFragment() + " " + src.Sender.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.IsReadTitle,
                    opt => opt.MapFrom(src => src.IsRead == false ? "new" : "is read"))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src is RequestMessage ? ((src as RequestMessage).RequestAction == RequestAction.Accept
                        ? "confirmed" : (src as RequestMessage).RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : (src as RequestMessage).RequestAction == RequestAction.Reject
                        ? "rejected" : "new") : "-"));

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.ToShortDateString()))
                .ForMember(dest => dest.SenderUsername,
                    opt => opt.MapFrom(src => src.Sender.UserName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SenderFullName,
                    opt => opt.MapFrom(src =>
                        src.Sender.Name.GetSafeHtmlFragment() + " " + src.Sender.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content.GetSafeHtmlFragment()))
                .ForMember(dest => dest.IsReadTitle,
                    opt => opt.MapFrom(src => src.IsRead == false ? "new" : "is read"))
                .ForMember(dest => dest.MessageTypeTitle,
                    opt => opt.MapFrom(src => src.MessageType == MessageType.Normal
                        ? "normal message" : src.MessageType == MessageType.Request
                        ? "request message" : ""));

            CreateMap<RequestMessage, MessageDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.ToShortDateString()))
                .ForMember(dest => dest.SenderUsername,
                    opt => opt.MapFrom(src => src.Sender.UserName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SenderFullName,
                    opt => opt.MapFrom(src =>
                        src.Sender.Name.GetSafeHtmlFragment() + " " + src.Sender.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content.GetSafeHtmlFragment()))
                .ForMember(dest => dest.IsReadTitle,
                    opt => opt.MapFrom(src => src.IsRead == false ? "new" : "is read"))
                .ForMember(dest => dest.MessageTypeTitle,
                    opt => opt.MapFrom(src => src.MessageType == MessageType.Normal
                        ? "normal message" : src.MessageType == MessageType.Request
                        ? "request message" : ""))
                .ForMember(dest => dest.RequestTypeTitle,
                    opt => opt.MapFrom(src => src.RequestType == RequestType.Dismissal
                        ? "dismissal request" : src.RequestType == RequestType.Duty
                        ? "duty request" : src.RequestType == RequestType.ShiftReplacement
                        ? "shift replacement request" : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));
            #endregion
        }
    }
}
