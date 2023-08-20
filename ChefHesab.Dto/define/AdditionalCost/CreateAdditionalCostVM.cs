using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.AdditionalCost
{
    public class CreateAdditionalCostVM
    {
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public bool IsShowRatio { get; set; }
        public Guid? PersonalId { get; set; }
    }
}
