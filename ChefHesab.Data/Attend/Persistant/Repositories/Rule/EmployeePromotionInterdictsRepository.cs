using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public  class EmployeePromotionInterdictsRepository : EfRepository<EmployeePromotionInterdicts, int>, IEmployeePromotionInterdictsRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeePromotionInterdictsRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeePromotion> GeInterdictsPromotion(int promotionId)
        {
            var data = _kscHrContext.EmployeePromotions.Include(x => x.EmployeePromotionInterdicts).ThenInclude(x => x.EmployeeInterdict)
                          .Where(x => x.Id == promotionId);

            return data;
        }

    }
}
