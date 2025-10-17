using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobPositionScoreRepository : EfRepository<Chart_JobPositionScore, int>, IJobPositionScoreRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobPositionScoreRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public int GetJobposionIdByMisCode(string misJobpositionCode)
        {
            var jobpositionId = _kscHrContext.Chart_JobPosition.Where(x => x.MisJobPositionCode == misJobpositionCode).Select(x => x.Id).FirstOrDefault();
            return jobpositionId;
        }
        public bool GetRepeateJobpositionScore(string misJobpositionCode, int scoreTypeId, DateTime startDate,DateTime? endDate, int score)
        {
            var jobpositionId = GetJobposionIdByMisCode(misJobpositionCode);
            var result = _kscHrContext
                .Chart_JobPositionScore
                .Any(x => x.JobPositionId == jobpositionId &&
                x.JobPositionScoreTypeId == scoreTypeId &&
                x.Score == score &&
                (x.StartDate == startDate ||
                x.EndDate == endDate)
                );

            return result;

        }

        public Chart_JobPositionScore FindRepeateJobpositionScore(string misJobpositionCode, int scoreTypeId, DateTime startDate, DateTime? endDate, int score)
        {
            var jobpositionId = GetJobposionIdByMisCode(misJobpositionCode);
            var result = _kscHrContext
                .Chart_JobPositionScore
                .FirstOrDefault(x => x.JobPositionId == jobpositionId &&
                x.JobPositionScoreTypeId == scoreTypeId &&
                x.Score == score &&
                (x.StartDate == startDate||
                x.EndDate == endDate)
                );

            return result;

        }



        public Chart_JobPositionScore GetByMisJobpositionScore(string misJobpositionCode, int scoreTypeId, DateTime startdate)
        {
            var jobpositionId = GetJobposionIdByMisCode(misJobpositionCode);
            //var scoreTypeId = Convert.ToInt32(scoreType);
            var result = _kscHrContext.Chart_JobPositionScore
                .Where(x => x.JobPositionId == jobpositionId
                && x.JobPositionScoreTypeId == scoreTypeId
                && x.IsActive == true
                && !x.EndDate.HasValue).OrderByDescending(x => x.InsertDate).FirstOrDefault();

            return result;

        }

        //public IQueryable<Chart_JobPositionScore> GetJobpositionScoreForMis(string misJobpositionCode, string scoreType)
        //{
        //    var jobpositionId = GetJobposionIdByMisCode(misJobpositionCode);
        //    var scoreTypeId = Convert.ToInt32(scoreType);
        //    var result = _kscHrContext.Chart_JobPositionScore.Where(x => x.JobPositionId == jobpositionId && x.JobPositionScoreTypeId == scoreTypeId && x.IsActive == true);
        //    return result;
        //}
        public IQueryable<Chart_JobPositionScore> GetJobPositionScoreById(int id)
        {
            return _kscHrContext.Chart_JobPositionScore.Where(a => a.Id == id);
        }
        public IQueryable<Chart_JobPositionScore> GetJobPositionScoreByJobPositionId(int JobPositionId)
        {
            var x= _kscHrContext.Chart_JobPositionScore.Where(a => a.JobPositionId == JobPositionId);
            return x;
        }
        public IQueryable<Chart_JobPositionScore> GetJobPositionScores()
        {
            var result = _kscHrContext.Chart_JobPositionScore.AsQueryable();
            return result;
        }
        public IQueryable<Chart_JobPositionScore> GetJobPositionScoresAsNoTracking()
        {
            var result = _kscHrContext.Chart_JobPositionScore.Include(x => x.Chart_JobPositionScoreType).AsQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<Chart_JobPositionScore> ExistJobPositionScore(int jobPositionScoreTypeId, int jobPositionId)
        {
            var isExists = _kscHrContext
                .Set<Chart_JobPositionScore>()
                .Where(x =>
                x.JobPositionScoreTypeId == jobPositionScoreTypeId &&
                x.JobPositionId == jobPositionId

                ).AsQueryable().AsNoTracking();
            return isExists;

        }

    }
}
