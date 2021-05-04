using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.ApprovalProc
{
    public class ApprovalProcDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }
        public string Title { get; set; }
        public int FirstPriorityGroupCategoryId { get; set; }
        public int FirstPriorityId { get; set; }
        public string FirstPriorityFullName { get; set; }
        public int? SecondPriorityGroupCategoryId { get; set; }
        public int? SecondPriorityId { get; set; }
        public string SecondPriorityFullName { get; set; }
        public int? ThirdPriorityGroupCategoryId { get; set; }
        public int? ThirdPriorityId { get; set; }
        public string ThirdPriorityFullName { get; set; }
        public ActiveState ActiveState { get; set; }
        public string ActiveStateTitle { get; set; }
    }

    public class ApprovalProcDtoForDDL
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class ApprovalProcDtoForProfile
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }
        public string Title { get; set; }
        public string FirstPriorityFullName { get; set; }
        public string SecondPriorityFullName { get; set; }
        public string ThirdPriorityFullName { get; set; }
    }
}
