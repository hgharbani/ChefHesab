using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface ISignatureImageRepository : IRepository<SignatureImage, int>
    {
        IQueryable<SignatureImage> GetActive();
        IQueryable<SignatureImage> GetAllByIds(int[] ids);
    }
}
