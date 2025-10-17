using Ksc.Hr.Domain.Entities;
using Ksc.HR.Share.Model;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
  {
    public interface IViewAttendItemReportRepository : IRepository<ViewAttendItemReport>
    {
        IQueryable<ViewAttendItemReport> GetAllQueryable();
        IQueryable<ViewAttendItemReport> GetAllQueryableWithEmployeeIds(List<int> employeeIds);
        List<List<PivoteMonthTimesheet>> GetPivoteAttendItem(string startShamsiDate, string endShamsiDate, string rollCallIds, string FromTeam, string toTeam);
        List<IDictionary<string,object>> GetPivoteAttendItem<T>(string startShamsiDate, string endShamsiDate, string rollCallIds) where T : new();
    }
}

