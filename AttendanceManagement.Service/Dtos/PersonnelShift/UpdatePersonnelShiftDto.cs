using AttendanceManagement.Service.Dtos.PersonnelShiftAssignment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;

namespace AttendanceManagement.Service.Dtos.PersonnelShift
{
    public class UpdatePersonnelShiftDto
    {
        public int Id { get; set; }
        public List<int> PersonnelIdList { get; set; }
            = new List<int>();
        public int ShiftId { get; set; }

        public List<PersonnelShiftAssignmentDto> ShiftAssignments { get; set; }
            = new List<PersonnelShiftAssignmentDto>();
    }

    public class UpdatePersonnelShiftDtoValidator : AbstractValidator<UpdatePersonnelShiftDto>
    {
        public UpdatePersonnelShiftDtoValidator()
        {
            RuleFor(x => x.PersonnelIdList)
                .Must(x => x.Count > 0)
                .WithMessage("please choose personnel");
        }
    }

    public class UpdatePersonnelShiftByPatternDto
    {
        public int Id { get; set; }
        public List<int> PersonnelIdList { get; set; }
            = new List<int>();
        public int ShiftId { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime ToDate { get; set; }
        public RepeatPattern? RepeatPattern { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
    }

    public class UpdatePersonnelShiftByPatternDtoValidator
        : AbstractValidator<UpdatePersonnelShiftByPatternDto>
    {
        public UpdatePersonnelShiftByPatternDtoValidator()
        {
            RuleFor(x => x.ToDate)
                .GreaterThan(x => x.FromDate)
                .WithMessage("end date cannot be less that start date");
        }
    }
}
