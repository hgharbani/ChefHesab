using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;
using ChefHesab.Share.model.KendoModel.Response;
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
        [HttpPost]

        public async Task<IActionResult> GetAllByKendoFilter([FromBody] Request request)
        {
            var result=  await _apiExtention.PostDataToApiAsync<Request,dynamic>($"{_configuration["ChefHesabApi"]}api/CompaniesApi/GetAllByKendoFilter", request);
        
            return Json(result);
        }

        public IActionResult Create()
        {
            var model=new ContractingCompanyVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ContractingCompanyVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiExtention.PostDataToApiAsync<ContractingCompanyVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/CompaniesApi/Create", model);
                if(result.IsSuccess)
                {
                    return Json(result);
                }
            }
            return View(model);
        }
    }
}
