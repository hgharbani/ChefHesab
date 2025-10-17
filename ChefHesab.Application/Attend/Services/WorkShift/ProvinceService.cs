using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.Province;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Chart.StudyField;
using Ksc.HR.Domain.Entities.Chart;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class ProvinceService : IProvinceService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ProvinceService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, string Title)
        {
            return _kscHrUnitOfWork.ProvinceRepository.Any(x => x.Id != id && x.Title == Title);
        }
        public bool Exists(string Title)
        {
            return _kscHrUnitOfWork.ProvinceRepository.Any(x => x.Title == Title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddProvince(AddProvinceModel model)
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

                //var Province = _mapper.Map<Province>(model);
                var Province = new Province
                {
                    Code = model.Code,
                    Title = model.Title,
                    CountryId = model.CountryId,
                };

                Province.InsertDate = DateTime.Now;
                Province.InsertUser = model.CurrentUserName;

                await _kscHrUnitOfWork.ProvinceRepository.AddAsync(Province);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public async Task<KscResult> UpdateProvince(AddProvinceModel model)
        {
            var result = model.IsValid();

            try
            {
                if (!result.Success)
                    return result;
                var oneProvince = GetOne(model.Id);
                if (oneProvince == null)
                {
                    result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                    return result;
                }
                oneProvince.Code = model.Code;
                oneProvince.Title = model.Title;
                oneProvince.UpdateDate = DateTime.Now;
                oneProvince.UpdateUser = model.CurrentUserName;

                _kscHrUnitOfWork.ProvinceRepository.Update(oneProvince);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

            }


            return result;

        }
        public KscResult RemoveProvince(EditProvinceModel model)
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




        public FilterResult<ProvinceModel> GetProvinces(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<Province>(_kscHrUnitOfWork.ProvinceRepository.GetAllData(), Filter, "Id");
            // var data = _mapper.Map<List<ProvinceModel>>(result.Data.ToList());
            var data = result.Data.Select(x => new ProvinceModel { Country = x.Country.Title, Code = x.Code, CountryId = x.CountryId, Title = x.Title, Id = x.Id });

            return new FilterResult<ProvinceModel>()
            {
                Data = data,
                Total = result.Total

            };
        }





        public Province GetOne(int id)
        {
            return _kscHrUnitOfWork.ProvinceRepository.FirstOrDefault(a => a.Id == id);
        }

        public AddProvinceModel GetForEdit(int id)
        {
            var model = GetOne(id);
            var data = new AddProvinceModel();
            data.Id = id;
            data.Code = model.Code;
            data.Title = model.Title;
            data.CountryId = (model.CountryId == null) ? 0 : model.CountryId.Value;

            return data;
        }

        public List<SearchProvinceModel> GetProvincesByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<Province>(_kscHrUnitOfWork.ProvinceRepository.GetAllQueryable().AsQueryable(), Filter, "Id");
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

        public List<SearchProvinceModel> GetProvincesByKendoFilterBycountryId(SearchProvinceByCountryId Filter)
        {
            var result = _FilterHandler.GetFilterResult<Province>(_kscHrUnitOfWork.ProvinceRepository.GetAllQueryable()
                .Where(x => x.CountryId == Filter.CountryId)
                .AsQueryable(), Filter, "Id");
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
