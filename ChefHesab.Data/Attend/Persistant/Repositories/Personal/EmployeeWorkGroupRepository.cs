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

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeWorkGroupRepository : EfRepository<EmployeeWorkGroup, int>, IEmployeeWorkGroupRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeWorkGroupRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupInclouded()
        {
            var result = _kscHrContext.EmployeeWorkGroups
                .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.ShiftBoards)
                    .ThenInclude(a => a.WorkCalendar)
                .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime).AsQueryable();
            return result;
        }
        public IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupIncloudWorkGroup()
        {
            var result = _kscHrContext.EmployeeWorkGroups
                .Include(a => a.WorkGroup).AsQueryable();
            return result;
        }
        public IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupIncludeTransferRequest()
        {
            var result = _kscHrContext.EmployeeWorkGroups
                .Include(a => a.Transfer_Request).Include(a => a.WorkGroup).ThenInclude(a => a.WorkTime).AsQueryable();
            return result;
        }
        public IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup()
        {
            var result = _kscHrContext.EmployeeWorkGroups
                .Include(a => a.WorkGroup).ThenInclude(a => a.WorkTime).AsQueryable();
            return result;
        }

        public async Task<EmployeeWorkGroup> GetActiveWorkGroupByEmployeeIdAsync(int EmployeeId)
        {
            return await _kscHrContext.EmployeeWorkGroups.FirstOrDefaultAsync(x => x.EmployeeId == EmployeeId && x.IsActive);
        }
        public EmployeeWorkGroup GetActiveWorkGroupByEmployeeId(int EmployeeId)
        {
            return _kscHrContext.EmployeeWorkGroups.FirstOrDefault(x => x.EmployeeId == EmployeeId && x.IsActive);
        }
        public EmployeeWorkGroup GetLastDeActiveWorkGroupByEmployeeId(int EmployeeId, DateTime endDate)
        {
            return _kscHrContext.EmployeeWorkGroups.OrderBy(x => x.Id).Last(x => x.EmployeeId == EmployeeId && !x.IsActive && x.EndDate == endDate);
        }
        public IQueryable<EmployeeWorkGroup> GetDeActiveWorkGroupByEmployeeId(int EmployeeId)
        {
            return _kscHrContext.EmployeeWorkGroups.Where(x => x.EmployeeId == EmployeeId && !x.IsActive).Include(x => x.WorkGroup);
        }
        public async Task<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeIdDate(int employeeId, DateTime date)
        {
            return await _kscHrContext.EmployeeWorkGroups.Include(x => x.WorkGroup).ThenInclude(x => x.WorkTime).FirstOrDefaultAsync(x => x.EmployeeId == employeeId &&
                    x.StartDate <= date &&
                    (x.EndDate >= date || x.EndDate.HasValue == false));
        }
        public async Task<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeIdDateIncludeWorkGroup(int employeeId, DateTime date)
        {
            return await _kscHrContext.EmployeeWorkGroups.Where(x => x.EmployeeId == employeeId &&
                x.StartDate <= date &&
                (x.EndDate >= date || x.EndDate.HasValue == false)).Include(x => x.WorkGroup).AsNoTracking().FirstOrDefaultAsync();
        }
        public EmployeeWorkGroup GetEmployeeWorkGroupByEmployeeIdDateIncludeByWorkGroup(int employeeId, DateTime date)
        {
            return _kscHrContext.EmployeeWorkGroups.Where(x => x.EmployeeId == employeeId &&
                x.StartDate <= date &&
                (x.EndDate >= date || x.EndDate.HasValue == false)).Include(x => x.WorkGroup).AsNoTracking().FirstOrDefault();
        }
        public async Task<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeIdDateOutIncluded(int employeeId, DateTime date)
        {
            return await _kscHrContext.EmployeeWorkGroups.Where(x => x.EmployeeId == employeeId &&
                x.StartDate <= date &&
                (x.EndDate >= date || x.EndDate.HasValue == false)).AsNoTracking().FirstOrDefaultAsync();
        }

        public IEnumerable<(EmployeeWorkGroup employeeWorkGroup, int? EmployeeCityId, int? WorkTimeId, string WorkGroupCode, bool ShiftSettingFromShiftboard, bool OfficialUnOfficialHolidayFromWorkCalendar)> GetEmployeeWorkGroupWithEmployeeCityId()
        {
            var result = _kscHrContext.EmployeeWorkGroups
                                        .Include(x => x.WorkGroup)
                                        .ThenInclude(x => x.WorkTime)
                                        .Include(x => x.Employee)
                                        .Select(x => new ValueTuple<EmployeeWorkGroup, int?, int?, string, bool, bool>
                                        (x, x.Employee.WorkCityId, x.WorkGroup.WorkTimeId, x.WorkGroup.Code, x.WorkGroup.WorkTime.ShiftSettingFromShiftboard, x.WorkGroup.WorkTime.OfficialUnOfficialHolidayFromWorkCalendar));
            return result;
        }
        public IEnumerable<(EmployeeWorkGroup employeeWorkGroup, int? EmployeeCityId, int? WorkTimeId, string WorkGroupCode, bool ShiftSettingFromShiftboard, bool OfficialUnOfficialHolidayFromWorkCalendar)> GetEmployeeWorkGroupByDateWithEmployeeCityId(DateTime date)
        {
            var tt = _kscHrContext.EmployeeWorkGroups.Where(x => x.StartDate <= date &&
                   (x.EndDate >= date || x.EndDate.HasValue == false))
                                         .Include(x => x.WorkGroup)
                                        .ThenInclude(x => x.WorkTime)
                                         .Include(x => x.Employee);
            var result = _kscHrContext.EmployeeWorkGroups.Where(x => x.StartDate <= date &&
                 (x.EndDate >= date || x.EndDate.HasValue == false))
                                        .Include(x => x.WorkGroup)
                                        .ThenInclude(x => x.WorkTime)
                                        .Include(x => x.Employee)
                                        .Select(x => new ValueTuple<EmployeeWorkGroup, int?, int?, string, bool, bool>
                                        (x, x.Employee.WorkCityId, x.WorkGroup.WorkTimeId, x.WorkGroup.Code, x.WorkGroup.WorkTime.ShiftSettingFromShiftboard, x.WorkGroup.WorkTime.OfficialUnOfficialHolidayFromWorkCalendar));
            return result;
        }

        //Task<IQueryable<EmployeeWorkGroup>> IEmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded()
        //{
        //    throw new NotImplementedException();
        //}
        public IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeId(int employeeId)
        {
            return _kscHrContext.EmployeeWorkGroups.Where(x => x.EmployeeId == employeeId);
        }
        public EmployeeWorkGroup GetEmployeeWorkGroupByEmployeeIdDate(DateTime date, List<EmployeeWorkGroup> model)
        {
            return
                model.FirstOrDefault(x =>
                    x.StartDate <= date &&
                    (x.EndDate >= date || x.EndDate.HasValue == false));
        }
    }

}

