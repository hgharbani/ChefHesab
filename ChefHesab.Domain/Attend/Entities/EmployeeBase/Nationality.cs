using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class Nationality : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }

        // Reverse navigation

        /// <summary>
        /// Child Emp_Families where [Family].[NationalityId] point to this entity (FK_Family_Nationality)
        /// </summary>
        public virtual ICollection<Family> Families { get; set; } // Family.FK_Family_Nationality

        /// <summary>
        /// Child Employees where [Employee].[NationalityId] point to this entity (FK_Employee_Nationality)
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } // Employee.FK_Employee_Nationality

        public Nationality()
        {
            Employees = new List<Employee>();
            Families = new List<Family>();
        }


    }
}

