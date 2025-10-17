using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.WorkDayType;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class WorkDayTypeService : IWorkDayTypeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public WorkDayTypeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool ExistsByTitle(int id, string title)
        {
            return _kscHrUnitOfWork.WorkDayTypeRepository.Any(x => x.Id != id == true && x.Title == title);
        }
        public bool ExistsByTitle(string title)
        {
            return _kscHrUnitOfWork.WorkDayTypeRepository.Any(x => x.Title == title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddWorkDayType(AddWorkDayTypeModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            if (ExistsByTitle(model.Title) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            if (ExistsByCode(model.Id, model.Code) == true)
            {
                result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
                return result;
            }
            if (model.IsOfficialHoliday == true && model.IsHoliday == false)
            {
                result.AddError("رکورد نامعتبر", "نوع روز تعطیلی نیست،امکان انتخاب تعطیل رسمی وجود ندارد");
                return result;
            }
            var WorkDayType = _mapper.Map<WorkDayType>(model);
            WorkDayType.InsertDate = DateTime.Now;
            WorkDayType.InsertUser = model.CurrentUserName;
            WorkDayType.DomainName = model.DomainName;
            WorkDayType.IsActive = true;
            WorkDayType.UseInRollCall = model.UseInRollCall;
            WorkDayType.IsOfficialHoliday = model.IsOfficialHoliday;
            WorkDayType.IsHoliday = model.IsHoliday;
            _kscHrUnitOfWork.WorkDayTypeRepository.Add(WorkDayType);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateWorkDayType(EditWorkDayTypeModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneWorkDayType = GetOne(model.Id);
            if (oneWorkDayType == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            //
            if (ExistsByTitle(model.Id, model.Title) == true)
            {
                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            if (ExistsByCode(model.Id, model.Code) == true)
            {
                result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
                return result;
            }
            if (model.IsOfficialHoliday == true && model.IsHoliday == false)
            {
                result.AddError("رکورد نامعتبر", "نوع روز تعطیلی نیست،امکان انتخاب تعطیل رسمی وجود ندارد");
                return result;
            }
            //

            oneWorkDayType.Code = model.Code;
            oneWorkDayType.Title = model.Title;
            oneWorkDayType.UpdateUser = model.CurrentUserName;
            oneWorkDayType.IsActive = model.IsActive;
            oneWorkDayType.UseInRollCall = model.UseInRollCall;
            oneWorkDayType.IsHoliday = model.IsHoliday;
            oneWorkDayType.IsOfficialHoliday = model.IsOfficialHoliday;

            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveWorkDayType(EditWorkDayTypeModel model)
        {

            var result = new KscResult();
            var item = GetOne(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;


            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<WorkDayTypeModel> GetWorkDayType()
        {
            var workDayTypes = _kscHrUnitOfWork.WorkDayTypeRepository.GetAllQueryable();
            return _mapper.Map<List<WorkDayTypeModel>>(workDayTypes);

        }
        public FilterResult<WorkDayTypeModel> GetWorkDayTypeByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<WorkDayType>(_kscHrUnitOfWork.WorkDayTypeRepository.GetAllQueryable().AsQueryable(), Filter, "Id");

            var WorkDayTypeFiltred = result.Data.Select(a => new WorkDayTypeModel()
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                IsActive = a.IsActive,
                UseInRollCall = a.UseInRollCall,
                IsHoliday = a.IsHoliday,
                IsOfficialHoliday = a.IsOfficialHoliday
            }).ToList();

            return new FilterResult<WorkDayTypeModel>()
            {
                Data = WorkDayTypeFiltred,
                Total = result.Total

            };
        }


        public WorkDayType GetOne(int id)
        {
            return _kscHrUnitOfWork.WorkDayTypeRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public EditWorkDayTypeModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditWorkDayTypeModel>(model);
        }



        public List<SearchWorkDayTypeModel> GetWorkDayTypesByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<WorkDayType>(_kscHrUnitOfWork.WorkDayTypeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchWorkDayTypeModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchWorkDayTypeModel>>(finalData);
        }

        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.WorkDayTypeRepository.Any(x => x.Id != id && x.Code == code);
        }
        public List<SearchWorkDayTypeModel> GetWorkDayTypesUseInRollCallByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<WorkDayType>(_kscHrUnitOfWork.WorkDayTypeRepository.WhereQueryable(a => a.IsActive && a.UseInRollCall).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchWorkDayTypeModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchWorkDayTypeModel>>(finalData);
        }
    }
}
