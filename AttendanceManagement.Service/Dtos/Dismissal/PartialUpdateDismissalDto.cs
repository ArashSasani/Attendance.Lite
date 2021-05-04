namespace AttendanceManagement.Service.Dtos.Dismissal
{
    public class PartialUpdateDismissalDto
    {
        public Core.Model.Dismissal DismissalEntity { get; set; }
        public DismissalPatchDto PatchDto { get; set; }
    }

    public class DismissalPatchDto
    {
        public string Title { get; set; }
    }
}
