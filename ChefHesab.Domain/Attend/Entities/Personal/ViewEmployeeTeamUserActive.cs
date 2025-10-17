using KSC.Domain;

namespace Ksc.HR.Domain.Entities.Personal
{
    // ViewEmployeeTeamUserActive
    public class ViewEmployeeTeamUserActive : IEntityBase<int>
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string EmployeeName { get; set; } // EmployeeName (length: 1001)
        public string WindowsUser { get; set; } // WindowsUser (length: 20)
        public decimal? DisplaySecurity { get; set; } // DisplaySecurity
        public bool IsActive { get; set; } // IsActive
        public DateTime TeamStartDate { get; set; } // TeamStartDate
        public DateTime? TeamEndDate { get; set; } // TeamEndDate
        public string Title { get; set; } // Title (length: 500)
        public string Code { get; set; } // Code (length: 50)
        public int Id { get; set; } // Id
    }

}
// </auto-generated>
