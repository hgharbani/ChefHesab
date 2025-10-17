using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("RPC_PER")]
public partial class RPC_PER
{
    public RPC_PER()
    {
        DETAIL_PER = new List<DETAIL_PER>();
    }
    public string NUM_PRSN { get; set; }
    public string NAM_FAM { get; set; }
    public string OPERATION { get; set; }
    public string INDEX { get; set; }
    public bool IsError { get; set; }
    public string MsgError { get; set; }

    [System.Xml.Serialization.XmlElementAttribute("DETAIL_PER")]
    public List<DETAIL_PER> DETAIL_PER { get; set; }
}

[XmlRoot("DETAIL_PER")]
public partial class DETAIL_PER
{
    public string NUM_PRSN_EMPL { get; set; }
    public string NAM_PER_EMPL { get; set; }
    public string NAM_FAM_EMPL { get; set; }
    public string COD_CC_CCRRX { get; set; }
    public string COD_STA_PYM_EMPL { get; set; }
    public string DES_STA_PYM_EMPL { get; set; }
}