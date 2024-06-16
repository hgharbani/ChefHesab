using ChefHesab.Application.Interface.define;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodStuffApiController : ControllerBase
    {

        private readonly IFoodStuffService _foodStuffService;
        public FoodStuffApiController( IFoodStuffService foodStuffService)
        {
            
            _foodStuffService = foodStuffService;
        }
        [HttpGet("GetFoodStuff")]
        public async Task<IActionResult> GetFoodStuff() {

            var result=await _foodStuffService.GetFoodStuff();
            return Ok(result);
        }
        
    }
}
