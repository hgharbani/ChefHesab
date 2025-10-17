using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
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
    public class ShiftConceptService : IShiftConceptService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ShiftConceptService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.ShiftConceptRepository.Any(x => x.Id != id && x.IsActive == true && x.Title == title);
            if (_kscHrUnitOfWork.ShiftConceptRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.ShiftConceptRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.ShiftConceptRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        public void ExistsByCode(int id, string code)
        {
            //return _kscHrUnitOfWork.ShiftConceptRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.ShiftConceptRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddShiftConcept(AddOrEditShiftConceptModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);
            

            var ShiftConcept = _mapper.Map<ShiftConcept>(model);
            //ShiftConcept.InsertDate = DateTime.Now;
            //ShiftConcept.InsertUser = model.CurrentUserName;
            //ShiftConcept.DomainName = model.DomainName;
            //ShiftConcept.IsActive = true;
            _kscHrUnitOfWork.ShiftConceptRepository.Add(ShiftConcept);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateShiftConcept(AddOrEditShiftConceptModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);
            
            var oneShiftConcept = GetOne(model.Id);
            if (oneShiftConcept == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            oneShiftConcept.Code = model.Code;
            oneShiftConcept.Title = model.Title;
            oneShiftConcept.IsActive = model.IsActive;
            oneShiftConcept.IsRest = model.IsRest;

            //var ShiftConcept = _mapper.Map<ShiftConcept>(model);
            oneShiftConcept.UpdateDate = DateTime.Now;
            oneShiftConcept.UpdateUser = model.CurrentUserName;

            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveShiftConcept(AddOrEditShiftConceptModel model)
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

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<ShiftConceptModel> GetShiftConcepts()
        {
            var cities = _kscHrUnitOfWork.ShiftConceptRepository.GetAllQueryable();
            return _mapper.Map<List<ShiftConceptModel>>(cities);

        }
        public FilterResult<ShiftConceptModel> GetShiftConceptsByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftConcept>(_kscHrUnitOfWork.ShiftConceptRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return new FilterResult<ShiftConceptModel>()
            {
                Data = _mapper.Map<List<ShiftConceptModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }
        public ShiftConcept GetOne(int id)
        {
            return _kscHrUnitOfWork.ShiftConceptRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }

        public AddOrEditShiftConceptModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<AddOrEditShiftConceptModel>(model);
        }

        public List<SearchShiftConceptModel> GetShiftConceptsByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftConcept>(_kscHrUnitOfWork.ShiftConceptRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchShiftConceptModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
                IsRest = a.IsRest,
            }).ToList();
            return _mapper.Map<List<SearchShiftConceptModel>>(finalData);
        }


    }
}
