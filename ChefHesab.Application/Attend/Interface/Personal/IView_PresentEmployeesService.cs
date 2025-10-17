using Ksc.Hr.Domain.Entities;
using Ksc.Hr.DTO.View_PresentEmployees;
using KSC.Common;
using KSC.Common.Filters.Models;
namespace Ksc.Hr.Application.Interfaces
{
    public interface IView_PresentEmployeesService
    {
        FilterResult<View_PresentEmployeesDto> GetAllContractInCompany(SearchPresentationDto filter);
        FilterResult<View_PresentEmployeesDto> GetAllKscEmployeeInCompany(SearchPresentationDto filter);
        FilterResult<TeamWorkPresentation> GetTeamGroupedKscInCompany(SearchPresentationDto filter);
        FilterResult<CostCenterPresentationDto> GetConstCenterGroupedKscInCompany(SearchPresentationDto filter);
        FilterResult<ContractPresentationDto> GetContractGroupedInCompany(SearchPresentationDto filter);
        FilterResult<JobPositionDetailsDto> GetTopChartKscInCompany(SearchPresentationDto filter);
        FilterResult<JobPositionDetailsDto> GetNexLevelTopChartKscInCompany(SearchPresentationDto filter);
        KscResult userIsTopChartManagement(SearchPresentationDto filter);
        FilterResult<WorkGroupPresentation> GetWorkGroupCountAllKscEmployeeInCompany(SearchPresentationDto filter);
    }
}

