using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Ksc.HR.Domain.Repositories.WorkShift;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class ViewShiftBoardRepository : EfRepository<ViewShiftBoard>, IViewShiftBoardRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewShiftBoardRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<ViewShiftBoard> GetShiftBoard()
        {
            return _kscHrContext.ViewShiftBoard;
        }

    }
}
