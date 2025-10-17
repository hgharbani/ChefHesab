using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model;
using System.Dynamic;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class ViewAttendItemReportRepository : EfRepository<ViewAttendItemReport>, IViewAttendItemReportRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewAttendItemReportRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<ViewAttendItemReport> GetAllQueryable()
        {
            var result = _kscHrContext.ViewAttendItemReport.AsQueryable();
            return result;
        }

        public IQueryable<ViewAttendItemReport> GetAllQueryableWithEmployeeIds(List<int> employeeIds)
        {
            var result = _kscHrContext.ViewAttendItemReport.AsQueryable()
                .Where(a => employeeIds.Contains(a.EmployeeId));


            return result;
        }

        public List<List<PivoteMonthTimesheet>> GetPivoteAttendItem(string startShamsiDate, string endShamsiDate, string rollCallIds,string FromTeam,string toTeam)
        {
            var MyList = _kscHrContext
                .CollectionFromSql("EXEC [dbo].[PivotReportAttendItem] @startShamsiDate,@endShamsiDate,@RollCallId,@FromTeam,@ToTeam",
                new Dictionary<string, object> {
                 { "@startShamsiDate", startShamsiDate},
                 {"@endShamsiDate",endShamsiDate},
                 {"@RollCallId",rollCallIds},
                 {"@FromTeam",FromTeam},
                 {"@ToTeam",toTeam},
                }).ToList();

     
            return MyList;
        }

        public List<IDictionary<string,object>> GetPivoteAttendItem<T>(string startShamsiDate, string endShamsiDate, string rollCallIds) where T : new()
        {
            var MyList = _kscHrContext
                .CollectionFromSqlReturnListDictunary("EXEC [dbo].[PivotReportAttendItem] @startShamsiDate,@endShamsiDate,@RollCallId",
                new Dictionary<string, object> {
                 { "@startShamsiDate", startShamsiDate},
                 {"@endShamsiDate",endShamsiDate},
                 {"@RollCallId",""}
                }).ToList();
            return MyList;
           
        }
    }
}


