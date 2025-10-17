using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewOdsJobPositionTree
{
    public class MeritJobViewModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string ChildDes { get; set; }
        //public string ParentDes { get; set; }
        //public int hasChild { get; set; }
        //public Nullable<decimal> TOT_NRM_PER_JPOS { get; set; }//روز کار
        //public Nullable<decimal> TOT_SHFT_PER_JPOS { get; set; }//شیفت
        //public Nullable<decimal> TOT_SHAR_PER_JPOS { get; set; }//برونسپاری روزکار
        //public Nullable<decimal> TOT_SHAR_PER_SHFT_JPOS { get; set; }// برون سپاری شیفت
        //public int? RASMI { get; set; }
        //public int? GHARDADI { get; set; }
    }
}
