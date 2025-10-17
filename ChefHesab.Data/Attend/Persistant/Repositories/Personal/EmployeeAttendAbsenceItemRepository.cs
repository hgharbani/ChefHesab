using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeAttendAbsenceItemRepository : EfRepository<EmployeeAttendAbsenceItem, long>, IEmployeeAttendAbsenceItemRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeAttendAbsenceItemRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemValidRecordAsNoTracking()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.InvalidRecord == false).AsNoTracking();

            return query;
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemValidRecordAndInCludedEmployeeAsNoTracking()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.InvalidRecord == false).Include(a => a.Employee).AsNoTracking();

            return query;
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemActiveByEmployeeIdAsNoTracking(int employeeId)
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.EmployeeId == employeeId && x.InvalidRecord == false).AsNoTracking();

            return query;
            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemForOncallByRelatedAsNoTracking()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.WorkCalendar).ThenInclude(x => x.WorkDayType).AsNoTracking();

            return query;
            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByRelated()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.Employee)
                .Include(x => x.WorkCalendar).ThenInclude(x => x.WorkDayType)
                .Include(x => x.RollCallDefinition).ThenInclude(x => x.RollCallCategory)
                .Include(x => x.RollCallDefinition).ThenInclude(x => x.RollCallConcept)
                .Include(x => x.WorkTime).ThenInclude(x => x.WorkGroups).AsQueryable();
            return query;
            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.WorkCalendar).AsQueryable();
            return query;
            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemIncludEmployeeEntryExitAttendAbsences()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.EmployeeEntryExitAttendAbsences).AsQueryable();
            return query;
            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemAsNoTracking()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.AsNoTracking();
            return query;
        }



        //public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByRelated1111(List<int> employees)
        //{
        //    //List<EmployeeAttendAbsenceItem> employeeAttendAbsenceItems =new List<EmployeeAttendAbsenceItem>();
        //   // var employeeAttendAbsenceItems = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.Employee).Where(x => employees.Contains(x.EmployeeId).AsQueryable();
        //    return _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.Employee).Where(x => employees.Contains(x.EmployeeId).AsQueryable();

        //}



        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemIncludedShiftConceptDetail()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.WorkCalendar).Include(x => x.ShiftConceptDetail_ShiftConceptDetailId).AsQueryable();
            return query;
            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(int employeeId, int workCalendarId)
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.RollCallDefinition).AsQueryable();
            if (employeeId > 0)
            {
                query = query.Where(a => a.EmployeeId == employeeId);
            }
            if (workCalendarId > 0)
            {
                query = query.Where(x => x.WorkCalendarId == workCalendarId);
            }
            return query.AsNoTracking();
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetAllQuaryble(List<long> ids)
        {
            var dataQuary = _kscHrContext.EmployeeAttendAbsenceItems.AsQueryable();
            if (ids.Any())
            {
                dataQuary = dataQuary.Where(a => ids.Contains(a.Id));
            }
            return (dataQuary);
        }

        //public async Task<List<SpEmployeeTeamNotConfirmedReturnModel>> GetSpEmployeeTeamNotConfirmedAsync(DateTime? startDate, DateTime? endDate, string userName)
        //{
        //    var startDateParam = new SqlParameter { ParameterName = "@StartDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = startDate.GetValueOrDefault() };
        //    if (!startDate.HasValue)
        //        startDateParam.Value = DBNull.Value;

        //    var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
        //    if (!endDate.HasValue)
        //        endDateParam.Value = DBNull.Value;

        //    var userNameParam = new SqlParameter { ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = userName, Size = 40 };
        //    if (userNameParam.Value == null)
        //        userNameParam.Value = DBNull.Value;


        //    const string sqlCommand = "EXEC [dbo].[Sp_EmployeeTeamNotConfirmed] @StartDate, @EndDate, @UserName";
        //    var procResultData = await _kscHrContext.SpEmployeeTeamNotConfirmedReturnModels
        //        .FromSqlRaw(sqlCommand, startDateParam, endDateParam, userNameParam)
        //        .ToListAsync();

        //    return procResultData;
        //}

        public async Task<List<SpEmployeeTeamNotConfirmedReturnModel>> GetSpEmployeeTeamNotConfirmedReportAsync(DateTime? startDate, DateTime? endDate, string userName, string startteamCode, string endteamCode, string personelNumberCode)
        {
            var startDateParam = new SqlParameter { ParameterName = "@StartDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = startDate.GetValueOrDefault() };
            if (!startDate.HasValue)
                startDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!endDate.HasValue)
                endDateParam.Value = DBNull.Value;
            var usernameParam = new SqlParameter { ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = userName, Size = 40 };
            if (userName == null)
                usernameParam.Value = DBNull.Value;

            var startteamCodeParam = new SqlParameter { ParameterName = "@StartTeamCode", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = startteamCode, Size = 40 };
            if (startteamCodeParam.Value == null)
                startteamCodeParam.Value = DBNull.Value;

            var endteamCodeParam = new SqlParameter { ParameterName = "@EndTeamCode", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = endteamCode, Size = 40 };
            if (endteamCodeParam.Value == null)
                endteamCodeParam.Value = DBNull.Value;
            var personnelNumberParam = new SqlParameter { ParameterName = "@PersonelNumber", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = personelNumberCode, Size = 40 };
            if (personnelNumberParam.Value == null)
                personnelNumberParam.Value = DBNull.Value;
            const string sqlCommand = "EXEC [dbo].[Sp_EmployeeTeamNotConfirmedReport] @StartDate, @EndDate, @StartTeamCode,@EndTeamCode,@UserName,@PersonelNumber";
            var procResultData = await _kscHrContext.SpEmployeeTeamNotConfirmedReturnModels
                .FromSqlRaw(sqlCommand, startDateParam, endDateParam, startteamCodeParam, endteamCodeParam, usernameParam, personnelNumberParam)
                .ToListAsync();

            return procResultData;
        }

        //MonthTimeSheetCalculate
        public IQueryable<EmployeeAttendAbsenceItem> GetMonthTimeSheetCalculateAsNoTracking(int yearMonth)
        {

            var employee = _kscHrContext.Employees.Include(x => x.EmploymentType).Where(x => x.EntryExitTypeId != 4 &&//(4)	افرادی که کارکرد ماهیانه آنها به صورت دستی 
                                x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id && x.EmploymentType.IsCreatedManualMonthTimeSheet != true);
            var paymentStatusIds = EnumEmployee.PaymentStatusId;
            var finalEmployee = employee.Where(x => !paymentStatusIds.Contains(x.PaymentStatusId));//.ToList();//افرادی که وضعیت اشتغال آنها 1و5و7و12و18و9  می باشد


            var attendAbcenseItem = GetEmployeeAttendAbsenceItemAsNoTracking()//GetEmployeeAttendAbsenceItemValidRecordAsNoTracking()
                .Include(x => x.WorkCalendar)
                .Include(x => x.RollCallDefinition)//.ThenInclude(x=>x.IncludedRollCalls).ThenInclude(x=>x.IncludedDefinition)
                .Where(x => x.WorkCalendar.YearMonthV1 == yearMonth
                // && x.RollCallDefinition.IncludedRollCalls.Any(y=>y.IncludedDefinition.IsMonthlyTimeSheet == true)
                ).AsQueryable();

            var employeeAttendAbsenceItem = (from item in attendAbcenseItem
                                             join emp in finalEmployee
                                             on item.EmployeeId equals emp.Id
                                             where
                                             (
                                             (emp.DismissalDate >= item.WorkCalendar.MiladiDateV1 && emp.DismissalDate != null && emp.EmploymentDate <= item.WorkCalendar.MiladiDateV1)
                                             ||
                                             emp.DismissalDate == null && emp.EmploymentDate <= item.WorkCalendar.MiladiDateV1)


                                             select item

                                             );



            return employeeAttendAbsenceItem;

            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetMonthTimeSheetEmployeeCalculateAsNoTracking(int yearMonth, long employeeId)
        {



            var attendAbcenseItem = GetEmployeeAttendAbsenceItemAsNoTracking()
                .Include(x => x.WorkCalendar)
                .Include(x => x.RollCallDefinition)
                .Where(x => x.WorkCalendar.YearMonthV1 == yearMonth && x.EmployeeId == employeeId).AsQueryable();

            return attendAbcenseItem;

            //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetAll();
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByOverTimeToken(string overTimeToken)
        {
            return _kscHrContext.EmployeeAttendAbsenceItems.AsQueryable().Where(x => x.OverTimeToken == overTimeToken).AsQueryable();
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdNoIncludedAsNoTracking(int employeeId, int workCalendarId)
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems
                .Where(x => x.EmployeeId == employeeId && x.WorkCalendarId == workCalendarId);
            return query.AsNoTracking();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByYearMonthAsNoTracking(int yearMonth)
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.WorkCalendar).Where(x => x.WorkCalendar.YearMonthV1 == yearMonth);
            return query.AsNoTracking();
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetItemsHolidayByworkcalendarIds(int employeeId, List<int> workcalendarIds)
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.AsQueryable()
                      .Where(a => a.EmployeeId == employeeId && workcalendarIds.Contains(a.WorkCalendarId)).AsQueryable();
            return query;

        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeEntry_ExitAttendAbsencesByWorkcalendarIds(int employeeId, List<int> workcalendarIds)
        {
            var query = GetItemsHolidayByworkcalendarIds(employeeId, workcalendarIds).Include(x => x.EmployeeEntryExitAttendAbsences);

            return query;

        }

        public IQueryable<EmployeeAttendAbsenceItem> GetValidItems()
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.InvalidRecord == false);
            return query;

        }



        public List<SpGetEmployeeLeaveStatusReturnModel> SpGetEmployeeLeaveStatus(int? from, int? to)
        {
            var fromParam = new SqlParameter { ParameterName = "@From", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = from.GetValueOrDefault(), Precision = 10, Scale = 0 };
            if (!from.HasValue)
                fromParam.Value = DBNull.Value;

            var toParam = new SqlParameter { ParameterName = "@To", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = to.GetValueOrDefault(), Precision = 10, Scale = 0 };
            if (!to.HasValue)
                toParam.Value = DBNull.Value;

            const string sqlCommand = "EXEC [dbo].[SP_GetEmployeeLeaveStatus] @From, @To";
            var procResultData = _kscHrContext.SpGetEmployeeLeaveStatusReturnModels
                .FromSqlRaw(sqlCommand, fromParam, toParam)
                .ToList();


            return procResultData;
        }



        public List<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByMissionId(int MissionRequestId)
        {
            //var misionRequest = _kscHrContext.Mission_Requests.First(a => a.Id == MissionRequestId);

            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.MissionRequestId == MissionRequestId).ToList();
            return query;
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByRangeDateAndRollCallConcept(DateTime startDate, DateTime endDate, int rollCallConceptId)
        {

            var startDateCheck = startDate.Date;
            var endDateCheck = endDate.Date;
            var quesry = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.WorkCalendar).Include(x => x.RollCallDefinition)
                .Where(x => x.RollCallDefinition.RollCallConceptId == rollCallConceptId && x.WorkCalendar.MiladiDateV1 >= startDateCheck && x.WorkCalendar.MiladiDateV1 <= endDateCheck);
            return quesry;
        }

        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployeeIdWorkCalendarIdAsNoTracking(List<int> employeeId, int workCalendarId)
        {
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.WorkCalendarId == workCalendarId &&
            employeeId.Any(e => e == x.EmployeeId));

            return query.Include(x => x.Employee).AsNoTracking();
        }
        public IQueryable<EmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByMissionRequestId(int MissionRequestId)
        {
            //var misionRequest = _kscHrContext.Mission_Requests.First(a => a.Id == MissionRequestId);

            var query = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.MissionRequestId == MissionRequestId);
            return query;
        }
        public int GetEmployeeAttendAbsenceItemByEmployeeIdForConditionalAbsence(int employeeId, int rollcallDefinitionId, DateTime date)
        {
            var persianWeekStartAndEndDates = date.GetPersianWeekStartAndEndDates();
            var weekStartDate = persianWeekStartAndEndDates.StartDate;
            var weekEndDate = persianWeekStartAndEndDates.EndDate;
            //var workCalendarsWeek = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByRangeDateAsNotracking(weekStartDate, weekEndDate).Select(x => x.Id);
            var query = _kscHrContext.EmployeeAttendAbsenceItems.Include(x => x.WorkCalendar).Where(
                x => x.EmployeeId == employeeId && x.RollCallDefinitionId == rollcallDefinitionId &&
                x.WorkCalendar.MiladiDateV1 >= weekStartDate && x.WorkCalendar.MiladiDateV1 <= weekEndDate
                ).Include(x => x.WorkCalendar).Count();
            return query;
        }
        public async Task<List<SpEmployeeTeamNotConfirmedReturnModel>> GetSpGetEmployeeDontHaveAttendItemModelReportAsync(DateTime? startDate, DateTime? endDate, string startteamCode, string endteamCode, string personelNumberCode)
        {
            try
            {


                var startDateParam = new SqlParameter { ParameterName = "@StartDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = startDate.GetValueOrDefault() };
                if (!startDate.HasValue)
                    startDateParam.Value = DBNull.Value;

                var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
                if (!endDate.HasValue)
                    endDateParam.Value = DBNull.Value;
                var startteamCodeParam = new SqlParameter { ParameterName = "@StartTeamCode", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = startteamCode, Size = 40 };
                if (startteamCodeParam.Value == null)
                    startteamCodeParam.Value = DBNull.Value;

                var endteamCodeParam = new SqlParameter { ParameterName = "@EndTeamCode", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = endteamCode, Size = 40 };
                if (endteamCodeParam.Value == null)
                    endteamCodeParam.Value = DBNull.Value;
                var personnelNumberParam = new SqlParameter { ParameterName = "@PersonelNumber", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = personelNumberCode, Size = 40 };
                if (personnelNumberParam.Value == null)
                    personnelNumberParam.Value = DBNull.Value;
                const string sqlCommand = "EXEC [dbo].[Sp_EmployeeDontHaveAttendItemReport] @StartDate, @EndDate, @StartTeamCode,@EndTeamCode,@PersonelNumber";
                var procResultData = await _kscHrContext.SpEmployeeTeamNotConfirmedReturnModels
                    .FromSqlRaw(sqlCommand, startDateParam, endDateParam, startteamCodeParam, endteamCodeParam, personnelNumberParam)
                    .ToListAsync();

                return procResultData;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
