using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IWorkCompanySettingRepository : IRepository<WorkCompanySetting, int>
    {
        IQueryable<WorkCompanySetting> GetAllWorkCompanySettingByWorkCityIdWorkTimeShiftConceptId(List<int> workCityId, List<int> workTimeShiftConceptIds);
        WorkCompanySetting GetWorkCompanySetting(int id);
        Task<WorkCompanySetting> GetWorkCompanySettingByWorkCityIdWorkTimeShiftConceptId(int workCityId, int workTimeShiftConceptId);
        IEnumerable<WorkCompanySetting> GetWorkCompanySettingIncluded();
    }
}
