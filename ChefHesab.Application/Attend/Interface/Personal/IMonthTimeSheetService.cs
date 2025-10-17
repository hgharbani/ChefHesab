//using Ksc.HR.DTO.Personal.MonthTimeSheets;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using System.Threading.Tasks;
using KSC.Common;
using System;
using Ksc.HR.DTO.Stepper;
using System.Collections.Generic;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.Pay;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IMonthTimeSheetService
    {
        Task<EmployeeMonthTimeSheetModel> GetEmployeeTimeSheetModel(int employeeID, int yearMonth);
        List<List<PivoteMonthTimesheet>> GetPivoteMonthTimeSheet(SearchPivoteReport model);
        Task<KscResult> MonthTimeSheetCalculate(SearchMonthTimeSheetModel model);
        Task<KscResult> MonthTimeSheetCalculateStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> MonthTimeSheetDeleteStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> MonthTimeSheetSendToMIS(SearchMonthTimeSheetModel model);
        Task<KscResult> MonthTimeSheetSendToMISStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> PostMonthTimeSheet(EmployeeMonthTimeSheetModel model);
        List<ReportMonthTimeSheet> ReportMonthTimeSheet(SearchPivoteReport search);
        Task<KscResult> TestCeilingOvertimeTimeSheetStep(UpdateStatusByYearMonthProcedureModel model);
        Task<KscResult> TestMonthTimeSheetCalculateStep(UpdateStatusByYearMonthProcedureModel model);

        Task<KscResult> TestSendFileStreamMIS(SearchMonthTimeSheetModel model);

    }
}
