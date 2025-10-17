using Ksc.Hr.Domain.Entities;
 using KSC.Domain;
  namespace Ksc.Hr.Domain.Repositories
  {
    public interface IHrOptionRepository : IRepository<HrOption, int>
    {
        HrOption GetptionWithCode(int code);
    }
}

