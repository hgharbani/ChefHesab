using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using KSC.Domain;
using EFCore.BulkExtensions;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class EmployeeHaveTravelRepository : EfRepository<EmployeeHaveTravel, long>, IEmployeeHaveTravelRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeHaveTravelRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public async Task<bool> AddBulkAsync(EmployeeHaveTravel entity)
        {
            try
            {
                List<EmployeeHaveTravel> list = new List<EmployeeHaveTravel>();
                list.Add(entity);
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });
           


                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> AddBulkAsync(List<EmployeeHaveTravel> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = false;
                    option.UseTempDB = true;
                    option.SetOutputIdentity = true;
                    option.CalculateStats = true;
                    option.BatchSize = 4000;
                    option.PreserveInsertOrder = true;
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> UpdateBulkAsync(EmployeeHaveTravel entity)
        {
            try
            {
                List<EmployeeHaveTravel> list = new List<EmployeeHaveTravel>();
                list.Add(entity);
                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });



                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public async Task<bool> SaveBulkAsync()
        {
            try
            {
                await _kscHrContext.BulkSaveChangesAsync();


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateBulkAsync(List<EmployeeVacationManagement> list)
        {
            try
            {

                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = false;
                });
     


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

    }
}

