using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWorkCategory;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace  Ksc.HR.Appication.Interfaces.WorkShift 
{
    public interface ITeamWorkCategoryService
    {
        void ExistsByCode(int id, string code);
        void Exists(int id, string name);

        void Exists(string name);
        List<TeamWorkCategoryModel> GetTeamWorkCategory();
        Task<FilterResult<TeamWorkCategoryModel>> GetTeamWorkCategoryByFilter(TeamWorkCategoryModel Filter);
        List<SearchTeamWorkCategoryModel> GetTeamWorkCategoryByKendoFilter(FilterRequest Filter);
        TeamWorkCategory GetOne(int id);
        
        EditTeamWorkCategoryModel GetForEdit(int id);

        Task<KscResult> AddTeamWorkCategory(AddTeamWorkCategoryModel model);
        Task<KscResult> UpdateTeamWorkCategory(EditTeamWorkCategoryModel model);
        Task<KscResult> RemoveTeamWorkCategory(EditTeamWorkCategoryModel model);
       
     
    }
}