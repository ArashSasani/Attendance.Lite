namespace AttendanceManagement.Service.Dtos.EmployeementType
{
    public class PartialUpdateEmployeementTypeDto
    {
        public Core.Model.EmployeementType EmployeementTypeEntity { get; set; }
        public EmployeementTypePatchDto PatchDto { get; set; }
    }

    public class EmployeementTypePatchDto
    {
        public string Title { get; set; }
    }
}
