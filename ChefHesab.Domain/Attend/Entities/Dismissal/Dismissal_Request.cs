using Ksc.HR.Domain.Entities.WorkFlow;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Dismissal
{
    public class Dismissal_Request : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شناسه گردش کار
        /// </summary>
        public int WfRequestId { get; set; } // WFRequestId

        /// <summary>
        /// شناسه وضعیت گردش کار
        /// </summary>
        public int DismissalStatusId { get; set; } // DismissalStatusId

        /// <summary>
        /// تاریخ ترک کار
        /// </summary>
        public DateTime DismissalDate { get; set; } // DismissalDate

        /// <summary>
        /// منازل سازمانی دارد؟
        /// </summary>
        public bool HasOrganizationHome { get; set; } // HasOrganizationHome

        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime? InsertDate { get; set; } // InsertDate

        /// <summary>
        /// کاربر ثبت کننده
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
        /// فعال؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive

        // Foreign keys

        /// <summary>
        /// Parent Dismissal_Status pointed by [Request].([DismissalStatusId]) (FK_Request_Status)
        /// </summary>
        public virtual Dismissal_Status Dismissal_Status { get; set; } // FK_Request_Status

        /// <summary>
        /// Parent WF_Request pointed by [Request].([WfRequestId]) (FK_Request_Request)
        /// </summary>
        public virtual WF_Request WF_Request { get; set; } // FK_Request_Request
    }

}
