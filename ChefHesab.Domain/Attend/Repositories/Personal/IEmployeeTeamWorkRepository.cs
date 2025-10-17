using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeTeamWorkRepository : IRepository<EmployeeTeamWork, int>
    {
        IQueryable<int> GetActivePersonsId();
        IQueryable<int> GetActivePersonsIdByEmployeeIds(List<int> ids);
        IQueryable<int> GetActivePersonsIdForOfficialHoliday(int workCalendarId, DateTime date);
        EmployeeTeamWork GetActiveTeamWorkByEmployeeId(int EmployeeId);
        Task<EmployeeTeamWork> GetActiveTeamWorkByEmployeeIdAsync(int EmployeeId);
        IQueryable<EmployeeTeamWork> GetAllTeamWorkInclude();
        IQueryable<EmployeeTeamWork> GetAllQueryable();
        IQueryable<EmployeeTeamWork> GetEmployeeTeamWorkByTeamWorkIdAsNoTracking(int teamWorkId, DateTime date);
        IQueryable<int> GetActivePersonsIdWithDate(DateTime date);
        IQueryable<int> GetActivePersonsIdByEmployeeIdsWithDate(List<int> ids, DateTime date);
        IQueryable<Employee> GetActivePersonsForValidOverTime(DateTime date);
        IQueryable<Employee> GetActivePersonsForValidOverTimeByEmployeeIds(List<int> ids, DateTime date);
        IQueryable<EmployeeTeamWork> GetEmployeeTeamWorkByTeamWorkId(int teamWorkId, DateTime entryExitDate);
    }
}

