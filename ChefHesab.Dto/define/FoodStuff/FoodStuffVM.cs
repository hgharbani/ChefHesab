using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.FoodStuff
{
    public class FoodStuffVM
    {
        public Guid Id { get; set; }
   
        public string Title { get; set; }
        public long Price { get; set; }
        public long FoodCategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public Guid CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public Guid? PersonalId { get; set; }
        public Guid? StuffPricesId { get; set; }
        public string FoodCategoryTitle { get; set; }
        public decimal? AmountPercent { get; set; }
        public long TotalPrice { get; set; }
    }
}
