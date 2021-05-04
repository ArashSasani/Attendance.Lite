namespace AttendanceManagement.Service.Dtos.HourlyShift
{
    public class PartialUpdateHourlyShiftDto
    {
        public Core.Model.HourlyShift HourlyShiftEntity { get; set; }
        public HourlyShiftPatchDto PatchDto { get; set; }
    }
    public class HourlyShiftPatchDto
    {
        public string Title { get; set; }
    }
}
