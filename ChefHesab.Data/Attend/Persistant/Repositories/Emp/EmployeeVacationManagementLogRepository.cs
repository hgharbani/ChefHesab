using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class EmployeeVacationManagementLogRepository : EfRepository<EmployeeVacationManagementLog, long>, IEmployeeVacationManagementLogRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeVacationManagementLogRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        //public List<EmployeeVacationManagement> GetFromLog()
        //{
        //    var filterStri
        //    var log = _kscHrContext.LogDatas.Where(a => a.EntityTypeId == 30 && a.Value.EmployeeId && EF.Functions.va(a.Value.EmployeeId,"$.EmployeeId")==);

        //    return new List<EmployeeVacationManagement>();
        //}
  
    }
}

