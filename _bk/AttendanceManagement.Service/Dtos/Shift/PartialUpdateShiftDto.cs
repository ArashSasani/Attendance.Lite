namespace AttendanceManagement.Service.Dtos.Shift
{
    public class PartialUpdateShiftDto
    {
        public Core.Model.Shift ShiftEntity { get; set; }
        public ShiftPatchDto PatchDto { get; set; }
    }

    public class ShiftPatchDto
    {
        public string Title { get; set; }
    }
}
