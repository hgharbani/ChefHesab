using ChefHesab.Application.Interface.define;
using ChefHesab.Domain;
using ChefHesab.Dto.define.AdditionalCost;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Share.model.KendoModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalCostApiController : ControllerBase
    {
        private readonly IAdditionalCostService _additionalCostService;
        public AdditionalCostApiController(IAdditionalCostService additionalCostService)
        {
            _additionalCostService = additionalCostService;
        }

        [HttpPost("GetAllByKendoFilter")]
        public IActionResult GetAllByKendoFilter(AdditionalCostSearchModel request)
        {
            var result = _additionalCostService.GetAdditionalCostWithCompany(request);
            return Ok(result);
        }

        [HttpPost("GetOne")]
        public IActionResult GetOne(AdditionalCostSearchModel model)
        {
            var result = _additionalCostService.GetForEdit(model.Id.Value);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateAdditionalCostVM model)
        {
            var result = await _additionalCostService.Add(model);
            return Ok(result);
        }
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(CreateAdditionalCostVM model)
        {
            var result = await _additionalCostService.Edit(model);
            return Ok(result);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(CreateAdditionalCostVM model)
        {
            var result = await _additionalCostService.Delete(model);
            return Ok(result);
        }
        [HttpPost("checkformula")]
        public async Task<IActionResult> checkformula()
        {
            var result = _additionalCostService.checkformula();
            return Ok(result);
        }
    }
}
