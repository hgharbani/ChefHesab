using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class CategoryCoefficient : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public decimal? Score { get; set; } // Score
        public bool? Active { get; set; } // Active

        public ICollection<Chart_JobPosition> Chart_JobPositions { get; set; }
        public virtual ICollection<JobGroup> Chart_JobGroups { get; set; } // JobGroup.FK_JobGroup_CategoryCoefficient
        /// <summary>
        /// Child Employees where [Employee].[CategoryCoefficientId] point to this entity (FK_Employee_CategoryCoefficient)
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } // Employee.FK_Employee_CategoryCoefficient

        public CategoryCoefficient()
        {
            Employees = new List<Employee>();

            Chart_JobPositions = new List<Chart_JobPosition>();
            Chart_JobGroups = new List<JobGroup>();
        }
    }
}

