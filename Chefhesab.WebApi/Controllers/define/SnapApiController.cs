using ChefHesab.Application.Interface.define;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnapApiController : ControllerBase
    {
        private readonly IFoodCategoryService _foodCategory;
        private readonly IFoodStuffService _foodStuffService;
        public SnapApiController(IFoodCategoryService foodCategoryService, IFoodStuffService foodStuffService)
        {
            _foodCategory = foodCategoryService;
            _foodStuffService = foodStuffService;
        }
        //[HttpPut("SaveFoodCategory")]
        //public async Task<IActionResult> SaveFoodCategory()
        //{
        //    var client = new HttpClient();
        //    var product = new Rootobject();
        //    HttpResponseMessage response = await client.GetAsync(@"https://core.snapp.market/api/v2/vendors/0r5ryz/products?limit=200&offset=0&categories[]&platform=WEB");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        product = await response.Content.ReadFromJsonAsync<Rootobject>();
        //    }
        //    var result = await _foodCategory.AddFoodCategory(product);

        //    return Ok(result ? "با موفقیت انجام شد" : "عملیات خطایی رخ داد");
        //}

        //[HttpPut("SaveFoodStuff")]
        //public async Task<IActionResult> SaveFoodStuff()
        //{
        //    var client = new HttpClient();
        //    var getCategory = _foodCategory.GetAll();
        //   var food= 0;
        //    foreach (var item in getCategory)
        //    {
        //        food++;
        //        var count = 9000;
        //        var i = 0;
        //        while (i <= count)
        //        {

        //            HttpResponseMessage response = await client.GetAsync($"https://core.snapp.market/api/v2/vendors/0r5ryz/products?limit=200&offset={i}&categories[]={item.SnapId}&platform=WEB");
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var product = await response.Content.ReadFromJsonAsync<Rootobject>();
        //                count = product.metadata.pagination.total;
        //                await _foodStuffService.AddFoodStuffFromSnap(product, item.CategoryId);
        //                i = i + 200;
        //            }
        //            else
        //            {
        //                break;
        //            }



        //        }

        //    }


        //    return Ok();
        //}


    }
}
