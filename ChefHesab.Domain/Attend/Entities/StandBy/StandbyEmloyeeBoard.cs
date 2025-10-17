using Ksc.HR.Domain.Entities.Workshift;
using KSC.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ksc.HR.Domain.Entities.StandBy;

public class StandbyEmloyeeBoard : IEntityBase<int>
{
    public int Id { get; set; }

    public int StandbyHeaderId { get; set; }

    public int? StandbyEmployeeRoleId { get; set; }

    public byte StandbyTurnId { get; set; }

    public int WorkCalendarId { get; set; }

    public DateTime InsertDate { get; set; }

    public string InsertUser { get; set; }

    public string? InsertAuthenticateUserName { get; set; }

    [ForeignKey("WorkCalendarId")]
    public virtual WorkCalendar WorkCalendar { get; set; }

    [ForeignKey("StandbyEmployeeRoleId")]
    public virtual StandbyEmployeeRole StandbyEmployeeRole { get; set; }

    [ForeignKey("StandbyHeaderId")]
    public virtual StandbyHeader StandbyHeader { get; set; }

    [ForeignKey("StandbyTurnId")]
    public virtual StandbyTurn StandbyTurn { get; set; }

    public ICollection<StandbyEmloyeeBoardLog> StandbyEmloyeeBoardLogs { get; set; }
    /// <summary>
    /// Child StandbyReplacementRequests where [StandbyReplacementRequests].[DestinationStandbyEmployeeBoardId] point to this entity (FK_StandbyReplacementRequests_StandbyEmloyeeBoard1)
    /// </summary>
    public virtual ICollection<StandbyReplacementRequests> StandbyReplacementRequests_DestinationStandbyEmployeeBoardId { get; set; } // StandbyReplacementRequests.FK_StandbyReplacementRequests_StandbyEmloyeeBoard1

    /// <summary>
    /// Child StandbyReplacementRequests where [StandbyReplacementRequests].[SourceStandbyEmployeeBoardId] point to this entity (FK_StandbyReplacementRequests_StandbyEmloyeeBoard)
    /// </summary>
    public virtual ICollection<StandbyReplacementRequests> StandbyReplacementRequests_SourceStandbyEmployeeBoardId { get; set; } // StandbyReplacementRequests.FK_StandbyReplacementRequests_StandbyEmloyeeBoard

}
