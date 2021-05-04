using FluentValidation;
using System;

namespace AttendanceManagement.Service.Dtos.PersonnelDutyEntrance
{
    public class UpdatePersonnelDutyEntranceDto
    {
        public int Id { get; set; }
        public TimeSpan Start { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? End { get; set; }
    }

    public class UpdatePersonnelDutyEntranceDtoValidator
        : AbstractValidator<UpdatePersonnelDutyEntranceDto>
    {
        public UpdatePersonnelDutyEntranceDtoValidator()
        {
            RuleFor(x => x.End.Value).GreaterThanOrEqualTo(x => x.Start)
                .When(x => x.End.HasValue)
                .WithMessage("end time cannot be less than start time");
        }
    }
}
