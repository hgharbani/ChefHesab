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
using Ksc.HR.Share.Model.City;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class CityRepository : EfRepository<City, int>, ICityRepository
    {
        private readonly KscHrContext _kscHrContext;
        public CityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<City> GetAllData()
        {
            return _kscHrContext.Cities.Include(x => x.Province).ThenInclude(x=>x.Country).AsQueryable().AsQueryable();
        } 
        public IQueryable<City> GetAllDataWithCityId(int cityId)
        {
            return _kscHrContext.Cities.Where(x=>x.Id== cityId).Include(x => x.Province).ThenInclude(x=>x.Country).AsQueryable().AsQueryable();
        }

        public IQueryable<City> GetCityWithProviceCompany()
        {
            var query = _kscHrContext.Cities.AsQueryable().Where(x => x.Province.CountryId == EnumCity.Iran.Id).OrderBy(x => x.Title).Include(a => a.Province).ThenInclude(a => a.Country).AsQueryable();
            return query;
        }

        public IQueryable<City> GetCityWithProviceCompanyValidForMission()
        {
            var query = _kscHrContext.Cities.AsQueryable().Where(x => x.Province.CountryId == EnumCity.Iran.Id && x.IsValidForMission).OrderBy(x => x.Title).Include(a => a.Province).ThenInclude(a => a.Country).AsQueryable();
            return query;
        }

        public IQueryable<City> GetDataFromCityForKSCContract()
        {
            return _kscHrContext.Cities
                .IgnoreAutoIncludes()
                .Include(x=>x.Province)
                .ThenInclude(x=>x.Country);
        }
    }
}
