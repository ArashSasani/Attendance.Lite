using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.WorkingHour
{
    public class UpdateWorkingHourDto
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public WorkingHourDuration WorkingHourDuration { get; set; }
        public Duration DailyDelay { get; set; }
        public Duration MonthlyDelay { get; set; }
        public Duration DailyRush { get; set; }
        public Duration MonthlyRush { get; set; }
        public Duration PriorExtraWorkTime { get; set; }
        public Duration LaterExtraWorkTime { get; set; }
        public Duration FloatingTime { get; set; }
        public TimeSpan? MealTimeBreakFromTime { get; set; }
        public TimeSpan? MealTimeBreakToTime { get; set; }
        //working hour breaks - lite version
        public TimeSpan? Break1FromTime { get; set; }
        public TimeSpan? Break1ToTime { get; set; }
        public TimeSpan? Break2FromTime { get; set; }
        public TimeSpan? Break2ToTime { get; set; }
        public TimeSpan? Break3FromTime { get; set; }
        public TimeSpan? Break3ToTime { get; set; }
    }

    public class UpdateWorkingHourDtoValidator : AbstractValidator<UpdateWorkingHourDto>
    {
        public UpdateWorkingHourDtoValidator()
        {
            RuleFor(x => x.MealTimeBreakToTime).GreaterThan(x => x.MealTimeBreakFromTime)
                .When(x => x.MealTimeBreakFromTime.HasValue && x.MealTimeBreakToTime.HasValue)
                .WithMessage("end time cannot be less that start time");

            RuleFor(x => x.Break1ToTime).GreaterThan(x => x.Break1FromTime)
                .When(x => x.Break1ToTime.HasValue && x.Break1FromTime.HasValue)
                .WithMessage("end time cannot be less that start time");

            RuleFor(x => x.Break2ToTime).GreaterThan(x => x.Break2FromTime)
                .When(x => x.Break2ToTime.HasValue && x.Break2FromTime.HasValue)
                .WithMessage("end time cannot be less that start time");

            RuleFor(x => x.Break3ToTime).GreaterThan(x => x.Break3FromTime)
                .When(x => x.Break3ToTime.HasValue && x.Break3FromTime.HasValue)
                .WithMessage("end time cannot be less that start time");
        }
    }
}
