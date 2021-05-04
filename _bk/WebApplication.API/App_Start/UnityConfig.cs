#region cms
using CMS.Service;
using CMS.Service.Interfaces;
using CMS.Core.Interfaces;
#endregion
#region attendance management namespaces
using AttendanceManagement.Service.Interfaces;
using AttendanceManagement.Service;
//reports mvc
#endregion
#region realtime presentation service
using WebApplication.API.Realtime.Interfaces;
using WebApplication.API.Realtime;
#endregion
using System.Web.Http;
using Unity;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Logging;
using WebApplication.Infrastructure.Security;
using WebApplication.SharedKernel.Interfaces;

namespace WebApplication.API
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            #region SignalR
            container.RegisterType<INotificationService, NotificationService>();
            #endregion

            #region Application Services
            //cms
            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IUserLoggerService, UserLoggerService>();
            container.RegisterType<IRestrictedIPService, RestrictedIPService>();
            container.RegisterType<IRestrictedAccessTimeService, RestrictedAccessTimeService>();
            container.RegisterType<IMessageService, MessageService>();
            //attendance management
            container.RegisterType<IDismissalApprovalService, DismissalApprovalService>();
            container.RegisterType<IDismissalService, DismissalService>();
            container.RegisterType<IDutyApprovalService, DutyApprovalService>();
            container.RegisterType<IDutyService, DutyService>();
            container.RegisterType<IEmployeementTypeService, EmployeementTypeService>();
            container.RegisterType<IGroupCategoryService, GroupCategoryService>();
            container.RegisterType<IPersonnelDismissalService, PersonnelDismissalService>();
            container.RegisterType<IPersonnelDutyService, PersonnelDutyService>();
            container.RegisterType<IPositionService, PositionService>();
            container.RegisterType<IPersonnelService, PersonnelService>();
            container.RegisterType<IPersonnelApprovalProcService, PersonnelApprovalProcService>();
            container.RegisterType<IWorkUnitService, WorkUnitService>();
            container.RegisterType<IApprovalProcService, ApprovalProcService>();
            container.RegisterType<IShiftService, ShiftService>();
            container.RegisterType<IHourlyShiftService, HourlyShiftService>();
            container.RegisterType<IWorkingHourService, WorkingHourService>();
            container.RegisterType<ICalendarDateService, CalendarDateService>();
            container.RegisterType<IPersonnelShiftService, PersonnelShiftService>();
            container.RegisterType<IPersonnelShiftAssignmentService, PersonnelShiftAssignmentService>();
            container.RegisterType<IPersonnelHourlyShiftService, PersonnelHourlyShiftService>();
            container.RegisterType<IPersonnelShiftReplacementService, PersonnelShiftReplacementService>();
            container.RegisterType<IPersonnelEntranceService, PersonnelEntranceService>();
            container.RegisterType<IPersonnelDismissalEntranceService, PersonnelDismissalEntranceService>();
            container.RegisterType<IPersonnelDutyEntranceService, PersonnelDutyEntranceService>();
            container.RegisterType<IPersonnelProfileService, PersonnelProfileService>();
            container.RegisterType<IRequestMessageHandlerService, RequestMessageHandlerService>();
            container.RegisterType<IDashboardService, DashboardService>();
            container.RegisterType<IReportCalcService, ReportCalcService>();
            #endregion

            #region Domain Services
            #endregion

            #region Generic Repositories
            //cms
            container.RegisterType<IAuthRepository, CMS.Data.Repositories.AuthRepository>();
            container.RegisterType<IRepository<CMS.Core.Model.UserLog>
                , CMS.Data.Repositories.Repository<CMS.Core.Model.UserLog>>();
            container.RegisterType<IRepository<CMS.Core.Model.RestrictedIP>
                , CMS.Data.Repositories.Repository<CMS.Core.Model.RestrictedIP>>();
            container.RegisterType<IRepository<CMS.Core.Model.RestrictedAccessTime>
                , CMS.Data.Repositories.Repository<CMS.Core.Model.RestrictedAccessTime>>();
            container.RegisterType<IRepository<CMS.Core.Model.RequestMessage>
                , CMS.Data.Repositories.Repository<CMS.Core.Model.RequestMessage>>();
            container.RegisterType<IRepository<CMS.Core.Model.Message>
                , CMS.Data.Repositories.Repository<CMS.Core.Model.Message>>();
            //attendance management
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.GroupCategory>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.GroupCategory>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.EmployeementType>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.EmployeementType>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.WorkUnit>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.WorkUnit>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.Position>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.Position>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.Personnel>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.Personnel>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.DismissalApproval>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.DismissalApproval>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.DutyApproval>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.DutyApproval>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelDailyDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelDailyDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelHourlyDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelHourlyDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.Dismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.Dismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.DemandedDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.DemandedDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.SicknessDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.SicknessDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.WithoutSalaryDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.WithoutSalaryDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.EncouragementDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.EncouragementDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.MarriageDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.MarriageDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.ChildBirthDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.ChildBirthDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.BreastFeedingDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.BreastFeedingDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.DeathOfRelativesDismissal>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.DeathOfRelativesDismissal>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.Duty>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.Duty>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelDuty>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelDuty>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelDailyDuty>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelDailyDuty>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelHourlyDuty>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelHourlyDuty>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelApprovalProc>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelApprovalProc>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.ApprovalProc>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.ApprovalProc>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.Shift>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.Shift>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.WorkingHour>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.WorkingHour>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.HourlyShift>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.HourlyShift>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.CalendarDate>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.CalendarDate>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelShift>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelShift>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelHourlyShift>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelHourlyShift>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelShiftAssignment>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelShiftAssignment>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelShiftReplacement>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelShiftReplacement>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelEntrance>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelEntrance>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelDutyEntrance>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelDutyEntrance>>();
            container.RegisterType<IRepository<AttendanceManagement.Core.Model.PersonnelDismissalEntrance>
                , AttendanceManagement.Data.Repositories.Repository<AttendanceManagement.Core.Model.PersonnelDismissalEntrance>>();
            #endregion

            #region None-Entity Repositories
            //cms
            //...
            //attendance management
            //...
            #endregion

            #region Infrastructure Services
            container.RegisterType<IImageService, ImageService>();
            container.RegisterType<ISecurityService, SecurityService>();
            container.RegisterType<IExceptionLogger, ExceptionLogger>();
            container.RegisterType<ICustomLogger, CustomLogger>();
            #endregion

            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}