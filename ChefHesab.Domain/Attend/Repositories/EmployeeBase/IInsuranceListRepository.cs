using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Entities.EmployeeBase;
using KSC.Common.Filters.Models;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IInsuranceListRepository : IRepository<InsuranceList, int>
    {
       
        IQueryable<InsuranceList> GetAllIncludeInsuranceType();
        IQueryable<InsuranceList> GetAllIncludeInsuranceTypeById(int? id);
        //  FilterResult<InsuranceListModel> GetJobIdentityByFilter(FilterRequest Filter);

    }
}
