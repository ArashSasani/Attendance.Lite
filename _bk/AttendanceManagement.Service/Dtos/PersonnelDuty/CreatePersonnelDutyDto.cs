using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDuty
{
    public class CreatePersonnelDutyDto
    {
        public int DutyId { get; set; }
        public RequestDuration DutyDuration { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }

        public DailyDuty DailyDuty { get; set; }
        public HourlyDuty HourlyDuty { get; set; }
    }

    public class DailyDuty
    {
        [Required(ErrorMessage = "*")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime ToDate { get; set; }
    }

    public class HourlyDuty
    {
        [Required(ErrorMessage = "*")]
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    public class CreatePersonnelDutyDtoValidator : AbstractValidator<CreatePersonnelDutyDto>
    {
        public CreatePersonnelDutyDtoValidator()
        {
            RuleFor(x => x.DailyDuty.ToDate)
                .GreaterThan(x => x.DailyDuty.FromDate)
                .When(x => x.DailyDuty != null)
                .WithMessage("end date cannot be less than start date");

            RuleFor(x => x.HourlyDuty.ToTime)
                .GreaterThan(x => x.HourlyDuty.FromTime)
                .When(x => x.HourlyDuty != null)
                .WithMessage("end time cannot be less than start time");
        }
    }
}
