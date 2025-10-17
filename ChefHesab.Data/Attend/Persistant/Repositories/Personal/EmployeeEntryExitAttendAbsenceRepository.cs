using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeEntryExitAttendAbsenceRepository : EfRepository<EmployeeEntryExitAttendAbsence, long>, IEmployeeEntryExitAttendAbsenceRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeEntryExitAttendAbsenceRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeEntryExitAttendAbsence> GetAllAsnotracking()
        {
            return _kscHrContext.EmployeeEntryExitAttendAbsences.AsNoTracking();
        }
        public List<long> GetEmployeeEntryExitHasAttendAbsences(List<long> entryExitIds)
        {
            var employeeEntryExitId = _kscHrContext.EmployeeEntryExitAttendAbsences
                                        .Where(a => entryExitIds.Any(x => x == a.EmployeeEntryExitId)

                                        ).Select(x => x.EmployeeEntryExitId).ToList();


            return employeeEntryExitId;
        }



    }
}
