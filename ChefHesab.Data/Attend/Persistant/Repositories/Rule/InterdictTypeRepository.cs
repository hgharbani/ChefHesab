using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
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
    public class InterdictTypeRepository : EfRepository<InterdictType, int>, IInterdictTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InterdictTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public bool IsChechedCapacityJobPosition(int id)
        {
            var rersult= _kscHrContext.InterdictTypes.Where(x=>x.Id==id).Select(x=>x.IsEditableJobPosition).FirstOrDefault();
            return rersult;
        }


    }
}
