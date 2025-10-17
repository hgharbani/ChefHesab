using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    // JobCategoryDefinitionGuide
    /// <summary>
    /// جدول راهنمای تبدیل شدن برای درخواست جابجایی
    /// </summary>
    public class JobCategoryDefinitionGuide : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int JobCategoryDefinationId { get; set; } // JobCategoryDefinationId
        public int ConvertedJobCategoryDefinationId { get; set; } // ConvertedJobCategoryDefinationId
        public bool IsActive { get; set; } // IsActive

        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime InsertDate { get; set; } // InsertDate

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

        // Foreign keys

        /// <summary>
        /// Parent Chart_JobCategoryDefination pointed by [JobCategoryDefinitionGuide].([ConvertedJobCategoryDefinationId]) (FK_JobCategoryDefinitionGuide_JobCategoryDefination1)
        /// </summary>
        public virtual Chart_JobCategoryDefination ConvertedJobCategoryDefination { get; set; } // FK_JobCategoryDefinitionGuide_JobCategoryDefination1

        /// <summary>
        /// Parent Chart_JobCategoryDefination pointed by [JobCategoryDefinitionGuide].([JobCategoryDefinationId]) (FK_JobCategoryDefinitionGuide_JobCategoryDefination)
        /// </summary>
        public virtual Chart_JobCategoryDefination JobCategoryDefination { get; set; } // FK_JobCategoryDefinitionGuide_JobCategoryDefination
    }
}

