using FluentValidation;
using System;

namespace AttendanceManagement.Service.Dtos.PersonnelEntrance
{
    public class UpdatePersonnelEntranceDto
    {
        public int Id { get; set; }
        public TimeSpan Enter { get; set; }
        public DateTime? ExitDate { get; set; }
        public TimeSpan? Exit { get; set; }
    }

    public class UpdatePersonnelEntranceDtoValidator : AbstractValidator<UpdatePersonnelEntranceDto>
    {
        public UpdatePersonnelEntranceDtoValidator()
        {
            RuleFor(x => x.Exit.Value).GreaterThanOrEqualTo(x => x.Enter)
                .When(x => x.Exit.HasValue)
                .WithMessage("end time cannot be less than start time");
        }
    }
}
