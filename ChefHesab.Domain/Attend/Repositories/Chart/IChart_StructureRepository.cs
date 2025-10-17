using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface  IChart_StructureRepository : IRepository<Chart_Structure, int>
    {
        IQueryable<Chart_Structure> GetAllInclude();
        IQueryable<Chart_Structure> GetAllIncludeForDiagram();
        Chart_Structure GetAssistance(int structureId);
        Chart_Structure GetAssistanceFromJobPositionwithStructureId(int structureId);
        IQueryable<Chart_Structure> GetByMisStructureCode(string misStructureCode);
        List<Chart_JobPosition> GetJobPostionFromStrucureId(int structureId, bool isCanSeeAll = false);
        List<Employee> GetManagementFromStrucureId(int structureId);
        List<Chart_Structure> GetOtherStrucureId(int jobPositionId);
        int GetStructureCode();
        Tuple<List<Chart_Structure>, List<Chart_JobPosition>, List<Employee>> GetSubgroupStructureAndJobPositionAndEmployee(int structureId);
    }
}

