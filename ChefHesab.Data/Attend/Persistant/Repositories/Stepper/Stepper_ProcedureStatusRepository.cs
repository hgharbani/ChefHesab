using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public partial class Stepper_ProcedureStatusRepository : EfRepository<Stepper_ProcedureStatus, long>, IStepper_ProcedureStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Stepper_ProcedureStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public Stepper_ProcedureStatus GetStepByYearmonth_procedure(int procedureId, int yearmonth)
        {
            return _kscHrContext.Stepper_ProcedureStatus.SingleOrDefault(x => x.YearMonth == yearmonth && x.ProcedureId == procedureId);
        }
        public IQueryable<Stepper_ProcedureStatus> GetStepsByYearmonth_Parentprocedure(int parentId, int yearmonth)
        {
            return _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Where(x => x.YearMonth == yearmonth && x.Stepper_Procedure.ParentId == parentId).AsQueryable();
        }
        public List<int> GetLastStepsByYearmonth_Parentprocedure(int parentId, IQueryable<Stepper_ProcedureStatus> allsteps)
        {
            var lastsIndex = allsteps.Any(x => x.IsDone) ? allsteps.Where(x => x.IsDone).OrderByDescending(x => x.Stepper_Procedure.RunSequence).FirstOrDefault()?.Stepper_Procedure?.RunSequence : allsteps.OrderBy(x => x.Stepper_Procedure.RunSequence).FirstOrDefault()?.Stepper_Procedure?.RunSequence ?? 0;
            if (allsteps.Count(x => x.Stepper_Procedure.RunSequence == lastsIndex && x.IsDone) != _kscHrContext.Stepper_Procedures.Count(x => x.ParentId == parentId && x.RunSequence == lastsIndex && x.IsActive) && lastsIndex > 0)
            {
                lastsIndex = --lastsIndex;
            }
            var list = _kscHrContext.Stepper_Procedures.Include(x => x.Stepper_ProcedureStatus).Where(x => x.ParentId == parentId && x.RunSequence > lastsIndex && x.IsActive).AsEnumerable().GroupBy(x => x.RunSequence).OrderBy(x => x.Key).FirstOrDefault();
            var result = list?.Select(x => x.Id).ToList();
            if (result == null)
                return new List<int>();
            return result;
        }
        public IQueryable<Stepper_ProcedureStatus> GetSteps(int parentId, int yearmonth)
        {
            var allsteps = _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Where(x => x.Stepper_Procedure.ParentId == parentId && x.YearMonth == yearmonth);
            return allsteps;
        }
        public List<Stepper_ProcedureStatus> GetNoActiveBeforeStepsByYearmonth_Procedure(Stepper_Procedure model, int yearmonth)
        {
            var lastSteps = _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Where(x => x.ProcedureId != model.Id && x.Stepper_Procedure.ParentId == model.ParentId && x.YearMonth == yearmonth && x.Stepper_Procedure.RunSequence < model.RunSequence && x.IsDone && x.IsActive == false).AsEnumerable().GroupBy(x => x.Stepper_Procedure.RunSequence).OrderBy(x => x.Key).FirstOrDefault()?.Select(x => x).ToList();
            return lastSteps;
        }
        public List<Stepper_ProcedureStatus> GetActiveBeforeStepsByYearmonth_Procedure(Stepper_Procedure model, int yearmonth)
        {
            var lastSteps = _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Where(x => x.ProcedureId != model.Id && x.Stepper_Procedure.ParentId == model.ParentId && x.YearMonth == yearmonth && x.Stepper_Procedure.RunSequence < model.RunSequence && x.IsDone && x.IsActive == true).AsEnumerable().GroupBy(x => x.Stepper_Procedure.RunSequence).OrderBy(x => x.Key).FirstOrDefault()?.Select(x => x).ToList();
            return lastSteps;
        }
        public bool CheckNoValidBeforeDoneStepsByYearmonth_Procedure(Stepper_Procedure model, int yearmonth)
        {
            var countOldSteps = _kscHrContext.Stepper_Procedures.Count(x => x.ParentId == model.ParentId && x.RunSequence < model.RunSequence && x.IsActive);
            var lastActiveSteps = _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Where(x => x.ProcedureId != model.Id && x.Stepper_Procedure.ParentId == model.ParentId && x.YearMonth == yearmonth && x.Stepper_Procedure.RunSequence < model.RunSequence && x.IsActive == true).AsEnumerable().GroupBy(x => x.Stepper_Procedure.RunSequence).OrderBy(x => x.Key).FirstOrDefault()?.Select(x => x).ToList();
            if (lastActiveSteps != null)
            {
                if (lastActiveSteps.Count != countOldSteps)
                    return false;
                return lastActiveSteps.Any(x => x.IsDone == false);
            }
            else return false;
        }
        public IQueryable<Stepper_ProcedureStatus> GetStepsByYearmonth(int yearmonth, int parent)
        {
            return _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Where(x => x.YearMonth == yearmonth && x.Stepper_Procedure.ParentId == parent).AsQueryable();
        }
        public bool CheckStepsDoneByYearMonth(int yearmonth, int parent, int procedureId)
        {
            var proceduremodel = _kscHrContext.Stepper_Procedures.Find(procedureId);
            var countsteps = _kscHrContext.Stepper_Procedures.Count(x => x.ParentId == parent && x.RunSequence < proceduremodel.RunSequence && x.IsActive);
            return _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Count(x => x.YearMonth == yearmonth && x.Stepper_Procedure.ParentId == parent && x.IsDone && x.Stepper_Procedure.RunSequence < proceduremodel.RunSequence) == countsteps;
        }
        public bool CheckAllStepsDoneByYearMonthParent(int yearmonth, int parent)
        {
            var countsteps = _kscHrContext.Stepper_Procedures.Count(x => x.ParentId == parent && x.IsActive);
            int countproceduredone = _kscHrContext.Stepper_ProcedureStatus.Include(x => x.Stepper_Procedure).Count(x => x.YearMonth == yearmonth && x.Stepper_Procedure.ParentId == parent && x.IsDone);
            return countproceduredone == countsteps;
        }
    }
}

