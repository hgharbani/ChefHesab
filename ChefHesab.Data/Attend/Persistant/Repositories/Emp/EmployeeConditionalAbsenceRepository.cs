using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Share.Model.EmployeeConditionalAbsence;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeConditionalAbsenceRepository : EfRepository<EmployeeConditionalAbsence, int>, IEmployeeConditionalAbsenceRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeConditionalAbsenceRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeConditionalAbsence> GetAllRelated() => _kscHrContext.EmployeeConditionalAbsences.Include(x => x.Family).Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceSubject).Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceType).Where(x => x.IsDeleted == false);
        public IQueryable<EmployeeConditionalAbsence> GetAllByDeletedRelated() => _kscHrContext.EmployeeConditionalAbsences.Include(x => x.Family).Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceSubject).Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceType);
        public IQueryable<EmployeeConditionalAbsence> GetAllByEmployeeId(int employeeId) => _kscHrContext.EmployeeConditionalAbsences.Include(a => a.ConditionalAbsenceSubjectType).Where(a => a.IsDeleted == false && a.EmployeeId == employeeId);

        public IQueryable<EmployeeConditionalAbsence> GetEmployeeConditionalAbsenceInDuration(int employeeId, DateTime startDate, DateTime endDate) => GetAllByEmployeeId(employeeId).Where(a =>
                                                                                                                                                                     ((a.StartDate <= startDate && a.EndDate >= startDate) ||
                                                                                                                                                                     (a.StartDate <= endDate && a.EndDate >= endDate) ||
                                                                                                                                                                     (a.StartDate >= startDate && a.EndDate <= endDate))
            );
        public EmployeeConditionalAbsenceForTimeSheetAnalysModel GetEmployeeConditionalAbsenceForTimeSheetAnalys(int employeeId, DateTime date)
        {
            var model = _kscHrContext.EmployeeConditionalAbsences.Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceType)
                   .Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceSubject)
                   .Where(x => x.EmployeeId == employeeId && x.IsDeleted == false
                               && x.StartDate.Date <= date && x.EndDate.Date >= date)
                   .Select(x => new EmployeeConditionalAbsenceForTimeSheetAnalysModel
                   {
                       RollCallDefinitionId = x.ConditionalAbsenceSubjectType.RollCallDefinitionId,
                       IsHourly = x.ConditionalAbsenceSubjectType.ConditionalAbsenceType.IsHourlyAbsence,
                       IsInStartShift = x.ConditionalAbsenceSubjectType.ConditionalAbsenceType.IsInStartShift,
                       HourlyAbsenceToleranceTime = x.ConditionalAbsenceSubjectType.ConditionalAbsenceSubject.HourlyAbsenceToleranceTime,
                       ConditionalAbsenceSubjectId = x.ConditionalAbsenceSubjectType.ConditionalAbsenceSubjectId,
                       //   HasForcedOvertime = x.ConditionalAbsenceSubjectType.ConditionalAbsenceSubject.HasForcedOvertime
                       HasForcedOvertime = x.ConditionalAbsenceSubjectType.HasForcedOvertime,
                   }).FirstOrDefault();
            if (model != null)
            {
                var rollCallDefinition = _kscHrContext.RollCallDefinitions.FirstOrDefault(x => x.Id == model.RollCallDefinitionId &&
                x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
                if (rollCallDefinition == null)
                    model = null;

            }
            return model;
        }
        public EmployeeConditionalAbsence GetEmployeeConditionalAbsenceForCreateItemAttendAbsence(int employeeId, DateTime date, int conditionalAbsenceSubjectTypeId)
        {
            var model = _kscHrContext.EmployeeConditionalAbsences.FirstOrDefault(x => x.EmployeeId == employeeId && x.IsDeleted == false
            && x.ConditionalAbsenceSubjectTypeId == conditionalAbsenceSubjectTypeId
                               && x.StartDate.Date <= date && x.EndDate.Date >= date);

            return model;
        }

        public IQueryable<EmployeeConditionalAbsence> GetEmployeeConditionalAbcenseNotHaveForcedOvertime(int yearMonth)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth).Select(x => x.MiladiDateV1).ToList();
            var startDate = workCalendar.Min();
            var endDate = workCalendar.Max();
            var query = _kscHrContext.EmployeeConditionalAbsences.Where(x =>
               !x.IsDeleted
            // && !x.ConditionalAbsenceSubjectType.ConditionalAbsenceSubject.HasForcedOvertime
            && !x.ConditionalAbsenceSubjectType.HasForcedOvertime
            && x.EndDate.Date >= startDate).Include(x => x.ConditionalAbsenceSubjectType).ThenInclude(x => x.ConditionalAbsenceSubject);
            return query;
        }
    }
}
