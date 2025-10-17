using Ksc.HR.Domain.Entities.Rule;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface ICoefficientSettingRepository : IRepository<CoefficientSetting, int>
    {
        CoefficientSetting GetCoefficientSettingByCoefficient_Year(int coefficientId, int year);
        IQueryable<CoefficientSetting> GetCoefficientSettings();
    }
}
