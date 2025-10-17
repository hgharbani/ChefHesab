using KSC.Common;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.Share.Model;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeEntryExitService
    {
        FilterResult<SearchEmployeeEntryExitModel> GetEmployeeEntryExitByKendoFilter(FilterRequest Filter);
        CALLING_RPC GetPersonalDataMis(InputMisApiModel model);

        EmployeeEntryExitModel GetOneBySearchModel(SearchEmployeeEntryExitModel model);

        EmployeeEntryExit GetOne(int id);
        EditEmployeeEntryExitModel GetForEdit(int id);

        List<EmployeeEntryExitModel> GetEmployeeEntryExit();
        List<EntryExitResult> GetEmployeeEntryExitByDate(EntryExitListSearchModel model);
        Task<List<JobCategorySpecificModel>> GetJobCategorySpecificModels(string employeeId);
        Task<KscResult> AddJobCategorySpecificModels(List<JobCategorySpecificEntryExitModel> model,string username);
        Task<List<JobCategorySpecificModel>> GetJobCategorySpecificModelsForOneDate(int employeeId, DateTime dateTime);
        List<EmployeeEntryExitManagementModel> GetEmployeeEntryExitManagement(EmployeeEntryExitManagementInputModel inputModel);
        Task<FilterResult<EmployeeEntryExistAbsencesModel>> GetEntryExitTeamForConfirmTimeSheet(SearchEmployeeEntryExitModel inputModel);

        Task<KscResult> EditEmployeeEntryExit(List<EmployeeEntryExitManagementModel> model);
        EmployeeEntryExitYesterdayToTomorrowModel GetEmployeeEntryExitForTimeSheet(int EmployeeId, DateTime date);
        List<EmployeeEntryExitForAttendAbsenceItemModel> GetEntryExitByEmployeelist(List<int> employeeId, DateTime date);
        Task<KscResult> FindEntryExit(string startDate, string EndDate, string personalNumber);
        Task<List<EntryExitResult>> GetEmployeeEntryExitForOnCall(EntryExitForOnCallSearchModel searchModel);
        Task<FilterResult<EmployeeEntryExistAbsencesModel>> GetEntryExitTeamForConfirmTimeSheetMobile(SearchEmployeeEntryExitModel Filter);
        List<EmployeeEntryExitForAttendAbsenceItemModel> GetEntryExitByEmployee(SearchEmployeeEntryExitModel model);
        Task<List<JobCategorySpecificModel>> ReportMonthlyEntryExitPersonel(SearchEmployeeEntryExitModel filter);
          Task<SearchEmployeeEntryExitModel> GetItemDate();
        Task<List<JobCategorySpecificModel>> GetMonthlyEntryExitSpecificPersonel(SearchEmployeeEntryExitModel filter);
        List<EmployeeDontHaveExist> GEtPersenisNotHaveFamily(int yearmonth);
        // List<EmployeeEntryExitViewModel> GetEmployeeEntryExitForTimeSheet(int EmployeeId, DateTime date, int shiftConceptDetailId);
        //KscResult OnCallConfirm(List<EntryExitResult> model);
    }
}


