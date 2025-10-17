using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS
{
    [XmlRoot("SYNC_ASSPY")]
    public partial class SYNC_ASSPY
    {
        public SYNC_ASSPY()
        {
            DETAIL_MESSAGES = new List<DETAIL_MESSAGES>();
        }
        //[System.Xml.Serialization.XmlElementAttribute("DAT_PYM_ASSPY")]
        public string DAT_PYM_ASSPY { get; set; }
        public string INDEX { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("DETAIL_MESSAGES")]
        public List<DETAIL_MESSAGES> DETAIL_MESSAGES { get; set; }
    }
    [XmlRoot("DETAIL_MESSAGES")]
    public partial class DETAIL_MESSAGES
    {
        public string MESSAGES_ASSPY { get; set; }
    }
}
