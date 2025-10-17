using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewOdsJobPositionTree
{
    public class ViewOdsJobPositionTreeModel
    {

        public Guid RowNumber { get; set; } // RowNumber
        public decimal? JobPositionId { get; set; } // JobPositionId
        public string JobPositionCodeParent { get; set; } // JobPositionCodeParent (length: 13)
        public int HasChild { get; set; } // HasChild
        public string DesParentPost { get; set; } // DesParentPost (length: 60)
        public string JobPositionCode { get; set; } // JobPositionCode (length: 13)
        public string DesPosJpos { get; set; } // DES_POS_JPOS (length: 60)
        public decimal? TotNrmPerJpos { get; set; } // TOT_NRM_PER_JPOS
        public decimal? TotShftPerJpos { get; set; } // TOT_SHFT_PER_JPOS
        public decimal? TotSharPerJpos { get; set; } // TOT_SHAR_PER_JPOS
        public decimal? TotSharPerShftJpos { get; set; } // TOT_SHAR_PER_SHFT_JPOS
        public int? Rasmi { get; set; } // RASMI
        public int? Ghardadi { get; set; } // GHARDADI
        public string Study { get; set; } // STUDY (length: 250)
        public decimal? CodLevJpos { get; set; } // COD_LEV_JPOS
        public string DesSubf { get; set; } // DES_SUBF (length: 60)
        public string Managment { get; set; } // managment (length: 60)
        public string Moavenat { get; set; } // moavenat (length: 60)
        public decimal? DatEndJpos { get; set; } // DAT_END_JPOS
        public decimal CodSubf { get; set; } // COD_SUBF
        public decimal? JobStatus { get; set; } // JobStatus
        public string CodJob { get; set; } // COD_JOB (length: 5)
        public decimal Managmentcode { get; set; } // managmentcode
        public decimal Moavenatcode { get; set; } // moavenatcode
        public decimal? CodCcOrglJpos { get; set; } // COD_CC_ORGL_JPOS
        public decimal? CodSubOrgl { get; set; } // COD_SUB_ORGL
        public string CodCatgJbcat { get; set; } // COD_CATG_JBCAT (length: 2)
    }
}
