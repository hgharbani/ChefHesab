using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.ODSViews
{
    public class ViewMisCostCenterRepository : EfRepository<ViewMisCostCenter>, IViewMisCostCenterRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisCostCenterRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        //public IEnumerable<ViewMisCostCenter> GetAll1()
        //{
        //    var model = _kscHrContext.ViewMisCostCenters; 
        //    return model;
        //}

        public ViewMisCostCenter GetViewMisCostCenterByCode(decimal costCenterCode)
        {
            return _kscHrContext.ViewMisCostCenters
                .FirstOrDefault(x => x.CostCenterCode == costCenterCode);
        }
        public List<ViewMisCostCenter> GetViewMisCostCenterByCodes(List<decimal> costCenterCode)
        {
            return _kscHrContext.ViewMisCostCenters
                .Where(x => costCenterCode.Contains(x.CostCenterCode)).ToList();
        }
    }
}
