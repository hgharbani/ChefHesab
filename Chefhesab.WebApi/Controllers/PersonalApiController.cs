using ChefHesab.Application.Interface.define;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var result =await _personalService.GetAllProducts();
            return Ok(result);
        }
    }
}
