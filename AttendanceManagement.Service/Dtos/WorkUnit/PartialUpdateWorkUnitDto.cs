namespace AttendanceManagement.Service.Dtos.WorkUnit
{
    public class PartialUpdateWorkUnitDto
    {
        public Core.Model.WorkUnit WorkUnitEntity { get; set; }
        public WorkUnitPatchDto PatchDto { get; set; }
    }

    public class WorkUnitPatchDto
    {
        public string Title { get; set; }
    }
}
