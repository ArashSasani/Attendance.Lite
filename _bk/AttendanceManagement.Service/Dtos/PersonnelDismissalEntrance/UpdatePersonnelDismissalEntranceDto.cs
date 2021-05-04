using FluentValidation;
using System;

namespace AttendanceManagement.Service.Dtos.PersonnelDismissalEntrance
{
    public class UpdatePersonnelDismissalEntranceDto
    {
        public int Id { get; set; }
        public TimeSpan Start { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? End { get; set; }
    }

    public class UpdatePersonnelDismissalEntranceDtoValidator
        : AbstractValidator<UpdatePersonnelDismissalEntranceDto>
    {
        public UpdatePersonnelDismissalEntranceDtoValidator()
        {
            RuleFor(x => x.End.Value).GreaterThanOrEqualTo(x => x.Start)
                .When(x => x.End.HasValue)
                .WithMessage("end date cannot be less that start date");
        }
    }
}
