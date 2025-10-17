using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.IncreaseSalary;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IIncreaseSalaryHeaderRepository : IRepository<IncreaseSalaryHeader, int>
    {
        IncreaseSalaryHeader GetLatestIncreaseSalaryHeader(int yearMonth);
        //IncreaseSalaryHeader GetLatestIncreaseSalaryHeader();
        //IQueryable<IncreaseSalaryHeader> GetLatestIncreaseSalaryHeader();
        IQueryable<IncreaseSalaryHeader> GetIncreaseSalaryHeaderByRelated(int? year);
        Task<List<IncreaseSalaryAccountEmploymentTypeInterdictDto>> GetInterdictsForIncreaseSalary(IncreaseSalaryHeader header, bool hasdetail, EmployeeSearchModel searchModel);
        IncreaseSalaryVM GetIncreaseSalary(DateTime? executeDate);
        IncreaseSalaryHeader GetLatestIncreaseSalaryHeaderWithRelation(int year);
    }
}
