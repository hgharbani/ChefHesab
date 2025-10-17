using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Dismissal;
using Ksc.HR.Domain.Repositories.Dismissal;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Dismissal
{
    public class Dismissal_StatusRepository : EfRepository<Dismissal_Status, int>, IDismissal_StatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Dismissal_StatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<bool> ExistsByTitle(int id, string title)
        {
            return await _kscHrContext.Dismissal_Status.AnyAsync(x => x.Id != id && x.Title == title);
        }
        public async Task<bool> ExistsByTitle(string title)
        {
            return await _kscHrContext.Dismissal_Status.AnyAsync(x => x.Title == title);
        }
        public async Task<Dismissal_Status> GetOne(int id)
        {
            return await _kscHrContext.Dismissal_Status.FindAsync(id);
        }
    }
}
