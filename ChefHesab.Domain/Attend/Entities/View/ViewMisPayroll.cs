using KSC.Domain;

namespace Ksc.HR.Domain.Entities.View;

public class ViewMisPayroll : IEntityBase
{
    public string? EmployeeNumber { get; set; }

    public int? YearMonth { get; set; }

    public long? CompanySaveAmount { get; set; }

    public long? EmployeeSaveAmount { get; set; }
}
