namespace AttendanceManagement.Service.Dtos.Personnel
{
    public class PartialUpdatePersonnelDto
    {
        public Core.Model.Personnel PersonnelEntity { get; set; }
        public PersonnelPatchDto PatchDto { get; set; }
    }

    public class PersonnelPatchDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
