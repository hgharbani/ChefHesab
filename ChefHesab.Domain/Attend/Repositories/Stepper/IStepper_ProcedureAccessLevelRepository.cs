using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Stepper;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public interface IStepper_ProcedureAccessLevelRepository : IRepository<Stepper_ProcedureAccessLevel, int>
    {
        bool CheckAccessLevelUser(List<string> roles, int procedureId);
        IQueryable<Stepper_ProcedureAccessLevel> GetAllAccessLevelsNoTracking();
    }
}

