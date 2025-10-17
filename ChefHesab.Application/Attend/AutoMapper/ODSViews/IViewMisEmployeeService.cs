using Ksc.HR.DTO.ODSViews.ViewMisEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewMisEmployeeService
    {
        Task<ViewMisEmployeeModel> GetViewEmployeeByEmployeeNumber(string employeeNumber);
        List<SearchViewMisEmployee> GetViewMisEmployeeByKendoFilter(SearchViewMisEmployee Filter);
     
        List<string> GetListUserNameByJob(string filter);
        List<string> GetListUserNameByCategory(int categoryId);
        Task<ViewEmployeeInfoModel> GetEmployeeInfoByUserWin(string userWin);
        Task<List<ViewMisEmployeeForFinancialModel>> GetEmployeeForFinancialByEmployeeNumbers(SearchEmployeeForFinancial filter);
        //Task<ViewMisEmployeeModel> GetViewEmployeeByEmployeeNumber_Ver1(string employeeNumber);
    }
}
