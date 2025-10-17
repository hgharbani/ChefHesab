using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class EmployeeVacationManagement : IEntityBase<long>
    {
        public long Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? VacationId { get; set; }
        public string ValueDuration { get; set; }
        public double? Duration { get; set; }

        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime? InsertDate { get; set; } // InsertDate

        /// <summary>
        /// کاربر درج کننده
        /// </summary>
        public string InsertUser { get; set; } // InsertUser (length: 50)

        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public DateTime? UpdateDate { get; set; } // UpdateDate

        /// <summary>
        /// کاربر ویرایش کننده
        /// </summary>
        public string UpdateUser { get; set; } // UpdateUser (length: 50)

        /// <summary>
        /// علت تغییر
        /// </summary>
        public string Remark { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Vacation Vacation { get; set; }

        public virtual ICollection<EmployeeVacationManagementLog> EmployeeVacationManagementLogs { get; set; }
    }
    public class EmployeeSurplusUpdateVacationModel
    {
        public string FullName { get; set; }
        public int EmployeeNumber { get; set; }

        /// <summary>
        /// مانده مرخصی سال قبل
        /// </summary>
        public string LastYearVacationRemaining { get; set; } // LastYearVacationRemaining
        public string SurPlusVacation { get; set; } // LastYearVacationRemaining
        /// <summary>
        /// استفاده شده سال جاری
        /// </summary>
        public string UsedCurrentYear { get; set; }

        public string CurrentMonthMerit { get; set; }

        public string UpdatedMonthMerit { get; set; }
        public string UpdatedLastYearVacationRemaining { get; set; }
        public string UpdatedUsedCurentYear { get; set; }
        public int SumDaySold { get; set; }
    }
    public class EmployeeDiffcurrentYearMeritModel
    {
        public string FullName { get; set; }
        public int EmployeeNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string OldMonthmerit { get; set; } // 
        public string NewMonthMerit { get; set; } // 

        /// <summary>
        ///
        /// </summary>
        public string OldYearmerit { get; set; } // 
        public string NewYearMerit { get; set; } // 


        public string OldRemainingMerit { get; set; }
        public string NewRemainingMerit { get; set; }
        public string CurrentYearMerit { get; set; }
        public string DiffCurrentMonthMerit { get; set; }
    }
}

