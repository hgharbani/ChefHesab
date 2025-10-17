using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Share.Model.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class AccountEmploymentTypeRepository : EfRepository<AccountEmploymentType, int>, IAccountEmploymentTypeRepository
    {
        private readonly KscHrContext _kscHrContext;

        public AccountEmploymentTypeRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;
        }

        public IQueryable<AccountEmploymentType> GetAccountEmploymeeWithEmpType(int? employeeId, int? empTypeId)
        {

            var result = _kscHrContext
                .AccountEmploymentType
                .Include(x => x.AccountCode)
                .ThenInclude(x=> x.InterdictCategory)
                .Include(x => x.EmploymentType)
                .Where(x => x.EmploymentTypeId == empTypeId &&
                            x.AccountCode.IsActive == true &&
                            x.IsActive == true &&
                            x.AccountCode.InterdictCategoryId !=null)
                .AsQueryable()
                .OrderBy(x => x.AccountCode.InterdictCategoryId);

            return result;
        }
        public IQueryable<AccountEmploymentType> GetAllIncludesAccount()
        {

            var result = _kscHrContext
                .AccountEmploymentType
                .Include(x => x.AccountCode)
                .ThenInclude(x => x.InterdictCategory)
                .Include(x => x.EmploymentType)
                .AsQueryable()
                .OrderBy(x => x.AccountCode.InterdictCategoryId);

            return result;
        }

        public IQueryable<AccountEmploymentType> GetAccountEmploymee(int? employeeId)
        {
            var result = _kscHrContext.AccountEmploymentType.Include(x => x.AccountCode).Include(x => x.EmploymentType).ThenInclude(x => x.Employees).Where(x => x.EmploymentType.Employees.Any(a => a.Id == employeeId)).AsQueryable();
            return result;
        }
        public IQueryable<AccountEmploymentType> GetAccountEmploymentTypes(int? employmentType)
        {
            var result = _kscHrContext.AccountEmploymentType.Include(x => x.AccountCode).Include(x => x.EmploymentType).Where(x=>x.EmploymentType.Id== employmentType && x.IsActive).AsQueryable();
            return result;
        }
        /// <summary>
        /// مربوط به مزد گروه شغل
        /// </summary>
        /// <param name="employmentTypeId"></param>
        /// <returns></returns>
        public IQueryable<AccountEmploymentType> GetActive_IsGroupSalaryAmount_AccountEmploymentTypes(int? employmentTypeId)
        {
            var result = GetAccountEmploymentTypes(employmentTypeId).Where(a => a.AccountCode.InterdictCategoryId == EnumInterdictCategory.GroupSalaryAmount.Id && a.IsActive).AsQueryable();
            return result;
        }

    }
}
