using Ksc.HR.Domain.Entities.Rule;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface ICoefficientRepository:IRepository<Coefficient, int>
    {
        IQueryable<Coefficient> GetCoefficientById(int id);
    }
}
