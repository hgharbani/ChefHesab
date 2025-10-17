using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisUserDefinition
{
    public class TreeTemplateInputModel
    {
        public int RequestId { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentDomain { get; set; }
    }
}
