using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;

using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using Ksc.HR.Share.Model.Vacation;
using Ksc.HR.Share.General;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class EmployeeVacationSoldRepository : EfRepository<EmployeeVacationSold, int>, IEmployeeVacationSoldRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeVacationSoldRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        /// <summary>
        /// AsNoTracking
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmployeeVacationSold> GetAllRelated()
        {
            return _kscHrContext.EmployeeVacationSolds.Include(a=>a.Employee).AsNoTracking().AsQueryable();
               

        }

        public IQueryable<EmployeeVacationSold> GetByIdRelated(int id)
        {
            return _kscHrContext.EmployeeVacationSolds.Where(x => x.Id == id).Include(a => a.Employee).AsQueryable();

        }


    }
}

