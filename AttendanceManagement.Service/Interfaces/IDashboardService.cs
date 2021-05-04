using AttendanceManagement.Service.Dtos.Personnel;
using AttendanceManagement.Service.Dtos.PersonnelEntrance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagement.Service.Interfaces
{
    public interface IDashboardService
    {
        Task<List<PersonnelEntranceSummaryDto>> GetSummary(string username, int numberToShow);
        Task<List<PersonnelEntranceChartDto>> GetChartData(string username, DateTime fromDate, DateTime toDate);
        int AbsentPersonnelCount(DateTime date);
        int PresentPersonnelCount(DateTime date);
        List<PersonnelWithMostIndex> GetPersonnelWithMostAbsentTime(int pastDays, int numberToShow);
        List<PersonnelWithMostIndex> GetPersonnelWithMostOperationTime(int pastDays, int numberToShow);
        List<PersonnelWithMostIndex> GetPersonnelWithMostExtraWork(int pastDays, int numberToShow);
    }
}
