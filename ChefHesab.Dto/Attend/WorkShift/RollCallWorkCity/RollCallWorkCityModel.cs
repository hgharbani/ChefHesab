using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.RollCallWorkCity
{
    public class RollCallWorkCityModel
    {
        public int Id { get; set; }
        public int WorkCityId { get; set; } // WorkCityId
        public string WorkCityTitle { get; set; }  
        public DateTime StartDate { get; set; } // StartDate
        public DateTime EndDate { get; set; } // EndDate
        public int RowId {  get; set; }
    }
}
