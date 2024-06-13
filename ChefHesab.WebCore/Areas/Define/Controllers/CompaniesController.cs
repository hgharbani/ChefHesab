using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.Extiontions;
using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.WebCore.Areas.Define.Controllers
{
    [Area("Define")]
    public class CompaniesController : Controller
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public CompaniesController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var model=await _apiExtention.GetServiceAsync<List<FoodStuffVM>>($"{_configuration["ChefHesabApi"]}api/FoodStuffApi/GetFoodStuff");
            return View(model);
        }
    }
}
