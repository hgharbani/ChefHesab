using KSC.Domain;
using Ksc.HR.Domain.Entities.EmployeeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IFranchiseTypeRepository :IRepository<FranchiseType,int>
    {
        Task<FranchiseType> GetOne(int id);
    }
}
