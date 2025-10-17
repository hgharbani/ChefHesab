using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface ITeamWorkRepository : IRepository<TeamWork, int>
    {
        TeamWork GetByCode(string code);
        IQueryable<TeamWork> GetTeamWorksByWinUser(string winUser);
        IQueryable<TeamWork> GetAllQueryable();
        TeamWork GetTeamWorkByIdIncludedTeamWorkCategory(int id);
    }
}
