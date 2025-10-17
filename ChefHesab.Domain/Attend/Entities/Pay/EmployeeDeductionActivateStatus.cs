using KSC.Domain;

namespace Ksc.HR.Domain.Entities.Pay;

public class EmployeeDeductionActivateStatus : IEntityBase<int>
{
    public int Id { get; set; }

    public int YearMonth { get; set; }

    public bool IsActive { get; set; }

    public DateTime InsertDate { get; set; }

    public string InsertUser { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdateUser { get; set; }
}
