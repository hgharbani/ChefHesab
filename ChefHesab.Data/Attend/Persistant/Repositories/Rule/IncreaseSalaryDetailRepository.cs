using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Common;
using Ksc.HR.DTO.Rule.EmployeePercentMeritHistory;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class IncreaseSalaryDetailRepository : EfRepository<IncreaseSalaryDetail, int>, IIncreaseSalaryDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
     
        public IncreaseSalaryDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<IncreaseSalaryDetail> GetIncreaseSalaryDetailByRelated(int? year)
        {
            var query = _kscHrContext.IncreaseSalaryDetails
                .Include(x => x.Rule_IncreaseSalaryHeader)
                .Where(a=>a.Rule_IncreaseSalaryHeader.Year== year  
                       && a.Rule_IncreaseSalaryHeader.IsFinal==false )
                ;
            return query;
        }

        public IQueryable<IncreaseSalaryDetail> GetIncreaseSalaryDetailByHeader(int headerId)
        {
            var query = _kscHrContext.IncreaseSalaryDetails.Where(x => x.IncreasedSalaryHeaderId == headerId);
                

            return query;
        }

        public void UpdateRange(List<IncreaseSalaryDetail> ncreaseSalary)
        {
            _kscHrContext.IncreaseSalaryDetails.UpdateRange(ncreaseSalary);
        }

    }
}
