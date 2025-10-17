using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public partial class SacrificePercentageSettingRepository : EfRepository<SacrificePercentageSetting, int>, ISacrificePercentageSettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public SacrificePercentageSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<string> GetAttendAbsenceToleranceBySacrificePercentage(int sacrificePercentage)
        {
            var sacrificePercentageSetting =await _kscHrContext.SacrificePercentageSettings.FirstOrDefaultAsync(x => x.MinimumPercentage <= sacrificePercentage && x.MaximumPercentage >= sacrificePercentage);
            if (sacrificePercentageSetting != null)
                return sacrificePercentageSetting.AttendAbsenceTolerance;
            else
                return null;
        }

    }
}
