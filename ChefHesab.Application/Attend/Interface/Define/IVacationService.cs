  using Ksc.Hr.Domain.Entities;
  using Ksc.Hr.DTO.Vacation;
using KSC.Common;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
namespace Ksc.Hr.Application.Interfaces
  { 
   public interface IVacationService
  {
     KscResult Add(AddOrEditVacationDto model);
    KscResult CanRemove(int id);
  KscResult Edit(AddOrEditVacationDto dto);
  FilterResult<VacationDto> GetAll(FilterRequest command);
  IList<VacationPairDto> GetAllVacationPairDto();
  AddOrEditVacationDto GetForEdit(int id);
  VacationDto GetOne(int id);
  Vacation GetOneItem(int id);
  KscResult Remove(int id);
  }
  }

