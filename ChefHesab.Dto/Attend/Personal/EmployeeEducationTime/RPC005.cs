using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEducationTime
{
    public class ARR
    {
        public string COD_TYP { get; set; }
        public string NUMP { get; set; }
        public string STIME { get; set; }
        public string ETIME { get; set; }
        public string FLG_ERR { get; set; }
        public string FLG_MIS { get; set; }
        public string COD_ERR { get; set; }
        public string DES_ERR { get; set; }
    }

   
    public class RPC005
    {
        public string WinUser { get; set; }
        public int TOT_CNT { get; set; }
        public string DATE { get; set; }
        public List<ARR> ARR { get; set; }
        public RPC005()
        {
            ARR = new List<ARR>();
        }
    }
}
