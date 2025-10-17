
using System.Collections.Generic;
using System.Threading.Tasks;
using Ksc.HR.DTO.Personal.EmployeeTimeSheet;
using Ksc.HR.DTO.Report;
using Ksc.HR.DTO.Stepper;
using KSC.Common;
using KSC.Common.Filters.Models;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeTimeSheetService
    {
        KscResult AddOrUpdateEmployeetimeSheet(List<EmployeeTimeSheetMonthModel> models, string currentUser);
        FilterResult<EmployeeTimeSheetGridManageModel> GetEmployeeOverTimeDetailForBalanceAverage(ReportSearchModel Filter);

        //Task<EmployeeEmployeeTimeSheetModel> GetEmployeeTimeSheetModel(int employeeID, int yearMonth);
        //Task<KscResult> EmployeeTimeSheetCalculate(SearchEmployeeTimeSheetModel model);
        //Task<KscResult> EmployeeTimeSheetSendToMIS(SearchEmployeeTimeSheetModel model);
        //Task<KscResult> PostEmployeeTimeSheet(EmployeeEmployeeTimeSheetModel model);
        FilterResult<EmployeeTimeSheetMonthReportModel> GetEmployeeTimeSheetByRelated(EmployeeTimeSheetMonthReportSearchModel Filter);

        Task<KscResult> AddTrainingOverTime(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> AddEmployeeCompensatoryOverTime(UpdateStatusByYearMonthProcedureModel model);
    }
}
