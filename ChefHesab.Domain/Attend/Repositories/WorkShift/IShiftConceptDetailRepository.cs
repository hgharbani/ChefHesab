using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IShiftConceptDetailRepository : IRepository<ShiftConceptDetail, int>
    {
        IQueryable<ShiftConceptDetail> GetAllShiftConceptDetailWithIncluded(List<int> ids);
        Task<ShiftConceptDetail> GetShiftConceptDetailForAttendAbcenseAnalysisAsNoTracking(int id);
        Task<ShiftConceptDetail> GetShiftConceptDetailOutIncludedAsNoTracking(int id);
        Task<ShiftConceptDetail> GetShiftConceptDetailWithIncluded(int id);
        Task<ShiftConceptDetail> GetShiftConceptDetailWithWOrkGroupIdDate(int workGroupId, int workCalendarId);
        Task<List<ShiftConceptDetail>> GetShiftConceptDetailWithWOrkGroupIdDates(int workGroupId, List<int> workCalendarIds);
        Task<List<ShiftConceptDetail>> GetShiftConceptDetailWithWOrkGroupIdDates(List<int> workGroupId, List<int> workCalendarIds);
        int GetShiftConceptIdByShiftConceptDetailId(int id);
    }
}
