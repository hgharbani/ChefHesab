using Ksc.Hr.Domain.Entities;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IBloodTypeRepository:IRepository<BloodType, int>
    {
        IQueryable<BloodType> GetBloodTypeById(int id);
        IQueryable<BloodType> GetBloodTypes();
    }
}
