using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Defines;
using Ksc.HR.Domain.Repositories.Define;
namespace Ksc.HR.Data.Persistant.Repositories
  {
  public partial class CountingUnitRepository : EfRepository<CountingUnit,int>,ICountingUnitRepository
  {
public CountingUnitRepository(KscHrContext context) : base(context)
{
}
}
}













