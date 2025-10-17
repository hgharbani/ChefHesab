using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class SignatureImageRepository : EfRepository<SignatureImage, int>, ISignatureImageRepository
    {
        private readonly KscHrContext _kscHrContext;
        public SignatureImageRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<SignatureImage> GetActive()
        {
            var query = _kscHrContext.SignatureImages.Where(x => x.IsActive == true).AsNoTracking();

            return query;
        }
        public IQueryable<SignatureImage> GetAllByIds(int[] ids)
        {
            var query = _kscHrContext.SignatureImages.Where(x => ids.Contains(x.Id) && x.IsActive == true).AsNoTracking();

            return query;
        }

    }
}
