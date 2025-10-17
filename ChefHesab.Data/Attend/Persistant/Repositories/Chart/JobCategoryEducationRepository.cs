using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobCategoryEducationRepository : EfRepository<Chart_JobCategoryEducation, int>, IJobCategoryEducationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobCategoryEducationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobCategoryEducation> GetJobCategoryEducationById(int id)
        {
            return _kscHrContext.Chart_JobCategoryEducation.Where(a => a.Id == id);
        }
        public IQueryable<Chart_JobCategoryEducation> GetJobCategoryEducations()
        {
            var result = _kscHrContext.Chart_JobCategoryEducation.AsQueryable();
            return result;
        }
        public IQueryable<Chart_JobCategoryEducation> GetAllIncludeCategoryEducation()
        {
            var result = _kscHrContext
                .Chart_JobCategoryEducation
                .Include(x => x.EducationCategory)
                .Include(x => x.Chart_JobCategory)
                .AsQueryable();

            return result;
        }
        public Chart_JobCategoryEducation GetJobCategoryEducationByJobCategoryIdLevelNumber(int jobCategoryId, int levelNumber)
        {
            return _kscHrContext.Chart_JobCategoryEducation.FirstOrDefault(x => x.IsActive && x.JobCategoryId == jobCategoryId && x.LevelNumber == levelNumber);
        }
     
    }
}
