using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface ICityRepository : IRepository<City, int>
    {
        IQueryable<City> GetAllData();
        IQueryable<City> GetAllDataWithCityId(int cityId);
        IQueryable<City> GetCityWithProviceCompany();
        IQueryable<City> GetCityWithProviceCompanyValidForMission();
        IQueryable<City> GetDataFromCityForKSCContract();
    }
}
