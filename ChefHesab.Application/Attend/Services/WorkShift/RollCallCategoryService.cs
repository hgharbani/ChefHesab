using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
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
    public class RollCallCategoryService : IRollCallCategoryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public RollCallCategoryService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool ExistsByTitle(int id, string title)
        {
            return _kscHrUnitOfWork.RollCallCategoryRepository.Any(x => x.Id != id == true && x.Title == title);
        }
        public bool ExistsByTitle(string title)
        {
            return _kscHrUnitOfWork.RollCallCategoryRepository.Any(x => x.Title == title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddRollCallCategory(AddRollCallCategoryModel model)
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

            var RollCallCategory = _mapper.Map<RollCallCategory>(model);
            RollCallCategory.InsertDate = DateTime.Now;
            RollCallCategory.InsertUser = model.CurrentUserName;
            RollCallCategory.DomainName = model.DomainName;
            RollCallCategory.IsActive = true;
            _kscHrUnitOfWork.RollCallCategoryRepository.Add(RollCallCategory);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateRollCallCategory(EditRollCallCategoryModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneRollCallCategory = GetOne(model.Id);
            if (oneRollCallCategory == null)
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
            //

            oneRollCallCategory.Code = model.Code;
            oneRollCallCategory.Title = model.Title;
            oneRollCallCategory.UpdateUser = model.CurrentUserName;
            oneRollCallCategory.IsActive = model.IsActive;

            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveRollCallCategory(EditRollCallCategoryModel model)
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

            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;


            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<RollCallCategoryModel> GetRollCallCategory()
        {
            var cities = _kscHrUnitOfWork.RollCallCategoryRepository.GetAllQueryable();
            return _mapper.Map<List<RollCallCategoryModel>>(cities);

        }
        public FilterResult<RollCallCategoryModel> GetRollCallCategoryByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<RollCallCategory>(_kscHrUnitOfWork.RollCallCategoryRepository.GetAllQueryable().AsQueryable(), Filter, "Id");

            var RollCallCategoryFiltred = result.Data.Select(a => new RollCallCategoryModel()
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                IsActive = a.IsActive
            }).ToList();

            return new FilterResult<RollCallCategoryModel>()
            {
                Data = RollCallCategoryFiltred,
                Total = result.Total

            };
        }


        public RollCallCategory GetOne(int id)
        {
            return _kscHrUnitOfWork.RollCallCategoryRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public EditRollCallCategoryModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditRollCallCategoryModel>(model);
        }



        public List<SearchRollCallCategoryModel> GetRollCallCategorysByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<RollCallCategory>(_kscHrUnitOfWork.RollCallCategoryRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchRollCallCategoryModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchRollCallCategoryModel>>(finalData);
        }

        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.RollCallCategoryRepository.Any(x => x.Id != id && x.Code == code);
        }
    }
}
