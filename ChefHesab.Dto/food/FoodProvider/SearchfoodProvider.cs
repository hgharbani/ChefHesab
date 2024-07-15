using ChefHesab.Share.model.KendoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.FoodProvider
{
    public class SearchfoodProvider:Request
    {
        public Guid? CompanyId { get; set; }
        public int? FoodCategoryId { get; set; }
    }
}
