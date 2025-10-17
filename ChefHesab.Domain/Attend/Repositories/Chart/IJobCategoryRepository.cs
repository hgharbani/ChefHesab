using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobCategoryRepository : IRepository<Chart_JobCategory, int>
    {
        IQueryable<Chart_JobCategory> GetJobCategoryById(int id);
        IQueryable<Chart_JobCategory> GetJobCategorys();
        IQueryable<Chart_JobCategory> GetAllIncludeCategory();

        Task<Chart_JobCategory> GetByIdIncludeEducationAsync(int id);
        Task<Chart_JobCategory> GetForEditIncludeEducationAsync(int id);
        IQueryable<Chart_JobCategory> GetJobCategoryByCode(string code);
        IQueryable<Chart_JobCategory> GetDataForKscContract();
    }
}
