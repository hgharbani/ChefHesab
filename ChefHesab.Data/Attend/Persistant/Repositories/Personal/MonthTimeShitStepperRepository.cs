using Ksc.HR.Data.Persistant.Context;
using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public partial class MonthTimeShitStepperRepository : EfRepository<MonthTimeShitStepper, int>, IMonthTimeShitStepperRepository
    {

        private readonly KscHrContext _kscHrContext;
        public MonthTimeShitStepperRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<MonthTimeShitStepper> GetActiveMonthTimeShitStepperAsNoTracking()
        {
            return _kscHrContext.MonthTimeShitSteppers.Where(x => x.IsActive).AsNoTracking();
        }
    }
}
