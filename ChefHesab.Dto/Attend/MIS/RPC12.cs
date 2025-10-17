using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS
{
    /// <summary>
    /// SUBPROGRAM : S6XML118 -- FUNCTION : UPDATE-ROUTINE
    /// </summary>
    [XmlRoot(ElementName = "RPC12")]
    public class RPC12
    {
        public RPC12()
        {
            TEAM_LIST = new List<TEAM_LIST>();
               
                }
        public string FUNCTION { get; set; }
        /// <summary>
        /// شماره پرسنلي
        /// </summary>
        public int NUM_PERSONAL { get; set; } // int

        /// <summary>
        /// ارور
        /// </summary>
        public string WEB_ERROR { get; set; }

        public string MIS_USER { get; set; }

        /// <summary>
        /// //INDEX_TEAM(INT)     /* COUNTER آرايه پاييني
        /// </summary>
        public int INDEX_TEAM { get; set; } // int

        [System.Xml.Serialization.XmlElementAttribute("TEAM_LIST")]
        public List<TEAM_LIST> TEAM_LIST { get; set; } 
    }

    public class TEAM_LIST
    {
        /// <summary>
        /// شماره تيم
        /// </summary>
        public int NUM_TEAM_WORK { get; set; }// int 

        /// <summary>
        /// اصلي و فرعي 1,2
        /// </summary>
        public int FLG_CONF_DISP { get; set; }// int

    }



}
