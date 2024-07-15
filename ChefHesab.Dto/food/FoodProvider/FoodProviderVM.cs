using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.FoodProvider
{
    public class FoodProviderVM
    {
        public Guid Id { get; set; }
        public Guid ContractCompanyId { get; set; }
        public Guid FoodStuffId { get; set; }
        public bool Active { get; set; }
        public string CompanyName { get; set; }
        public string FoodName { get; set; }
        public string CategoryName { get; set; }
        public long AmountRequested { get; set; }
        public long TotalPrice { get; set; }
    }
}
