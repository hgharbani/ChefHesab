using Ksc.HR.Domain.Entities.Rule;
using KSC.Domain;
using System.Linq;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IEmployeePromotionInterdictsRepository : IRepository<EmployeePromotionInterdicts, int>
    {
        IQueryable<EmployeePromotion> GeInterdictsPromotion(int promotionId);
    }
}
