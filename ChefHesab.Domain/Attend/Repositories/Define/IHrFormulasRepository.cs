using Ksc.Hr.Domain.Entities;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
{
    public interface IHrFormulasRepository : IRepository<HrFormulas, int>
    {
        double GetValueAsFormula(int CodeFormula, Dictionary<string, object> parameters);
    }
}

