using KSC.Common;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisCostCenter;
using Ksc.HR.DTO.ODSViews.ViewMisEmployeeSecurity;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Personal.Employee;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewMisEmployeeSecurityService
    {
        FilterResult<SearchViewMisEmployeeSecurityModel> GetViewEmployeeByKendoFilterForTeamWork(SearchViewMisEmployeeSecurityModel Filter);
        FilterResult<SearchViewMisEmployeeSecurityModel> GetViewMisEmployeeSecurityByKendoFilterSalaryUser(SearchViewMisEmployeeSecurityModel Filter);

        FilterResult<SearchViewMisEmployeeSecurityModel> GetViewMisEmployeeSecurityByKendoFilter(SearchViewMisEmployeeSecurityModel Filter);
        /// <summary>
        /// نمایش تمامی تیم های کاری
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        FilterResult<SearchViewMisEmployeeSecurityModel> GetAllViewMisEmployeeSecurityByKendoFilter(FilterRequest Filter);
        Task<decimal?> GetFirstDisplayTeamUser(string CurentUserName);
        List<decimal> GetcostCenterUserAccess(SearchUserInCostCenters filter);
        List<UserWindowTeamAccess> GetEmployeeWorkInCostCenter(SearchUserInCostCenters filter);
        FilterResult<SearchViewMisEmployeeSecurityModel> GetViewMisEmployeeSecurityByFilter(SearchViewMisEmployeeSecurityModel Filter);
    }
}
