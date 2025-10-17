using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Share.Model.Rule;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IBasisSalaryItemPerGroupRepository : IRepository<BasisSalaryItemPerGroup, int>
    {
        List<BasisSalaryItemPerGroup> GetBasisSalaryItemPerGroupByItemId(int id);
        BasisSalaryItemPerGroup GetBasisSalaryItemPerGroupByFilterInterdict(SearchInterdictDetailDto model);
    }
}
