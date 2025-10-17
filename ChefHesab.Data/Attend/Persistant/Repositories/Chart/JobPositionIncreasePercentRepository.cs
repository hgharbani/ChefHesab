using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobPositionIncreasePercentRepository : EfRepository<Chart_JobPositionIncreasePercent, int>, IJobPositionIncreasePercentRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobPositionIncreasePercentRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncreasePercentById(int id)
        {
            var result = _kscHrContext.Chart_JobPositionIncreasePercent.Where(a => a.Id == id);
            return result;
        }
        public IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncreasePercentByCode(string code)
        {
            var result = _kscHrContext.Chart_JobPositionIncreasePercent.AsQueryable().Where(a => a.Chart_JobPosition.MisJobPositionCode == code).Include(a=>a.Chart_JobPosition);
            return result;
        }



        public IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncreasePercents()
        {
            var result = _kscHrContext.Chart_JobPositionIncreasePercent.AsQueryable();
            return result;
        }

        public IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncrese(int jobPositionId)
        {
            var result = _kscHrContext.Chart_JobPositionIncreasePercent
                .Where(x=>x.JobPositioinId==jobPositionId).Include(a=>a.Chart_JobPosition)
                .AsQueryable();
            return result;
        }



        public bool GetRepeateJobpositionScore(int jobpositionId, double CoefficientYearsDay, double CoefficientYearsShiftc, DateTime startDate, DateTime? endDate)
        {
         
            var result = _kscHrContext
                .Chart_JobPositionIncreasePercent
                .Any(x => x.JobPositioinId == jobpositionId &&
                x.CoefficientYearsDay == CoefficientYearsDay &&
                x.CoefficientYearsShift == CoefficientYearsShiftc &&
                (x.StartDate == startDate ||
                x.EndDate == endDate)
                );

            return result;

        }

        public Chart_JobPositionIncreasePercent FindRepeateJobpositionScore(int jobpositionId, double CoefficientYearsDay, double CoefficientYearsShiftc, DateTime startDate, DateTime? endDate)
        {
          
            var result = _kscHrContext
                .Chart_JobPositionIncreasePercent
                .FirstOrDefault(x => x.JobPositioinId == jobpositionId &&
                x.CoefficientYearsDay == CoefficientYearsDay &&
                x.CoefficientYearsShift == CoefficientYearsShiftc &&
                (x.StartDate == startDate ||
                x.EndDate == endDate)
                );

            return result;

        }







    }
}
