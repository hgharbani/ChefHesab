using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.Country;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.DTO.Chart.Chart_JobCategory;
using Ksc.HR.DTO.WorkShift.City;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class CountryService : ICountryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public CountryService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, string title)
        {
            return _kscHrUnitOfWork.CountryRepository.Any(x => x.Id != id && x.Title == title);
        }
        public bool Exists(string title)
        {
            return _kscHrUnitOfWork.CountryRepository.Any(x => x.Title == title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public KscResult AddCountry(AddOrEditCountryModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            if (Exists(model.Title) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }

            var Country = _mapper.Map<Country>(model);
            Country.InsertDate = DateTime.Now;
            Country.InsertUser = model.CurrentUserName;
            Country.OrderNo = 100;
            _kscHrUnitOfWork.CountryRepository.Add(Country);
            _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public KscResult UpdateCountry(AddOrEditCountryModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneCountry = _kscHrUnitOfWork.CountryRepository.GetById(model.Id);
            if (oneCountry == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            //oneCountry = _mapper.Map<Country>(model);
            oneCountry.Code = model.Code;
            oneCountry.Title = model.Title;
            oneCountry.UpdateDate = DateTime.Now;
            oneCountry.UpdateUser = model.CurrentUserName;

            _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public KscResult RemoveCountry(AddOrEditCountryModel model)
        {

            var result = new KscResult();
            //if (!result.Success)
            //    return result;
            var item = _kscHrUnitOfWork.CountryRepository.GetById(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            _kscHrUnitOfWork.CountryRepository.Delete(item);
            _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<CountryModel> GetCountries()
        {
            var cities = _kscHrUnitOfWork.CountryRepository.GetAllQueryable().AsQueryable();
            return _mapper.Map<List<CountryModel>>(cities);

        }
        public FilterResult<CountryModel> GetCountriesByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<Country>(_kscHrUnitOfWork.CountryRepository.GetAllQueryable().AsQueryable(), Filter, "Id");
            return new FilterResult<CountryModel>()
            {
                Data = _mapper.Map<List<CountryModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public List<SearchCountryModel> GetCountryByKendoFilter(CityFilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.CountryRepository
                .GetAll()
                .AsQueryable()
                .AsNoTracking();

            if (Filter.CountryId > 0)
                query = query.Where(x => x.Id == Filter.CountryId);

            var result = _FilterHandler.GetFilterResult<Country>(query, Filter, "Id");

            var finalData = result.Data.Select(a => new SearchCountryModel()
            {
                Id = a.Id,
                Title = a.Title,

                Value = a.Id.ToString(),
                Text = a.Title,
                Selected = (a.Id == Filter.CountryId) ? true : false
            }).ToList();



            return _mapper.Map<List<SearchCountryModel>>(finalData);


        }
        public AddOrEditCountryModel GetEntity(int id)
        {
            var model = _kscHrUnitOfWork.CountryRepository.GetById(id);
            return _mapper.Map<AddOrEditCountryModel>(model);
        }


    }
}
