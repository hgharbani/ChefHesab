using Ksc.HR.Domain.Entities;
using KSC.Domain;
namespace Ksc.HR.Domain.Repositories
{
    public interface IEducationCategoryRepository : IRepository<EducationCategory, int>
    {
        IQueryable<EducationCategory> GetDataFromEducationCategoryForKSCContract();
    }
}

