using Ksc.HR.Domain.Entities.StandBy;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.StandBy;

public interface IStandbyHeaderRepository : IRepository<StandbyHeader, int>
{
    StandbyHeader GetByYearMonthIncluded(int yearMonth);
}
