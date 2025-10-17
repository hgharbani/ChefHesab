using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public  class InterdictCategoryRepository : EfRepository<InterdictCategory, int>, IInterdictCategoryRepository
    {
        public InterdictCategoryRepository(KscHrContext context) : base(context)
        {
        }
    }
}
