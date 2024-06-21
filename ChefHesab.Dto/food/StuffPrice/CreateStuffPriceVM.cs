using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.StuffPrice
{
    public class CreateStuffPriceVM
    {
        public Guid? Id { get; set; }
        public Guid FoodStuffId { get; set; }
        public long Price { get; set; }

        public decimal AmountPercent { get; set; }
        public long TotalPrice { get; set; }

        public Guid? PersonalId { get; set; }
        public Guid CompanyId { get; set; } // CompanyId
    }
}
