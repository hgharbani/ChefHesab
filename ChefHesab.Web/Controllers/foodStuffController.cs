using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.Web.Controllers
{
    public class foodStuffController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetFoodStuffVm()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7111/api/FoodStuffApi/GetFoodStuff");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                return Json(product);
            }
           return Json(null);
        }
    }
}
