using AutoMapper;
using Ksc.Hr.Application.Interfaces;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.DTO.Vacation;
using Ksc.Hr.Domain.Entities;
using KSC.Common.Filters.Models;
using KSC.Common.Filters.Contracts;
using KSC.Common;
using System.Linq;
using System.Collections.Generic;




namespace Ksc.Hr.Application.Services
{


    public class VacationService : IVacationService
    {
        private readonly IMapper _mapper;
        private readonly IFilterHandler _filterHandler;
        private readonly IVacationRepository _repository;
        public VacationService( IMapper mapper,  IFilterHandler filterHandler, IVacationRepository repository  )
        {
            _filterHandler = filterHandler;
            _repository = repository;
            _mapper = mapper;
        }


        public KscResult Add(AddOrEditVacationDto dto)
        {
            var result = dto.IsValid();
            if (!result.Success)
                return result;
            var vacation = _mapper.Map<Vacation>(dto);


            _repository.AddAsync(vacation);
            if (_repository.SaveChanges() > 0)
                return result;
            result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;
        }
        public KscResult CanRemove(int id)
        {
            var result = new KscResult();
            var item = GetOneItem(id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            return result;
        }
        public KscResult Edit(AddOrEditVacationDto dto)
        {
            var result = dto.IsValid();
            if (!result.Success)
                return result;
            var model = GetOneItem(dto.Id);
            if (model == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            var vacation = _mapper.Map(dto, model);

            _repository.Update(vacation);
            if (_repository.SaveChanges() > 0) return result;
            result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;
        }
        public FilterResult<VacationDto> GetAll(FilterRequest command)
        {
            var query = _repository.GetAllQueryable().AsQueryable();
            var list = _filterHandler.GetFilterResult<Vacation>(query, command, "Id");

            return new FilterResult<VacationDto>
            {
                Data = _mapper.Map<List<VacationDto>>(list.Data.ToList()),
                Total = list.Total
            };
        }


        public IList<VacationPairDto> GetAllVacationPairDto()
        {
            var entities = _repository.GetAllQueryable();
            var dto = _mapper.Map<List<VacationPairDto>>(entities);
            return dto;
        }

        public AddOrEditVacationDto GetForEdit(int id)
        {
            var model = _repository.GetById(id);
            return _mapper.Map<AddOrEditVacationDto>(model);
        }
        public VacationDto GetOne(int id)
        {
            var model = _repository.GetById(id);
            return _mapper.Map<VacationDto>(model);
        }
        public Vacation GetOneItem(int id)
        {
            return _repository.Where(x => x.Id == id).FirstOrDefault();
        }

        public KscResult Remove(int id)
        {
            var result = new KscResult();
            //if (!result.Success)        
            //    return result;          
            var item = GetOneItem(id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            _repository.Delete(item);

            if (_repository.SaveChanges() > 0) return result;
            result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;
        }


    }
}


