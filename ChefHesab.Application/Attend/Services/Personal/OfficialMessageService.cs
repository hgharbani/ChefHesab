using AutoMapper;
using Ksc.Hr.Application.Interfaces.Personal;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.Hr.DTO.Personal.OfficialMessage;
using Ksc.Hr.Domain.Entities.Personal;
using KSC.Common.Filters.Models;
using KSC.Common.Filters.Contracts;
using KSC.Common;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Ksc.Hr.Application.Services.Personal
{
    public class OfficialMessageService : IOfficialMessageService
    {
        private readonly IMapper _mapper;
        private readonly IFilterHandler _filterHandler;
        private readonly IOfficialMessageRepository _repository;
        public OfficialMessageService(IMapper mapper,
            IFilterHandler filterHandler,
            IOfficialMessageRepository repository
            )
        {
            _filterHandler = filterHandler;
            _repository = repository;
            _mapper = mapper;
        }


        public KscResult Add(AddOrEditOfficialMessageDto dto)
        {
            var result = dto.IsValid();
            if (!result.Success)
                return result;
            if(_repository.Any(a=>(a.StartDate<= dto.StartDate&&a.EndDate>= dto.StartDate)|| (a.StartDate <= dto.EndDate && a.EndDate >= dto.EndDate)))
            {
                result.AddError("رکورد نامعتبر", "این بازه قابل ثبت نمی باشد");
                return result;
            }
            var officialMessage = _mapper.Map<OfficialMessage>(dto);
            _repository.AddAsync(officialMessage);
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
        public KscResult Edit(AddOrEditOfficialMessageDto dto)
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
            if (_repository.Any(a =>a.Id!=dto.Id&& ((a.StartDate <= dto.StartDate && a.EndDate >= dto.StartDate) || (a.StartDate <= dto.EndDate && a.EndDate >= dto.EndDate))))
            {
                result.AddError("رکورد نامعتبر", "این بازه قابل ثبت نمی باشد");
                return result;
            }
            var officialMessage = _mapper.Map(dto, model);

            _repository.Update(officialMessage);
            if (_repository.SaveChanges() > 0) return result;
            result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;
        }
        public FilterResult<OfficialMessageDto> GetAll(FilterRequest command)
        {
            var query = _repository.GetAllQueryable().AsQueryable();
            var list = _filterHandler.GetFilterResult<OfficialMessage>(query, command, "Id");

            return new FilterResult<OfficialMessageDto>
            {
                Data = _mapper.Map<List<OfficialMessageDto>>(list.Data.ToList()),
                Total = list.Total
            };
        }


        public IList<OfficialMessagePairDto> GetAllOfficialMessagePairDto()
        {
            var entities = _repository.GetAllQueryable();
            var dto = _mapper.Map<List<OfficialMessagePairDto>>(entities);
            return dto;
        }

        public AddOrEditOfficialMessageDto GetForEdit(int id)
        {
            var model = _repository.GetById(id);
            return _mapper.Map<AddOrEditOfficialMessageDto>(model);
        }
        public OfficialMessageDto GetOne(int id)
        {
            var model = _repository.GetById(id);
            return _mapper.Map<OfficialMessageDto>(model);
        }
        public OfficialMessage GetOneItem(int id)
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

        public string GetMessage()
        {
            var datetime = DateTime.Now.Date;
            var find = _repository.FirstOrDefault(a => a.StartDate.Date <= datetime && a.EndDate.Date >= datetime);

            return find != null ? find.Messages : "";
        }

    }
}


