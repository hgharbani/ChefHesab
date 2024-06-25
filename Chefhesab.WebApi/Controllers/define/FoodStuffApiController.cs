using ChefHesab.Application.Interface.define;
using ChefHesab.Dto.define.FoodStuff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodStuffApiController : ControllerBase
    {

        private readonly IFoodStuffService _foodStuffService;
        public FoodStuffApiController(IFoodStuffService foodStuffService)
        {

            _foodStuffService = foodStuffService;
        }

        [HttpPost("GetFoodStuffPriceWithCompany")]
        public IActionResult GetFoodStuffPriceWithCompany(FoodStuffSearch model)
        {

            var result = _foodStuffService.GetFoodStuffPriceWithCompany(model);
            return Ok(result);
        }



        [HttpPost("GetFoodStuffWithCategory")]
        public IActionResult GetFoodStuffWithCategory(FoodfSearch model)
        {

            var result = _foodStuffService.GetFoodStuffWithCategory(model);
            return Ok(result);
        }

        [HttpPost("GetExcelFileAndSaveInSql")]
        public async Task<IActionResult> GetExcelFileAndSaveInSql()
        {

            var result =await _foodStuffService.GetExcelFileAndSaveInSql();
            return Ok(result);
        }

        [HttpGet("GetFoodStuffForEdit")]
        public async Task<IActionResult> GetFoodStuffForEdit(Guid id)
        {

            var result =await _foodStuffService.GetFoodStuffForEdit(id);
            return Ok(result);
        }


        [HttpPut("Add")]
        public async Task<IActionResult> Add(CreateFoodStuffVM model)
        {

            var result =await _foodStuffService.Add(model);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult>  Edit(CreateFoodStuffVM model)
        {

            var result =await _foodStuffService.Edit(model);
            return Ok(result);
        }
    }
}
