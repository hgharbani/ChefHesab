using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IMaritalStatusRepository : IRepository<MaritalStatus, int>
    {
        IQueryable<MaritalStatus> GetMaritalStatusById(int id);
        IQueryable<MaritalStatus> GetMaritalStatus();
        IQueryable<MaritalStatus> GetDataFromMaritalStatusForKSCContract();
    }
}
