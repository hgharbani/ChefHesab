using KSC.Domain;
using Ksc.Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IGenderRepository : IRepository<GenderType, int>
    {
        IQueryable<GenderType> GetGenderById(int id);
        IQueryable<GenderType> GetGenders();
    }
}
