using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.DismissalApproval;
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
    public class DismissalApprovalService : IDismissalApprovalService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepository<DismissalApproval> _dismissalApprovalRepository;
        private readonly IRepository<Personnel> _personnelRepository;
        private readonly IRepository<Dismissal> _dismissalRepository;

        public DismissalApprovalService(IAuthRepository authRepository
            , IRepository<DismissalApproval> dismissalApprovalRepository
            , IRepository<Personnel> personnelRepository
            , IRepository<Dismissal> dismissalRepository)
        {
            _authRepository = authRepository;
            _dismissalApprovalRepository = dismissalApprovalRepository;
            _personnelRepository = personnelRepository;
            _dismissalRepository = dismissalRepository;
        }

        public async Task<List<DismissalApprovalDtoDDL>> GetForDDL(string username)
        {
            var approvals = new List<DismissalApproval>();

            var user = await _authRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                var personnel = _personnelRepository.Get(q => q.Code == user.UserName)
                    .SingleOrDefault();
                if (personnel != null)
                {
                    approvals = _dismissalApprovalRepository
                    .Get(q => q.Personnel.Id == personnel.Id
                        && q.Dismissal.DismissalSystemType == DismissalSystemType.Customized
                        , includeProperties: "Personnel,Dismissal")
                    .ToList();

                    #region default dismissals should apply to all personnel
                    var defaultDismissals = _dismissalRepository
                        .Get(q => q.DismissalSystemType == DismissalSystemType.Default).ToList();
                    foreach (var defaultDismissal in defaultDismissals)
                    {
                        approvals.Add(new DismissalApproval
                        {
                            DismissalId = defaultDismissal.Id,
                            Dismissal = defaultDismissal,
                            PersonnelId = personnel.Id,
                            Personnel = personnel
                        });
                    }
                    #endregion
                }
            }
            return Mapper.Map<List<DismissalApprovalDtoDDL>>(approvals);
        }

        public void Create(List<int> dismissalIds, int personnelId)
        {
            foreach (int id in dismissalIds)
            {
                _dismissalApprovalRepository.Insert(new DismissalApproval
                {
                    PersonnelId = personnelId,
                    DismissalId = id
                });
            }
        }

        public void Update(List<int> dismissalIds, int personnelId)
        {
            //prev dismisssal approvals for the person
            var dismissalApprovals = _dismissalApprovalRepository
                .Get(q => q.PersonnelId == personnelId).ToList();
            if (dismissalApprovals.Count > 0)
            {
                //items that should be removed
                var shouldBeRemovedDismissalIds = dismissalApprovals.Select(x => x.DismissalId)
                    .Except(dismissalIds).ToList();
                var shouldBeRemoved = dismissalApprovals.Where(x => x.PersonnelId == personnelId
                    && shouldBeRemovedDismissalIds.Contains(x.DismissalId)).ToList();

                //new items that should be added
                var shouldBeAddedIds = dismissalIds
                    .Except(dismissalApprovals.Select(x => x.DismissalId).ToList()).ToList();
                using (var scope = new TransactionScope())
                {
                    shouldBeRemoved.ForEach(item => _dismissalApprovalRepository.Delete(item, DeleteState.Permanent));
                    shouldBeAddedIds.ForEach(id => _dismissalApprovalRepository.Insert(new DismissalApproval
                    {
                        DismissalId = id,
                        PersonnelId = personnelId
                    }));
                    scope.Complete();
                }
            }
            else
            {
                dismissalIds.ForEach(id => _dismissalApprovalRepository.Insert(new DismissalApproval
                {
                    DismissalId = id,
                    PersonnelId = personnelId
                }));
            }
        }
    }
}
