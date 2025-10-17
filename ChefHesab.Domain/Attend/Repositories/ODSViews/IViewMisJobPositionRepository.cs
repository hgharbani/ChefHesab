using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.ODSViews
{
    public interface IViewMisJobPositionRepository : IRepository<ViewMisJobPosition>
    {
        IEnumerable<ViewMisJobPosition> GetAllActiveJobPosition();
        ViewMisJobPosition GetJobPosition(string JobPositionCode);
    }
}
