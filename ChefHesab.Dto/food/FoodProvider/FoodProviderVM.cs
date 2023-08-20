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
        [Column(TypeName = "date")]
        public DateTime InsertDate { get; set; }
    }
}
