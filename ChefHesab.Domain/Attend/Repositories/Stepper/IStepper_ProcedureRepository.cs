using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Stepper;
using KSC.Common;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public interface IStepper_ProcedureRepository : IRepository<Stepper_Procedure, int>
    {
        Tuple<int, string> CheckEventIsFinished(int? StepId, int? YearMonth);
        Tuple<int, string> CheckEventWithTitleIsFinished(int? YearMonth, string title);
        Task<Stepper_Procedure> GetOne(int id);
        IQueryable<Stepper_Procedure> GetProceduresByParentID(int id);
    }
}

