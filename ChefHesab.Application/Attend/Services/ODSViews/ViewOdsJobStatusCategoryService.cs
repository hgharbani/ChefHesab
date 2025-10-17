using AutoMapper;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmployee;
using Ksc.HR.DTO.ODSViews.ViewOdsJobStatusCategory;
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
    public class ViewOdsJobStatusCategoryService : IViewOdsJobStatusCategoryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ViewOdsJobStatusCategoryService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler filterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = filterHandler;
        }

        public List<ViewOdsJobStatusCategoryModel> GetJobStatusCategory()
        {
            var query = _kscHrUnitOfWork.ViewOdsJobStatusCategoryRepository.GetViewOdsJobStatusCategory();
            //var result = _FilterHandler.GetFilterResult<ViewOdsJobStatusCategory>(query, filter, nameof(MeritJobViewModel.id));
            var finalData = query.Select(x => new ViewOdsJobStatusCategoryModel
            {
                CategoryId = x.CategoryId,
                CategoryTitle =x.CategoryTitle
            });
            var data = _mapper.Map<List<ViewOdsJobStatusCategoryModel>>(finalData);
            return data;
        }
    }
}
