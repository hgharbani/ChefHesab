using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IRollCallDefinitionRepository : IRepository<RollCallDefinition, int>
    {
        IQueryable<RollCallDefinition> GetIncluded();
        IQueryable<RollCallDefinition> GetIncludedRollCallEmploymentTypes();

        IQueryable<RollCallDefinition> GetAllIncluded();
        Task<int?> GetRollCallDefinitionIdByCategoryAndWorkTimeDayType(int rollCallCategoryId, int workTimeId, int workDayTypeId, DateTime date);
        IQueryable<RollCallDefinition> GetRollCallDefinitionIdByDate(DateTime date);
        IQueryable<RollCallDefinition> GetRollCallDefinitionByWorkTimeDayTypeEmploymentTypeId(int workTimeId, int workDayTypeId, int? employmentTypeId, DateTime date);
        IQueryable<RollCallDefinition> GetRollCallDefinitionByIncludedForAttendAbsence();
        IQueryable<RollCallDefinition> GetRollCallDefinitionAsNoTracking();
        IQueryable<RollCallDefinition> GetRollCallDefinitionByIncludedForAttendAbsenceAsnoTracking();
        IQueryable<RollCallDefinition> GetRollCallDefinitionByRollCallConceptIdAsNoTracking(int rollCallConceptId);
        List<RollCallDefinition> GetRollCallDefinitionIndustrialCode();
        List<RollCallDefinition> GetRollCallDefinitionLongTermCode();
        RollCallDefinition GetRollCallDefinition(int id);
        IQueryable<RollCallDefinition> GetRollCallDefinicationInCeiling(int includedDefinitionId);
    }
}
