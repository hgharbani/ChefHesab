using ChefHesab.Application.Interface.define;
using ChefHesab.Dto.define.FoodStuff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace Chefhesab.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalApiController : ControllerBase
    {
        private readonly IPersonalService _personalService;
        public PersonalApiController(IPersonalService personalService)
        {
            _personalService = personalService;
        }
        [HttpGet("GetAllPersonel")]
        public async Task<IActionResult> Get()
        {
            var client=new HttpClient();
            var product = new Rootobject();
            HttpResponseMessage response = await client.GetAsync(@"https://core.snapp.market/api/v2/vendors/0r5ryz/products?limit=200&offset=0&categories[]&platform=WEB");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadFromJsonAsync<Rootobject>();
            }

            return Ok(product);
        }
    }
}
