using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.TeamWorkCategoryType;
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
    public class TeamWorkCategoryTypeService : ITeamWorkCategoryTypeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public TeamWorkCategoryTypeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Any(x => x.Id != id && x.IsActive == true && x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        public void ExistsByCode(int id, string code)
        {
            //return _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddTeamWorkCategoryType(AddTeamWorkCategoryTypeModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);

            Exists(model.Id, model.Title);
            

            var TeamWorkCategoryType = _mapper.Map<TeamWorkCategoryType>(model);
            //TeamWorkCategoryType.InsertDate = DateTime.Now;
            //TeamWorkCategoryType.InsertUser = model.CurrentUserName;
            //TeamWorkCategoryType.DomainName = model.DomainName;
            //TeamWorkCategoryType.IsActive = true;
            _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.Add(TeamWorkCategoryType);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateTeamWorkCategoryType(EditTeamWorkCategoryTypeModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);

            var oneTeamWorkCategoryType = GetOne(model.Id);
            if (oneTeamWorkCategoryType == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            oneTeamWorkCategoryType.Code = model.Code;
            oneTeamWorkCategoryType.Title = model.Title;
            oneTeamWorkCategoryType.IsActive = model.IsActive;


            //var TeamWorkCategoryType = _mapper.Map<TeamWorkCategoryType>(model);
            oneTeamWorkCategoryType.UpdateDate = DateTime.Now;
            oneTeamWorkCategoryType.UpdateUser = model.CurrentUserName;
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveTeamWorkCategoryType(EditTeamWorkCategoryTypeModel model)
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

            item.InsertDate = DateTime.Now;
            item.InsertUser = model.CurrentUserName;

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<TeamWorkCategoryTypeModel> GetTeamWorkCategoryTypes()
        {
            var cities = _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.GetAllQueryable();
            return _mapper.Map<List<TeamWorkCategoryTypeModel>>(cities);

        }
        public FilterResult<TeamWorkCategoryTypeModel> GetTeamWorkCategoryTypesByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<TeamWorkCategoryType>(_kscHrUnitOfWork.TeamWorkCategoryTypeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return new FilterResult<TeamWorkCategoryTypeModel>()
            {
                Data = _mapper.Map<List<TeamWorkCategoryTypeModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }
        public TeamWorkCategoryType GetOne(int id)
        {
            return _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }

        public EditTeamWorkCategoryTypeModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditTeamWorkCategoryTypeModel>(model);
        }

        public List<SearchTeamWorkCategoryTypeModel> GetTeamWorkCategoryTypesByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<TeamWorkCategoryType>(_kscHrUnitOfWork.TeamWorkCategoryTypeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchTeamWorkCategoryTypeModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchTeamWorkCategoryTypeModel>>(finalData);
        }


    }
}
