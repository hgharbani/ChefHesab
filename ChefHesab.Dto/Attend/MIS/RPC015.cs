using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS
{
    [XmlRoot(ElementName = "RPC015")]
    public class RPC015
    {
        public RPC015() { }

        public string NUM_PRSN_EMPL { get; set; }
        public string DAT_PYM_EPAYO { get; set; }
        public string COUNTER { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("DETAIL")]
        public List<DETAIL> DETAIL { get; set; }

    }
}

public class DETAIL
{
    public string DES_ACN_ACNDF { get; set; }
    public string DES_TYP_ACN { get; set; }
    public string AMN_CRD_EPAYO { get; set; }

    public string AMN_TAX_EPAYO { get; set; }

    public string AMN_NET_EPAYO { get; set; }

    public string DAT_FINAL_PAID_EPAYO { get; set; }
}
