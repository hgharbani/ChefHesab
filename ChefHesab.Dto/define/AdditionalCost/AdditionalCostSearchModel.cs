using ChefHesab.Share.model.KendoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.AdditionalCost
{
    public class AdditionalCostSearchModel:Request
    {
        public Guid? Id { get; set; }
        public Guid? CompanyId { get; set; }
        public AdditionalCostSearchModel()
        {
            CompanyId = null;
        }

    }
   
}
