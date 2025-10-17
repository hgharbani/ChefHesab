using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class AccountCodeCategoryRepository : EfRepository<AccountCodeCategory, int>, IAccountCodeCategoryRepository
    {

        private readonly KscHrContext _kscHrContext;
        public AccountCodeCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<AccountCodeCategory> GetAccountCodeCategoryById(int id)
        {
            return _kscHrContext.AccountCodeCategories.Where(a => a.Id == id);
        }
        public IQueryable<AccountCodeCategory> GetAccountCodeCategories()
        {
            var result = _kscHrContext.AccountCodeCategories.AsQueryable();
            return result;
        }

        public IQueryable<AccountCodeCategory> GetAllInclude()
        {
            return _kscHrContext.AccountCodeCategories
                .Include(a => a.Parent)
                .Include(a => a.AccountCodeCategories)
                .AsQueryable();

            //var result = _kscHrContext.Chart_JobPosition
            //    .Include(a => a.Parent)
            //    .Include(a => a.Chart_Structure)
            //    .ThenInclude(a => a.Chart_StructureType)
            //    .Include(a => a.Chart_JobPositions)
            //    .ThenInclude(a => a.Chart_JobPositions)
            //    .ThenInclude(a => a.Chart_JobPositions)
            //    .ThenInclude(a => a.Chart_JobPositions)
            //    .ThenInclude(a => a.Chart_JobPositions)
            //    .ThenInclude(a => a.Chart_JobPositions)
            //    .Include(a => a.Chart_JobPositions)
            //    .ThenInclude(a => a.Chart_Structure)
            //    .ThenInclude(a => a.Chart_StructureType)
            //    .AsQueryable().AsNoTracking();
        }
    }
}
