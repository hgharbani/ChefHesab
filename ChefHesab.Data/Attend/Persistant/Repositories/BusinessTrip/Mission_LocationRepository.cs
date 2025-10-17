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
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Repositories.BusinessTrip;

namespace Ksc.HR.Data.Persistant.Repositories.BusinessTrip
{
    public class Mission_LocationRepository : EfRepository<Mission_Location,int>, IMission_LocationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Mission_LocationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}
