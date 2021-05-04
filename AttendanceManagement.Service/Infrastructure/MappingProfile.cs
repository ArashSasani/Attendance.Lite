using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.ApprovalProc;
using AttendanceManagement.Service.Dtos.CalendarDate;
using AttendanceManagement.Service.Dtos.Dismissal;
using AttendanceManagement.Service.Dtos.DismissalApproval;
using AttendanceManagement.Service.Dtos.Duty;
using AttendanceManagement.Service.Dtos.DutyApproval;
using AttendanceManagement.Service.Dtos.EmployeementType;
using AttendanceManagement.Service.Dtos.GroupCategory;
using AttendanceManagement.Service.Dtos.HourlyShift;
using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Dtos.PersonnelApprovalProc;
using AttendanceManagement.Service.Dtos.PersonnelDismissal;
using AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance;
using AttendanceManagement.Service.Dtos.PersonnelDuty;
using AttendanceManagement.Service.Dtos.PersonnelDutyEntrance;
using AttendanceManagement.Service.Dtos.PersonnelEntrance;
using AttendanceManagement.Service.Dtos.PersonnelHourlyShift;
using AttendanceManagement.Service.Dtos.PersonnelShift;
using AttendanceManagement.Service.Dtos.PersonnelShiftAssignment;
using AttendanceManagement.Service.Dtos.PersonnelShiftReplacement;
using AttendanceManagement.Service.Dtos.Position;
using AttendanceManagement.Service.Dtos.Shift;
using AttendanceManagement.Service.Dtos.WorkingHour;
using AttendanceManagement.Service.Dtos.WorkUnit;
using AutoMapper;
using WebApplication.Infrastructure.Enums;
using WebApplication.Infrastructure.Localization;
using WebApplication.Infrastructure.Security;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region GroupCategory
            CreateMap<GroupCategory, GroupCategoryDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            CreateMap<GroupCategory, GroupCategoryPatchDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region EmployeementType
            CreateMap<EmployeementType, EmployeementTypeDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            CreateMap<EmployeementType, EmployeementTypePatchDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region WorkUnit
            CreateMap<WorkUnit, WorkUnitDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            CreateMap<WorkUnit, WorkUnitPatchDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region Position
            CreateMap<Position, PositionDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.WorkUnitTitle
                    , opt => opt.MapFrom(src => src.WorkUnit.Title.GetSafeHtmlFragment()));
            CreateMap<Position, PositionDDLDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            CreateMap<Position, PositionPatchDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region Personnel
            CreateMap<Personnel, PersonnelDtoForPaging>()
                .ForMember(dest => dest.Name
                    , opt => opt.MapFrom(src => src.Name.GetSafeHtmlFragment()))
                .ForMember(dest => dest.LastName
                    , opt => opt.MapFrom(src => src.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.GroupCategoryTitle
                    , opt => opt.MapFrom(src => src.GroupCategory.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ActiveStateTitle
                    , opt => opt.MapFrom(src => src.ActiveState == ActiveState.Active
                        ? "active" : "inactive"));

            CreateMap<Personnel, PersonnelDto>()
                .ForMember(dest => dest.Name
                    , opt => opt.MapFrom(src => src.Name.GetSafeHtmlFragment()))
                .ForMember(dest => dest.LastName
                    , opt => opt.MapFrom(src => src.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.BirthDate
                    , opt => opt.MapFrom(src => src.BirthDate.HasValue
                        ? src.BirthDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.EducationTitle
                    , opt => opt.MapFrom(src => src.Education == Education.BeforeDiploma
                        ? "no degree" : src.Education == Education.Diploma
                        ? "diploma" : src.Education == Education.AdvancedDiploma
                        ? "certificate" : src.Education == Education.Bachelor
                        ? "under graduate" : src.Education == Education.Master
                        ? "graduate" : src.Education == Education.Phd
                        ? "phs" : src.Education == Education.PostDoc
                        ? "post phd" : ""))
                .ForMember(dest => dest.MilitaryServiceStatusTitle
                    , opt => opt.MapFrom(src => src.MilitaryServiceStatus == MilitaryServiceStatus.Completed
                        ? "done" : src.MilitaryServiceStatus == MilitaryServiceStatus.Included
                        ? "in progress" : src.MilitaryServiceStatus == MilitaryServiceStatus.Exempt
                        ? "exempt" : ""))
                .ForMember(dest => dest.GenderTitle
                    , opt => opt.MapFrom(src => src.Gender == Gender.Female
                        ? "female" : "male"))
                .ForMember(dest => dest.MaritalStatusTitle
                    , opt => opt.MapFrom(src => src.MaritalStatus == MaritalStatus.Single
                        ? "single" : src.MaritalStatus == MaritalStatus.Married
                        ? "married" : ""))
                .ForMember(dest => dest.GroupCategoryTitle
                    , opt => opt.MapFrom(src => src.GroupCategory.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.EmployeementTypeTitle
                    , opt => opt.MapFrom(src => src.EmployeemnetType.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.WorkUnitId
                    , opt => opt.MapFrom(src => src.Position.WorkUnit.Id))
                .ForMember(dest => dest.WorkUnitTitle
                    , opt => opt.MapFrom(src => src.Position.WorkUnit.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.PositionTitle
                    , opt => opt.MapFrom(src => src.Position.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DateOfEmployeement
                    , opt => opt.MapFrom(src => src.DateOfEmployeement.ToShortDateString()))
                .ForMember(dest => dest.FirstDateOfWork
                    , opt => opt.MapFrom(src => src.FirstDateOfWork.ToShortDateString()))
                .ForMember(dest => dest.LastDateOfWork
                    , opt => opt.MapFrom(src => src.LastDateOfWork.HasValue
                        ? src.LastDateOfWork.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.DismissalApprovalProcId
                    , opt => opt.MapFrom(src => src.PersonnelApprovalProc.DismissalApprovalProc.Id))
                .ForMember(dest => dest.DismissalApprovalProcTitle
                    , opt => opt.MapFrom(src => src.PersonnelApprovalProc.DismissalApprovalProc.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyApprovalProcId
                    , opt => opt.MapFrom(src => src.PersonnelApprovalProc.DutyApprovalProc.Id))
                .ForMember(dest => dest.DutyApprovalProcTitle
                    , opt => opt.MapFrom(src => src.PersonnelApprovalProc.DutyApprovalProc.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftReplacementProcId
                    , opt => opt.MapFrom(src => src.PersonnelApprovalProc.ShiftReplacementProc.Id))
                .ForMember(dest => dest.ShiftReplacementProcTitle
                    , opt => opt.MapFrom(src => src.PersonnelApprovalProc.ShiftReplacementProc.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ActiveStateTitle
                    , opt => opt.MapFrom(src => src.ActiveState == ActiveState.Active
                        ? "active" : "inactive"));

            CreateMap<Personnel, PersonnelRecordDto>()
                .ForMember(dest => dest.Name
                    , opt => opt.MapFrom(src => src.Name.GetSafeHtmlFragment()))
                .ForMember(dest => dest.LastName
                    , opt => opt.MapFrom(src => src.LastName.GetSafeHtmlFragment()));

            CreateMap<Personnel, PersonnelPatchDto>()
                .ForMember(dest => dest.Name
                    , opt => opt.MapFrom(src => src.Name.GetSafeHtmlFragment()))
                .ForMember(dest => dest.LastName
                    , opt => opt.MapFrom(src => src.LastName.GetSafeHtmlFragment()));

            #endregion

            #region PersonnelApprovalProc
            CreateMap<PersonnelApprovalProc, PersonnelApprovalProcDto>();
            #endregion

            #region DismissalApproval
            CreateMap<DismissalApproval, DismissalApprovalDto>()
                .ForMember(dest => dest.PersonnelFullName
                    , opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName)
                         .GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalTitle
                    , opt => opt.MapFrom(src => src.Dismissal.Title.GetSafeHtmlFragment()));

            CreateMap<DismissalApproval, DismissalApprovalDtoDDL>()
                .ForMember(dest => dest.DismissalId
                    , opt => opt.MapFrom(src => src.Dismissal.Id))
                .ForMember(dest => dest.DismissalTitle
                    , opt => opt.MapFrom(src => src.Dismissal.Title.GetSafeHtmlFragment()));
            #endregion

            #region DutyApproval
            CreateMap<DutyApproval, DutyApprovalDto>()
                .ForMember(dest => dest.PersonnelFullName
                    , opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName)
                         .GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyTitle
                    , opt => opt.MapFrom(src => src.Duty.Title.GetSafeHtmlFragment()));

            CreateMap<DutyApproval, DutyApprovalDtoDDL>()
                .ForMember(dest => dest.DutyId
                    , opt => opt.MapFrom(src => src.Duty.Id))
                .ForMember(dest => dest.DutyTitle
                    , opt => opt.MapFrom(src => src.Duty.Title.GetSafeHtmlFragment()));
            #endregion

            #region PersonnelDismissal
            CreateMap<PersonnelDismissal, PersonnelDismissalDtoForPaging>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalTitle,
                    opt => opt.MapFrom(src => src.Dismissal.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalDurationTitle,
                    opt => opt.MapFrom(src => src.DismissalDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));

            CreateMap<PersonnelDismissal, PersonnelDismissalDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalTitle,
                    opt => opt.MapFrom(src => src.Dismissal.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.DismissalDurationTitle,
                    opt => opt.MapFrom(src => src.DismissalDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));

            CreateMap<PersonnelDailyDismissal, PersonnelDismissalDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalTitle,
                    opt => opt.MapFrom(src => src.Dismissal.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.DismissalDurationTitle,
                    opt => opt.MapFrom(src => src.DismissalDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"))
                .ForMember(dest => dest.FromDate,
                    opt => opt.MapFrom(src => src.FromDate.ToShortDateString()))
                .ForMember(dest => dest.ToDate,
                    opt => opt.MapFrom(src => src.ToDate.ToShortDateString()));

            CreateMap<PersonnelHourlyDismissal, PersonnelDismissalDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalTitle,
                    opt => opt.MapFrom(src => src.Dismissal.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.DismissalDurationTitle,
                    opt => opt.MapFrom(src => src.DismissalDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.ToShortDateString()))
                .ForMember(dest => dest.FromTime,
                    opt => opt.MapFrom(src => src.FromTime.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.ToTime,
                    opt => opt.MapFrom(src => src.ToTime.ToString("hh\\:mm\\:ss")));
            #endregion

            #region Dismissal
            CreateMap<Dismissal, DismissalDtoForPaging>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"));

            CreateMap<Dismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
                .ForMember(dest => dest.ActionLimitDaysTitle,
                    opt => opt.MapFrom(src => src.ActionLimitDays.HasValue
                        ? $"{src.ActionLimitDays.Value} day" : "no restriction"));

            #region Demanded
            CreateMap<DemandedDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
                .ForMember(dest => dest.DemandedAllowanceInMonth,
                    opt => opt.MapFrom(src => src.DemandedAllowanceInMonth.GetDurationFromSeconds()))
                .ForMember(dest => dest.DemandedIsTransferableToNextMonthTitle,
                    opt => opt.MapFrom(src => src.DemandedIsTransferableToNextMonth == true ? "active" : "inactive"))
                .ForMember(dest => dest.DemandedTransferableAllowanceToNextMonth,
                    opt => opt.MapFrom(src => src.DemandedTransferableAllowanceToNextMonth.GetDurationFromSeconds()))
                .ForMember(dest => dest.DemandedAllowanceInYear,
                    opt => opt.MapFrom(src => src.DemandedAllowanceInYear.GetDurationFromSeconds()))
                .ForMember(dest => dest.DemandedIsTransferableToNextYearTitle,
                    opt => opt.MapFrom(src => src.DemandedIsTransferableToNextYear == true ? "active" : "inactive"))
                .ForMember(dest => dest.DemandedTransferableAllowanceToNextYear,
                    opt => opt.MapFrom(src => src.DemandedTransferableAllowanceToNextYear.GetDurationFromSeconds()))
                .ForMember(dest => dest.DemandedIsAllowedToSaveTitle,
                    opt => opt.MapFrom(src => src.DemandedIsAllowedToSave == true ? "active" : "inactive"))
                .ForMember(dest => dest.DemandedAllowanceToSave,
                    opt => opt.MapFrom(src => src.DemandedAllowanceToSave.GetDurationFromSeconds()))
                .ForMember(dest => dest.DemandedMealTimeIsIncludedTitle,
                    opt => opt.MapFrom(src => src.DemandedMealTimeIsIncluded == true ? "active" : "inactive"))
                .ForMember(dest => dest.DemandedDoesDismissalMeansExtraWorkTitle,
                    opt => opt.MapFrom(src => src.DemandedDoesDismissalMeansExtraWork == true ? "active" : "inactive"))
                .ForMember(dest => dest.DemandedIsNationalHolidysConsideredInDismissalTitle,
                    opt => opt.MapFrom(src => src.DemandedIsNationalHolidysConsideredInDismissal == true ? "active" : "inactive"))
                .ForMember(dest => dest.DemandedIsFridaysConsideredInDismissalTitle,
                    opt => opt.MapFrom(src => src.DemandedIsFridaysConsideredInDismissal == true ? "active" : "inactive"));

            CreateMap<DemandedDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForMonth,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " in month"))
                .ForMember(dest => dest.LimitValueForMonth,
                    opt => opt.MapFrom(src => src.DemandedAllowanceInMonth.GetDurationFromSeconds()))
                .ForMember(dest => dest.CountTitleForMonth,
                    opt => opt.MapFrom(src => "dismissal count limit " + src.Title.GetSafeHtmlFragment() + " in month"))
                .ForMember(dest => dest.CountValueForMonth,
                    opt => opt.MapFrom(src => src.DemandedCountInMonth))
                .ForMember(dest => dest.LimitTitleForYear,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " in year"))
                .ForMember(dest => dest.LimitValueForYear,
                    opt => opt.MapFrom(src => src.DemandedAllowanceInYear.GetDurationFromSeconds()))
                .ForMember(dest => dest.CountTitleForYear,
                    opt => opt.MapFrom(src => "dismissal count limit " + src.Title.GetSafeHtmlFragment() + " in year"))
                .ForMember(dest => dest.CountValueForYear,
                    opt => opt.MapFrom(src => src.DemandedCountInYear));
            #endregion

            #region Sickness
            CreateMap<SicknessDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.SicknessAllowanceInYear,
                    opt => opt.MapFrom(src => src.SicknessAllowanceInYear.GetDurationFromSeconds()))
                .ForMember(dest => dest.SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle,
                    opt => opt.MapFrom(src => src.SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit == true
                        ? "active" : "inactive"));

            CreateMap<SicknessDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForYear,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " in year"))
                .ForMember(dest => dest.LimitValueForYear,
                    opt => opt.MapFrom(src => src.SicknessAllowanceInYear.GetDurationFromSeconds()))
                .ForMember(dest => dest.CountTitleForYear,
                    opt => opt.MapFrom(src => "dismissal count limit " + src.Title.GetSafeHtmlFragment() + " in year"))
                .ForMember(dest => dest.CountValueForYear,
                    opt => opt.MapFrom(src => src.SicknessCountInYear));
            #endregion

            #region WithoutSalary
            CreateMap<WithoutSalaryDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.WithoutSalaryAllowanceInMonth,
                    opt => opt.MapFrom(src => src.WithoutSalaryAllowanceInMonth.GetDurationFromSeconds()));

            CreateMap<WithoutSalaryDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForMonth,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " in month"))
                .ForMember(dest => dest.LimitValueForMonth,
                    opt => opt.MapFrom(src => src.WithoutSalaryAllowanceInMonth.GetDurationFromSeconds()))
                .ForMember(dest => dest.CountTitleForMonth,
                    opt => opt.MapFrom(src => "dismissal count limit " + src.Title.GetSafeHtmlFragment() + " in month"))
                .ForMember(dest => dest.CountValueForMonth,
                    opt => opt.MapFrom(src => src.WithoutSalaryCountInMonth));
            #endregion

            #region Encouragement
            CreateMap<EncouragementDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.EncouragementFromDate,
                    opt => opt.MapFrom(src => src.EncouragementFromDate.ToShortDateString()))
               .ForMember(dest => dest.EncouragementToDate,
                    opt => opt.MapFrom(src => src.EncouragementToDate.ToShortDateString()))
               .ForMember(dest => dest.EncouragementConsiderWithoutSalaryTitle,
                    opt => opt.MapFrom(src => src.EncouragementConsiderWithoutSalary == true
                        ? "active" : "inactive"));
            #endregion

            #region Marriage
            CreateMap<MarriageDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.MarriageAllowanceInTotal,
                    opt => opt.MapFrom(src => src.MarriageAllowanceInTotal.GetDurationFromSeconds()))
               .ForMember(dest => dest.MarriageConsiderWithoutSalaryTitle,
                    opt => opt.MapFrom(src => src.MarriageConsiderWithoutSalary == true
                        ? "active" : "inactive"))
               .ForMember(dest => dest.MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle,
                    opt => opt.MapFrom(src => src.MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit == true
                        ? "active" : "inactive"));

            CreateMap<MarriageDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForTotal,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " total"))
                .ForMember(dest => dest.LimitValueForTotal,
                    opt => opt.MapFrom(src => src.MarriageAllowanceInTotal.GetDurationFromSeconds()))
                .ForMember(dest => dest.CountTitleForTotal,
                    opt => opt.MapFrom(src => "dismissal count limit " + src.Title.GetSafeHtmlFragment() + " total"))
                .ForMember(dest => dest.CountValueForTotal,
                    opt => opt.MapFrom(src => src.MarriageCountInTotal));
            #endregion

            #region ChildBirth
            CreateMap<ChildBirthDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.ChildBirthAllowanceInTotal,
                    opt => opt.MapFrom(src => src.ChildBirthAllowanceInTotal.GetDurationFromSeconds()))
               .ForMember(dest => dest.ChildBirthConsiderWithoutSalaryTitle,
                    opt => opt.MapFrom(src => src.ChildBirthConsiderWithoutSalary == true
                        ? "active" : "inactive"))
               .ForMember(dest => dest.ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle,
                    opt => opt.MapFrom(src => src.ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit == true
                        ? "active" : "inactive"));

            CreateMap<ChildBirthDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForTotal,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " total"))
                .ForMember(dest => dest.LimitValueForTotal,
                    opt => opt.MapFrom(src => src.ChildBirthAllowanceInTotal.GetDurationFromSeconds()));
            #endregion

            #region BreastFeeding
            CreateMap<BreastFeedingDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.BreastFeedingAllowanceInTotal,
                    opt => opt.MapFrom(src => src.BreastFeedingAllowanceInTotal.GetDurationFromSeconds()))
               .ForMember(dest => dest.BreastFeedingAllowanceInDay,
                    opt => opt.MapFrom(src => src.BreastFeedingAllowanceInDay.GetDurationFromSeconds()))
               .ForMember(dest => dest.BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle,
                    opt => opt.MapFrom(src => src.BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit == true
                        ? "active" : "inactive"));

            CreateMap<BreastFeedingDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForDay,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " در day"))
                .ForMember(dest => dest.LimitValueForDay,
                    opt => opt.MapFrom(src => src.BreastFeedingAllowanceInDay.GetDurationFromSeconds()))
                .ForMember(dest => dest.LimitTitleForTotal,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " total"))
                .ForMember(dest => dest.LimitValueForTotal,
                    opt => opt.MapFrom(src => src.BreastFeedingAllowanceInTotal.GetDurationFromSeconds()))
                .ForMember(dest => dest.CountTitleForDay,
                    opt => opt.MapFrom(src => "dismissal count limit " + src.Title.GetSafeHtmlFragment() + " در day"))
                .ForMember(dest => dest.CountValueForDay,
                    opt => opt.MapFrom(src => src.BreastFeedingCountInDay));
            #endregion

            #region DeathOfRelatives
            CreateMap<DeathOfRelativesDismissal, DismissalDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DismissalSystemTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalSystemType == DismissalSystemType.Default
                        ? "default" : "customized"))
                .ForMember(dest => dest.DismissalTypeTitle,
                    opt => opt.MapFrom(src => src.DismissalType == DismissalType.Demanded
                        ? "demanded" : src.DismissalType == DismissalType.Sickness
                        ? "sickness" : src.DismissalType == DismissalType.WithoutSalary
                        ? "without salary" : src.DismissalType == DismissalType.Encouragement
                        ? "encouragement" : src.DismissalType == DismissalType.Marriage
                        ? "marriage" : src.DismissalType == DismissalType.ChildBirth
                        ? "child birth" : src.DismissalType == DismissalType.BreastFeeding
                        ? "breast feeding" : src.DismissalType == DismissalType.DeathOfRelatives
                        ? "death of relatives" : ""))
                .ForMember(dest => dest.DismissalExcessiveReactionTitle,
                    opt => opt.MapFrom(src => src.DismissalExcessiveReaction == DismissalExcessiveReaction.Alarm
                        ? "alarm" : src.DismissalExcessiveReaction == DismissalExcessiveReaction.Forbid
                        ? "forbidden" : "Ok"))
               .ForMember(dest => dest.DeathOfRelativesAllowanceInTotal,
                    opt => opt.MapFrom(src => src.DeathOfRelativesAllowanceInTotal.GetDurationFromSeconds()))
               .ForMember(dest => dest.DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle,
                    opt => opt.MapFrom(src => src.DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit == true
                        ? "active" : "inactive"));

            CreateMap<DeathOfRelativesDismissal, DismissalChartDto>()
                .ForMember(dest => dest.LimitTitleForTotal,
                    opt => opt.MapFrom(src => "dismissal limit " + src.Title.GetSafeHtmlFragment() + " total"))
                .ForMember(dest => dest.LimitValueForTotal,
                    opt => opt.MapFrom(src => src.DeathOfRelativesAllowanceInTotal.GetDurationFromSeconds()));
            #endregion

            CreateMap<Dismissal, DismissalDtoDDL>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));

            CreateMap<Dismissal, DismissalPatchDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region Duty
            CreateMap<Duty, DutyDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ActionLimitDaysTitle,
                    opt => opt.MapFrom(src => src.ActionLimitDays.HasValue
                        ? $"{src.ActionLimitDays.Value} day" : "no restriction"));

            CreateMap<Duty, DutyPatchDto>()
                .ForMember(dest => dest.Title
                    , opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region PersonnelDuty
            CreateMap<PersonnelDuty, PersonnelDutyDtoForPaging>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyTitle,
                    opt => opt.MapFrom(src => src.Duty.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyDurationTitle,
                    opt => opt.MapFrom(src => src.DutyDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));

            CreateMap<PersonnelDuty, PersonnelDutyDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyTitle,
                    opt => opt.MapFrom(src => src.Duty.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.DutyDurationTitle,
                    opt => opt.MapFrom(src => src.DutyDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));

            CreateMap<PersonnelDailyDuty, PersonnelDutyDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyTitle,
                    opt => opt.MapFrom(src => src.Duty.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.DutyDurationTitle,
                    opt => opt.MapFrom(src => src.DutyDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"))
                .ForMember(dest => dest.FromDate,
                    opt => opt.MapFrom(src => src.FromDate.ToShortDateString()))
                .ForMember(dest => dest.ToDate,
                    opt => opt.MapFrom(src => src.ToDate.ToShortDateString()));

            CreateMap<PersonnelHourlyDuty, PersonnelDutyDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName).GetSafeHtmlFragment()))
                .ForMember(dest => dest.DutyTitle,
                    opt => opt.MapFrom(src => src.Duty.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SubmittedDate,
                    opt => opt.MapFrom(src => src.SubmittedDate.ToShortDateString()))
                .ForMember(dest => dest.DutyDurationTitle,
                    opt => opt.MapFrom(src => src.DutyDuration == RequestDuration.Daily
                        ? "daily" : "hourly"))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.ToShortDateString()))
                .ForMember(dest => dest.FromTime,
                    opt => opt.MapFrom(src => src.FromTime.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.ToTime,
                    opt => opt.MapFrom(src => src.ToTime.ToString("hh\\:mm\\:ss")));
            #endregion

            #region Shift
            CreateMap<Shift, ShiftDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));

            CreateMap<Shift, ShiftDDLDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));

            CreateMap<Shift, ShiftPatchDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region HourlyShift
            CreateMap<HourlyShift, HourlyShiftDtoForPaging>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.HoursShouldWorkInDayTitle,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInDay.HasValue
                            ? src.HoursShouldWorkInDay.Value.GetHoursFromSeconds() + " " + "hour(s)" : null))
                .ForMember(dest => dest.HoursShouldWorkInMonthTitle,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInMonth.HasValue
                            ? src.HoursShouldWorkInMonth.Value.GetHoursFromSeconds() + " " + "hour(s)" : null))
                .ForMember(dest => dest.HoursShouldWorkInWeekTitle,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInWeek.HasValue
                            ? src.HoursShouldWorkInWeek.Value.GetHoursFromSeconds() + " " + "hour(s)" : null));

            CreateMap<HourlyShift, HourlyShiftDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.HoursShouldWorkInDay,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInDay.HasValue
                            ? (int?)src.HoursShouldWorkInDay.Value.GetHoursFromSeconds() : null))
                .ForMember(dest => dest.HoursShouldWorkInDayTitle,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInDay.HasValue
                            ? src.HoursShouldWorkInDay.Value.GetHoursFromSeconds() + " " + "hour(s)" : null))
                .ForMember(dest => dest.HoursShouldWorkInMonth,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInMonth.HasValue
                            ? (int?)src.HoursShouldWorkInMonth.Value.GetHoursFromSeconds() : null))
                .ForMember(dest => dest.HoursShouldWorkInMonthTitle,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInMonth.HasValue
                            ? src.HoursShouldWorkInMonth.Value.GetHoursFromSeconds() + " " + "hour(s)" : null))
                .ForMember(dest => dest.HoursShouldWorkInWeek,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInWeek.HasValue
                            ? (int?)src.HoursShouldWorkInWeek.Value.GetHoursFromSeconds() : null))
                .ForMember(dest => dest.HoursShouldWorkInWeekTitle,
                        opt => opt.MapFrom(src => src.HoursShouldWorkInWeek.HasValue
                            ? src.HoursShouldWorkInWeek.Value.GetHoursFromSeconds() + " " + "hour(s)" : null));

            CreateMap<HourlyShift, HourlyShiftPatchDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region WorkingHour
            CreateMap<WorkingHour, WorkingHourDtoForPaging>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => src.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.FromTime,
                    opt => opt.MapFrom(src => src.FromTime.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.ToTime,
                    opt => opt.MapFrom(src => src.ToTime.ToString("hh\\:mm\\:ss")));

            CreateMap<WorkingHour, WorkingHourDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => src.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.FromTime,
                    opt => opt.MapFrom(src => src.FromTime.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.ToTime,
                    opt => opt.MapFrom(src => src.ToTime.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.WorkingHourDurationTitle,
                    opt => opt.MapFrom(src => src.WorkingHourDuration == WorkingHourDuration.OneDay
                        ? "same day" : src.WorkingHourDuration == WorkingHourDuration.TwoDays
                        ? "day after" : "2 days after"))
                .ForMember(dest => dest.DailyDelay,
                    opt => opt.MapFrom(src => src.DailyDelay.GetDurationFromSeconds()))
                .ForMember(dest => dest.MonthlyDelay,
                    opt => opt.MapFrom(src => src.MonthlyDelay.GetDurationFromSeconds()))
                .ForMember(dest => dest.DailyRush,
                    opt => opt.MapFrom(src => src.DailyRush.GetDurationFromSeconds()))
                .ForMember(dest => dest.MonthlyRush,
                    opt => opt.MapFrom(src => src.MonthlyRush.GetDurationFromSeconds()))
                .ForMember(dest => dest.PriorExtraWorkTime,
                    opt => opt.MapFrom(src => src.PriorExtraWorkTime.GetDurationFromSeconds()))
                .ForMember(dest => dest.LaterExtraWorkTime,
                    opt => opt.MapFrom(src => src.LaterExtraWorkTime.GetDurationFromSeconds()))
                .ForMember(dest => dest.FloatingTime,
                    opt => opt.MapFrom(src => src.FloatingTime.GetDurationFromSeconds()))
                .ForMember(dest => dest.MealTimeBreakFromTime,
                   opt => opt.MapFrom(src => src.MealTimeBreakFromTime.HasValue
                        ? src.MealTimeBreakFromTime.Value.ToString("hh\\:mm\\:ss") : ""))
                .ForMember(dest => dest.MealTimeBreakToTime,
                   opt => opt.MapFrom(src => src.MealTimeBreakToTime.HasValue
                        ? src.MealTimeBreakToTime.Value.ToString("hh\\:mm\\:ss") : ""))
                //breaks
                .ForMember(dest => dest.Break1FromTime,
                   opt => opt.MapFrom(src => src.Break1FromTime.HasValue
                        ? src.Break1FromTime.Value.ToString("hh\\:mm\\:ss") : ""))
                .ForMember(dest => dest.Break1ToTime,
                   opt => opt.MapFrom(src => src.Break1ToTime.HasValue
                        ? src.Break1ToTime.Value.ToString("hh\\:mm\\:ss") : ""))
                .ForMember(dest => dest.Break2FromTime,
                   opt => opt.MapFrom(src => src.Break2FromTime.HasValue
                        ? src.Break2FromTime.Value.ToString("hh\\:mm\\:ss") : ""))
                .ForMember(dest => dest.Break2ToTime,
                   opt => opt.MapFrom(src => src.Break2ToTime.HasValue
                        ? src.Break2ToTime.Value.ToString("hh\\:mm\\:ss") : ""))
                .ForMember(dest => dest.Break3FromTime,
                   opt => opt.MapFrom(src => src.Break3FromTime.HasValue
                        ? src.Break3FromTime.Value.ToString("hh\\:mm\\:ss") : ""))
                .ForMember(dest => dest.Break3ToTime,
                   opt => opt.MapFrom(src => src.Break3ToTime.HasValue
                        ? src.Break3ToTime.Value.ToString("hh\\:mm\\:ss") : ""));

            CreateMap<WorkingHour, WorkingHourDDLDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));

            CreateMap<WorkingHour, WorkingHourPatchDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region PersonnelShift
            CreateMap<PersonnelShift, PersonnelShiftDtoForPaging>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => src.Personnel.Name.GetSafeHtmlFragment()
                        + " " + src.Personnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => src.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DateAssigned,
                    opt => opt.MapFrom(src => src.DateAssigned.ToShortDateString()))
                .ForMember(dest => dest.AssignmentsCount,
                    opt => opt.MapFrom(src => src.PersonnelShiftAssignments.Count));

            CreateMap<PersonnelShift, PersonnelShiftDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => src.Personnel.Name.GetSafeHtmlFragment()
                        + " " + src.Personnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => src.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DateAssigned,
                    opt => opt.MapFrom(src => src.DateAssigned.ToShortDateString()))
                .ForMember(dest => dest.ShiftAssignments,
                    opt => opt.MapFrom(src => src.PersonnelShiftAssignments));
            #endregion

            #region PersonnelHourlyShift
            CreateMap<PersonnelHourlyShift, PersonnelHourlyShiftDtoForPaging>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => src.Personnel.Name.GetSafeHtmlFragment()
                        + " " + src.Personnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.HourlyShiftTitle,
                    opt => opt.MapFrom(src => src.HourlyShift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DateAssigned,
                    opt => opt.MapFrom(src => src.DateAssigned.ToShortDateString()));

            CreateMap<PersonnelHourlyShift, PersonnelHourlyShiftDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => src.Personnel.Name.GetSafeHtmlFragment()
                        + " " + src.Personnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.HourlyShiftTitle,
                    opt => opt.MapFrom(src => src.HourlyShift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DateAssigned,
                    opt => opt.MapFrom(src => src.DateAssigned.ToShortDateString()));
            #endregion

            #region PersonnelShiftAssignment
            CreateMap<PersonnelShiftAssignment, PersonnelShiftAssignmentDisplayDto>()
                .ForMember(dest => dest.FormattedDate,
                    opt => opt.MapFrom(src => src.Date.ToShortDateString()))
                .ForMember(dest => dest.DayOfWeek,
                    opt => opt.MapFrom(src => src.Date.DayOfWeek.GetDayOfWeekGC(CultureInfoTag.English_US, OutputDateFormat.ShortForm)))
                .ForMember(dest => dest.Label,
                    opt => opt.MapFrom(src => (src.PersonnelShift.Shift.Title)
                        .GetSafeHtmlFragment()));
            #endregion

            #region PersonnelShiftReplacement
            CreateMap<PersonnelShiftReplacement, PersonnelShiftReplacementDtoForPaging>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => src.Personnel.Name.GetSafeHtmlFragment()
                        + " " + src.Personnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ReplacedPersonnelFullName,
                    opt => opt.MapFrom(src => src.ReplacedPersonnel.Name.GetSafeHtmlFragment()
                        + " " + src.ReplacedPersonnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => src.WorkingHour.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ReplacedShiftTitle,
                    opt => opt.MapFrom(src => src.ReplacedWorkingHour.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ReplacementDate,
                    opt => opt.MapFrom(src => src.ReplacementDate.ToShortDateString()))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));

            CreateMap<PersonnelShiftReplacement, PersonnelShiftReplacementDto>()
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => src.Personnel.Name.GetSafeHtmlFragment()
                        + " " + src.Personnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ReplacedPersonnelFullName,
                    opt => opt.MapFrom(src => src.ReplacedPersonnel.Name.GetSafeHtmlFragment()
                        + " " + src.ReplacedPersonnel.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => src.WorkingHour.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ShiftId,
                    opt => opt.MapFrom(src => src.WorkingHour.Shift.Id))
                .ForMember(dest => dest.WorkingHourTitle,
                    opt => opt.MapFrom(src => src.WorkingHour.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ReplacedShiftId,
                    opt => opt.MapFrom(src => src.ReplacedWorkingHour.Shift.Id))
                .ForMember(dest => dest.ReplacedShiftTitle,
                    opt => opt.MapFrom(src => src.ReplacedWorkingHour.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ReplacedWorkingHourTitle,
                    opt => opt.MapFrom(src => src.ReplacedWorkingHour.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.RequestedDate,
                    opt => opt.MapFrom(src => src.RequestedDate.ToShortDateString()))
                .ForMember(dest => dest.ReplacementDate,
                    opt => opt.MapFrom(src => src.ReplacementDate.ToShortDateString()))
                .ForMember(dest => dest.ActionDate,
                    opt => opt.MapFrom(src => src.ActionDate.HasValue
                        ? src.ActionDate.Value.ToShortDateString() : ""))
                .ForMember(dest => dest.ReplacementDate,
                    opt => opt.MapFrom(src => src.ReplacementDate.ToShortDateString()))
                .ForMember(dest => dest.RequestActionTitle,
                    opt => opt.MapFrom(src => src.RequestAction == RequestAction.Accept
                        ? "confirmed" : src.RequestAction == RequestAction.PartialAccept
                        ? "waiting for confirmation" : src.RequestAction == RequestAction.Reject
                        ? "rejected" : "new"));

            CreateMap<PersonnelShiftReplacement, ReplaceShiftDto>()
                .ForMember(dest => dest.ShiftTitle,
                    opt => opt.MapFrom(src => "shift replacement: " + src.WorkingHour.Shift.Title.GetSafeHtmlFragment()
                        + " with " + src.ReplacedWorkingHour.Shift.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.WorkingHourTitle,
                    opt => opt.MapFrom(src => "replacement: " + src.WorkingHour.Title.GetSafeHtmlFragment()
                        + " with " + src.ReplacedWorkingHour.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.ReplacementDate));
            #endregion

            #region ApprovalProc
            CreateMap<ApprovalProc, ApprovalProcDtoForPaging>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ParentTitle,
                    opt => opt.MapFrom(src => src.ParentProc.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ActiveStateTitle
                    , opt => opt.MapFrom(src => src.ActiveState == ActiveState.Active
                        ? "active" : "inactive"));

            CreateMap<ApprovalProc, ApprovalProcDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ParentTitle,
                    opt => opt.MapFrom(src => src.ParentProc.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.FirstPriorityId,
                    opt => opt.MapFrom(src => src.FirstPriority.Id))
                .ForMember(dest => dest.FirstPriorityGroupCategoryId,
                    opt => opt.MapFrom(src => src.FirstPriority.GroupCategory.Id))
                .ForMember(dest => dest.FirstPriorityFullName,
                    opt => opt.MapFrom(src => src.FirstPriority.Name.GetSafeHtmlFragment()
                        + " " + src.FirstPriority.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SecondPriorityId,
                    opt => opt.MapFrom(src => src.SecondPriority.Id))
                .ForMember(dest => dest.SecondPriorityGroupCategoryId,
                    opt => opt.MapFrom(src => src.SecondPriority.GroupCategory.Id))
                .ForMember(dest => dest.SecondPriorityFullName,
                    opt => opt.MapFrom(src => src.SecondPriority.Name.GetSafeHtmlFragment()
                        + " " + src.SecondPriority.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ThirdPriorityId,
                    opt => opt.MapFrom(src => src.ThirdPriority.Id))
                .ForMember(dest => dest.ThirdPriorityGroupCategoryId,
                    opt => opt.MapFrom(src => src.ThirdPriority.GroupCategory.Id))
                .ForMember(dest => dest.ThirdPriorityFullName,
                    opt => opt.MapFrom(src => src.ThirdPriority.Name.GetSafeHtmlFragment()
                        + " " + src.ThirdPriority.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ActiveStateTitle
                    , opt => opt.MapFrom(src => src.ActiveState == ActiveState.Active
                        ? "active" : "inactive"));

            CreateMap<ApprovalProc, ApprovalProcDtoForProfile>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ParentTitle,
                    opt => opt.MapFrom(src => src.ParentProc.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.FirstPriorityFullName,
                    opt => opt.MapFrom(src => src.FirstPriority.Name.GetSafeHtmlFragment()
                        + " " + src.FirstPriority.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.SecondPriorityFullName,
                    opt => opt.MapFrom(src => src.SecondPriority.Name.GetSafeHtmlFragment()
                        + " " + src.SecondPriority.LastName.GetSafeHtmlFragment()))
                .ForMember(dest => dest.ThirdPriorityFullName,
                    opt => opt.MapFrom(src => src.ThirdPriority.Name.GetSafeHtmlFragment()
                        + " " + src.ThirdPriority.LastName.GetSafeHtmlFragment()));

            CreateMap<ApprovalProc, ApprovalProcDtoForDDL>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));

            CreateMap<ApprovalProc, ApprovalProcPatchDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()));
            #endregion

            #region CalendarDate
            CreateMap<CalendarDate, CalendarDateDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.GetSafeHtmlFragment()))
                .ForMember(dest => dest.DateType,
                    opt => opt.MapFrom(src => src.IsHoliday == true ? "holiday" : "working day"));
            #endregion

            #region PersonnelEntrance
            CreateMap<PersonnelEntrance, PersonnelEntranceDto>()
                .ForMember(dest => dest.PersonnelCode,
                    opt => opt.MapFrom(src => src.Personnel.Code.GetSafeHtmlFragment()))
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName)
                        .GetSafeHtmlFragment()))
                .ForMember(dest => dest.EnterDate,
                    opt => opt.MapFrom(src => src.EnterDate.ToShortDateString()))
                .ForMember(dest => dest.Enter,
                    opt => opt.MapFrom(src => src.Enter.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.AutoEnterTitle,
                    opt => opt.MapFrom(src => src.AutoEnter ? "auto entrance" : "personnel entrance"))
                .ForMember(dest => dest.ExitDate,
                    opt => opt.MapFrom(src => src.ExitDate.HasValue
                        ? src.ExitDate.Value.ToShortDateString() : null))
                .ForMember(dest => dest.Exit,
                    opt => opt.MapFrom(src => src.Exit.HasValue
                        ? src.Exit.Value.ToString("hh\\:mm\\:ss") : null))
                .ForMember(dest => dest.AutoExitTitle,
                    opt => opt.MapFrom(src => src.AutoExit ? "خروج خودکار" : "personnel entrance"))
                .ForMember(dest => dest.IsEditedTitle,
                    opt => opt.MapFrom(src => !src.IsEdited ? "without edit" : "edited"))
                .ForMember(dest => dest.EditDate,
                    opt => opt.MapFrom(src => src.EditDate.HasValue
                        ? src.EditDate.Value.ToShortDateString() : null));

            CreateMap<PersonnelEntrance, PersonnelEntranceSummaryDto>()
                .ForMember(dest => dest.EnterDate,
                    opt => opt.MapFrom(src => src.EnterDate.ToShortDateString()))
                .ForMember(dest => dest.Enter,
                    opt => opt.MapFrom(src => src.Enter.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.ExitDate,
                    opt => opt.MapFrom(src => src.ExitDate.HasValue
                        ? src.ExitDate.Value.ToShortDateString() : "-"))
                .ForMember(dest => dest.Exit,
                    opt => opt.MapFrom(src => src.Exit.HasValue
                        ? src.Exit.Value.ToString("hh\\:mm\\:ss") : "-"));
            #endregion

            #region PersonnelDismissalEntrance
            CreateMap<PersonnelDismissalEntrance, PersonnelDismissalEntranceDto>()
                .ForMember(dest => dest.PersonnelCode,
                    opt => opt.MapFrom(src => src.Personnel.Code.GetSafeHtmlFragment()))
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName)
                        .GetSafeHtmlFragment()))
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.StartDate.ToShortDateString()))
                .ForMember(dest => dest.Start,
                    opt => opt.MapFrom(src => src.Start.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.EndDate,
                    opt => opt.MapFrom(src => src.EndDate.HasValue
                        ? src.EndDate.Value.ToShortDateString() : null))
                .ForMember(dest => dest.End,
                    opt => opt.MapFrom(src => src.End.HasValue
                        ? src.End.Value.ToString("hh\\:mm\\:ss") : null));
            #endregion

            #region PersonnelDutyEntrance
            CreateMap<PersonnelDutyEntrance, PersonnelDutyEntranceDto>()
                .ForMember(dest => dest.PersonnelCode,
                    opt => opt.MapFrom(src => src.Personnel.Code.GetSafeHtmlFragment()))
                .ForMember(dest => dest.PersonnelFullName,
                    opt => opt.MapFrom(src => (src.Personnel.Name + " " + src.Personnel.LastName)
                        .GetSafeHtmlFragment()))
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.StartDate.ToShortDateString()))
                .ForMember(dest => dest.Start,
                    opt => opt.MapFrom(src => src.Start.ToString("hh\\:mm\\:ss")))
                .ForMember(dest => dest.EndDate,
                    opt => opt.MapFrom(src => src.EndDate.HasValue
                        ? src.EndDate.Value.ToShortDateString() : null))
                .ForMember(dest => dest.End,
                    opt => opt.MapFrom(src => src.End.HasValue
                        ? src.End.Value.ToString("hh\\:mm\\:ss") : null));
            #endregion

        }
    }
}
