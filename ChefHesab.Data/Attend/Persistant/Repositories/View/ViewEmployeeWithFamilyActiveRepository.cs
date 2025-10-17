using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Repositories.View;

namespace Ksc.HR.Data.Persistant.Repositories.View
{
    public class ViewEmployeeWithFamilyActiveRepository : EfRepository<ViewEmployeeWithFamilyActive>, IViewEmployeeWithFamilyActiveRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewEmployeeWithFamilyActiveRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }
}
