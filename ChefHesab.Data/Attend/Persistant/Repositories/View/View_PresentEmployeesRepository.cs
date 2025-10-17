using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class View_PresentEmployeesRepository : EfRepository<View_PresentEmployees>, IView_PresentEmployeesRepository
    {
        private readonly KscHrContext _kscHrContext;
        public View_PresentEmployeesRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }
   
}

