using AttendanceManagement.Service.Dtos.DismissalApproval;
using AttendanceManagement.Service.Dtos.DutyApproval;
using System.Collections.Generic;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.Personnel
{
    public class PersonnelDto
    {
        public int Id { get; set; }

        #region Personnel Info
        public string Code { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return Name + " " + LastName; } }
        public string FathersName { get; set; }
        public string NationalCode { get; set; }
        public string BirthCertificateCode { get; set; }
        public string PlaceOfBirth { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Education? Education { get; set; }
        public string EducationTitle { get; set; }
        public MilitaryServiceStatus? MilitaryServiceStatus { get; set; }
        public string MilitaryServiceStatusTitle { get; set; }
        public Gender Gender { get; set; }
        public string GenderTitle { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string MaritalStatusTitle { get; set; }
        #endregion

        #region Personnel Job Info
        public int GroupCategoryId { get; set; }
        public string GroupCategoryTitle { get; set; }
        public int EmployeementTypeId { get; set; }
        public string EmployeementTypeTitle { get; set; }
        public int WorkUnitId { get; set; }
        public string WorkUnitTitle { get; set; }
        public int PositionId { get; set; }
        public string PositionTitle { get; set; }
        public string InsuranceRecordDuration { get; set; }
        public string NoneInsuranceRecordDuration { get; set; }
        public string BankAccountNumber { get; set; }
        public string DateOfEmployeement { get; set; }
        public string FirstDateOfWork { get; set; }
        public string LastDateOfWork { get; set; }
        public string LeavingWorkCause { get; set; }
        #endregion

        #region Approval Procedures
        public int? DismissalApprovalProcId { get; set; }
        public string DismissalApprovalProcTitle { get; set; }
        public int? DutyApprovalProcId { get; set; }
        public string DutyApprovalProcTitle { get; set; }
        public int? ShiftReplacementProcId { get; set; }
        public string ShiftReplacementProcTitle { get; set; }
        #endregion

        #region Approvals
        public List<DismissalApprovalDto> DismissalApprovals { get; set; }
            = new List<DismissalApprovalDto>();
        public List<DutyApprovalDto> DutyApprovals { get; set; }
            = new List<DutyApprovalDto>();
        #endregion

        public ActiveState ActiveState { get; set; }
        public string ActiveStateTitle { get; set; }
    }

    public class PersonnelRecordDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return Name + " " + LastName; } }
    }

    public class PersonnelWithMostIndex
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public double Total { get; set; }
        public string Value { get; set; }
    }
}
