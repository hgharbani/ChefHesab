using AutoMapper;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.MaritalStatus;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class MaritalStatusService : IMaritalStatusService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public MaritalStatusService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public async Task<KscResult> AddMaritalStatus(AddMaritalStatusDto model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            if (await ExistsByTitle(model.Title) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            model.IsActive = true;
            //model.InsertDate = DateTime.Now;
            var MaritalStatusObj = _mapper.Map<MaritalStatus>(model);
            await _kscHrUnitOfWork.MaritalStatusRepository.AddAsync(MaritalStatusObj);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<bool> ExistsByTitle(int id, string title)
        {
            return (await _kscHrUnitOfWork.MaritalStatusRepository.GetAllAsync()).Any(x => x.Id != id && x.Title == title);
        }

        public async Task<bool> ExistsByTitle(string title)
        {
            return await _kscHrUnitOfWork.MaritalStatusRepository.AnyAsync(x => x.Title == title);
        }


        public async Task<EditMaritalStatusDto> GetForEdit(int id)
        {
            var model = await GetOne(id);
            return _mapper.Map<EditMaritalStatusDto>(model);
        }

        public async Task<MaritalStatus> GetOne(int id)
        {
            return await _kscHrUnitOfWork.MaritalStatusRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// لیست تحصیلات
        /// </summary>
        public List<ListMaritalStatusDto> GetMaritalStatuss()
        {
            var transferTypes = _kscHrUnitOfWork.MaritalStatusRepository.GetAllQueryable();
            return _mapper.Map<List<ListMaritalStatusDto>>(transferTypes);
        }

        /// <summary>
        /// فیلتر کندو برای دراپ داون
        /// </summary>
        public FilterResult<ListMaritalStatusDto> GetMaritalStatusByFilter(FilterRequest Filter)
        {

            //var result = _FilterHandler.GetFilterResult<MaritalStatus>(_kscHrUnitOfWork.MaritalStatusRepository.GetAllQueryable(), Filter, "Id");
            var query = _kscHrUnitOfWork.MaritalStatusRepository.GetAllQueryable().AsQueryable();
            var result = _FilterHandler.GetFilterResult<MaritalStatus>(query, Filter, nameof(MaritalStatus.Id));


            var MaritalStatusFiltered = result.Data.Select(a => new ListMaritalStatusDto()
            {
                Id = a.Id,
                Title = a.Title,
                IsActive = a.IsActive
            }).ToList();

            return new FilterResult<ListMaritalStatusDto>()
            {
                Data = MaritalStatusFiltered,
                Total = result.Total
            };
        }

        /// <summary>
        /// فیلتر کندو برای نمایش لیستی از تحصیلات
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public List<SearchMaritalStatusDto> GetMaritalStatusByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.MaritalStatusRepository.WhereQueryable(x => x.IsActive == true).AsQueryable().AsNoTracking();
            var result = _FilterHandler.GetFilterResult<MaritalStatus>(query, Filter, "Id");
            var finalData = result.Data.Select(a => new SearchMaritalStatusDto()
            {
                Id = a.Id,
                Title = a.Title,

                IsActive = a.IsActive
            }).ToList();
            return _mapper.Map<List<SearchMaritalStatusDto>>(finalData);
        }


        public async Task<KscResult> RemoveMaritalStatus(EditMaritalStatusDto model)
        {

            var result = new KscResult();
            var item = await GetOne(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            item.IsActive = false;
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        /// <summary>
        /// post  ویرایش
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> UpdateMaritalStatus(EditMaritalStatusDto model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneMaritalStatus = await GetOne(model.Id);
            if (oneMaritalStatus == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            //
            if (await ExistsByTitle(model.Id, model.Title) == true)
            {
                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            //
            oneMaritalStatus.Title = model.Title;
            //oneMaritalStatus.UpdateUser = model.UpdateUser;
            //oneMaritalStatus.UpdateDate = DateTime.Now;
            oneMaritalStatus.IsActive = model.IsActive;

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
    }
}
