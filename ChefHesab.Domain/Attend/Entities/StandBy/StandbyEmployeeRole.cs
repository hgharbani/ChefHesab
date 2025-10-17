using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ksc.HR.Domain.Entities.StandBy;

public class StandbyEmployeeRole : IEntityBase<int>
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public byte StandbyRoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime InsertDate { get; set; }

    public string InsertUser { get; set; }

    public DateTime? DeactiveDate { get; set; }

    public string? DeactiveUser { get; set; }

    public string? InsertAuthenticateUserName { get; set; }

    public string? DeactiveAuthenticateUserName { get; set; }

    public ICollection<StandbyEmloyeeBoard> StandbyEmployeeBoards { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee Employee { get; set; }

    [ForeignKey("StandbyRoleId")]
    public virtual StandbyRole StandbyRole { get; set; }

}
