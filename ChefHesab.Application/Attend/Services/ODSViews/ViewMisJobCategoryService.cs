using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisJobCategory;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.ODSViews
{
    public class ViewMisJobCategoryService : IViewMisJobCategoryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisJobCategoryService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public List<SearchViewMisJobCategoryModel> GetAllViewMisJobCategoryByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ViewMisJobCategory>(_kscHrUnitOfWork.ViewMisJobCategoryRepository.GetAllQueryable().AsQueryable(), Filter, nameof(ViewMisJobCategory.CodeCategoryJobCategory));
            return _mapper.Map<List<SearchViewMisJobCategoryModel>>(result.Data.DistinctBy(i=>i.CodeCategoryJobCategory).ToList());
        }

        public FilterResult<SearchViewMisJobCategoryModel> GetViewMisJobCategoryByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ViewMisJobCategory>(_kscHrUnitOfWork.ViewMisJobCategoryRepository.GetAllQueryable().AsQueryable(), Filter, nameof(ViewMisJobCategory.CodeCategoryJobCategory));
            return new FilterResult<SearchViewMisJobCategoryModel>()
            {
                Data = _mapper.Map<List<SearchViewMisJobCategoryModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public List<SearchViewMisJobCategoryModel> GetCodeCategoryJobCategoryByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.ViewMisJobCategoryRepository.GetAllQueryable().AsQueryable().Select(x => x.CodeCategoryJobCategory).Distinct();
            //var result = _FilterHandler.GetFilterResult<string>(query, Filter, "JobCategoryCode");
            var finalData = query.Select(a => new SearchViewMisJobCategoryModel()
            {
                CodeCategoryJobCategory = a,
                
            }).ToList();
            return _mapper.Map<List<SearchViewMisJobCategoryModel>>(finalData);
        }


    }
}
