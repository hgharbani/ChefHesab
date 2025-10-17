using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using KSC.Common.Filters.Models;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class InsuranceBookletRepository : EfRepository<InsuranceBooklet, long>, IInsuranceBookletRepository
    {


        private readonly KscHrContext _kscHrContext;
        public InsuranceBookletRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<InsuranceBooklet> GetAllWithFranshiseType(int employeeId)
        {
            var query = _kscHrContext.InsuranceBooklet.Include(x => x.FranchiseType).Where(x => x.IsActive == true && x.EmployeeId == employeeId).AsNoTracking()
                .AsQueryable();
            return query;
        }
    }
}

