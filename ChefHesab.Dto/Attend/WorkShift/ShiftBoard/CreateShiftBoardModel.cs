using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class CreateShiftBoardModel
    {
        public List<CreateShiftBoardListModel> CreateShiftBoardListModel { get; set; }
        public int WorkGroupId { get; set; }
        public string PeriodStartDate { get; set; }
        public int RepetitionPeriod { get; set; }
        public List<SearchShiftConceptDetailModel> ShiftConceptDetailList { get; set; }
        public bool ModifyIsValid { get; set; }
        public bool HasPattern { get; set; }
        public bool HasRepetitionPeriod { get; set; }
        public bool ShiftBoardIsAlready { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; }

    }
}
