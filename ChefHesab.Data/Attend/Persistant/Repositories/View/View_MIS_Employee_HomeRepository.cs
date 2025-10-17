using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class View_MIS_Employee_HomeRepository : EfRepository<View_MIS_Employee_Home>, IView_MIS_Employee_HomeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public View_MIS_Employee_HomeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }

    
}

