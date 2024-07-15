using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Dto.food.IngredinsFood;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.analyze.Controllers
{
    [Area("analyze")]
    public class IngredinsFoodController : BaseController
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public IngredinsFoodController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] SearchIngredinsFoodVm request)
        {
            var result = await _apiExtention.PostDataToApiAsync<SearchIngredinsFoodVm, dynamic>($"{_configuration["ChefHesabApi"]}api/IngredinsFoodApi/GetAllByKendoFilter", request);

            return Json(result);
        }

        public IActionResult Create()
        {
            var model = new CreateFoodProviderVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateIngredinsFoodVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PostDataToApiAsync<CreateIngredinsFoodVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/IngredinsFoodApi/Add", model);
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
        public async Task<IActionResult> Delete([FromForm] SearchIngredinsFoodVm model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PostDataToApiAsync<SearchIngredinsFoodVm, ChefResult>($"{_configuration["ChefHesabApi"]}api/IngredinsFoodApi/Delete", model);
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
