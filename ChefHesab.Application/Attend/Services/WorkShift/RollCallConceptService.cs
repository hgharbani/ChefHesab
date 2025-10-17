using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
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
    public class RollCallConceptService : IRollCallConceptService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public RollCallConceptService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool ExistsByTitle(int id, string title)
        {
            return _kscHrUnitOfWork.RollCallConceptRepository.Any(x => x.Id != id == true && x.Title == title);
        }
        public bool ExistsByTitle(string title)
        {
            return _kscHrUnitOfWork.RollCallConceptRepository.Any(x => x.Title == title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddRollCallConcept(AddRollCallConceptModel model)
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

            var RollCallConcept = _mapper.Map<RollCallConcept>(model);
            RollCallConcept.InsertDate = DateTime.Now;
            RollCallConcept.InsertUser = model.CurrentUserName;
            RollCallConcept.DomainName = model.DomainName;
            RollCallConcept.IsActive = true;
            _kscHrUnitOfWork.RollCallConceptRepository.Add(RollCallConcept);
           await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateRollCallConcept(EditRollCallConceptModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneRollCallConcept = GetOne(model.Id);
            if (oneRollCallConcept == null)
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

            oneRollCallConcept.Code = model.Code;
            oneRollCallConcept.Title = model.Title;
            oneRollCallConcept.UpdateUser = model.CurrentUserName;
            oneRollCallConcept.IsActive = model.IsActive;

           await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveRollCallConcept(EditRollCallConceptModel model)
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

        public List<RollCallConceptModel> GetRollCallConcept()
        {
            var cities = _kscHrUnitOfWork.RollCallConceptRepository.GetAllQueryable();
            return _mapper.Map<List<RollCallConceptModel>>(cities);

        }
        public FilterResult<RollCallConceptModel> GetRollCallConceptByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<RollCallConcept>(_kscHrUnitOfWork.RollCallConceptRepository.GetAllQueryable().AsQueryable(), Filter, "Id");

            var RollCallConceptFiltred = result.Data.Select(a => new RollCallConceptModel()
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                IsActive=a.IsActive
            }).ToList();

            return new FilterResult<RollCallConceptModel>()
            {
                Data = RollCallConceptFiltred,
                Total = result.Total

            };
        }


        public RollCallConcept GetOne(int id)
        {
            return _kscHrUnitOfWork.RollCallConceptRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public EditRollCallConceptModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditRollCallConceptModel>(model);
        }



        public List<SearchRollCallConceptModel> GetRollCallConceptsByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<RollCallConcept>(_kscHrUnitOfWork.RollCallConceptRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchRollCallConceptModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchRollCallConceptModel>>(finalData);
        }

        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.RollCallConceptRepository.Any(x => x.Id != id && x.Code == code);
        }
    }
}
