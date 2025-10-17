using KSC.Domain;

namespace Ksc.HR.Domain.Entities.StandBy;

public class StandbyEmloyeeBoardLog : IEntityBase<long>
{
    public long Id { get; set; }

    public int StandbyEmloyeeBoardId { get; set; }

    public int StandbyEmployeeRoleId { get; set; }

    public int StandbyHeaderId { get; set; }

    public byte StandbyTurnId { get; set; }

    public int WorkCalendarId { get; set; }

    public DateTime InsertDate { get; set; }

    public string InsertUser { get; set; }

    public string? InsertAuthenticateUserName { get; set; }

    public byte? StatusCode { get; set; }
}
