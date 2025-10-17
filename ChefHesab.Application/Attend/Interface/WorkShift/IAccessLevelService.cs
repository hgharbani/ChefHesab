using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.AccessLevel;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.DTO.WorkShift.WorkTime;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IAccessLevelService
    {
        void ExistsByTitle(int id, string name);
        void ExistsByTitle(string name);
        List<AccessLevelModel> GetAccessLevel();
        FilterResult<AccessLevelModel> GetAccessLevelByFilter(FilterRequest Filter);
        AccessLevel GetOne(int id);
        EditAccessLevelModel GetForEdit(int id);
       
        Task<KscResult> AddAccessLevel(AddAccessLevelModel model);
        Task<KscResult> UpdateAccessLevel(EditAccessLevelModel model);
        Task<KscResult> RemoveAccessLevel(EditAccessLevelModel model);
        List<SearchAccessLevelModel> GetAccessLevelsByKendoFilter(FilterRequest Filter);

    }
}
