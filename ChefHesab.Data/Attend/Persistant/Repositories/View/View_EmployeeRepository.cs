using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.View;
using Ksc.HR.Domain.Entities.ODSViews;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class View_EmployeeRepository : EfRepository<View_Employee>, IView_EmployeeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public View_EmployeeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }

        public async Task<View_Employee> GetEmployeesByWinUser(string userName)
        {
            return _kscHrContext.View_Employees.Where(x => x.WindowsUser == userName).FirstOrDefault();
        }
    }
}

