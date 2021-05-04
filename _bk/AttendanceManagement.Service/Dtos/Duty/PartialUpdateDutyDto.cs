namespace AttendanceManagement.Service.Dtos.Duty
{
    public class PartialUpdateDutyDto
    {
        public Core.Model.Duty DutyEntity { get; set; }
        public DutyPatchDto PatchDto { get; set; }
    }

    public class DutyPatchDto
    {
        public string Title { get; set; }
    }
}
