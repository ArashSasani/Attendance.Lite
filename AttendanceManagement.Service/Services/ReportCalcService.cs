using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Interfaces;
using System;
using System.Linq;
using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class ReportCalcService : IReportCalcService
    {
        private readonly IRepository<PersonnelDuty> _personnelDutyRepository;
        private readonly IRepository<PersonnelDismissal> _personnelDismissalRepository;
        private readonly IRepository<PersonnelDutyEntrance> _personnelDutyEntranceRepository;
        private readonly IRepository<PersonnelDismissalEntrance> _personnelDismissalEntranceRepository;

        public ReportCalcService(IRepository<PersonnelDuty> personnelDutyRepository
            , IRepository<PersonnelDismissal> personnelDismissalRepository
            , IRepository<PersonnelDutyEntrance> personnelDutyEntranceRepository
            , IRepository<PersonnelDismissalEntrance> personnelDismissalEntranceRepository)
        {
            _personnelDutyRepository = personnelDutyRepository;
            _personnelDismissalRepository = personnelDismissalRepository;
            _personnelDutyEntranceRepository = personnelDutyEntranceRepository;
            _personnelDismissalEntranceRepository = personnelDismissalEntranceRepository;
        }

        public bool EntranceHasDelay(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            return entrance.Enter > workingHour.FromTime
                .Add(TimeSpan.FromSeconds(workingHour.DailyDelay));
        }

        public double GetRushDuration(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            //check exit
            if (!entrance.ExitDate.HasValue || !entrance.Exit.HasValue)
                return 0;

            double rush = 0;
            //if entrance has rush
            if (!entrance.AutoExit
                && (workingHour.ToTime - entrance.Exit.Value).TotalSeconds > workingHour.DailyRush)
            {
                //default
                rush = (workingHour.ToTime - entrance.Exit.Value).TotalSeconds;

                //check dismissal
                var dismissalTimePeriod = GetStartAndEndOfDismissal(entrance);
                //check duty
                var dutyTimePeriod = GetStartAndEndOfDuty(entrance);
                //entrance exit
                var exit = entrance.ExitDate.Value.Add(entrance.Exit.Value);
                if (dismissalTimePeriod != null)
                {
                    if (exit >= dismissalTimePeriod.Start && exit <= dismissalTimePeriod.End)
                    //dismissal fills the gap
                    {
                        rush = 0;
                    }
                }
                if (dutyTimePeriod != null)
                {
                    if (exit >= dutyTimePeriod.Start && exit <= dutyTimePeriod.End)
                    //dismissal fills the gap
                    {
                        rush = 0;
                    }
                }
            }

            return rush;
        }

        public double GetDelayDuration(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            double delay = 0;
            //if entrance has delay
            if (!entrance.AutoEnter
                && (entrance.Enter - workingHour.FromTime).TotalSeconds > workingHour.DailyDelay)
            {
                //default
                delay = (entrance.Enter - workingHour.FromTime).TotalSeconds;

                //check dismissal
                var dismissalTimePeriod = GetStartAndEndOfDismissal(entrance);
                //check duty
                var dutyTimePeriod = GetStartAndEndOfDuty(entrance);
                //entrance enter
                var enter = entrance.EnterDate.Add(entrance.Enter);
                if (dismissalTimePeriod != null)
                {
                    if (enter >= dismissalTimePeriod.Start && enter <= dismissalTimePeriod.End)
                    //dismissal fills the gap
                    {
                        delay = 0;
                    }
                }
                if (dutyTimePeriod != null)
                {
                    if (enter >= dutyTimePeriod.Start && enter <= dutyTimePeriod.End)
                    //duty fills the gap
                    {
                        delay = 0;
                    }
                }
            }
            return delay;
        }

        public OperationType GetOperationType(PersonnelEntrance entrance)
        {
            if ((entrance.AutoEnter && entrance.AutoExit)
                || (entrance.AutoEnter && !entrance.Exit.HasValue))
            {
                if (IsDailyDismissalOrDutyDay(entrance))
                    return OperationType.OnDutyOrDismissal;

                return OperationType.Absent;
            }
            return OperationType.Present;
        }

        public bool IsAbsent(PersonnelEntrance entrance)
        {
            return GetOperationType(entrance) == OperationType.Absent;
        }

        public double GetOperationTime(PersonnelEntrance entrance, WorkingHour workingHour
            , bool forceCalculate = false)
        {
            if (forceCalculate)
            {
                return CalcOperationTime(entrance, workingHour);
            }

            var operationType = GetOperationType(entrance);
            switch (operationType)
            {
                case OperationType.OnDutyOrDismissal:
                case OperationType.Absent:
                    return 0;
                default: //present
                    return CalcOperationTime(entrance, workingHour);
            }
        }

        public string GetOperationTimeFormatted(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            var timeDifference = GetOperationTime(entrance, workingHour);
            if (timeDifference == 0)
                TimeSpan.Zero.ToDetailedString();
            return SecondsToTimeFormatted(timeDifference);
        }

        public double GetTotalAbsenceDuration(PersonnelEntrance entrance, WorkingHour workingHour
            , bool forceDutyOrDismissalDay = false)
        {
            double absenceDuration = 0;
            //if entrance is daily dismissal or duty
            if (forceDutyOrDismissalDay || IsDailyDismissalOrDutyDay(entrance))
                return absenceDuration;
            //otherwise
            if (IsAbsent(entrance))
            {
                absenceDuration = GetOperationTime(entrance, workingHour, forceCalculate: true);
            }
            else
            {
                absenceDuration = GetRushDuration(entrance, workingHour)
                    + GetDelayDuration(entrance, workingHour)
                    + GetDutyAbsenceDuration(entrance)
                    + GetDismissalAbsenceDuration(entrance);
            }
            return absenceDuration;
        }

        public string GetAbsenceTitle(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            if (IsAbsent(entrance))
                return "absence";
            if (GetTotalAbsenceDuration(entrance, workingHour) > 0)
                return "shortage of work time";

            return "-";
        }

        public string GetTotalExtraWorkTitle(double priorExtraWorkDuration
            , double laterExtraWorkDuration, WorkingHour workingHour)
        {
            if (laterExtraWorkDuration > workingHour.LaterExtraWorkTime)
            {
                return "not authorized last extra work";
            }
            if (priorExtraWorkDuration > workingHour.PriorExtraWorkTime)
            {
                return "not authorized first extra work";
            }
            return "-";
        }

        public double GetLaterExtraWorkDuration(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            //check exit
            if (!entrance.ExitDate.HasValue || !entrance.Exit.HasValue)
                return 0;

            double laterExtraWork = 0;
            if (!IsAbsent(entrance))
            {
                laterExtraWork = (entrance.Exit.Value - workingHour.ToTime).TotalSeconds > 0
                        ? (entrance.Exit.Value - workingHour.ToTime).TotalSeconds : 0;
            }

            return laterExtraWork > workingHour.LaterExtraWorkTime
                ? workingHour.LaterExtraWorkTime : laterExtraWork;
        }

        public double GetPriorExtraWorkDuration(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            double priorExtraWork = 0;
            if (!IsAbsent(entrance))
            {
                priorExtraWork = (workingHour.FromTime - entrance.Enter).TotalSeconds > 0
                        ? (workingHour.FromTime - entrance.Enter).TotalSeconds : 0;
            }

            return priorExtraWork > workingHour.PriorExtraWorkTime
                ? workingHour.PriorExtraWorkTime : priorExtraWork;
        }

        public double GetTotalExtraWorkDuration(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            var totalExtraWork = GetPriorExtraWorkDuration(entrance, workingHour)
                + GetLaterExtraWorkDuration(entrance, workingHour);

            return totalExtraWork;
        }

        public double GetDismissalDuration(PersonnelDismissal personDismissal)
        {
            if (personDismissal == null)
                return 0;

            return personDismissal.DismissalDuration == RequestDuration.Daily
                        ? ((personDismissal as PersonnelDailyDismissal).ToDate - (personDismissal as PersonnelDailyDismissal).FromDate)
                            .TotalSeconds
                        : ((personDismissal as PersonnelHourlyDismissal).ToTime - (personDismissal as PersonnelHourlyDismissal).FromTime)
                            .TotalSeconds;
        }

        public double GetDutyDuration(PersonnelDuty personDuty)
        {
            if (personDuty == null)
                return 0;

            return personDuty.DutyDuration == RequestDuration.Daily
                        ? ((personDuty as PersonnelDailyDuty).ToDate - (personDuty as PersonnelDailyDuty).FromDate)
                            .TotalSeconds
                        : ((personDuty as PersonnelHourlyDuty).ToTime - (personDuty as PersonnelHourlyDuty).FromTime)
                            .TotalSeconds;
        }

        public PersonnelDuty GetEntranceDateDuty(PersonnelEntrance entrance)
        {
            PersonnelDuty personDuty = null;

            var personDuties = _personnelDutyRepository
                .Get(q => q.PersonnelId == entrance.PersonnelId
                    && q.ActionDate.HasValue
                    && q.RequestAction == RequestAction.Accept
                , includeProperties: "Duty")
                .OrderByDescending(o => o.ActionDate).ToList();

            foreach (var item in personDuties)
            {
                if (personDuty != null)
                    break;
                switch (item.DutyDuration)
                {
                    case RequestDuration.Daily:
                        if ((item as PersonnelDailyDuty).FromDate == entrance.EnterDate)
                        {
                            personDuty = item;
                        }
                        break;
                    case RequestDuration.Hourly:
                        if ((item as PersonnelHourlyDuty).Date == entrance.EnterDate)
                        {
                            personDuty = item;
                        }
                        break;
                }
            }
            return personDuty;
        }

        public PersonnelDismissal GetEntranceDateDismissal(PersonnelEntrance entrance)
        {
            PersonnelDismissal personDismissal = null;

            var personDismissals = _personnelDismissalRepository
                .Get(q => q.PersonnelId == entrance.PersonnelId
                    && q.ActionDate.HasValue
                    && q.RequestAction == RequestAction.Accept
                , includeProperties: "Dismissal")
                .OrderByDescending(o => o.ActionDate).ToList();

            foreach (var item in personDismissals)
            {
                if (personDismissal != null)
                    break;
                switch (item.DismissalDuration)
                {
                    case RequestDuration.Daily:
                        if ((item as PersonnelDailyDismissal).FromDate == entrance.EnterDate)
                        {
                            personDismissal = item;
                        }
                        break;
                    case RequestDuration.Hourly:
                        if ((item as PersonnelHourlyDismissal).Date == entrance.EnterDate)
                        {
                            personDismissal = item;
                        }
                        break;
                }
            }
            return personDismissal;
        }

        public string SecondsToTimeFormatted(double totalSeconds, bool useTotalHours = true)
        {
            return totalSeconds > 0
                ? TimeSpan.FromSeconds(totalSeconds).ToDetailedString(useTotalHours) : "-";
        }


        private double CalcOperationTime(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            if (!entrance.ExitDate.HasValue || !entrance.Exit.HasValue)
                return 0;
            var exit = entrance.Exit.Value <= workingHour.ToTime ? entrance.Exit.Value
                    : workingHour.ToTime;
            var enter = entrance.Enter >= workingHour.FromTime ? entrance.Enter
                : workingHour.FromTime;

            var exitDate = entrance.ExitDate.Value.Date.Add(exit);
            var enterDate = entrance.EnterDate.Date.Add(enter);
            return (exitDate - enterDate).TotalSeconds
                - GetWorkingHourBreakTimes(entrance, workingHour);
        }
        private double GetWorkingHourBreakTimes(PersonnelEntrance entrance, WorkingHour workingHour)
        {
            if (!entrance.ExitDate.HasValue || !entrance.Exit.HasValue)
                return 0;

            var exitDate = entrance.ExitDate.Value.Date.Add(entrance.Exit.Value);
            var enterDate = entrance.EnterDate.Date.Add(entrance.Enter);

            int duration = 0;
            switch (workingHour.WorkingHourDuration)
            {
                case WorkingHourDuration.OneDay:
                    duration = 0;
                    break;
                case WorkingHourDuration.TwoDays:
                    duration = 1;
                    break;
                case WorkingHourDuration.ThreeDays:
                    duration = 2;
                    break;
            }

            double totalBreakTime = 0;
            if (workingHour.MealTimeBreakToTime.HasValue && workingHour.MealTimeBreakFromTime.HasValue)
            {
                for (int i = 0; i <= duration; i++)
                {
                    if (exitDate >= entrance.EnterDate.AddDays(i).Add(workingHour.MealTimeBreakToTime.Value)
                        && enterDate <= entrance.EnterDate.AddDays(i).Add(workingHour.MealTimeBreakFromTime.Value))
                    {
                        totalBreakTime += (workingHour.MealTimeBreakToTime - workingHour.MealTimeBreakFromTime)
                            .Value.TotalSeconds;
                    }
                }
            }
            if (workingHour.Break1ToTime.HasValue && workingHour.Break1FromTime.HasValue)
            {
                for (int i = 0; i <= duration; i++)
                {
                    if (exitDate >= entrance.EnterDate.AddDays(i).Add(workingHour.Break1ToTime.Value)
                        && enterDate <= entrance.EnterDate.AddDays(i).Add(workingHour.Break1FromTime.Value))
                    {
                        totalBreakTime += (workingHour.Break1ToTime - workingHour.Break1FromTime)
                            .Value.TotalSeconds;
                    }
                }
            }
            if (workingHour.Break2ToTime.HasValue && workingHour.Break2FromTime.HasValue)
            {
                for (int i = 0; i <= duration; i++)
                {
                    if (exitDate >= entrance.EnterDate.AddDays(i).Add(workingHour.Break2ToTime.Value)
                        && enterDate <= entrance.EnterDate.AddDays(i).Add(workingHour.Break2FromTime.Value))
                    {
                        totalBreakTime += (workingHour.Break2ToTime - workingHour.Break2FromTime)
                            .Value.TotalSeconds;
                    }
                }
            }
            if (workingHour.Break3ToTime.HasValue && workingHour.Break3FromTime.HasValue)
            {
                for (int i = 0; i <= duration; i++)
                {
                    if (exitDate >= entrance.EnterDate.AddDays(i).Add(workingHour.Break3ToTime.Value)
                        && enterDate <= entrance.EnterDate.AddDays(i).Add(workingHour.Break3FromTime.Value))
                    {
                        totalBreakTime += (workingHour.Break3ToTime - workingHour.Break3FromTime)
                            .Value.TotalSeconds;
                    }
                }
            }
            return totalBreakTime;
        }
        private bool IsDailyDismissalOrDutyDay(PersonnelEntrance entrance)
        {
            //if entrance is daily dismissal or duty
            var personnelDismissal = GetEntranceDateDismissal(entrance);
            if (personnelDismissal != null)
            {
                if (personnelDismissal.DismissalDuration == RequestDuration.Daily)
                    return true;
            }
            var personnelDuty = GetEntranceDateDuty(entrance);
            if (personnelDuty != null)
            {
                if (personnelDuty.DutyDuration == RequestDuration.Daily)
                    return true;
            }
            return false;
        }

        #region personnel dismissal calc helpers
        private PersonnelDismissalEntrance GetPersonnelDismissalEntrance(PersonnelEntrance entrance)
        {
            return _personnelDismissalEntranceRepository
                .Get(q => q.PersonnelId == entrance.PersonnelId && q.IsCompleted)
                .AsEnumerable()
                .Where(q => q.StartDate.Add(q.Start) >= entrance.EnterDate.Add(entrance.Enter)
                    && q.EndDate.Value.Add(q.End.Value) <= entrance.ExitDate.Value.Add(entrance.Exit.Value))
                .FirstOrDefault();
        }

        private double GetDismissalEntranceDuration(PersonnelEntrance entrance)
        {
            if (!entrance.ExitDate.HasValue || !entrance.Exit.HasValue)
                return 0;

            double dismissalDuration = 0;

            var personnelDismissalEntrance = GetPersonnelDismissalEntrance(entrance);

            //recorded by dismissal card
            if (personnelDismissalEntrance != null)
            {
                dismissalDuration = (personnelDismissalEntrance.EndDate.Value.Add(personnelDismissalEntrance.End.Value)
                    - personnelDismissalEntrance.StartDate.Add(personnelDismissalEntrance.Start)).TotalSeconds;
            }
            else //applied and approved in system
            {
                var personnelDismissal = GetEntranceDateDismissal(entrance);
                if (personnelDismissal != null)
                {
                    dismissalDuration = GetDismissalDuration(personnelDismissal);
                }
            }

            return dismissalDuration;
        }

        private TimePeriod GetStartAndEndOfDismissal(PersonnelEntrance entrance)
        {
            var personnelDismissalEntrance = GetPersonnelDismissalEntrance(entrance);
            if (personnelDismissalEntrance != null) //recorded
            {
                return new TimePeriod
                    (personnelDismissalEntrance.StartDate.Add(personnelDismissalEntrance.Start)
                    , personnelDismissalEntrance.EndDate.Value.Add(personnelDismissalEntrance.End.Value));
            }
            else //system
            {
                var personnelDismissal = GetEntranceDateDismissal(entrance);
                if (personnelDismissal != null)
                {
                    switch (personnelDismissal.DismissalDuration)
                    {
                        case RequestDuration.Daily:
                            var daily = (personnelDismissal as PersonnelDailyDismissal);
                            if (daily.FromDate == entrance.EnterDate)
                            {
                                return new TimePeriod
                                    (daily.FromDate, daily.ToDate);
                            }
                            break;
                        case RequestDuration.Hourly:
                            var hourly = (personnelDismissal as PersonnelHourlyDismissal);
                            if (hourly.Date == entrance.EnterDate)
                            {
                                return new TimePeriod
                                    (hourly.Date.Add(hourly.FromTime), hourly.Date.Add(hourly.ToTime));
                            }
                            break;
                    }
                }
            }
            return null;
        }

        private double GetDismissalAbsenceDuration(PersonnelEntrance entrance)
        {
            double dismissalDuration = 0;

            var entranceDateDismissal = GetEntranceDateDismissal(entrance);
            if (entranceDateDismissal != null)
            {
                dismissalDuration = GetDismissalDuration(entranceDateDismissal);
            }
            double dismissalEntranceDuration = GetDismissalEntranceDuration(entrance);
            return dismissalDuration > dismissalEntranceDuration ? 0 : dismissalEntranceDuration - dismissalDuration;
        }
        #endregion

        #region personnel duty calc helpers
        private PersonnelDutyEntrance GetPersonnelDutyEntrance(PersonnelEntrance entrance)
        {
            return _personnelDutyEntranceRepository
                .Get(q => q.PersonnelId == entrance.PersonnelId && q.IsCompleted)
                .AsEnumerable()
                .Where(q => q.StartDate.Add(q.Start) >= entrance.EnterDate.Add(entrance.Enter)
                    && q.EndDate.Value.Add(q.End.Value) <= entrance.ExitDate.Value.Add(entrance.Exit.Value))
                .FirstOrDefault();
        }

        private double GetDutyEntranceDuration(PersonnelEntrance entrance)
        {
            if (!entrance.ExitDate.HasValue || !entrance.Exit.HasValue)
                return 0;

            double dutyDuration = 0;

            var personnelDutyEntrance = GetPersonnelDutyEntrance(entrance);

            //recorded by duty card
            if (personnelDutyEntrance != null)
            {
                dutyDuration = (personnelDutyEntrance.EndDate.Value.Add(personnelDutyEntrance.End.Value)
                    - personnelDutyEntrance.StartDate.Add(personnelDutyEntrance.Start)).TotalSeconds;
            }
            else //applied and approved in system
            {
                var personnelDuty = GetEntranceDateDuty(entrance);
                if (personnelDuty != null)
                {
                    dutyDuration = GetDutyDuration(personnelDuty);
                }
            }

            return dutyDuration;
        }

        private TimePeriod GetStartAndEndOfDuty(PersonnelEntrance entrance)
        {
            var personnelDutyEntrance = GetPersonnelDutyEntrance(entrance);
            if (personnelDutyEntrance != null) //recorded
            {
                return new TimePeriod
                    (personnelDutyEntrance.StartDate.Add(personnelDutyEntrance.Start)
                    , personnelDutyEntrance.EndDate.Value.Add(personnelDutyEntrance.End.Value));
            }
            else //system
            {
                var personnelDuty = GetEntranceDateDuty(entrance);
                if (personnelDuty != null)
                {
                    switch (personnelDuty.DutyDuration)
                    {
                        case RequestDuration.Daily:
                            var daily = (personnelDuty as PersonnelDailyDuty);
                            if (daily.FromDate == entrance.EnterDate)
                            {
                                return new TimePeriod
                                    (daily.FromDate, daily.ToDate);
                            }
                            break;
                        case RequestDuration.Hourly:
                            var hourly = (personnelDuty as PersonnelHourlyDuty);
                            if (hourly.Date == entrance.EnterDate)
                            {
                                return new TimePeriod
                                    (hourly.Date.Add(hourly.FromTime), hourly.Date.Add(hourly.ToTime));
                            }
                            break;
                    }
                }
            }
            return null;
        }

        private double GetDutyAbsenceDuration(PersonnelEntrance entrance)
        {
            double dutyDuration = 0;

            var entranceDateDuty = GetEntranceDateDuty(entrance);
            if (entranceDateDuty != null)
            {
                dutyDuration = GetDutyDuration(entranceDateDuty);
            }
            double dutyEntranceDuration = GetDutyEntranceDuration(entrance);
            return dutyDuration > dutyEntranceDuration ? 0 : dutyEntranceDuration - dutyDuration;
        }
        #endregion
    }
}
