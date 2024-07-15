using ChefHesab.Application.Interface.define;
using ChefHesab.Application.Interface.food;
using ChefHesab.Application.services.define;
using ChefHesab.Domain.Peresentition.IRepositories.food;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.IngredinsFood;
using ChefHesab.Dto.food.StuffPrice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class StuffPriceApiController : ControllerBase
    {
        private readonly IStuffPriceService _stuffPriceService;
        public StuffPriceApiController(IStuffPriceService stuffPriceService)
        {

            _stuffPriceService = stuffPriceService;
        }

        [HttpPost("AddOrEditStuffPrice")]
        public async Task<IActionResult> AddOrEditStuffPrice(CreateStuffPriceVM model)
        {

            var result = await _stuffPriceService.AddOrUpdate(model);
            return Ok(result);
        }
        [HttpPost("GetAllByKendoFilter")]
        public IActionResult GetAllByKendoFilter(FoodStuffSearch model)
        {

            var result = _stuffPriceService.GetIngredinsFoods(model);
            return Ok(result);
        }
        
    }
}
