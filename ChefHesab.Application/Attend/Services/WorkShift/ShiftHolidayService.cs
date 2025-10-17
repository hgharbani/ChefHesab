using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.ShiftHoliday;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Ksc.HR.DTO.Other;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Extention;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class ShiftHolidayService : IShiftHolidayService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ShiftHolidayService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, int dayNumber)
        {
            return _kscHrUnitOfWork.ShiftHolidayRepository.Any(x => x.Id != id && x.IsActive == true && x.DayNumber == dayNumber);
        }
        public bool Exists(int dayNumber)
        {
            return _kscHrUnitOfWork.ShiftHolidayRepository.Any(x => x.IsActive == true && x.DayNumber == dayNumber);
        }
        public bool ExistsByWorkCompany(int workCompanySettingId, int dayNumber)
        {
            return _kscHrUnitOfWork.ShiftHolidayRepository.Any(x => x.WorkCompanySettingId == workCompanySettingId && x.IsActive == true && x.DayNumber == dayNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddShiftHoliday(AddShiftHolidayModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            if (ExistsByWorkCompany(model.WorkCompanySettingId, (int)model.DayNumber) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }


            var ShiftHoliday = _mapper.Map<ShiftHoliday>(model);
            ShiftHoliday.InsertDate = DateTime.Now;
            ShiftHoliday.InsertUser = model.CurrentUserName;
            ShiftHoliday.DomainName = model.DomainName;
            ShiftHoliday.IsActive = true;

            _kscHrUnitOfWork.ShiftHolidayRepository.Add(ShiftHoliday);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public KscResult UpdateShiftHoliday(EditShiftHolidayModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneShiftHoliday = GetOne(model.Id);
            if (oneShiftHoliday == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            if (ExistsByWorkCompany(model.WorkCompanySettingId, (int)model.DayNumber) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            //
            var ShiftHoliday = _mapper.Map<ShiftHoliday>(model);
            ShiftHoliday.UpdateDate = DateTime.Now;
            ShiftHoliday.UpdateUser = model.CurrentUserName;
            ShiftHoliday.IsActive = true;

            _kscHrUnitOfWork.ShiftHolidayRepository.Update(ShiftHoliday);
            _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public KscResult RemoveShiftHoliday(EditShiftHolidayModel model)
        {

            var result = new KscResult();
            //if (!result.Success)
            //    return result;
            var item = GetOne(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            item.IsActive = false;
            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;


            _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<ShiftHolidayModel> GetShiftHoliday()
        {
            var cities = _kscHrUnitOfWork.ShiftHolidayRepository.GetAllQueryable();
            return _mapper.Map<List<ShiftHolidayModel>>(cities);

        }
        public FilterResult<ShiftHolidayModel> GetShiftHolidayByFilter(ShiftHolidayModel Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftHoliday>(_kscHrUnitOfWork.ShiftHolidayRepository.GetAllQueryable().AsQueryable().Include(a => a.WorkCompanySetting).Where(a => a.IsActive && a.WorkCompanySettingId == Filter.WorkCompanySettingId).AsQueryable(), Filter, "Id");

            return new FilterResult<ShiftHolidayModel>()
            {
                Data = _mapper.Map<List<ShiftHolidayModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public List<SelectListItem> GetListNumberType()
        {
            var dayNumberType = new DayNumberType();
            var result = new List<SelectListItem>();
            var itemValues = typeof(DayNumberType).GetEnumValues();

            var i = 0;
            foreach (var name in itemValues)
            {
                result.Add(new SelectListItem()
                {
                    Text = name.ToString().GetDisplayName(dayNumberType),
                    Value = ((int)name).ToString()
                });
            }

            return result;
        }

        public ShiftHoliday GetOne(int id)
        {
            return _kscHrUnitOfWork.ShiftHolidayRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }

        public EditShiftHolidayModel GetForEdit(int id)
        {
            var data = GetOne(id);
            var model = _mapper.Map<EditShiftHolidayModel>(data);
            return model;

        }

        public List<SearchShiftHolidayModel> GetWorkByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftHoliday>(_kscHrUnitOfWork.ShiftHolidayRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchShiftHolidayModel>>(result.Data.ToList());
        }
    }
}
