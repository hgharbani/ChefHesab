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
using Ksc.Hr.Domain.Entities;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public partial class SavingTypeRepository : EfRepository<SavingType, int>, ISavingTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public SavingTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<bool> ExistsByTitle(int id, string title)
        {
            return await _kscHrContext.SavingTypes.AnyAsync(x => x.Id != id && x.Title == title);
        }
        public async Task<bool> ExistsByTitle(string title)
        {
            return await _kscHrContext.SavingTypes.AnyAsync(x => x.Title == title);
        }
        public async Task<SavingType> GetOne(int id)
        {
            return await _kscHrContext.SavingTypes.FindAsync(id);
        }
    }
}

