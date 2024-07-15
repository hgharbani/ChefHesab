using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.IngredinsFood
{
    public class CreateIngredinsFoodVM
    {
        public Guid Id { get; set; }
        public Guid? StuffPriceId { get; set; }
        public Guid FoodProviderId { get; set; }
        public double? Amount { get; set; }
        public double? Cost { get; set; }
        public string Unit { get; set; }
    }
}
