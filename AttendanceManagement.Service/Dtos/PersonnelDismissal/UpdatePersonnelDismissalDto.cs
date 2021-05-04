using FluentValidation;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissal
{
    public class UpdatePersonnelDismissalDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DismissalId { get; set; }
        public RequestDuration DismissalDuration { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }

        public DailyDismissal DailyDismissal { get; set; }
        public HourlyDismissal HourlyDismissal { get; set; }
    }

    public class UpdatePersonnelDismissalDtoValidator : AbstractValidator<UpdatePersonnelDismissalDto>
    {
        public UpdatePersonnelDismissalDtoValidator()
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
