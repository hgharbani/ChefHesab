using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.Define.Controllers
{
    [Area("Define")]
    public class FoodStuffController : Controller
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
                if (result.IsSuccess)
                {
                    return Json(result);
                }
            }
            return Json(result);
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
                var result = await _apiExtention.PostDataToApiAsync<CreateFoodStuffVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/Edit", model);
                if (result.IsSuccess)
                {
                    return Json(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(FoodStuffSearch model)
        {

            var result = await _apiExtention.PostDataToApiAsync<FoodStuffSearch, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/Delete", model);
            if (result.IsSuccess)
            {
                return Json(result);
            }

            return Json(model);
        }


    }
}
