using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeLongTermAbsenceRepository : IRepository<EmployeeLongTermAbsence, int>
    {
        void BulkInser(IList<EmployeeLongTermAbsence> entity);
        Task BulkInsertOrUpdateOrDeleteAsync(IList<EmployeeLongTermAbsence> entity);
        IQueryable<EmployeeLongTermAbsence> GetEmployeeLongTermAbsences();
        Task<List<TimeShiftSetting>> GetListShiftStartEndTime(List<int> shiftConceptDetailId, int workCityId, List<int> workGroupId, List<int> workCalendarId);
        Task<Tuple<string, string, string>> GetShiftStartEndTime(int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId);
        Task<Tuple<string, string, string>> GetShiftStartEndTimeWithWorkCalendarMiladiDate(int shiftConceptDetailId, int workCityId, int workGroupId, DateTime date);
    }
}
