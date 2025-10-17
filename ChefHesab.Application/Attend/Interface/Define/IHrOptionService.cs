using Ksc.Hr.Domain.Entities;
using Ksc.Hr.DTO.HrOption;
using KSC.Common;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
namespace Ksc.Hr.Application.Interfaces
{
    public interface IHrOptionService
    {
        KscResult Add(AddOrEditHrOptionDto model);
        KscResult CanRemove(int id);
        KscResult Edit(AddOrEditHrOptionDto dto);
        FilterResult<HrOptionDto> GetAll(FilterRequest command);
        IList<HrOptionPairDto> GetAllHrOptionPairDto();
        AddOrEditHrOptionDto GetForEdit(int id);
        HrOptionDto GetOne(int id);
        HrOption GetOneItem(int id);
        KscResult Remove(int id);
    }
}

