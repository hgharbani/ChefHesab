using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IBasisSalaryItemRepository : IRepository<BasisSalaryItem, int>
    {
        IQueryable<BasisSalaryItem> GetAllByIncluded();
        IQueryable<BasisSalaryItem> GetAllByIncludedSalaryItemPerGroups();
        BasisSalaryItem GetBasisSalaryItemByYear(string year, int employmentTypeId);
        BasisSalaryItem GetLastBasisSalaryItemByYear(string year, int employmentTypeId);
        BasisSalaryItem GetLastBasisSalaryItemByYearMonth(int yearmonth, int employmentTypeId);
    }
}
