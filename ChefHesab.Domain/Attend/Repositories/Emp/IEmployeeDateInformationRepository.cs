using KSC.Domain;
using Ksc.HR.Domain.Entities.Emp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.EmployeeDateInformation;

namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeeDateInformationRepository : IRepository<EmployeeDateInformation, int>
    {
        Task<bool> AddBulkAsync(List<EmployeeDateInformation> list);
        IQueryable<EmployeeDateInformation> GetAllRelated();
        public EmployeeDateInformation GetByEmployeeId(int employeeId);
        Task<bool> UpdateLastUpgradeDate(List<UpdateLastUpgradeDateModel> list, string username);
        Task<bool> UpdateLastUpgradeDate(List<UpdateDateModel> list, string username);
        void UpdateLastUpgradeDate(int yearMonth);
        void UpdateRange(List<EmployeeDateInformation> list);
    }
}
