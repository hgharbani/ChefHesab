using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.StuffPrice;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.Define.Controllers
{
    [Area("Define")]
    public class StuffPriceController : Controller
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public StuffPriceController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<IActionResult> Create(FoodStuffVM model)
        {
            var result = new ChefResult();
            try
            {
                var stuffPriceModel = new CreateStuffPriceVM()
                {
                    Price = model.Price,
                    Id = model.StuffPricesId,
                    FoodStuffId = model.Id,
                    AmountPercent = model.AmountPercent.HasValue ? model.AmountPercent.Value : 0,
                    CompanyId = model.CompanyId,
                    PersonalId = new Guid("FC769A7E-6A78-42CE-B7F9-0E1619CD5EFB"),
                };
                result = await _apiExtention.PostDataToApiAsync<CreateStuffPriceVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/StuffPriceApi/AddOrEditStuffPrice", stuffPriceModel);



                return Json(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return Json(result);
            }
               
                
           
        }

    }
}
