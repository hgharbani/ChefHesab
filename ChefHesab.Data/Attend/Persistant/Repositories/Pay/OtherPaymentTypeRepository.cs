using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class OtherPaymentTypeRepository : EfRepository<OtherPaymentType, int>, IOtherPaymentTypeRepository
    {

        private readonly KscHrContext _kscHrContext;
        public OtherPaymentTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<OtherPaymentType> GetOtherPaymentTypeById(int id)
        {
            return _kscHrContext.OtherPaymentType.Where(a => a.Id == id);
        }
        public IQueryable<OtherPaymentType> GetOtherPaymentTypes()
        {
            var result = _kscHrContext.OtherPaymentType.AsQueryable();
            return result;
        }
    }
}
