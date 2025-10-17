using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallConcept
{
    public class EnumRollCallConcept : Enumeration
    {
        /// <summary>
        /// عدم حضور 
        /// </summary>
        public static readonly EnumRollCallConcept Absence = new EnumRollCallConcept(1, "عدم حضور", null);
        /// <summary>
        /// حضور 
        /// </summary>
        public static readonly EnumRollCallConcept Attend = new EnumRollCallConcept(2, "حضور", null);
        /// <summary>
        /// اضافه کار 
        /// </summary>
        public static readonly EnumRollCallConcept OverTime = new EnumRollCallConcept(3, "اضافه کار", null);

        public EnumRollCallConcept(int id, string name, string group) : base(id, name, group)
        {
        }
    }

}
