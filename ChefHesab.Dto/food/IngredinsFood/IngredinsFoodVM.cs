using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.IngredinsFood
{
    public class IngredinsFoodVM
    {
        public Guid Id { get; set; }
        public Guid StuffPriceId { get; set; }
        public string StuffPriceTitle { get; set; }
        public Guid FoodProviderId { get; set; }
        public double? Amount { get; set; }
        public double? Cost { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
        public string Unit { get; set; }
        public string CategoryTitle { get; set; }
    }
}
