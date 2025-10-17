using AutoMapper;
using Ksc.Hr.Application.Interfaces.Personal;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.Hr.Domain.Entities.Personal;
using KSC.Common.Filters.Models;
using KSC.Common.Filters.Contracts;
using KSC.Common;
using System.Linq;
using System.Collections.Generic;
using System;
using Ksc.HR.Domain.Repositories;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.MonthTimeShitStepper;
using Ksc.HR.DTO.Personal.MonthTimeSheet;

namespace Ksc.Hr.Application.Services.Personal
{
    public class MonthTimeShitStepperService : IMonthTimeShitStepperService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public MonthTimeShitStepperService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public async Task<KscResult> AddMonthTimeShitStepper(AddOrEditMonthTimeShitStepperModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            if (ExistsByTitle(model.Label) == true)
            {

                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            //if (ExistsByOrderNo(model.OrderNo) == true)
            //{

            //    result.AddError("رکورد نامعتبر", "اولویت وارد شده موجود می باشد");
            //    return result;
            //}
            if (model.OrderNo == 0)
            {
                result.AddError("رکورد نامعتبر", "اولویت وارد شده نباید صفر باشد");
                return result;
            }
            if (model.IsShowDetails && string.IsNullOrEmpty(model.FunctionDetails))
            {
                result.AddError("رکورد نامعتبر", "فانکشن نمایش جزییات بادی مقدار داشته باشد");
                return result;
            }
            //
            var MonthTimeShitStepperObj = _mapper.Map<MonthTimeShitStepper>(model);
            await _kscHrUnitOfWork.MonthTimeShitStepperRepository.AddAsync(MonthTimeShitStepperObj);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public bool ExistsByTitle(int id, string name)
        {
            return _kscHrUnitOfWork.MonthTimeShitStepperRepository.Any(x => x.Id != id && x.Label == name);
        }

        public bool ExistsByTitle(string name)
        {
            return _kscHrUnitOfWork.MonthTimeShitStepperRepository.Any(x => x.Label == name);
        }
        public bool ExistsByOrderNo(int orderNo)
        {
            return _kscHrUnitOfWork.MonthTimeShitStepperRepository.Any(x => x.OrderNo == orderNo);
        }
        public bool ExistsByOrderNo(int id, int orderNo)
        {
            return _kscHrUnitOfWork.MonthTimeShitStepperRepository.Any(x => x.Id != id && x.OrderNo == orderNo);
        }

        public AddOrEditMonthTimeShitStepperModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<AddOrEditMonthTimeShitStepperModel>(model);
        }

        public MonthTimeShitStepper GetOne(int id)
        {
            return _kscHrUnitOfWork.MonthTimeShitStepperRepository.GetById(id);

        }

        public List<MonthTimeShitStepperViewModel> GetMonthTimeShitStepper()
        {
            var MonthTimeShitSteppers = _kscHrUnitOfWork.MonthTimeShitStepperRepository.GetAllQueryable();
            return _mapper.Map<List<MonthTimeShitStepperViewModel>>(MonthTimeShitSteppers);

        }

        public FilterResult<MonthTimeShitStepperViewModel> GetMonthTimeShitStepperByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<MonthTimeShitStepper>(_kscHrUnitOfWork.MonthTimeShitStepperRepository.GetAllQueryable(), Filter, "Id");
            var MonthTimeShitStepperFiltered = result.Data.Select(a => new MonthTimeShitStepperViewModel()
            {
                Id = a.Id,
                Label = a.Label,
                OrderNo = a.OrderNo,
                Action = a.Action,
                IsActive = a.IsActive,
                FunctionDetails = a.FunctionDetails,
                IsShowDetails = a.IsShowDetails,
                IsRequiredForMonthSheet = a.IsRequiredForMonthSheet
            }).ToList();
            return new FilterResult<MonthTimeShitStepperViewModel>()
            {
                Data = MonthTimeShitStepperFiltered,
                Total = result.Total
            };
        }

