using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.Dismissal;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Localization;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class DismissalService : IDismissalService
    {
        private readonly IRepository<Dismissal> _dismissalRepository;
        private readonly IRepository<DemandedDismissal> _demandedDismissalRepository;
        private readonly IRepository<SicknessDismissal> _sicknessDismissalRepository;
        private readonly IRepository<WithoutSalaryDismissal> _withoutSalaryDismissalRepository;
        private readonly IRepository<EncouragementDismissal> _encouragementDismissalRepository;
        private readonly IRepository<MarriageDismissal> _marriageDismissalRepository;
        private readonly IRepository<ChildBirthDismissal> _childBirthDismissalRepository;
        private readonly IRepository<BreastFeedingDismissal> _breastFeedingDismissalRepository;
        private readonly IRepository<DeathOfRelativesDismissal> _deathOfRelativesDismissalRepository;

        private readonly IExceptionLogger _logger;

        public DismissalService(IRepository<Dismissal> dismissalRepository
            , IRepository<DemandedDismissal> demandedDismissalRepository
            , IRepository<SicknessDismissal> sicknessDismissalRepository
            , IRepository<WithoutSalaryDismissal> withoutSalaryDismissalRepository
            , IRepository<EncouragementDismissal> encouragementDismissalRepository
            , IRepository<MarriageDismissal> marriageDismissalRepository
            , IRepository<ChildBirthDismissal> childBirthRepository
            , IRepository<BreastFeedingDismissal> breastFeedingDismissalRepository
            , IRepository<DeathOfRelativesDismissal> deathOfRelativesDismissalRepository
            , IExceptionLogger logger)
        {
            _dismissalRepository = dismissalRepository;
            _demandedDismissalRepository = demandedDismissalRepository;
            _sicknessDismissalRepository = sicknessDismissalRepository;
            _withoutSalaryDismissalRepository = withoutSalaryDismissalRepository;
            _encouragementDismissalRepository = encouragementDismissalRepository;
            _marriageDismissalRepository = marriageDismissalRepository;
            _childBirthDismissalRepository = childBirthRepository;
            _breastFeedingDismissalRepository = breastFeedingDismissalRepository;
            _deathOfRelativesDismissalRepository = deathOfRelativesDismissalRepository;

            _logger = logger;
        }


        public IPaging<DismissalDtoForPaging> Get(DismissalSystemTypeAccess typeAccess
            , string searchTerm, string sortItem, string sortOrder, PagingQueryString pagingQueryString)
        {
            IPaging<DismissalDtoForPaging> model = new DismissalDtoPagingList();

            IQueryable<Dismissal> query;

            switch (typeAccess)
            {
                case DismissalSystemTypeAccess.Default:
                    query = !string.IsNullOrEmpty(searchTerm)
                       ? _dismissalRepository.Get(q => q.DismissalSystemType == DismissalSystemType.Default
                            && q.Title.Contains(searchTerm.ToLower()))
                       : _dismissalRepository.Get(q => q.DismissalSystemType == DismissalSystemType.Default);
                    break;
                case DismissalSystemTypeAccess.Customized:
                    query = !string.IsNullOrEmpty(searchTerm)
                       ? _dismissalRepository.Get(q => q.DismissalSystemType == DismissalSystemType.Customized
                            && q.Title.Contains(searchTerm.ToLower()))
                       : _dismissalRepository.Get(q => q.DismissalSystemType == DismissalSystemType.Customized);
                    break;
                default: //all
                    query = !string.IsNullOrEmpty(searchTerm)
                       ? _dismissalRepository.Get(q => q.Title.Contains(searchTerm.ToLower()))
                       : _dismissalRepository.Get();
                    break;
            }
            //total number of items
            int queryCount = query.Count();
            switch (sortItem)
            {
                case "title":
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Title)
                        : query.OrderByDescending(o => o.Title);
                    break;
                default:
                    query = query.OrderByDescending(o => o.Id);
                    break;
            }

            List<Dismissal> queryResult;
            if (pagingQueryString != null) //with paging
            {
                var pageSetup = new PagingSetup(pagingQueryString.Page, pagingQueryString.PageSize);
                queryResult = query.Skip(pageSetup.Offset).Take(pageSetup.Next).ToList();
                //paging controls
                var controls = pageSetup.GetPagingControls(queryCount, PagingStrategy.ReturnNull);
                if (controls != null)
                {
                    model.PagesCount = controls.PagesCount;
                    model.NextPage = controls.NextPage;
                    model.PrevPage = controls.PrevPage;
                }
            }
            else //without paging
            {
                queryResult = query.ToList();
            }
            model.PagingList = Mapper.Map<List<DismissalDtoForPaging>>(queryResult);

            return model;
        }

        public DismissalDto GetById(int id)
        {
            var dismissal = _dismissalRepository.GetById(id);
            if (dismissal == null)
            {
                return null;
            }
            switch (dismissal.DismissalType)
            {
                case DismissalType.Demanded:
                    var demanded = dismissal as DemandedDismissal;
                    return Mapper.Map<DismissalDto>(demanded);
                case DismissalType.Sickness:
                    var sickness = dismissal as SicknessDismissal;
                    return Mapper.Map<DismissalDto>(sickness);
                case DismissalType.WithoutSalary:
                    var withoutSalary = dismissal as WithoutSalaryDismissal;
                    return Mapper.Map<DismissalDto>(withoutSalary);
                case DismissalType.Encouragement:
                    var encouragement = dismissal as EncouragementDismissal;
                    return Mapper.Map<DismissalDto>(encouragement);
                case DismissalType.Marriage:
                    var marriage = dismissal as MarriageDismissal;
                    return Mapper.Map<DismissalDto>(marriage);
                case DismissalType.ChildBirth:
                    var childBirth = dismissal as ChildBirthDismissal;
                    return Mapper.Map<DismissalDto>(childBirth);
                case DismissalType.BreastFeeding:
                    var breastFeeding = dismissal as BreastFeedingDismissal;
                    return Mapper.Map<DismissalDto>(breastFeeding);
                case DismissalType.DeathOfRelatives:
                    var deathOfRelatives = dismissal as DeathOfRelativesDismissal;
                    return Mapper.Map<DismissalDto>(deathOfRelatives);
                default:
                    break;
            }
            return Mapper.Map<DismissalDto>(dismissal);
        }

        public DismissalDto GetDefault(DismissalType dismissalType)
        {
            var dismissal = _dismissalRepository.Get(q => q.DismissalType == dismissalType
                && q.DismissalSystemType == DismissalSystemType.Default).SingleOrDefault();
            if (dismissal == null)
            {
                return null;
            }
            switch (dismissalType)
            {
                case DismissalType.Demanded:
                    var demanded = dismissal as DemandedDismissal;
                    return Mapper.Map<DismissalDto>(demanded);
                case DismissalType.Sickness:
                    var sickness = dismissal as SicknessDismissal;
                    return Mapper.Map<DismissalDto>(sickness);
                case DismissalType.WithoutSalary:
                    var withoutSalary = dismissal as WithoutSalaryDismissal;
                    return Mapper.Map<DismissalDto>(withoutSalary);
                case DismissalType.Encouragement:
                    var encouragement = dismissal as EncouragementDismissal;
                    return Mapper.Map<DismissalDto>(encouragement);
                case DismissalType.Marriage:
                    var marriage = dismissal as MarriageDismissal;
                    return Mapper.Map<DismissalDto>(marriage);
                case DismissalType.ChildBirth:
                    var childBirth = dismissal as ChildBirthDismissal;
                    return Mapper.Map<DismissalDto>(childBirth);
                case DismissalType.BreastFeeding:
                    var breastFeeding = dismissal as BreastFeedingDismissal;
                    return Mapper.Map<DismissalDto>(breastFeeding);
                case DismissalType.DeathOfRelatives:
                    var deathOfRelatives = dismissal as DeathOfRelativesDismissal;
                    return Mapper.Map<DismissalDto>(deathOfRelatives);
                default:
                    return null;
            }
        }

        public List<DismissalDtoDDL> GetForDDL(DismissalSystemTypeAccess typeAccess)
        {
            var dismissals = new List<Dismissal>();
            switch (typeAccess)
            {
                case DismissalSystemTypeAccess.Default:
                    dismissals = _dismissalRepository
                        .Get(q => q.DismissalSystemType == DismissalSystemType.Default).ToList();
                    break;
                case DismissalSystemTypeAccess.Customized:
                    dismissals = _dismissalRepository
                        .Get(q => q.DismissalSystemType == DismissalSystemType.Customized).ToList();
                    break;
                case DismissalSystemTypeAccess.All:
                    dismissals = _dismissalRepository.Get().ToList();
                    break;
            }
            return Mapper.Map<List<DismissalDtoDDL>>(dismissals);
        }

        public DismissalAllowancesDto GetAllowances(int id)
        {
            var selectedDismissal = _dismissalRepository.GetById(id);

            var allowances = new DismissalAllowancesDto();

            switch (selectedDismissal.DismissalType)
            {
                case DismissalType.Demanded:
                    var demanded = selectedDismissal as DemandedDismissal;
                    allowances.AllowanceInYear = demanded.DemandedAllowanceInYear;
                    allowances.CountInYear = demanded.DemandedCountInYear;
                    allowances.AllowanceInMonth = demanded.DemandedAllowanceInMonth;
                    allowances.CountInMonth = demanded.DemandedCountInMonth;
                    break;
                case DismissalType.Sickness:
                    var sickness = selectedDismissal as SicknessDismissal;
                    allowances.AllowanceInYear = sickness.SicknessAllowanceInYear;
                    allowances.CountInYear = sickness.SicknessCountInYear;
                    break;
                case DismissalType.WithoutSalary:
                    var withoutSalary = selectedDismissal as WithoutSalaryDismissal;
                    allowances.AllowanceInMonth = withoutSalary.WithoutSalaryAllowanceInMonth;
                    allowances.CountInMonth = withoutSalary.WithoutSalaryCountInMonth;
                    break;
                case DismissalType.Encouragement:
                    break;
                case DismissalType.Marriage:
                    var marriage = selectedDismissal as MarriageDismissal;
                    allowances.AllowanceInTotal = marriage.MarriageAllowanceInTotal;
                    allowances.CountInTotal = marriage.MarriageCountInTotal;
                    break;
                case DismissalType.ChildBirth:
                    var childBirth = selectedDismissal as ChildBirthDismissal;
                    allowances.AllowanceInTotal = childBirth.ChildBirthAllowanceInTotal;
                    break;
                case DismissalType.BreastFeeding:
                    var breastFeeding = selectedDismissal as BreastFeedingDismissal;
                    allowances.AllowanceInTotal = breastFeeding.BreastFeedingAllowanceInTotal;
                    allowances.AllowanceInDay = breastFeeding.BreastFeedingAllowanceInDay;
                    allowances.CountInDay = breastFeeding.BreastFeedingCountInDay;
                    break;
                case DismissalType.DeathOfRelatives:
                    var deathOfRelatives = selectedDismissal as DeathOfRelativesDismissal;
                    allowances.AllowanceInTotal = deathOfRelatives.DeathOfRelativesAllowanceInTotal;
                    break;
                default:
                    break;
            }

            return allowances;
        }

        public DismissalChartDto GetChartInfo(int id)
        {
            var dismissal = _dismissalRepository.Get(q => q.Id == id
                && q.DismissalType != DismissalType.Encouragement).SingleOrDefault();
            if (dismissal == null)
            {
                return null;
            }
            switch (dismissal.DismissalType)
            {
                case DismissalType.Demanded:
                    var demanded = dismissal as DemandedDismissal;
                    return Mapper.Map<DismissalChartDto>(demanded);
                case DismissalType.Sickness:
                    var sickness = dismissal as SicknessDismissal;
                    return Mapper.Map<DismissalChartDto>(sickness);
                case DismissalType.WithoutSalary:
                    var withoutSalary = dismissal as WithoutSalaryDismissal;
                    return Mapper.Map<DismissalChartDto>(withoutSalary);
                //case DismissalType.Encouragement:
                //    var encouragement = dismissal as EncouragementDismissal;
                //    return Mapper.Map<DismissalChartDto>(encouragement);
                case DismissalType.Marriage:
                    var marriage = dismissal as MarriageDismissal;
                    return Mapper.Map<DismissalChartDto>(marriage);
                case DismissalType.ChildBirth:
                    var childBirth = dismissal as ChildBirthDismissal;
                    return Mapper.Map<DismissalChartDto>(childBirth);
                case DismissalType.BreastFeeding:
                    var breastFeeding = dismissal as BreastFeedingDismissal;
                    return Mapper.Map<DismissalChartDto>(breastFeeding);
                case DismissalType.DeathOfRelatives:
                    var deathOfRelatives = dismissal as DeathOfRelativesDismissal;
                    return Mapper.Map<DismissalChartDto>(deathOfRelatives);
                default:
                    return null;
            }
        }

        //only for customized dismissals
        public void Create(CreateDismissalDto dto)
        {
            switch (dto.DismissalType)
            {
                case DismissalType.Demanded:
                    if (dto.Demanded != null)
                    {
                        var demandedDismissal = new DemandedDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.Demanded,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            DemandedAllowanceInMonth = dto.Demanded.AllowanceInMonth.GetSecondsFromDuration(),
                            DemandedCountInMonth = dto.Demanded.CountInMonth,
                            DemandedIsTransferableToNextMonth = dto.Demanded.IsTransferableToNextMonth,
                            DemandedTransferableAllowanceToNextMonth = dto.Demanded.TransferableAllowanceToNextMonth
                                .GetSecondsFromDuration(),
                            DemandedAllowanceInYear = dto.Demanded.AllowanceInYear.GetSecondsFromDuration(),
                            DemandedCountInYear = dto.Demanded.CountInYear,
                            DemandedIsTransferableToNextYear = dto.Demanded.IsTransferableToNextYear,
                            DemandedTransferableAllowanceToNextYear = dto.Demanded.TransferableAllowanceToNextYear
                                .GetSecondsFromDuration(),
                            DemandedIsAllowedToSave = dto.Demanded.IsAllowedToSave,
                            DemandedAllowanceToSave = dto.Demanded.AllowanceToSave.GetSecondsFromDuration(),
                            DemandedMealTimeIsIncluded = dto.Demanded.MealTimeIsIncluded,
                            DemandedDoesDismissalMeansExtraWork = dto.Demanded.DoesDismissalMeansExtraWork,
                            DemandedIsNationalHolidysConsideredInDismissal = dto.Demanded.IsNationalHolidysConsideredInDismissal,
                            DemandedIsFridaysConsideredInDismissal = dto.Demanded.IsFridaysConsideredInDismissal,
                            DemandedAmountOfHoursConsideredDailyDismissal = dto.Demanded.AmountOfHoursConsideredDailyDismissal
                        };
                        _demandedDismissalRepository.Insert(demandedDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.Sickness:
                    if (dto.Sickness != null)
                    {
                        var sicknessDismissal = new SicknessDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.Sickness,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            SicknessAllowanceInYear = dto.Sickness.AllowanceInYear.GetSecondsFromDuration(),
                            SicknessCountInYear = dto.Sickness.CountInYear,
                            SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit
                                = dto.Sickness.IsAllowedToSubtractFromDemandedDismissalAfterLimit
                        };
                        _sicknessDismissalRepository.Insert(sicknessDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.WithoutSalary:
                    if (dto.WithoutSalary != null)
                    {
                        var withoutSalaryDismissal = new WithoutSalaryDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.WithoutSalary,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            WithoutSalaryAllowanceInMonth = dto.WithoutSalary.AllowanceInMonth
                                .GetSecondsFromDuration(),
                            WithoutSalaryCountInMonth = dto.WithoutSalary.CountInMonth
                        };
                        _withoutSalaryDismissalRepository.Insert(withoutSalaryDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.Encouragement:
                    if (dto.Encouragement != null)
                    {
                        var encouragementDismissal = new EncouragementDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.Encouragement,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            EncouragementFromDate = dto.Encouragement.FromDate,
                            EncouragementToDate = dto.Encouragement.ToDate,
                            EncouragementConsiderWithoutSalary = dto.Encouragement.ConsiderWithoutSalary
                        };
                        _encouragementDismissalRepository.Insert(encouragementDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.Marriage:
                    if (dto.Marriage != null)
                    {
                        var marriageDismissal = new MarriageDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.Marriage,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            MarriageAllowanceInTotal = dto.Marriage.AllowanceInTotal.GetSecondsFromDuration(),
                            MarriageCountInTotal = dto.Marriage.CountInTotal,
                            MarriageConsiderWithoutSalary = dto.Marriage.ConsiderWithoutSalary,
                            MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.Marriage
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit
                        };
                        _marriageDismissalRepository.Insert(marriageDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.ChildBirth:
                    if (dto.ChildBirth != null)
                    {
                        var childBirthDismissal = new ChildBirthDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.ChildBirth,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            ChildBirthAllowanceInTotal = dto.ChildBirth.AllowanceInTotal.GetSecondsFromDuration(),
                            ChildBirthConsiderWithoutSalary = dto.ChildBirth.ConsiderWithoutSalary,
                            ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.ChildBirth
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit
                        };
                        _childBirthDismissalRepository.Insert(childBirthDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.BreastFeeding:
                    if (dto.BreastFeeding != null)
                    {
                        var breastFeedingDismissal = new BreastFeedingDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.BreastFeeding,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            BreastFeedingAllowanceInTotal = dto.BreastFeeding.AllowanceInTotal
                                .GetSecondsFromDuration(),
                            BreastFeedingAllowanceInDay = dto.BreastFeeding.AllowanceInDay.GetSecondsFromDuration(),
                            BreastFeedingCountInDay = dto.BreastFeeding.CountInDay,
                            BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.BreastFeeding
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit
                        };
                        _breastFeedingDismissalRepository.Insert(breastFeedingDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                case DismissalType.DeathOfRelatives:
                    if (dto.DeathOfRelatives != null)
                    {
                        var deathOfRelativesDismissal = new DeathOfRelativesDismissal
                        {
                            Title = dto.Title,
                            DismissalType = DismissalType.DeathOfRelatives,
                            DismissalSystemType = DismissalSystemType.Customized,
                            DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                            DeathOfRelativesAllowanceInTotal = dto.DeathOfRelatives.AllowanceInTotal
                                .GetSecondsFromDuration(),
                            DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.DeathOfRelatives
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit
                        };
                        _deathOfRelativesDismissalRepository.Insert(deathOfRelativesDismissal);
                    }
                    else
                    {
                        DismissalDetailsError(dto.DismissalType, "create");
                    }
                    break;
                default:
                    break;
            }
        }

        //only for customized dismissals
        public void Update(UpdateDismissalDto dto)
        {
            var dismissal = _dismissalRepository.GetById(dto.Id);
            if (dismissal != null)
            {
                switch (dto.DismissalType)
                {
                    case DismissalType.Demanded:
                        if (dismissal.DismissalType == dto.DismissalType) //update the same type
                        {
                            var demandedDismissal = dismissal as DemandedDismissal;
                            demandedDismissal.Title = dto.Title;
                            demandedDismissal.DismissalType = DismissalType.Demanded;
                            demandedDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            demandedDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            demandedDismissal.DemandedAllowanceInMonth = dto.Demanded.AllowanceInMonth.GetSecondsFromDuration();
                            demandedDismissal.DemandedCountInMonth = dto.Demanded.CountInMonth;
                            demandedDismissal.DemandedIsTransferableToNextMonth = dto.Demanded.IsTransferableToNextMonth;
                            demandedDismissal.DemandedTransferableAllowanceToNextMonth = dto.Demanded.TransferableAllowanceToNextMonth
                                .GetSecondsFromDuration();
                            demandedDismissal.DemandedAllowanceInYear = dto.Demanded.AllowanceInYear.GetSecondsFromDuration();
                            demandedDismissal.DemandedCountInYear = dto.Demanded.CountInYear;
                            demandedDismissal.DemandedIsTransferableToNextYear = dto.Demanded.IsTransferableToNextYear;
                            demandedDismissal.DemandedTransferableAllowanceToNextYear = dto.Demanded.TransferableAllowanceToNextYear
                                .GetSecondsFromDuration();
                            demandedDismissal.DemandedIsAllowedToSave = dto.Demanded.IsAllowedToSave;
                            demandedDismissal.DemandedAllowanceToSave = dto.Demanded.AllowanceToSave.GetSecondsFromDuration();
                            demandedDismissal.DemandedMealTimeIsIncluded = dto.Demanded.MealTimeIsIncluded;
                            demandedDismissal.DemandedDoesDismissalMeansExtraWork = dto.Demanded.DoesDismissalMeansExtraWork;
                            demandedDismissal.DemandedIsNationalHolidysConsideredInDismissal = dto.Demanded.IsNationalHolidysConsideredInDismissal;
                            demandedDismissal.DemandedIsFridaysConsideredInDismissal = dto.Demanded.IsFridaysConsideredInDismissal;
                            demandedDismissal.DemandedAmountOfHoursConsideredDailyDismissal = dto.Demanded.AmountOfHoursConsideredDailyDismissal;

                            _demandedDismissalRepository.Update(demandedDismissal);
                        }
                        else //update entity type -> remove and insert again
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.Sickness:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var sicknessDismissal = dismissal as SicknessDismissal;
                            sicknessDismissal.Title = dto.Title;
                            sicknessDismissal.DismissalType = DismissalType.Sickness;
                            sicknessDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            sicknessDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            sicknessDismissal.SicknessAllowanceInYear = dto.Sickness.AllowanceInYear.GetSecondsFromDuration();
                            sicknessDismissal.SicknessCountInYear = dto.Sickness.CountInYear;
                            sicknessDismissal.SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit
                                = dto.Sickness.IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                            _sicknessDismissalRepository.Update(sicknessDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.WithoutSalary:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var withoutSalaryDismissal = dismissal as WithoutSalaryDismissal;
                            withoutSalaryDismissal.Title = dto.Title;
                            withoutSalaryDismissal.DismissalType = DismissalType.WithoutSalary;
                            withoutSalaryDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            withoutSalaryDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            withoutSalaryDismissal.WithoutSalaryAllowanceInMonth = dto.WithoutSalary.AllowanceInMonth
                                .GetSecondsFromDuration();
                            withoutSalaryDismissal.WithoutSalaryCountInMonth = dto.WithoutSalary.CountInMonth;

                            _withoutSalaryDismissalRepository.Update(withoutSalaryDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.Encouragement:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var encouragementDismissal = dismissal as EncouragementDismissal;
                            encouragementDismissal.Title = dto.Title;
                            encouragementDismissal.DismissalType = DismissalType.Encouragement;
                            encouragementDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            encouragementDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            encouragementDismissal.EncouragementFromDate = dto.Encouragement.FromDate;
                            encouragementDismissal.EncouragementToDate = dto.Encouragement.ToDate;
                            encouragementDismissal.EncouragementConsiderWithoutSalary = dto.Encouragement.ConsiderWithoutSalary;

                            _encouragementDismissalRepository.Update(encouragementDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.Marriage:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var marriageDismissal = dismissal as MarriageDismissal;
                            marriageDismissal.Title = dto.Title;
                            marriageDismissal.DismissalType = DismissalType.Marriage;
                            marriageDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            marriageDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            marriageDismissal.MarriageAllowanceInTotal = dto.Marriage.AllowanceInTotal.GetSecondsFromDuration();
                            marriageDismissal.MarriageCountInTotal = dto.Marriage.CountInTotal;
                            marriageDismissal.MarriageConsiderWithoutSalary = dto.Marriage.ConsiderWithoutSalary;
                            marriageDismissal.MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.Marriage
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                            _marriageDismissalRepository.Update(marriageDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.ChildBirth:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var childBirthDismissal = dismissal as ChildBirthDismissal;
                            childBirthDismissal.Title = dto.Title;
                            childBirthDismissal.DismissalType = DismissalType.ChildBirth;
                            childBirthDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            childBirthDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            childBirthDismissal.ChildBirthAllowanceInTotal = dto.ChildBirth.AllowanceInTotal.GetSecondsFromDuration();
                            childBirthDismissal.ChildBirthConsiderWithoutSalary = dto.ChildBirth.ConsiderWithoutSalary;
                            childBirthDismissal.ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.ChildBirth
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                            _childBirthDismissalRepository.Update(childBirthDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.BreastFeeding:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var breastFeedingDismissal = dismissal as BreastFeedingDismissal;
                            breastFeedingDismissal.Title = dto.Title;
                            breastFeedingDismissal.DismissalType = DismissalType.BreastFeeding;
                            breastFeedingDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            breastFeedingDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            breastFeedingDismissal.BreastFeedingAllowanceInTotal = dto.BreastFeeding.AllowanceInTotal
                                .GetSecondsFromDuration();
                            breastFeedingDismissal.BreastFeedingAllowanceInDay = dto.BreastFeeding.AllowanceInDay.GetSecondsFromDuration();
                            breastFeedingDismissal.BreastFeedingCountInDay = dto.BreastFeeding.CountInDay;
                            breastFeedingDismissal.BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.BreastFeeding
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                            _breastFeedingDismissalRepository.Update(breastFeedingDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    case DismissalType.DeathOfRelatives:
                        if (dismissal.DismissalType == dto.DismissalType)
                        {
                            var deathOfRelativesDismissal = dismissal as DeathOfRelativesDismissal;
                            deathOfRelativesDismissal.Title = dto.Title;
                            deathOfRelativesDismissal.DismissalType = DismissalType.DeathOfRelatives;
                            deathOfRelativesDismissal.DismissalSystemType = DismissalSystemType.Customized;
                            deathOfRelativesDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                            deathOfRelativesDismissal.DeathOfRelativesAllowanceInTotal = dto.DeathOfRelatives.AllowanceInTotal
                                .GetSecondsFromDuration();
                            deathOfRelativesDismissal.DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.DeathOfRelatives
                                .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                            _deathOfRelativesDismissalRepository.Update(deathOfRelativesDismissal);
                        }
                        else
                        {
                            try
                            {
                                DeleteAndInsertAgain(dto);
                            }
                            catch (TransactionAbortedException ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message
                                    + " Either update or delete operation failed.");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogRunTimeError(ex, ex.Message);
                                throw;
                            }
                        }
                        break;
                    default:
                        break;
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
                    _logger.LogLogicalError
                        (ex, "Dismissal entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        //only for default dismissals
        public CustomResult UpdateDefault(UpdateDismissalDto dto)
        {
            var dismissal = _dismissalRepository.Get(q => q.DismissalType == dto.DismissalType
                && q.DismissalSystemType == DismissalSystemType.Default).SingleOrDefault();
            if (dismissal.DismissalType != dto.DismissalType)
            {
                return new CustomResult
                {
                    IsValid = false,
                    Message = "default dismissal is not editable"
                };
            }
            if (dismissal != null)
            {
                switch (dto.DismissalType)
                {
                    case DismissalType.Demanded:
                        var demandedDismissal = dismissal as DemandedDismissal;
                        demandedDismissal.DismissalType = DismissalType.Demanded;
                        demandedDismissal.DismissalSystemType = DismissalSystemType.Default;
                        demandedDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        demandedDismissal.DemandedAllowanceInMonth = dto.Demanded.AllowanceInMonth.GetSecondsFromDuration();
                        demandedDismissal.DemandedCountInMonth = dto.Demanded.CountInMonth;
                        demandedDismissal.DemandedIsTransferableToNextMonth = dto.Demanded.IsTransferableToNextMonth;
                        demandedDismissal.DemandedTransferableAllowanceToNextMonth = dto.Demanded.TransferableAllowanceToNextMonth
                            .GetSecondsFromDuration();
                        demandedDismissal.DemandedAllowanceInYear = dto.Demanded.AllowanceInYear.GetSecondsFromDuration();
                        demandedDismissal.DemandedCountInYear = dto.Demanded.CountInYear;
                        demandedDismissal.DemandedIsTransferableToNextYear = dto.Demanded.IsTransferableToNextYear;
                        demandedDismissal.DemandedTransferableAllowanceToNextYear = dto.Demanded.TransferableAllowanceToNextYear
                            .GetSecondsFromDuration();
                        demandedDismissal.DemandedIsAllowedToSave = dto.Demanded.IsAllowedToSave;
                        demandedDismissal.DemandedAllowanceToSave = dto.Demanded.AllowanceToSave.GetSecondsFromDuration();
                        demandedDismissal.DemandedMealTimeIsIncluded = dto.Demanded.MealTimeIsIncluded;
                        demandedDismissal.DemandedDoesDismissalMeansExtraWork = dto.Demanded.DoesDismissalMeansExtraWork;
                        demandedDismissal.DemandedIsNationalHolidysConsideredInDismissal = dto.Demanded.IsNationalHolidysConsideredInDismissal;
                        demandedDismissal.DemandedIsFridaysConsideredInDismissal = dto.Demanded.IsFridaysConsideredInDismissal;
                        demandedDismissal.DemandedAmountOfHoursConsideredDailyDismissal = dto.Demanded.AmountOfHoursConsideredDailyDismissal;

                        _demandedDismissalRepository.Update(demandedDismissal);
                        break;
                    case DismissalType.Sickness:
                        var sicknessDismissal = dismissal as SicknessDismissal;
                        sicknessDismissal.DismissalType = DismissalType.Sickness;
                        sicknessDismissal.DismissalSystemType = DismissalSystemType.Default;
                        sicknessDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        sicknessDismissal.SicknessAllowanceInYear = dto.Sickness.AllowanceInYear.GetSecondsFromDuration();
                        sicknessDismissal.SicknessCountInYear = dto.Sickness.CountInYear;
                        sicknessDismissal.SicknessIsAllowedToSubtractFromDemandedDismissalAfterLimit
                            = dto.Sickness.IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                        _sicknessDismissalRepository.Update(sicknessDismissal);
                        break;
                    case DismissalType.WithoutSalary:
                        var withoutSalaryDismissal = dismissal as WithoutSalaryDismissal;
                        withoutSalaryDismissal.DismissalType = DismissalType.WithoutSalary;
                        withoutSalaryDismissal.DismissalSystemType = DismissalSystemType.Default;
                        withoutSalaryDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        withoutSalaryDismissal.WithoutSalaryAllowanceInMonth = dto.WithoutSalary.AllowanceInMonth
                            .GetSecondsFromDuration();
                        withoutSalaryDismissal.WithoutSalaryCountInMonth = dto.WithoutSalary.CountInMonth;

                        _withoutSalaryDismissalRepository.Update(withoutSalaryDismissal);
                        break;
                    case DismissalType.Encouragement:
                        var encouragementDismissal = dismissal as EncouragementDismissal;
                        encouragementDismissal.DismissalType = DismissalType.Encouragement;
                        encouragementDismissal.DismissalSystemType = DismissalSystemType.Default;
                        encouragementDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        encouragementDismissal.EncouragementFromDate = dto.Encouragement.FromDate;
                        encouragementDismissal.EncouragementToDate = dto.Encouragement.ToDate;
                        encouragementDismissal.EncouragementConsiderWithoutSalary = dto.Encouragement.ConsiderWithoutSalary;

                        _encouragementDismissalRepository.Update(encouragementDismissal);
                        break;
                    case DismissalType.Marriage:
                        var marriageDismissal = dismissal as MarriageDismissal;
                        marriageDismissal.DismissalType = DismissalType.Marriage;
                        marriageDismissal.DismissalSystemType = DismissalSystemType.Default;
                        marriageDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        marriageDismissal.MarriageAllowanceInTotal = dto.Marriage.AllowanceInTotal.GetSecondsFromDuration();
                        marriageDismissal.MarriageCountInTotal = dto.Marriage.CountInTotal;
                        marriageDismissal.MarriageConsiderWithoutSalary = dto.Marriage.ConsiderWithoutSalary;
                        marriageDismissal.MarriageIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.Marriage
                            .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                        _marriageDismissalRepository.Update(marriageDismissal);
                        break;
                    case DismissalType.ChildBirth:
                        var childBirthDismissal = dismissal as ChildBirthDismissal;
                        childBirthDismissal.DismissalType = DismissalType.ChildBirth;
                        childBirthDismissal.DismissalSystemType = DismissalSystemType.Default;
                        childBirthDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        childBirthDismissal.ChildBirthAllowanceInTotal = dto.ChildBirth.AllowanceInTotal.GetSecondsFromDuration();
                        childBirthDismissal.ChildBirthConsiderWithoutSalary = dto.ChildBirth.ConsiderWithoutSalary;
                        childBirthDismissal.ChildBirthIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.ChildBirth
                            .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                        _childBirthDismissalRepository.Update(childBirthDismissal);
                        break;
                    case DismissalType.BreastFeeding:
                        var breastFeedingDismissal = dismissal as BreastFeedingDismissal;
                        breastFeedingDismissal.DismissalType = DismissalType.BreastFeeding;
                        breastFeedingDismissal.DismissalSystemType = DismissalSystemType.Default;
                        breastFeedingDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        breastFeedingDismissal.BreastFeedingAllowanceInTotal = dto.BreastFeeding.AllowanceInTotal
                            .GetSecondsFromDuration();
                        breastFeedingDismissal.BreastFeedingAllowanceInDay = dto.BreastFeeding.AllowanceInDay.GetSecondsFromDuration();
                        breastFeedingDismissal.BreastFeedingCountInDay = dto.BreastFeeding.CountInDay;
                        breastFeedingDismissal.BreastFeedingIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.BreastFeeding
                            .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                        _breastFeedingDismissalRepository.Update(breastFeedingDismissal);
                        break;
                    case DismissalType.DeathOfRelatives:
                        var deathOfRelativesDismissal = dismissal as DeathOfRelativesDismissal;
                        deathOfRelativesDismissal.DismissalType = DismissalType.DeathOfRelatives;
                        deathOfRelativesDismissal.DismissalSystemType = DismissalSystemType.Default;
                        deathOfRelativesDismissal.DismissalExcessiveReaction = dto.DismissalExcessiveReaction;
                        deathOfRelativesDismissal.DeathOfRelativesAllowanceInTotal = dto.DeathOfRelatives.AllowanceInTotal
                            .GetSecondsFromDuration();
                        deathOfRelativesDismissal.DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit = dto.DeathOfRelatives
                            .IsAllowedToSubtractFromDemandedDismissalAfterLimit;

                        _deathOfRelativesDismissalRepository.Update(deathOfRelativesDismissal);
                        break;
                    default:
                        break;
                }

                return new CustomResult
                {
                    IsValid = true
                };
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError
                        (ex, "Dismissal entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateDismissalDto PrepareForPartialUpdate(int id)
        {
            var dismissal = _dismissalRepository.GetById(id);
            if (dismissal != null)
            {
                return new PartialUpdateDismissalDto
                {
                    PatchDto = Mapper.Map<DismissalPatchDto>(dismissal),
                    DismissalEntity = dismissal
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateDismissalDto dto)
        {
            dto.DismissalEntity.Title = dto.PatchDto.Title;

            _dismissalRepository.Update(dto.DismissalEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var dismissal = _dismissalRepository.GetById(id);
            if (dismissal != null)
            {
                _dismissalRepository.Delete(dismissal, deleteState);
            }
            else
            {
                try
                {
                    throw new LogicalException();
                }
                catch (LogicalException ex)
                {
                    _logger.LogLogicalError
                        (ex, "Dismissal entity with the id: '{0}', is not available." +
                        " delete operation failed.", id);
                    throw;
                }
            }
        }

        public int DeleteAll(string items)
        {
            try
            {
                var idsToRemove = items.ParseToIntArray().ToList();
                idsToRemove.ForEach(i => _dismissalRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }

        #region Helpers
        private void DeleteAndInsertAgain(UpdateDismissalDto dto)
        {
            using (var scope = new TransactionScope())
            {
                _dismissalRepository.Delete(dto.Id, DeleteState.Permanent);
                Create(new CreateDismissalDto
                {
                    Title = dto.Title,
                    DismissalType = dto.DismissalType,
                    DismissalSystemType = DismissalSystemType.Customized,
                    DismissalExcessiveReaction = dto.DismissalExcessiveReaction,
                    Demanded = dto.Demanded,
                    Sickness = dto.Sickness,
                    WithoutSalary = dto.WithoutSalary,
                    Encouragement = dto.Encouragement,
                    Marriage = dto.Marriage,
                    ChildBirth = dto.ChildBirth,
                    BreastFeeding = dto.BreastFeeding,
                    DeathOfRelatives = dto.DeathOfRelatives
                });

                scope.Complete();
            }
        }
        private void DismissalDetailsError(DismissalType dismissalType, string operation)
        {
            try
            {
                throw new LogicalException();
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, "The Dismissal dto that has passed to " +
                    "the service should also have '{0}' dismissal properties." +
                    " {1} operation failed.", dismissalType, operation);
                throw;
            }
        }
        #endregion
    }
}
