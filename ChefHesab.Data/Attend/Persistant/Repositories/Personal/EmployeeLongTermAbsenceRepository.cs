using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.Hr.Domain.Shared;
//using Ksc.HR.Resources.Messages;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeLongTermAbsenceRepository : EfRepository<EmployeeLongTermAbsence, int>, IEmployeeLongTermAbsenceRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeLongTermAbsenceRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeLongTermAbsence> GetEmployeeLongTermAbsences()
        {
            return _kscHrContext.EmployeeLongTermAbsences.Include(a => a.RollCallDefinition).Include(a => a.Employee).AsQueryable();
        }


        public async Task<Tuple<string, string, string>> GetShiftStartEndTimeWithWorkCalendarMiladiDate(int shiftConceptDetailId, int workCityId, int workGroupId ,DateTime date)
        {
            var workGroup = await _kscHrContext.WorkGroups.FindAsync(workGroupId);
            var workTime = await _kscHrContext.WorkTimes.FindAsync(workGroup.WorkTimeId);
            var shiftConceptDetail = await _kscHrContext.ShiftConceptDetails.FindAsync(shiftConceptDetailId);
            var workTimeShiftConcept = shiftConceptDetail.ShiftConcept.WorkTimeShiftConcepts.First(x => x.WorkTimeId == workGroup.WorkTimeId);
            var workCompanySetting = await _kscHrContext.WorkCompanySettings.Include(x => x.TimeShiftSettings).AsNoTracking().FirstAsync(x => x.WorkCityId == workCityId && x.WorkTimeShiftConceptId == workTimeShiftConcept.Id);
            var timeShiftSetting = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
            if (timeShiftSetting == null)
            {
                // throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تنظیمات شیفت"));
                throw new Exception("تنظیمات شیفت یافت نشد");
            }
            var result = new Tuple<string, string, string>(timeShiftSetting.ShiftStartTime, timeShiftSetting.ShiftEndtTime, timeShiftSetting.TotalWorkHourInDay);
            return result;
        }





        public async Task<Tuple<string, string, string>> GetShiftStartEndTime(int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId)
        {

            var workCalendar =  _kscHrContext.WorkCalendars.Find(workCalendarId);
            DateTime date = workCalendar.MiladiDateV1;
            var workGroup = await _kscHrContext.WorkGroups.FindAsync(workGroupId);
            var workTime = await _kscHrContext.WorkTimes.FindAsync(workGroup.WorkTimeId);
            var shiftConceptDetail = await _kscHrContext.ShiftConceptDetails.FindAsync(shiftConceptDetailId);
            var workTimeShiftConcept = shiftConceptDetail.ShiftConcept.WorkTimeShiftConcepts.First(x => x.WorkTimeId == workGroup.WorkTimeId);
            var workCompanySetting = await _kscHrContext.WorkCompanySettings.Include(x => x.TimeShiftSettings).AsNoTracking().FirstAsync(x => x.WorkCityId == workCityId && x.WorkTimeShiftConceptId == workTimeShiftConcept.Id);
            var timeShiftSetting = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
            if (timeShiftSetting == null)
            {
               // throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تنظیمات شیفت"));
                throw new Exception("تنظیمات شیفت یافت نشد");
            }
            var result = new Tuple<string, string, string>(timeShiftSetting.ShiftStartTime, timeShiftSetting.ShiftEndtTime, timeShiftSetting.TotalWorkHourInDay);
            return result;
        }

        public async Task<List<TimeShiftSetting>> GetListShiftStartEndTime(List<int> shiftConceptDetailId, int workCityId, List<int> workGroupId, List<int> workCalendarId)
        {

            var workCalendars = await _kscHrContext.WorkCalendars.AsNoTracking().Where(a => workCalendarId.Contains(a.Id)).ToListAsync();

            var workGroups = await _kscHrContext.WorkGroups.AsNoTracking()
                .Where(a => workGroupId.Contains(a.Id)).ToListAsync();

            var workTimeIds = workGroups.Select(a => a.WorkTimeId).ToList();

            var workTimes = await _kscHrContext.WorkTimes.AsNoTracking()
                .Where(a => workTimeIds.Contains(a.Id)).ToListAsync();

            var shiftConceptDetail = await _kscHrContext.ShiftConceptDetails
                .Include(a => a.ShiftConcept)
                .ThenInclude(a => a.WorkTimeShiftConcepts).AsNoTracking()
                .Where(a => shiftConceptDetailId.Contains(a.Id)).ToListAsync();

            var workTimeShiftConcepts = shiftConceptDetail.SelectMany(a => a.ShiftConcept.WorkTimeShiftConcepts.Where(x => workTimes.Any(c => x.WorkTimeId == c.Id)).ToList()).ToList();
            var worktimeShiftconceprIds = workTimeShiftConcepts.Select(a => a.Id).ToList();
            var workCompanySettings = await _kscHrContext.WorkCompanySettings
                .Include(a => a.WorkTimeShiftConcept)
                         .ThenInclude(c => c.ShiftConcept)
                         .ThenInclude(c => c.ShiftConceptDetails)
                .Include(x => x.TimeShiftSettings)
                .AsNoTracking()
                .Where(x => x.WorkCityId == workCityId && worktimeShiftconceprIds.Contains(x.WorkTimeShiftConceptId)).ToListAsync();

            var timeShiftSetting = workCompanySettings.SelectMany(c => c.TimeShiftSettings.Where(x => x.IsTemporaryTime == false && workCalendars.Any(c => x.ValidityStartDate.Value.Date <= c.MiladiDateV1.Date && x.ValidityEndDate.Value.Date >= c.MiladiDateV1.Date)).ToList()).ToList();
            if (!timeShiftSetting.Any())
            {
                throw new Exception("تنظیمات شیفت یافت نشد");
            }

            return timeShiftSetting;
        }

        public void BulkInser(IList<EmployeeLongTermAbsence> entity)
        {
            try
            {
                _kscHrContext.BulkInsert(entity);
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public async Task BulkInsertOrUpdateOrDeleteAsync(IList<EmployeeLongTermAbsence> entity)
        {
            try
            {
                await _kscHrContext.BulkInsertAsync(entity);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

    }
}
