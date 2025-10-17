using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeEntryExitRepository : EfRepository<EmployeeEntryExit, long>, IEmployeeEntryExitRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeEntryExitRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<EmployeeEntryExit> GetEmployeeEntryExitValidByDate(string person, DateTime entryExitDate)
        {
            var entryExitValid = _kscHrContext.EmployeeEntryExits.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.PersonalNumber == person);
            var data = entryExitValid.Where(x => x.EntryExitDate.Date == entryExitDate.Date);
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitByDate(int employeeId, DateTime entryExitDate)
        {

            // var employee = _kscHrContext.Employees.FirstOrDefault(x => x.Id == employeeId);
            var data = _kscHrContext.EmployeeEntryExits.AsQueryable()
                .Where(x => x.EmployeeId == employeeId && x.EntryExitDate.Date == entryExitDate.Date).AsQueryable();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdDate(int employeeId, DateTime entryExitDate)
        {
            var data = _kscHrContext.EmployeeEntryExits.AsQueryable().Where(x => x.EmployeeId == employeeId && x.IsDeleted == false && x.EntryExitDate.Date == entryExitDate.Date).AsQueryable();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdRangeDate(int employeeId, DateTime startDate, DateTime endDate)
        {
            var data = _kscHrContext.EmployeeEntryExits.AsQueryable()
                .Where(x => x.EmployeeId == employeeId && x.IsDeleted == false 
                && x.EntryExitDate.Date >= startDate.Date && x.EntryExitDate.Date <= endDate.Date).AsQueryable();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdRangeDateForReport(int employeeId, DateTime startDate, DateTime endDate)
        {
            var data = _kscHrContext.EmployeeEntryExits.AsQueryable().Where(x => x.EmployeeId == employeeId 
            && x.EntryExitDate.Date >= startDate.Date && x.EntryExitDate.Date <= endDate.Date).AsQueryable();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByDate(DateTime date)
        {
            var data = _kscHrContext.EmployeeEntryExits.AsQueryable().Where(x => x.IsDeleted == false 
            && x.EntryExitDate.Date >= date.Date && x.EntryExitDate.Date <= date.Date).AsQueryable();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitActiveByDate(DateTime date)
        {
            var data = _kscHrContext.EmployeeEntryExits.AsQueryable().Where(x => x.IsDeleted == false
            && x.EntryExitDate.Date == date.Date).AsQueryable().AsNoTracking();
            return data;
        }

        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdsRangeDateForReport(List<int> employeeId, DateTime startDate, DateTime endDate)
        {
            var data = _kscHrContext.EmployeeEntryExits.Include(a => a.Employee)
                .Where(x => employeeId.Contains(x.EmployeeId)
                && x.EntryExitDate.Date >= startDate.Date && x.EntryExitDate.Date <= endDate.Date).AsQueryable().AsNoTracking();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdsRangeDateForReportWithEmployeeeId(int employeeId, DateTime startDate, DateTime endDate)
        {

            var data = _kscHrContext.EmployeeEntryExits.Include(a => a.Employee).AsQueryable()
   .Where(x => x.EmployeeId == employeeId && x.EntryExitDate>= startDate 
    && x.EntryExitDate <= endDate).AsQueryable().AsNoTracking();

            //var data = _kscHrContext.EmployeeEntryExits.Include(a => a.Employee)
            //    .AsNoTracking().Where(x => x.EmployeeId== employeeId
            //    && x.EntryExitDate.Date >= startDate.Date && x.EntryExitDate.Date <= endDate.Date);
            return data;
        }
        public async void DisposeOperation()
        {
            await _kscHrContext.DisposeAsync();
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdsRangeDate(List<int> employeeId, DateTime entryExitDate)
        {
            var data = _kscHrContext.EmployeeEntryExits.Include(a => a.Employee)
                .Where(x => employeeId.Contains(x.EmployeeId)
                && x.EntryExitDate.Date ==  entryExitDate).AsQueryable().AsNoTracking();
            return data;
        }
        public IQueryable<EmployeeEntryExit> GetEmployeeEntryExitByDate( DateTime entryExitDate)
        {
            var data = _kscHrContext.EmployeeEntryExits.Include(a => a.Employee)
                .Where(x=> x.EntryExitDate.Date == entryExitDate).AsQueryable().AsNoTracking();
            return data;
        }

    }
}
