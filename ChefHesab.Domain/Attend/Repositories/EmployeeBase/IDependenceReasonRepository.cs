
using Ksc.HR.Domain.Entities;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IDependenceReasonRepository : IRepository<DependenceReason, int>
    {
        IQueryable<DependenceReason> GetDataFromDependenceReasonForKSCContract();
    }
}
