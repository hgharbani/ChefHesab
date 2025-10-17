using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class Chart_JobPositionScoreTypeRepository : EfRepository<Chart_JobPositionScoreType, int>, IChart_JobPositionScoreTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobPositionScoreTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_JobPositionScoreType> GetAllData()
        {
            return _kscHrContext.Chart_JobPositionScoreType.Where(x => x.IsActive == true).AsQueryable();
        }
        public int GetScoreTypeIdByCode(int? code)
        {
            var result= _kscHrContext.Chart_JobPositionScoreType.Where(x => x.IsActive == true && x.Code== code).Select(x=>x.Id).FirstOrDefault();
            return result;
        }
        //public IQueryable<Chart_JobPositionScoreType> GetChart_JobPositionScoreTypes()
        //{
        //    var result = _kscHrContext.Chart_JobPositionScoreType.AsQueryable();
        //    return result;
        //}
    }
}
