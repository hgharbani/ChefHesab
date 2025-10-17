using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
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
    public class IncludedDefinitionService : IIncludedDefinitionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public IncludedDefinitionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool ExistsByTitle(int id, string title)
        {
            return _kscHrUnitOfWork.IncludedDefinitionRepository.Any(x => x.Id != id == true && x.Title == title);
        }
        public bool ExistsByTitle(string title)
        {
            return _kscHrUnitOfWork.IncludedDefinitionRepository.Any(x => x.Title == title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddIncludedDefinition(AddIncludedDefinitionModel model)
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

            var IncludedDefinition = _mapper.Map<IncludedDefinition>(model);
            IncludedDefinition.InsertDate = DateTime.Now;
            IncludedDefinition.InsertUser = model.CurrentUserName;
            IncludedDefinition.DomainName = model.DomainName;
            IncludedDefinition.IsActive = true;
            _kscHrUnitOfWork.IncludedDefinitionRepository.Add(IncludedDefinition);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateIncludedDefinition(EditIncludedDefinitionModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneIncludedDefinition = GetOne(model.Id);
            if (oneIncludedDefinition == null)
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

            oneIncludedDefinition.Code = model.Code;
            oneIncludedDefinition.Title = model.Title;
            oneIncludedDefinition.UpdateUser = model.CurrentUserName;
            oneIncludedDefinition.IsActive = model.IsActive;

            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveIncludedDefinition(EditIncludedDefinitionModel model)
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

        public List<IncludedDefinitionModel> GetIncludedDefinition()
        {
            var cities = _kscHrUnitOfWork.IncludedDefinitionRepository.GetAllQueryable();
            return _mapper.Map<List<IncludedDefinitionModel>>(cities);

        }
        public FilterResult<IncludedDefinitionModel> GetIncludedDefinitionByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<IncludedDefinition>(_kscHrUnitOfWork.IncludedDefinitionRepository.GetAllQueryable().AsQueryable(), Filter, "Id");

            var IncludedDefinitionFiltred = result.Data.Select(a => new IncludedDefinitionModel()
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                IsActive = a.IsActive
            }).ToList();

            return new FilterResult<IncludedDefinitionModel>()
            {
                Data = IncludedDefinitionFiltred,
                Total = result.Total

            };
        }


        public IncludedDefinition GetOne(int id)
        {
            return _kscHrUnitOfWork.IncludedDefinitionRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public EditIncludedDefinitionModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditIncludedDefinitionModel>(model);
        }



        public List<SearchIncludedDefinitionModel> GetIncludedDefinitionsByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<IncludedDefinition>(_kscHrUnitOfWork.IncludedDefinitionRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchIncludedDefinitionModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchIncludedDefinitionModel>>(finalData);
        }

        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.IncludedDefinitionRepository.Any(x => x.Id != id && x.Code == code);
        }

        public List<SearchIncludedDefinitionModel> GetIncludedSalaryDeductionByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<IncludedDefinition>(_kscHrUnitOfWork.IncludedDefinitionRepository.WhereQueryable(a => a.IsActive && IncludedDefinitionConst.SalaryDeductionIncludedId.Contains(a.Id)).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchIncludedDefinitionModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchIncludedDefinitionModel>>(finalData);
        }
        public List<SearchIncludedDefinitionModel> GetIncludedRewardDeductionByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<IncludedDefinition>(_kscHrUnitOfWork.IncludedDefinitionRepository.WhereQueryable(a => a.IsActive && IncludedDefinitionConst.RewardDeductionIncludedId.Contains(a.Id)).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchIncludedDefinitionModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchIncludedDefinitionModel>>(finalData);
        }
        public List<SearchIncludedDefinitionModel> GetIncludedCodeForMultiSelectByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<IncludedDefinition>(_kscHrUnitOfWork.IncludedDefinitionRepository.WhereQueryable(a => a.IsActive && !IncludedDefinitionConst.SalaryDeductionIncludedId.Contains(a.Id) && !IncludedDefinitionConst.RewardDeductionIncludedId.Contains(a.Id)).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchIncludedDefinitionModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchIncludedDefinitionModel>>(finalData);
        }
    }
}
