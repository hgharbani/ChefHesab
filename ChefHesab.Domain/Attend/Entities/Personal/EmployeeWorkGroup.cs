using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KSC.Domain;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Entities.WorkFlow;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Domain.Entities.Personal
{
    // EmployeeWorkGroup
    public class EmployeeWorkGroup : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// گروه کاری
        /// </summary>
        public int WorkGroupId { get; set; } // WorkGroupId

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime StartDate { get; set; } // StartDate

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime? EndDate { get; set; } // EndDate
        public int? TransferRequestId { get; set; } // TransferRequestId
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public bool IsActive { get; set; } // IsActive
        public bool WorkTimeChange { get; set; } // WorkTimeChange
        public byte[] RowVersion { get; set; } // RowVersion (length: 8)

        // Foreign keys

        /// <summary>
        /// Parent Employee pointed by [EmployeeWorkGroup].([EmployeeId]) (FK_EmployeeWorkGroup_Employee)
        /// </summary>
        public virtual Employee Employee { get; set; } // FK_EmployeeWorkGroup_Employee

        /// <summary>
        /// Parent Transfer_Request pointed by [EmployeeWorkGroup].([TransferRequestId]) (FK_EmployeeWorkGroup_Request)
        /// </summary>
        public virtual Transfer_Request Transfer_Request { get; set; } // FK_EmployeeWorkGroup_Request

        /// <summary>
        /// Parent WorkGroup pointed by [EmployeeWorkGroup].([WorkGroupId]) (FK_EmployeeWorkGroup_WorkGroup)
        /// </summary>
        public virtual WorkGroup WorkGroup { get; set; } // FK_EmployeeWorkGroup_WorkGroup

        ///// <summary>
        ///// Child Relocations where [Relocation].[DestinationEmployeeWorkGroupId] point to this entity (FK_Relocation_EmployeeWorkGroup1)
        ///// </summary>
        //public virtual ICollection<Relocation> Relocations_DestinationEmployeeWorkGroupId { get; set; } // Relocation.FK_Relocation_EmployeeWorkGroup1

        ///// <summary>
        ///// Child Relocations where [Relocation].[SourceEmployeeWorkGroupId] point to this entity (FK_Relocation_EmployeeWorkGroup)
        ///// </summary>
        //public virtual ICollection<Relocation> Relocations_SourceEmployeeWorkGroupId { get; set; } // Relocation.FK_Relocation_EmployeeWorkGroup

        public EmployeeWorkGroup()
        {
            //Relocations_DestinationEmployeeWorkGroupId = new List<Relocation>();
            //Relocations_SourceEmployeeWorkGroupId = new List<Relocation>();
        }
    }

}
// </auto-generated>
