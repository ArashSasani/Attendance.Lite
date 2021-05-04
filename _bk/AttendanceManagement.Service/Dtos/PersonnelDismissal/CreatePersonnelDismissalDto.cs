using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissal
{
    public class CreatePersonnelDismissalDto
    {
        public int DismissalId { get; set; }
        public RequestDuration DismissalDuration { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }

        public DailyDismissal DailyDismissal { get; set; }
        public HourlyDismissal HourlyDismissal { get; set; }
    }

    public class DailyDismissal
    {
        [Required(ErrorMessage = "*")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime ToDate { get; set; }
    }

    public class HourlyDismissal
    {
        [Required(ErrorMessage = "*")]
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    public class CreatePersonnelDismissalDtoValidator : AbstractValidator<CreatePersonnelDismissalDto>
    {
        public CreatePersonnelDismissalDtoValidator()
        {
            RuleFor(x => x.DailyDismissal.ToDate)
                .GreaterThan(x => x.DailyDismissal.FromDate)
                .When(x => x.DailyDismissal != null)
                .WithMessage("end date cannot be less than start date");

            RuleFor(x => x.HourlyDismissal.ToTime)
                .GreaterThan(x => x.HourlyDismissal.FromTime)
                .When(x => x.HourlyDismissal != null)
                .WithMessage("end time cannot be less than start time");
        }
    }
}
