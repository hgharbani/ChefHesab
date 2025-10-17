using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    // EmployeeVacationSold
    /// <summary>
    /// بازخرید مرخصی پرسنل
    /// </summary>
    public class EmployeeVacationSold : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// پرسنل
        /// </summary>
        public int EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// مدت بازخرید
        /// </summary>
        public int DaysSold { get; set; } // DaysSold

        /// <summary>
        /// مبلغ بازخرید
        /// </summary>
        public decimal PricePerDay { get; set; } // PricePerDay

        /// <summary>
        /// مبلغ بازخرید
        /// </summary>
        public decimal PriceSold { get; set; } // PriceSold

        /// <summary>
        /// سال ماه
        /// </summary>
        public int YearMonth { get; set; } // YearMonth
        public string RemoteIpAddress { get; set; } // RemoteIpAddress (length: 50)
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string AuthenticateUserName { get; set; } // AuthenticateUserName (length: 50)

        public string RemainingLastYear { get; set; }
        public double? RemainingLastYearDuration { get; set; }

        public string RemainingCurrentYear { get; set; }
        public double? RemainingCurrentYearDuration { get; set; }

        // Foreign keys

        /// <summary>
        /// Parent Employee pointed by [EmployeeVacationSold].([EmployeeId]) (FK_EmployeeVacationSold_Employee)
        /// </summary>
        public virtual Employee Employee { get; set; } // FK_EmployeeVacationSold_Employee
    }

}
// </auto-generated>
