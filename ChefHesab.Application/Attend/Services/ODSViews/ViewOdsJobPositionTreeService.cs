using AutoMapper;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmployee;
using Ksc.HR.DTO.ODSViews.ViewOdsJobPositionTree;
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
    public class ViewOdsJobPositionTreeService : IViewOdsJobPositionTreeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ViewOdsJobPositionTreeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler filterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = filterHandler;
        }

        public List<MeritJobViewModel> GetViewOdsJobPositionTree()
        {
            var query = _kscHrUnitOfWork.ViewOdsJobPositionTreeRepository.GetViewOdsJobPositionTree();
            //var result = _FilterHandler.GetFilterResult<ViewOdsJobPositionTree>(query, filter, nameof(MeritJobViewModel.id));
            var finalData = query.Select(x => new MeritJobViewModel
            {
                parentId = x.JobPositionCodeParent,
                id = x.JobPositionCode,
                ChildDes = x.DesPosJpos,
                //ParentDes = x.DesParentPost,
                //hasChild = x.HasChild,
                //TOT_NRM_PER_JPOS = x.TotNrmPerJpos,
                //TOT_SHFT_PER_JPOS = x.TotShftPerJpos,// x.TOT_SHAR_PER_JPOS,
                //TOT_SHAR_PER_JPOS = x.TotSharPerJpos,
                //TOT_SHAR_PER_SHFT_JPOS = x.TotSharPerShftJpos,
                //RASMI = x.Rasmi,
                //GHARDADI = x.Ghardadi
            });
            var data = _mapper.Map<List<MeritJobViewModel>>(finalData);
            return data;
        }

      
    }
}
