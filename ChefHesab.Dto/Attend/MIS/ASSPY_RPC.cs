using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.MIS
{
    public class ASSPY_RPC
    {
        public string OPERATION { get; set; }

        public string NUM_PRSN_EMPL { get; set; } // پرسنلي

        public string DAT_STR_ASSPY { get; set; } // تاريخ شروع

        public string DAT_END_ASSPY { get; set; } // تاريخ پايان	

        public string TOT_DAY_ASSPY { get; set; } // مدت ماموريت		

        public string DAT_ISSU_ASSPY { get; set; }  // تاريخ صدور

        public string FK_ASSTY { get; set; }    // نوع ماموريت

        public string NUM_INTRD_ASSPY { get; set; }  // شماره حکم

        public string COD_CNTRY_CITY { get; set; }  // کشور

        public string COD_RGN_CITY { get; set; }    // استان

        public string COD_CITY { get; set; }        // شهر

        public string DES_ASSPY { get; set; }       // موضوع ماموريت

        public string COD_STATUS_ASSPY { get; set; }    // وضعيت ماموريت

        public string COD_USR_ASSPY { get; set; }   // کاربر انجام دهنده

        public string NUM_PRSN_DPTY { get; set; } // جانشين

        public string COD_STY_ASSPY { get; set; } // نوع اقامت	

        public string AMN_STY_ASSPY { get; set; } // هزينه اقامت	

        public string AMN_CAR_ASSPY { get; set; } // هزينه وسيله نقليه	

        public string AMN_TRP_ASSPY { get; set; } // هزينه سفر	

        public string AMN_VRS_ASSPY { get; set; } // هزينه هاي متفرقه	

        public string TOT_AMN_EXP_ASSPY { get; set; } //  جمع کل هزينه ها	
    }
}
