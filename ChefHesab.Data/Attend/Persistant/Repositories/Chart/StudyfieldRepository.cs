using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class StudyFieldRepository : EfRepository<StudyField, int>, IStudyFieldRepository
    {
        private readonly KscHrContext _kscHrContext;
        public StudyFieldRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<StudyField> GetStudyFieldById(int id)
        {
            return  _kscHrContext.StudyField.Where(a => a.Id == id);
        }
        public IQueryable<StudyField> GetStudyFieldByIds(List<int> ids)
        {
            return _kscHrContext.StudyField.Where(a => ids.Contains(a.Id));
        }
        public IQueryable<StudyField> GetStudyFieldsAsNoTracking()
        {
            return _kscHrContext.StudyField.AsNoTracking().AsQueryable();
        }
        public IQueryable<StudyField> GetStudyFieldsByJobPositionId()
        {
            return _kscHrContext.StudyField.Include(a=>a.Chart_JobPositionFields).AsNoTracking().AsQueryable();
        }
        public IQueryable<StudyField> GetDataFromStudyFieldForKSCContract()
        {
            return _kscHrContext.StudyField
                .IgnoreAutoIncludes();
        }
    }
}
