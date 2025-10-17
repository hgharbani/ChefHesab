using KSC.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Chart
{
    public class ViewJobPosition:IEntityBase<int>
    {

        public string Code { get; set; } // Code (length: 50)


        public string Title { get; set; } // Title (length: 500)


        public string StatusTitle { get; set; } // StatusTitle (length: 50)


        public string ColorChart { get; set; } // ColorChart (length: 50)

        public string StructureCode { get; set; } // structureCode


        public string StructureTitle { get; set; } // structureTitle (length: 50)

        public int? WorkingShiftCount { get; set; }
        public int? WorkingDayCount { get; set; }

        public bool IsActive { get; set; } // IsActive
        
        [Key]
        public int Id { get; set; } // Id

        public int? ParentId { get; set; }
        public virtual ICollection<ViewJobPosition> Childrens { get; set; } // JobPosition.FK_JobPosition_JobPosition
        public virtual ViewJobPosition Parent { get; set; } // JobPosition.FK_JobPosition_JobPosition
    }
}
