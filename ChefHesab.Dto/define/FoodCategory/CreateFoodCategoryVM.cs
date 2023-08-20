using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.FoodCategory
{
    public class CreateFoodCategoryVM
    {
  
        public Guid Id { get; set; }
        public string Title { get; set; }
   
        public bool? Active { get; set; }
    }
}
