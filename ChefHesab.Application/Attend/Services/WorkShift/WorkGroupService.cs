using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.WorkGroup;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Ksc.HR.Appication.Services.WorkShift
{
    public class WorkGroupService : IWorkGroupService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public WorkGroupService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public List<string> GetLatinAlphabet()
        {
            return new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        }

        public WorkGroup GetWorkGroupByCode(string code)
        {
            return _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().SingleOrDefault(x => x.Code == code);
        }
        public IEnumerable<WorkGroup> GetWorkGroupByWorkTimeId(int id)
        {
            return _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.WorkTimeId == id);
        }
        public FilterResult<WorkGroupListModel> GetWorkGroupByFilter(FilterRequest Filter)
        {
            // var result = _FilterHandler.GetFilterResult<WorkGroup>(_kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().AsQueryable().Include(a => a.WorkTime).Where(a => a.IsActive).AsQueryable(), filter, "Id");
            var Data = _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().AsQueryable().Include(a => a.WorkTime).Where(a => a.IsActive).AsQueryable().Select(a => new WorkGroupListModel()
            {

                Id = a.Id,
                Code = a.Code,
                WorkTimeTitle = a.WorkTime.Title,
                //RepetitionPeriod = a.WorkTime.RepetitionPeriod,
                RepetitionPeriod = a.RepetitionPeriod,
                WorkGroupTitle = a.WorkTime.Title + "-" + a.Code
            }).AsQueryable();
            var result = _FilterHandler.GetFilterResult<WorkGroupListModel>(Data, Filter, nameof(WorkGroup.Code));
            return new FilterResult<WorkGroupListModel>()
            {
                Data = _mapper.Map<List<WorkGroupListModel>>(result.Data.ToList()),
                Total = result.Total

            };
            //workGroupFiltred
            //return new FilterResult<WorkGroupListModel>()
            //{
            //    Data = workGroupFiltred,
            //    Total = result.Total

            //};

        }

        public List<SearchWorkGroupModel> GetWorkGroupByKendoFilter(FilterRequest Filter)
        {

            var query = _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().AsQueryable()
                .Include(x => x.WorkTime)
                .Where(x => x.IsActive == true);
            var result = _FilterHandler.GetFilterResult<WorkGroup>(query, Filter, "Id");
            var finalData = result.Data.Select(a => new SearchWorkGroupModel()
            {
                Id = a.Id,
                Code = a.WorkTime.Title + " _ " + a.Code,
            }).ToList();
            return _mapper.Map<List<SearchWorkGroupModel>>(finalData);
        }

    }
}
