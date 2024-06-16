using ChefHesab.Application.Interface.define;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Share.model.KendoModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chefhesab.WebApi.Controllers.define
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesApiController : ControllerBase
    {
        private readonly IContractingCompanyService _companyService;
        public CompaniesApiController(IContractingCompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("GetAllByKendoFilter")]
        public IActionResult GetAllByKendoFilter([FromBody] Request request)
        {
            var result = _companyService.GetAllByKendoFilter(request);
            return Ok(result);
        }

        [HttpPost("GetOne")]
        public IActionResult GetOne([FromBody] ContractingCompanyVM model)
        {
            var result = _companyService.GetContractingCompany(request);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ContractingCompanyVM model)
        {
            var result = await _companyService.Add(model);
            return Ok(result);
        }
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(ContractingCompanyVM model)
        {
            var result = await _companyService.Edit(model);
            return Ok(result);
        }
    }
}

