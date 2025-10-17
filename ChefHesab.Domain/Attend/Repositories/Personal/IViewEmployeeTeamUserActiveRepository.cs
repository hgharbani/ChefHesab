using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IViewEmployeeTeamUserActiveRepository : IRepository<ViewEmployeeTeamUserActive>
    {
        IQueryable<ViewEmployeeTeamUserActive> GetAllAsNoTracking();
        IQueryable<ViewEmployeeTeamUserActive> GetAllQueryable();
        IQueryable<ViewEmployeeTeamUserActive> GetByUser(string currentUser);
    }
}