        public List<SearchMonthTimeShitStepperModel> GetMonthTimeShitStepperByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<MonthTimeShitStepper>(_kscHrUnitOfWork.MonthTimeShitStepperRepository.GetAllQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchMonthTimeShitStepperModel()
            {
                Id = a.Id,
                Label = a.Label,
                OrderNo = a.OrderNo,
                Action = a.Action,
                IsActive = a.IsActive
            }).ToList();
            return _mapper.Map<List<SearchMonthTimeShitStepperModel>>(finalData);
        }

        public async Task<KscResult> RemoveMonthTimeShitStepper(AddOrEditMonthTimeShitStepperModel model)
        {
            var result = new KscResult();
            var item = GetOne(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            item.IsActive = false;
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateMonthTimeShitStepper(AddOrEditMonthTimeShitStepperModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneMonthTimeShitStepper = GetOne(model.Id);
            if (oneMonthTimeShitStepper == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            //
            if (ExistsByTitle(model.Id, model.Label) == true)
            {
                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            //if (ExistsByOrderNo(model.Id, model.OrderNo) == true)
            //{
            //    result.AddError("رکورد نامعتبر", "اولویت وارد شده موجود می باشد");
            //    return result;
            //}
            if (model.OrderNo == 0)
            {
                result.AddError("رکورد نامعتبر", "اولویت وارد شده نباید صفر باشد");
                return result;
            }
            if (model.IsShowDetails && string.IsNullOrEmpty(model.FunctionDetails))
            {
                result.AddError("رکورد نامعتبر", "فانکشن نمایش جزییات بادی مقدار داشته باشد");
                return result;
            }
            //
            oneMonthTimeShitStepper.Label = model.Label;
            oneMonthTimeShitStepper.OrderNo = model.OrderNo;
            oneMonthTimeShitStepper.Action = model.Action;
            oneMonthTimeShitStepper.IsActive = model.IsActive;
            oneMonthTimeShitStepper.IsShowDetails = model.IsShowDetails;
            oneMonthTimeShitStepper.FunctionDetails = model.FunctionDetails;
            oneMonthTimeShitStepper.IsRequiredForMonthSheet = model.IsRequiredForMonthSheet;
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<MonthTimeShitStepperModel> GetMonthTimeShitStepperActive(string yearMonth)
        {
            var monthTimeShitSteps = _kscHrUnitOfWork.MonthTimeShitStepperRepository.GetActiveMonthTimeShitStepperAsNoTracking().OrderBy(x => x.OrderNo).ToList();
            List<MonthTimeShitStepperModel> model = new List<MonthTimeShitStepperModel>();
            int index = 0;
            int yearMonthInt = int.Parse(yearMonth);
            var monthTimeSheetLog = _kscHrUnitOfWork.MonthTimeSheetLogRepository.GetMonthTimeSheetLogByMonthAsNoTracking(yearMonthInt).ToList();
            int orderNoLog = monthTimeSheetLog.Count() != 0 ? monthTimeSheetLog.OrderByDescending(x => x.Id).First().MonthTimeShitStepper.OrderNo : 0;
            var maxOrderNo = monthTimeShitSteps.Max(x => x.OrderNo);
            var parallelSelected = false;
            foreach (var item in monthTimeShitSteps)
            {
                bool selected = false;
                var monthTimeSheetLogByStepperId = monthTimeSheetLog.FirstOrDefault(x => x.MonthTimeShitStepperId == item.Id);


                if (monthTimeSheetLogByStepperId != null)
                {
                    selected = true;
                }
                else
                {



                    if (orderNoLog != 0 && item.OrderNo == orderNoLog && monthTimeSheetLogByStepperId == null) // موارد موازی
                    {
                        selected = true;
                        parallelSelected = true;
                    }
                    else
                    {
                        if (item.OrderNo != maxOrderNo)
                        {
                            if (item.OrderNo == orderNoLog + 1 && parallelSelected == false)
                            {
                                selected = true;
                            }

                            else
                            {

                                selected = false;
                            }
                        }
                        else
                        {
                            if (item.OrderNo == orderNoLog + 1 && monthTimeSheetLogByStepperId == null)
                                selected = true;
                        }
                    }

                }

                model.Add(new MonthTimeShitStepperModel()
                {
                    label = item.Label,
                    id = item.Id,
                    index = index++,
                    selected = selected,
                    action = item.Action + "(" + item.Id + ")",
                    OrderNo = item.OrderNo,
                    IsComplate = monthTimeSheetLogByStepperId != null,
                    ShowDetailMessage = monthTimeSheetLogByStepperId != null ? monthTimeSheetLogByStepperId.Result : null,
                    IsShowDetails = item.IsShowDetails,
                    IsRequiredForMonthSheet = item.IsRequiredForMonthSheet,
                    FunctionDetails = item.IsShowDetails ? item.FunctionDetails + "(" + yearMonth + ")" : null
                });
            }
            return model;
        }

    }
}


