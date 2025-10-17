using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class ProductionEfficiencyRepository : EfRepository<ProductionEfficiency, int>, IProductionEfficiencyRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ProductionEfficiencyRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        //دیتای خام تولید ماهانه به ازای rewardin که سال/ماه دارد
        public IQueryable<ProductionEfficiency> GetProductionEfficiency()//(int yearmonth)
        {
            var query = _kscHrContext.ProductionEfficiencys
                .Include(x => x.RewardInSmcMonthlyProductions)
                .ThenInclude(x => x.RewardIn)
                .Where(x => x.CPercent.HasValue)//<>0
                                                //.Where(x => x..RewardIn.YearMonth == yearmonth)
                .AsNoTracking();

            return query;
        }

        /// <summary>
        /// جدول نمایش ضریب c 
        /// استفاده در فرم مبنا کارانه تولید
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductionEfficiency> GetActiveNotZero()
        {
            var result = _kscHrContext.ProductionEfficiencies
                .Where(x => (x.Active.HasValue && x.Active.Value == true) && (x.CPercent.HasValue && x.CPercent.Value != 0))
                .AsQueryable();
            return result;
        }
    }
}


