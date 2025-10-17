using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Repositories;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class ShiftBoardRepository : EfRepository<ShiftBoard, int>, IShiftBoardRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ShiftBoardRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public void AddReangeShiftBoard(IEnumerable<ShiftBoard> entities)
        {
            _kscHrContext.AddRange(entities);
        }

        public IQueryable<ShiftBoard> GetShiftBoardEmployeeIncluded()
        {
            var shiftBoard = _kscHrContext.ShiftBoards.Include(a => a.WorkCalendar).Include(a => a.WorkGroup).ThenInclude(a => a.EmployeeWorkGroups).AsQueryable();
            return shiftBoard;
        }

        public IQueryable<ShiftBoard> GetShiftBoardEmployee(int employeeId, DateTime StartDate, DateTime EndDate)
        {

            var shiftBoards = _kscHrContext.ShiftBoards
               .Include(a => a.WorkCalendar)
               .Include(a => a.WorkGroup).ThenInclude(a => a.EmployeeWorkGroups)
               .Where(a => StartDate.Date <= a.WorkCalendar.MiladiDateV1 && EndDate.Date >= a.WorkCalendar.MiladiDateV1);
            shiftBoards = shiftBoards.Where(a => a.WorkGroup.EmployeeWorkGroups.Any(c => c.EmployeeId == employeeId) && a.WorkGroup.Code != "R");
            return shiftBoards;
        }

        public bool ISHaveShiftBoard(int employeeId, DateTime StartDate, DateTime EndDate)
        {
            var shiftBoards = _kscHrContext.ShiftBoards.Where(a => StartDate.Date <= a.WorkCalendar.MiladiDateV1 && EndDate.Date >= a.WorkCalendar.MiladiDateV1)
                .Include(a => a.WorkCalendar)
                .Include(a => a.WorkGroup).ThenInclude(a => a.EmployeeWorkGroups).AsNoTracking()
                ;
            shiftBoards = shiftBoards.Where(a => (a.WorkGroup.EmployeeWorkGroups.Any(c => c.EmployeeId == employeeId ) && a.WorkGroup.Code != "R"));
            return shiftBoards.Any();
        }
        public bool IsRestShiftInShiftBorad(int employeeId, DateTime date)
        {
            var employeeWorkGroup = _kscHrContext.EmployeeWorkGroups.FirstOrDefault(x => x.EmployeeId == employeeId && x.StartDate <= date && (x.EndDate >= date || x.EndDate.HasValue == false));
            //var 
            if (employeeWorkGroup == null)
                return true;
            var shiftBoard = _kscHrContext.ShiftBoards
               .AsNoTracking()
                .Where(a => date == a.WorkCalendar.MiladiDateV1 && a.WorkGroupId == employeeWorkGroup.WorkGroupId).Include(x => x.ShiftConceptDetail).ThenInclude(x => x.ShiftConcept).FirstOrDefault();
            //   shiftBoards =;
            if (shiftBoard == null)
                return true;
            //شروع پایان شیفت
            return shiftBoard.ShiftConceptDetail.ShiftConcept.IsRest;
        }
        public ShiftBoard GetShiftBoardByworkGoupIdWorkCalendarId(int workGoupId, int workcalendarId)
        {
            var shiftBoard = _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).AsQueryable();
            return shiftBoard.FirstOrDefault(x => x.WorkCalendarId == workcalendarId && x.WorkGroupId == workGoupId);
        }
        public IQueryable<ShiftBoard> GetShiftBoardByworkGoupIds(List<int> workGoupId, int workCalendarId)
        {
            var shiftBoard = _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).AsQueryable();
            return shiftBoard.Where(x => workGoupId.Contains(x.WorkGroupId) && x.WorkCalendarId == workCalendarId).AsQueryable();
        }
        public IQueryable<ShiftBoard> GetShiftBoardByworkGoupIdsAndWorkCalendarIds(List<int> workGoupId, List<int> workCalendarIds)
        {
            var shiftBoard = _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).AsQueryable();
            return shiftBoard.Where(x => workGoupId.Contains(x.WorkGroupId) && workCalendarIds.Contains(x.WorkCalendarId)).AsQueryable();
        }
        public IQueryable<ShiftBoard> GetShiftBoardByWorkCalendarId(int workcalendarId)
        {
            return _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).Where(x => x.WorkCalendarId == workcalendarId);

        }
        public async Task<ShiftBoard> GetShiftBoardByworkGoupIdWorkCalendarIdAsync(int workGoupId, int workcalendarId)
        {
            var shiftBoard = _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).AsQueryable();
            return await shiftBoard.FirstOrDefaultAsync(x => x.WorkCalendarId == workcalendarId && x.WorkGroupId == workGoupId);
        }
        public IQueryable<ShiftBoard> GetShiftBoardByWorkCalendarAsNoTracking(int workcalendarId)
        {
            return _kscHrContext.ShiftBoards.Where(x => x.WorkCalendarId == workcalendarId).Include(a => a.WorkGroup).Include(x => x.ShiftConceptDetail).AsNoTracking();

        }
        public IQueryable<ShiftBoard> GetShiftBoardIncludeShiftConceptDetail()
        {
            return _kscHrContext.ShiftBoards.Include(x => x.ShiftConceptDetail).AsNoTracking();

        }
        public IQueryable<ShiftBoard> GetShiftBoardByYearMothNoTracking(int yearMonth)
        {
            return _kscHrContext.ShiftBoards.Include(a => a.WorkCalendar)
                 .Where(x => x.WorkCalendar.YearMonthV1 == yearMonth).Include(a => a.WorkGroup).AsNoTracking();


        }
    }
}
