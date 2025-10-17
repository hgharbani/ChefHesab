using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobPositionNatureSubGroupRepository : IRepository<Chart_JobPositionNatureSubGroup, int>
    {
        IQueryable<Chart_JobPositionNatureSubGroup> GetSubGroupById(int id);
        IQueryable<Chart_JobPositionNatureSubGroup> GetSubGroups();
        IQueryable<Chart_JobPositionNatureSubGroup> GetSubGroupsRelated();
    }
}
