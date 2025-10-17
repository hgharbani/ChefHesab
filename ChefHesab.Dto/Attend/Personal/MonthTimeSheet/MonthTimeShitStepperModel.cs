using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeSheet
{
    public class MonthTimeShitStepperModel
    {
        public int id { get; set; }
        public string label { get; set; }

        public int OrderNo { get; set; }

        public int index { get; set; }

        public bool selected { get; set; }

        public string action { get; set; }

        public bool IsComplate { get; set; }
        public string ShowDetailMessage { get; set; }

        public bool IsShowDetails { get; set; }
        public bool IsRequiredForMonthSheet { get; set; }
        public string FunctionDetails { get; set; }

    }
}
