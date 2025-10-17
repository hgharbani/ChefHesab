using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.TeamWorkMangementCode;
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
    public class TeamWorkMangementCodeService : ITeamWorkMangementCodeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public TeamWorkMangementCodeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.TeamWorkMangementCodeRepository.Any(x => x.Id != id && x.IsActive == true && x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkMangementCodeRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.TeamWorkMangementCodeRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkMangementCodeRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        public void ExistsByCode(int id, string code)
        {
            //return _kscHrUnitOfWork.TeamWorkMangementCodeRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.TeamWorkMangementCodeRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddTeamWorkMangementCode(AddTeamWorkMangementCodeModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);

            var TeamWorkMangementCode = _mapper.Map<TeamWorkMangementCode>(model);
            //TeamWorkMangementCode.InsertDate = DateTime.Now;
            //TeamWorkMangementCode.InsertUser = model.CurrentUserName;
            //TeamWorkMangementCode.DomainName = model.DomainName;
            //TeamWorkMangementCode.IsActive = true;
            _kscHrUnitOfWork.TeamWorkMangementCodeRepository.Add(TeamWorkMangementCode);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateTeamWorkMangementCode(EditTeamWorkMangementCodeModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            ExistsByCode(model.Id, model.Code);

            Exists(model.Id, model.Title);

            var oneTeamWorkMangementCode = GetOne(model.Id);
            if (oneTeamWorkMangementCode == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            oneTeamWorkMangementCode.Code = model.Code;
            oneTeamWorkMangementCode.Title = model.Title;
            oneTeamWorkMangementCode.IsActive = model.IsActive;


            //var TeamWorkMangementCode = _mapper.Map<TeamWorkMangementCode>(model);
            oneTeamWorkMangementCode.UpdateDate = DateTime.Now;
            oneTeamWorkMangementCode.UpdateUser = model.CurrentUserName;
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveTeamWorkMangementCode(EditTeamWorkMangementCodeModel model)
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

        public List<TeamWorkMangementCodeModel> GetTeamWorkMangementCodes()
        {
            var cities = _kscHrUnitOfWork.TeamWorkMangementCodeRepository.GetAllQueryable();
            return _mapper.Map<List<TeamWorkMangementCodeModel>>(cities);

        }
        public FilterResult<TeamWorkMangementCodeModel> GetTeamWorkMangementCodesByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<TeamWorkMangementCode>(_kscHrUnitOfWork.TeamWorkMangementCodeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return new FilterResult<TeamWorkMangementCodeModel>()
            {
                Data = _mapper.Map<List<TeamWorkMangementCodeModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }
        public TeamWorkMangementCode GetOne(int id)
        {
            return _kscHrUnitOfWork.TeamWorkMangementCodeRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }

        public EditTeamWorkMangementCodeModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditTeamWorkMangementCodeModel>(model);
        }

        public List<SearchTeamWorkMangementCodeModel> GetTeamWorkMangementCodesByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<TeamWorkMangementCode>(_kscHrUnitOfWork.TeamWorkMangementCodeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchTeamWorkMangementCodeModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchTeamWorkMangementCodeModel>>(finalData);
        }


    }
}
