using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.InvalidDayTypeInForcedOvertime;
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
    public class InvalidDayTypeInForcedOvertimeService : IInvalidDayTypeInForcedOvertimeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public InvalidDayTypeInForcedOvertimeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        //public void ExistsByTitle(int id, string title)
        //{
        //    if (_kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.Any(x => x.Id != id == true && x.Title == title))
        //        throw new HRBusinessException(Validations.RepetitiveId,
        //            String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        //}
        //public void ExistsByTitle(string title)
        //{
        //    if (_kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.Any(x => x.Title == title))
        //        throw new HRBusinessException(Validations.RepetitiveId,
        //            String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddInvalidDayTypeInForcedOvertime(AddInvalidDayTypeInForcedOvertimeModel model)
        {
            var result = new KscResult();
            //ExistsByTitle(model.Title);
            //ExistsByCode(model.Id, model.Code);
            if (_kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.Any(x => x.Id != model.Id && x.WorkDayTypeId == model.WorkDayTypeId && x.WorkTimeId == model.WorkTimeId))
            {
                throw new HRBusinessException("رکورد غیرمعتبر", "اطلاعات تکراری است");

            }
            var InvalidDayTypeInForcedOvertime = _mapper.Map<InvalidDayTypeInForcedOvertime>(model);
            InvalidDayTypeInForcedOvertime.IsActive = true;
            _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.Add(InvalidDayTypeInForcedOvertime);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateInvalidDayTypeInForcedOvertime(EditInvalidDayTypeInForcedOvertimeModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneInvalidDayTypeInForcedOvertime = GetOne(model.Id);
            if (oneInvalidDayTypeInForcedOvertime == null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
            }
            //
            //ExistsByTitle(model.Id, model.Title);
            //ExistsByCode(model.Id, model.Code);
            //
            if(_kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.Any(x=>x.Id!=model.Id && x.WorkDayTypeId == model.WorkDayTypeId && x.WorkTimeId == model.WorkTimeId))
            {
                throw new HRBusinessException("رکورد غیرمعتبر", "اطلاعات تکراری است");

            }
            oneInvalidDayTypeInForcedOvertime.WorkDayTypeId = model.WorkDayTypeId;
            oneInvalidDayTypeInForcedOvertime.WorkTimeId = model.WorkTimeId;
            oneInvalidDayTypeInForcedOvertime.UpdateUser = model.CurrentUserName;
            oneInvalidDayTypeInForcedOvertime.UpdateDate = System.DateTime.Now;
            oneInvalidDayTypeInForcedOvertime.IsActive = model.IsActive;
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        //public async Task<KscResult> RemoveInvalidDayTypeInForcedOvertime(EditInvalidDayTypeInForcedOvertimeModel model)
        //{

        //    var result = new KscResult();
        //    //if (!result.Success)
        //    //    return result;
        //    var item = GetOne(model.Id);
        //    if (item == null)
        //    {
        //        throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
        //    }

        //    item.UpdateDate = DateTime.Now;
        //    item.UpdateUser = model.CurrentUserName;

        //   await _kscHrUnitOfWork.SaveAsync();
        //    return result;
        //}

        //public List<InvalidDayTypeInForcedOvertimeModel> GetInvalidDayTypeInForcedOvertime()
        //{
        //    var cities = _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.GetAll();
        //    return _mapper.Map<List<InvalidDayTypeInForcedOvertimeModel>>(cities);

        //}
        public FilterResult<InvalidDayTypeInForcedOvertimeModel> GetInvalidDayTypeInForcedOvertimeByFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.GetAllQueryable().Include(x => x.WorkDayType).Include(x => x.WorkTime);
            var result = _FilterHandler.GetFilterResult<InvalidDayTypeInForcedOvertime>(query, Filter, "Id");

            var InvalidDayTypeInForcedOvertimeFiltred = result.Data.Select(a => new InvalidDayTypeInForcedOvertimeModel()
            {
                Id = a.Id,
                WorkDayTypeId = a.WorkDayTypeId,
                WorkTimeId = a.WorkTimeId,
                WorkDayTypeTitle = a.WorkDayType.Title,
                WorkTimeTitle = a.WorkTime.Title,
                IsActive = a.IsActive
            }).ToList();

            return new FilterResult<InvalidDayTypeInForcedOvertimeModel>()
            {
                Data = InvalidDayTypeInForcedOvertimeFiltred,
                Total = result.Total

            };
        }


        public InvalidDayTypeInForcedOvertime GetOne(int id)
        {
            return _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.GetAll().First(a => a.Id == id);
        }

        public EditInvalidDayTypeInForcedOvertimeModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditInvalidDayTypeInForcedOvertimeModel>(model);
        }



        //public List<SearchInvalidDayTypeInForcedOvertimeModel> GetInvalidDayTypeInForcedOvertimesByKendoFilter(FilterRequest Filter)
        //{
        //    var result = _FilterHandler.GetFilterResult<InvalidDayTypeInForcedOvertime>(_kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
        //    var finalData = result.Data.Select(a => new SearchInvalidDayTypeInForcedOvertimeModel()
        //    {
        //        Id = a.Id,
        //        Title = a.Title,
        //        Code = a.Code,
        //    }).ToList();
        //    return _mapper.Map<List<SearchInvalidDayTypeInForcedOvertimeModel>>(finalData);
        //}

        //public bool ExistsByCode(int id, string code)
        //{
        //    return _kscHrUnitOfWork.InvalidDayTypeInForcedOvertimeRepository.Any(x => x.Id != id && x.Code == code);
        //}
    }
}
