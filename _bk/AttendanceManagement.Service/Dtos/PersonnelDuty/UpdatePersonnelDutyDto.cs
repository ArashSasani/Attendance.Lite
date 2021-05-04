using FluentValidation;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelDuty
{
    public class UpdatePersonnelDutyDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DutyId { get; set; }
        public RequestDuration DutyDuration { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }

        public DailyDuty DailyDuty { get; set; }
        public HourlyDuty HourlyDuty { get; set; }
    }

    public class UpdatePersonnelDutyDtoValidator : AbstractValidator<UpdatePersonnelDutyDto>
    {
        public UpdatePersonnelDutyDtoValidator()
        {
            RuleFor(x => x.DailyDuty.ToDate)
                .GreaterThan(x => x.DailyDuty.FromDate)
                .When(x => x.DailyDuty != null)
                .WithMessage("end time cannot be less than start time");

            RuleFor(x => x.HourlyDuty.ToTime)
                .GreaterThan(x => x.HourlyDuty.FromTime)
                .When(x => x.HourlyDuty != null)
                .WithMessage("end time cannot be less than start time");
        }
    }
}
