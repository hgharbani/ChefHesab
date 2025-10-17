using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Chart;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IStudyFieldRepository : IRepository<StudyField, int>
    {
        IQueryable<StudyField> GetStudyFieldById(int id);
        IQueryable<StudyField> GetStudyFieldByIds(List<int> id);
        IQueryable<StudyField> GetStudyFieldsAsNoTracking();
        IQueryable<StudyField> GetStudyFieldsByJobPositionId();
        IQueryable<StudyField> GetDataFromStudyFieldForKSCContract();
    }
}
