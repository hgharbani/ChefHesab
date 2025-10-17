using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeTimeSheet
{
    public class EmployeeTimeSheetMonthReportSearchModel: FilterRequest
    {
     public int YearMonth { get; set; }// 
     public string YearMonthString { get; set; }// 

    }
}
