using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSC.Common;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Share.Model.Chart;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IChart_JobPositionRepository : IRepository<Chart_JobPosition, int>
    {
        int? GetJobPositionIdByMisCode(string misCode);
        Chart_JobPosition GetChart_JobPositionParentByMisCode(string ParentMisCode);
        int GetJobPositionCode();
        IQueryable<Chart_JobPosition> GetChart_JobPositionIncludedById(int id);
        IQueryable<Chart_JobPosition> GetChart_JobPositions();
        IQueryable<Chart_JobPosition> GetChart_JobPositionsIncluded();
        IQueryable<Chart_JobPosition> GetChart_JobPositionsIncludedParent();
        IQueryable<Chart_JobPosition> GetHobCategoryDefinationIdByJobPositionId(int id);
        Chart_JobPosition GetChart_JobPositionByMisCode(string MisCode);
        Tuple<bool, string> CheckCountRemaining(int EmployeeId, int JobPositionId);
        IQueryable<Chart_JobPosition> GetjobCategoryDefinationIdStatusIdByJobPositionId(int id);
        List<KscResult<Chart_JobPosition>> GetSuperiorJobPosition(int[] ids);
        IQueryable<Chart_JobPosition> GetChart_JobPositionsByStatusCategory(int StatusCategoryId);
        int GetJobPositionOrderNo();
        IQueryable<Chart_JobPosition> GetChart_JobPositionById(int id);
        IQueryable<Chart_JobPosition> GetByIncluded();
        List<Chart_JobPosition> GetChart_JobPositionsforInterdictCalculation(List<int?> JobPositionsId);
        IQueryable<Chart_JobPosition> getActived();
        //int GetSpecificReward(string SpecificRewardCode);

        Chart_JobPosition GetOneByRelations(int id);
        Tuple<int, int, int> GetRemainingCapacity(int jobPositionId);
        List<Employee> GetSubGroupEmployeeFromJobPositionId(int jobPositionId, bool isWantCheckRemain = false, bool isCanSeeAll = false, bool excludeEndStructure = true);
        List<Chart_JobPosition> GetSubGroupJobPostionFromJobPositionId(int jobPositionId, bool isCanSeeAll = false, bool excludeEndStructure = true);
        JobPositionDetailModel GetDetailedJobPostion(int jobPositionId);
        List<JobPositionDetailModel> GetDetailedSubGroupJobPostion(int jobPositionId, bool isCanSeeAll = false);
        List<Tuple<Chart_JobPosition, Employee>> GetEmployeeParent(List<int> jobpositionIds);
        List<Chart_JobPosition> GetAllSubGroupJobPostionFromJobPositionId(int jobPositionId, bool checkStructureEnd = true , bool checkEndDate = false , bool checkIsActive = true, bool checkIdendtity = true);
        List<Tuple<Chart_JobPosition, Employee>> GetListSuperiorJobPosition(int id);
        List<Tuple<Chart_JobPosition, int>> GetListRemainingJobPosition(List<int> jobpositionId);
        bool IsActiveEmployee(int employeeId);
        List<Tuple<Chart_JobPosition, int, int>> GetListRemainingJobPositionWithEmployeeCount(List<int> jobpositionId);
        List<Employee> GetAllEmployeeFromJobPositionId(int jobPositionId, bool isWantCheckRemain = false, bool isCanSeeAll = false);
        List<Share.Chart.ChartJobPosition.ChartSuperiorPositionWithEmployees> GetSuperiorJobPositionForCartabl(List<int> ids);
        List<Tuple<Chart_JobPosition, Employee>> GetAllSuperiorJobPosition(int id);
        string FindCommonPost(int source, int destination);
    }
}
