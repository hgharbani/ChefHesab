using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public partial class Stepper_ProcedureAccessLevelRepository : EfRepository<Stepper_ProcedureAccessLevel, int>, IStepper_ProcedureAccessLevelRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Stepper_ProcedureAccessLevelRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Stepper_ProcedureAccessLevel> GetAllAccessLevelsNoTracking()
        {
            try
            {
                var result = _kscHrContext.Stepper_ProcedureAccessLevels.AsQueryable().Include(a => a.AccessLevel).AsNoTracking();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public bool CheckAccessLevelUser(List<string> roles,int procedureId)
        {
            var accessLeveldata = GetAllAccessLevelsNoTracking().Any(a => roles.Contains(a.AccessLevel.RoleInIdentityService) && a.ProcedureId == procedureId);
            return accessLeveldata;
        }
    }
}

