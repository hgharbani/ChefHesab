using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.TimeShiftSetting;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DNTPersianUtils.Core;
using System.Threading.Tasks;
using Ksc.HR.Share.General;
using Ksc.HR.Resources.OnCall;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using Ksc.HR.Share.Model.ShiftConcept;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class TimeShiftSettingService : ITimeShiftSettingService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public TimeShiftSettingService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, string? durationTimeAfterShiftEndTime)
        {
            return _kscHrUnitOfWork.TimeShiftSettingRepository.Any(x => x.Id != id && x.IsActive == true
            && x.DurationTimeAfterShiftEndTime == durationTimeAfterShiftEndTime);
        }
        public bool Exists(string? durationTimeAfterShiftEndTime)
        {
            return _kscHrUnitOfWork.TimeShiftSettingRepository.Any(x => x.IsActive == true && x.DurationTimeAfterShiftEndTime == durationTimeAfterShiftEndTime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddTimeShiftSetting(AddTimeShiftSettingModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var workCompanySetting = _kscHrUnitOfWork.WorkCompanySettingRepository.GetAllQueryable().AsQueryable().Include(x => x.WorkTimeShiftConcept).ThenInclude(x => x.WorkTime).First(x => x.Id == model.WorkCompanySettingId);
            var shiftSettingFromShiftboard = workCompanySetting.WorkTimeShiftConcept.WorkTime.ShiftSettingFromShiftboard;
            result = IsCanAdd(model, shiftSettingFromShiftboard);
            if (!result.Success)
            {
                return result;
            }
            //
            //
            var timeSheetSetting = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
            model.ShiftEndtTime = model.ShiftEndtTime.ToEnglishNumbers();
            model.ShiftStartTime = model.ShiftStartTime.ToEnglishNumbers();
            model.TotalWorkHourInDay = Utility.GetDurationStartTimeToEndTime(model.ShiftStartTime, model.ShiftEndtTime);
            //if (shiftSettingFromShiftboard)
            //{
            //    if (timeSheetSetting.ForcedOverTimeBasic.ConvertStringToTimeSpan() < model.TotalWorkHourInDay.ConvertStringToTimeSpan())
            //        model.ForcedOverTime = (model.TotalWorkHourInDay.ConvertStringToTimeSpan() - timeSheetSetting.ForcedOverTimeBasic.ConvertStringToTimeSpan()).ToString().Substring(0, 5);
            //}
            if (string.IsNullOrWhiteSpace(model.ForcedOverTime))
            {
                model.ForcedOverTime = null;
            }
            model.BreastfeddingToleranceTime = model.BreastfeddingToleranceTime != null ? model.BreastfeddingToleranceTime.ToEnglishNumbers() : null;
            model.TemprorayOverTimeDuration = model.TemprorayOverTimeDuration != null ? model.TemprorayOverTimeDuration.ToEnglishNumbers() : null;
            model.TemprorayOverTimeDurationInStartShift = model.TemprorayOverTimeDurationInStartShift != null ? model.TemprorayOverTimeDurationInStartShift.ToEnglishNumbers() : null;
            model.MinimumWorkHourInDay = model.MinimumWorkHourInDay != null ? model.MinimumWorkHourInDay.ToEnglishNumbers() : null;
            model.ConditionalAbsenceToleranceTime = model.ConditionalAbsenceToleranceTime != null ? model.ConditionalAbsenceToleranceTime.ToEnglishNumbers() : null;
            model.ValidOverTimeStartTime = model.ValidOverTimeStartTime != null ? model.ValidOverTimeStartTime.ToEnglishNumbers() : null;
            var TimeShiftSetting = _mapper.Map<TimeShiftSetting>(model);
            TimeShiftSetting.InsertDate = DateTime.Now;
            TimeShiftSetting.InsertUser = model.CurrentUserName;
            TimeShiftSetting.DomainName = model.DomainName;
            TimeShiftSetting.IsActive = true;
            if (model.TemporaryRollCallDefinitionStartShift == 0)
                TimeShiftSetting.TemporaryRollCallDefinitionStartShift = null;
            if (model.TemporaryRollCallDefinitionEndShift == 0)
                TimeShiftSetting.TemporaryRollCallDefinitionEndShift = null;
            if (model.TemprorayOverTimeRollCallDefinitionStartShift == 0)
                TimeShiftSetting.TemprorayOverTimeRollCallDefinitionStartShift = null;
            if (model.TemprorayOverTimeRollCallDefinitionEndShift == 0)
                TimeShiftSetting.TemprorayOverTimeRollCallDefinitionEndShift = null;
            _kscHrUnitOfWork.TimeShiftSettingRepository.Add(TimeShiftSetting);
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        private KscResult IsCanAdd(AddTimeShiftSettingModel model, bool shiftSettingFromShiftboard)
        {
            var result = new KscResult();
            if (!model.ValidityStartDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ شروع اعتبار را وارد نمایید");
                return result;
            }
            if (!model.ValidityEndDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ پایان اعتبار را وارد نمایید");
                return result;
            }
            if (model.ValidityStartDate > model.ValidityEndDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع اعتبار نمیتواند از تاریخ پایان اعتبار بزرگتر باشد");
                return result;
            }
            //if (string.IsNullOrEmpty(model.DurationTimeBeforeShiftStartTime))
            //{
            //    result.AddError("رکورد نامعتبر", "مدت زمان قبل از شروع شیف  اجباری می باشد");
            //    return result;
            //}
            //if (string.IsNullOrEmpty(model.DurationTimeAfterShiftEndTime))
            //{
            //    result.AddError("رکورد نامعتبر", "مدت زمان بعد از پایان شیف  اجباری می باشد");
            //    return result;
            //}
            //var DurationTimeBeforeShiftStartTime = int.Parse(model.DurationTimeBeforeShiftStartTime.Replace(":", ""));
            //var DurationTimeAfterShiftEndTime = int.Parse(model.DurationTimeAfterShiftEndTime.Replace(":", ""));
            //if (DurationTimeBeforeShiftStartTime > DurationTimeAfterShiftEndTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  مدت زمان قبل از شروع شیف نمیتواند بزرگتر از مدت زمان بعد از پایان شیف باشد ");
            //    return result;
            //}



            if (model.IsTemporaryTime == false)
            {
                if (string.IsNullOrEmpty(model.ShiftStartTime))
                {
                    result.AddError("رکورد نامعتبر", "ساعت شروع کار  اجباری می باشد");
                    return result;
                }
                if (string.IsNullOrEmpty(model.ShiftEndtTime))
                {
                    result.AddError("رکورد نامعتبر", "ساعت پایان کار اجباری می باشد");
                    return result;
                }
                model.ShiftEndtTime = model.ShiftEndtTime.ToEnglishNumbers();
                model.ShiftStartTime = model.ShiftStartTime.ToEnglishNumbers();
                var ShiftStartTime = int.Parse(model.ShiftStartTime.Replace(":", ""));
                var ShiftEndtTime = int.Parse(model.ShiftEndtTime.Replace(":", ""));
                //if (ShiftStartTime > ShiftEndtTime)
                //{
                //    result.AddError("رکورد نامعتبر", "  ساعت شروع کار نمیتواند بزرگتر ازساعت پایان کار باشد ");
                //    return result;
                //}
                if (!model.ToleranceShiftStartTime.HasValue)
                {
                    result.AddError("رکورد نامعتبر", "فرجه شروع کار  اجباری می باشد");
                    return result;
                }
                if (!model.ToleranceShiftEndTime.HasValue)
                {
                    result.AddError("رکورد نامعتبر", "فرجه پایان کار اجباری می باشد");
                    return result;
                }

                //if (model.ToleranceShiftStartTime > model.ToleranceShiftEndTime)
                //{
                //    result.AddError("رکورد نامعتبر", "  فرجه شروع کار نمیتواند بزرگتر از فرجه پایان کار باشد ");
                //    return result;
                //}
            }
            else
            {
                var workCompanySetting = _kscHrUnitOfWork.WorkCompanySettingRepository.GetWorkCompanySetting(model.WorkCompanySettingId);
                if (workCompanySetting.WorkTimeShiftConcept.ShiftConceptId != EnumShiftConcept.Rest.Id)
                {
                    if (
                    (model.TemporaryRollCallDefinitionStartShift == null || model.TemporaryRollCallDefinitionStartShift == 0)
                    && (model.TemporaryRollCallDefinitionEndShift == null || model.TemporaryRollCallDefinitionEndShift == 0)
                     && (model.TemprorayOverTimeRollCallDefinitionStartShift == null || model.TemprorayOverTimeRollCallDefinitionStartShift == 0)
                      && (model.TemprorayOverTimeRollCallDefinitionEndShift == null || model.TemprorayOverTimeRollCallDefinitionEndShift == 0)
                    )
                    {
                        result.AddError("رکورد نامعتبر", "کد عدم حضور یا کد اضافه کار در ابتدای شیفت یا پایان شیفت باید مقدارد داشته باشد");
                        return result;
                    }

                    if ((model.TemporaryRollCallDefinitionStartShift != null && model.TemporaryRollCallDefinitionStartShift != 0)
                       || (model.TemporaryRollCallDefinitionEndShift != null && model.TemporaryRollCallDefinitionEndShift != 0))
                    {
                        if (string.IsNullOrEmpty(model.ShiftStartTime))
                        {
                            result.AddError("رکورد نامعتبر", "ساعت شروع کار  اجباری می باشد");
                            return result;
                        }
                        if (string.IsNullOrEmpty(model.ShiftEndtTime))
                        {
                            result.AddError("رکورد نامعتبر", "ساعت پایان کار اجباری می باشد");
                            return result;
                        }
                        if (model.ToleranceShiftStartTime == null || model.ToleranceShiftStartTime < 0)
                        {
                            result.AddError("رکورد نامعتبر", "فرجه ساعت شروع حداقل باید صفر باشد");
                            return result;
                        }
                        if (model.ToleranceShiftEndTime == null || model.ToleranceShiftEndTime < 0)
                        {
                            result.AddError("رکورد نامعتبر", "فرجه ساعت پایان حداقل باید صفر باشد");
                            return result;
                        }
                    }
                    if ((model.TemprorayOverTimeRollCallDefinitionStartShift != null && model.TemprorayOverTimeRollCallDefinitionStartShift != 0)
                          )
                    {
                        if (string.IsNullOrEmpty(model.TemprorayOverTimeDurationInStartShift))
                        {
                            result.AddError("رکورد نامعتبر", "مدت زمان اضافه کار در ابتدای شیفت باید مقدار داشته می باشد");
                            return result;
                        }
                    } 
                    if ( (model.TemprorayOverTimeRollCallDefinitionEndShift != null && model.TemprorayOverTimeRollCallDefinitionEndShift != 0))
                    {
                        if (string.IsNullOrEmpty(model.TemprorayOverTimeDuration))
                        {
                            result.AddError("رکورد نامعتبر", "مدت زمان اضافه کار در پایان شیفت باید مقدار داشته می باشد");
                            return result;
                        }
                    }

                }
            }
            return result;
        }
        public async Task<KscResult> UpdateTimeShiftSetting(EditTimeShiftSettingModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneTimeShiftSetting = GetOne(model.Id);
            if (oneTimeShiftSetting == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            result = IsCanEdit(model);
            model.ShiftEndtTime = model.ShiftEndtTime.ToEnglishNumbers();
            model.ShiftStartTime = model.ShiftStartTime.ToEnglishNumbers();
            model.BreastfeddingToleranceTime = model.BreastfeddingToleranceTime != null ? model.BreastfeddingToleranceTime.ToEnglishNumbers() : null;
            model.TemprorayOverTimeDuration = model.TemprorayOverTimeDuration != null ? model.TemprorayOverTimeDuration.ToEnglishNumbers() : null;
            model.TemprorayOverTimeDurationInStartShift = model.TemprorayOverTimeDurationInStartShift != null ? model.TemprorayOverTimeDurationInStartShift.ToEnglishNumbers() : null;
            model.MinimumWorkHourInDay = model.MinimumWorkHourInDay != null ? model.MinimumWorkHourInDay.ToEnglishNumbers() : null;
            model.ConditionalAbsenceToleranceTime = model.ConditionalAbsenceToleranceTime != null ? model.ConditionalAbsenceToleranceTime.ToEnglishNumbers() : null;
            model.ValidOverTimeStartTime = model.ValidOverTimeStartTime != null ? model.ValidOverTimeStartTime.ToEnglishNumbers() : null;
            if (!result.Success)
            {
                return result;
            }
            //
            var timeSheetSetting = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
            model.TotalWorkHourInDay = Utility.GetDurationStartTimeToEndTime(model.ShiftStartTime, model.ShiftEndtTime);

            //if (oneTimeShiftSetting.WorkCompanySetting.WorkTimeShiftConcept.WorkTime.ShiftSettingFromShiftboard)
            //{
            //    if (timeSheetSetting.ForcedOverTimeBasic.ConvertStringToTimeSpan() < model.TotalWorkHourInDay.ConvertStringToTimeSpan())
            //        model.ForcedOverTime = (model.TotalWorkHourInDay.ConvertStringToTimeSpan() - timeSheetSetting.ForcedOverTimeBasic.ConvertStringToTimeSpan()).ToString().Substring(0, 5);
            //}
            if (string.IsNullOrWhiteSpace(model.ForcedOverTime))
            {
                model.ForcedOverTime = null;
            }

            var TimeShiftSetting = _mapper.Map<TimeShiftSetting>(model);
            TimeShiftSetting.UpdateDate = DateTime.Now;
            TimeShiftSetting.UpdateUser = model.CurrentUserName;
            TimeShiftSetting.IsActive = true;
            if (model.TemporaryRollCallDefinitionStartShift == 0)
                TimeShiftSetting.TemporaryRollCallDefinitionStartShift = null;
            if (model.TemporaryRollCallDefinitionEndShift == 0)
                TimeShiftSetting.TemporaryRollCallDefinitionEndShift = null;
            if (model.TemprorayOverTimeRollCallDefinitionStartShift == 0)
                TimeShiftSetting.TemprorayOverTimeRollCallDefinitionStartShift = null;
            if (model.TemprorayOverTimeRollCallDefinitionEndShift == 0)
                TimeShiftSetting.TemprorayOverTimeRollCallDefinitionEndShift = null;
            _kscHrUnitOfWork.TimeShiftSettingRepository.Update(TimeShiftSetting);
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        private KscResult IsCanEdit(EditTimeShiftSettingModel model)
        {
            var result = new KscResult();
            if (!model.ValidityStartDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ شروع اعتبار را وارد نمایید");
                return result;
            }
            if (!model.ValidityEndDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ پایان اعتبار را وارد نمایید");
                return result;
            }
            if (model.ValidityStartDate > model.ValidityEndDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع اعتبار نمیتواند از تاریخ پایان اعتبار بزرگتر باشد");
                return result;
            }


            //if (ShiftStartTime >= ShiftEndtTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  ساعت شروع کار نمیتواند بزرگتر ازساعت پایان کار باشد ");
            //    return result;
            //}

            //if (!model.ToleranceShiftStartTime.HasValue)
            //{
            //    result.AddError("رکورد نامعتبر", "فرجه شروع کار  اجباری می باشد");
            //    return result;
            //}
            //if (!model.ToleranceShiftEndTime.HasValue)
            //{
            //    result.AddError("رکورد نامعتبر", "فرجه پایان کار اجباری می باشد");
            //    return result;
            //}

            //if (model.ToleranceShiftStartTime > model.ToleranceShiftEndTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  فرجه شروع کار نمیتواند بزرگتر از فرجه پایان کار باشد ");
            //    return result;
            //}
            if (model.IsTemporaryTime == false)
            {
                if (string.IsNullOrEmpty(model.ShiftStartTime))
                {
                    result.AddError("رکورد نامعتبر", "ساعت شروع کار  اجباری می باشد");
                    return result;
                }
                if (string.IsNullOrEmpty(model.ShiftEndtTime))
                {
                    result.AddError("رکورد نامعتبر", "ساعت پایان کار اجباری می باشد");
                    return result;
                }
                model.ShiftEndtTime = model.ShiftEndtTime.ToEnglishNumbers();
                model.ShiftStartTime = model.ShiftStartTime.ToEnglishNumbers();
                var ShiftStartTime = int.Parse(model.ShiftStartTime.Replace(":", ""));
                var ShiftEndtTime = int.Parse(model.ShiftEndtTime.Replace(":", ""));
                if (!model.ToleranceShiftStartTime.HasValue)
                {
                    result.AddError("رکورد نامعتبر", "فرجه شروع کار  اجباری می باشد");
                    return result;
                }
                if (!model.ToleranceShiftEndTime.HasValue)
                {
                    result.AddError("رکورد نامعتبر", "فرجه پایان کار اجباری می باشد");
                    return result;
                }

                //if (model.ToleranceShiftStartTime > model.ToleranceShiftEndTime)
                //{
                //    result.AddError("رکورد نامعتبر", "  فرجه شروع کار نمیتواند بزرگتر از فرجه پایان کار باشد ");
                //    return result;
                //}
            }
            else
            {
                var workCompanySetting = _kscHrUnitOfWork.WorkCompanySettingRepository.GetWorkCompanySetting(model.WorkCompanySettingId);
                if (workCompanySetting.WorkTimeShiftConcept.ShiftConceptId != EnumShiftConcept.Rest.Id)
                {
                    if (
                           (model.TemporaryRollCallDefinitionStartShift == null || model.TemporaryRollCallDefinitionStartShift == 0)
                           && (model.TemporaryRollCallDefinitionEndShift == null || model.TemporaryRollCallDefinitionEndShift == 0)
                            && (model.TemprorayOverTimeRollCallDefinitionStartShift == null || model.TemprorayOverTimeRollCallDefinitionStartShift == 0)
                             && (model.TemprorayOverTimeRollCallDefinitionEndShift == null || model.TemprorayOverTimeRollCallDefinitionEndShift == 0)
                           )
                    {
                        result.AddError("رکورد نامعتبر", "کد عدم حضور یا کد اضافه کار در ابتدای شیفت یا پایان شیفت باید مقدارد داشته باشد");
                        return result;
                    }

                    if ((model.TemporaryRollCallDefinitionStartShift != null && model.TemporaryRollCallDefinitionStartShift != 0)
                       || (model.TemporaryRollCallDefinitionEndShift != null && model.TemporaryRollCallDefinitionEndShift != 0))
                    {
                        if (string.IsNullOrEmpty(model.ShiftStartTime))
                        {
                            result.AddError("رکورد نامعتبر", "ساعت شروع کار  اجباری می باشد");
                            return result;
                        }
                        if (string.IsNullOrEmpty(model.ShiftEndtTime))
                        {
                            result.AddError("رکورد نامعتبر", "ساعت پایان کار اجباری می باشد");
                            return result;
                        }
                        if (model.ToleranceShiftStartTime == null || model.ToleranceShiftStartTime < 0)
                        {
                            result.AddError("رکورد نامعتبر", "فرجه ساعت شروع حداقل باید صفر باشد");
                            return result;
                        }
                        if (model.ToleranceShiftEndTime == null || model.ToleranceShiftEndTime < 0)
                        {
                            result.AddError("رکورد نامعتبر", "فرجه ساعت پایان حداقل باید صفر باشد");
                            return result;
                        }
                    }
                    if ((model.TemprorayOverTimeRollCallDefinitionStartShift != null && model.TemprorayOverTimeRollCallDefinitionStartShift != 0)
                         )
                    {
                        if (string.IsNullOrEmpty(model.TemprorayOverTimeDurationInStartShift))
                        {
                            result.AddError("رکورد نامعتبر", "مدت زمان اضافه کار در ابتدای شیفت باید مقدار داشته می باشد");
                            return result;
                        }
                    }
                    if ((model.TemprorayOverTimeRollCallDefinitionEndShift != null && model.TemprorayOverTimeRollCallDefinitionEndShift != 0))
                    {
                        if (string.IsNullOrEmpty(model.TemprorayOverTimeDuration))
                        {
                            result.AddError("رکورد نامعتبر", "مدت زمان اضافه کار در پایان شیفت باید مقدار داشته می باشد");
                            return result;
                        }
                    }
                }
            }

            return result;
        }
        public KscResult RemoveTimeShiftSetting(EditTimeShiftSettingModel model)
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

            item.IsActive = false;
            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;


            _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<TimeShiftSettingModel> GetTimeShiftSetting()
        {
            var cities = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllQueryable();
            return _mapper.Map<List<TimeShiftSettingModel>>(cities);

        }
        public FilterResult<TimeShiftSettingModel> GetTimeShiftSettingByFilter(TimeShiftSettingModel Filter)
        {
            var result = _FilterHandler.GetFilterResult<TimeShiftSetting>(_kscHrUnitOfWork.TimeShiftSettingRepository.GetAllQueryable().AsQueryable().Include(a => a.WorkCompanySetting).Where(a => a.IsActive && a.WorkCompanySettingId == Filter.WorkCompanySettingId).AsQueryable(), Filter, "Id");

            return new FilterResult<TimeShiftSettingModel>()
            {
                Data = _mapper.Map<List<TimeShiftSettingModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }


        public TimeShiftSetting GetOne(int id)
        {
            return _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllQueryable()
                .AsQueryable().Include(x => x.WorkCompanySetting).ThenInclude(x => x.WorkTimeShiftConcept).ThenInclude(x => x.WorkTime)
                .First(a => a.Id == id && a.IsActive);
        }

        public EditTimeShiftSettingModel GetForEdit(int id)
        {
            var data = GetOne(id);
            var model = _mapper.Map<EditTimeShiftSettingModel>(data);
            return model;

        }

        public List<SearchTimeShiftSettingModel> GetWorkByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<TimeShiftSetting>(_kscHrUnitOfWork.TimeShiftSettingRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchTimeShiftSettingModel>>(result.Data.ToList());
        }
        public List<ForcedOverTimeModel> GetDataForcedOverTime(DateTime date)
        {
            var query = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(date);
            var data = query.Select(x => new ForcedOverTimeModel()
            {
                ShiftConceptId = x.WorkCompanySetting.WorkTimeShiftConcept.ShiftConceptId,
                WorkTimeId = x.WorkCompanySetting.WorkTimeShiftConcept.WorkTimeId,
                WorkCityId = x.WorkCompanySetting.WorkCityId,
                ForcedOverTime = x.ForcedOverTime,
                TotalWorkHourInWeek = x.TotalWorkHourInWeek
            }).ToList();
            return data;
        }
        private KscResult IsCanAdd1(AddTimeShiftSettingModel model)
        {
            var result = new KscResult();
            if (!model.ValidityStartDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ شروع اعتبار را وارد نمایید");
                return result;
            }
            if (!model.ValidityEndDate.HasValue)
            {
                result.AddError("رکورد نامعتبر", "لطفا تاریخ پایان اعتبار را وارد نمایید");
                return result;
            }
            if (model.ValidityStartDate > model.ValidityEndDate)
            {
                result.AddError("رکورد نامعتبر", " تاریخ شروع اعتبار نمیتواند از تاریخ پایان اعتبار بزرگتر باشد");
                return result;
            }
            //if (string.IsNullOrEmpty(model.DurationTimeBeforeShiftStartTime))
            //{
            //    result.AddError("رکورد نامعتبر", "مدت زمان قبل از شروع شیف  اجباری می باشد");
            //    return result;
            //}
            //if (string.IsNullOrEmpty(model.DurationTimeAfterShiftEndTime))
            //{
            //    result.AddError("رکورد نامعتبر", "مدت زمان بعد از پایان شیف  اجباری می باشد");
            //    return result;
            //}
            //var DurationTimeBeforeShiftStartTime = int.Parse(model.DurationTimeBeforeShiftStartTime.Replace(":", ""));
            //var DurationTimeAfterShiftEndTime = int.Parse(model.DurationTimeAfterShiftEndTime.Replace(":", ""));
            //if (DurationTimeBeforeShiftStartTime > DurationTimeAfterShiftEndTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  مدت زمان قبل از شروع شیف نمیتواند بزرگتر از مدت زمان بعد از پایان شیف باشد ");
            //    return result;
            //}


            if (string.IsNullOrEmpty(model.ShiftStartTime))
            {
                result.AddError("رکورد نامعتبر", "ساعت شروع کار  اجباری می باشد");
                return result;
            }
            if (string.IsNullOrEmpty(model.ShiftEndtTime))
            {
                result.AddError("رکورد نامعتبر", "ساعت پایان کار اجباری می باشد");
                return result;
            }
            model.ShiftEndtTime = model.ShiftEndtTime.ToEnglishNumbers();
            model.ShiftStartTime = model.ShiftStartTime.ToEnglishNumbers();
            var ShiftStartTime = int.Parse(model.ShiftStartTime.Replace(":", ""));
            var ShiftEndtTime = int.Parse(model.ShiftEndtTime.Replace(":", ""));
            //if (ShiftStartTime > ShiftEndtTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  ساعت شروع کار نمیتواند بزرگتر ازساعت پایان کار باشد ");
            //    return result;
            //}
            if (model.IsTemporaryTime == false)
            {
                if (!model.ToleranceShiftStartTime.HasValue)
                {
                    result.AddError("رکورد نامعتبر", "فرجه شروع کار  اجباری می باشد");
                    return result;
                }
                if (!model.ToleranceShiftEndTime.HasValue)
                {
                    result.AddError("رکورد نامعتبر", "فرجه پایان کار اجباری می باشد");
                    return result;
                }

                //if (model.ToleranceShiftStartTime > model.ToleranceShiftEndTime)
                //{
                //    result.AddError("رکورد نامعتبر", "  فرجه شروع کار نمیتواند بزرگتر از فرجه پایان کار باشد ");
                //    return result;
                //}
            }
            else
            {
                var temporaryTimeShiftSettingStart = _kscHrUnitOfWork.TimeShiftSettingRepository.GetTemporaryTimeByIncludedAsNotracking(model.ValidityStartDate.Value.Date).FirstOrDefault(x => x.WorkCompanySettingId == model.WorkCompanySettingId);
                if (temporaryTimeShiftSettingStart != null)
                {
                    result.AddError("رکورد نامعتبر", "برای این تاریخ زمان موقت تعریف شده است");
                    return result;
                }

                if ((model.TemporaryRollCallDefinitionStartShift == null || model.TemporaryRollCallDefinitionStartShift == 0) && (model.TemporaryRollCallDefinitionEndShift == null || model.TemporaryRollCallDefinitionEndShift == 0))
                {
                    result.AddError("رکورد نامعتبر", "کد حضور-غیاب در ابتدای شیفت یا پایان شیفت باید مقدارد داشته باشد");
                    return result;
                }
                var workCompanySetting = _kscHrUnitOfWork.WorkCompanySettingRepository.GetWorkCompanySetting(model.WorkCompanySettingId);
                if (model.TemporaryRollCallDefinitionStartShift != null && model.TemporaryRollCallDefinitionStartShift != 0)
                {
                    var timeShiftSettingStart = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(model.ValidityStartDate.Value.Date).FirstOrDefault(x => x.WorkCompanySettingId == model.WorkCompanySettingId);
                    if (timeShiftSettingStart == null)
                    {
                        result.AddError("رکورد نامعتبر", "برای این تاریخ تنظیمات شیفت تعریف نشده است");
                        return result;
                    }
                    if (model.ShiftStartTime.ConvertStringToTimeSpan() < timeShiftSettingStart.ShiftStartTime.ConvertStringToTimeSpan())
                    {
                        result.AddError("رکورد نامعتبر", $"ساعت شروع نباید کوچکتر از {timeShiftSettingStart.ShiftStartTime} باشد");
                        return result;
                    }
                    if (model.ShiftEndtTime.ConvertStringToTimeSpan() > timeShiftSettingStart.ShiftEndtTime.ConvertStringToTimeSpan())
                    {
                        result.AddError("رکورد نامعتبر", $"ساعت پایان نباید بزرگتر از {timeShiftSettingStart.ShiftEndtTime} باشد");
                        return result;
                    }
                    var rollCallWorkTimeDayType = _kscHrUnitOfWork.RollCallWorkTimeDayTypeRepository.GetRollCallWorkTimeDayTypeByRollCallDefinitionId(model.TemporaryRollCallDefinitionStartShift.Value).ToList();
                    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.MiladiDateV1 <= model.ValidityStartDate.Value.Date
                     && x.MiladiDateV1 >= model.ValidityEndDate.Value.Date
                     ).ToList();
                    foreach (var item in workCalendar)
                    {
                        if (!rollCallWorkTimeDayType.Any(x => x.WorkDayTypeId == item.WorkDayTypeId
                        || x.WorkTimeId != workCompanySetting.WorkTimeShiftConcept.WorkTimeId))

                        {
                            result.AddError("رکورد نامعتبر", "کد حضور-غیاب در ابتدای شیفت ،برای این نوع زمان کاری و روزکاری تعریف نشده است");
                            return result;
                        }
                    }
                }
                if (model.TemporaryRollCallDefinitionEndShift != null && model.TemporaryRollCallDefinitionEndShift != 0)
                {

                    var timeShiftSettingEnd = _kscHrUnitOfWork.TimeShiftSettingRepository.GetAllByIncludedAsNotracking(model.ValidityEndDate.Value.Date).FirstOrDefault(x => x.WorkCompanySettingId == model.WorkCompanySettingId);
                    if (timeShiftSettingEnd == null)
                    {
                        result.AddError("رکورد نامعتبر", "برای این تاریخ تنظیمات شیفت تعریف نشده است");
                        return result;
                    }
                    if (model.ShiftStartTime.ConvertStringToTimeSpan() < timeShiftSettingEnd.ShiftStartTime.ConvertStringToTimeSpan())
                    {
                        result.AddError("رکورد نامعتبر", "ساعت شروع نباید کوچکتر از {timeShiftSettingStart.ShiftStartTime} باشد");
                        return result;
                    }
                    if (model.ShiftEndtTime.ConvertStringToTimeSpan() > timeShiftSettingEnd.ShiftEndtTime.ConvertStringToTimeSpan())
                    {
                        result.AddError("رکورد نامعتبر", "ساعت پایان نباید بزرگتر از {timeShiftSettingStart.ShiftEndtTime} باشد");
                        return result;
                    }

                    var rollCallWorkTimeDayType = _kscHrUnitOfWork.RollCallWorkTimeDayTypeRepository.GetRollCallWorkTimeDayTypeByRollCallDefinitionId(model.TemporaryRollCallDefinitionEndShift.Value).ToList();
                    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.Where(x => x.MiladiDateV1 <= model.ValidityStartDate.Value.Date
                     && x.MiladiDateV1 >= model.ValidityEndDate.Value.Date
                     ).ToList();
                    foreach (var item in workCalendar)
                    {
                        if (!rollCallWorkTimeDayType.Any(x => x.WorkDayTypeId == item.WorkDayTypeId
                        || x.WorkTimeId != workCompanySetting.WorkTimeShiftConcept.WorkTimeId))

                        {
                            result.AddError("رکورد نامعتبر", "کد حضور-غیاب در پایان شیفت ،برای این نوع زمان کاری و روزکاری تعریف نشده است");
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public int GetTotalWorkHourInDayForRequestSystem(string month)
        {
            var worktimeId = 1;
            var shiftConceptId = 1;
            var timeShiftSettingByWorkCityIdModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetTimeShiftSettingByWorkCityId(1).ToList(); //اهواز
            int sumWorkDayTime = 0;
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarByYearMonthAsNoTracking(int.Parse(month))
                 .Where(x => x.WorkDayTypeId == EnumWorkDayType.NormalDay.Id).Select(x => x.MiladiDateV1).ToList();
            var timeShiftSettingData = timeShiftSettingByWorkCityIdModel
                      .Where(x => x.WorktimeId == worktimeId && x.ShiftConceptId == shiftConceptId).ToList();
            foreach (var date in workCalendar)
            {
                var timeShiftSettingTemporary = timeShiftSettingData.FirstOrDefault(x => x.IsTemporaryTime == true
                                        && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date
                                        && (x.TemporaryRollCallDefinitionStartShift != null || x.TemporaryRollCallDefinitionEndShift != null)
                                        );
                if (timeShiftSettingTemporary != null)
                {
                    var duarion = Utility.GetDurationStartTimeToEndTime(timeShiftSettingTemporary.ShiftStartTime, timeShiftSettingTemporary.ShiftEndtTime);
                    var minute = duarion.ConvertDurationToMinute() != null ? duarion.ConvertDurationToMinute().Value : 0;
                    sumWorkDayTime += minute;
                }
                else
                {
                    var timeShiftSetting = timeShiftSettingData.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
                    if (timeShiftSetting != null)
                        sumWorkDayTime += timeShiftSetting.TotalWorkHourInDay.ConvertDurationToMinute().Value;
                }
            }
            return sumWorkDayTime;


        }
    }
}
