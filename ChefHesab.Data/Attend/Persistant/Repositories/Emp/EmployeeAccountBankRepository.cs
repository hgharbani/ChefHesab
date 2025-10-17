using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories.EmployeeBase;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class EmployeeAccountBankRepository : EfRepository<EmployeeAccountBank, int>, IEmployeeAccountBankRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeAccountBankRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeAccountBank> GetAllRelated()
        {
            return _kscHrContext.EmployeeAccountBanks
                .Include(x => x.AccountBankType)
                .AsNoTracking()
                .AsQueryable();

        }

        public IQueryable<EmployeeAccountBank> GetByIdRelated(int id)
        {
            return _kscHrContext.EmployeeAccountBanks.Where(x => x.Id == id)
                .Include(x => x.AccountBankType)
                .AsQueryable();

        }

        public IQueryable<EmployeeAccountBank> GetActiveByAccountBankType(int accountBankTypeId)
        {
            return _kscHrContext.EmployeeAccountBanks
                .Where(x => x.AccountBankTypeId == accountBankTypeId && x.IsActive == true)
                .AsQueryable();

        }

    }
}
