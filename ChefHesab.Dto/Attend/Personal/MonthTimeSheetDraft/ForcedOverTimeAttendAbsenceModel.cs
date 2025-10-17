using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.MonthTimeSheetDraft
{
    public class ForcedOverTimeAttendAbsenceModel
    {
        public List<MonthTimeSheetDraftModel> InsertData { get; set; }
        public List<MonthTimeSheetDraftModel> DeleteData { get; set; }
    }
}
