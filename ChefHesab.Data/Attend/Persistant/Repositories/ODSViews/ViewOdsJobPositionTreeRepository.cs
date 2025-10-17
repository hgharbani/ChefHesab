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
    public class ViewOdsJobPositionTreeRepository : EfRepository<ViewOdsJobPositionTree>, IViewOdsJobPositionTreeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewOdsJobPositionTreeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ViewOdsJobPositionTree> GetViewOdsJobPositionTree()
        {
            return _kscHrContext.ViewOdsJobPositionTrees;
        }
    }
}
