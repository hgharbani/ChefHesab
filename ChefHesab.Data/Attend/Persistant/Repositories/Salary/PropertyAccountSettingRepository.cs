using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class PropertyAccountSettingRepository : EfRepository<PropertyAccountSetting, int>, IPropertyAccountSettingRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PropertyAccountSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<PropertyAccountSetting> GetPropertyAccountSettingById(int id)
        {
            return _kscHrContext.PropertyAccountSettings.Where(a => a.Id == id);
        }
        public IQueryable<PropertyAccountSetting> GetPropertyAccountSettings()
        {
            var result = _kscHrContext.PropertyAccountSettings.AsQueryable();
            return result;
        }

        public IQueryable<PropertyAccountSetting> GetAllInclude()
        {
            return _kscHrContext.PropertyAccountSettings
                .AsQueryable();
        }
    }
}
