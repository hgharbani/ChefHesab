using Ksc.Hr.Domain.Entities;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
{
    public interface IProductionEfficiencyRepository : IRepository<ProductionEfficiency, int>
    {
        IQueryable<ProductionEfficiency> GetActiveNotZero();
        //IQueryable<ProductionEfficiency> GetProductionEfficiency(int yearmonth);
        IQueryable<ProductionEfficiency> GetProductionEfficiency();

    }
}

