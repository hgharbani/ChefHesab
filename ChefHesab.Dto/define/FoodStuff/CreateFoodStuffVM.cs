using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.FoodStuff
{
    public class CreateFoodStuffVM
    {
        public Guid? Id { get; set; }  
        public string Title { get; set; }  
        public long FoodCategoryId { get; set; }
    }
}
