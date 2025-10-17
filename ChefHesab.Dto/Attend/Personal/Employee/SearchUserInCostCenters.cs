using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class SearchUserInCostCenters
    {
        public SearchUserInCostCenters()
        {
            CostCenter = new List<decimal>();
        }
        public string WindowsUser { get;set; }
        public List<decimal> CostCenter { get;set; }
        public bool IsAdmin { get;set; }
    }
}
