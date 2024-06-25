using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.Define.Controllers
{
    [Area("Define")]
    public class FoodStuffController : BaseController
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public FoodStuffController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
           
            return View();
        }
        public async Task<IActionResult> FoodIndex()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] FoodStuffSearch request)
        {
            var result = await _apiExtention.PostDataToApiAsync<FoodStuffSearch, dynamic>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/GetFoodStuffPriceWithCompany", request);

            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> GetAllFoodByKendoFilter([FromBody] FoodfSearch request)
        {
            var result = await _apiExtention.PostDataToApiAsync<FoodfSearch, dynamic>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/GetFoodStuffWithCategory", request);

            return Json(result);
        }

        public IActionResult Create()
        {
            var model = new CreateFoodStuffVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodStuffVM model)
        {
            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                 result = await _apiExtention.PutDataToApiAsync<CreateFoodStuffVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/Add", model);
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
                           .Where(y => y.Count > 0).SelectMany(a=>a).ToList();
                ErrorNotifications(ListErrors.Select(a=>a.ErrorMessage).ToList(), true, true);
                return InvokeNotifications(false);
            }
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            var result = await _apiExtention.GetServiceAsync<CreateFoodStuffVM>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/GetFoodStuffForEdit?id={Id}");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateFoodStuffVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiExtention.PutDataToApiAsync<CreateFoodStuffVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/Edit", model);
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
        public async Task<IActionResult> Delete(FoodStuffSearch model)
        {

            var result = await _apiExtention.PutDataToApiAsync<FoodStuffSearch, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/Delete", model);
            if (!result.IsSuccess)
            {
                ErrorNotifications(result.Errors, true, true);
                return InvokeNotifications(false);
            }
            SuccessNotification("اطلاعات با موفقیت ذخیره شد");

            return InvokeNotifications(true);
        }

    }
}
