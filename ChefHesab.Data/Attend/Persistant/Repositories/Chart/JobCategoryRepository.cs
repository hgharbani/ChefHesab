using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobCategoryRepository : EfRepository<Chart_JobCategory, int>, IJobCategoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_JobCategory> GetJobCategoryByCode(string code)
        {
            return _kscHrContext.Chart_JobCategory.Where(a => a.Code == code);
        }
        public IQueryable<Chart_JobCategory> GetJobCategoryById(int id)
        {
            return _kscHrContext.Chart_JobCategory.Where(a => a.Id == id);
        }
        public IQueryable<Chart_JobCategory> GetJobCategorys()
        {
            var result = _kscHrContext.Chart_JobCategory.AsQueryable();
            return result;
        }
        public IQueryable<Chart_JobCategory> GetAllIncludeCategory()
        {
            var result = _kscHrContext.Chart_JobCategory.Include(x => x.Chart_JobCategoryDefination).AsQueryable().AsNoTracking();
            return result;
        }

        public async Task<Chart_JobCategory> GetByIdIncludeEducationAsync(int id)
        {
            var result =await _kscHrContext.Chart_JobCategory.Include(x => x.Chart_JobCategoryEducations)
                .ThenInclude(c => c.EducationCategory)
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id == id);
            return result;
        }

        public async Task<Chart_JobCategory> GetForEditIncludeEducationAsync(int id)
        {
            var result =await _kscHrContext.Chart_JobCategory.Include(x => x.Chart_JobCategoryEducations)
                .ThenInclude(c => c.EducationCategory)
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public IQueryable<Chart_JobCategory> GetDataForKscContract()
        {
            var result = _kscHrContext.Chart_JobCategory
                .IgnoreAutoIncludes()
                .Include(x => x.Chart_JobCategoryDefination);
            return result;
        }
    }
}
