using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class PropertyAccountTypeRepository : EfRepository<PropertyAccountType, int>, IPropertyAccountTypeRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PropertyAccountTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

    }
}
