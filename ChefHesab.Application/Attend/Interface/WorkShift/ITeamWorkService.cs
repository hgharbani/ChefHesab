using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.WorkShift.TeamWork;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ksc.HR.DTO.ODSViews.ViewMisEmployeeSecurity;

namespace  Ksc.HR.Appication.Interfaces.WorkShift 
{
    public interface ITeamWorkService
    {
        List<SearchTeamWorkModel> GetTeamWorkByKendoFilterByUser(SearchTeamWorkModel Filter);
        void ExistsByCode(int id, string code,int teamWorkCategoryId);
        void Exists(int id, string name, int teamWorkCategoryId);
        void Exists(string name, int teamWorkCategoryId);
        List<TeamWorkModel> GetTeamWork();
        FilterResult<TeamWorkModel> GetTeamWorkByFilter(TeamWorkGrdModel Filter);
        TeamWork GetOne(int id);
        EditTeamWorkModel GetForEdit(int id);
        List<SearchTeamWorkModel> GetWorkByKendoFilter(FilterRequest Filter);
        Task<KscResult> AddTeamWork(AddTeamWorkModel model);
        Task<KscResult> UpdateTeamWork(EditTeamWorkModel model);
        Task<KscResult> RemoveTeamWork(EditTeamWorkModel model);
        FilterResult<SearchTeamWorkModel> GetAllActiveWorkByFilter(FilterRequest Filter);
        FilterResult<SearchTeamWorkModel> GetActiveTeamWorkAsync(SearchTeamWorkModel Filter);
        

        FilterResult<SearchViewMisEmployeeSecurityModel> GetAllTeamWithCheckAccessKendoFilter(SearchViewMisEmployeeSecurityModel Filter);
        FilterResult<SearchViewMisEmployeeSecurityModel> GetAllTeamMapper(int[] ids);
        FilterResult<TeamWorkCostCenterViewModel> GetAllTeamWorksWithCostCenters(FilterRequest request);

        //List<TeamWorkModel> GetTeamWork();
        //FilterResult<TeamWorkModel> GetTeamWorkByFilter(FilterRequest Filter);

        //List<SearchTeamWorkModel> GetTeamWorkByKendoFilter(FilterRequest Filter);


    }
}