using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.AdditionalCost
{
    public class AdditionalCostVM: filterDataRequest
    {
     
        public Guid Id { get; set; }
    
        public string? Title { get; set; }
        public bool IsShowRatio { get; set; }
        public Guid? PersonalId { get; set; }
    }
}
