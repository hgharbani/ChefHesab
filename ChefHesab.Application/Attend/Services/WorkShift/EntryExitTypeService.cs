using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.EntryExitType;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class EntryExitTypeService : IEntryExitTypeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public EntryExitTypeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, string title)
        {
            return _kscHrUnitOfWork.EntryExitTypeRepository.Any(x => x.Id != id && x.Title == title);
        }
        public bool Exists(string title)
        {
            return _kscHrUnitOfWork.EntryExitTypeRepository.Any(x => x.Title == title);
            //return _kscHrUnitOfWork.EntryExitTypeRepository.Any(x => x.IsActive == true && x.Title == title);
        }
        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.EntryExitTypeRepository.Any(x => x.Id != id && x.Code == code);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public KscResult AddEntryExitType(AddOrEditEntryExitTypeModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            //if (Exists(model.Title) == true)
            //{

            //    result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
            //    return result;
            //}
            if (ExistsByCode(model.Id, model.Code) == true)
            {

                result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
                return result;
            }

            if (Exists(model.Id, model.Title) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }

            var EntryExitType = _mapper.Map<EntryExitType>(model);
            EntryExitType.InsertDate = DateTime.Now;
            EntryExitType.InsertUser = model.CurrentUserName;
            EntryExitType.DomainName = model.DomainName;
            EntryExitType.IsActive = true;
            _kscHrUnitOfWork.EntryExitTypeRepository.Add(EntryExitType);
            _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public KscResult UpdateEntryExitType(AddOrEditEntryExitTypeModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneEntryExitType = GetOne(model.Id);
            if (oneEntryExitType == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            if (ExistsByCode(model.Id, model.Code) == true)
            {

                result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
                return result;
            }



            if (Exists(model.Id, model.Title) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            oneEntryExitType.UpdateDate = DateTime.Now;
            oneEntryExitType.UpdateUser = model.CurrentUserName;
            oneEntryExitType.IsActive = model.IsActive;
            oneEntryExitType.Code = model.Code;
            oneEntryExitType.Title = model.Title;


            _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public KscResult RemoveEntryExitType(AddOrEditEntryExitTypeModel model)
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

        public List<EntryExitTypeModel> GetEntryExitTypes()
        {
            var cities = _kscHrUnitOfWork.EntryExitTypeRepository.GetAllQueryable();
            return _mapper.Map<List<EntryExitTypeModel>>(cities);

        }
        public FilterResult<EntryExitTypeModel> GetEntryExitTypesByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<EntryExitType>(_kscHrUnitOfWork.EntryExitTypeRepository.GetAllQueryable().AsQueryable(), Filter, "Id");
            return new FilterResult<EntryExitTypeModel>()
            {
                Data = _mapper.Map<List<EntryExitTypeModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }
        public async Task<List<AutomCompleteModel>> GetEntryExitTypes_AutoComplete()
        {
            var entryexistsquery = _kscHrUnitOfWork.EntryExitTypeRepository.WhereQueryable(x => x.IsActive);
            var data = entryexistsquery.Select(x => new AutomCompleteModel() { text = x.Title, value = x.Id.ToString() });
            return data.ToList();

        }
        public EntryExitType GetOne(int id)
        {
            return _kscHrUnitOfWork.EntryExitTypeRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public AddOrEditEntryExitTypeModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<AddOrEditEntryExitTypeModel>(model);
        }


    }
}
