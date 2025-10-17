using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS
{
    [XmlRoot("RPC_INTERDICT")]
    public partial class RPC_INTERDICT
    {
        public string FUNCTION { get; set; }
        public string INDEX { get; set; }
    }


    [XmlRoot("RPC_DEAD")]
    public partial class RPC_DEAD
    {
        public string FILENAME { get; set; }
        public string FUNCTION { get; set; }
        public string DAT_PAY { get; set; }
        public string COD_PAY { get; set; }
        public string IS_DUPL { get; set; }
        public string IS_COMFIRM { get; set; }
        public string WIN_USER { get; set; }
        public string DES_ERROR { get; set; }
        //public string INDEX { get; set; }

    }
}
