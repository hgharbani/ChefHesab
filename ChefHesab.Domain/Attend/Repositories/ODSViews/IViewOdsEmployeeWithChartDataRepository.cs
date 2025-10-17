using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.ODSViews
{
    public interface IViewOdsEmployeeWithChartDataRepository : IRepository<ViewOdsEmployeeWithChartData>
    {
        IQueryable<ViewOdsEmployeeWithChartData> GetAllEmployeeWithChartData();
        IQueryable<ViewOdsEmployeeWithChartData> GetEmployeeRoozkarWithChartData();
        ViewOdsEmployeeWithChartData GetEmployeeWithChartDataByEmployeeNumber(string employeeNumber);
        List<ViewOdsEmployeeWithChartData> GetEmployeeWithChartDataByEmployeeNumbers(List<string> employeeNumber);
    }
}
