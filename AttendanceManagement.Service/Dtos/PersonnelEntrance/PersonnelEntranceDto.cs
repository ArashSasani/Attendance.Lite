using System;

namespace AttendanceManagement.Service.Dtos.PersonnelEntrance
{
    public class PersonnelEntranceDto
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public string PersonnelCode { get; set; }
        public string PersonnelFullName { get; set; }
        public string EnterDate { get; set; }
        public string Enter { get; set; }
        public bool AutoEnter { get; set; }
        public string AutoEnterTitle { get; set; }
        public string ExitDate { get; set; }
        public string Exit { get; set; }
        public bool AutoExit { get; set; }
        public string AutoExitTitle { get; set; }
        public bool IsEdited { get; set; }
        public string IsEditedTitle { get; set; }
        public string EditDate { get; set; }
        public long RowNumber { get; set; }
    }

    public class PersonnelEntranceSummaryDto
    {
        public string EnterDate { get; set; }
        public string Enter { get; set; }
        public string ExitDate { get; set; }
        public string Exit { get; set; }
    }

    public class PersonnelEntranceChartDto
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
