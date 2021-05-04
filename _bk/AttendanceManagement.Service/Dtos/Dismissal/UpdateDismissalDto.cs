using FluentValidation;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.Dismissal
{
    public class UpdateDismissalDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public DismissalType DismissalType { get; set; }
        public DismissalSystemType DismissalSystemType { get; set; }
        public DismissalExcessiveReaction DismissalExcessiveReaction { get; set; }
        public int? ActionLimitDays { get; set; }

        public Demanded Demanded { get; set; }
        public Sickness Sickness { get; set; }
        public WithoutSalary WithoutSalary { get; set; }
        public Encouragement Encouragement { get; set; }
        public Marriage Marriage { get; set; }
        public ChildBirth ChildBirth { get; set; }
        public BreastFeeding BreastFeeding { get; set; }
        public DeathOfRelatives DeathOfRelatives { get; set; }
    }

    public class UpdateDismissalDtoValidator : AbstractValidator<UpdateDismissalDto>
    {
        public UpdateDismissalDtoValidator()
        {
            RuleFor(x => x.Encouragement.ToDate)
                .GreaterThanOrEqualTo(x => x.Encouragement.FromDate)
                .When(x => x.Encouragement != null)
                .WithMessage("end date cannot be less that start end");
        }
    }
}
