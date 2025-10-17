
using Ksc.HR.DTO.WorkShift.OverTimeDefinition;
using Ksc.HR.DTO.WorkShift.TeamWorkCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWork
{
    public class EditTeamWorkModel
    {
        public EditTeamWorkModel()
        {
            AvailableTeamWorkCategory = new List<SearchTeamWorkCategoryModel>();
            AvailableOverTimeDefinition = new List<SearchOverTimeDefinitionModel>();
        }
        public List<SearchTeamWorkCategoryModel> AvailableTeamWorkCategory { get; set; }
        public List<SearchOverTimeDefinitionModel> AvailableOverTimeDefinition { get; set; }

        public int Id { get; set; } // Id (Primary key)

        [DisplayName("عنوان تیم")]
        public string Title { get; set; } // Title (length: 500)

        [DisplayName("شماره تیم")]
        public string Code { get; set; } // Code (length: 50)

        [DisplayName("گروه")]
        public int TeamWorkCategoryId { get; set; } // TeamWorkCategoryId

        [DisplayName("سقف اضافه کاری")]
        public int? OverTimeDefinitionId { get; set; } // OverTimeDefinitionId

        [DisplayName("تاریخ شروع اعتبار")]
        public DateTime? ValidityStartDate { get; set; } // ValidityStartDate


        [DisplayName("تاریخ پایان اعتبار")]
        public DateTime? ValidityEndDate { get; set; } // ValidityEndDate


        [DisplayName("به این تیم کمد تعلق می گیرد؟")]
        public bool HasCommode { get; set; } // HasCommode
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        [DisplayName("فعال؟")]
        public bool IsActive { get; set; } // IsActive
        public string CurrentUserName { get; set; }

    }
}
