using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftHoliday;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Other;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IShiftHolidayService
    {
        bool Exists(int id, int dayNumber);
        bool Exists(int dayNumber);
        List<ShiftHolidayModel> GetShiftHoliday();
        FilterResult<ShiftHolidayModel> GetShiftHolidayByFilter(ShiftHolidayModel Filter);
        ShiftHoliday GetOne(int id);
        EditShiftHolidayModel GetForEdit(int id);
        Task<KscResult> AddShiftHoliday(AddShiftHolidayModel model);
        KscResult UpdateShiftHoliday(EditShiftHolidayModel model);
        KscResult RemoveShiftHoliday(EditShiftHolidayModel model);
        List<SelectListItem> GetListNumberType();

    }
}
