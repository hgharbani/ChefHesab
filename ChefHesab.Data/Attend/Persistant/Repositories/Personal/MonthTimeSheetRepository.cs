using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Share.Model.Pay;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Share.Model.Salary;
using KSC.Common.Filters.Models;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Entities.Chart;

namespace Ksc.Hr.Data.Persistant.Repositories.Personal
{
    public partial class MonthTimeSheetRepository : EfRepository<MonthTimeSheet, int>, IMonthTimeSheetRepository
    {

        private readonly KscHrContext _kscHrContext;
        public MonthTimeSheetRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public MonthTimeSheet GetMonthTimeSheet(int employeeId, int yearMonth)
        {
            return _kscHrContext.MonthTimeSheets.Where(x => x.EmployeeId == employeeId && x.YearMonth == yearMonth).Include(m => m.MonthTimeSheetIncludeds).Include(m => m.MonthTimeSheetRollCalls).ThenInclude(m => m.RollCallDefinition).Include(m => m.MonthTimeSheetWorkTimes).ThenInclude(m => m.WorkTime).FirstOrDefault();

        }

        public IQueryable<CommutingOnCallVM> GetCommutingOnCall(int yearMonth)
        {
            var query = _kscHrContext.MonthTimeSheets.AsNoTracking().Where(x => x.YearMonth == yearMonth)
                .Include(x => x.Employee)
                .Include(m => m.MonthTimeSheetRollCalls)
                .SelectMany(x => x.MonthTimeSheetRollCalls)
                .Where(x => x.RollCallDefinitionId == 13 || x.RollCallDefinitionId == 26)
                ;

            var Group_query = query.GroupBy(x => new
            {
                EmployeeId = x.MonthTimeSheet.EmployeeId,
                EmployeeNumber = x.MonthTimeSheet.Employee.EmployeeNumber,
                FullName = x.MonthTimeSheet.Employee.Name + " " + x.MonthTimeSheet.Employee.Family,
                JobPositionId = x.MonthTimeSheet.Employee.JobPositionId,
                WorkCityId = x.MonthTimeSheet.Employee.WorkCityId,
            })
            .Select(x => new CommutingOnCallVM
            {
                EmployeeId = x.Key.EmployeeId,
                EmployeeNumber = x.Key.EmployeeNumber,
                FullName = x.Key.FullName,
                JobPositionId = x.Key.JobPositionId,
                WorkCityId = x.Key.WorkCityId,
                CountOfOnCall = x.Sum(b => b.DayCountInDailyTimeSheet),
            });

            var result = Group_query;//.ToList();
            return result;
        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheet()
        {
            return _kscHrContext.MonthTimeSheets
                .Include(x => x.Employee)
                .Include(m => m.MonthTimeSheetIncludeds)
                .Include(m => m.MonthTimeSheetRollCalls)
                .Include(m => m.MonthTimeSheetWorkTimes)
                ;
        }
        public IQueryable<MonthTimeSheet> GetMonthTimeSheetByYearMonthAsNoTracking(int yearMonth)
        {
            return _kscHrContext.MonthTimeSheets.AsNoTracking().Where(x => x.YearMonth == yearMonth);
        }
        public IQueryable<MonthTimeSheet> GetMonthTimeSheetAutomaticByYearMonthAsNoTracking(int yearMonth)
        {
            return GetIncludedMonthTimeSheet().AsNoTracking().Where(x => x.YearMonth == yearMonth && x.IsCreatedManual == false);
        }
        //public IQueryable<ViewTimeSheetToMis> GetVMMonthTimeSheetByYearMonthAsNoTracking(int yearMonth)
        //{
        //    return _kscHrContext.view

        //        //.AsNoTracking().Where(x => x.YearMonth == yearMonth)
        //        //.Include(x => x.Employee)
        //        //.Include(m => m.MonthTimeSheetIncludeds)
        //        //.Include(m => m.MonthTimeSheetRollCalls)
        //        //.Include(m => m.MonthTimeSheetWorkTimes)
        //        //;

        //}

        public async Task<bool> AddBulkAsync(List<MonthTimeSheet> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                    option.UseTempDB = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public IQueryable<MonthTimeSheet> GetMonthTimeSheetByRangeYearMonth(int yearMonthStart, int yearMonthEnd)
        {
            return _kscHrContext.MonthTimeSheets.Where(x => x.YearMonth >= yearMonthStart && x.YearMonth <= yearMonthEnd)
               ;

        }
        public void DeleteMonthTimeSheet(int yearMonth)
        {
            try
            {

                var monthTimeSheets = _kscHrContext.MonthTimeSheets
                    .Where(x => x.YearMonth == yearMonth && x.IsCreatedManual == false)
                    .Select(x => new MonthTimeSheet
                    {
                        Id = x.Id,
                    });
                var monthTimeSheetIds = monthTimeSheets.Select(x => x.Id);
                var monthTimeSheetIncludeds = _kscHrContext.MonthTimeSheetIncludeds
                    .Join(monthTimeSheetIds, a => a.MonthTimeSheetId, b => b, (a, b) => new MonthTimeSheetIncluded
                    {
                        Id = a.Id,
                    });

                var monthTimeSheetRollCalls = _kscHrContext.MonthTimeSheetRollCalls
                    .Join(monthTimeSheetIds, a => a.MonthTimeSheetId, b => b, (a, b) => new MonthTimeSheetRollCall
                    {
                        Id = a.Id,
                    });
                var monthTimeSheetWorkTimes = _kscHrContext.MonthTimeSheetWorkTimes
                    .Join(monthTimeSheetIds, a => a.MonthTimeSheetId, b => b, (a, b) => new MonthTimeSheetWorkTime
                    {
                        Id = a.Id,
                    });
                //.Where(x => monthTimeSheetIds.Any(y => y == x.MonthTimeSheetId));

                _kscHrContext.BulkDelete(monthTimeSheetIncludeds, bulkConfig: new BulkConfig() { UseTempDB = true });
                _kscHrContext.BulkDelete(monthTimeSheetRollCalls, bulkConfig: new BulkConfig() { UseTempDB = true });
                _kscHrContext.BulkDelete(monthTimeSheetWorkTimes, bulkConfig: new BulkConfig() { UseTempDB = true });
                _kscHrContext.BulkDelete(monthTimeSheets, bulkConfig: new BulkConfig() { UseTempDB = true });
                _kscHrContext.BulkSaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public List<List<PivoteMonthTimesheet>> GetPivoteMonthTimeSheet(string startYearMonth, string endYearMonth, string rollCallIds)
        {
            var MyList = _kscHrContext
                .CollectionFromSql("EXEC [dbo].[PivotReportMonthTimeSheet] @startYearMonth,@endYearMonth,@RollCallId",
                new Dictionary<string, object> {
                 { "@startYearMonth", startYearMonth},
                 {"@endYearMonth",endYearMonth},
                 {"@RollCallId",rollCallIds}
                }).ToList();



            //var MyListtest = _kscHrContext
            //    .CollectionFromSqlReturnGeneric<Employee>("EXEC [dbo].[AttendanceStatisticsReport] @startDateShamsi_int ,@endDateShamsi_int "
            //    , new Dictionary<string, object> {
            //     { "@startDateShamsi_int", 14030301},
            //     {"@endDateShamsi_int",14030331}
            //    }).ToList();



            return MyList;
        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeeId(List<int> employeesId, int yearMonth)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => a.YearMonth == yearMonth && employeesId.Contains(a.EmployeeId)).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds)
                ;
            return query;

        }  
        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeeId(List<int> employeesId, int yearMonth,List<int> jobPositions)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => a.YearMonth == yearMonth && employeesId.Contains(a.EmployeeId) && jobPositions.Contains(a.Employee.JobPositionId.Value)).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds)
                ;
            return query;

        }


        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetBySingleEmployeeAndYearMonthes(int employeeId, List<int> yearMonths)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => yearMonths.Contains(a.YearMonth) && a.EmployeeId == employeeId).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds);

            return query;

        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeesAndYearMonthes(List<int> employeesId, List<int> yearMonths)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => yearMonths.Contains(a.YearMonth) && employeesId.Contains(a.EmployeeId)).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds);

            return query;

        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetEmployeesForYearMonth(List<int> employeesId, int yearMonth)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => a.YearMonth == yearMonth && employeesId.Contains(a.EmployeeId)).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds);

            return query;

        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetSingleEmployeeForYearMonth(int employeeId, int yearMonth)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => a.YearMonth == yearMonth && a.EmployeeId == employeeId).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds);

            return query;

        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetByEmployeesAndYearMonthesAsQueryable(IQueryable<int> employeesId, List<int> yearMonths)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => yearMonths.Contains(a.YearMonth) && employeesId.Contains(a.EmployeeId)).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds);

            return query;
        }

        public IQueryable<MonthTimeSheet> GetIncludedMonthTimeSheetEmployeesForYearMonthAsQueryable(IQueryable<int> employeesId, int yearMonth)
        {
            var query = _kscHrContext.MonthTimeSheets
                  .Include(m => m.MonthTimeSheetIncludeds)
                  .Where(a => a.YearMonth == yearMonth && employeesId.Contains(a.EmployeeId)).AsQueryable()
                  .Include(a => a.MonthTimeSheetIncludeds);

            return query;
        }
    }
}


