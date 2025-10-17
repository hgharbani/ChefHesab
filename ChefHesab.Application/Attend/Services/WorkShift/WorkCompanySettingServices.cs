using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.WorkCompanySetting;
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
    public class WorkCompanySettingServices : IWorkCompanySettingServices
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public WorkCompanySettingServices(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        //public void Exists(int? WorktimeId, int? shiftconceptId)
        //{

        //    if (_kscHrUnitOfWork.WorkCompanySettingRepository.Any(x => x.IsActive == true && x.WorkTimeId == WorktimeId && x.ShiftConceptId == shiftconceptId))
        //        throw new HRBusinessException(Validations.RepetitiveId, "عنوان وارد شده موجود می باشد");


        //}

        //public void Exists(int id, int? WorktimeId, int? shiftconceptId)
        //{
        //    if (_kscHrUnitOfWork.WorkCompanySettingRepository.Any(x => x.Id != id && x.IsActive == true && x.WorkTimeId == WorktimeId && x.ShiftConceptId == shiftconceptId))
        //        throw new HRBusinessException(Validations.RepetitiveId, "عنوان وارد شده موجود می باشد");


        //}
        public void Exists(int workTimeShiftConceptId, int workCityId)
        {

            if (_kscHrUnitOfWork.WorkCompanySettingRepository.Any(x => x.IsActive == true && x.WorkTimeShiftConceptId == workTimeShiftConceptId && x.WorkCityId == workCityId))
                throw new HRBusinessException(Validations.RepetitiveId, "عنوان وارد شده موجود می باشد");


        }

        public void Exists(int id, int workTimeShiftConceptId, int workCityId)
        {
            if (_kscHrUnitOfWork.WorkCompanySettingRepository.Any(x => x.Id != id && x.IsActive == true && x.WorkTimeShiftConceptId == workTimeShiftConceptId && x.WorkCityId == workCityId))
                throw new HRBusinessException(Validations.RepetitiveId, "عنوان وارد شده موجود می باشد");


        }

        public FilterResult<SearchWorkCompanySettingModel> GetSettingByModel(FilterRequest Filter)
        {
            var workCompanySettingQuery = _kscHrUnitOfWork.WorkCompanySettingRepository.GetWorkCompanySettingIncluded().Where(a => a.IsActive)
                .Select(a => new SearchWorkCompanySettingModel
                {
                    Id = a.Id,
                    WorkTimeId = a.WorkTimeShiftConcept.WorkTimeId,
                    WorkTimeTitle = a.WorkTimeShiftConcept.WorkTime.Title,
                    CompanyName = a.WorkCity.Company.Title,
                    CityName = a.WorkCity.City.Title,
                    ShiftConceptTitle = a.WorkTimeShiftConcept.ShiftConcept.Title,
                    WorkCompanySettingTitle = (a.WorkTimeShiftConcept.WorkTime.Title) + " -" + (a.WorkTimeShiftConcept.ShiftConcept.Title) + "-" + (a.WorkCity != null ? a.WorkCity.Company.Title + "-" + a.WorkCity.City.Title : ""),
                }).AsQueryable();
            var result = _FilterHandler.GetFilterResult<SearchWorkCompanySettingModel>(workCompanySettingQuery, Filter, nameof(WorkCompanySetting.Id));

            var modelResult = new FilterResult<SearchWorkCompanySettingModel>
            {
                Data = _mapper.Map<List<SearchWorkCompanySettingModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddWorkCompanySetting(AddWorkCompanySettingModel model)
        {
            var result = new KscResult();
            //Exists(model.WorkTimeId, model.ShiftConceptId);
            Exists(model.WorkTimeShiftConceptId, model.WorkCityId.Value);
            var WorkCompanySetting = _mapper.Map<WorkCompanySetting>(model);
            _kscHrUnitOfWork.WorkCompanySettingRepository.Add(WorkCompanySetting);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateWorkCompanySetting(EditWorkCompanySettingModel model)
        {
            var result = new KscResult();
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }

        public int? GetLastSetting()
        {
            var model = _kscHrUnitOfWork.WorkCompanySettingRepository.GetAllQueryable().OrderByDescending(a => a.Id).FirstOrDefault()?.Id;
            return model;
        }
    }
}
