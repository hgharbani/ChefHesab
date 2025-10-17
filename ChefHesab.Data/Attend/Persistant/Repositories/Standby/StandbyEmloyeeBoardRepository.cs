using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.StandBy;
using Ksc.HR.Domain.Repositories.StandBy;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Standby;

public class StandbyEmloyeeBoardRepository : EfRepository<StandbyEmloyeeBoard, int>, IStandbyEmloyeeBoardRepository
{
    private readonly KscHrContext _context;

    public StandbyEmloyeeBoardRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }

    public StandbyEmloyeeBoard GetByIdIncludeStandbyEmployeeRole(int id)
    {
        return _context.StandbyEmloyeeBoard.Include(x => x.StandbyEmployeeRole).FirstOrDefault(x => x.Id == id);
    }

    public StandbyEmloyeeBoard GetByIdIncludeStandbyEmployeeRoleAndEmployee(int id)
    {
        return _context
            .StandbyEmloyeeBoard
            .Include(x => x.StandbyEmployeeRole)
            .ThenInclude(x => x.Employee)
            .FirstOrDefault(x => x.Id == id);
    }
    
}
