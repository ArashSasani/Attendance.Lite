using AttendanceManagement.Service.Interfaces;
using CMS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Localization;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private readonly Guid _managerDashboardAccessId = AppSettings.ManagerDashboardAccessId;
        private readonly Guid _personnelDashboardAccessId = AppSettings.PersonnelDashboardAccessId;

        private readonly IDashboardService _dashboardService;
        private readonly IPersonnelDismissalService _personnelDismissalService;
        private readonly IDismissalService _dismissalService;
        private readonly IPersonnelService _personnelService;
        private readonly IAuthService _authService;

        public HomeController(IDashboardService dashboardService
            , IDismissalService dismissalService
            , IPersonnelDismissalService personnelDismissalService
            , IPersonnelService personnelService
            , IAuthService authService)
        {
            _dashboardService = dashboardService;
            _personnelDismissalService = personnelDismissalService;
            _dismissalService = dismissalService;
            _personnelService = personnelService;
            _authService = authService;
        }

        [Route("dashboard")]
        [HttpGet]
        public async Task<IHttpActionResult> LoadDashboard()
        {
            string username = User.Identity.Name;
            if (await HasAccessToManagerDashboard(username))
            {
                return await LoadManagerDashboard();
            }
            else if (await HasAccessToPersonnelDashboard(username))
            {
                return await LoadPersonnelDashboard();
            }

            return Unauthorized();
        }

        [Route("menu")]
        [HttpGet]
        public async Task<IHttpActionResult> LoadMenu()
        {
            var result = await _authService.GetRolesForUserByUsernameAsync(User.Identity.Name);
            var parentIds = new List<string>();
            foreach (var role in result.Roles)
            {
                var parentId = _authService.GetAccessPathsForRole(role.Id).AccessPaths
                    .Select(x => x.ParentId.ToString());
                parentIds.AddRange(parentId.Distinct().Select(x => x.Split('-')[0]));
            }
            return Ok(parentIds);
        }

        private async Task<bool> HasAccessToManagerDashboard(string username)
        {
            var result = await _authService.GetRolesForUserByUsernameAsync(username);
            foreach (var role in result.Roles)
            {
                if (_authService.DoesRoleHaveAccessPath(role.Id, _managerDashboardAccessId))
                    return true;
            }
            return false;
        }

        private async Task<bool> HasAccessToPersonnelDashboard(string username)
        {
            var result = await _authService.GetRolesForUserByUsernameAsync(username);
            foreach (var role in result.Roles)
            {
                if (_authService.DoesRoleHaveAccessPath(role.Id, _personnelDashboardAccessId))
                    return true;
            }
            return false;
        }

        private async Task<IHttpActionResult> LoadPersonnelDashboard()
        {
            string username = User.Identity.Name;
            var codeExists = _personnelService.CodeExists(username);
            if (codeExists)
            {
                //left widget
                var entrances = await _dashboardService.GetSummary(username, 10);
                //chart data
                var thisMonthChartData = await _dashboardService
                    .GetChartData(username, DateTime.Now.AddDays(-10), DateTime.Now);
                var prevMonthChartData = await _dashboardService
                    .GetChartData(username, DateTime.Now.AddMonths(-1).AddDays(-10), DateTime.Now.AddMonths(-1));
                //bottom widget
                var personnelDismissalInfo = await _personnelDismissalService
                    .GetChartInfo(username, (int)DismissalType.Demanded);
                var limitChartInfo = _dismissalService.GetChartInfo((int)DismissalType.Demanded);

                if (personnelDismissalInfo.DayData.UsedDismissalDuration.Days > 0)
                {
                    personnelDismissalInfo.DayData.UsedDismissalPercentage = "100";
                }
                else if (personnelDismissalInfo.DayData.UsedDismissalDuration.Hours > 0)
                {
                    personnelDismissalInfo.DayData.UsedDismissalPercentage
                        = (((float)personnelDismissalInfo.DayData.UsedDismissalDuration.Hours / 24) * 100)
                        .ToString("n2");
                }
                else
                {
                    personnelDismissalInfo.DayData.UsedDismissalPercentage = "0.00";
                }
                personnelDismissalInfo.MonthData.UsedDismissalPercentage =
                        (((float)personnelDismissalInfo.MonthData.UsedDismissalDuration.GetSecondsFromDuration() /
                        limitChartInfo.LimitValueForMonth.GetSecondsFromDuration()) * 100).ToString("n2");
                personnelDismissalInfo.YearData.UsedDismissalPercentage =
                    (((float)personnelDismissalInfo.YearData.UsedDismissalDuration.GetSecondsFromDuration() /
                    limitChartInfo.LimitValueForYear.GetSecondsFromDuration()) * 100).ToString("n2");
                personnelDismissalInfo.TotalData.UsedDismissalPercentage = personnelDismissalInfo.YearData.UsedDismissalPercentage;

                return Ok(new
                {
                    showManagerDashboard = false,
                    entrances,
                    thisMonthChartData,
                    prevMonthChartData,
                    personnelDismissalInfo
                });
            }
            return Ok();
        }

        private async Task<IHttpActionResult> LoadManagerDashboard()
        {
            //top widget
            int dismissalRequestsCount = _personnelDismissalService.GetNumberOfRequests(DateTime.Now);
            int absentsCount = _dashboardService.AbsentPersonnelCount(DateTime.Now);
            int presentsCount = _dashboardService.PresentPersonnelCount(DateTime.Now);
            int personnelCount = _personnelService.TotalNumberOfPersonnel();

            //chart data
            var thisMonthChartData = await _dashboardService
                    .GetChartData(null, DateTime.Now.AddDays(-7), DateTime.Now);
            var prevMonthChartData = await _dashboardService
                .GetChartData(null, DateTime.Now.AddMonths(-1).AddDays(-7)
                    , DateTime.Now.AddMonths(-1));
            //bottom widget
            var personnelWithMostAbsentTime = _dashboardService
                .GetPersonnelWithMostAbsentTime(7, 4);
            var personnelWithMostOperationTime = _dashboardService
                .GetPersonnelWithMostOperationTime(7, 4);
            var personnelWithMostExtraWork = _dashboardService
                .GetPersonnelWithMostExtraWork(7, 4);

            return Ok(new
            {
                showManagerDashboard = true,
                dismissalRequestsCount,
                absentsCount,
                presentsCount,
                personnelCount,
                thisMonthChartData,
                prevMonthChartData,
                personnelWithMostAbsentTime,
                personnelWithMostOperationTime,
                personnelWithMostExtraWork
            });
        }

    }
}
