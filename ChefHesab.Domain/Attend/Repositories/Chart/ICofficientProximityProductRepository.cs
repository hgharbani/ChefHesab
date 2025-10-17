using Ksc.Hr.Domain.Entities;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
{
    public interface ICofficientProximityProductRepository : IRepository<CofficientProximityProduct, int>
    {
        IQueryable<CofficientProximityProduct> GetByCofficientProximityProductId(int? cofficientProximityProductId);
    }
}

