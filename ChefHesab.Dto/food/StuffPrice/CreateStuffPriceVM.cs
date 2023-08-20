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
        public Guid Id { get; set; }
        public Guid FoodStuffId { get; set; }
        public string FoodStuffTitle { get; set; }
        [Required]
        [StringLength(10)]
        public string Price { get; set; }
        [Required]
        [StringLength(10)]
        public string Unit { get; set; }
        [Required]
        [StringLength(10)]
        public string InsertDate { get; set; }
        public bool Active { get; set; }
        public Guid? PersonalId { get; set; }
        public string PersonnelFullName { get; set; }
    }
}
