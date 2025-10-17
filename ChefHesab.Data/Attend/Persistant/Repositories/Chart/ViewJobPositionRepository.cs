using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class ViewJobPositionRepository : EfRepository<ViewJobPosition>, IViewJobPositionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewJobPositionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ViewJobPosition> GetChart_JobPositionsIncluded()
        {
            var result = _kscHrContext.viewJobPositions.Include(a => a.Parent)
                .Include(a => a.Childrens).ThenInclude(a => a.Childrens)
                .ThenInclude(a => a.Childrens).ThenInclude(a => a.Childrens).ThenInclude(a => a.Childrens)
                .ThenInclude(a => a.Childrens)
                .Include(a => a.Childrens).ThenInclude(a => a.Childrens).ThenInclude(a => a.Childrens)
                .AsQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<ViewJobPosition> GetChart_JobPositionsIncludedHaveCount()
        {
            var result = _kscHrContext.viewJobPositions.Include(a => a.Parent)
                .Include(a => a.Childrens).ThenInclude(a => a.Childrens)
                .ThenInclude(a => a.Childrens).ThenInclude(a => a.Childrens).ThenInclude(a => a.Childrens)
                .ThenInclude(a => a.Childrens)
                .Include(a => a.Childrens).ThenInclude(a => a.Childrens).ThenInclude(a => a.Childrens)
                .AsQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<ViewJobPosition> GetChartJobPositionsAsnotracking()
        {
            var result = _kscHrContext.viewJobPositions
                .AsQueryable().AsNoTracking();
            return result;
        }




    }
}
