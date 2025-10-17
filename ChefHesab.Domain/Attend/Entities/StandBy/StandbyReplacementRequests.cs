using Ksc.HR.Domain.Entities.WorkFlow;
using KSC.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ksc.HR.Domain.Entities.StandBy;

public class StandbyReplacementRequests : IEntityBase<int>
{
    public int Id { get; set; } // Id (Primary key)
    public int SourceStandbyEmployeeBoardId { get; set; } // SourceStandbyEmployeeBoardId
    public int DestinationStandbyEmployeeBoardId { get; set; } // DestinationStandbyEmployeeBoardId
    public byte ReplacementType { get; set; } // ReplacementType
    public int? WfRequestId { get; set; } // WFRequestId
    public byte Status { get; set; } // Status
    public DateTime RequestDate { get; set; } // RequestDate

    // Foreign keys

    /// <summary>
    /// Parent StandbyEmloyeeBoard pointed by [StandbyReplacementRequests].([DestinationStandbyEmployeeBoardId]) (FK_StandbyReplacementRequests_StandbyEmloyeeBoard1)
    /// </summary>
    public virtual StandbyEmloyeeBoard DestinationStandbyEmployeeBoard { get; set; } // FK_StandbyReplacementRequests_StandbyEmloyeeBoard1

    /// <summary>
    /// Parent StandbyEmloyeeBoard pointed by [StandbyReplacementRequests].([SourceStandbyEmployeeBoardId]) (FK_StandbyReplacementRequests_StandbyEmloyeeBoard)
    /// </summary>
    public virtual StandbyEmloyeeBoard SourceStandbyEmployeeBoard { get; set; } // FK_StandbyReplacementRequests_StandbyEmloyeeBoard

    /// <summary>
    /// Parent WF_Request pointed by [StandbyReplacementRequests].([WfRequestId]) (FK_StandbyReplacementRequests_Request)
    /// </summary>
    public virtual WF_Request WF_Request { get; set; } // FK_StandbyReplacementRequests_Request
}
