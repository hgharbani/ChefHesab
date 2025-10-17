using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.ODSViews
{
    public class CallingRpc:IEntityBase
    {

        public bool IsError { get; set; }
        public string MsgError { get; set; }

        private string nUM_PRSN_EMPLField;

        private string dAT_CALLINGField;

        private string uSER_FK_JPOSField;

        private string fUNCTIONField;

        private DETAIL dETAILField;

        /// <remarks/>
        public string NUM_PRSN_EMPL
        {
            get
            {
                return this.nUM_PRSN_EMPLField;
            }
            set
            {
                this.nUM_PRSN_EMPLField = value;
            }
        }

        /// <remarks/>
        public string DAT_CALLING
        {
            get
            {
                return this.dAT_CALLINGField;
            }
            set
            {
                this.dAT_CALLINGField = value;
            }
        }

        /// <remarks/>
        public string USER_FK_JPOS
        {
            get
            {
                return this.uSER_FK_JPOSField;
            }
            set
            {
                this.uSER_FK_JPOSField = value;
            }
        }

        /// <remarks/>
        public string FUNCTION
        {
            get
            {
                return this.fUNCTIONField;
            }
            set
            {
                this.fUNCTIONField = value;
            }
        }

        /// <remarks/>
        public DETAIL DETAIL
        {
            get
            {
                return this.dETAILField;
            }
            set
            {
                this.dETAILField = value;
            }
        }
    }

    /// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="S6PSA025.dtd")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace="S6PSA025.dtd", IsNullable=false)]
    public partial class DETAIL
    {

        private string nAM_PER_EMPLField;

        private string nAM_FAM_EMPLField;

        private string cOD_CC_CCRRXField;

        private string cOD_SUBFField;

        private string cOD_TYP_WRK_EMPLField;

        private string dES_SUBFField;

        private string fLG_ONCAL_JPOSField;

        private string dES_POS_JPOSField;

        private string cOD_JOB_ORGLField;

        private string nUM_TEAM_EMPLField;

        private string dES_TEAM_EMTEMField;

        private string cOD_WRK_GRP_EMPLField;

        private string fK_JPOS_EMPL;
        private string dES_EMPLT;

        private string dAT_STA_PYM_EMPL;

        private string dAT_EMPLT_EMPL;

        private string cOD_CAT_JOB;
        private string cOD_JOB_STA_JPOS;

        private string dES_STA_PYM_EMPL;
        private string cOD_STA_PYM_EMPL;
        private string dAT_DSMSL_EMPL;

        private string tOT_SHFT_PER_JPOS;
        private string tOT_NRM_PER_JPOS;
        private string nUM_PER_CNTRC_JPOS;


        /// <remarks/>
        public string NAM_PER_EMPL
        {
            get
            {
                return this.nAM_PER_EMPLField;
            }
            set
            {
                this.nAM_PER_EMPLField = value;
            }
        }

        /// <remarks/>
        public string NAM_FAM_EMPL
        {
            get
            {
                return this.nAM_FAM_EMPLField;
            }
            set
            {
                this.nAM_FAM_EMPLField = value;
            }
        }

        /// <remarks/>
        public string COD_CC_CCRRX
        {
            get
            {
                return this.cOD_CC_CCRRXField;
            }
            set
            {
                this.cOD_CC_CCRRXField = value;
            }
        }

        /// <remarks/>
        public string COD_SUBF
        {
            get
            {
                return this.cOD_SUBFField;
            }
            set
            {
                this.cOD_SUBFField = value;
            }
        }

        /// <remarks/>
        public string COD_TYP_WRK_EMPL
        {
            get
            {
                return this.cOD_TYP_WRK_EMPLField;
            }
            set
            {
                this.cOD_TYP_WRK_EMPLField = value;
            }
        }

        /// <remarks/>
        public string DES_SUBF
        {
            get
            {
                return this.dES_SUBFField;
            }
            set
            {
                this.dES_SUBFField = value;
            }
        }

        /// <remarks/>
        public string FLG_ONCAL_JPOS
        {
            get
            {
                return this.fLG_ONCAL_JPOSField;
            }
            set
            {
                this.fLG_ONCAL_JPOSField = value;
            }
        }

        /// <remarks/>
        public string DES_POS_JPOS
        {
            get
            {
                return this.dES_POS_JPOSField;
            }
            set
            {
                this.dES_POS_JPOSField = value;
            }
        }

        /// <remarks/>
        public string COD_JOB_ORGL
        {
            get
            {
                return this.cOD_JOB_ORGLField;
            }
            set
            {
                this.cOD_JOB_ORGLField = value;
            }
        }

        /// <remarks/>
        public string NUM_TEAM_EMPL
        {
            get
            {
                return this.nUM_TEAM_EMPLField;
            }
            set
            {
                this.nUM_TEAM_EMPLField = value;
            }
        }

        /// <remarks/>
        public string DES_TEAM_EMTEM
        {
            get
            {
                return this.dES_TEAM_EMTEMField;
            }
            set
            {
                this.dES_TEAM_EMTEMField = value;
            }
        }

        /// <remarks/>
        public string COD_WRK_GRP_EMPL
        {
            get
            {
                return this.cOD_WRK_GRP_EMPLField;
            }
            set
            {
                this.cOD_WRK_GRP_EMPLField = value;
            }
        }

        public string FK_JPOS_EMPL
        {
            get
            {
                return this.fK_JPOS_EMPL;
            }
            set
            {
                this.fK_JPOS_EMPL = value;
            }
        }

        public string DES_EMPLT
        {
            get
            {
                return this.dES_EMPLT;
            }
            set
            {
                this.dES_EMPLT = value;
            }
        }

        public string DAT_STA_PYM_EMPL
        {
            get
            {
                return this.dAT_STA_PYM_EMPL;
            }
            set
            {
                this.dAT_STA_PYM_EMPL = value;
            }
        }

        public string DAT_EMPLT_EMPL
        {
            get
            {
                return this.dAT_EMPLT_EMPL;
            }
            set
            {
                this.dAT_EMPLT_EMPL = value;
            }
        }

        public string COD_CAT_JOB
        {
            get
            {
                return this.cOD_CAT_JOB;
            }
            set
            {
                this.cOD_CAT_JOB = value;
            }
        }

        public string COD_JOB_STA_JPOS
        {
            get
            {
                return this.cOD_JOB_STA_JPOS;
            }
            set
            {
                this.cOD_JOB_STA_JPOS = value;
            }
        }

        public string DES_STA_PYM_EMPL
        {
            get
            {
                return this.dES_STA_PYM_EMPL;
            }
            set
            {
                this.dES_STA_PYM_EMPL = value;
            }
        }

        public string COD_STA_PYM_EMPL
        {
            get
            {
                return this.cOD_STA_PYM_EMPL;
            }
            set
            {
                this.cOD_STA_PYM_EMPL = value;
            }
        }
        public string DAT_DSMSL_EMPL
        {
            get
            {
                return this.dAT_DSMSL_EMPL;
            }
            set
            {
                this.dAT_DSMSL_EMPL = value;
            }
        }
        public string TOT_SHFT_PER_JPOS
        {
            get
            {
                return this.tOT_SHFT_PER_JPOS;
            }
            set
            {
                this.tOT_SHFT_PER_JPOS = value;
            }
        }

        public string TOT_NRM_PER_JPOS
        {
            get
            {
                return this.tOT_NRM_PER_JPOS;
            }
            set
            {
                this.tOT_NRM_PER_JPOS = value;
            }
        }
        public string NUM_PER_CNTRC_JPOS
        {
            get
            {
                return this.nUM_PER_CNTRC_JPOS;
            }
            set
            {
                this.nUM_PER_CNTRC_JPOS = value;
            }
        }
        //private string TOT_SHFT_PER_JPOS;
        //private string TOT_NRM_PER_JPOS;
        //private string TOT_PER_CNTRC_JPOS;


    }
}
