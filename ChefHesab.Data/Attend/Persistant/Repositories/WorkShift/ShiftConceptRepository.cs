using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public partial class ShiftConceptRepository : EfRepository<ShiftConcept,int>, IShiftConceptRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ShiftConceptRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        //public bool Exists(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> ExistsAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public ShiftConcept GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ShiftConcept> GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(ShiftConcept entity)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<ShiftConcept>> GetShiftConceptWithWOrkGroupIdDates(int workGroupId, List<int> workCalendarIds)
        {
            List<ShiftConcept> shiftConcepts = new List<ShiftConcept>();
            var query = _kscHrContext.WorkGroups.AsQueryable()
                .Include(x => x.WorkTime)
                 .ThenInclude(x => x.WorkTimeShiftConcepts)
                .ThenInclude(x => x.ShiftConcept)
                    .ThenInclude(x => x.ShiftConceptDetails);
            var workGroup = await query.FirstOrDefaultAsync(x => x.Id == workGroupId);

            if (workGroup.WorkTime.ShiftSettingFromShiftboard)
            {
                var shiftBoardquery = _kscHrContext.ShiftBoards.Include(a => a.ShiftConceptDetail).AsQueryable();
                shiftConcepts = shiftBoardquery.Where(x => workCalendarIds.Contains(x.WorkCalendarId) && x.WorkGroupId == workGroupId).Select(a=>a.ShiftConceptDetail.ShiftConcept).ToList();
           

            }
            else
            {
                var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.Where(x => x.IsActive == true &&x.ShiftConcept.ShiftConceptDetails.Any(a=>a.IsActive));
                if (workTimeShiftConcept != null)
                    shiftConcepts = workTimeShiftConcept.Select(a => a.ShiftConcept).ToList();
            }

            return shiftConcepts;
        }
    }
}
