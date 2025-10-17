using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class GenderType : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }

        // Reverse navigation

        /// <summary>
        /// Child Employee_Families where [Family].[GenderId] point to this entity (FK_Family_Gender)
        /// </summary>
        public virtual ICollection<Family> Families { get; set; } // Family.FK_Family_Gender
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<RollCallDefinition> RollCallDefinitions { get; set; } // RollCallDefinition.FK_RollCallDefinition_GenderType
        public GenderType()
        {
            Employees = new List<Employee>();
            Families = new List<Family>();
            RollCallDefinitions = new List<RollCallDefinition>();
        }
    }
}
