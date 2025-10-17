
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Share.Model.Pay;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories.Personal
{
    public interface IMonthTimeSheetRepository : IRepository<MonthTimeSheet, int>
    {
        Task<bool> AddBulkAsync(List<MonthTimeSheet> list);
        void DeleteMonthTimeSheet(int yearMonth);
        IQueryable<CommutingOnCallVM> GetCommutingOnCall(int yearMonth);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheet();
        MonthTimeSheet GetMonthTimeSheet(int employeeId, int yearMonth);
        IQueryable<MonthTimeSheet> GetMonthTimeSheetAutomaticByYearMonthAsNoTracking(int yearMonth);
        IQueryable<MonthTimeSheet> GetMonthTimeSheetByRangeYearMonth(int yearMonthStart, int yearMonthEnd);
        List<List<HR.Share.Model.PivoteMonthTimesheet>> GetPivoteMonthTimeSheet(string startYearMonth, string endYearMonth, string rollCallIds);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeeId(List<int> employeesId, int yearMonth);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeeId(List<int> employeesId, int yearMonth, List<int> jobPositions);
        IQueryable<MonthTimeSheet> GetMonthTimeSheetByYearMonthAsNoTracking(int yearMonth);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeesAndYearMonthes(List<int> employeesId, List<int> yearMonths);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeesAndYearMonthesAsQueryable(IQueryable<int> employeesId, List<int> yearMonths);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetBySingleEmployeeAndYearMonthes(int employeeId, List<int> yearMonths);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetEmployeesForYearMonth(List<int> employeesId, int yearMonth);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetEmployeesForYearMonthAsQueryable(IQueryable<int> employeesId, int yearMonth);
        IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetSingleEmployeeForYearMonth(int employeeId, int yearMonth);
    }
}

