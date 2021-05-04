using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class WorkingHour : IEntity
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        [Required]
        public string Title { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public WorkingHourDuration WorkingHourDuration { get; set; }
        public int DailyDelay { get; set; }
        public int MonthlyDelay { get; set; }
        public int DailyRush { get; set; }
        public int MonthlyRush { get; set; }
        public int PriorExtraWorkTime { get; set; }
        public int LaterExtraWorkTime { get; set; }
        public int FloatingTime { get; set; }
        public TimeSpan? MealTimeBreakFromTime { get; set; }
        public TimeSpan? MealTimeBreakToTime { get; set; }
        //working hour breaks - lite version
        //break1
        public TimeSpan? Break1FromTime { get; set; }
        public TimeSpan? Break1ToTime { get; set; }
        //break2
        public TimeSpan? Break2FromTime { get; set; }
        public TimeSpan? Break2ToTime { get; set; }
        //break3
        public TimeSpan? Break3FromTime { get; set; }
        public TimeSpan? Break3ToTime { get; set; }
        public int Priority { get; set; }
        public DeleteState DeleteState { get; set; }

        public virtual Shift Shift { get; set; }
        public ICollection<PersonnelShiftReplacement> PersonnelShiftReplacements { get; set; }
        public ICollection<PersonnelShiftReplacement> ReplacedPersonnelShiftReplacements { get; set; }
        public ICollection<PersonnelEntrance> PersonnelEntrances { get; set; }
    }
}
