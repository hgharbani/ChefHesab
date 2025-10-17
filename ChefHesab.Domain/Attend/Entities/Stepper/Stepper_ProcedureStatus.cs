using KSC.Domain; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Stepper
{
    public class  Stepper_ProcedureStatus : IEntityBase<long>
  {
        public long Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شناسه فرایند
        /// </summary>
        public int ProcedureId { get; set; } // ProcedureId

        /// <summary>
        /// تاریخ زمانبندی
        /// </summary>
        public int YearMonth { get; set; } // YearMonth
        public DateTime? InsertDate { get; set; } // InsertDate

        /// <summary>
        /// ثبت کننده
        /// </summary>
        public string InsertUser { get; set; } // InsertUser (length: 100)
        /// <summary>
        /// کاربر جانشین
        /// </summary>
        public string InsertAuthenticateUserName { get; set; } // InsertAuthenticateUserName (length: 100)

        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public DateTime? UpdateDate { get; set; } // UpdateDate

        /// <summary>
        /// کاربر ویرایش کننده
        /// </summary>
        public string UpdateUser { get; set; } // UpdateUser (length: 100)

        /// <summary>
        /// کاربر ویرایش کننده جانشین
        /// </summary>
        public string UpdateAuthenticateUserName { get; set; } // UpdateAuthenticateUserName (length: 100)

        /// <summary>
        /// نتیجه
        /// </summary>
        public string Result { get; set; } // Result

        /// <summary>
        /// تعداد
        /// </summary>
        public int? ResultCount { get; set; } // ResultCount

        /// <summary>
        /// انجام شده
        /// </summary>
        public bool IsDone { get; set; } // IsDone

        /// <summary>
        /// فعال بودن؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive
   

        // Foreign keys

        /// <summary>
        /// Parent Stepper_Procedure pointed by [ProcedureStatus].([ProcedureId]) (FK_ProcedureStatus_Procedure)
        /// </summary>
        public virtual Stepper_Procedure Stepper_Procedure { get; set; } // FK_ProcedureStatus_Procedure

    }
}

