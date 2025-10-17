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
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Repositories.BusinessTrip;

namespace Ksc.HR.Data.Persistant.Repositories.BusinessTrip
{
    public class Mission_TypeAccessLevelRepository : EfRepository<Mission_TypeAccessLevel, int>, IMission_TypeAccessLevelRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Mission_TypeAccessLevelRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Mission_TypeAccessLevel> GetAllMissionTypeAccessLevelsNoTracking()
        {
            var result = _kscHrContext.Mission_TypeAccessLevels.AsQueryable().Include(a => a.Mission_Type).AsNoTracking();
            return result;
        }
        public IQueryable<Mission_TypeAccessLevel> GetAllAccessLevelsNoTracking()
        {
            try
            {
                var result = _kscHrContext.Mission_TypeAccessLevels.AsQueryable().Include(a => a.AccessLevel).AsNoTracking();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public bool CheckAccessLevelUser(List<string> roles)
        {
            var accessLeveldata = GetAllAccessLevelsNoTracking().Any(a => roles.Contains(a.AccessLevel.RoleInIdentityService));
            return accessLeveldata;
        }
    }
}
