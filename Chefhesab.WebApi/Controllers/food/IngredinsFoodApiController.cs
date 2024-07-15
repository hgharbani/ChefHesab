using ChefHesab.Application.Interface.food;
using ChefHesab.Domain.Peresentition.IRepositories.food;
using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Dto.food.IngredinsFood;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chefhesab.WebApi.Controllers.food
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredinsFoodApiController : ControllerBase
    {
        private readonly IIngredinsFoodService _ingredinsFoodService;
        public IngredinsFoodApiController(IIngredinsFoodService ingredinsFoodService)
        {
            _ingredinsFoodService = ingredinsFoodService;
        }

        [HttpPost("GetAllByKendoFilter")]
        public IActionResult GetAllByKendoFilter(SearchIngredinsFoodVm model)
        {

            var result = _ingredinsFoodService.GetIngredinsFoods(model);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateIngredinsFoodVM model)
        {

            var result =await _ingredinsFoodService.Create(model);
            return Ok(result);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(SearchIngredinsFoodVm model)
        {

            var result = await _ingredinsFoodService.Delete(model);
            return Ok(result);
        }

    }
}
