using KSC.Domain;
using Ksc.Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.EmployeeBase;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IIsarStatusRepository : IRepository<IsarStatus, int>
    {
        IQueryable<IsarStatus> GetIsarStatusById(int id);
        IQueryable<IsarStatus> GetIsarStatus();
    }
}
