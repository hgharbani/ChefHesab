using KSC.Common;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisJobPosition;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.ODSViews.ViewOdsJobPositionTree;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewOdsJobPositionTreeService
    {
        
        List<MeritJobViewModel> GetViewOdsJobPositionTree();
    }
}
