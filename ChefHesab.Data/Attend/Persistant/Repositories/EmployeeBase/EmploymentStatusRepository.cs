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
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public partial class EmploymentStatusRepository : EfRepository<EmploymentStatus, int>, IEmploymentStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmploymentStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<bool> ExistsByTitle(int id, string title)
        {
            return await _kscHrContext.EmploymentStatuses.AnyAsync(x => x.Id != id && x.Title == title);
        }
        public async Task<bool> ExistsByTitle(string title)
        {
            return await _kscHrContext.EmploymentStatuses.AnyAsync(x => x.Title == title);
        }
        public async Task<EmploymentStatus> GetOne(int id)
        {
            return await _kscHrContext.EmploymentStatuses.FindAsync(id);
        }
    }
}

