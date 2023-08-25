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
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        public Guid? FoodCategoryId { get; set; }
        public string CategoryTitle { get; set; }
    }
}
