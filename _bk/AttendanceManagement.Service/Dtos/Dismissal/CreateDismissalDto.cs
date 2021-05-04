using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.Dismissal
{
    public class CreateDismissalDto
    {
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

    public class Demanded
    {
        public Duration AllowanceInMonth { get; set; }
        public int CountInMonth { get; set; }
        public bool IsTransferableToNextMonth { get; set; }
        public Duration TransferableAllowanceToNextMonth { get; set; }
        public Duration AllowanceInYear { get; set; }
        public int CountInYear { get; set; }
        public bool IsTransferableToNextYear { get; set; }
        public Duration TransferableAllowanceToNextYear { get; set; }
        public bool IsAllowedToSave { get; set; }
        public Duration AllowanceToSave { get; set; }
        public bool MealTimeIsIncluded { get; set; }
        public bool DoesDismissalMeansExtraWork { get; set; }
        public bool IsNationalHolidysConsideredInDismissal { get; set; }
        public bool IsFridaysConsideredInDismissal { get; set; }
        public int AmountOfHoursConsideredDailyDismissal { get; set; }
    }

    public class Sickness
    {
        public Duration AllowanceInYear { get; set; }
        public int CountInYear { get; set; }
        public bool IsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }

    public class WithoutSalary
    {
        public Duration AllowanceInMonth { get; set; }
        public int CountInMonth { get; set; }
    }

    public class Encouragement
    {
        [Required(ErrorMessage = "*")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime ToDate { get; set; }
        public bool ConsiderWithoutSalary { get; set; }
    }

    public class Marriage
    {
        public Duration AllowanceInTotal { get; set; }
        public int CountInTotal { get; set; }
        public bool ConsiderWithoutSalary { get; set; }
        public bool IsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }

    public class ChildBirth
    {
        public Duration AllowanceInTotal { get; set; }
        public bool ConsiderWithoutSalary { get; set; }
        public bool IsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }

    public class BreastFeeding
    {
        public Duration AllowanceInTotal { get; set; }
        public Duration AllowanceInDay { get; set; }
        public int CountInDay { get; set; }
        public bool IsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }

    public class DeathOfRelatives
    {
        public Duration AllowanceInTotal { get; set; }
        public bool IsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }

    public class CreateDismissalDtoValidator : AbstractValidator<CreateDismissalDto>
    {
        public CreateDismissalDtoValidator()
        {
            RuleFor(x => x.Encouragement.ToDate)
                .GreaterThanOrEqualTo(x => x.Encouragement.FromDate)
                .When(x => x.Encouragement != null)
                .WithMessage("end date cannot be less that start end");
        }
    }
}
