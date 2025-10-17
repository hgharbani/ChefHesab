using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.ODSViews
{
    public class ViewMisJobPositionRepository : EfRepository<ViewMisJobPosition>, IViewMisJobPositionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisJobPositionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<ViewMisJobPosition> GetAllActiveJobPosition()
        {
            var model = _kscHrContext.ViewMisJobPosition.Where(x => x.JobPositionEndDate == 0);
            return model;
        }
        public ViewMisJobPosition GetJobPosition(string JobPositionCode)
        {
            var model = _kscHrContext.ViewMisJobPosition.FirstOrDefault(x =>x.JobPositionCode== JobPositionCode);
            return model;
        }
    }
}
