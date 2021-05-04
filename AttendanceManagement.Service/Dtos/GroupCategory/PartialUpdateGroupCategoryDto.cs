namespace AttendanceManagement.Service.Dtos.GroupCategory
{
    public class PartialUpdateGroupCategoryDto
    {
        public Core.Model.GroupCategory GroupCategoryEntity { get; set; }
        public GroupCategoryPatchDto PatchDto { get; set; }
    }

    public class GroupCategoryPatchDto
    {
        public string Title { get; set; }
    }
}
