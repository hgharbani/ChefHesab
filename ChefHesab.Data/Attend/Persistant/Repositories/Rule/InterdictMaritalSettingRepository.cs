using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
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
    public class InterdictMaritalSettingRepository : EfRepository<InterdictMaritalSetting, int>, IInterdictMaritalSettingRepository
    {
        private readonly KscHrContext _kscHrContext;

        public InterdictMaritalSettingRepository(KscHrContext context) : base(context)
        {
            _kscHrContext= context;
        }
        public IQueryable<InterdictMaritalSetting> GetAllByIncluded()
        {
            var query = _kscHrContext.InterdictMaritalSetting.Include(x => x.EmploymentType).OrderByDescending(x => x.Id).AsNoTracking();
            return query;
        }   
        
        public IQueryable<InterdictMaritalSetting> GetAllByRelatedDetail()
        {
            var query = _kscHrContext.InterdictMaritalSetting.Include(x => x.InterdictMaritalSettingDetails).OrderByDescending(x => x.Id).AsNoTracking();
            return query;
        }



    }
}
