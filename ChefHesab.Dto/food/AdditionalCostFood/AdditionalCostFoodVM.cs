using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.AdditionalCostFood
{
    public class AdditionalCostFoodVM
    {
        public Guid Id { get; set; }
        public Guid AdditionalCostId { get; set; }
        public string AdditionalCostTitle { get; set; }
        public Guid FoodProviderId { get; set; }
        public string FoodProviderTitle { get; set; }
        public double Ratio { get; set; }
        public double Cost { get; set; }
    }
}
