using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IFamilyRepository : IRepository<Family, int>
    {
        IQueryable<Family> GetForBookletRelated(int familyId);

        IQueryable<Family> GetAllQueryable();
        IQueryable<Family> GetAllRelated();

        IQueryable<Family> GetByIdRelated(int id);
        IQueryable<Family> GetfamilyByEmployeeId(int employeeId);

        IQueryable<Family> GetByNationalCode(string nationalCode);
        IQueryable<Family> GetFamilyHaveChildCount(DateTime miladiDateV1);
        IQueryable<Family> GetFamilyChildCount(DateTime miladiDateV1, int employeeId);
        IQueryable<Family> GetFamilyHaveChildCountForInterdict(DateTime miladiDateV1);
        IQueryable<Employee> GetEmployeeFamilyHaveChildCountForInterdict(DateTime miladiDateV1);
        //IQueryable<Family> GetfamilyByEmployeeIds(List<int> employeesId);
    }
}
