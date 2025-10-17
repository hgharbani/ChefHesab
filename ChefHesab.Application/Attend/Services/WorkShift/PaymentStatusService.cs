using AutoMapper;
using KSC.Common;

using Ksc.HR.Domain.Repositories;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;

using Ksc.HR.DTO.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Appication.Interfaces.WorkShift;

namespace Ksc.HR.Appication.Services.WorkShift

{
    public class PaymentStatusService : IPaymentStatusService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public PaymentStatusService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public List<SearchPaymentStatusModel> GetPaymentStatusByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.PaymentStatusRepository.GetAllQueryable().AsQueryable();
            var result = _FilterHandler.GetFilterResult<PaymentStatus>(query, Filter, nameof(PaymentStatus.Id));
            var finalData = result.Data.Select(a => new SearchPaymentStatusModel()
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();
            return _mapper.Map<List<SearchPaymentStatusModel>>(finalData);
        }

        public FilterResult<SearchPaymentStatusModel> GetPaymentStatusByFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.PaymentStatusRepository.GetAllQueryable().AsQueryable();
            var result = _FilterHandler.GetFilterResult<PaymentStatus>(query, Filter, nameof(PaymentStatus.Id));
            //var finalData = result.Data.Select(a => new SearchPaymentStatusModel()
            //{
            //    Id = a.Id,
            //    Title = a.Title,
            //    MaximumDayCount = a.MaximumDayCount,
            //    MinimumDayCount = a.MinimumDayCount,
            //    OnlyCurrentDay = a.OnlyCurrentDay,
            //    InsertDate = a.InsertDate,
            //    InsertUser = a.InsertUser,
            //    IsActive = a.IsActive,
            //}).ToList();
            var modelResult = new FilterResult<SearchPaymentStatusModel>
            {
                Data = _mapper.Map<List<SearchPaymentStatusModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
            //var finalData = result.Data.ToList();
            //return _mapper.Map<List<SearchPaymentStatusModel>>(finalData);

        }

        public async Task<KscResult> AddPaymentStatus(AddPaymentStatusModel model)
        {
            var result = model.IsValid();
            try
            {

            
            if (!result.Success)
                return result;
            if (await ExistsByTitle(model.Title) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            if (string.IsNullOrEmpty(model.Title))
            {

                result.AddError("رکورد نامعتبر", "عنوان باید مقدار داشته باشد");
                return result;
            }
            var PaymentStatusObj = _mapper.Map<PaymentStatus>(model);
                PaymentStatusObj.IsActive = true;
                PaymentStatusObj.InsertDate = DateTime.Now;
                PaymentStatusObj.InsertUser = model.InsertUser;

                await _kscHrUnitOfWork.PaymentStatusRepository.AddAsync(PaymentStatusObj);
            await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }
            return result;

        }
        public async Task<bool> ExistsByTitle(int id, string title)
        {
            return (await _kscHrUnitOfWork.PaymentStatusRepository.GetAllAsync()).Any(x => x.Id != id && x.Title == title);
        }

        public async Task<bool> ExistsByTitle(string title)
        {
            return await _kscHrUnitOfWork.PaymentStatusRepository.AnyAsync(x => x.Title == title);
        }



        public async Task<PaymentStatus> GetOne(int id)
        {
            return await _kscHrUnitOfWork.PaymentStatusRepository.GetByIdAsync(id);
        }

       
     
        public async Task<List<AutomCompleteModel>> GetPaymentStatusAutoComplete()
        {
            var goalsquery = _kscHrUnitOfWork.PaymentStatusRepository.WhereQueryable(x => x.IsActive);
            var data = goalsquery.Select(x => new AutomCompleteModel() { text = x.Title, value = x.Id.ToString() });
            return data.ToList();

        }


        public async Task<List<AutomCompleteModel>> GetPaymentStatusAutoCompleteByAccess(List<string> roles)
        {
            var goalsquery = _kscHrUnitOfWork.PaymentStatusRepository.WhereQueryable(x => x.IsActive);

            /////
            //
            var paymentStatusAccess = _kscHrUnitOfWork.PaymentStatusAccessRepository.GetPaymentStatusAccessesByRoles(roles);
            var paymentStatusId = paymentStatusAccess.Select(x => x.PaymentStatusId).Distinct().ToList();

            if (!paymentStatusId.Any(x => x == null)) //ادمین نباشد
            {
                goalsquery = goalsquery.Where(x => paymentStatusId.Any(p => p == x.Id));
            }

            //
            //////

            var data = goalsquery.Select(x => new AutomCompleteModel() { text = x.Title, value = x.Id.ToString() });
            return data.ToList();

        }

        public async Task<EditPaymentStatusModel> GetForEdit(int id)
        {
            var model = await GetOne(id);
            return _mapper.Map<EditPaymentStatusModel>(model);
        }

        

        public async Task<KscResult> UpdatePaymentStatus(EditPaymentStatusModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneMissionGoal = await GetOne(model.Id);
            if (oneMissionGoal == null)
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
            //if (await ExistsByTitle(model.Title) == true)
            //{

            //    result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
            //    return result;
            //}
            //
            oneMissionGoal.Title = model.Title;
            oneMissionGoal.UpdateUser = model.UpdateUser;
            oneMissionGoal.UpdateDate = DateTime.Now;
            oneMissionGoal.IsActive = model.IsActive;
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }



    }
}
