using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkGroup;
using KSC.Common.Filters.Models;
using System.Collections.Generic;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IWorkGroupService
    {
       List<string> GetLatinAlphabet();
        WorkGroup GetWorkGroupByCode(string code);
        IEnumerable<WorkGroup> GetWorkGroupByWorkTimeId(int id);
        FilterResult<WorkGroupListModel> GetWorkGroupByFilter(FilterRequest Filter);

        List<SearchWorkGroupModel> GetWorkGroupByKendoFilter(FilterRequest Filter);
    }
}