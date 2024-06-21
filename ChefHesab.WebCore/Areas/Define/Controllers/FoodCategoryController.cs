using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model.KendoModel;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.Define.Controllers
{
    [Area("Define")]
    public class FoodCategoryController : Controller
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public FoodCategoryController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] SearchFoodCategoryVM request)
        {
            
            var result = await _apiExtention.PostDataToApiAsync<SearchFoodCategoryVM, dynamic>($"{_configuration["ChefHesabApi"]}api/FoodCategoryApi/GetAllByKendoFilter", request);

            return Json(result);
        }
   

    }
}
