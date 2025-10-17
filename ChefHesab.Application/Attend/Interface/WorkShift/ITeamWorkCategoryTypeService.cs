using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWorkCategoryType;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface ITeamWorkCategoryTypeService
    {
        void ExistsByCode(int id, string code);
        void Exists(int id, string name);

        void Exists(string name);
        List<TeamWorkCategoryTypeModel> GetTeamWorkCategoryTypes();
        FilterResult<TeamWorkCategoryTypeModel> GetTeamWorkCategoryTypesByFilter(FilterRequest Filter);
        TeamWorkCategoryType GetOne(int id);
        List<SearchTeamWorkCategoryTypeModel> GetTeamWorkCategoryTypesByKendoFilter(FilterRequest Filter);
        EditTeamWorkCategoryTypeModel GetForEdit(int id);

        Task<KscResult> AddTeamWorkCategoryType(AddTeamWorkCategoryTypeModel model);
        Task<KscResult> UpdateTeamWorkCategoryType(EditTeamWorkCategoryTypeModel model);
        Task<KscResult> RemoveTeamWorkCategoryType(EditTeamWorkCategoryTypeModel model);
       
     
    }
}