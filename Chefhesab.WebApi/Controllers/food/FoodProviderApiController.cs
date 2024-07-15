using ChefHesab.Application.Interface.define;
using ChefHesab.Application.Interface.food;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.FoodProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.food
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodProviderApiController : ControllerBase
    {
        private readonly IFoodProviderService _foodProviderService;
        public FoodProviderApiController(IFoodProviderService foodProviderService)
        {

            _foodProviderService = foodProviderService;
        }

        [HttpPost("GetAllByKendoFilter")]
        public IActionResult GetAllByKendoFilter(SearchfoodProvider model)
        {

            var result = _foodProviderService.GetFoodProviders(model);
            return Ok(result);
        }


        [HttpGet("GetForEdit")]
        public async Task<IActionResult> GetForEdit(Guid id)
        {

            var result = await _foodProviderService.GetForEdit(id);
            return Ok(result);
        }
        [HttpGet("GetHeaderAnalyzeFood")]
        public async Task<IActionResult> GetHeaderAnalyzeFood(Guid id)
        {

            var result = await _foodProviderService.GetHeaderAnalyzeFood(id);
            return Ok(result);
        }
        
        [HttpPut("Add")]
        public async Task<IActionResult> Add(CreateFoodProviderVM model)
        {

            var result = await _foodProviderService.Add(model);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(CreateFoodProviderVM model)
        {

            var result = await _foodProviderService.Edit(model);
            return Ok(result);
        }
    }
}
