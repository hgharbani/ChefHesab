using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities;

using Ksc.HR.Domain.Repositories.Emp;
using EFCore.BulkExtensions;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeVacationHistoryRepository : EfRepository<EmployeeVacationHistory, int>, IEmployeeVacationHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeVacationHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeVacationHistory> GetLatestData(int yearMonth)
        {
            var result = _kscHrContext.EmployeeVacationHistories.Include(x => x.Employee)
                .Where(x => x.YearMonth == yearMonth);
            return result;
        }

        public IQueryable<EmployeeVacationHistory> GetAllRelated()
        {
            return _kscHrContext.EmployeeVacationHistories
                .AsNoTracking()
                .AsQueryable();

        }
        public IQueryable<EmployeeVacationHistory> GetByRelated()
        {
            return _kscHrContext.EmployeeVacationHistories.AsQueryable().Include(a => a.Employee)
                .AsNoTracking();


        }
        public IQueryable<EmployeeVacationHistory> GetByIdRelated(int id)
        {
            return _kscHrContext.EmployeeVacationHistories.Where(x => x.Id == id)
                .AsQueryable();

        }

        public async Task<bool> AddBulkAsync(EmployeeVacationHistory entity)
        {
            try
            {
                List<EmployeeVacationHistory> list = new List<EmployeeVacationHistory>();
                list.Add(entity);
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public async Task<bool> BulkInsertOrUpdateAsync(List<EmployeeVacationHistory> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public async Task<bool> AddBulkAsync(List<EmployeeVacationHistory> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public async Task<bool> BulkDeleteAsync(List<EmployeeVacationHistory> list)
        {
            try
            {
                await _kscHrContext.BulkDeleteAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }


        public async Task<bool> BulkUpdateAsync(EmployeeVacationHistory entity)
        {
            try
            {
                List<EmployeeVacationHistory> list = new List<EmployeeVacationHistory>();
                list.Add(entity);
                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public async Task<bool> BulkUpdateAsync(List<EmployeeVacationHistory> list)
        {
            try
            {
            
                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
