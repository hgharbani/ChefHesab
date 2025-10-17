using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Appication.Interfaces.OnCall;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Appication.Interfaces.Transfer;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmploymentType;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.MIS;
using DNTPersianUtils.Core;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.Share.General;
using Ksc.HR.DTO.Personal.EmployeeTeamWork;
using Ksc.HR.Share.Model.PaymentStatus;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeTeamWorkService : IEmployeeTeamWorkService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IMisUpdateService _misUpdateService;
        public EmployeeTeamWorkService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IMisUpdateService misUpdateService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _misUpdateService = misUpdateService;
        }
        //for taeeeeeeeeed karkard
        public FilterResult<EmployeeModel> GetEmployeeTeamWorkforConfirmTimeSheetByFilter(EmployeeModel Filter)
        {
            var teamCodesUserWindows = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
             .Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName).Select(a => a.TeamCode.ToString()).ToList();

            var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable().AsQueryable().Include(x => x.TeamWork)
                .Where(a => a.IsActive == true && teamCodesUserWindows.Contains(a.TeamWork.Code)).Select(a => a.EmployeeId).ToList();

            //var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork().AsQueryable()
            //    .Where(a => teamCodesUserWindows.Contains(a.TeamWork.Code)).AsNoTracking();

            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking();

            if (!string.IsNullOrEmpty(Filter.TeamWorkCode) && int.Parse(Filter.TeamWorkCode) > 0)
            {

                EmployeeList = EmployeeList.Where(a => a.TeamWork.Code == Filter.TeamWorkCode);
            }


            var result = _FilterHandler.GetFilterResult<Employee>(EmployeeList, Filter, "Id");
            return new FilterResult<EmployeeModel>()
            {
                Data = _mapper.Map<List<EmployeeModel>>(result.Data.Distinct().ToList()),
                Total = result.Total

            };

        }
        public FilterResult<EmployeeModel> GetEmployeeTeamWorkByFilter(EmployeeModel Filter)
        {
            var teamCodesUserWindows = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
             .Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName).Select(a => a.TeamCode.ToString()).ToList();

            var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable().AsQueryable().Include(x => x.TeamWork)
                .Where(a => a.IsActive == true && a.TeamEndDate == null && teamCodesUserWindows.Contains(a.TeamWork.Code)).Select(a => a.EmployeeId).ToList();

            //var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork().AsQueryable()
            //    .Where(a => teamCodesUserWindows.Contains(a.TeamWork.Code)).AsNoTracking();

            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking();

            if (!string.IsNullOrEmpty(Filter.TeamWorkCode) && int.Parse(Filter.TeamWorkCode) > 0)
            {

                EmployeeList = EmployeeList.Where(a => a.TeamWork.Code == Filter.TeamWorkCode);
            }

            if (!string.IsNullOrEmpty(Filter.EmployeeNumber) && int.Parse(Filter.EmployeeNumber) > 0)
            {
                EmployeeList = EmployeeList.Where(a => a.EmployeeNumber == Filter.EmployeeNumber);
            }
            var result = _FilterHandler.GetFilterResult<Employee>(EmployeeList, Filter, "Id");
            return new FilterResult<EmployeeModel>()
            {
                Data = _mapper.Map<List<EmployeeModel>>(result.Data.Distinct().ToList()),
                Total = result.Total

            };

        }
        public async Task EmployeeTeamWorkTransferManagement(ResultEmployeeTransferModel model)
        {
            KscResult result = new KscResult();
            try
            {


                var employeeTeamWorkActive = await _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActiveTeamWorkByEmployeeIdAsync(model.EmployeeId);
                if (employeeTeamWorkActive == null)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تیم کاری فعال "));
                }
                if (employeeTeamWorkActive != null)
                {
                    employeeTeamWorkActive.IsActive = false;
                    if (employeeTeamWorkActive.TeamEndDate == null)
                    {
                        employeeTeamWorkActive.TeamEndDate = model.TransferChangeDate.Value.AddDays(-1);
                    }
                    if (model.IsTransferReturn) // برگشت جابه جایی
                    {
                        employeeTeamWorkActive.TeamEndDate = model.TransferReturnDate;
                    }
                    employeeTeamWorkActive.UpdateUser = model.CurrentUserName;
                    employeeTeamWorkActive.UpdateDate = System.DateTime.Now;

                }
                EmployeeTeamWork newEmployeeTeamWork = new EmployeeTeamWork();
                newEmployeeTeamWork.EmployeeId = model.EmployeeId;
                newEmployeeTeamWork.IsActive = true;
                newEmployeeTeamWork.TeamWorkId = model.TeamWorkId.Value;
                newEmployeeTeamWork.TransferRequestId = model.TransferRequestId;
                newEmployeeTeamWork.InsertUser = model.CurrentUserName;
                newEmployeeTeamWork.InsertDate = System.DateTime.Now;
                if (!model.IsTransferReturn)
                {
                    newEmployeeTeamWork.TeamStartDate = model.TransferChangeDate.Value;
                }
                else
                {
                    newEmployeeTeamWork.TeamStartDate = employeeTeamWorkActive.TeamEndDate.Value.AddDays(1);
                }
                //if (model.IsTemporaryTransfer && !model.IsTransferReturn)
                //{
                //    newEmployeeTeamWork.TeamEndDate = model.TransferReturnDate;
                //}
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.EmployeeId);
                employee.TeamWorkId = model.TeamWorkId;
                employee.TeamWorkStartDate = newEmployeeTeamWork.TeamStartDate;
                //
                await _kscHrUnitOfWork.EmployeeTeamWorkRepository.AddAsync(newEmployeeTeamWork);
                //
                var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetById(model.TeamWorkId.Value);

                //MIS بروزرسانی 
                UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
                {
                    Domain = "KSC",
                    FUNCTION = "EMPLOYEE-TEAM",
                    NUM_PRSN = employee.EmployeeNumber,
                    NUM_TEAM_EMPL = employeeTeamWorkActive.TeamWork.Code,
                    NUM_TEAM_LIST = teamWork.Code,
                    DAT_STR_TEAM = employeeTeamWorkActive.TeamStartDate.ToPersianDate().Replace("/", ""),
                    STR_TEAM_LIST = model.TransferChangeDate.ToPersianDate().Replace("/", ""),
                };
                var resultApiMis = _misUpdateService.UpdateTeamAndGroup(updateTeamAndGroupInputModel);
                if (resultApiMis.IsError)
                {
                    throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                }
                //
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// فقط افراد جاری سازمان را می آورد
        /// این متد در مدیریت جابه جایی تیم و شیف استفاده می شود
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public FilterResult<EmployeeTeamModel> GetAllEmployeeTeamWorkByFilter(EmployeeTeamModel Filter)
        {

            //var teamCodesUserWindows = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
            // .Select(a => a.TeamCode.ToString()).ToList();
            var currentDate = System.DateTime.Now.Date;
            var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable().AsQueryable().Include(x => x.TeamWork)
                .Where(a => a.IsActive == true 
                /*&& teamCodesUserWindows.Contains(a.TeamWork.Code)*/
                && (a.TeamEndDate == null || a.TeamEndDate.Value.Date >= currentDate)).Select(a => a.EmployeeId).ToList();
            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet().AsQueryable()
                           .Where(a => activePersonsId.Contains(a.Id)).AsNoTracking();
            if (!string.IsNullOrEmpty(Filter.TeamWorkCode) && int.Parse(Filter.TeamWorkCode) > 0)
            {

                EmployeeList = EmployeeList.Where(a => a.TeamWork.Code == Filter.TeamWorkCode);
            }

            if (!string.IsNullOrEmpty(Filter.EmployeeNumber) && int.Parse(Filter.EmployeeNumber) > 0)
            {
                EmployeeList = EmployeeList.Where(a => a.EmployeeNumber == Filter.EmployeeNumber);
            }
            if (Filter.EmployeeId > 0)
            {
                EmployeeList = EmployeeList.Where(a => a.Id == Filter.EmployeeId);
            }
            var query = _mapper.Map<List<EmployeeTeamModel>>(EmployeeList);

            var result = _FilterHandler.GetFilterResult<EmployeeTeamModel>(query, Filter, "Id");
            return new FilterResult<EmployeeTeamModel>()
            {
                Data = result.Data.OrderBy(i => i.TeamWorkCode).Distinct().ToList(),
                Total = result.Total

            };
        }
        public async Task<KscResult> AddEmployeeTeamWorkManagement(AddEmployeeTeamWorkModel.EmployeeTeamWork model)
        {
            KscResult result = model.IsValid();
            try
            {
                if (!result.Success)
                    return result;
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.EmployeeId);
                var employeeTeamWorkActive = await _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActiveTeamWorkByEmployeeIdAsync(model.EmployeeId);
                if (employeeTeamWorkActive != null && employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تیم کاری فعال دارید"));

                }
                if (model.TeamStartDate < employee.EmploymentDate)
                {
                    result.AddError("", "تاریخ شروع  کوچک تر از تاریخ استخدام می باشد");
                    return result;
                }
                EmployeeTeamWork newEmployeeTeamWork = new EmployeeTeamWork();
                newEmployeeTeamWork.EmployeeId = model.EmployeeId;
                newEmployeeTeamWork.IsActive = true;
                newEmployeeTeamWork.TeamWorkId = model.TeamWorkId;
                newEmployeeTeamWork.TeamStartDate = model.TeamStartDate;
                newEmployeeTeamWork.InsertUser = model.InsertUser;
                newEmployeeTeamWork.InsertDate = System.DateTime.Now;
                employee.TeamWorkId = model.TeamWorkId;
                employee.TeamWorkStartDate = newEmployeeTeamWork.TeamStartDate;
                await _kscHrUnitOfWork.EmployeeTeamWorkRepository.AddAsync(newEmployeeTeamWork);
                _kscHrUnitOfWork.EmployeeRepository.Update(employee);
                //
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }
        public async Task<KscResult> UpdateEmployeeTeamWorkManagement(AddEmployeeTeamWorkModel.EmployeeTeamWork model)
        {
            KscResult result = model.IsValid();
            try
            {
                if (!result.Success)
                    return result;
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.EmployeeId);
                if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تیم کاری فعال دارید"));
                }
                if (model.TeamStartDate < employee.EmploymentDate)
                {
                    result.AddError("", "تاریخ شروع  کوچک تر از تاریخ استخدام می باشد");
                    return result;
                }
                EmployeeTeamWork updateEmployeeTeamWork = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetById(model.Id);
                updateEmployeeTeamWork.TeamWorkId = model.TeamWorkId;
                updateEmployeeTeamWork.TeamStartDate = model.TeamStartDate;
                updateEmployeeTeamWork.UpdateUser = model.InsertUser;
                updateEmployeeTeamWork.UpdateDate = System.DateTime.Now;
                employee.TeamWorkId = model.TeamWorkId;
                employee.TeamWorkStartDate = updateEmployeeTeamWork.TeamStartDate;
                _kscHrUnitOfWork.EmployeeTeamWorkRepository.Update(updateEmployeeTeamWork);
                _kscHrUnitOfWork.EmployeeRepository.Update(employee);
                //
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }
        public async Task<KscResult> RemoveEmployeeTeamWorkManagement(int id)
        {
            KscResult result = new KscResult();
            try
            {
                if (!result.Success)
                    return result;
                EmployeeTeamWork employeeTeamWork = await _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetByIdAsync(id);
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(employeeTeamWork.EmployeeId);
                if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "امکان حذف تیم نمی باشد"));
                }
                employee.TeamWorkId = null;
                employee.TeamWorkStartDate = null;
                _kscHrUnitOfWork.EmployeeTeamWorkRepository.Delete(employeeTeamWork);
                _kscHrUnitOfWork.EmployeeRepository.Update(employee);
                //
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }
        public FilterResult<EmployeeTeamModel> GetAllByFilter(EmployeeTeamModel filter)
        {
            var teamworkemployee = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllTeamWorkInclude().Where(x => x.EmployeeId == filter.EmployeeId).OrderByDescending(x=>x.Id).AsQueryable()
                .Select(x => new EmployeeTeamModel()
                {
                    Id = x.Id,
                    PaymentStatusId = x.Employee.PaymentStatusId,
                    TeamWorkId = x.TeamWorkId.ToString(),
                    TeamWorkTitle = x.TeamWork.Title,
                    TeamWorkCode = x.TeamWork.Code,
                    TeamStartDate = x.TeamStartDate,
                    TeamEndDate = x.TeamEndDate,
                    IsActive = x.IsActive
                });
            var result = _FilterHandler.GetFilterResult<EmployeeTeamModel>(teamworkemployee, filter, "Id");
            return new FilterResult<EmployeeTeamModel>()
            {
                Data = _mapper.Map<List<EmployeeTeamModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public EmployeeTeamModel GetActiveTeamWorkByEmployeeId(int employeeId)
        {
            var query = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActiveTeamWorkByEmployeeId(employeeId);
            if (query!=null){
                var result = new EmployeeTeamModel()
                {
                    Id = query.Id,
                    TeamWorkId = query.TeamWorkId.ToString(),
                    TeamWorkTitle = query.TeamWork.Title,
                    TeamWorkCode = query.TeamWork.Code,
                    TeamStartDate = query.TeamStartDate,
                    TeamEndDate = query.TeamEndDate,
                    IsActive = query.IsActive
                };
                return result;
            }
            return new EmployeeTeamModel();
        }
    }
}
