using ChefHesab.Application.Interface.define;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodCategoryApiController : ControllerBase
    {
        private readonly IFoodCategoryService _foodCategory;
        private readonly IFoodStuffService _foodStuffService;
        public FoodCategoryApiController(IFoodCategoryService foodCategoryService, IFoodStuffService foodStuffService)
        {
            _foodCategory = foodCategoryService;
            _foodStuffService = foodStuffService;
        }
     
    }
}
