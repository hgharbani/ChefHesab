using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class BasisSalaryItemRepository : EfRepository<BasisSalaryItem, int>, IBasisSalaryItemRepository
    {
        private readonly KscHrContext _kscHrContext;
        public BasisSalaryItemRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<BasisSalaryItem> GetAllByIncluded()
        {
            var query = _kscHrContext.BasisSalaryItems.Include(x => x.EmploymentType).AsNoTracking();

            return query;
        }
        public IQueryable<BasisSalaryItem> GetAllByIncludedSalaryItemPerGroups()
        {
            var query = _kscHrContext.BasisSalaryItems.Include(x => x.BasisSalaryItemPerGroups).AsNoTracking();

            return query;
        }
        public BasisSalaryItem GetBasisSalaryItemByYear(string year, int employmentTypeId)
        {
            var query = _kscHrContext.BasisSalaryItems.Where(x => x.StartDate.ToString().Substring(0, 4) == year
              && x.EmploymentTypeId == employmentTypeId
            && x.IsConfirmed == true && x.IsActive

            )
                .OrderByDescending(x => x.Id).FirstOrDefault();
            return query;
        }
        public BasisSalaryItem GetLastBasisSalaryItemByYear(string year, int employmentTypeId)
        {
           
            var query = _kscHrContext.BasisSalaryItems.Where(
                x => x.StartDate.ToString().Substring(0, 4)
                .CompareTo(year) < 0
            && x.EmploymentTypeId == employmentTypeId
            && x.IsConfirmed == true
            //&& x.IsActive
            ).OrderByDescending(x => x.Id).FirstOrDefault();

            return query;
        }
        public BasisSalaryItem GetLastBasisSalaryItemByYearMonth(int yearmonth, int employmentTypeId)
        {
            var query = _kscHrContext.BasisSalaryItems.Where(x => x.EmploymentTypeId == employmentTypeId && x.StartDate <= yearmonth && x.EndDate >= yearmonth &&  x.IsConfirmed)
                .OrderByDescending(x => x.Id).FirstOrDefault();
            return query;
        }

    }
}
