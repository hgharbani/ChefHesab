using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.WorkTime;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.City;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.Share.Extention;
using Ksc.HR.DTO.WorkFlow.BaseFile;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class CityService : ICityService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public CityService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, string title)
        {
            return _kscHrUnitOfWork.CityRepository.Any(x => x.Id != id == true && x.Title == title);
        }
        public bool Exists(string title)
        {
            return _kscHrUnitOfWork.CityRepository.Any(x => x.Title == title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddCity(AddCityModel model)
        {
            var result = model.IsValid();
            try
            {
                if (!result.Success)
                    return result;

                if (Exists(model.Title) == true)
                {

                    result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                    return result;
                }

                var City = _mapper.Map<City>(model);
                City.InsertDate = DateTime.Now;
                City.InsertUser = model.CurrentUserName;

                await _kscHrUnitOfWork.CityRepository.AddAsync(City);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public async Task<KscResult> UpdateCity(EditCityModel model)
        {
            var result = model.IsValid();

            try
            {
                if (!result.Success)
                    return result;
                var oneCity = GetOne(model.Id);
                if (oneCity == null)
                {
                    result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                    return result;
                }
                oneCity.Code = model.Code;
                oneCity.Title = model.Title;
                oneCity.ProvinceId = model.ProvinceId;
                oneCity.UpdateDate = DateTime.Now;
                oneCity.UpdateUser = model.CurrentUserName;

            }
            catch (Exception ex)
            {

            }

            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public KscResult RemoveCity(EditCityModel model)
        {

            var result = new KscResult();
            //if (!result.Success)
            //    return result;
            var item = GetOne(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;


            _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<CityModel> GetCity()
        {
            var cities = _kscHrUnitOfWork.CityRepository.GetAllQueryable();
            return _mapper.Map<List<CityModel>>(cities);

        }
        public List<AutomCompleteModel> AutoComplete(string text)
        {
            var cities = _kscHrUnitOfWork.CityRepository.GetAllQueryable();
            return cities.Select(x => new AutomCompleteModel()
            {
                value = x.Id.ToString(),
                text = x.Title
            }).ToList();

        }
        public List<CityTrainingModel> GetCityTraining()
        {
            //var cities = _kscHrUnitOfWork.CityRepository.WhereQueryable(c=>
            //                                                        c.Id == EnumCity.Abadan.Id
            //                                                        || c.Id == EnumCity.Ahvaz.Id
            //                                                        );//.Where(x=>x.Id==1513);
            var cities = _kscHrUnitOfWork.CityRepository.WhereQueryable(x => x.Id == 1513);
            return _mapper.Map<List<CityTrainingModel>>(cities);

        }

        /// <summary>
        /// محل صدور دفترچه ها
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public FilterResult<CityModel> GetCityByFilterBooklet(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.CityRepository.GetAllData().Where(x => x.IsBookletIssuingPlace == true)
                .OrderBy(x => x.Province.Country.OrderNo)
                .Select(a => new CityModel()
                {
                    ProvinceId = a.ProvinceId,
                    Id = a.Id,
                    Code = a.Code,
                    ProvinceTitle = a.Province != null ? a.Province.Title : "",
                    ProvinceCode = a.Province.Code,
                    Title = a.Title,
                    InsertDate = a.InsertDate,
                    InsertUser = a.InsertUser,
                    Country = a.Province.Country != null ? a.Province.Country.Title : "",
                    OrderNo = a.Province.Country.OrderNo
                });

            var result = _FilterHandler.GetFilterResult<CityModel>(query, Filter, "OrderNo");

            var CitiesFiltred = result.Data.ToList();

            return new FilterResult<CityModel>()
            {
                Data = CitiesFiltred,
                Total = result.Total

            };
        }


        public FilterResult<CityModel> GetCityByFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.CityRepository.GetAllData()
                .OrderBy(x => x.Province.Country.OrderNo)
                .Select(a => new CityModel()
                {
                    ProvinceId = a.ProvinceId,
                    Id = a.Id,
                    Code = a.Code,
                    ProvinceTitle = a.Province != null ? a.Province.Title : "",
                    ProvinceCode = a.Province.Code,
                    Title = a.Title,
                    InsertDate = a.InsertDate,
                    InsertUser = a.InsertUser,
                    Country = a.Province.Country != null ? a.Province.Country.Title : "",
                    OrderNo = a.Province.Country.OrderNo
                });

            var result = _FilterHandler.GetFilterResult<CityModel>(query, Filter, "OrderNo");

            var CitiesFiltred = result.Data.ToList();

            return new FilterResult<CityModel>()
            {
                Data = CitiesFiltred,
                Total = result.Total

            };
        }
        public FilterResult<CityModel> GetCityWithProviceCountryByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<City>(_kscHrUnitOfWork.CityRepository.GetCityWithProviceCompany(), Filter, "Id");

            var CitiesFiltred = result.Data.Select(a => new CityModel()
            {
                ProvinceId = a.ProvinceId,
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                ProvinceTitle = a.Province?.Title ?? "",
                ProvinceCode = a.Province?.Code ?? "",
                ProviceId = a.ProvinceId ?? null,
                CountryId = a.Province?.Country?.Id ?? null,
                CountryTitle = a.Province?.Country?.Title ?? "",
                CountryCode = a.Province?.Country?.Code ?? "",
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser
            }).ToList();

            return new FilterResult<CityModel>()
            {
                Data = CitiesFiltred,
                Total = result.Total

            };
        }
        public FilterResult<CityModel> GetCityWithProviceCountryValidForMissionByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<City>(_kscHrUnitOfWork.CityRepository.GetCityWithProviceCompanyValidForMission(), Filter, "Id");

            var CitiesFiltred = result.Data.Select(a => new CityModel()
            {
                ProvinceId = a.ProvinceId,
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                ProvinceTitle = a.Province?.Title ?? "",
                ProvinceCode = a.Province?.Code ?? "",
                ProviceId = a.ProvinceId ?? null,
                CountryId = a.Province?.Country?.Id ?? null,
                CountryTitle = a.Province?.Country?.Title ?? "",
                CountryCode = a.Province?.Country?.Code ?? "",
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser
            }).ToList();

            return new FilterResult<CityModel>()
            {
                Data = CitiesFiltred,
                Total = result.Total

            };
        }

        public City GetOne(int id)
        {
            var data = _kscHrUnitOfWork.CityRepository.GetAllDataWithCityId(id).First(a => a.Id == id);
            return data;


        }



        public List<AutomCompleteModel> GetByIds(List<int> ids)
        {
            var cities = _kscHrUnitOfWork.CityRepository.GetAllQueryable().AsQueryable().Where(a => ids.Contains(a.Id));
            return cities.Select(x => new AutomCompleteModel()
            {
                value = x.Id.ToString(),
                text = x.Title
            }).ToList();



        }
        public EditCityModel GetForEdit(int id)
        {
            var model = GetOne(id);


            var data = _mapper.Map<EditCityModel>(model);
            data.CountryId = (model.Province.CountryId == null) ? 0 : model.Province.CountryId.Value;
            return data;
        }

        //public List<SearchCityModel> GetWorkByKendoFilter(FilterRequest Filter)
        //{
        //    var result = _FilterHandler.GetFilterResult<City>(_kscHrUnitOfWork.CityRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
        //    return _mapper.Map<List<SearchCityModel>>(result.Data.ToList());
        //}

        public List<SearchCityModel> GetCitiesByKendoFilter(FilterRequest Filter)
        {

            var result = _FilterHandler.GetFilterResult<WorkCity>(_kscHrUnitOfWork.WorkCityRepository.GetAllQueryable().AsQueryable().Include(x => x.City).Include(x => x.Company).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchCityModel()
            {
                Id = a.Id,
                Title = a.City.Title,
                Code = a.City.Code,
                CompanyTitle = a.Company.Title
            }).ToList();
            return _mapper.Map<List<SearchCityModel>>(finalData);
        }
        public FilterResult<SearchCityModel> GetWorkCitiesProviceByKendoFilter(FilterRequest Filter)
        {
            var finalData = _kscHrUnitOfWork.WorkCityRepository.GetCityWithProvice().AsNoTracking().Select(a => new SearchCityModel()
            {
                Id = a.Id,
                Title = a.City.Title+"-"+a.Company.Title,
                Code = a.City.Code,
                ProviceTitle = a.City.Province.Title,
                //CompanyTitle = a.Company.Title
            });
            var result = _FilterHandler.GetFilterResult<SearchCityModel>(finalData, Filter, "Id");

            //  var finalData = result.Data.Select(a => new SearchCityModel()
            //{
            //    Id = a.Id,
            //    Title = a.City.Title,
            //    Code = a.City.Code,
            //    ProviceTitle = a.City.Province.Title
            //}).ToList();
            return new FilterResult<SearchCityModel>()
            {
                Data = result.Data,
                Total = result.Total

            };
            //return _mapper.Map<List<SearchCityModel>>(finalData);
        }

        public List<SearchProvinceModel> GetProvincesByKendoFilterCountryId(SearchProvinceByCountryId Filter)
        {
            var result = _FilterHandler.GetFilterResult<Province>(_kscHrUnitOfWork.ProvinceRepository.GetAllQueryable()
                .AsQueryable()
                .Where(a => a.CountryId == Filter.CountryId).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchProvinceModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
                Value = a.Id.ToString(),
                Text = a.Title
            }).ToList();

            return _mapper.Map<List<SearchProvinceModel>>(finalData);
        }

    }
}
