  using KSC.Infrastructure.Persistance;
  using Ksc.HR.Domain.Entities;
  using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Data.Persistant.Context;

namespace Ksc.HR.Domain.Repositories.Stepper
{
  public partial class Stepper_ProcedureLogRepository : EfRepository<Stepper_ProcedureLog,long>,IStepper_ProcedureLogRepository
  {
public Stepper_ProcedureLogRepository(KscHrContext context) : base(context)
{
}
}
}

