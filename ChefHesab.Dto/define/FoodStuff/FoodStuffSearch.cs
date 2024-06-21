using ChefHesab.Share.model.KendoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.FoodStuff
{
    public class FoodStuffSearch: Request
    {
        public Guid? Id { get; set; }
        public Guid? CompanyId { get; set; }
        public long? FoodCategoryId { get; set; }
    }
    public class FoodfSearch : Request
    {
        public Guid? Id { get; set; }
        public long? FoodCategoryId { get; set; }
    }
}
