using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class CategoryCoefficientRepository : EfRepository<CategoryCoefficient, int>, ICategoryCoefficientRepository
    {
        private readonly KscHrContext _kscHrContext;
        public CategoryCoefficientRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

