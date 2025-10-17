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

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class RollCallDefinitionRepository : EfRepository<RollCallDefinition, int>, IRollCallDefinitionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RollCallDefinitionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<RollCallDefinition> GetIncluded()
        {
            var result = _kscHrContext.RollCallDefinitions.Include(a => a.EmployeeLongTermAbsences)
                .Include(a => a.RollCallCategory).AsQueryable();
            return result;
        }
        public IQueryable<RollCallDefinition> GetIncludedRollCallEmploymentTypes()
        {
            var result = _kscHrContext.RollCallDefinitions.Include(a => a.RollCallEmploymentTypes).AsQueryable();
            return result;
        }
        public IQueryable<RollCallDefinition> GetAllIncluded()
        {
            var result = _kscHrContext.RollCallDefinitions.Include(a => a.RollCallConcept).Include(a => a.RollCallWorkTimeDayTypes).ThenInclude(a => a.WorkTime).Include(a => a.RollCallWorkTimeDayTypes).ThenInclude(a => a.WorkDayType).Include(a => a.RollCallCategory).Include(a => a.RollCallEmploymentTypes).Include(a => a.RollCallJobCategories)
                .Include(a => a.CompatibleRollCalls_RollCallDefinitionId).Include(a => a.IncludedRollCalls).Include(a => a.RollCallSalaryCodes).Include(x=>x.RollCallWorkCities).AsQueryable();
            return result;
        }
        public async Task<int?> GetRollCallDefinitionIdByCategoryAndWorkTimeDayType(int rollCallCategoryId, int workTimeId, int workDayTypeId, DateTime date)
        {
            var rollCallDefinition = await _kscHrContext.RollCallDefinitions.Include(x => x.RollCallWorkTimeDayTypes).FirstOrDefaultAsync(x => x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date &&
              x.RollCallCategoryId == rollCallCategoryId && (x.IsValidForAllWorkTimeDayType == true || x.RollCallWorkTimeDayTypes.Any(y => y.WorkTimeId == workTimeId && y.WorkDayTypeId == workDayTypeId)));
            if (rollCallDefinition != null)
                return rollCallDefinition.Id;
            else
                return null;
        }
        public IQueryable<RollCallDefinition> GetRollCallDefinitionIdByDate(DateTime date)
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.Include(x => x.RollCallWorkTimeDayTypes).Include(x => x.CompatibleRollCalls_CompatibleRollCallId).ThenInclude(x => x.RollCallDefinition_CompatibleRollCallId)
                .Include(x => x.RollCallEmploymentTypes)
                .Where(x => x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
            return rollCallDefinition;
        }
        public IQueryable<RollCallDefinition> GetRollCallDefinitionByWorkTimeDayTypeEmploymentTypeId(int workTimeId, int workDayTypeId, int? employmentTypeId, DateTime date)
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.Include(x => x.RollCallWorkTimeDayTypes).Include(x => x.RollCallEmploymentTypes).Include(x => x.RollCallJobCategories)
                .Where(x => x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date &&
                                                       (x.IsValidForAllEmploymentType || x.RollCallEmploymentTypes.Any(y => y.EmploymentTypeCode == employmentTypeId)) &&
                            (x.IsValidForAllWorkTimeDayType == true || x.RollCallWorkTimeDayTypes.Any(y => y.WorkTimeId == workTimeId && y.WorkDayTypeId == workDayTypeId)));
            return rollCallDefinition;
        }
        public IQueryable<RollCallDefinition> GetRollCallDefinitionByIncludedForAttendAbsence()
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.Include(x => x.RollCallWorkTimeDayTypes)
                                                                      .Include(x => x.CompatibleRollCalls_RollCallDefinitionId)
                                                                      .Include(x => x.RollCallEmploymentTypes)
                                                                      .Include(x => x.RollCallJobCategories)
                                                                      .Include(x => x.AccessLevel)
                                                                      .Include(x => x.RollCallWorkCities)
                                                                      ;

            return rollCallDefinition;
        }
        public IQueryable<RollCallDefinition> GetRollCallDefinitionAsNoTracking()
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.AsNoTracking();
            return rollCallDefinition;
        }
        public IQueryable<RollCallDefinition> GetRollCallDefinitionByIncludedForAttendAbsenceAsnoTracking()
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.Include(x => x.RollCallWorkTimeDayTypes)
                                                                      .Include(x => x.RollCallEmploymentTypes)
                                                                      ;

            return rollCallDefinition;
        }
        IQueryable<RollCallDefinition> IRollCallDefinitionRepository.GetRollCallDefinitionByRollCallConceptIdAsNoTracking(int rollCallConceptId)
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.Where(x => x.RollCallConceptId == rollCallConceptId).AsNoTracking();
            return rollCallDefinition;
        }

        public List<RollCallDefinition> GetRollCallDefinitionIndustrialCode()
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.AsNoTracking().Where(a => a.RollCallCategoryId == 2).ToList();
            return rollCallDefinition;
        }

        public List<RollCallDefinition> GetRollCallDefinitionLongTermCode()
        {
            var rollCallDefinition = _kscHrContext.RollCallDefinitions.AsNoTracking().Where(a => a.RollCallCategoryId == 3).ToList();
            return rollCallDefinition;
        }

        public RollCallDefinition GetRollCallDefinition(int id)
        {
            return _kscHrContext.RollCallDefinitions.Where(x => x.Id == id).Include(x => x.RollCallWorkTimeDayTypes).FirstOrDefault();
        }
        public IQueryable<RollCallDefinition> GetRollCallDefinicationInCeiling(int includedDefinitionId)
        {
            var query = _kscHrContext.RollCallDefinitions.Include(x => x.IncludedRollCalls).Where(x => x.IsActive == true && x.IncludedRollCalls
            .Any(i => i.IsActive && i.IncludedDefinitionId == includedDefinitionId)).AsNoTracking();
            return query;
        }

    }

}
