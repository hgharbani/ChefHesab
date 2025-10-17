using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.ODSViews
{
    public class ViewMisEmployeeRepository : EfRepository<ViewMisEmployee>, IViewMisEmployeeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisEmployeeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ViewMisEmployee> GetAllAsnoTracking()
        {
            return _kscHrContext.ViewMisEmployee.AsNoTracking();
        }
        public IEnumerable<ViewMisEmployee> GetMisEmployeesByJobPositionCode(string jobPositionCode)
        {
            return _kscHrContext.ViewMisEmployee.Where(x => x.JobPositionCode == jobPositionCode && x.PaymentStatusCode != 7);
        }
        public ViewMisEmployee GetEmployeeByPersonalNumber(string employeeNumber)
        {
            return _kscHrContext.ViewMisEmployee.FirstOrDefault(x => x.EmployeeNumber == employeeNumber);
        }
        public async Task<ViewMisEmployee> GetMisEmployeesByWinUser(string userName)
        {

          
            return  _kscHrContext.ViewMisEmployee.Where(x => x.WinUser == userName).FirstOrDefault();
        } 
        public ViewMisEmployee GetEmployeeByWinUser(string userName)
        {
            return  _kscHrContext.ViewMisEmployee.Where(x => x.WinUser == userName).FirstOrDefault();
        }

        public IQueryable<ViewMisEmployee> GetEmployeeByPersonalNumbers(List<string> employeeNumbers)
        {
            return this.GetAllAsnoTracking().Where(x => employeeNumbers.Contains(x.EmployeeNumber)); 
        }
        public IQueryable<ViewMisEmployee> GetMisEmployeesAcvtive()
        {
            return _kscHrContext.ViewMisEmployee.Where(x => x.PaymentStatusCode != 7);
        }
    }
}
