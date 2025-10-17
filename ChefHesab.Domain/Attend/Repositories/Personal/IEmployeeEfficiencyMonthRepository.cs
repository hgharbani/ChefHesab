using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeEfficiencyMonthRepository:IRepository<EmployeeEfficiencyMonth, int>
    {
        IQueryable<EmployeeEfficiencyMonth> GetByYearMonth(int yearMonth);
    }
}
