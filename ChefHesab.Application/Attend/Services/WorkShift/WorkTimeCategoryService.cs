using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkShift;
using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class WorkTimeCategoryService : IWorkTimeCategoryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public WorkTimeCategoryService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void ExistsTitle(int id, string title)
        {
            if (_kscHrUnitOfWork.WorkTimeCategoryRepository.Any(x => x.Id != id && x.IsActive == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));

        }
        public void ExistsTitle(string title)
        {
            if (_kscHrUnitOfWork.WorkTimeCategoryRepository.Any(x => x.IsActive == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));

        }

        public void ExistsCode(string code)
        {
            if (_kscHrUnitOfWork.WorkTimeCategoryRepository.Any(x => x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Code));
        }
        public void ExistsCode(int id, string code)
        {
            if (_kscHrUnitOfWork.WorkTimeCategoryRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Code));

        }

        public async Task<KscResult> AddWorkTimeCategory(AddOrEditWorkTimeCategoryModel model)
        {
            var result = new KscResult();
            try
            {
                ExistsCode(model.Code);
                ExistsTitle(model.Title);
                var getall = _kscHrUnitOfWork.EmployeeJobPositionRepository.GetAllQueryable();
                var WorkTimeCategory = _mapper.Map<WorkTimeCategory>(model);
                _kscHrUnitOfWork.WorkTimeCategoryRepository.Add(WorkTimeCategory);
                //_kscHrUnitOfWork.LogDataRepository.InsertLog();
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                //throw;
            }
            return result;

        }

        public async Task<KscResult> UpdateWorkTimeCategory(AddOrEditWorkTimeCategoryModel model)
        {
            var result = new KscResult();
            var oneWorkTimeCategory = GetOne(model.Id);
            if (oneWorkTimeCategory == null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");
            }
            ExistsCode(model.Id, model.Code);
            ExistsTitle(model.Id, model.Title);
            model.InsertDate = oneWorkTimeCategory.InsertDate;

            var employeeLongTermAbsence = _mapper.Map<WorkTimeCategory>(model);
            _kscHrUnitOfWork.WorkTimeCategoryRepository.Update(employeeLongTermAbsence);
            //_kscHrUnitOfWork.LogDataRepository.InsertLog();

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
        public async Task<KscResult> RemoveWorkTimeCategory(AddOrEditWorkTimeCategoryModel model)
        {

            var result = new KscResult();

            var item = GetOne(model.Id);
            if (item == null)
            {
                throw new HRBusinessException(Validations.NotFound, "رکورد حذف شده است");

            }

            item.IsActive = model.IsActive;
            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;

            //_kscHrUnitOfWork.LogDataRepository.InsertLog();


            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<WorkTimeCategoryModel> GetWorkTimeCategories()
        {
            var cities = _kscHrUnitOfWork.WorkTimeCategoryRepository.GetAllQueryable();
            return _mapper.Map<List<WorkTimeCategoryModel>>(cities);

        }
        public FilterResult<WorkTimeCategoryModel> GetWorktimeCategories(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<WorkTimeCategory>(_kscHrUnitOfWork.WorkTimeCategoryRepository.GetAllQueryable().AsQueryable(), Filter, "Id");
            return new FilterResult<WorkTimeCategoryModel>()
            {
                Data = _mapper.Map<List<WorkTimeCategoryModel>>(result.Data.ToList()),
                Total = result.Total

            };
            ;

        }
        public WorkTimeCategory GetOne(int id)
        {
            return _kscHrUnitOfWork.WorkTimeCategoryRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public AddOrEditWorkTimeCategoryModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<AddOrEditWorkTimeCategoryModel>(model);
        }

        public List<SearchWorkTimeCategoryModel> GetWorkTimeCategoriesByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<WorkTimeCategory>(_kscHrUnitOfWork.WorkTimeCategoryRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchWorkTimeCategoryModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchWorkTimeCategoryModel>>(finalData);
        }


    }
}
