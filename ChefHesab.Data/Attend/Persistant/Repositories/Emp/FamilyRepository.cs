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

using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Domain.Repositories;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class FamilyRepository : EfRepository<Family, int>, IFamilyRepository
    {
        private readonly KscHrContext _kscHrContext;
        public FamilyRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Family> GetAllQueryable()
        {
            return _kscHrContext.Family.AsQueryable();

        }

        public IQueryable<Family> GetForBookletRelated(int familyId)
        {
            return _kscHrContext.Family.Include(x => x.DependenceType).Include(x => x.Employee)
                .Include(x => x.InsuranceBooklets)
                .Where(x => x.Id == familyId)
                 .AsNoTracking()
                .AsQueryable();

        }
        /// <summary>
        /// شامل پرسنل شرکتی و جاری
        /// </summary>
        /// <param name="miladiDateV1"></param>
        /// <returns></returns>
        public IQueryable<Family> GetFamilyHaveChildCountForInterdict(DateTime miladiDateV1)
        {
            var familyQuery = GetAllQueryable()
                .Include(a => a.Employee).ThenInclude(x => x.PersonalType)
                //.ThenInclude(a => a.EmploymentType)
                // .Include(a => a.Employee).ThenInclude(a => a.MilitaryStatus)
               .AsNoTracking()
               .Where(p => p.Employee.PaymentStatusId != 7 && p.Employee.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id)
               .Where(c => (c.DependenceTypeId == EnumDependentType.ChildGirl.Id
               || c.DependenceTypeId == EnumDependentType.ChildBoye.Id)
               && c.Employee.PaymentStatusId != 7 &&
                      c.BirthDate <= miladiDateV1 &&
                     (!c.EndDateDependent.HasValue ||
                     ((c.DependenceTypeId == EnumDependentType.ChildGirl.Id && c.IsContinuesDependent == true)
                     || c.EndDateDependent.Value.Date >= miladiDateV1
                      )
                     ));
            return familyQuery;

        }
        /// <summary>
        /// شامل پرسنل شرکتی و جاری
        /// </summary>
        /// <param name="miladiDateV1"></param>
        /// <returns></returns>
        public IQueryable<Employee> GetEmployeeFamilyHaveChildCountForInterdict(DateTime miladiDateV1)
        {
            var familyQuery = _kscHrContext.Employees//.Where(x=>x.EmployeeNumber == "83186" ||  x.EmployeeNumber == "8924075")
                .Include(a => a.Families).Include(x => x.PersonalType)
               .AsNoTracking()
               .Where(p => p.PaymentStatusId != 7 && p.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id)
               .Where(a => a.Families.Any(c => (c.DependenceTypeId == EnumDependentType.ChildGirl.Id
               || c.DependenceTypeId == EnumDependentType.ChildBoye.Id)
               && c.Employee.PaymentStatusId != 7 &&
                      c.BirthDate <= miladiDateV1 &&
                     (!c.EndDateDependent.HasValue ||
                     ((c.DependenceTypeId == EnumDependentType.ChildGirl.Id && c.IsContinuesDependent == true)
                     || c.EndDateDependent.Value.Date >= miladiDateV1
                      )
                     )));
            return familyQuery;

        }






        public IQueryable<Family> GetFamilyHaveChildCount(DateTime miladiDateV1)
        {
            var familyQuery = GetAllQueryable()
                .Include(a => a.Employee).ThenInclude(a => a.EmploymentType)
                .Include(a => a.Employee).ThenInclude(a => a.MilitaryStatus)

               .AsNoTracking().Where(c => (c.DependenceTypeId == EnumDependentType.ChildGirl.Id
               || c.DependenceTypeId == EnumDependentType.ChildBoye.Id)
               && c.Employee.PaymentStatusId != 7 &&
                      c.BirthDate <= miladiDateV1 &&
                     (!c.EndDateDependent.HasValue ||
                     ((c.DependenceTypeId == EnumDependentType.ChildGirl.Id && c.IsContinuesDependent == true)
                     || c.EndDateDependent.Value.Date >= miladiDateV1
                      )
                     ));
            return familyQuery;

        }
        public IQueryable<Family> GetFamilyChildCount(DateTime miladiDateV1, int employeeId)
        {
            var familyQuery = GetAllQueryable().Include(a => a.Employee)
                .ThenInclude(a => a.EmploymentType)
                .Include(a => a.Employee).ThenInclude(a => a.MilitaryStatus)
               .AsNoTracking()
               .Where(c => c.EmployeeId == employeeId 
               && (c.DependenceTypeId == EnumDependentType.ChildGirl.Id || c.DependenceTypeId == EnumDependentType.ChildBoye.Id)
               && c.Employee.PaymentStatusId != 7 &&
                      c.BirthDate <= miladiDateV1 &&
                      c.IsActive == true &&
                     (!c.EndDateDependent.HasValue ||
                     ((c.DependenceTypeId == EnumDependentType.ChildGirl.Id
                     && c.IsContinuesDependent == true)
                     || miladiDateV1 <= c.EndDateDependent.Value.Date
                      )
                     ));
            return familyQuery;

        }

        public IQueryable<Family> GetAllRelated()
        {
            return _kscHrContext.Family.Include(x => x.DependenceType)
                .Include(x => x.Employee)
                 .AsNoTracking()
                .AsQueryable();

        }
        public IQueryable<Family> GetByIdRelated(int id)
        {
            return _kscHrContext.Family.Where(x => x.Id == id)
                .Include(x => x.DependenceType)
                .Include(x => x.BirthCity)
                .Include(x => x.CertificateCity)
                .AsQueryable();

        }

        public IQueryable<Family> GetByNationalCode(string nationalCode)
        {
            return _kscHrContext.Family.Where(x => x.NationalCode == nationalCode)

                .AsQueryable();

        }

        public IQueryable<Family> GetfamilyByEmployeeId(int employeeId)
        {
            return _kscHrContext.Family.Where(x => x.EmployeeId == employeeId).AsQueryable();
        }
        //public IQueryable<Family> GetfamilyByEmployeeIds(List<int> employeesId)
        //{
        //    return _kscHrContext.Family.Where().Include(a => a.Employee)
        //      .ThenInclude(a => a.EmploymentType)
        //      .Include(a => a.Employee).ThenInclude(a => a.MilitaryStatus)
        //     .AsNoTracking()
        //     .Where(a => employeesId.Any(x => x == a.EmployeeId)).AsQueryable();
        //}
    }
}
