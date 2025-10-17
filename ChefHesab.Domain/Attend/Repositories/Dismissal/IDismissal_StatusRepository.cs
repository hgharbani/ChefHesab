using Ksc.HR.Domain.Entities.Dismissal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Dismissal
{
    public interface IDismissal_StatusRepository : IRepository<Dismissal_Status, int>
    {
        Task<bool> ExistsByTitle(int id, string title);
        Task<bool> ExistsByTitle(string title);
        Task<Dismissal_Status> GetOne(int id);
    }
}
