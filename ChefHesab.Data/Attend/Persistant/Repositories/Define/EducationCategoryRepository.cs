using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories
{
    public partial class EducationCategoryRepository : EfRepository<EducationCategory, int>, IEducationCategoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EducationCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EducationCategory> GetDataFromEducationCategoryForKSCContract()
        {
            return _kscHrContext.EducationCategory
                .IgnoreAutoIncludes();
        }
    }
}

