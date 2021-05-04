using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.WorkingHour
{
    public class WorkingHourDto
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public string ShiftTitle { get; set; }
        public string Title { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public WorkingHourDuration WorkingHourDuration { get; set; }
        public string WorkingHourDurationTitle { get; set; }
        public Duration DailyDelay { get; set; }
        public string DailyDelayTitle
        {
            get
            {
                return DailyDelay.GetDurationFormatted();
            }
        }
        public Duration MonthlyDelay { get; set; }
        public string MonthlyDelayTitle
        {
            get
            {
                return MonthlyDelay.GetDurationFormatted();
            }
        }
        public Duration DailyRush { get; set; }
        public string DailyRushTitle
        {
            get
            {
                return DailyRush.GetDurationFormatted();
            }
        }
        public Duration MonthlyRush { get; set; }
        public string MonthlyRushTitle
        {
            get
            {
                return MonthlyRush.GetDurationFormatted();
            }
        }
        public Duration PriorExtraWorkTime { get; set; }
        public string PriorExtraWorkTimeTitle
        {
            get
            {
                return PriorExtraWorkTime.GetDurationFormatted();
            }
        }
        public Duration LaterExtraWorkTime { get; set; }
        public string LaterExtraWorkTimeTitle
        {
            get
            {
                return LaterExtraWorkTime.GetDurationFormatted();
            }
        }
        public Duration FloatingTime { get; set; }
        public string FloatingTimeTitle
        {
            get
            {
                return FloatingTime.GetDurationFormatted();
            }
        }
        public string MealTimeBreakFromTime { get; set; }
        public string MealTimeBreakToTime { get; set; }
        //break1
        public string Break1FromTime { get; set; }
        public string Break1ToTime { get; set; }
        //break2
        public string Break2FromTime { get; set; }
        public string Break2ToTime { get; set; }
        //break3
        public string Break3FromTime { get; set; }
        public string Break3ToTime { get; set; }
    }

    public class WorkingHourDDLDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
