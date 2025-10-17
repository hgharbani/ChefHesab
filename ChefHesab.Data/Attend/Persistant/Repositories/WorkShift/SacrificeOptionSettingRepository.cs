using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public partial class SacrificeOptionSettingRepository : EfRepository<SacrificeOptionSetting, int>, ISacrificeOptionSettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public SacrificeOptionSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

    }
}
