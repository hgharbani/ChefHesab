using ChefHesab.Application.Interface.food;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Dto.food.AdditionalCostFood;
using ChefHesab.Dto.food.IngredinsFood;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.food
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalCostFoodApiController : ControllerBase
    {
        private readonly IAdditionalCostFoodService _additionalCostFoodService;
        public AdditionalCostFoodApiController(IAdditionalCostFoodService additionalCostFoodService)
        {
            _additionalCostFoodService = additionalCostFoodService;
        }

        [HttpPost("GetAllByKendoFilter")]
        public IActionResult GetAllByKendoFilter(SearchAdditionalCostfoodVM model)
        {

            var result = _additionalCostFoodService.GetAdditionalCostFood(model);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateAdditionalCostFoodVM model)
        {

            var result = await _additionalCostFoodService.Create(model);
            return Ok(result);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(SearchAdditionalCostfoodVM model)
        {

            var result = await _additionalCostFoodService.Delete(model);
            return Ok(result);
        }

    }
}
