using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Transfer
{
    public partial class Transfer_RequestReasonTypeRepository : EfRepository<Transfer_RequestReasonType, int>, ITransfer_RequestReasonTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Transfer_RequestReasonTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Transfer_RequestReasonType> GetRequestReasonTypeByCategoryCode(string categoryCode, int requestTypeId)
        {

            var query = _kscHrContext.Transfer_RequestReasonTypes.AsQueryable()
                .Include(x => x.Transfer_Type)
                .Include(x => x.Transfer_RequestType)
                .Include(x => x.Transfer_RequestReason)
                .ThenInclude(x => x.Transfer_RequestReasonJobCategories)
                .Where(x => x.TransferRequestTypeId == requestTypeId && x.IsActive);
            return query.Where(x => x.Transfer_RequestReason.Transfer_RequestReasonJobCategories.Any(y => y.CodeCategoryJobCategory == categoryCode));
        }

        public IQueryable<Transfer_RequestReasonType> GetRequestReasonTypeByRequestTypeId(int requestTypeId)
        {

            var query = _kscHrContext.Transfer_RequestReasonTypes.AsQueryable().Where(x => x.IsActive).Include(x => x.Transfer_Type).Include(x => x.Transfer_RequestReason).Include(x=>x.Transfer_RequestType)
                .Where(x => x.TransferRequestTypeId == requestTypeId);
            return query;
        }
        public Transfer_RequestReasonType GetRequestReasonTypeByRequestReasonTypeId(int requestReasonTypeId)
        {

            var query = _kscHrContext.Transfer_RequestReasonTypes.Where(x=>x.IsActive).AsQueryable()
                .Include(x => x.Transfer_Type)
                .Include(x => x.Transfer_RequestReason)
                .Include(x=>x.Transfer_RequestType)
                .FirstOrDefault(x => x.Id == requestReasonTypeId);
            return query;
        }

    }
}
