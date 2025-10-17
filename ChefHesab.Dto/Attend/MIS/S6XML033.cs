using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS
{
    [XmlRoot("SYNC_VACATION")]
    public partial class SYNC_VACATION
    {
        public string DAT_VACATION_ASSPY { get; set; }
        public string INDEX { get; set; }
    }
}
