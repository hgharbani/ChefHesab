using Ksc.HR.Domain.Entities.Emp;

using KSC.Domain;


namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeePictureRepository : IRepository<EmployeePicture, int>
    {
        //IQueryable<EmployeePicture> GetAllEmployeePictureByRelated(int? id);
    }
}
