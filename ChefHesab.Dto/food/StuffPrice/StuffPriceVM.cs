using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.StuffPrice
{
    public class StuffPriceVM
    {
        public Guid? Id { get; set; }
        public Guid FoodStuffId { get; set; }
        public string FoodStuffTitle { get; set; }

        public long Price { get; set; }

        public decimal AmountPercent { get; set; }
        public long TotalPrice { get; set; }

        public string InsertDate { get; set; }
        public bool Active { get; set; }
        public Guid? PersonalId { get; set; }
        public string PersonnelFullName { get; set; }
        public Guid CompanyId { get; set; } // CompanyId
    }
}
