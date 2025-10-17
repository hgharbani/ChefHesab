using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Dismissal
{
    // Status
    public class Dismissal_Status : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title (length: 200)
        public string Code { get; set; } // Code (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive

        // Reverse navigation

        /// <summary>
        /// Child Dismissal_Requests where [Request].[DismissalStatusId] point to this entity (FK_Request_Status)
        /// </summary>
        public virtual ICollection<Dismissal_Request> Dismissal_Requests { get; set; } // Request.FK_Request_Status

        /// <summary>
        /// Child Employees where [Employee].[DismissalStatusId] point to this entity (FK_Employee_Status)
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } // Employee.FK_Employee_Status

        public Dismissal_Status()
        {
            Employees = new List<Employee>();
            Dismissal_Requests = new List<Dismissal_Request>();
        }
    }

}
