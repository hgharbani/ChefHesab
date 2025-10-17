using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DeductionAdditional;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IEmployeeDeductionAdditionalRepository : IRepository<EmployeeDeductionAdditional, int>
    {
        IQueryable<EmployeeDeductionAdditional> GetByDeductionAdditionalId(int deductionAdditionalId);
        IQueryable<EmployeeDeductionAdditional> GetDeductionAdditionals();
    }
}
