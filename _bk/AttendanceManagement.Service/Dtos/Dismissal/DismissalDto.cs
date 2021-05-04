using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.Dismissal
{
    public class DismissalDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DismissalType DismissalType { get; set; }
        public string DismissalTypeTitle { get; set; }
        public DismissalSystemType DismissalSystemType { get; set; }
        public string DismissalSystemTypeTitle { get; set; }
        public DismissalExcessiveReaction DismissalExcessiveReaction { get; set; }
        public string DismissalExcessiveReactionTitle { get; set; }
        public int? ActionLimitDays { get; set; }
        public string ActionLimitDaysTitle { get; set; }

        #region Demanded
        public Duration DemandedAllowanceInMonth { get; set; }
        public string DemandedAllowanceInMonthTitle
        {
            get
            {
                return DemandedAllowanceInMonth.GetDurationFormatted();
            }
        }
        public int DemandedCountInMonth { get; set; }
        public bool DemandedIsTransferableToNextMonth { get; set; }
        public string DemandedIsTransferableToNextMonthTitle { get; set; }
        public Duration DemandedTransferableAllowanceToNextMonth { get; set; }
        public string DemandedTransferableAllowanceToNextMonthTitle
        {
            get
            {
                return DemandedTransferableAllowanceToNextMonth.GetDurationFormatted();
            }
        }
        public Duration DemandedAllowanceInYear { get; set; }
        public string DemandedAllowanceInYearTitle
        {
            get
            {
                return DemandedAllowanceInYear.GetDurationFormatted();
            }
        }
        public int DemandedCountInYear { get; set; }
        public bool DemandedIsTransferableToNextYear { get; set; }
        public string DemandedIsTransferableToNextYearTitle { get; set; }
        public Duration DemandedTransferableAllowanceToNextYear { get; set; }
        public string DemandedTransferableAllowanceToNextYearTitle
        {
            get
            {
                return DemandedTransferableAllowanceToNextYear.GetDurationFormatted();
            }
        }
        public bool DemandedIsAllowedToSave { get; set; }
        public string DemandedIsAllowedToSaveTitle { get; set; }
        public Duration DemandedAllowanceToSave { get; set; }
        public string DemandedAllowanceToSaveTitle
        {
            get
            {
                return DemandedAllowanceToSave.GetDurationFormatted();
            }
        }
        public bool DemandedMealTimeIsIncluded { get; set; }
        public string DemandedMealTimeIsIncludedTitle { get; set; }
        public bool DemandedDoesDismissalMeansExtraWork { get; set; }
        public string DemandedDoesDismissalMeansExtraWorkTitle { get; set; }
        public bool DemandedIsNationalHolidysConsideredInDismissal { get; set; }
        public string DemandedIsNationalHolidysConsideredInDismissalTitle { get; set; }
        public bool DemandedIsFridaysConsideredInDismissal { get; set; }
        public string DemandedIsFridaysConsideredInDismissalTitle { get; set; }
        public int DemandedAmountOfHoursConsideredDailyDismissal { get; set; }
        #endregion

        #region Sickness
        public Duration SicknessAllowanceInYear { get; set; }
        public string SicknessAllowanceInYearTitle
        {
            get
            {
                return SicknessAllowanceInYear.GetDurationFormatted();
            }
        }
        public int SicknessCountInYear { get; set; }
        public bool SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
        public string SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle { get; set; }
        #endregion

        #region WithoutSalary
        public Duration WithoutSalaryAllowanceInMonth { get; set; }
        public string WithoutSalaryAllowanceInMonthTitle
        {
            get
            {
                return WithoutSalaryAllowanceInMonth.GetDurationFormatted();
            }
        }
        public int WithoutSalaryCountInMonth { get; set; }
        #endregion

        #region Encouragement
        public string EncouragementFromDate { get; set; }
        public string EncouragementToDate { get; set; }
        public bool EncouragementConsiderWithoutSalary { get; set; }
        public string EncouragementConsiderWithoutSalaryTitle { get; set; }
        #endregion

        #region Marriage
        public Duration MarriageAllowanceInTotal { get; set; }
        public string MarriageAllowanceInTotalTitle
        {
            get
            {
                return MarriageAllowanceInTotal.GetDurationFormatted();
            }
        }
        public int MarriageCountInTotal { get; set; }
        public bool MarriageConsiderWithoutSalary { get; set; }
        public string MarriageConsiderWithoutSalaryTitle { get; set; }
        public bool MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
        public string MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle { get; set; }
        #endregion

        #region ChildBirth
        public Duration ChildBirthAllowanceInTotal { get; set; }
        public string ChildBirthAllowanceInTotalTitle
        {
            get
            {

                return ChildBirthAllowanceInTotal.GetDurationFormatted();
            }
        }
        public bool ChildBirthConsiderWithoutSalary { get; set; }
        public string ChildBirthConsiderWithoutSalaryTitle { get; set; }
        public bool ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
        public string ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle { get; set; }
        #endregion

        #region BreastFeeding
        public Duration BreastFeedingAllowanceInTotal { get; set; }
        public string BreastFeedingAllowanceInTotalTitle
        {
            get
            {
                return BreastFeedingAllowanceInTotal.GetDurationFormatted();
            }
        }
        public Duration BreastFeedingAllowanceInDay { get; set; }
        public string BreastFeedingAllowanceInDayTitle
        {
            get
            {
                return BreastFeedingAllowanceInDay.GetDurationFormatted();
            }
        }
        public int BreastFeedingCountInDay { get; set; }
        public bool BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
        public string BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle { get; set; }
        #endregion

        #region DeathOfRelatives
        public Duration DeathOfRelativesAllowanceInTotal { get; set; }
        public string DeathOfRelativesAllowanceInTotalTitle
        {
            get
            {

                return DeathOfRelativesAllowanceInTotal.GetDurationFormatted();
            }
        }
        public bool DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
        public string DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimitTitle { get; set; }
        #endregion
    }

    public class DismissalDtoDDL
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class DismissalAllowancesDto
    {
        public int? AllowanceInTotal { get; set; }
        public int? CountInTotal { get; set; }
        public int? AllowanceInYear { get; set; }
        public int? CountInYear { get; set; }
        public int? AllowanceInMonth { get; set; }
        public int? CountInMonth { get; set; }
        public int? AllowanceInDay { get; set; }
        public int? CountInDay { get; set; }
    }

    public class DismissalChartDto
    {
        public string LimitTitleForDay { get; set; }
        public Duration LimitValueForDay { get; set; }
        public string LimitValueForDayTitle
        {
            get
            {
                return LimitValueForDay.GetDurationFormatted();
            }
        }
        public string CountTitleForDay { get; set; }
        public string CountValueForDay { get; set; }
        public string LimitTitleForMonth { get; set; }
        public Duration LimitValueForMonth { get; set; }
        public string LimitValueForMonthTitle
        {
            get
            {
                return LimitValueForMonth.GetDurationFormatted();
            }
        }
        public string CountTitleForMonth { get; set; }
        public string CountValueForMonth { get; set; }
        public string LimitTitleForYear { get; set; }
        public Duration LimitValueForYear { get; set; }
        public string LimitValueForYearTitle
        {
            get
            {
                return LimitValueForYear.GetDurationFormatted();
            }
        }
        public string CountTitleForYear { get; set; }
        public string CountValueForYear { get; set; }
        public string LimitTitleForTotal { get; set; }
        public Duration LimitValueForTotal { get; set; }
        public string LimitValueForTotalTitle
        {
            get
            {
                return LimitValueForTotal.GetDurationFormatted();
            }
        }
        public string CountTitleForTotal { get; set; }
        public string CountValueForTotal { get; set; }
    }
}
