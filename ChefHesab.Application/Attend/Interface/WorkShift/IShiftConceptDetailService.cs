using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.ShiftConcept;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IShiftConceptDetailService
    {
        void ExistsByCode(int id, string code);
        void Exists(int id, string name);
        void Exists(string name);
        List<ShiftConceptDetailModel> GetShiftConceptDetail();
        FilterResult<ShiftConceptDetailModel> GetShiftConceptDetailByFilter(FilterRequest Filter);
        ShiftConceptDetail GetOne(int id);
        EditShiftConceptDetailModel GetForEdit(int id);
        List<SearchShiftConceptDetailModel> GetWorkByKendoFilter(FilterRequest Filter);
        Task<KscResult> AddShiftConceptDetail(AddShiftConceptDetailModel model);
        Task<KscResult> UpdateShiftConceptDetail(EditShiftConceptDetailModel model);
        Task<KscResult> RemoveShiftConceptDetail(EditShiftConceptDetailModel model);
        Task<SearchShiftConceptDetailModel> GetShiftConceptDetailWithWOrkGroupIdDate(int workGroupId, int workCalendarId);
        Task<FilterResult<ShiftConceptDetailModel>> GetShiftConceptDetailListWithWOrkGroupId(SearchShiftConceptDetailModel model);
        Task<List<SearchShiftConceptModel>> GetShiftConceptDetailWithWOrkGroupIdDate(List<int> workGroupId, int workCalendarId);
        List<SearchShiftConceptDetailModel> GetShiftConceptDetailWithWOrkGroupIdAndWokCalendarIds(List<int> workGroupIds, List<int> workCalendarIds);
    }
}
