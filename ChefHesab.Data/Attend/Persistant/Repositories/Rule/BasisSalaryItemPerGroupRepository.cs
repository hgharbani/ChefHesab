using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Share.Model.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class BasisSalaryItemPerGroupRepository : EfRepository<BasisSalaryItemPerGroup, int>, IBasisSalaryItemPerGroupRepository
    {
        private readonly KscHrContext _kscHrContext;
        public BasisSalaryItemPerGroupRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public List<BasisSalaryItemPerGroup> GetBasisSalaryItemPerGroupByItemId(int id)
        {
            var query = _kscHrContext.BasisSalaryItemPerGroups.Include(a => a.Chart_JobGroup).Where(x => x.BasisSalaryItemId == id && x.IsActive).OrderByDescending(x => x.Id).ToList();
            return query;
        }

        /// <summary>
        /// مزد گروه شغل
        /// </summary>
        public BasisSalaryItemPerGroup GetBasisSalaryItemPerGroupByFilterInterdict(SearchInterdictDetailDto model)
        {
            var query = _kscHrContext.BasisSalaryItemPerGroups.Include(a => a.BasisSalaryItem)
                .Where(x =>
            x.BasisSalaryItem.StartDate <= model.YearMonth && x.BasisSalaryItem.EndDate >= model.YearMonth
            && x.WorkGroupId == model.JobGroupId && x.SalaryAccountCodeId == model.AccountCodeId
            && x.BasisSalaryItem.IsConfirmed == true
            
            ).OrderByDescending(x => x.Id).FirstOrDefault();

            return query;
        }
    }
}
