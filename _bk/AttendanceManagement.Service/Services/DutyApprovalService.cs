using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.DutyApproval;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using CMS.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class DutyApprovalService : IDutyApprovalService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepository<DutyApproval> _dutyApprovalRepository;

        public DutyApprovalService(IAuthRepository authRepository
            , IRepository<DutyApproval> dutyApprovalRepository)
        {
            _authRepository = authRepository;
            _dutyApprovalRepository = dutyApprovalRepository;
        }

        public async Task<List<DutyApprovalDtoDDL>> GetForDDL(string username)
        {
            var approvals = new List<DutyApproval>();

            var user = await _authRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                approvals = _dutyApprovalRepository
                    .Get(q => q.Personnel.Code == user.UserName, includeProperties: "Personnel")
                .ToList();
            }

            return Mapper.Map<List<DutyApprovalDtoDDL>>(approvals);
        }

        public void Create(List<int> dutyIds, int personnelId)
        {
            foreach (int id in dutyIds)
            {
                _dutyApprovalRepository.Insert(new DutyApproval
                {
                    PersonnelId = personnelId,
                    DutyId = id
                });
            }
        }

        public void Update(List<int> dutyIds, int personnelId)
        {
            //prev duty approvals for the person
            var dutyApprovals = _dutyApprovalRepository
                .Get(q => q.PersonnelId == personnelId).ToList();
            if (dutyApprovals.Count > 0)
            {
                //items that should be removed
                var shouldBeRemovedDutyIds = dutyApprovals.Select(x => x.DutyId)
                    .Except(dutyIds).ToList();
                var shouldBeRemoved = dutyApprovals.Where(x => x.PersonnelId == personnelId
                    && shouldBeRemovedDutyIds.Contains(x.DutyId)).ToList();

                //new items that should be added
                var shouldBeAddedIds = dutyIds
                    .Except(dutyApprovals.Select(x => x.DutyId).ToList()).ToList();
                using (var scope = new TransactionScope())
                {
                    shouldBeRemoved.ForEach(item => _dutyApprovalRepository.Delete(item, DeleteState.Permanent));
                    shouldBeAddedIds.ForEach(id => _dutyApprovalRepository.Insert(new DutyApproval
                    {
                        DutyId = id,
                        PersonnelId = personnelId
                    }));
                    scope.Complete();
                }
            }
            else
            {
                dutyIds.ForEach(id => _dutyApprovalRepository.Insert(new DutyApproval
                {
                    DutyId = id,
                    PersonnelId = personnelId
                }));
            }
        }
    }
}
