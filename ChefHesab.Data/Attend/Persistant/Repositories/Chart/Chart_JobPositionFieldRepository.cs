using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class Chart_JobPositionFieldRepository : EfRepository<Chart_JobPositionField, int>, IChart_JobPositionFieldRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobPositionFieldRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobPositionField> GetAllData()
        {
            return _kscHrContext.Chart_JobPositionField.AsQueryable();
        }

       
    }
}
