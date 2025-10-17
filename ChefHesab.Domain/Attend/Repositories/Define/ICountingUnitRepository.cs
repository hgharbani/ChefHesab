using KSC.Domain;
using Ksc.HR.Domain.Entities.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Entities.Defines;

namespace Ksc.HR.Domain.Repositories.Define
{
    public interface ICountingUnitRepository : IRepository<CountingUnit, int>
    {
       
    }
}
