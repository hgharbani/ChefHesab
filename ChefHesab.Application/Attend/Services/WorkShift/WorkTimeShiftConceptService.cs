using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.WorkTimeShiftConcept;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class WorkTimeShiftConceptService : IWorkTimeShiftConceptService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public WorkTimeShiftConceptService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public FilterResult<SearchWorkTimeShiftConceptModel> GetWorkTimeShiftConceptByModel(FilterRequest Filter)
        {
            var WorkTimeShiftConceptQuery = _kscHrUnitOfWork.WorkTimeShiftConceptRepository.GetAllByIncluded().Where(a => a.IsActive)
                .Select(a => new SearchWorkTimeShiftConceptModel
                {
                    Id = a.Id,
                    WorkTimeTitle = a.WorkTime.Title,
                    ShiftConceptTitle = a.ShiftConcept.Title,
                    WorkTimeShiftConcepTitle = a.WorkTime.Title + "-" + a.ShiftConcept.Title,
                }).AsQueryable();
            var result = _FilterHandler.GetFilterResult<SearchWorkTimeShiftConceptModel>(WorkTimeShiftConceptQuery, Filter, nameof(WorkTimeShiftConcept.Id));

            var modelResult = new FilterResult<SearchWorkTimeShiftConceptModel>
            {
                Data = _mapper.Map<List<SearchWorkTimeShiftConceptModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;


        }


    }
}
