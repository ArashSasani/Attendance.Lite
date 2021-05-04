using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.PersonnelShiftAssignment;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class PersonnelShiftAssignmentService : IPersonnelShiftAssignmentService
    {
        private readonly IRepository<PersonnelShiftAssignment> _personnelShiftAssignmentRepository;
        private readonly IRepository<PersonnelShift> _personnelShiftRepository;

        private readonly IExceptionLogger _logger;

        public PersonnelShiftAssignmentService(IRepository<PersonnelShift> personnelShiftRepository
            , IRepository<PersonnelShiftAssignment> personnelShiftAssignmentRepository
            , IExceptionLogger logger)
        {
            _personnelShiftRepository = personnelShiftRepository;
            _personnelShiftAssignmentRepository = personnelShiftAssignmentRepository;

            _logger = logger;
        }

        public List<PersonnelShiftAssignmentDisplayDto> Get(int personnelId)
        {
            var assignments = _personnelShiftAssignmentRepository
                .Get(q => q.PersonnelShift.PersonnelId == personnelId
                    && q.PersonnelShift.DeleteState != DeleteState.SoftDelete
                , includeProperties: "PersonnelShift").ToList();

            return Mapper.Map<List<PersonnelShiftAssignmentDisplayDto>>(assignments);
        }

        public PersonnelShiftAssignmentDisplayDto GetById(int id)
        {
            var assignment = _personnelShiftAssignmentRepository.GetById(id);
            return Mapper.Map<PersonnelShiftAssignmentDisplayDto>(assignment);
        }

        public CustomResult Update(UpdatePersonnelShiftAssignmentDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var assignment = _personnelShiftAssignmentRepository.Get(q => q.Id == id
                , includeProperties: "PersonnelShift").SingleOrDefault();
            if (assignment != null)
            {
                //delete this assignment
                _personnelShiftAssignmentRepository.Delete(assignment, DeleteState.Permanent);

                //check if personnel shift has other assigments otherwise delete it
                var relativeAssignments = _personnelShiftAssignmentRepository
                    .Get(q => q.PersonnelShiftId == assignment.PersonnelShiftId && q.Id != assignment.Id)
                    .ToList();
                if (relativeAssignments.Count == 0)
                {
                    _personnelShiftRepository.Delete(assignment.PersonnelShiftId, DeleteState.Permanent);
                }
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError(ex, "Shift Assignment with Id: {0} should not be null" +
                        ", delete operation failed.", assignment.Id);
                    throw;
                }
            }
        }
    }
}
