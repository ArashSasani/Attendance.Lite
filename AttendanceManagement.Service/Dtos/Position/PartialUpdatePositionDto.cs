namespace AttendanceManagement.Service.Dtos.Position
{
    public class PartialUpdatePositionDto
    {
        public Core.Model.Position PositionEntity { get; set; }
        public PositionPatchDto PatchDto { get; set; }
    }

    public class PositionPatchDto
    {
        public string Title { get; set; }
    }
}
