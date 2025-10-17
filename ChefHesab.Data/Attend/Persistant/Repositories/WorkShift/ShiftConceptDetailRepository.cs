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
using Ksc.HR.Share.General;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class ShiftConceptDetailRepository : EfRepository<ShiftConceptDetail, int>, IShiftConceptDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ShiftConceptDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<ShiftConceptDetail> GetShiftConceptDetailWithIncluded(int id)
        {
            var query = _kscHrContext.ShiftConceptDetails.Include(x => x.ShiftConcept).ThenInclude(x => x.WorkTimeShiftConcepts).AsNoTracking();
            var shiftConceptDetail = await query.FirstAsync(x => x.Id == id);
            return shiftConceptDetail;

        }
        public async Task<ShiftConceptDetail> GetShiftConceptDetailOutIncludedAsNoTracking(int id)
        {
            var query = _kscHrContext.ShiftConceptDetails.AsNoTracking();
            var shiftConceptDetail = await query.FirstAsync(x => x.Id == id);
            return shiftConceptDetail;

        }

        public IQueryable<ShiftConceptDetail> GetAllShiftConceptDetailWithIncluded(List<int> ids)
        {
            var query = _kscHrContext.ShiftConceptDetails.Include(x => x.ShiftConcept).ThenInclude(x => x.WorkTimeShiftConcepts).Where(x => ids.Contains(x.Id)).AsQueryable();

            return query;

        }



        public async Task<ShiftConceptDetail> GetShiftConceptDetailWithWOrkGroupIdDate(int workGroupId, int workCalendarId)
        {
            ShiftConceptDetail shiftConceptDetail = new ShiftConceptDetail();
            var query = _kscHrContext.WorkGroups.AsQueryable().Include(x => x.WorkTime).ThenInclude(x => x.WorkTimeShiftConcepts).ThenInclude(x => x.ShiftConcept).ThenInclude(x => x.ShiftConceptDetails);
            var workGroup = await query.FirstOrDefaultAsync(x => x.Id == workGroupId);
            if (workGroup.WorkTime.ShiftSettingFromShiftboard)
            {
                var shiftBoardquery = _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).AsQueryable();
                var ShiftBoard = shiftBoardquery.FirstOrDefault(x => x.WorkCalendarId == workCalendarId && x.WorkGroupId == workGroupId);
                if (ShiftBoard != null)
                    shiftConceptDetail = ShiftBoard.ShiftConceptDetail;

            }
            else
            {
                var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                if (workTimeShiftConcept != null)
                    shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.FirstOrDefault(x => x.IsActive == true);
            }

            return shiftConceptDetail;
        }

        public async Task<List<ShiftConceptDetail>> GetShiftConceptDetailWithWOrkGroupIdDates(int workGroupId, List<int> workCalendarIds)
        {
            List<ShiftConceptDetail> shiftConceptDetail = new List<ShiftConceptDetail>();
            var query = _kscHrContext.WorkGroups.AsQueryable().Include(x => x.WorkTime).ThenInclude(x => x.WorkTimeShiftConcepts)
                .ThenInclude(x => x.ShiftConcept).ThenInclude(x => x.ShiftConceptDetails);
            var workGroup = await query.FirstOrDefaultAsync(x => x.Id == workGroupId).ConfigureAwait(false);

            if (workGroup.WorkTime.ShiftSettingFromShiftboard)
            {
                shiftConceptDetail = await _kscHrContext.ShiftBoards.AsQueryable().Include(a => a.ShiftConceptDetail)/*.AsNoTracking()*/
                    .Where(x => workCalendarIds.Contains(x.WorkCalendarId) && x.WorkGroupId == workGroupId)
                    .Select(a => a.ShiftConceptDetail).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true); 
                if (workTimeShiftConcept != null)
                    shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.Where(x => x.IsActive == true).ToList();
            }

            return shiftConceptDetail;
        }
        public async Task<List<ShiftConceptDetail>> GetShiftConceptDetailWithWOrkGroupIdDates(List<int> workGroupId, List<int> workCalendarIds)
        {
            List<ShiftConceptDetail> shiftConceptDetails = new List<ShiftConceptDetail>();
            var query = _kscHrContext.WorkGroups.AsQueryable().Include(x => x.WorkTime).ThenInclude(x => x.WorkTimeShiftConcepts)
                .ThenInclude(x => x.ShiftConcept).ThenInclude(x => x.ShiftConceptDetails);
            var workGroups = await query.Where(x => workGroupId.Contains(x.Id)).ToListAsync().ConfigureAwait(false);

            var shiftCnceptIDs =await _kscHrContext.ShiftBoards.AsQueryable().Include(a => a.ShiftConceptDetail).AsNoTracking()
                    .Where(x => workCalendarIds.Contains(x.WorkCalendarId) && workGroupId.Contains(x.WorkGroupId)).ToListAsync();


            foreach (var workGroup in workGroups)
            {
                if (workGroup.WorkTime.ShiftSettingFromShiftboard)
                {
                    var shiftConceptDetail = shiftCnceptIDs.Where(x => x.WorkGroupId == workGroup.Id)
                          .Select(a => a.ShiftConceptDetail).ToList();
                    shiftConceptDetails.AddRange(shiftConceptDetail);
                }
                else
                {
                    var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                    if (workTimeShiftConcept != null)
                    {
                        var shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.Where(x => x.IsActive == true).ToList();

                        shiftConceptDetails.AddRange(shiftConceptDetail);
                    }
                
                }
            }
            

            return shiftConceptDetails;
        }

        public async Task<ShiftConceptDetail> GetShiftConceptDetailForAttendAbcenseAnalysisAsNoTracking(int id)
        {
            var query = _kscHrContext.ShiftConceptDetails.Where(x=>x.Id==id).Include(x => x.ShiftConcept).ThenInclude(x => x.WorkTimeShiftConcepts).ThenInclude(x=>x.WorkTime).AsNoTracking();
            var shiftConceptDetail = await query.FirstAsync();
            return shiftConceptDetail;

        }

        public int GetShiftConceptIdByShiftConceptDetailId(int id)
        {
            return _kscHrContext.ShiftConceptDetails.Where(x => x.Id == id).Select(x => x.ShiftConceptId).First();
        }
    }
       
}
