using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.CompatibleRollCall;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class CompatibleRollCallRepository : EfRepository<CompatibleRollCall, int>, ICompatibleRollCallRepository
    {
        private readonly KscHrContext _kscHrContext;
        public CompatibleRollCallRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<CompatibleRollCall> GetCompatibleRollCallByCompatibleRollCallType(int CompatibleRollCallTypeId)
        {
            return _kscHrContext.CompatibleRollCalls.Where(x => x.CompatibleRollCallType == CompatibleRollCallTypeId);
        }
        public IQueryable<CompatibleRollCallByCompatibleTypeModel> GetCompatibleRollCallByCompatibleTypeAsNoTracking(int compatibleRollCallType)
        {
            var compatibleRollCall = _kscHrContext.CompatibleRollCalls.Where(x => x.IsActive == true && x.CompatibleRollCallType == compatibleRollCallType).AsNoTracking();
            var rollCallDefinitions = _kscHrContext.RollCallDefinitions.AsNoTracking();
            var query = from comp in compatibleRollCall
                        join roll in rollCallDefinitions
                        on comp.CompatibleRollCallId equals roll.Id
                        select new CompatibleRollCallByCompatibleTypeModel()
                        {
                            RollCallDefinitionId = comp.RollCallDefinitionId,
                            CompatibleRollCallDefinitionId = roll.Id,
                            CompatibleRollCallTitle = roll.Title,
                            CompatibleRollCallCode = roll.Code,
                            CompatibleRollCallConceptId = roll.RollCallConceptId,
                            DayNumber = comp.DayNumber,
                            WorkDayTypeId = comp.WorkDayTypeId,
                            IsValidSingleDelete= roll.IsValidSingleDelete,
                        };
            return query;
        }
    }
}
