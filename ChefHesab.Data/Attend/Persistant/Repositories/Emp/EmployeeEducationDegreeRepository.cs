using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class EmployeeEducationDegreeRepository : EfRepository<EmployeeEducationDegree, int>, IEmployeeEducationDegreeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeEducationDegreeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeEducationDegree> GetAllRelated()
        {
            return _kscHrContext.EmployeeEducationDegrees.Include(x => x.StudyField).Include(x => x.Education).Include(x => x.Employee).AsQueryable();

        }
        public EmployeeEducationDegree GetOneRelatedDegree(int id)
        {
            return _kscHrContext.EmployeeEducationDegrees
                .Include(x => x.StudyField)
                .Include(x => x.Education).
                Include(x => x.Employee).FirstOrDefault(x=>x.Id==id);
        }
        public IQueryable<EmployeeEducationDegree> GetActiveByEmployeeID(int empId)
        {
            return _kscHrContext.EmployeeEducationDegrees.Where(x => x.EmployeeId == empId && x.IsActive).Include(x => x.StudyField)
                .Include(x => x.Education).ThenInclude(a=>a.EducationCategory).AsQueryable();

        }

        public EmployeeEducationDegree GetActiveByEmployeeIDForDetail(int empId)
        {
            return _kscHrContext.EmployeeEducationDegrees.AsNoTracking()
                .Where(x => x.EmployeeId == empId && x.IsActive).Include(x => x.StudyField)
                .Include(x => x.Education).ThenInclude(a => a.EducationCategory).FirstOrDefault();

        }
        public IQueryable<EmployeeEducationDegree> GetActiveByEmployeeIDs(List<int> empIds)
        {
            return _kscHrContext.EmployeeEducationDegrees.Where(x => empIds.Contains(x.EmployeeId));

        }

        public IQueryable<EmployeeEducationDegree> GetByEmployeeIdAndEducationId(int empId, int educationId)
        {
            return _kscHrContext.EmployeeEducationDegrees.Where(x => x.EmployeeId == empId && x.EducationId == educationId && x.IsActive);

        }

    }
}
