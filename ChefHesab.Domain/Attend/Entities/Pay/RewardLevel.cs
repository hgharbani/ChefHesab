using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Pay
{
    public class RewardLevel : IEntityBase<int>
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title (length: 500)

        /// <summary>
        /// فعال بودن؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        // Reverse navigation

        /// <summary>
        /// Child Pay_StudentRewardSettings where [StudentRewardSetting].[RewardLevelId] point to this entity (FK_StudentRewardSetting_RewardLevel)
        /// </summary>
        public virtual ICollection<StudentRewardSetting> StudentRewardSettings { get; set; } // StudentRewardSetting.FK_StudentRewardSetting_RewardLevel

       
        public RewardLevel()
        {
            StudentRewardSettings = new List<StudentRewardSetting>();
        }

    }
}
