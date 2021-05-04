using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Dtos.PersonnelEntrance;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepository<PersonnelEntrance> _personnelEntranceRepository;

        private readonly IReportCalcService _reportCalcService;

        private readonly IExceptionLogger _logger;

        public DashboardService(IAuthRepository authRepository
            , IRepository<PersonnelEntrance> personnelEntranceRepository
            , IReportCalcService reportCalcService
            , IExceptionLogger logger)
        {
            _authRepository = authRepository;
            _personnelEntranceRepository = personnelEntranceRepository;

            _reportCalcService = reportCalcService;

            _logger = logger;
        }

        public async Task<List<PersonnelEntranceSummaryDto>> GetSummary(string username, int numberToShow)
        {
            var entrances = new List<PersonnelEntrance>();

            var user = await _authRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                entrances = _personnelEntranceRepository
                    .Get(q => q.Personnel.Code == user.UserName && (!q.AutoEnter || !q.AutoExit)
                    , includeProperties: "Personnel")
                    .OrderByDescending(o => o.EnterDate).ThenByDescending(o => o.Enter)
                    .Take(numberToShow).ToList();
            }

            return Mapper.Map<List<PersonnelEntranceSummaryDto>>(entrances);
        }

        public async Task<List<PersonnelEntranceChartDto>> GetChartData(string username
            , DateTime fromDate, DateTime toDate)
        {
            var entrances = new List<PersonnelEntrance>();

            if (!string.IsNullOrEmpty(username))
            {
                var user = await _authRepository.FindUserByUsernameAsync(username);
                if (user != null)
                {
                    entrances = _personnelEntranceRepository
                        .Get(q => q.Personnel.Code == user.UserName, includeProperties: "Personnel,WorkingHour")
                    .Where(q => q.WorkingHourId.HasValue)
                    .Where(q => q.EnterDate >= fromDate.Date && q.EnterDate <= toDate.Date
                        && q.ExitDate.HasValue && q.IsCompleted)
                    .OrderByDescending(o => o.EnterDate).ToList();
                }
            }
            else
            {
                entrances = _personnelEntranceRepository.Get(includeProperties: "WorkingHour")
                .Where(q => q.WorkingHourId.HasValue)
                .Where(q => q.EnterDate >= fromDate.Date && q.EnterDate <= toDate.Date
                    && q.ExitDate.HasValue && q.IsCompleted)
                .OrderByDescending(o => o.EnterDate).ToList();
            }

            var chartData = entrances.GroupBy(grp => grp.EnterDate)
                .Take((int)(toDate - fromDate).Days)
                .Select(x => new
                {
                    Date = x.Key,
                    Avg = x.Average(q => _reportCalcService.GetOperationTime(q, q.WorkingHour))
                }).Select(x => new PersonnelEntranceChartDto
                {
                    Date = x.Date,
                    Value = ((int)x.Avg).GetHoursFromSeconds()
                }).ToList();

            for (DateTime date = fromDate.Date; date < toDate.Date; date = date.AddDays(1))
            {
                if (!chartData.Any(e => e.Date.Date == date.Date))
                {
                    chartData.Add(new PersonnelEntranceChartDto
                    {
                        Date = date,
                        Value = 0
                    });
                }
            }

            return chartData.OrderBy(o => o.Date).Take((toDate - fromDate).Days).ToList();
        }

        public int AbsentPersonnelCount(DateTime date)
        {
            return _personnelEntranceRepository
                .Get().AsEnumerable().Where(q => q.EnterDate.Date == date.Date)
                .Where(q => _reportCalcService.IsAbsent(q)).Count();
        }

        public int PresentPersonnelCount(DateTime date)
        {
            return _personnelEntranceRepository
                .Get().AsEnumerable().Where(q => q.EnterDate.Date == date.Date)
                .Where(q => !_reportCalcService.IsAbsent(q)).Count();
        }

        public List<PersonnelWithMostIndex> GetPersonnelWithMostAbsentTime(int pastDays, int numberToShow)
        {
            return _personnelEntranceRepository.Get(q => q.WorkingHourId.HasValue
                && q.IsCompleted, includeProperties: "Personnel,workingHour")
                .OrderByDescending(o => o.EnterDate).Take(pastDays)
                .AsEnumerable()
                //.Where(q => _reportCalcService.IsAbsent(q))
                .GroupBy(g => g.Personnel)
                .Select(x => new PersonnelWithMostIndex
                {
                    Id = x.Key.Id,
                    FullName = x.Key.Name + " " + x.Key.LastName,
                    Total = x.Sum(q => _reportCalcService.GetTotalAbsenceDuration(q, q.WorkingHour))
                }).OrderByDescending(o => o.Total).Take(numberToShow)
                .Select(x => new PersonnelWithMostIndex
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Value = _reportCalcService.SecondsToTimeFormatted(x.Total)
                }).ToList();
        }

        public List<PersonnelWithMostIndex> GetPersonnelWithMostOperationTime(int pastDays, int numberToShow)
        {
            return _personnelEntranceRepository.Get(q => q.WorkingHourId.HasValue
                && q.IsCompleted, includeProperties: "Personnel,WorkingHour")
                .OrderByDescending(o => o.EnterDate).Take(pastDays)
                .AsEnumerable()
                .Where(q => !_reportCalcService.IsAbsent(q))
                .GroupBy(g => g.Personnel)
                .Select(x => new PersonnelWithMostIndex
                {
                    Id = x.Key.Id,
                    FullName = x.Key.Name + " " + x.Key.LastName,
                    Total = x.Sum(q => _reportCalcService.GetOperationTime(q, q.WorkingHour))
                }).OrderByDescending(o => o.Total).Take(numberToShow)
                .Select(x => new PersonnelWithMostIndex
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Value = _reportCalcService.SecondsToTimeFormatted(x.Total)
                }).ToList();
        }

        public List<PersonnelWithMostIndex> GetPersonnelWithMostExtraWork(int pastDays, int numberToShow)
        {
            return _personnelEntranceRepository.Get(q => q.WorkingHourId.HasValue
                && q.IsCompleted, includeProperties: "Personnel,WorkingHour")
                .OrderByDescending(o => o.EnterDate).Take(pastDays)
                .AsEnumerable()
                .Where(q => !_reportCalcService.IsAbsent(q))
                .GroupBy(g => g.Personnel)
                .Select(x => new PersonnelWithMostIndex
                {
                    Id = x.Key.Id,
                    FullName = x.Key.Name + " " + x.Key.LastName,
                    Total = x.Sum(q =>
                            _reportCalcService.GetTotalExtraWorkDuration(q, q.WorkingHour))
                }).OrderByDescending(o => o.Total).Take(numberToShow)
                .Select(x => new PersonnelWithMostIndex
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Value = _reportCalcService.SecondsToTimeFormatted(x.Total)
                }).ToList();
        }
    }
}
