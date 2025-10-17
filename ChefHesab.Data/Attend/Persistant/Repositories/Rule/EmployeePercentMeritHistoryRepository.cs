using EFCore.BulkExtensions;
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
    public class EmployeePercentMeritHistoryRepository : EfRepository<EmployeePercentMeritHistory, int>, IEmployeePercentMeritHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeePercentMeritHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<bool> AddBulkAsync(List<EmployeePercentMeritHistory> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                    option.UseTempDB = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public IQueryable<EmployeePercentMeritHistory> EmployeePercentMeritHistoryByYear(int? year)
        {
            var result = _kscHrContext.EmployeePercentMeritHistories.Where(x=>x.Year == year)
               .AsNoTracking().AsQueryable();
            return result;
        }

        public void RemoveEmployeePercentMeritHistoryByYear(int? year)
        {
            var result = _kscHrContext.EmployeePercentMeritHistories.Any(x => x.Year == year);
            if (result==true)
            {
                var data = _kscHrContext.EmployeePercentMeritHistories.Where(a => a.Year == year);
                _kscHrContext.EmployeePercentMeritHistories.RemoveRange(data);
            }
        }

    }
}
