using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IShiftBoardRepository : IRepository<ShiftBoard, int>
    {
        void AddReangeShiftBoard(IEnumerable<ShiftBoard> entities);
        IQueryable<ShiftBoard> GetShiftBoardEmployeeIncluded();
        IQueryable<ShiftBoard> GetShiftBoardEmployee(int employeeId, DateTime StartDate, DateTime EndDate);
        bool ISHaveShiftBoard(int employeeId, DateTime StartDate, DateTime EndDate);
        ShiftBoard GetShiftBoardByworkGoupIdWorkCalendarId(int workGoupId, int workcalendarId);

        IQueryable<ShiftBoard> GetShiftBoardByworkGoupIds(List<int> workGoupId, int workCalendarId);

        IQueryable<ShiftBoard> GetShiftBoardByWorkCalendarId(int workcalendarId);
        Task<ShiftBoard> GetShiftBoardByworkGoupIdWorkCalendarIdAsync(int workGoupId, int workcalendarId);
        bool IsRestShiftInShiftBorad(int employeeId, DateTime date);
        IQueryable<ShiftBoard> GetShiftBoardByWorkCalendarAsNoTracking(int workcalendarId);
        IQueryable<ShiftBoard> GetShiftBoardByworkGoupIdsAndWorkCalendarIds(List<int> workGoupId, List<int> workCalendarIds);
        IQueryable<ShiftBoard> GetShiftBoardIncludeShiftConceptDetail();
        IQueryable<ShiftBoard> GetShiftBoardByYearMothNoTracking(int yearMonth);
    }
}
