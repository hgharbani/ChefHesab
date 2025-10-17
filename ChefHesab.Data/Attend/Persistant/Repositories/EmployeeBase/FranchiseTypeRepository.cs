using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class FranchiseTypeRepository : EfRepository<FranchiseType, int>, IFranchiseTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public FranchiseTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<FranchiseType> GetOne(int id)
        {
            return await _kscHrContext.FranchiseType.FindAsync(id);
        }
    }
}
