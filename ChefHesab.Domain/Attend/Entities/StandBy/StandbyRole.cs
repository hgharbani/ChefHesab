using KSC.Domain;

namespace Ksc.HR.Domain.Entities.StandBy;

public class StandbyRole : IEntityBase<byte>
{
    public byte Id { get; set; }

    public string Title { get; set; }

    public byte SortOrder { get; set; }

    public bool IsActive { get; set; }
}
