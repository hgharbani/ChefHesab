using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class CofficientProximityProductRepository : EfRepository<CofficientProximityProduct, int>, ICofficientProximityProductRepository
    {
        private readonly KscHrContext _kscHrContext;
        public CofficientProximityProductRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<CofficientProximityProduct> GetByCofficientProximityProductId(int? cofficientProximityProductId)
        {
            return _kscHrContext.CofficientProximityProducts.Where(x => x.Id == cofficientProximityProductId).AsQueryable();
        }
    }
}

