using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeSafetyDeduction;
using KSC.Common;
using KSC.Common.Filters.Models;
using KSC.MIS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeSafetyDeductionService
    {
        Task<KscResult> AddByModel(List<SafetyPerformanceMonthlyVM> model);
    }
}
