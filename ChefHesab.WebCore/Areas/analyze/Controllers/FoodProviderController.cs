using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.analyze.Controllers
{
    [Area("analyze")]
    public class FoodProviderController : BaseController
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public FoodProviderController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] SearchfoodProvider request)
        {
            var result = await _apiExtention.PostDataToApiAsync<SearchfoodProvider, dynamic>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetAllByKendoFilter", request);

            return Json(result);
        }

        public IActionResult Create()
        {
            var model = new CreateFoodProviderVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateFoodProviderVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PutDataToApiAsync<CreateFoodProviderVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/Add", model);
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
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _apiExtention.GetServiceAsync<CreateFoodProviderVM>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetForEdit?id={id}");

            return View(result);
          
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CreateFoodProviderVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PutDataToApiAsync<CreateFoodProviderVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/Edit", model);
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

        public async Task<IActionResult> CostMeal(Guid id)
        {
            var result = await _apiExtention.GetServiceAsync<FoodProviderVM>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetHeaderAnalyzeFood?id={id}");

            return View(result);
        }

        public async Task<IActionResult> HeaderFoodAnalyze(Guid id)
        {
            var result = await _apiExtention.GetServiceAsync<FoodProviderVM>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetForEdit?id={id}");

            return View(result);
        }
    }
}
