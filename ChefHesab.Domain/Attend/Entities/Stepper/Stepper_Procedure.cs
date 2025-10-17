using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using KSC.Domain; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Stepper
{
    public class  Stepper_Procedure : IEntityBase<int>
  {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شناسه والد
        /// </summary>
        public int? ParentId { get; set; } // ParentId

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title (length: 150)
        public string EnglishTitle { get; set; } // EnglishTitle (length: 150)

        /// <summary>
        /// ترتیب اجرا
        /// </summary>
        public int RunSequence { get; set; } // RunSequence

        /// <summary>
        /// آدرس url
        /// </summary>
        public string UrlName { get; set; } // UrlName (length: 50)

        /// <summary>
        ///  برگشت دارد؟
        /// </summary>
        public bool IsBackValid { get; set; } // IsBackValid (length: 50)
        /// <summary>
        /// وضعیت سیستم
        /// </summary>
        public int? SystemSequenceStatusId { get; set; } // SystemSequenceStatusId
        /// <summary>
        /// وضعیت سیستم
        /// </summary>
        public int? SystemSequenceStatusIdBack { get; set; } // SystemSequenceStatusIdBack

        /// <summary>
        /// کاربر ثبت کننده
        /// </summary>
        public string InsertUser { get; set; } // InsertUser (length: 100)

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime? InsertDate { get; set; } // InsertDate

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
        /// فعال بودن؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive
        /// <summary>
        /// نمایش جزییات
        /// </summary>
        public bool? IsShowDetails { get; set; } //IsShowDetails

        public string UrlDetails { get; set; }//UrlDetails
        public int? SortColumn { get; set; }

        public bool? ShowStatus { get; set; } // IsActive

        // Reverse navigation

        /// <summary>
        /// Child Stepper_ProcedureLogs where [ProcedureLog].[ProcedureId] point to this entity (FK_ProcedureLog_Procedure)
        /// </summary>
        public virtual ICollection<Stepper_ProcedureLog> Stepper_ProcedureLogs { get; set; } // ProcedureLog.FK_ProcedureLog_Procedure

        /// <summary>
        /// Child Stepper_ProcedureStatus where [ProcedureStatus].[ProcedureId] point to this entity (FK_ProcedureStatus_Procedure)
        /// </summary>
        public virtual ICollection<Stepper_ProcedureStatus> Stepper_ProcedureStatus { get; set; } // ProcedureStatus.FK_ProcedureStatus_Procedure
                                                                                                  // Foreign keys
                                                                                                  // Reverse navigation

        /// <summary>
        /// Child Stepper_ProcedureAccessLevels where [ProcedureAccessLevel].[ProcedureId] point to this entity (FK_ProcedureAccessLevel_Procedure)
        /// </summary>
        public virtual ICollection<Stepper_ProcedureAccessLevel> Stepper_ProcedureAccessLevels { get; set; } // ProcedureAccessLevel.FK_ProcedureAccessLevel_Procedure
        /// <summary>
        /// Parent SystemSequenceStatu pointed by [Procedure].([SystemSequenceStatusId]) (FK_Procedure_SystemSequenceStatus)
        /// </summary>
        public virtual SystemSequenceStatus SystemSequenceStatus { get; set; } // FK_Procedure_SystemSequenceStatus
        public Stepper_Procedure()
        {
            Stepper_ProcedureAccessLevels = new List<Stepper_ProcedureAccessLevel>();
            Stepper_ProcedureLogs = new List<Stepper_ProcedureLog>();
            Stepper_ProcedureStatus = new List<Stepper_ProcedureStatus>();
        }
    }
}

