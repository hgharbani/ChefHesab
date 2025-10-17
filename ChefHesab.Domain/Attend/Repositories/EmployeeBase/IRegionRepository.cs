using Ksc.Hr.Domain.Entities;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IRegionRepository:IRepository<Region, int>
    {
        IQueryable<Region> GetRegionById(int id);
        IQueryable<Region> GetRegions();
    }
}
