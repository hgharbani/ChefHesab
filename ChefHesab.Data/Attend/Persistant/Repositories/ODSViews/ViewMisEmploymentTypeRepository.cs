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
    public class ViewMisEmploymentTypeRepository : EfRepository<ViewMisEmploymentType>, IViewMisEmploymentTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisEmploymentTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public decimal GetEmployeTypeCode(int id)
        {
            var model = _kscHrContext.ViewMisEmploymentType.FirstOrDefault(a=>a.EmploymentTypeCode==id);
            return model.EmploymentTypeCode;
        }
        public string GetEmployeTypeTitle(int id)
        {
            var model = _kscHrContext.ViewMisEmploymentType.FirstOrDefault(a=>a.EmploymentTypeCode==id);
            return model?.EmploymentTypeTitle;
        }
        public IQueryable<ViewMisEmploymentType> GetEmployeTypeCode()
        {
            var model = _kscHrContext.ViewMisEmploymentType.AsQueryable();
            return model;
        }
        //public IEnumerable<ViewMisEmploymentType> GetAll1()
        //{
        //    var model = _kscHrContext.ViewMisEmploymentTypes; 
        //    return model;
        //}
    }
}
