using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.AccessLevel;
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
    public class AccessLevelService : IAccessLevelService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public AccessLevelService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void ExistsByTitle(int id, string title)
        {
            if (_kscHrUnitOfWork.AccessLevelRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void ExistsByTitle(string title)
        {
            if (_kscHrUnitOfWork.AccessLevelRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddAccessLevel(AddAccessLevelModel model)
        {
            var result = new KscResult();
            ExistsByTitle(model.Title);
            ExistsByCode(model.Id, model.Code);
            var AccessLevel = _mapper.Map<AccessLevel>(model);
            _kscHrUnitOfWork.AccessLevelRepository.Add(AccessLevel);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateAccessLevel(EditAccessLevelModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneAccessLevel = GetOne(model.Id);
            if (oneAccessLevel == null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
            }
            //
            ExistsByTitle(model.Id, model.Title);
            ExistsByCode(model.Id, model.Code);
            //

            oneAccessLevel.Code = model.Code;
            oneAccessLevel.Title = model.Title;
            oneAccessLevel.UpdateUser = model.CurrentUserName;
            oneAccessLevel.IsActive = model.IsActive;
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveAccessLevel(EditAccessLevelModel model)
        {

            var result = new KscResult();
            //if (!result.Success)
            //    return result;
            var item = GetOne(model.Id);
            if (item == null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
            }

            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;

           await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<AccessLevelModel> GetAccessLevel()
        {
            var cities = _kscHrUnitOfWork.AccessLevelRepository.GetAll();
            return _mapper.Map<List<AccessLevelModel>>(cities);

        }
        public FilterResult<AccessLevelModel> GetAccessLevelByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<AccessLevel>(_kscHrUnitOfWork.AccessLevelRepository.GetAllQueryable().AsQueryable(), Filter, "Id");

            var AccessLevelFiltred = result.Data.Select(a => new AccessLevelModel()
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                IsActive = a.IsActive
            }).ToList();

            return new FilterResult<AccessLevelModel>()
            {
                Data = AccessLevelFiltred,
                Total = result.Total

            };
        }


        public AccessLevel GetOne(int id)
        {
            return _kscHrUnitOfWork.AccessLevelRepository.GetAll().First(a => a.Id == id);
        }

        public EditAccessLevelModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditAccessLevelModel>(model);
        }



        public List<SearchAccessLevelModel> GetAccessLevelsByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<AccessLevel>(_kscHrUnitOfWork.AccessLevelRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchAccessLevelModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchAccessLevelModel>>(finalData);
        }

        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.AccessLevelRepository.Any(x => x.Id != id && x.Code == code);
        }
    }
}
