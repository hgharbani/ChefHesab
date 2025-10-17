using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.ODSViews
{
    public interface IViewMisEmploymentTypeRepository : IRepository<ViewMisEmploymentType>
    {
        //public IEnumerable<ViewMisCostCenter> GetAll1();
        decimal GetEmployeTypeCode(int id);
        IQueryable<ViewMisEmploymentType> GetEmployeTypeCode();
        string GetEmployeTypeTitle(int id);
    }
}
