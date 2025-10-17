using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Domain.Repositories.Salary;
using Ksc.HR.Share.Model.Pay;
using KSC.Common;
using KSC.Infrastructure.Persistance;
using KSCCommunicationAPI.Models.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class AccountCodeDeductionTypeRepository : EfRepository<AccountCodeDeductionType, int>, IAccountCodeDeductionTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public AccountCodeDeductionTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
       }

        public IQueryable<AccountCodeDeductionType> GetAllInclude()
        {
            return _kscHrContext.AccountCodeDeductionTypes
                .AsQueryable().Include(x => x.AccountCode);
        }

        public AccountCodeDeductionType GetDeductonTypeByAccountCode(int id)
        {
            var query = _kscHrContext.AccountCodeDeductionTypes
                .Where(x => x.AccountCodeId == id)
                .FirstOrDefault();

            return query;
        }



    }
}
