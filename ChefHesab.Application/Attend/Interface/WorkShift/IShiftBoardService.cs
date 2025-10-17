using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftBoard;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IShiftBoardService
    {
        bool Exists(int id, string name);
        bool Exists(string name);
        List<ShiftBoardModel> GetShiftBoard();
        FilterResult<ShiftBoardModel> GetShiftBoardByFilter(FilterRequest Filter);
        ShiftBoard GetOne(int id);
       // EditShiftBoardModel GetForEdit(int id);
        List<SearchShiftBoardModel> GetWorkByKendoFilter(FilterRequest Filter);
        CreateShiftBoardModel GetDataCreateShiftBorad(CreateWorkShiftInputModel inputModel);
        //KscResult AddShiftBoard(AddShiftBoardModel model);
        //KscResult UpdateShiftBoard(EditShiftBoardModel model);
        //KscResult RemoveShiftBoard(EditShiftBoardModel model);

        WorkShiftViewModel GetMonthShift(searchMonthShift model);

        List<WorkShiftViewModel> GetShift(searchMonthShift model);

        Task<KscResult> CreateShiftBoard(CreateShiftBoardModel model);
        FilterResult<ShiftBoardListModel> GeShiftBoradGrid(ShiftBoardGridSearchModel Filter);
        List<WorkShiftViewModel> GetShiftName(searchMonthShift model);
        List<ShiftBoardByRangeDateModel> GetShiftBoardByRangeDate(ShiftBoardByRangeDateInputModel model);
    }
}
