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
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class InsuranceListRepository : EfRepository<InsuranceList, int>, IInsuranceListRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InsuranceListRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
      
        public IQueryable<InsuranceList> GetAllIncludeInsuranceType()
        {
            var result = _kscHrContext.InsuranceLists.Include(x => x.InsuranceType).AsQueryable().AsNoTracking();
            return result;
        }
        public IQueryable<InsuranceList> GetAllIncludeInsuranceTypeById(int? id)
        {
            var result = _kscHrContext.InsuranceLists.Include(x => x.InsuranceType).Where(x=>x.InsuranceTypeId==id).AsQueryable().AsNoTracking();
            return result;
        }
    }
}
