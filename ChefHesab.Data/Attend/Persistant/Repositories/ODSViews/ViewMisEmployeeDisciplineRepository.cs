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
    public class ViewMisEmployeeDisciplineRepository : EfRepository<ViewMisEmployeeDiscipline>, IViewMisEmployeeDisciplineRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisEmployeeDisciplineRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ViewMisEmployeeDiscipline> GetViewMisEmployeeDiscipline(string employeenumber)
        {
            return _kscHrContext.ViewMisEmployeeDiscipline.Where(a => a.EmployeeNumber == employeenumber).OrderByDescending(a => a.ExecuteDate);
        }
        //public IEnumerable<ViewTeamManager> GetAll1()
        //{
        //    var model = _kscHrContext.ViewTeamManagers; 
        //    return model;
        //}
    }
}
