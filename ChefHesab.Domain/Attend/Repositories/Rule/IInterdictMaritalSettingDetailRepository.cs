using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.Rule;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IInterdictMaritalSettingDetailRepository : IRepository<InterdictMaritalSettingDetail, int>
    {
        IQueryable<InterdictMaritalSettingDetail> GetAllByInterdictMaritalSettingId(int interdictMaritalSettingId);
        IQueryable<InterdictMaritalSettingDetail> GetInterdictMaritalSettingDetailsByFilterInterdict(SearchInterdictDetailDto model);
    }
}
