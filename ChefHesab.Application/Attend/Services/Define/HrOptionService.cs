using AutoMapper;
using Ksc.Hr.Application.Interfaces;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.DTO.HrOption;
using Ksc.Hr.Domain.Entities;
using KSC.Common.Filters.Models;
using KSC.Common.Filters.Contracts;
using KSC.Common;
using System.Collections.Generic;
using System.Linq;




namespace Ksc.Hr.Application.Services
{


    public class HrOptionService : IHrOptionService
    {



        private readonly IMapper _mapper;
        private readonly IFilterHandler _filterHandler;
        private readonly IHrOptionRepository _repository;





        public HrOptionService(
            IMapper mapper,
            IFilterHandler filterHandler,
            IHrOptionRepository repository
       )
        {
            _filterHandler = filterHandler;
            _repository = repository;
            _mapper = mapper;
        }


        public KscResult Add(AddOrEditHrOptionDto dto)
        {
            var result = dto.IsValid();
            if (!result.Success)
                return result;
            var hrOption = _mapper.Map<HrOption>(dto);


            _repository.AddAsync(hrOption);
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
        public KscResult Edit(AddOrEditHrOptionDto dto)
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
            var hrOption = _mapper.Map(dto, model);

            _repository.Update(hrOption);
            if (_repository.SaveChanges() > 0) return result;
            result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;
        }
        public FilterResult<HrOptionDto> GetAll(FilterRequest command)
        {
            var query = _repository.GetAll().AsQueryable();
            var list = _filterHandler.GetFilterResult<HrOption>(query, command, "Id");

            return new FilterResult<HrOptionDto>
            {
                Data = _mapper.Map<List<HrOptionDto>>(list.Data.ToList()),
                Total = list.Total
            };
        }


        public IList<HrOptionPairDto> GetAllHrOptionPairDto()
        {
            var entities = _repository.GetAll();
            var dto = _mapper.Map<List<HrOptionPairDto>>(entities);
            return dto;
        }

        public AddOrEditHrOptionDto GetForEdit(int id)
        {
            var model = _repository.GetById(id);
            return _mapper.Map<AddOrEditHrOptionDto>(model);
        }
        public HrOptionDto GetOne(int id)
        {
            var model = _repository.GetById(id);
            return _mapper.Map<HrOptionDto>(model);
        }
        public HrOption GetOneItem(int id)
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


