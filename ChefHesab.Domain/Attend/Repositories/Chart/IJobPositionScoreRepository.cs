using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobPositionScoreRepository : IRepository<Chart_JobPositionScore, int>
    {
        Chart_JobPositionScore GetByMisJobpositionScore(string misJobpositionCode, int scoreType,DateTime startdate);
        public int GetJobposionIdByMisCode(string misJobpositionCode);
        //bool GetByMisJobpositionScore(string misJobpositionCode, string scoreType);
        IQueryable<Chart_JobPositionScore> GetJobPositionScoreById(int id);
        IQueryable<Chart_JobPositionScore> GetJobPositionScores();
        IQueryable<Chart_JobPositionScore> GetJobPositionScoresAsNoTracking();
        IQueryable<Chart_JobPositionScore> ExistJobPositionScore(int jobPositionScoreTypeId, int jobPositionId);
        bool GetRepeateJobpositionScore(string misJobpositionCode, int scoreType, DateTime startDate, DateTime? endDate, int score);
        Chart_JobPositionScore FindRepeateJobpositionScore(string misJobpositionCode, int scoreTypeId, DateTime startDate, DateTime? endDate, int score);
        IQueryable<Chart_JobPositionScore> GetJobPositionScoreByJobPositionId(int JobPositionId);
        //IQueryable<Chart_JobPositionScore> GetJobpositionScoreForMis(string misJobpositionCode, string scoreType);
    }
}
