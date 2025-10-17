using KSC.Domain;
using Ksc.HR.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Emp;

namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeeBlackListRepository : IRepository<EmployeeBlackList, int>
    {
        IQueryable<EmployeeBlackList> GetAllByDate(DateTime startdate, DateTime enddate, int[] otherPaymentTypeId);
        IQueryable<EmployeeBlackList> GetAllByOtherPaymentTypeId(int[] otherPaymentTypeId);
        IQueryable<EmployeeBlackList> GetAllRelated();
    }
}
