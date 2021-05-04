using System;
using System.Collections.Generic;
using System.Data.Entity;
using WebApplication.Infrastructure.Localization;
using WebApplication.SharedDatabase.Model;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.DataModel
{
    public class TestInitializerForUberContext : DropCreateDatabaseAlways<UberContext>
    {
        protected override void Seed(UberContext context)
        {
            #region Dismissals
            var defaultDemanded = new DemandedDismissal
            {
                Title = "Demanded",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Demanded,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                DemandedAllowanceInMonth = (2).GetDaysInSeconds(),
                DemandedCountInMonth = 2,
                DemandedAllowanceInYear = (24).GetDaysInSeconds(),
                DemandedCountInYear = 24,
                DemandedMealTimeIsIncluded = true,
                DemandedIsNationalHolidysConsideredInDismissal = true,
                DemandedIsFridaysConsideredInDismissal = true,
                DemandedAmountOfHoursConsideredDailyDismissal = 6
            };
            context.DemandedDismissals.Add(defaultDemanded);

            var defaultSickness = new SicknessDismissal
            {
                Title = "Sickness",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Sickness,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                SicknessAllowanceInYear = (12).GetDaysInSeconds(),
                SicknessCountInYear = 12,
                SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            context.SicknessDismissals.Add(defaultSickness);

            var defaultWithoutSalary = new WithoutSalaryDismissal
            {
                Title = "Without Salary",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.WithoutSalary,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                WithoutSalaryAllowanceInMonth = (5).GetDaysInSeconds(),
                WithoutSalaryCountInMonth = 2
            };
            context.WithoutSalaryDismissals.Add(defaultWithoutSalary);

            var defaultEncouragement = new EncouragementDismissal
            {
                Title = "Encouragement",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Encouragement,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                EncouragementFromDate = DateTime.Now.AddDays(-2).Date,
                EncouragementToDate = DateTime.Now.Date,
                EncouragementConsiderWithoutSalary = true
            };
            context.EncouragementDismissals.Add(defaultEncouragement);

            var defaultMarriage = new MarriageDismissal
            {
                Title = "Marriage",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Marriage,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                MarriageAllowanceInTotal = (20).GetDaysInSeconds(),
                MarriageCountInTotal = 1,
                MarriageConsiderWithoutSalary = true,
                MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            context.MarriageDismissals.Add(defaultMarriage);

            var defaultChildBirth = new ChildBirthDismissal
            {
                Title = "Child Birth",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.ChildBirth,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                ChildBirthAllowanceInTotal = (180).GetDaysInSeconds(),
                ChildBirthConsiderWithoutSalary = false,
                ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            context.ChildBirthDismissals.Add(defaultChildBirth);

            var defaultBreastFeeding = new BreastFeedingDismissal
            {
                Title = "Breast Feeding",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.BreastFeeding,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                BreastFeedingAllowanceInTotal = (20).GetDaysInSeconds(),
                BreastFeedingAllowanceInDay = (3).GetHoursInSeconds(),
                BreastFeedingCountInDay = 2,
                BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            context.BreastFeedingDismissals.Add(defaultBreastFeeding);

            var defaultDeathOfRelatives = new DeathOfRelativesDismissal
            {
                Title = "Death of Relatives",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.DeathOfRelatives,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                DeathOfRelativesAllowanceInTotal = (20).GetDaysInSeconds(),
                DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            context.DeathOfRelativesDismissals.Add(defaultDeathOfRelatives);
            #endregion

            #region Duties
            var duties = new List<Duty>
            {
                new Duty
                {
                    Title = "Domestic"
                },
                new Duty
                {
                    Title = "International"
                }
            };
            context.Duties.AddRange(duties);
            #endregion

            context.SaveChanges();

            #region personnel
            var workUnits = new List<WorkUnit>
            {
                new WorkUnit
                {
                    Title = "Sales department"
                },
                new WorkUnit
                {
                    Title = "Engineering department"
                }
            };
            context.WorkUnits.AddRange(workUnits);

            context.SaveChanges();

            var groupCategories = new List<GroupCategory>
            {
                new GroupCategory
                {
                    Title = "Sales"
                },
                new GroupCategory
                {
                    Title = "Managers"
                }
            };
            context.GroupCategories.AddRange(groupCategories);
            var employeemntTypes = new List<EmployeementType>
            {
                new EmployeementType
                {
                    Title = "Sales"
                },
                new EmployeementType
                {
                    Title = "Engineering"
                }
            };
            context.EmployeementTypes.AddRange(employeemntTypes);
            var positions = new List<Position>
            {
                new Position
                {
                    WorkUnitId = 1,
                    Title = "CFO"
                },
                new Position
                {
                    WorkUnitId = 1,
                    Title = "Sales VP"
                },
                new Position
                {
                    WorkUnitId = 2,
                    Title = "CEO"
                },
                new Position
                {
                    WorkUnitId = 2,
                    Title = "Engineer"
                }
            };
            context.Positions.AddRange(positions);

            context.SaveChanges();

            var personnel1 = new Personnel
            {
                Code = "1",
                Name = "Arash",
                LastName = "Sasani",
                FathersName = "Arash",
                NationalCode = "1234567890",
                BirthCertificateCode = "123123123",
                PlaceOfBirth = "Tehran",
                State = "Tehran",
                City = "Tehran",
                PostalCode = "123123123",
                BirthDate = DateTime.Now.AddYears(-29),
                Email = "arash@yahoo.com",
                Mobile = "09128027821",
                Phone = "44223344",
                Address = "Some place",
                Education = Education.Bachelor,
                MilitaryServiceStatus = MilitaryServiceStatus.Completed,
                Gender = Gender.Male,
                MaritalStatus = MaritalStatus.Single,
                GroupCategoryId = 1,
                EmployeementTypeId = 1,
                PositionId = 1,
                InsuranceRecordDuration = "2 months",
                NoneInsuranceRecordDuration = "2 years",
                BankAccountNumber = "12312a123c",
                DateOfEmployeement = DateTime.Now.AddYears(-2),
                FirstDateOfWork = DateTime.Now.AddYears(-2).AddDays(2),
                ActiveState = ActiveState.Active,
                IsPresent = true
            };
            var personnel2 = new Personnel
            {
                Code = "2",
                Name = "John",
                LastName = "Doe",
                FathersName = "John",
                NationalCode = "1212567820",
                BirthCertificateCode = "1231234333",
                PlaceOfBirth = "LA",
                State = "LA",
                City = "LA",
                PostalCode = "125623123",
                BirthDate = DateTime.Now.AddYears(-39),
                Email = "john@doe.com",
                Mobile = "09123457821",
                Phone = "4422334544",
                Address = "Some place",
                Education = Education.Phd,
                MilitaryServiceStatus = null,
                Gender = Gender.Female,
                MaritalStatus = MaritalStatus.Married,
                GroupCategoryId = 2,
                EmployeementTypeId = 2,
                PositionId = 2,
                InsuranceRecordDuration = "2 months",
                NoneInsuranceRecordDuration = "2 years",
                BankAccountNumber = "12312a123c",
                DateOfEmployeement = DateTime.Now.AddYears(-3),
                FirstDateOfWork = DateTime.Now.AddYears(-3).AddDays(2),
                ActiveState = ActiveState.Active,
                IsPresent = true
            };
            context.Personnel.Add(personnel1);
            context.Personnel.Add(personnel2);
            #endregion

            context.SaveChanges();

            #region Duty Approvals
            var dutiesApprovals = new List<DutyApproval>
            {
                new DutyApproval
                {
                    PersonnelId = 1,
                    DutyId = 1,
                },
                new DutyApproval
                {
                    PersonnelId = 1,
                    DutyId = 2
                }
            };
            context.DutiesApprovals.AddRange(dutiesApprovals);
            #endregion

            #region Personnel Dismissals
            var dailyPersonnelDismissal = new PersonnelDailyDismissal
            {
                PersonnelId = 1,
                DismissalId = 1,
                SubmittedDate = DateTime.Now.AddDays(-2),
                DismissalDuration = RequestDuration.Daily,
                ActionDate = DateTime.Now,
                RequestAction = RequestAction.Accept,
                FromDate = DateTime.Now.AddDays(-1).Date,
                ToDate = DateTime.Now.Date
            };
            context.PersonnelDailyDismissals.Add(dailyPersonnelDismissal);
            var hourlyPersonnelDismissal = new PersonnelHourlyDismissal
            {
                PersonnelId = 1,
                DismissalId = 1,
                SubmittedDate = DateTime.Now.AddDays(-1),
                DismissalDuration = RequestDuration.Hourly,
                ActionDate = DateTime.Now,
                RequestAction = RequestAction.Reject,
                Date = DateTime.Now.Date,
                FromTime = TimeSpan.Parse("12:00:00"),
                ToTime = TimeSpan.Parse("14:00:00")
            };
            context.PersonnelHourlyDismissals.Add(hourlyPersonnelDismissal);
            #endregion

            #region Personnel Duties
            var dailyPersonnelDuty = new PersonnelDailyDuty
            {
                PersonnelId = 1,
                DutyId = 1,
                SubmittedDate = DateTime.Now.AddDays(-2),
                DutyDuration = RequestDuration.Daily,
                ActionDate = DateTime.Now,
                RequestAction = RequestAction.Accept,
                FromDate = DateTime.Now.AddDays(-1).Date,
                ToDate = DateTime.Now.Date
            };
            context.PersonnelDailyDuties.Add(dailyPersonnelDuty);
            var hourlyPersonnelDuty = new PersonnelHourlyDuty
            {
                PersonnelId = 1,
                DutyId = 1,
                SubmittedDate = DateTime.Now.AddDays(-1),
                DutyDuration = RequestDuration.Hourly,
                ActionDate = DateTime.Now,
                RequestAction = RequestAction.Reject,
                Date = DateTime.Now.Date,
                FromTime = TimeSpan.Parse("12:00:00"),
                ToTime = TimeSpan.Parse("14:00:00")
            };
            context.PersonnelHourlyDuties.Add(hourlyPersonnelDuty);
            #endregion

            #region Shifts and Working Hours
            var shifts = new List<Shift>
            {
                new Shift
                {
                    Title = "Employment"
                },
                new Shift
                {
                    Title = "Management"
                }
            };
            context.Shifts.AddRange(shifts);
            context.SaveChanges();

            var workingHours = new List<WorkingHour>
            {
                new WorkingHour
                {
                    ShiftId = 1,
                    Title = "Working hour 1",
                    FromTime = TimeSpan.Parse("8:00"),
                    ToTime = TimeSpan.Parse("17:00"),
                    WorkingHourDuration = WorkingHourDuration.OneDay,
                    DailyDelay = (0).GetHoursInSeconds(),
                    MonthlyDelay = (0).GetHoursInSeconds(),
                    DailyRush = (0).GetHoursInSeconds(),
                    MonthlyRush = (0).GetHoursInSeconds(),
                    PriorExtraWorkTime = (1).GetHoursInSeconds(),
                    LaterExtraWorkTime = (1).GetHoursInSeconds(),
                    FloatingTime = (0).GetHoursInSeconds(),
                    MealTimeBreakFromTime = null,
                    MealTimeBreakToTime = null
                },
                new WorkingHour
                {
                    ShiftId = 2,
                    Title = "Working hour 1",
                    FromTime = TimeSpan.Parse("12:00"),
                    ToTime = TimeSpan.Parse("14:00"),
                    WorkingHourDuration = WorkingHourDuration.OneDay,
                    DailyDelay = (2).GetHoursInSeconds(),
                    MonthlyDelay = (12).GetHoursInSeconds(),
                    DailyRush = (2).GetHoursInSeconds(),
                    MonthlyRush = (12).GetHoursInSeconds(),
                    PriorExtraWorkTime = (2).GetHoursInSeconds(),
                    LaterExtraWorkTime = (1).GetHoursInSeconds(),
                    FloatingTime = (6).GetHoursInSeconds(),
                    MealTimeBreakFromTime = TimeSpan.Parse("12:00:00"),
                    MealTimeBreakToTime = TimeSpan.Parse("13:00:00")
                },
                new WorkingHour
                {
                    ShiftId = 2,
                    Title = "Working hour 2",
                    FromTime = TimeSpan.Parse("18:00"),
                    ToTime = TimeSpan.Parse("22:00"),
                    WorkingHourDuration = WorkingHourDuration.OneDay,
                    DailyDelay = (2).GetHoursInSeconds(),
                    MonthlyDelay = (12).GetHoursInSeconds(),
                    DailyRush = (2).GetHoursInSeconds(),
                    MonthlyRush = (12).GetHoursInSeconds(),
                    PriorExtraWorkTime = (2).GetHoursInSeconds(),
                    LaterExtraWorkTime = (1).GetHoursInSeconds(),
                    FloatingTime = (6).GetHoursInSeconds(),
                    MealTimeBreakFromTime = TimeSpan.Parse("19:00:00"),
                    MealTimeBreakToTime = TimeSpan.Parse("20:00:00")
                }
            };
            context.WorkingHours.AddRange(workingHours);
            context.SaveChanges();

            #endregion

            context.SaveChanges();

            #region Personnel Shift
            var personnelShifts = new List<PersonnelShift>
            {
                new PersonnelShift
                {
                    PersonnelId = 1,
                    ShiftId = 1,
                    DateAssigned = DateTime.Now
                },
                new PersonnelShift
                {
                    PersonnelId = 2,
                    ShiftId = 1,
                    DateAssigned = DateTime.Now.AddDays(1)
                }
            };
            context.PersonnelShifts.AddRange(personnelShifts);
            #endregion

            context.SaveChanges();

            #region Personnel Shift Assignments
            var shiftAssignDates = new List<PersonnelShiftAssignment>
            {
                new PersonnelShiftAssignment
                {
                    PersonnelShiftId = 1,
                    Date = DateTime.Now.Date
                },
                new PersonnelShiftAssignment
                {
                    PersonnelShiftId = 2,
                    Date = DateTime.Now.Date.AddDays(1)
                }
            };
            context.PersonnelShiftAssignments.AddRange(shiftAssignDates);
            #endregion

            #region Personnel Shift Replacements
            var shiftReplacements = new List<PersonnelShiftReplacement>
            {
                new PersonnelShiftReplacement
                {
                    PersonnelId = 1,
                    ReplacedPersonnelId = 2,
                    WorkingHourId = 1,
                    ReplacedWorkingHourId = 1,
                    RequestedDate = DateTime.Now.AddDays(-2),
                    ActionDate = DateTime.Now.AddDays(-1),
                    ReplacementDate = DateTime.Now.AddDays(-1),
                    RequestAction = RequestAction.Accept
                },
                new PersonnelShiftReplacement
                {
                    PersonnelId = 1,
                    ReplacedPersonnelId = 2,
                    WorkingHourId = 2,
                    ReplacedWorkingHourId = 2,
                    RequestedDate = DateTime.Now.AddDays(-1),
                    ActionDate = DateTime.Now,
                    ReplacementDate = DateTime.Now,
                    RequestAction = RequestAction.Accept
                },
            };
            context.PersonnelShiftReplacements.AddRange(shiftReplacements);
            #endregion

            #region Approval Procs
            var approvalProc1 = new ApprovalProc
            {
                ParentId = null,
                Title = "Approval Group 1",
                FirstPriorityId = 1,
                SecondPriorityId = 2,
                ThirdPriorityId = null,
                ActiveState = ActiveState.Active
            };
            var approvalProc2 = new ApprovalProc
            {
                ParentId = 1,
                Title = "Approval Group 2",
                FirstPriorityId = 2,
                SecondPriorityId = null,
                ThirdPriorityId = null,
                ActiveState = ActiveState.Active
            };
            context.ApprovalProcs.Add(approvalProc1);
            context.SaveChanges();
            context.ApprovalProcs.Add(approvalProc2);

            #endregion

            base.Seed(context);
        }
    }
}
