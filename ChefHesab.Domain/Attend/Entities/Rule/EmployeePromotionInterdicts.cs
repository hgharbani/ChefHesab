using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Rule
{
    public class EmployeePromotionInterdicts : IEntityBase<int>
    {
        public int Id { get; set; }
        public int EmployeeInterdictId { get; set; }
        public int EmployeePromotionId { get; set; }


        public virtual EmployeeInterdict EmployeeInterdict { get; set; }    
        public virtual EmployeePromotion EmployeePromotion { get; set; }    
    }
}
