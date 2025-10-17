using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.EmployeeSafetyDeduction;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using KSC.MIS.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeSafetyDeductionService : IEmployeeSafetyDeductionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public EmployeeSafetyDeductionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public async Task<KscResult> AddByModel(List<SafetyPerformanceMonthlyVM> model)
        {
            var result = new KscResult();
            try
            {
                var today = DateTime.Now;
                var emps = model.Select(x => x.EmployeeNumber).ToList();
                var empFiltered = _kscHrUnitOfWork.EmployeeRepository.Where(x => emps.Contains(x.EmployeeNumber)).ToList();

                var dataInsert = model.Join(empFiltered,
                    m => m.EmployeeNumber,
                    e => e.EmployeeNumber,
                    (m, e) => new EmployeeSafetyDeduction
                    {
                        EmployeeId = e.Id,
                        //EmployeeNumber = m.EmployeeNumber,
                        ActionId = m.ActionId,
                        YearMonth = m.YearMonth,
                        SumPercent = m.SumPercent,
                        SumNegativeScore = m.SumNegativeScore,
                        CreateUser = m.CurrentUser,
                        CreateDateTime = today,

                    }).ToList();

                var yearMonth = model.FirstOrDefault()?.YearMonth;
                var OldData = _kscHrUnitOfWork.EmployeeSafetyDeductionRepository.Where(x => x.YearMonth == yearMonth).ToList();
                if (dataInsert.Count > 0)
                {
                    //اگر دیتای همان سال ماه بود قبلی پاک شود
                    if (OldData.Count > 0)
                    {
                        _kscHrUnitOfWork.EmployeeSafetyDeductionRepository.DeleteRange(OldData);
                    }
                    //add
                    _kscHrUnitOfWork.EmployeeSafetyDeductionRepository.AddRange(dataInsert);
                    await _kscHrUnitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                result.AddError("خطا", $"Message:{ex.Message} |InnerExceptionMessage:{ex.InnerException?.Message}");
            }


            return result;
        }

    }
}
