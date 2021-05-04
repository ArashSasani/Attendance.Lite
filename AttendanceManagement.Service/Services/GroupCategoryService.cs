using AttendanceManagement.Core.Model;
using AttendanceManagement.Service.Dtos.GroupCategory;
using AttendanceManagement.Service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.Infrastructure.Paging;
using WebApplication.Infrastructure.Parser;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Service
{
    public class GroupCategoryService : IGroupCategoryService
    {
        private readonly IRepository<GroupCategory> _groupCategoryRepository;

        private readonly IExceptionLogger _logger;

        public GroupCategoryService(IRepository<GroupCategory> groupCategoryRepository
            , IExceptionLogger logger)
        {
            _groupCategoryRepository = groupCategoryRepository;

            _logger = logger;
        }

        public IPaging<GroupCategoryDto> Get(string searchTerm, string sortItem, string sortOrder
            , PagingQueryString pagingQueryString)
        {
            IPaging<GroupCategoryDto> model = new GroupCategoryDtoPagingList();

            var query = !string.IsNullOrEmpty(searchTerm)
                ? _groupCategoryRepository.Get(q => q.Title.Contains(searchTerm.ToLower()))
                : _groupCategoryRepository.Get();
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

            List<GroupCategory> queryResult;
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
            model.PagingList = Mapper.Map<List<GroupCategoryDto>>(queryResult);

            return model;
        }

        public List<GroupCategoryDto> GetForDDL()
        {
            var groupCategories = _groupCategoryRepository.Get().ToList();
            return Mapper.Map<List<GroupCategoryDto>>(groupCategories);
        }

        public GroupCategoryDto GetById(int id)
        {
            var groupCategory = _groupCategoryRepository.GetById(id);
            if (groupCategory == null)
            {
                return null;
            }
            return Mapper.Map<GroupCategoryDto>(groupCategory);
        }

        public void Create(CreateGroupCategoryDto dto)
        {
            var groupCategory = new GroupCategory
            {
                Title = dto.Title
            };

            _groupCategoryRepository.Insert(groupCategory);
        }

        public void Update(UpdateGroupCategoryDto dto)
        {
            var groupCategory = _groupCategoryRepository.GetById(dto.Id);
            if (groupCategory != null)
            {
                groupCategory.Title = dto.Title;

                _groupCategoryRepository.Update(groupCategory);
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
                        (ex, "GroupCategory entity with the id: '{0}', is not available." +
                        " update operation failed.", dto.Id);
                    throw;
                }
            }
        }

        public PartialUpdateGroupCategoryDto PrepareForPartialUpdate(int id)
        {
            var groupCategory = _groupCategoryRepository.GetById(id);
            if (groupCategory != null)
            {
                return new PartialUpdateGroupCategoryDto
                {
                    PatchDto = Mapper.Map<GroupCategoryPatchDto>(groupCategory),
                    GroupCategoryEntity = groupCategory
                };
            }
            else
            {
                return null;
            }
        }

        public void ApplyPartialUpdate(PartialUpdateGroupCategoryDto dto)
        {
            dto.GroupCategoryEntity.Title = dto.PatchDto.Title;

            _groupCategoryRepository.Update(dto.GroupCategoryEntity);
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var groupCategory = _groupCategoryRepository.GetById(id);
            if (groupCategory != null)
            {
                _groupCategoryRepository.Delete(groupCategory, deleteState);
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
                        (ex, "GroupCategory entity with the id: '{0}', is not available." +
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
                idsToRemove.ForEach(i => _groupCategoryRepository.Delete(i, DeleteState.SoftDelete));

                return idsToRemove.Count;
            }
            catch (LogicalException ex)
            {
                _logger.LogRunTimeError(ex, ex.Message);
                throw;
            }
        }
    }
}
