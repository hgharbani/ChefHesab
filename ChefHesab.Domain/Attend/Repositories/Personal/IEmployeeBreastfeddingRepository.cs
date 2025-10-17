using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
  namespace Ksc.HR.Domain.Repositories
  {
    public interface IEmployeeBreastfeddingRepository : IRepository<EmployeeBreastfedding, int>
    {
        void ChangeDeActiveEmployeeBreastfeddingsById(int id);
    }
}

