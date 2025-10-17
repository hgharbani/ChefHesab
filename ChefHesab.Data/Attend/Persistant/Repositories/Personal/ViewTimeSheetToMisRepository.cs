using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.Hr.Domain.Repositories.Personal;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{//ViewTimeSheetToMis
    public class ViewTimeSheetToMisRepository : EfRepository<ViewTimeSheetToMis>, IViewTimeSheetToMisRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewTimeSheetToMisRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<ViewTimeSheetToMis> GetVMMonthTimeSheetByYearMonthAsNoTracking(int yearMonth)
        {
            return _kscHrContext.ViewTimeSheetToMiss.AsNoTracking().Where(x => x.YearMonth == yearMonth)
                
                ;

        }
    }
}
