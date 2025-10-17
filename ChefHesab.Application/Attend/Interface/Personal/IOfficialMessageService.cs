using Ksc.Hr.Domain.Entities.Personal;
using Ksc.Hr.DTO.Personal.OfficialMessage;
using KSC.Common;
using KSC.Common.Filters.Models;
using System.Collections.Generic;

namespace Ksc.Hr.Application.Interfaces.Personal
{
    public interface IOfficialMessageService
    {
        KscResult Add(AddOrEditOfficialMessageDto model);
        KscResult CanRemove(int id);
        KscResult Edit(AddOrEditOfficialMessageDto dto);
        FilterResult<OfficialMessageDto> GetAll(FilterRequest command);
        IList<OfficialMessagePairDto> GetAllOfficialMessagePairDto();
        AddOrEditOfficialMessageDto GetForEdit(int id);
        string GetMessage();
        OfficialMessageDto GetOne(int id);
        OfficialMessage GetOneItem(int id);
        KscResult Remove(int id);
    }
}

