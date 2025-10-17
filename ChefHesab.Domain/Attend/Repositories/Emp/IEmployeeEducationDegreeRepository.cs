using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IEmployeeEducationDegreeRepository : IRepository<EmployeeEducationDegree, int>
    {
        IQueryable<EmployeeEducationDegree> GetActiveByEmployeeID(int empId);
        EmployeeEducationDegree GetActiveByEmployeeIDForDetail(int empId);
        IQueryable<EmployeeEducationDegree> GetActiveByEmployeeIDs(List<int> empIds);
        IQueryable<EmployeeEducationDegree> GetAllRelated();
        IQueryable<EmployeeEducationDegree> GetByEmployeeIdAndEducationId(int empId, int educationId);
        EmployeeEducationDegree GetOneRelatedDegree(int id);
    }
}
