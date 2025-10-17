using Ksc.HR.Domain.Entities.WorkFlow;
using KSC.Domain;

namespace Ksc.HR.Domain.Entities.StandBy;

public class StandbyHeader : IEntityBase<int>
{
    public int Id { get; set; } // Id (Primary key)
    public int? WfRequestId { get; set; } // WfRequestId
    public int YearMonth { get; set; } // YearMonth
    public DateTime InsertDate { get; set; } // InsertDate
    public string InsertUser { get; set; } // InsertUser (length: 50)
    public string InsertAuthenticateUserName { get; set; } // InsertAuthenticateUserName (length: 50)

    // Reverse navigation

    /// <summary>
    /// Child StandbyEmloyeeBoards where [StandbyEmloyeeBoard].[StandbyHeaderId] point to this entity (FK_StandbyEmloyeeBoard_StandbyHeader)
    /// </summary>
    public virtual ICollection<StandbyEmloyeeBoard> StandbyEmloyeeBoards { get; set; } // StandbyEmloyeeBoard.FK_StandbyEmloyeeBoard_StandbyHeader

    // Foreign keys

    /// <summary>
    /// Parent WF_Request pointed by [StandbyHeader].([WfRequestId]) (FK_StandbyHeader_Request)
    /// </summary>
    public virtual WF_Request WF_Request { get; set; } // FK_StandbyHeader_Request

    public StandbyHeader()
    {
        StandbyEmloyeeBoards = new List<StandbyEmloyeeBoard>();
    }
}
