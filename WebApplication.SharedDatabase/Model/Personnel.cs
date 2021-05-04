using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedDatabase.Model
{
    public class Personnel
    {
        public int Id { get; set; }

        #region Personnel Info
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FathersName { get; set; }
        [Required]
        public string NationalCode { get; set; }
        public string BirthCertificateCode { get; set; }
        public string PlaceOfBirth { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Education? Education { get; set; }
        public MilitaryServiceStatus? MilitaryServiceStatus { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        #endregion

        #region Personnel Job Info
        public int GroupCategoryId { get; set; }
        public int EmployeementTypeId { get; set; }
        public int PositionId { get; set; }
        public string InsuranceRecordDuration { get; set; }
        public string NoneInsuranceRecordDuration { get; set; }
        public string BankAccountNumber { get; set; }
        public DateTime DateOfEmployeement { get; set; }
        public DateTime FirstDateOfWork { get; set; }
        public DateTime? LastDateOfWork { get; set; }
        public string LeavingWorkCause { get; set; }

        public GroupCategory GroupCategory { get; set; }
        public EmployeementType EmployeemnetType { get; set; }
        public Position Position { get; set; }
        #endregion

        #region Approval Procedures
        public PersonnelApprovalProc PersonnelApprovalProc { get; set; }
        #endregion

        #region Dismissal Settings
        #endregion

        #region Approvals
        public ICollection<DismissalApproval> DismissalApprovals { get; set; }
        public ICollection<DutyApproval> DutyApprovals { get; set; }
        #endregion

        public bool IsPresent { get; set; }

        public DeleteState DeleteState { get; set; }
        public ActiveState ActiveState { get; set; }

        public ICollection<PersonnelDismissal> PersonnelDismissals { get; set; }
        public ICollection<PersonnelDuty> PersonnelDuties { get; set; }
        public ICollection<PersonnelEntrance> PersonnelEntrances { get; set; }
        public ICollection<PersonnelDismissalEntrance> PersonnelDismissalEntrances { get; set; }
        public ICollection<PersonnelDutyEntrance> PersonnelDutyEntrances { get; set; }
        public ICollection<PersonnelShift> PersonnelShifts { get; set; }
        public ICollection<PersonnelShiftReplacement> PersonnelShiftReplacements { get; set; }
        public ICollection<PersonnelShiftReplacement> ReplacedPersonnelShiftReplacements { get; set; }
        public ICollection<ApprovalProc> FirstPriorities { get; set; }
        public ICollection<ApprovalProc> SecondPriorities { get; set; }
        public ICollection<ApprovalProc> ThirdPriorities { get; set; }
    }
}
