using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWorkMangementCode;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public interface ITeamWorkMangementCodeService
    {
        void ExistsByCode(int id, string code);
        void Exists(int id, string name);

        void Exists(string name);
        List<TeamWorkMangementCodeModel> GetTeamWorkMangementCodes();
        FilterResult<TeamWorkMangementCodeModel> GetTeamWorkMangementCodesByFilter(FilterRequest Filter);
        TeamWorkMangementCode GetOne(int id);
        List<SearchTeamWorkMangementCodeModel> GetTeamWorkMangementCodesByKendoFilter(FilterRequest Filter);
        EditTeamWorkMangementCodeModel GetForEdit(int id);

        Task<KscResult> AddTeamWorkMangementCode(AddTeamWorkMangementCodeModel model);
        Task<KscResult> UpdateTeamWorkMangementCode(EditTeamWorkMangementCodeModel model);
        Task<KscResult> RemoveTeamWorkMangementCode(EditTeamWorkMangementCodeModel model);
       
     
    }
}