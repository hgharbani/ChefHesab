using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IEducationRepository : IRepository<Education, int>
    {
        IQueryable<Education> GetEducationById(int id);
        IQueryable<Education> GetEducationByIds(List<int> ids);
        IQueryable<Education> GetEducations();
    }
}
