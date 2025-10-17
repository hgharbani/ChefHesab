using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.EmployeeBase;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface ISavingTypeRepository : IRepository<SavingType, int>
    {
        Task<bool> ExistsByTitle(int id, string title);
        Task<bool> ExistsByTitle(string title);
        Task<SavingType> GetOne(int id);
    }
}
