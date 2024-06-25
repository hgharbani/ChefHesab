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
        public string? Title { get; set; } // Title
        public long? FoodCategoryId { get; set; } // FoodCategoryId
        public long? Price { get; set; } // FoodCategoryId
        public bool IsShowRatio { get; set; } // IsShowRatio
        public Guid? PersonalId { get; set; } // PersonalId
        public Guid? CompanyId { get; set; } // CompanyId
        public string? CompanyTitle { get; set; } // CompanyId

    }
}
