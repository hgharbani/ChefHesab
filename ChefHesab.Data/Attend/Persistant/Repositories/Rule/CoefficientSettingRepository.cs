using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class CoefficientSettingRepository : EfRepository<CoefficientSetting, int>, ICoefficientSettingRepository
    {
        private readonly KscHrContext _kscHrContext;

        public CoefficientSettingRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;

        }
        public IQueryable<CoefficientSetting> GetCoefficientSettings()
        {
            var result = _kscHrContext.CoefficientSetting.Include(x => x.Coefficient).AsQueryable();
            return result;
        }
        public CoefficientSetting GetCoefficientSettingByCoefficient_Year(int coefficientId, int year)
        {
            var x= _kscHrContext.CoefficientSetting.FirstOrDefault(x => x.CoefficientId == coefficientId && x.Year == year && x.IsActive == true);
            return x;
        }
    }
}
