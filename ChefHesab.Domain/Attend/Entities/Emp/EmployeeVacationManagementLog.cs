using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class EmployeeVacationManagementLog : IEntityBase<long>
    {
        public long Id { get; set; }
        public long? EmployeeVacationManagementId { get; set; }
        public string VacationTitle { get; set; }
        public string ValueDuration { get; set; }
        public int? Duration { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string Remark { get; set; }
        public virtual EmployeeVacationManagement EmployeeVacationManagement { get; set; }
    }
}

