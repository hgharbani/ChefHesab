using ChefHesab.Dto.define.AdditionalCost;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.Define.Controllers
{
    [Area("Define")]
    public class AdditionalCostController: BaseController
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public AdditionalCostController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Create()
        {
            var model = new CreateAdditionalCostVM();
            return View(model);
        }
    
        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] AdditionalCostSearchModel request)
        {
            var result = await _apiExtention.PostDataToApiAsync<AdditionalCostSearchModel, dynamic>($"{_configuration["ChefHesabApi"]}api/AdditionalCostApi/GetAllByKendoFilter", request);

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdditionalCostVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiExtention.PostDataToApiAsync<AdditionalCostVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/AdditionalCostApi/Create", model);
                if (result.IsSuccess)
                {
                    return Json(result);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(AdditionalCostSearchModel model)
        {
            var result = await _apiExtention.PostDataToApiAsync<AdditionalCostSearchModel, CreateAdditionalCostVM>($"{_configuration["ChefHesabApi"]}api/AdditionalCostApi/GetOne", model);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateAdditionalCostVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiExtention.PostDataToApiAsync<CreateAdditionalCostVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/AdditionalCostApi/Edit", model);
                if (result.IsSuccess)
                {
                    return Json(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AdditionalCostSearchModel model)
        {

            var result = await _apiExtention.PostDataToApiAsync<AdditionalCostSearchModel, ChefResult>($"{_configuration["ChefHesabApi"]}api/AdditionalCostApi/Delete", model);
            if (result.IsSuccess)
            {
                return Json(result);
            }

            return Json(model);
        }
    }
}
