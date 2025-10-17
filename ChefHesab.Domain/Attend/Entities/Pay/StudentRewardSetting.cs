using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Pay
{
    public class StudentRewardSetting : IEntityBase<int>
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// سال
        /// </summary>
        public int? Year { get; set; } // Year
        public int? RewardTypeId { get; set; } // RewardTypeId

        /// <summary>
        /// مقطع تحصیلی
        /// </summary>
        public int? RewardLevelId { get; set; } // RewardLevelId
        /// <summary>
        /// مبلغ واحد سال
        /// </summary>
        public int? KUnitSettingId { get; set; } // KUnitSettingId

        public string Title { get; set; } // Title (length: 500)

        /// <summary>
        /// حداقل رتبه
        /// </summary>
        public int? MinRank { get; set; } // MinRank

        /// <summary>
        /// حداکثر رتبه
        /// </summary>
        public int? MaxRank { get; set; } // MaxRank

        /// <summary>
        /// حداقل معدل
        /// </summary>
        public double? MinMean { get; set; } // MinMean

        /// <summary>
        /// حداکثر معدل
        /// </summary>
        public double? MaxMean { get; set; } // MaxMean
        public long? Amount { get; set; } // Amount

        /// <summary>
        /// فعال بودن؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate

        /// <summary>
        /// ضریب k
        /// </summary>
        public int? KPercent { get; set; } // KPercent
        public int? MisCodeDgreeStwrbs { get; set; } // MISCodeDgreeStwrbs

        // Foreign keys

        /// <summary>
        /// Parent Pay_RewardLevel pointed by [StudentRewardSetting].([RewardLevelId]) (FK_StudentReward_RewardLevel)
        /// </summary>
        public virtual RewardLevel RewardLevel { get; set; } // FK_StudentReward_RewardLevel

        /// <summary>
        /// Parent Pay_RewardType pointed by [StudentRewardSetting].([RewardTypeId]) (FK_StudentRewardSetting_RewardType)
        /// </summary>
        public virtual RewardType RewardType { get; set; } // FK_StudentRewardSetting_RewardType
        public virtual KUnitSetting KUnitSetting { get; set; } // FK_KUnitSetting
        public virtual ICollection<StudentRewardRequest> StudentRewardRequests { get; set; } // StudentRewardRequest.FK_StudentRewardRequest_Family
        public StudentRewardSetting()
        {
            StudentRewardRequests = new List<StudentRewardRequest>();
        }

    }
}
