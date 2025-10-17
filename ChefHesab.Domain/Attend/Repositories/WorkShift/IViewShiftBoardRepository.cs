using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IViewShiftBoardRepository : IRepository<ViewShiftBoard>
    {
        IQueryable<ViewShiftBoard> GetShiftBoard();
    }
}
