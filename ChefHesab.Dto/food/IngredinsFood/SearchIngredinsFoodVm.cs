using ChefHesab.Share.model.KendoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.IngredinsFood
{
    public class SearchIngredinsFoodVm:Request
    {
        public Guid? Id { get; set; }
        public Guid FoodProviderId { get; set; }
    }
}
