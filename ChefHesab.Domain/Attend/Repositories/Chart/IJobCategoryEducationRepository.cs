using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobCategoryEducationRepository : IRepository<Chart_JobCategoryEducation, int>
    {
        IQueryable<Chart_JobCategoryEducation> GetJobCategoryEducationById(int id);
        IQueryable<Chart_JobCategoryEducation> GetJobCategoryEducations();
        IQueryable<Chart_JobCategoryEducation> GetAllIncludeCategoryEducation();
        Chart_JobCategoryEducation GetJobCategoryEducationByJobCategoryIdLevelNumber(int jobCategoryId, int levelNumber);
    }
}
