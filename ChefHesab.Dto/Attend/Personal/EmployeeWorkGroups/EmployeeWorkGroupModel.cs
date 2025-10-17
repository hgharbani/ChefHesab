using Ksc.HR.DTO.WorkShift.ShiftBoard;
using Ksc.HR.DTO.WorkShift.TeamWork;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeWorkGroups
{
    public class EmployeeWorkGroupModel : FilterRequest
    {
        public List<ShiftBoardModel> ShiftBoardModels { get; set; } = new List<ShiftBoardModel>();
        public List<TeamWorkModel> TeamWorkModels { get; set; } = new List<TeamWorkModel>();
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string PersianFromDate { get; set; }
        public string PersianToDate { get; set; }
        //public string FromDateUtc { get { return FromDate.HasValue ? FromDate.Value.ToString("s") : null; } }
        //public DateTime? ToDateUtc { get { return FromDate.HasValue ? ToDateUtc.Value.ToString("s") : null; } }
        public string WorkGroupCode { get; set; }
        public string WorkTimeTitle { get; set; }
        public bool IsActive { get; set; }
        public int WorkGroupId { get; set; }
        public string UserName { get; set; }
        public string DomainName { get; set; }
    }
}
