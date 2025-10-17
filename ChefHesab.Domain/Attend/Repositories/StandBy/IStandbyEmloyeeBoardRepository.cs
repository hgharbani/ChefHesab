using Ksc.HR.Domain.Entities.StandBy;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.StandBy;

public interface IStandbyEmloyeeBoardRepository : IRepository<StandbyEmloyeeBoard, int>
{
    StandbyEmloyeeBoard GetByIdIncludeStandbyEmployeeRole(int id);
    StandbyEmloyeeBoard GetByIdIncludeStandbyEmployeeRoleAndEmployee(int id);
}
