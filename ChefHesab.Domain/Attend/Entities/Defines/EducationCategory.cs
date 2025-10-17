using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities
{
    public class EducationCategory : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IsActive { get; set; }
        public double? Score { get; set; }  
        public int? LevelNumber { get; set; }


        public virtual ICollection<Education> Education { get; set; }
        public virtual ICollection<Chart_JobCategoryEducation> Chart_JobCategoryEducations { get; set; }
    }
}

