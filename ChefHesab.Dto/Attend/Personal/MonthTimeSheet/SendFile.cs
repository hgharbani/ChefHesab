using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeSheet
{
    public class SendFile
    {
        /// <summary>
        /// per
        /// </summary>
        public string application { get; set; }
        /// <summary>
        /// filename.txt
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// Load L
        /// Dev M
        /// </summary>
        public string enviroment { get; set; }


        public string file { get; set; }

    }
    public class Fileinfo
    {
        /// <summary>
        /// per
        /// </summary>
        public string application { get; set; }
        /// <summary>
        /// filename.txt
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// Load L
        /// Dev M
        /// </summary>
        public string enviroment { get; set; }


        public string file { get; set; }

    }
    public class File
    {
        /// <summary>
        /// filename.txt
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// content
        /// </summary>
        public string file { get; set; }
    }

}
