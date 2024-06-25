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
    public class StuffPriceController : BaseController
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
                    PersonalId = currentUser,
                };
                result = await _apiExtention.PostDataToApiAsync<CreateStuffPriceVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/StuffPriceApi/AddOrEditStuffPrice", stuffPriceModel);
                if (!result.IsSuccess)
                {
                    ErrorNotifications(result.Errors, true, true);
                    return InvokeNotifications(false);
                }
                SuccessNotification("اطلاعات با موفقیت ذخیره شد");

                return InvokeNotifications(true);
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.Message);

                return InvokeNotifications(false);
            }
               
                
           
        }

    }
}
