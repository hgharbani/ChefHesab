using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.WorkCompanyUnOfficialHolidaySetting;
using Ksc.HR.Resources.Messages;
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
    public class WorkCompanyUnOfficialHolidaySettingService : IWorkCompanyUnOfficialHolidaySettingService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public WorkCompanyUnOfficialHolidaySettingService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public List<WorkCompanyUnOfficialHolidaySettingModel> GetWorkCompanyUnOfficialHolidaySettings()
        {
            var cities = _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.GetAllQueryable();
            return _mapper.Map<List<WorkCompanyUnOfficialHolidaySettingModel>>(cities);

        }
        public FilterResult<WorkCompanyUnOfficialHolidaySettingModel> GetWorkCompanyUnOfficialHolidaySettingsByFilter(WorkCompanyUnOfficialHolidaySettingModel Filter)
        {
            var query = _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.GetAllWithInclude()
                .Where(x => x.WorkCompanySettingId == Filter.WorkCompanySettingId);


            var result = _FilterHandler.GetFilterResult<WorkCompanyUnOfficialHolidaySetting>(query, Filter, "Id");
            return new FilterResult<WorkCompanyUnOfficialHolidaySettingModel>()
            {
                Data = _mapper.Map<List<WorkCompanyUnOfficialHolidaySettingModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }
        /// <summary>
        /// بررسی تکراری بودن تاریخ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workCalendarId"></param>
        /// <exception cref="HRBusinessException"></exception>
        public void Exists(int id, int workCalendarId, int workCompanySettingId)
        {
            if (_kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.Any(x => x.Id != id == true
                                    && x.WorkCalendarId == workCalendarId
                                    && x.WorkCompanySettingId == workCompanySettingId))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkCompanyUnOfficialHolidaySettingResource.WorkCalendarId));
        }

        public async Task<KscResult> AddUnOfficialHolidaySetting(AddWorkCompanyUnOfficialHolidaySettingModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;


            if (model.WorkCalendarDate.HasValue == false)
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format("تاریخ بایستی پر باشد"));

            if (model.IsValidExtraWork == true && (model.CodeCategoryJobCategorySelected.Any() == false && model.IsValidExtraWorkForAllCategoryCode == false))
                throw new HRBusinessException(Validations.RepetitiveId,
                   String.Format("رده ای تعیین نشده است"));

            //if (model.IsHoliday == false && model.IsValidExtraWork == false)
            //    throw new HRBusinessException(Validations.RepetitiveId,
            //        String.Format("بایستی تعطیل است یا اضافه کار تعلق میگیرد را مشخص نمایید"));
            if (model.IsValidExtraWork == true)
            {
                if (model.IsValidExtraWorkForAllCategoryCode == true && model.CodeCategoryJobCategorySelected.Any() == true)
                    throw new HRBusinessException(Validations.RepetitiveId,
                        String.Format("در صورت انتخاب تمام رده ها ، امکان انتخاب رده وجود ندارد"));

                if (model.IsValidExtraWorkForAllCategoryCode == false && model.CodeCategoryJobCategorySelected.Any() == false)
                    throw new HRBusinessException(Validations.RepetitiveId,
                        String.Format("در صورت عدم انتخاب تمام رده ها ، رده بایستی انتخاب شود"));
            }
            // _mapper.Map<WorkCompanyUnOfficialHolidaySetting>(model);

            try
            {
                var entity = new WorkCompanyUnOfficialHolidaySetting()
                {
                    IsActive = true,
                    InsertDate = DateTime.Now,
                    InsertUser = model.CurrentUserName,
                    DomainName = model.DomainName,
                    IsHoliday = model.IsHoliday,
                    IsValidExtraWork = model.IsValidExtraWork,
                    //IsValidExtraWorkForAllCategoryCode = model.IsValidExtraWorkForAllCategoryCode,
                    WorkCompanySettingId = model.WorkCompanySettingId,
                    UnofficialHolidayReasonId = model.UnofficialHolidayReasonId,
                    WorkCompanyUnOfficialHolidayJobCategories = new List<WorkCompanyUnOfficialHolidayJobCategory>(),
                };
                var wCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.WorkCalendarDate.Value);
                if (wCalendar != null)
                {
                    entity.WorkCalendarId = wCalendar.Id;
                }
                else
                    throw new HRBusinessException(Validations.RepetitiveId,
                        String.Format(Validations.Repetitive, "تاریخ معتبر نمیباشد"));

                Exists(model.Id, entity.WorkCalendarId, model.WorkCompanySettingId);

                if (model.IsValidExtraWork == true)
                {
                    entity.IsValidExtraWorkForAllCategoryCode = model.IsValidExtraWorkForAllCategoryCode;

                    if (model.CodeCategoryJobCategorySelected.Any())
                        foreach (var item in model.CodeCategoryJobCategorySelected)
                        {
                            var temp = new WorkCompanyUnOfficialHolidayJobCategory() { CodeCategoryJobCategory = item };
                            entity.WorkCompanyUnOfficialHolidayJobCategories.Add(temp);
                        }
                }
                _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.Add(entity);


                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                throw new HRBusinessException(Validations.RepetitiveId, e.Message);

            }

            return result;
        }

        public EditWorkCompanyUnOfficialHolidaySettingModel GetEntity(int id)
        {
            var model = _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.GetById(id);
            return _mapper.Map<EditWorkCompanyUnOfficialHolidaySettingModel>(model);
        }

        public EditWorkCompanyUnOfficialHolidaySettingModel GetForEdit(int id)
        {
            var model = GetOne(id);
            var result = _mapper.Map<EditWorkCompanyUnOfficialHolidaySettingModel>(model);
            result.WorkCalendarDate = _kscHrUnitOfWork.WorkCalendarRepository.GetById(result.WorkCalendarId).MiladiDateV1;
            result.CodeCategoryJobCategorySelected = _kscHrUnitOfWork.WorkCompanyUnOfficialHolidayJobCategoryRepository.GetAllQueryable()
                                                      .Where(x => x.WorkCompanyUnOfficialHolidaySettingId == id)
                                                      .Select(x => x.CodeCategoryJobCategory);
            return result;
        }
        public WorkCompanyUnOfficialHolidaySetting GetOne(int id)
        {
            return _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.GetOneWithInclude(id);
        }

        public async Task<KscResult> UpdateWorkCompanyUnOfficialHolidaySetting(EditWorkCompanyUnOfficialHolidaySettingModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            if (model.WorkCalendarDate.HasValue == false)
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format("تاریخ بایستی پر باشد"));

            if (model.IsValidExtraWork == true && (model.CodeCategoryJobCategorySelected.Any() == false && model.IsValidExtraWorkForAllCategoryCode == false))
                throw new HRBusinessException(Validations.RepetitiveId,
                   String.Format("رده ای تعیین نشده است"));


            //if (model.IsHoliday == false && model.IsValidExtraWork == false)
            //    throw new HRBusinessException(Validations.RepetitiveId,
            //        String.Format("بایستی تعطیل است یا اضافه کار تعلق میگیرد را مشخص نمایید"));
            if (model.IsValidExtraWork == true)
            {
                if (model.IsValidExtraWorkForAllCategoryCode == true && model.CodeCategoryJobCategorySelected.Any() == true)
                    throw new HRBusinessException(Validations.RepetitiveId,
                        String.Format("در صورت انتخاب تمام رده ها ، امکان انتخاب رده وجود ندارد"));

                if (model.IsValidExtraWorkForAllCategoryCode == false && model.CodeCategoryJobCategorySelected.Any() == false)
                    throw new HRBusinessException(Validations.RepetitiveId,
                        String.Format("در صورت عدم انتخاب تمام رده ها ، رده بایستی انتخاب شود"));
            }
            // _mapper.Map<WorkCompanyUnOfficialHolidaySetting>(model);

            try
            {
                var workCompanyUnOfficialHolidaySetting = GetOne(model.Id);
                if (workCompanyUnOfficialHolidaySetting == null)
                {
                    result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                    return result;
                }


                workCompanyUnOfficialHolidaySetting.IsActive = model.IsActive;
                workCompanyUnOfficialHolidaySetting.UpdateDate = DateTime.Now;
                workCompanyUnOfficialHolidaySetting.UpdateUser = model.CurrentUserName;
                workCompanyUnOfficialHolidaySetting.IsHoliday = model.IsHoliday;
                workCompanyUnOfficialHolidaySetting.IsValidExtraWork = model.IsValidExtraWork;
                workCompanyUnOfficialHolidaySetting.IsValidExtraWorkForAllCategoryCode = false;
                //workCompanyUnOfficialHolidaySetting.IsValidExtraWorkForAllCategoryCode = model.IsValidExtraWorkForAllCategoryCode;
                workCompanyUnOfficialHolidaySetting.WorkCompanySettingId = model.WorkCompanySettingId;
                workCompanyUnOfficialHolidaySetting.UnofficialHolidayReasonId = model.UnofficialHolidayReasonId;
                

                var wCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.WorkCalendarDate.Value);
                if (wCalendar != null)
                    workCompanyUnOfficialHolidaySetting.WorkCalendarId = wCalendar.Id;
                else
                    throw new HRBusinessException(Validations.RepetitiveId,
                        String.Format(Validations.Repetitive, "تاریخ معتبر نمیباشد"));

                Exists(model.Id, workCompanyUnOfficialHolidaySetting.WorkCalendarId, model.WorkCompanySettingId);

                //حذف رده های قبلی
                foreach (var item in workCompanyUnOfficialHolidaySetting.WorkCompanyUnOfficialHolidayJobCategories.ToList())
                {
                    //workCompanyUnOfficialHolidaySetting.WorkCompanyUnOfficialHolidayJobCategories.Remove(item);
                    _kscHrUnitOfWork.WorkCompanyUnOfficialHolidayJobCategoryRepository.Delete(item);
                }
                if (model.IsValidExtraWork == true)
                {
                    workCompanyUnOfficialHolidaySetting.IsValidExtraWorkForAllCategoryCode = model.IsValidExtraWorkForAllCategoryCode;
                    //افزودن رده های روی صفحه
                    if (model.IsValidExtraWorkForAllCategoryCode == false)
                    {
                        if (model.CodeCategoryJobCategorySelected.Any())
                        {
                            foreach (var item in model.CodeCategoryJobCategorySelected)
                            {
                                var temp = new WorkCompanyUnOfficialHolidayJobCategory() { CodeCategoryJobCategory = item };
                                workCompanyUnOfficialHolidaySetting.WorkCompanyUnOfficialHolidayJobCategories.Add(temp);

                            }
                        }
                    }
                }

                _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.Update(workCompanyUnOfficialHolidaySetting);

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                throw new HRBusinessException(Validations.RepetitiveId, e.Message);

            }

            return result;
        }

        public async Task<KscResult> RemoveWorkCompanyUnOfficialHolidaySetting(EditWorkCompanyUnOfficialHolidaySettingModel model)
        {
            var result = new KscResult();
            //if (!result.Success)
            //    return result;
            var item = _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.GetOneWithInclude(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            foreach (var subitem in item.WorkCompanyUnOfficialHolidayJobCategories)
            {
                _kscHrUnitOfWork.WorkCompanyUnOfficialHolidayJobCategoryRepository.Delete(subitem);
            }

            _kscHrUnitOfWork.WorkCompanyUnOfficialHolidaySettingRepository.Delete(item);

            try
            {
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }

            return result;
        }

    }
}
