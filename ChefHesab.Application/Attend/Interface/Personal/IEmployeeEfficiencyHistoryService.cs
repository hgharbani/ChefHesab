using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeEfficiencyHistory;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Report;
using Ksc.HR.DTO.Stepper;
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
    public interface IEmployeeEfficiencyHistoryService
    {
        
        public KscResult CreateEmployeeEfficiency(int yearMonth);
        KscResult SaveEmployeeEfficiency(EmployeeEfficiencyGridManageModel models, string currentUser, bool IsSalaryUser);
        FilterResult<EmployeeEfficiencyGridManageModel> GetEmployeeForEfficiency(ReportSearchModel Filter);

        FilterResult<EmployeeEfficiencyHistoryModel> GetReportEmployeeEfficiencyHistoryData(SearchEmployeeEfficiencyHistoryModel Filter);

        Task<KscResult> EmployeeEfficiencyLatestSendToMIS(SearchMonthTimeSheetModel model);
        Task<KscResult> CreateEmployeeEfficiencyStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> EmployeeEfficiencyLatestSendToMISStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> EmployeeEfficiencyChangeStatus(UpdateStatusByYearMonthProcedureModel model);
        FilterResult<EmployeeEfficiencyGridManageModel> GetEmployeeForEfficiencyReportMonth(ReportSearchModel Filter);
        ////---------------------------------------
        //       //void ExistsByCode(int id, string code);
        //void Exists(int id, string name);
        //void Exists(string name);
        //List<EmployeeEfficiencyHistoryModel> GetEmployeeEfficiencyHistorys();
        ////FilterResult<EmployeeEfficiencyHistoryModel> GetEmployeeEfficiencyHistorysByFilter(FilterRequest Filter);
        //EmployeeEfficiencyHistory GetOne(int id);
        //List<SearchEmployeeEfficiencyHistoryModel> GetEmployeeEfficiencyHistorysByKendoFilter(FilterRequest Filter);
        //EditEmployeeEfficiencyHistoryModel GetForEdit(int id);

        //Task<KscResult> AddEmployeeEfficiencyHistory(AddEmployeeEfficiencyHistoryModel model);
        //Task<KscResult> UpdateEmployeeEfficiencyHistory(EditEmployeeEfficiencyHistoryModel model);
        //Task<KscResult> RemoveEmployeeEfficiencyHistory(EditEmployeeEfficiencyHistoryModel model);
    }
}
