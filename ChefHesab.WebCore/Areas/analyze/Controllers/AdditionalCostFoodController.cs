using ChefHesab.Dto.food.AdditionalCostFood;
using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Dto.food.IngredinsFood;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.analyze.Controllers
{
    [Area("analyze")]
    public class AdditionalCostFoodController : BaseController
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public AdditionalCostFoodController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] SearchAdditionalCostfoodVM request)
        {
            var result = await _apiExtention.PostDataToApiAsync<SearchAdditionalCostfoodVM, dynamic>($"{_configuration["ChefHesabApi"]}api/AdditionalCostFoodApi/GetAllByKendoFilter", request);

            return Json(result);
        }

        public IActionResult Create()
        {
            var model = new CreateFoodProviderVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAdditionalCostFoodVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PostDataToApiAsync<CreateAdditionalCostFoodVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/AdditionalCostFoodApi/Add", model);
                if (!result.IsSuccess)
                {
                    ErrorNotifications(result.Errors, true, true);
                    return InvokeNotifications(false);
                }
                SuccessNotification("اطلاعات با موفقیت ذخیره شد");

                return InvokeNotifications(true);
            }
            else
            {
                var ListErrors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0).SelectMany(a => a).ToList();
                ErrorNotifications(ListErrors.Select(a => a.ErrorMessage).ToList(), true, true);
                return InvokeNotifications(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] SearchAdditionalCostfoodVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PostDataToApiAsync<SearchAdditionalCostfoodVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/AdditionalCostFoodApi/Delete", model);
                if (!result.IsSuccess)
                {
                    ErrorNotifications(result.Errors, true, true);
                    return InvokeNotifications(false);
                }
                SuccessNotification("اطلاعات با موفقیت ذخیره شد");

                return InvokeNotifications(true);
            }
            else
            {
                var ListErrors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0).SelectMany(a => a).ToList();
                ErrorNotifications(ListErrors.Select(a => a.ErrorMessage).ToList(), true, true);
                return InvokeNotifications(false);
            }
        }
    }
}
