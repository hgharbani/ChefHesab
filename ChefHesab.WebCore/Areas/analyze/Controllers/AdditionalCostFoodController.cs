using ChefHesab.Share.Extiontions;
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
    }
}
