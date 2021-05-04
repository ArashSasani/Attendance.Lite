using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.Personnel
{
    public class CreatePersonnelDto
    {
        #region Personnel Info
        [Required(ErrorMessage = "*")]
        public string Code { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
        public string FathersName { get; set; }
        [Required(ErrorMessage = "*")]
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
        [Required(ErrorMessage = "*")]
        public DateTime DateOfEmployeement { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime FirstDateOfWork { get; set; }
        public DateTime? LastDateOfWork { get; set; }
        public string LeavingWorkCause { get; set; }
        #endregion

        #region Approval Procedures
        public int? DismissalApprovalProcId { get; set; }
        public int? DutyApprovalProcId { get; set; }
        public int? ShiftReplacementProcId { get; set; }
        #endregion

        #region Approvals
        public List<int> DismissalApprovals { get; set; } = new List<int>();
        public List<int> DutyApprovals { get; set; } = new List<int>();
        #endregion

        public ActiveState ActiveState { get; set; }
    }

    public class CreatePersonnelDtoValidator : AbstractValidator<CreatePersonnelDto>
    {
        public CreatePersonnelDtoValidator()
        {
            RuleFor(x => x.FirstDateOfWork)
                .GreaterThanOrEqualTo(x => x.DateOfEmployeement)
                .WithMessage("first date of work cannot be less than date of employeement");

            RuleFor(x => x.LastDateOfWork.Value)
                .GreaterThanOrEqualTo(x => x.FirstDateOfWork)
                .When(x => x.LastDateOfWork.HasValue)
                .WithMessage("last date of work cannot be less than first date of work");
        }
    }
}
