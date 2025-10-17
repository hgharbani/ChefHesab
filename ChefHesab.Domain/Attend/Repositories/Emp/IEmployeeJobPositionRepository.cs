using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Common;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IEmployeeJobPositionRepository : IRepository<EmployeeJobPosition, int>
    {
        Task<bool> AddBulkAsync(List<EmployeeJobPosition> list);
        Task<KscResult> AddEmployeeJobPosition(EmployeeJobPosition model);
        List<EmployeeJobPosition> Get2LastRelatedJobPositionsEmployeeId(int? employeeId);
        IQueryable<EmployeeJobPosition> GetAllRelated();
        IQueryable<EmployeeJobPosition> GetByEmployeeIdRelated(int? employeeId);
        IQueryable<EmployeeJobPosition> GetByIdRelated(int id);
        IQueryable<EmployeeJobPosition> GetEmployeeJobPositionByEmployeeId(int? employeeId);
        List<EmployeeJobPosition> GetEmployeeJobPositionByEmployeeNumbersAsNoList(List<string> employeeId);
        IQueryable<EmployeeJobPosition> GetHistoriesRelocationByEmployeeIdRelated(int? employeeId);
        EmployeeJobPosition GetLastJobPositionEmployeeId(int? employeeId);
        KscResult UpdateEmployeeJobPosition(EmployeeJobPosition model);
    }
}
