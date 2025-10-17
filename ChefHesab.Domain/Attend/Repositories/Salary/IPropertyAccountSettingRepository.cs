using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IPropertyAccountSettingRepository : IRepository<PropertyAccountSetting, int>
    {
        IQueryable<PropertyAccountSetting> GetPropertyAccountSettingById(int id);
        IQueryable<PropertyAccountSetting> GetPropertyAccountSettings();
        IQueryable<PropertyAccountSetting> GetAllInclude();
    }
}
