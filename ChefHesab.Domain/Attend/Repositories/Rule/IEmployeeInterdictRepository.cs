using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.Rule;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Domain.Entities;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IEmployeeInterdictRepository : IRepository<EmployeeInterdict, int>
    {
        IQueryable<EmployeeInterdict> GetEmployeeInterdictReport(string employeeNumber, string firstName, string lastName,
                                                                 DateTime? startDate, DateTime? endDate,int? InterdictTypeId);
        void UpdateRange(List<EmployeeInterdict> employeeInterdicts);

        EmployeeInterdict GetlatestRelatedByEmployeeId(int employeeId, DateTime? ExecuteDate,int interdictId);
        EmployeeInterdict GetandDetailById(int id);
        IQueryable<EmployeeInterdict> GetAllByRelatedGrid();
        EmployeeInterdict GetlatestRelatedByEmployeeId(int employeeId);
        EmployeeInterdict GetRelatedById(int id);
        EmployeeInterdict GetandDetailByEmployeeId(int employeeId);
        EmployeeInterdict GetlatestByEmployeeId(int employeeId);
        Task<bool> AddBulkAsync(List<EmployeeInterdict> list);
        EmployeeInterdict GetlatestRelatedByEmployeeIdForEdit(int employeeId, int? employeeInterdictId);
        bool CheckRepateDateInssuingCreate(DateTime InssuingDate, int employeeId);
        bool CheckRepateDateInssuingUpdate(DateTime InssuingDate, int employeeId, int interdictId);
        bool CheckRepateDateExecuteCreate(DateTime ExecuteDate, int employeeId);
        bool CheckRepateDateExecuteUpdate(DateTime ExecuteDate, int employeeId, int interdictId, bool isAmendment);
        IQueryable<EmployeeInterdict> GetEmployeeInterdictByEmployeeIdForPortal(int employeeId);
        Task<EmployeeInterdict> GetEmployeeInterdictByIdForPortal(int id);
        IQueryable<EmployeeInterdict> GetAllByIds(List<int> Ids);
        IQueryable<EmployeeInterdict> GetAllReportSteppr();
        IQueryable<EmployeeInterdict> GetDataByYearMonth(int yearmonth);
        bool DeleteById(int id);
        EmployeeInterdict GetRelatedByEmployeeInterdictId(int employeeInterdictId);
        Task<EmployeeInterdict> GetEmployeeInterdictByIdForPrint(int id);
        IQueryable<EmployeeInterdict> GetlatestByEmployeeIds(List<int> employeeId, bool portalFinalConfirmFlag);
        //EmployeeInterdict AddByModel(EmployeeInterdict LastEmployeeInterdict, EmployeeInterdictFamilyViewModel model);
        EmployeeInterdict AddByModelAccountCodes(EmployeeInterdict LastEmployeeInterdict, EmployeeInterdictFamilyViewModel model);
        IQueryable<EmployeeInterdict> GetAllByRelated();
        IQueryable<EmployeeInterdict> GetEmployeeInterdicttyEmployeeId(int employeeId);
        string LastInterdictNumber(int empId);
        long? OverTimeAmountByHour(int employeeId);
        List<Tuple<int, long?>> OverTimeAmountItems(List<int> employeeIds);
        long GetSpecialLiability(List<CoefficientSetting> CoefficientSettings, double? specialLiabilityScore, decimal tableFactor);
        long GetHardWork(List<CoefficientSetting> CoefficientSettings, double? HardWorkScoreFilter, decimal tableFactor);
        long GetSupervision(List<CoefficientSetting> CoefficientSettings, double? SupervisionScoreFilter, decimal tableFactor);
        long GetRadiationPercentage(double? RadiationPercentageFilter, decimal GroupJob, decimal years);
        IQueryable<EmployeeInterdict> GetLastesInterdictsPerMonthYear();
        List<EmployeeInterdict> GetPersonelInterdictChangeJobPosition(List<EmployeeInterdict> interdicts);
        IQueryable<EmployeeInterdict> GetLastesInterdictsPerMonthYearForInsert();
        IQueryable<EmployeeInterdict> GetEmployeeInterdict(List<int?> employeeIds);

        Task<List<EmployeeInterdict>> GetEmployeeInterdictByEmployeementTypeForPrint();
        EmployeeInterdict GetlatestByEmployeeIdForDetail(int employeeId);
        EmployeeInterdict GetLastInterdictEmployee(int employeeId, bool isAsnoTracking = true);
        // IEnumerable<EmployeeInterdict> GetPersonelInterdictChangeJobPosition(List<int> employeeNumbers, DateTime startDate, DateTime endDate);
        //IQueryable<EmployeeInterdict> GetInterdictForUpdateInLastLevel(string calPerianDate);
    }
}
