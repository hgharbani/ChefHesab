using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Personal.MonthTimeShitStepper;
using KSC.Common;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.Hr.Application.Interfaces.Personal
{
    public interface IMonthTimeShitStepperService
    {
        Task<KscResult> AddMonthTimeShitStepper(AddOrEditMonthTimeShitStepperModel model);
        bool ExistsByOrderNo(int orderNo);
        bool ExistsByOrderNo(int id, int orderNo);
        bool ExistsByTitle(int id, string name);
        bool ExistsByTitle(string name);
        AddOrEditMonthTimeShitStepperModel GetForEdit(int id);
        List<MonthTimeShitStepperViewModel> GetMonthTimeShitStepper();
        List<MonthTimeShitStepperModel> GetMonthTimeShitStepperActive(string yearMonth);
        FilterResult<MonthTimeShitStepperViewModel> GetMonthTimeShitStepperByFilter(FilterRequest Filter);
        List<SearchMonthTimeShitStepperModel> GetMonthTimeShitStepperByKendoFilter(FilterRequest Filter);
        MonthTimeShitStepper GetOne(int id);
        Task<KscResult> RemoveMonthTimeShitStepper(AddOrEditMonthTimeShitStepperModel model);
        Task<KscResult> UpdateMonthTimeShitStepper(AddOrEditMonthTimeShitStepperModel model);
    }
}

