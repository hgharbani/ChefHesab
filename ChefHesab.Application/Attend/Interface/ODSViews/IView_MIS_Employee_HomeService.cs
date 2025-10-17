using Ksc.Hr.Domain.Entities;
using Ksc.Hr.DTO.View_MIS_Employee_Home;
using KSC.Common.Filters.Models;
namespace Ksc.Hr.Application.Interfaces
{
    public interface IView_MIS_Employee_HomeService
    {
        View_MIS_Employee_HomeDto GetView_MIS_Employee_Home(string employeenumber);
    }
}

