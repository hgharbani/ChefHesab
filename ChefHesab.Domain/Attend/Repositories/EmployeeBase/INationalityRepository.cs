using KSC.Domain;
using Ksc.Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface INationalityRepository : IRepository<Nationality, int>
    {
        IQueryable<Nationality> GetNationalityById(int id);
        IQueryable<Nationality> GetNationalities();
        IQueryable<Nationality> GetDataFromNationalityForKSCContract();
    }
}
