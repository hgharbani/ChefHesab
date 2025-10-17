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
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Domain.Repositories;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeTeamWorkRepository : EfRepository<EmployeeTeamWork, int>, IEmployeeTeamWorkRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeTeamWorkRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<EmployeeTeamWork> GetActiveTeamWorkByEmployeeIdAsync(int EmployeeId)
        {
            return await _kscHrContext.EmployeeTeamWorks.Include(x => x.TeamWork).FirstOrDefaultAsync(x => x.EmployeeId == EmployeeId && x.IsActive);
        }
        public EmployeeTeamWork GetActiveTeamWorkByEmployeeId(int EmployeeId)
        {
            return _kscHrContext.EmployeeTeamWorks.Include(x => x.TeamWork)
                .Include(x => x.TeamWork)
                .FirstOrDefault(x => x.EmployeeId == EmployeeId && x.IsActive);
        }

        public IQueryable<EmployeeTeamWork> GetAllQueryable()
        {
            return _kscHrContext.EmployeeTeamWorks.AsQueryable();
        }
        public IQueryable<EmployeeTeamWork> GetAllTeamWorkInclude()
        {
            return GetAllQueryable().Include(x => x.Employee).Include(x => x.TeamWork);
        }
        public IQueryable<EmployeeTeamWork> GetEmployeeTeamWorkByTeamWorkIdAsNoTracking(int teamWorkId, DateTime date)
        {
            return _kscHrContext.EmployeeTeamWorks.Where(a => a.IsActive == true && a.TeamWorkId == teamWorkId
            && a.TeamStartDate <= date && (a.TeamEndDate >= date || a.TeamEndDate.HasValue == false)
            ).AsNoTracking();
        }

        public IQueryable<int> GetActivePersonsIdWithDate(DateTime date)
        {
            return _kscHrContext.EmployeeTeamWorks
                //.Where(x => x.IsActive)
                .Where(x => x.TeamEndDate >= date || x.TeamEndDate.HasValue == false || x.IsActive)
                .Include(x => x.Employee).Where(x =>
                x.Employee.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.Employee.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.Employee.EntryExitTypeId != EnumEntryExitType.Manual.Id).Select(x => x.EmployeeId);

        }

        public IQueryable<int> GetActivePersonsIdByEmployeeIdsWithDate(List<int> ids, DateTime date)
        {
            var result = this.GetActivePersonsIdWithDate(date).Where(x => ids.Contains(x));
            return result;

        }

        public IQueryable<int> GetActivePersonsId()
        {
            return _kscHrContext.EmployeeTeamWorks
                .Where(x => x.IsActive)
                .Include(x => x.Employee).Where(x =>
                x.Employee.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.Employee.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.Employee.EntryExitTypeId != EnumEntryExitType.Manual.Id).Select(x => x.EmployeeId);

        }

        public IQueryable<int> GetActivePersonsIdByEmployeeIds(List<int> ids)
        {
            var result = this.GetActivePersonsId().Where(x => ids.Contains(x));
            return result;

        }

        public IQueryable<int> GetActivePersonsIdForOfficialHoliday(int workCalendarId, DateTime date)
        {
            var person = _kscHrContext.EmployeeTeamWorks.Where(x => x.IsActive).Include(x => x.Employee).Where(x =>
            x.Employee.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.Employee.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.Employee.EntryExitTypeId != EnumEntryExitType.Manual.Id);
            var item = _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.WorkCalendarId == workCalendarId);
            // var entryExit = _kscHrContext.EmployeeEntryExits.Where(x => x.EntryExitDate == date && x.IsDeleted == false);
            var query = person.Select(x => x.EmployeeId).Except(item.Select(x => x.EmployeeId));
            // .Except(entryExit.Select(x => x.EmployeeId));
            return query;

        }
        public IQueryable<Employee> GetActivePersonsForValidOverTimeByEmployeeIds(List<int> ids, DateTime date/*, bool isShift = false*/)
        {
            var result = this.GetActivePersonsForValidOverTime(date/*, isShift*/).Where(x => ids.Contains(x.Id));
            return result;

        }
        public IQueryable<Employee> GetActivePersonsForValidOverTime(DateTime date/*, bool isShift*/)
        {
            return _kscHrContext.EmployeeTeamWorks
                //.Where(x => x.IsActive)
                .Where(x => x.TeamEndDate >= date || x.TeamEndDate.HasValue == false || x.IsActive)
                .Include(x => x.Employee).ThenInclude(x => x.WorkGroup)
                .Include(x => x.Employee).ThenInclude(x => x.TeamWork)
                .Where(x =>
                x.Employee.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.Employee.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&/* (isShift == false ? */x.Employee.WorkGroup.WorkTimeId == 1/* : x.Employee.WorkGroup.WorkTimeId != 1)*/).Select(x => x.Employee);

        }
        //public IQueryable<EmployeeTeamWork> GetEmployeeTeamWorkInclouded()
        //{
        //    var result = _kscHrContext.EmployeeTeamWorks
        //        .Include(a => a.id).ThenInclude(a => a.WorkTime).AsQueryable();
        //    return result;
        //}
        public IQueryable<EmployeeTeamWork> GetEmployeeTeamWorkByTeamWorkId(int teamWorkId, DateTime entryExitDate)
        {
            var activeEmployeeTeamWroks = _kscHrContext.EmployeeTeamWorks
                 .Where(x => x.IsActive == true && x.TeamWorkId == teamWorkId
                 && x.TeamStartDate <= entryExitDate && (x.TeamEndDate >= entryExitDate || x.TeamEndDate.HasValue == false)
                 );
            return activeEmployeeTeamWroks;
        }
    }
}

