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
        public IActionResult GetExcelFileAndSaveInSql()
        {

            var result = _foodStuffService.GetExcelFileAndSaveInSql();
            return Ok(result);
        }

        [HttpPost("GetFoodStuffForEdit")]
        public async Task<IActionResult> GetFoodStuffForEdit(Guid id)
        {

            var result = _foodStuffService.GetFoodStuffForEdit(id);
            return Ok(result);
        }


        [HttpPut("Add")]
        public IActionResult Add(CreateFoodStuffVM model)
        {

            var result = _foodStuffService.Add(model);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public IActionResult Edit(CreateFoodStuffVM model)
        {

            var result = _foodStuffService.Edit(model);
            return Ok(result);
        }
    }
}
