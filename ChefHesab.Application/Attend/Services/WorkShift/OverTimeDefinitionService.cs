using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.OverTimeDefinition;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class OverTimeDefinitionService : IOverTimeDefinitionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public OverTimeDefinitionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.OverTimeDefinitionRepository.Any(x => x.Id != id && x.IsActive == true && x.Title == title);
            if (_kscHrUnitOfWork.OverTimeDefinitionRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.OverTimeDefinitionRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.OverTimeDefinitionRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        public void ExistsByCode(int id, string code)
        {
            //return _kscHrUnitOfWork.OverTimeDefinitionRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.OverTimeDefinitionRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddOverTimeDefinition(AddOverTimeDefinitionModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);

            var OverTimeDefinition = _mapper.Map<OverTimeDefinition>(model);
            //OverTimeDefinition.InsertDate = DateTime.Now;
            //OverTimeDefinition.InsertUser = model.CurrentUserName;
            //OverTimeDefinition.DomainName = model.DomainName;
            //OverTimeDefinition.IsActive = true;

            OverTimeDefinition.AverageDurationMinute = Utility.ConvertDurationToMinute(model.AverageDuration) ?? 0;
            OverTimeDefinition.MaximumDurationMinute = Utility.ConvertDurationToMinute(model.MaximumDuration) ?? 0;

            _kscHrUnitOfWork.OverTimeDefinitionRepository.Add(OverTimeDefinition);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateOverTimeDefinition(EditOverTimeDefinitionModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);
            
            var oneOverTimeDefinition = GetOne(model.Id);
            if (oneOverTimeDefinition == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            oneOverTimeDefinition.Code = model.Code;
            oneOverTimeDefinition.Title = model.Title;
            oneOverTimeDefinition.IsActive = model.IsActive;
            oneOverTimeDefinition.MaximumDuration = model.MaximumDuration;

            oneOverTimeDefinition.AverageDuration = model.AverageDuration;

            oneOverTimeDefinition.AverageDurationMinute = Utility.ConvertDurationToMinute(model.AverageDuration) ?? 0;
            oneOverTimeDefinition.MaximumDurationMinute = Utility.ConvertDurationToMinute(model.MaximumDuration) ?? 0;


            //var OverTimeDefinition = _mapper.Map<OverTimeDefinition>(model);
            oneOverTimeDefinition.UpdateDate = DateTime.Now;
            oneOverTimeDefinition.UpdateUser = model.CurrentUserName;


            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveOverTimeDefinition(EditOverTimeDefinitionModel model)
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

        public List<OverTimeDefinitionModel> GetOverTimeDefinitions()
        {
            var cities = _kscHrUnitOfWork.OverTimeDefinitionRepository.GetAllQueryable();
            return _mapper.Map<List<OverTimeDefinitionModel>>(cities);

        }
        public FilterResult<OverTimeDefinitionModel> GetOverTimeDefinitionsByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<OverTimeDefinition>(_kscHrUnitOfWork.OverTimeDefinitionRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return new FilterResult<OverTimeDefinitionModel>()
            {
                Data = _mapper.Map<List<OverTimeDefinitionModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }
        public OverTimeDefinition GetOne(int id)
        {
            return _kscHrUnitOfWork.OverTimeDefinitionRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }

        public EditOverTimeDefinitionModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditOverTimeDefinitionModel>(model);
        }

        public List<SearchOverTimeDefinitionModel> GetOverTimeDefinitionsByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<OverTimeDefinition>(_kscHrUnitOfWork.OverTimeDefinitionRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchOverTimeDefinitionModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchOverTimeDefinitionModel>>(finalData);
        }


    }
}
