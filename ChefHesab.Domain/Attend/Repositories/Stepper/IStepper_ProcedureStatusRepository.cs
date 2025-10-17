using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Stepper;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public interface IStepper_ProcedureStatusRepository : IRepository<Stepper_ProcedureStatus, long>
    {
        bool CheckStepsDoneByYearMonth(int yearmonth, int parent, int procedureID);
        //List<int> GetLastStepsByYearmonth_Parentprocedure(int parentId, int yearmonth, int? lastsIndex);
        IQueryable<Stepper_ProcedureStatus> GetStepsByYearmonth(int yearmonth, int parent);
        IQueryable<Stepper_ProcedureStatus> GetStepsByYearmonth_Parentprocedure(int parentId, int yearmonth);
        Stepper_ProcedureStatus GetStepByYearmonth_procedure(int procedureId,int yearmonth);
        List<Stepper_ProcedureStatus> GetActiveBeforeStepsByYearmonth_Procedure(Stepper_Procedure model, int yearmonth);
        List<Stepper_ProcedureStatus> GetNoActiveBeforeStepsByYearmonth_Procedure(Stepper_Procedure model, int yearmonth);
        /// <summary>
        /// چک کردن اینکه مراحل قبل انجام شده؟
        /// </summary>
        /// <param name="model"></param>
        /// <param name="yearmonth"></param>
        /// <returns></returns>
        bool CheckNoValidBeforeDoneStepsByYearmonth_Procedure(Stepper_Procedure model, int yearmonth);
        bool CheckAllStepsDoneByYearMonthParent(int yearmonth, int parent);
        IQueryable<Stepper_ProcedureStatus> GetSteps(int parentId, int yearmonth);
        List<int> GetLastStepsByYearmonth_Parentprocedure(int parentId, IQueryable<Stepper_ProcedureStatus> allsteps);
    }
}

