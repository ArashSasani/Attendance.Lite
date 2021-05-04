using AttendanceManagement.Core.Model;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IReportCalcService
    {
        bool EntranceHasDelay(PersonnelEntrance entrance, WorkingHour workingHour);
        double GetRushDuration(PersonnelEntrance entrance, WorkingHour workingHour);
        double GetDelayDuration(PersonnelEntrance entrance, WorkingHour workingHour);
        OperationType GetOperationType(PersonnelEntrance entrance);
        bool IsAbsent(PersonnelEntrance entrance);
        double GetOperationTime(PersonnelEntrance entrance, WorkingHour workingHour
            , bool forceCalculate = false);
        string GetOperationTimeFormatted(PersonnelEntrance entrance, WorkingHour workingHour);
        double GetTotalAbsenceDuration(PersonnelEntrance entrance, WorkingHour workingHour
            , bool forceDutyOrDismissalDay = false);
        string GetAbsenceTitle(PersonnelEntrance entrance, WorkingHour workingHour);
        string GetTotalExtraWorkTitle(double priorExtraWorkDuration
            , double laterExtraWorkDuration, WorkingHour workingHour);
        double GetLaterExtraWorkDuration(PersonnelEntrance entrance, WorkingHour workingHour);
        double GetPriorExtraWorkDuration(PersonnelEntrance entrance, WorkingHour workingHour);
        double GetTotalExtraWorkDuration(PersonnelEntrance entrance, WorkingHour workingHour);
        double GetDismissalDuration(PersonnelDismissal personDismissal);
        double GetDutyDuration(PersonnelDuty personDuty);
        PersonnelDuty GetEntranceDateDuty(PersonnelEntrance entrance);
        PersonnelDismissal GetEntranceDateDismissal(PersonnelEntrance entrance);
        string SecondsToTimeFormatted(double totalSeconds, bool useTotalHours = true);
    }
}
