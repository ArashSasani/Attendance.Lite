namespace AttendanceManagement.Service.Dtos.WorkingHour
{
    public class PartialUpdateWorkingHourDto
    {
        public Core.Model.WorkingHour WorkingHourEntity { get; set; }
        public WorkingHourPatchDto PatchDto { get; set; }
    }

    public class WorkingHourPatchDto
    {
        public string Title { get; set; }
    }
}
