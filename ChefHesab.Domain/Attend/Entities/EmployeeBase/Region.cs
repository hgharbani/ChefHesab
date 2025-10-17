using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class Region : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } // Employee.FK_Employee_BloodType

        public Region()
        {
            Employees = new List<Employee>();
        }
    }
}

