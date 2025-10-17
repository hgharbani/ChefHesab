using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS
{
    [XmlRoot(ElementName = "RPC")]
    public class RPC
    {
        public RPC()
        {
        }
        [XmlElement(ElementName = "AMN_FULL_PAY_EMPL")]
        public string AMN_FULL_PAY_EMPL { get; set; }
        [XmlElement(ElementName = "DAT_EMPLT_EMPL")]
        public string DAT_EMPLT_EMPL { get; set; }
        [XmlElement(ElementName = "DES_CITY_DOC")]
        public string DES_CITY_DOC { get; set; }
        [XmlElement(ElementName = "EMPLOYEMENT_DESC")]
        public string EMPLOYEMENT_DESC { get; set; }
        [XmlElement(ElementName = "GENDER_DESC")]
        public string GENDER_DESC { get; set; }
        [XmlElement(ElementName = "NAM_FAM_EMPL")]
        public string NAM_FAM_EMPL { get; set; }
        [XmlElement(ElementName = "NAM_PER_EMPL")]
        public string NAM_PER_EMPL { get; set; }
        [XmlElement(ElementName = "NUM_CRT_EMPL")]
        public string NUM_CRT_EMPL { get; set; }
        [XmlElement(ElementName = "NUM_NNAL_EMPL")]
        public string NUM_NNAL_EMPL { get; set; }
        [XmlElement(ElementName = "NUM_PRSN_EMPL")]
        public string NUM_PRSN_EMPL { get; set; }
        [XmlElement(ElementName = "COD_SUBF")]
        public int COD_SUBF { get; set; }
        [XmlElement(ElementName = "DES_SUBF")]
        public string DES_SUBF { get; set; }

    }
}


