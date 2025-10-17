using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkGroup
{
    public class LatinAlphabetModel
    {
        public List<string> LatinAlphabetList
        {
            get
            {
                return new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            }
        }
    }
}
