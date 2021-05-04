using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebApplication.Infrastructure.Enums;
using WebApplication.Infrastructure.Localization;
using AttendanceManagement.Core.Model;
using WebApplication.SharedKernel.Enums;
using System.Data.Entity.Migrations;

namespace AttendanceManagement.Data.Data
{
    public static class Seed
    {
        public static void Init(CrudContext context)
        {
            #region Dismissals
            var defaultDemanded = new DemandedDismissal
            {
                Title = "استحقاقی",
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
            if (!context.DemandedDismissals.Any())
                context.DemandedDismissals.AddOrUpdate(defaultDemanded);

            var defaultSickness = new SicknessDismissal
            {
                Title = "استعلاجی",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Sickness,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                SicknessAllowanceInYear = (12).GetDaysInSeconds(),
                SicknessCountInYear = 12,
                SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            if (!context.SicknessDismissals.Any())
                context.SicknessDismissals.AddOrUpdate(defaultSickness);

            var defaultWithoutSalary = new WithoutSalaryDismissal
            {
                Title = "بدون حقوق",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.WithoutSalary,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                WithoutSalaryAllowanceInMonth = (5).GetDaysInSeconds(),
                WithoutSalaryCountInMonth = 2
            };
            if (!context.WithoutSalaryDismissals.Any())
                context.WithoutSalaryDismissals.AddOrUpdate(defaultWithoutSalary);

            var defaultEncouragement = new EncouragementDismissal
            {
                Title = "تشویقی",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Encouragement,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                EncouragementFromDate = DateTime.Now.AddDays(-2).Date,
                EncouragementToDate = DateTime.Now.Date,
                EncouragementConsiderWithoutSalary = true
            };
            if (!context.EncouragementDismissals.Any())
                context.EncouragementDismissals.AddOrUpdate(defaultEncouragement);

            var defaultMarriage = new MarriageDismissal
            {
                Title = "ازدواج",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.Marriage,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                MarriageAllowanceInTotal = (20).GetDaysInSeconds(),
                MarriageCountInTotal = 1,
                MarriageConsiderWithoutSalary = true,
                MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            if (!context.MarriageDismissals.Any())
                context.MarriageDismissals.AddOrUpdate(defaultMarriage);

            var defaultChildBirth = new ChildBirthDismissal
            {
                Title = "زایمان",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.ChildBirth,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                ChildBirthAllowanceInTotal = (180).GetDaysInSeconds(),
                ChildBirthConsiderWithoutSalary = false,
                ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            if (!context.ChildBirthDismissals.Any())
                context.ChildBirthDismissals.AddOrUpdate(defaultChildBirth);

            var defaultBreastFeeding = new BreastFeedingDismissal
            {
                Title = "شیردهی",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.BreastFeeding,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                BreastFeedingAllowanceInTotal = (20).GetDaysInSeconds(),
                BreastFeedingAllowanceInDay = (3).GetHoursInSeconds(),
                BreastFeedingCountInDay = 2,
                BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            if (!context.BreastFeedingDismissals.Any())
                context.BreastFeedingDismissals.AddOrUpdate(defaultBreastFeeding);

            var defaultDeathOfRelatives = new DeathOfRelativesDismissal
            {
                Title = "فوت بستگان",
                DismissalSystemType = DismissalSystemType.Default,
                DismissalType = DismissalType.DeathOfRelatives,
                DismissalExcessiveReaction = DismissalExcessiveReaction.Forbid,
                DeathOfRelativesAllowanceInTotal = (20).GetDaysInSeconds(),
                DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit = true
            };
            if (!context.DeathOfRelativesDismissals.Any())
                context.DeathOfRelativesDismissals.AddOrUpdate(defaultDeathOfRelatives);
            #endregion

            #region Duties
            var duties = new List<Duty>
            {
                new Duty
                {
                    Title = "دورن شهری"
                },
                new Duty
                {
                    Title = "بیرون شهری"
                }
            };
            if (!context.Duties.Any())
                duties.ForEach(i => context.Duties.AddOrUpdate(i));
            #endregion

            context.SaveChanges();

            #region personnel
            var workUnits = new List<WorkUnit>
            {
                new WorkUnit
                {
                    Title = "فروش"
                },
                new WorkUnit
                {
                    Title = "فنی"
                }
            };
            if (!context.WorkUnits.Any())
                workUnits.ForEach(i => context.WorkUnits.AddOrUpdate(i));

            context.SaveChanges();

            var groupCategories = new List<GroupCategory>
            {
                new GroupCategory
                {
                    Title = "فروش"
                },
                new GroupCategory
                {
                    Title = "مدیران"
                }
            };
            if (!context.GroupCategories.Any())
                groupCategories.ForEach(i => context.GroupCategories.AddOrUpdate(i));

            var employeementTypes = new List<EmployeementType>
            {
                new EmployeementType
                {
                    Title = "فروش"
                },
                new EmployeementType
                {
                    Title = "فنی"
                }
            };
            if (!context.EmployeementTypes.Any())
                employeementTypes.ForEach(i => context.EmployeementTypes.AddOrUpdate(i));

            var positions = new List<Position>
            {
                new Position
                {
                    WorkUnitId = 1,
                    Title = "مدیر فروش"
                },
                new Position
                {
                    WorkUnitId = 1,
                    Title = "مسئول فروش"
                },
                new Position
                {
                    WorkUnitId = 2,
                    Title = "مدیر فنی"
                },
                new Position
                {
                    WorkUnitId = 2,
                    Title = "برنامه نویس"
                }
            };
            if (!context.Positions.Any())
                positions.ForEach(i => context.Positions.AddOrUpdate(i));

            context.SaveChanges();

            var personnel1 = new Personnel
            {
                Code = "1",
                Name = "آرش",
                LastName = "ساسانی",
                FathersName = "مصطفی",
                NationalCode = "1234567890",
                BirthCerficiateCode = "123123123",
                PlaceOfBirth = "تهران",
                State = "تهران",
                City = "تهران",
                PostalCode = "123123123",
                BirthDate = DateTime.Now.AddYears(-29),
                Email = "arash@yahoo.com",
                Mobile = "09128027821",
                Phone = "44223344",
                Address = "تهران خ الف",
                Education = Education.Bachelor,
                MilitaryServiceStatus = MilitaryServiceStatus.Completed,
                Gender = Gender.Male,
                MaritalStatus = MaritalStatus.Single,
                GroupCategoryId = 1,
                EmployeementTypeId = 1,
                PositionId = 1,
                InsuranceRecordDuration = "2 ماه",
                NoneInsuranceRecordDuration = "2 سال",
                BankAccountNumber = "12312a123c",
                DateOfEmployeement = DateTime.Now.AddYears(-2),
                FirstDateOfWork = DateTime.Now.AddYears(-2).AddDays(2),
                ActiveState = ActiveState.Active,
                IsPresent = true
            };
            var personnel2 = new Personnel
            {
                Code = "2",
                Name = "ساناز",
                LastName = "محمدی",
                FathersName = "محمد",
                NationalCode = "1212567820",
                BirthCerficiateCode = "1231234333",
                PlaceOfBirth = "تهران",
                State = "تهران",
                City = "تهران",
                PostalCode = "125623123",
                BirthDate = DateTime.Now.AddYears(-39),
                Email = "arash@yahoo.com",
                Mobile = "09123457821",
                Phone = "4422334544",
                Address = "تهران خ الف",
                Education = Education.Phd,
                MilitaryServiceStatus = null,
                Gender = Gender.Female,
                MaritalStatus = MaritalStatus.Married,
                GroupCategoryId = 2,
                EmployeementTypeId = 2,
                PositionId = 2,
                InsuranceRecordDuration = "2 ماه",
                NoneInsuranceRecordDuration = "2 سال",
                BankAccountNumber = "12312a123c",
                DateOfEmployeement = DateTime.Now.AddYears(-3),
                FirstDateOfWork = DateTime.Now.AddYears(-3).AddDays(2),
                ActiveState = ActiveState.Active,
                IsPresent = true
            };
            if (!context.Personnel.Any())
            {
                context.Personnel.AddOrUpdate(personnel1);
                context.Personnel.AddOrUpdate(personnel2);
            }
            #endregion

            context.SaveChanges();

            #region Personnel Logs
            var personnelLogs = new List<PersonnelLog>
            {
                new PersonnelLog
                {
                    PersonnelId = 1,
                    LogDate = DateTime.Now.AddDays(-1),
                    SubmittedDate = DateTime.Now.AddDays(-1),
                    DeviceId = 1,
                    IsFingerEntrance = true,
                    FingerNo = 0
                }
                ,new PersonnelLog
                {
                    PersonnelId = 1,
                    LogDate = DateTime.Now.AddDays(-2),
                    SubmittedDate = DateTime.Now.AddDays(-2).AddMinutes(8),
                    DeviceId = 1,
                    IsFaceEntrance = true
                }
                ,new PersonnelLog
                {
                    PersonnelId = 1,
                    LogDate = DateTime.Now.AddDays(-3),
                    SubmittedDate = DateTime.Now.AddDays(-3).AddMinutes(8),
                    DeviceId = 1,
                    IsCardEntrance = true
                },
                new PersonnelLog
                {
                    PersonnelId = 2,
                    LogDate = DateTime.Now.AddDays(-1).AddMinutes(8),
                    SubmittedDate = DateTime.Now.AddDays(-1).AddMinutes(8),
                    DeviceId = 1,
                    IsFingerEntrance = true,
                    FingerNo = 0
                }
                ,new PersonnelLog
                {
                    PersonnelId = 2,
                    LogDate = DateTime.Now.AddDays(-2),
                    SubmittedDate = DateTime.Now.AddDays(-2).AddMinutes(8),
                    DeviceId = 1,
                    IsFaceEntrance = true
                }
                ,new PersonnelLog
                {
                    PersonnelId = 2,
                    LogDate = DateTime.Now.AddDays(-3),
                    SubmittedDate = DateTime.Now.AddDays(-3).AddMinutes(8),
                    DeviceId = 1,
                    IsCardEntrance = true
                }
            };
            if (!context.PersonnelLogs.Any())
                personnelLogs.ForEach(i => context.PersonnelLogs.AddOrUpdate(i));
            #endregion

            #region Dismissal Approvals
            //var dismissalsApprovals = new List<DismissalApproval>
            //{
            //    new DismissalApproval
            //    {
            //        PersonnelId = 1,
            //        DismissalId = 1
            //    },
            //    new DismissalApproval
            //    {
            //        PersonnelId = 1,
            //        DismissalId = 2
            //    }
            //};
            //if (!context.DismissalsApprovals.Any())
            //    dismissalsApprovals.ForEach(i => context.DismissalsApprovals.AddOrUpdate(i));
            #endregion

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
            if (!context.DutiesApprovals.Any())
                dutiesApprovals.ForEach(i => context.DutiesApprovals.AddOrUpdate(i));
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
            if (!context.PersonnelDailyDismissals.Any())
                context.PersonnelDailyDismissals.AddOrUpdate(dailyPersonnelDismissal);
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
            if (!context.PersonnelHourlyDismissals.Any())
                context.PersonnelHourlyDismissals.AddOrUpdate(hourlyPersonnelDismissal);
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
            if (!context.PersonnelDailyDuties.Any())
                context.PersonnelDailyDuties.AddOrUpdate(dailyPersonnelDuty);
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
            if (!context.PersonnelHourlyDuties.Any())
                context.PersonnelHourlyDuties.AddOrUpdate(hourlyPersonnelDuty);
            #endregion

            #region Shifts and Working Hours
            var shifts = new List<Shift>
            {
                new Shift
                {
                    Title = "شیفت نگهبانی"
                }
            };
            if (!context.Shifts.Any())
                shifts.ForEach(i => context.Shifts.AddOrUpdate(i));
            context.SaveChanges();

            var workingHours = new List<WorkingHour>
            {
                new WorkingHour
                {
                    ShiftId = 1,
                    Title = "ساعت کاری اول",
                    FromTime = TimeSpan.Parse("12:00"),
                    ToTime = TimeSpan.Parse("14:00"),
                    WorkingHourDuration = WorkingHourDuration.OneDay,
                    DailyDelay = (2).GetHoursInSeconds(),
                    MonthlyDelay = (12).GetHoursInSeconds(),
                    DelayPolicy = DelayPolicy.DailyLimit,
                    DailyRush = (2).GetHoursInSeconds(),
                    MonthlyRush = (12).GetHoursInSeconds(),
                    RushPolicy = RushPolicy.DailyLimit,
                    PriorExtraWorkTime = (2).GetHoursInSeconds(),
                    LaterExtraWorkTime = (1).GetHoursInSeconds(),
                    FloatingTime = (6).GetHoursInSeconds(),
                    MealTimeBreakFromTime = TimeSpan.Parse("12:00:00"),
                    MealTimeBreakToTime = TimeSpan.Parse("13:00:00")
                },
                new WorkingHour
                {
                    ShiftId = 1,
                    Title = "ساعت کاری دوم",
                    FromTime = TimeSpan.Parse("18:00"),
                    ToTime = TimeSpan.Parse("22:00"),
                    WorkingHourDuration = WorkingHourDuration.OneDay,
                    DailyDelay = (2).GetHoursInSeconds(),
                    MonthlyDelay = (12).GetHoursInSeconds(),
                    DelayPolicy = DelayPolicy.DailyLimit,
                    DailyRush = (2).GetHoursInSeconds(),
                    MonthlyRush = (12).GetHoursInSeconds(),
                    RushPolicy = RushPolicy.DailyLimit,
                    PriorExtraWorkTime = (2).GetHoursInSeconds(),
                    LaterExtraWorkTime = (1).GetHoursInSeconds(),
                    FloatingTime = (6).GetHoursInSeconds(),
                    MealTimeBreakFromTime = TimeSpan.Parse("19:00:00"),
                    MealTimeBreakToTime = TimeSpan.Parse("20:00:00")
                }
            };
            if (!context.WorkingHours.Any())
                workingHours.ForEach(i => context.WorkingHours.AddOrUpdate(i));
            context.SaveChanges();

            //var workingHourBreaks = new List<WorkingHourBreak>
            //{
            //    new WorkingHourBreak
            //    {
            //        WorkingHourId = 1,
            //        FromTime = TimeSpan.Parse("14:00:00"),
            //        ToTime = TimeSpan.Parse("15:00:00")
            //    },
            //    new WorkingHourBreak
            //    {
            //        WorkingHourId = 2,
            //        FromTime = TimeSpan.Parse("22:00:00"),
            //        ToTime = TimeSpan.Parse("23:00:00")
            //    }
            //};
            //context.WorkingHourBreaks.AddRange(workingHourBreaks);

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
                    DateAssigned = DateTime.Now.AddDays(-2)
                }
            };
            if (!context.PersonnelShifts.Any())
                personnelShifts.ForEach(i => context.PersonnelShifts.AddOrUpdate(i));
            #endregion

            context.SaveChanges();

            #region Personnel Shift Assignments
            var shiftAssignDates = new List<PersonnelShiftAssignment>
            {
                new PersonnelShiftAssignment
                {
                    PersonnelShiftId = 1,
                    Date = DateTime.Now.Date.AddDays(10)
                },
                new PersonnelShiftAssignment
                {
                    PersonnelShiftId = 2,
                    Date = DateTime.Now.Date.AddDays(12)
                }
            };
            if (!context.PersonnelShiftAssignments.Any())
                shiftAssignDates.ForEach(i => context.PersonnelShiftAssignments.AddOrUpdate(i));
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
            if (!context.PersonnelShiftReplacements.Any())
                shiftReplacements.ForEach(i => context.PersonnelShiftReplacements.AddOrUpdate(i));
            #endregion

            #region Approval Procs
            var approvalProc1 = new ApprovalProc
            {
                ParentId = null,
                Title = "تایید کننده 1",
                FirstPriorityId = 1,
                SecondPriorityId = 2,
                ThirdPriorityId = null,
                ActiveState = ActiveState.Active
            };
            var approvalProc2 = new ApprovalProc
            {
                ParentId = 1,
                Title = "تایید کننده 2",
                FirstPriorityId = 2,
                SecondPriorityId = null,
                ThirdPriorityId = null,
                ActiveState = ActiveState.Active
            };
            if (!context.ApprovalProcs.Any())
            {
                context.ApprovalProcs.AddOrUpdate(approvalProc1);
                context.SaveChanges();
                context.ApprovalProcs.AddOrUpdate(approvalProc2);
            }

            //personnel1.DismissalApprovalProcId = 1;
            //personnel1.DutyApprovalProcId = 1;
            //personnel1.ShiftReplacementProcId = 1;
            //personnel2.DismissalApprovalProcId = 2;
            //personnel2.DutyApprovalProcId = 2;
            //personnel2.ShiftReplacementProcId = 2;
            //context.SaveChanges();
            #endregion

            #region Calendar Dates
            //get shamsi and hijri holidays for the next 10 years
            var now = DateTime.Now;
            for (var date = now.AddYears(-1); date <= now.AddYears(10);
                date = date.AddYears(1))
            {
                SetupCalendarDates(context, date);
            }
            #endregion

            #region Adjustments And Ratios
            var adjustments = new List<Adjustment>
            {
                new Adjustment
                {
                    ShiftId = 1,
                    Title = "تهاتر 1",
                    AdjustmentType1 = AdjustmentType.Rush,
                    AdjustmentType2 = AdjustmentType.ExtraWork,
                    AdjustmentPriority = AdjustmentPriority.BeforeRatio
                },
                    new Adjustment
                {
                    ShiftId = 1,
                    Title = "تهاتر 2",
                    AdjustmentType1 = AdjustmentType.Rush,
                    AdjustmentType2 = AdjustmentType.ExtraWork,
                    AdjustmentPriority = AdjustmentPriority.BeforeRatio
                }
            };
            if (!context.Adjustments.Any())
                adjustments.ForEach(i => context.Adjustments.AddOrUpdate(i));

            var ratios = new List<Ratio>
            {
                new Ratio
                {
                    ShiftId = 1,
                    RatioType = RatioType.Delay,
                    Amount = 2,
                    FromTime = TimeSpan.Parse("12:00:00"),
                    ToTime = TimeSpan.Parse("14:00:00"),
                    AddedConstant = (30).GetMinutesInSeconds(),
                    HasStep = true
                },
                new Ratio
                {
                    ShiftId = 1,
                    RatioType = RatioType.Rush,
                    Amount = 2,
                    FromTime = TimeSpan.Parse("11:00:00"),
                    ToTime = TimeSpan.Parse("13:00:00"),
                    AddedConstant = (40).GetMinutesInSeconds(),
                    HasStep = false
                }
            };
            if (!context.Ratios.Any())
                ratios.ForEach(i => context.Ratios.AddOrUpdate(i));

            context.SaveChanges();
            #endregion

            #region Devices
            var devices = new List<Device>
            {
                new Device
                {
                    DeviceId = 1,
                    DeviceType = "Finger",
                    Model = "Finger",
                    SerialNumber = "123",
                    ActiveState = ActiveState.Active
                },
                new Device
                {
                    DeviceId = 2,
                    DeviceType = "Face",
                    Model = "Face",
                    SerialNumber = "123",
                    ActiveState = ActiveState.Deactive
                }
            };
            if (!context.Devices.Any())
                devices.ForEach(i => context.Devices.AddOrUpdate(i));
            #endregion
        }

        private static void SetupCalendarDates(CrudContext context, DateTime date)
        {
            int persianYear = date.GetYearPC();

            bool isLeapYear = persianYear.IsLeapYearPC();

            //1 farvardin converted to Gregorian
            DateTime fromDate = (persianYear + "/1/1").PersianToGregorian(TimeStatus.NoTime);
            //last day of persian date in the selected persian year converted to Gregorian
            DateTime toDate = isLeapYear == true ? (persianYear + "/12/30")
                .PersianToGregorian(TimeStatus.NoTime)
                : (persianYear + "/12/29").PersianToGregorian(TimeStatus.NoTime);

            //get persian holidays
            var persianHolidays = new List<CalendarDate>
            {
                new CalendarDate
                {
                    Title = "تعطیلات نوروز",
                    Date = (persianYear + "/1/1").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "تعطیلات نوروز",
                    Date = (persianYear + "/1/2").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "تعطیلات نوروز",
                    Date = (persianYear + "/1/3").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "تعطیلات نوروز",
                    Date = (persianYear + "/1/4").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "روز جمهوری اسلامی",
                    Date = (persianYear + "/1/12").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "13 به در",
                    Date = (persianYear + "/1/13").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "وفات آیت الله خمینی",
                    Date = (persianYear + "/3/14").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "قیام 15 خرداد",
                    Date = (persianYear + "/3/15").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "سالروز انقلاب اسلامی",
                    Date = (persianYear + "/11/22").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                },
                new CalendarDate
                {
                    Title = "سالروز ملی شدن صنعت نفت",
                    Date = (persianYear + "/12/29").PersianToGregorian(TimeStatus.NoTime),
                    IsHoliday = true
                }
            };

            //get hijri holidays
            List<int> hijriYears = new List<int>
            {
                fromDate.GetYearHC(),
                toDate.GetYearHC()
            };
            //because some days in current persian year can be in next hijri year
            List<CalendarDate> hijriHolidays = new List<CalendarDate>();
            foreach (var hijriYear in hijriYears)
            {
                hijriHolidays.AddRange(new List<CalendarDate>
                {
                    new CalendarDate
                    {
                        Title = "تاسوعا",
                        Date = (hijriYear + "/1/9").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "عاشورا",
                        Date = (hijriYear + "/1/10").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "اربعین حسینی",
                        Date = (hijriYear + "/2/20").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شهادت پیامبر اسلام و امام حسن مجتبی",
                        Date = (hijriYear + "/2/28").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شهادت امام رضا",
                        Date = (hijriYear + "/2/29").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شهادت امام حسن عسکری",
                        Date = (hijriYear + "/3/8").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "ولادت پیامبر اسلام و امام جعفر صادق",
                        Date = (hijriYear + "/3/17").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شهادت حضرت زهرا",
                        Date = (hijriYear + "/6/3").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "ولادت امام علی",
                        Date = (hijriYear + "/7/13").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "روز لیله معراج",
                        Date = (hijriYear + "/7/27").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "ولادت امام مهدی",
                        Date = (hijriYear + "/8/15").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شب قدر",
                        Date = (hijriYear + "/9/19").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شب قدر",
                        Date = (hijriYear + "/9/21").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "عید فطر",
                        Date = (hijriYear + "/10/1").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "شهادت امام جعفر صادق",
                        Date = (hijriYear + "/10/25").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "عید قربان",
                        Date = (hijriYear + "/12/10").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    },
                    new CalendarDate
                    {
                        Title = "عید قدیر",
                        Date = (hijriYear + "/12/18").HijriToGregorian(TimeStatus.NoTime),
                        IsHoliday = true
                    }
                });
            }

            if (!context.Holidays.Any())
            {
                persianHolidays.ForEach(i => context.Holidays.AddOrUpdate(i));
                hijriHolidays.ForEach(i => context.Holidays.AddOrUpdate(i));
            }
        }

    }
}
