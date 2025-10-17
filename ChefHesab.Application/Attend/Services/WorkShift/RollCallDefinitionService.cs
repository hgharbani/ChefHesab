using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkShift;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using Ksc.HR.DTO.WorkShift.RollCallSalaryCode;
using Ksc.HR.DTO.WorkShift.RollCallWorkTimeDayType;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using Ksc.HR.Share.General;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.DTO.WorkShift.RollCallWorkCity;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class RollCallDefinitionService : IRollCallDefinitionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        //  private readonly IRollCallDefinitionRepository _rollCallDefinitionRepository;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public RollCallDefinitionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            //_rollCallDefinitionRepository = rollCallDefinitionRepository;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public List<SearchRollCallDefinicationModel> GetAllRollCallDefinicationRollCallDefinicationByModel(SearchRollCallDefinicationModel Filter)
        {
            var rollCallDefinicationModelQuery = _kscHrUnitOfWork.RollCallDefinitionRepository.GetIncludedRollCallEmploymentTypes().Where(a => a.IsActive);
            if (Filter.RollCallCategoryId.HasValue)
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallCategoryId == Filter.RollCallCategoryId).AsQueryable();
            if (Filter.EmploymentTypeCodeId.HasValue)
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallEmploymentTypes.Any(r => r.EmploymentTypeCode == Filter.EmploymentTypeCodeId || a.IsValidForAllEmploymentType));
            if (Filter.ShiftConceptDetailId.HasValue)
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallConceptId == Filter.ShiftConceptDetailId.Value);
            }
            var query = rollCallDefinicationModelQuery.Select(a => new SearchRollCallDefinicationModel
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                RollCallCategoryId = a.RollCallCategoryId,
                IsIncluded = a.IncludedRollCalls.Any(c => c.IncludedDefinitionId == 1),
            });
            var result = _FilterHandler.GetFilterResult<SearchRollCallDefinicationModel>(query, Filter, nameof(RollCallDefinition.Id));

            return _mapper.Map<List<SearchRollCallDefinicationModel>>(result.Data);
        }



        public FilterResult<RollCallDefinicationModel> GetRollCallDefinicationByModel(SearchRollCallDefinicationModel Filter)
        {

            var rollCallDefinicationModelQuery = _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllIncluded().Where(a => a.IsActive).AsNoTracking();
            if (Filter.RollCallCategoryId.HasValue)
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallCategoryId == Filter.RollCallCategoryId);
            if (Filter.EmploymentTypeCodeId.HasValue)
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.IsValidForAllEmploymentType == true || a.RollCallEmploymentTypes.Any(r => r.EmploymentTypeCode == Filter.EmploymentTypeCodeId));
            if (Filter.ShiftConceptDetailId.HasValue)
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallConceptId == Filter.ShiftConceptDetailId.Value);
            }

            var query = rollCallDefinicationModelQuery.Select(a => new RollCallDefinicationModel
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                RollCallCategoryId = a.RollCallCategoryId,
                ValidityStartDate = a.ValidityStartDate,
                ValidityEndDate = a.ValidityEndDate,
                RollCallCategoryTitle = a.RollCallCategory != null ? a.RollCallCategory.Title : "",
                RollCallConceptTitle = a.RollCallConcept.Title,
                IsValidForAllWorkTimeDayType = a.IsValidForAllWorkTimeDayType,
                LongTermAbsenceCheck = a.LongTermAbsenceCheck
            });
            //query.Add(new RollCallDefinicationModel()
            //{
            //    Code = "0",
            //    Title = "همه موارد"
            //});
            query = query.OrderBy(a => a.Code);
            var result = _FilterHandler.GetFilterResult<RollCallDefinicationModel>(query, Filter, nameof(RollCallDefinition.Id));
            var modelResult = new FilterResult<RollCallDefinicationModel>
            {
                Data = _mapper.Map<List<RollCallDefinicationModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        public List<SearchRollCallDefinicationModel> GeTemporaryRollCallDefinition(RollCallDefinicationFilterRequest Filter)
        {

            var rollCallDefinicationModelQuery = _kscHrUnitOfWork.RollCallDefinitionRepository.WhereQueryable(a => a.IsActive);
            if (Filter.IsValidForTemporaryStartDate)
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(x => x.IsValidForTemporaryStartDate);
            }
            else
            {
                if (Filter.IsValidForTemporaryEndDate)
                {
                    rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(x => x.IsValidForTemporaryEndDate);
                }
            }
            var query = rollCallDefinicationModelQuery.Select(a => new SearchRollCallDefinicationModel
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                RollCallCategoryId = a.RollCallCategoryId,
                ValidityStartDate = a.ValidityStartDate,
                ValidityEndDate = a.ValidityEndDate,
                IsValidForTemporaryEndDate = a.IsValidForTemporaryEndDate,
                IsValidForTemporaryStartDate = a.IsValidForTemporaryStartDate,
                RollCallConceptId = a.RollCallConceptId
            }).AsQueryable();
            var result = _FilterHandler.GetFilterResult<SearchRollCallDefinicationModel>(query, Filter, nameof(RollCallDefinition.Id));
            List<SearchRollCallDefinicationModel> list = new List<SearchRollCallDefinicationModel>();
            list.Add(new SearchRollCallDefinicationModel() { });
            list.AddRange(result.Data);
            return _mapper.Map<List<SearchRollCallDefinicationModel>>(list);
        }

        public FilterResult<SearchRollCallDefinicationModel> GetRollCallDefinicationForAnalysAsync(EmployeeAttendAbsenceAnalysisModel Filter)
        {
            var employeeeDetail = _kscHrUnitOfWork.EmployeeRepository.GetById(Filter.EmployeeId);

            var rollCallDefinicationModelQuery = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByIncludedForAttendAbsence()
                .Where(a => a.IsActive && (a.GenderTypeId == null || a.GenderTypeId == employeeeDetail.Gender)).AsNoTracking();
            if (Filter.ModifyIsValid == false)
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.Id == Filter.RollCallDefinitionId);
                var firstCode = rollCallDefinicationModelQuery.Select(a => new SearchRollCallDefinicationModel
                {
                    Id = a.Id,
                    Code = a.Code,
                    Title = a.Title,
                    RollCallCategoryId = a.RollCallCategoryId,
                    ValidityStartDate = a.ValidityStartDate,
                    ValidityEndDate = a.ValidityEndDate,
                    //LongTermAbsenceCheck = a.LongTermAbsenceCheck
                }).AsQueryable();
                var firstCoderesult = _FilterHandler.GetFilterResult<SearchRollCallDefinicationModel>(firstCode, Filter, "Code");
                var firstCodemodelResult = new FilterResult<SearchRollCallDefinicationModel>
                {
                    Data = _mapper.Map<List<SearchRollCallDefinicationModel>>(firstCoderesult.Data),
                    Total = firstCoderesult.Total
                };
                return firstCodemodelResult;
            }
            rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallConceptId == Filter.RollCallConceptId);
            if (Filter.RollCallDefinitionId == 0) // هیچ کد حضور-غیاب نداشته باشد
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.IsValidForDailyAbcenseInAnalyz);
            }
            //   rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallConceptId == Filter.RollCallConceptId && a.IsValidForDailyAbcenseInAnalyz);
            var workDaytype = _kscHrUnitOfWork.WorkCalendarRepository.GetById(Filter.WokCalendarId);
            rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(x => x.ValidityStartDate.Value.Date <= workDaytype.MiladiDateV1 && x.ValidityEndDate.Value.Date >= workDaytype.MiladiDateV1);

            if (employeeeDetail.EmploymentTypeId > 0)
            {
                //var employeeeTypecode = _kscHrUnitOfWork.ViewMisEmploymentTypeRepository.GetEmployeTypeCode(employeeeDetail.EmploymentTypeId.Value);
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(x => x.IsValidForAllEmploymentType
                || x.RollCallEmploymentTypes.Any(a => a.EmploymentTypeCode == employeeeDetail.EmploymentTypeId.Value));

            }
            var compatibaleRollCall = _kscHrUnitOfWork.CompatibleRollCallRepository.Where(a => a.CompatibleRollCallType == 2 && a.RollCallDefinitionId == Filter.RollCallDefinitionId).Select(a => a.CompatibleRollCallId);
            if (compatibaleRollCall.Any())
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => compatibaleRollCall.Contains(a.Id));
            }
            else
            {
                if (Filter.RollCallDefinitionId != 0)
                {
                    rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => compatibaleRollCall.Any());
                }
            }
            //  var workTimeEmployee = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup().First(a => a.EmployeeId == Filter.EmployeeId);
            var workTimeEmployee = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupByEmployeeIdDateIncludeByWorkGroup(Filter.EmployeeId, workDaytype.MiladiDateV1);

            rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(x => x.IsValidForAllWorkTimeDayType == true ||
                                                                                       x.RollCallWorkTimeDayTypes.Any(y => y.WorkTimeId == workTimeEmployee.WorkGroup.WorkTimeId
                                                                                                                            && y.WorkDayTypeId == workDaytype.WorkDayTypeId));


            if (Filter.RolesForUser.Any() == false)
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.AccessLevelId == 1);
            }
            else
            {
                rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => Filter.RolesForUser.Contains(a.AccessLevel.RoleInIdentityService) || a.AccessLevelId == 1);
            }
            var userDetail = _kscHrUnitOfWork.ViewMisEmployeeRepository.FirstOrDefault(a => a.WinUser == Filter.CurentUserName);
            if (userDetail != null)
            {
                var isTmMa = userDetail.JobCategoryCode == "TM" || userDetail.JobCategoryCode == "MA" || Filter.IsEntryExit;
                if (isTmMa == true)
                {
                    rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallJobCategories.Any(c => c.CodeCategoryJobCategory == "TM" || c.CodeCategoryJobCategory == "MA") || a.IsValidForAllCategoryCode);
                }
                else
                {
                    rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(a => a.RollCallJobCategories.Any(c => c.CodeCategoryJobCategory == userDetail.JobCategoryCode) || a.IsValidForAllCategoryCode);

                }
            }
            // بررسی شهرمحل کار
            rollCallDefinicationModelQuery = rollCallDefinicationModelQuery.Where(x => x.RollCallWorkCities.Count() == 0
            || x.RollCallWorkCities.Any(w => w.IsActive && w.WorkCityId == employeeeDetail.WorkCityId
            && w.StartDate.Date <= workDaytype.MiladiDateV1 && w.EndDate.Date >= workDaytype.MiladiDateV1));
            //
            var query = rollCallDefinicationModelQuery.Select(a => new SearchRollCallDefinicationModel
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                RollCallCategoryId = a.RollCallCategoryId,
                ValidityStartDate = a.ValidityStartDate,
                ValidityEndDate = a.ValidityEndDate,
                //LongTermAbsenceCheck = a.LongTermAbsenceCheck
            }).AsQueryable();
            //

            if (Filter.RollCallConceptId == EnumRollCallConcept.Absence.Id)
            {
                var conditionalAbsenceRollCall = _kscHrUnitOfWork.ConditionalAbsenceSubjectTypeRepository.GetRollCallDailyAbsence()
                .Select(x => new { ConditionalAbsenceSubjectTypeId = x.Id, RollCallDefinitionId = x.RollCallDefinitionId });
                var conditionalAbsenceDailyRollCall = conditionalAbsenceRollCall
                    .Join(query, x => x.RollCallDefinitionId, m => m.Id, (x, m) => x).FirstOrDefault();
                if (conditionalAbsenceDailyRollCall != null)
                {
                    var employeeConditionalAbsence = _kscHrUnitOfWork.EmployeeConditionalAbsenceRepository.GetAllQueryable()
                        .Any(x => x.EmployeeId == Filter.EmployeeId);
                    if (employeeConditionalAbsence == false)
                    {
                        query = query.Where(x => x.Id != conditionalAbsenceDailyRollCall.RollCallDefinitionId);
                    }
                }
            }

            //
            var result = _FilterHandler.GetFilterResult<SearchRollCallDefinicationModel>(query, Filter, "Code");
            var modelResult = new FilterResult<SearchRollCallDefinicationModel>
            {
                Data = _mapper.Map<List<SearchRollCallDefinicationModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        public async Task<KscResult> AddRollCallDefinication(AddRollCallDefinicationModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            if (await ExistsByTitle(model.Title) == true)
            {
                result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                return result;
            }
            if (await ExistsByCode(model.Code) == true)
            {
                result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
                return result;
            }
            if (model.ValidityMinimumTime.ConvertStringToTimeSpan() > TimeSpan.Parse("23:59") || model.ValidityMinimumTime.ConvertStringToTimeSpan() < TimeSpan.Parse("00:01"))
            {
                result.AddError("رکورد نامعتبر", "حداقل زمان مجاز صحیح نمی باشد");
                return result;
            }
            if (model.ValidityMaximumTime.ConvertStringToTimeSpan() > TimeSpan.Parse("23:59") || model.ValidityMaximumTime.ConvertStringToTimeSpan() < TimeSpan.Parse("00:01"))
            {
                result.AddError("رکورد نامعتبر", "حداکثر زمان مجاز صحیح نمی باشد");
                return result;
            }
            bool IsNotValidateRollCallWorkTimeDayTypeModels = (model.RollCallWorkTimeDayTypeModels.Count == 0 && !model.IsValidForAllWorkTimeDayType);
            if (IsNotValidateRollCallWorkTimeDayTypeModels)
            {
                result.AddError("رکورد نامعتبر", "حداقل یک رکورد روز - زمان کاری وارد نمایید.");
                return result;
            }
            /// نوع کد - اضافه کار باشد - اجباری می شد
            if (model.RollCallSalariesCode.Count() == 0 && (model.RollCallConceptId == EnumRollCallConcept.OverTime.Id))
            {
                result.AddError("رکورد نامعتبر", "حداقل یک رکورد کد های حقوقی  نوع استخدام - کد حقوق را وارد نمایید.");
                return result;
            }
            /// نوع کد - اضافه کار نباشد - نباید مقدار داشته باشد
            //if (model.RollCallSalariesCode.Count() != 0 && (model.RollCallConceptId != EnumRollCallConcept.OverTime.Id))
            //{
            //    result.AddError("رکورد نامعتبر", "برای کدهای غیر از اضافه کار، امکان ثبت  کد های حقوقی  وجود ندارد.");
            //    return result;
            //}
            /// نوع کد - اضافه کار باشد و شامل میانگین باشد، باید مقدار داشته باشد
            if (model.OverTimePriority == null && (model.RollCallConceptId == EnumRollCallConcept.OverTime.Id)
                && model.IncludedRollCallsId.Any(x => x == EnumIncludedDefinition.MaximunOverTime.Id.ToString()))
            {
                result.AddError("رکورد نامعتبر", "ترتیب کسر از مازاد اضافه کاری را وارد نمایید");
                return result;
            }

            if (model.OverTimePriority != null &&
                (model.RollCallConceptId != EnumRollCallConcept.OverTime.Id
                || !model.IncludedRollCallsId.Any(x => x == EnumIncludedDefinition.MaximunOverTime.Id.ToString())))
            {
                result.AddError("رکورد نامعتبر", "ترتیب کسر از مازاد اضافه کاری نباید مقدار داشته باشد");
                return result;
            }
            if (model.RollCallJobCategoriesId == null && !model.IsValidForAllCategoryCode)
            {
                result.AddError("رکورد نامعتبر", "حداقل یک مورد رده شغلی مجاز را وارد نمایید.");
                return result;
            }
            if (model.EmploymentTypeCodesId == null && !model.IsValidForAllEmploymentType)
            {
                result.AddError("رکورد نامعتبر", "نوع استخدام دسترسی به کد را وارد نمایید.");
                return result;
            }

            //if (model.IncludedRollCallsId == null)
            //{
            //    result.AddError("رکورد نامعتبر", "حداقل یک مورد موارد مشمول این کد را وارد نمایید.");
            //    return result;
            //}
            if (model.CompatibleRollCallsId == null)
            {
                result.AddError("رکورد نامعتبر", "حداقل یک مورد کد سازگار را وارد نمایید.");
                return result;
            }
            //
            result = await CheckRollCallWorkCity(model);
            if (!result.Success) return result;
            //
            var rollCallDefinitionObj = _mapper.Map<RollCallDefinition>(model);
            rollCallDefinitionObj.IsActive = true;
            rollCallDefinitionObj.InsertDate = DateTime.Now;
            /// نوع روز کاری 
            if (!model.IsValidForAllWorkTimeDayType)
            {
                foreach (var objRollCallWorkTimeDayType in model.RollCallWorkTimeDayTypeModels)
                {
                    rollCallDefinitionObj.RollCallWorkTimeDayTypes.Add(new RollCallWorkTimeDayType()
                    {
                        WorkTimeId = objRollCallWorkTimeDayType.WorkTimeId,
                        WorkDayTypeId = objRollCallWorkTimeDayType.WorkDayTypeId,
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    });
                }
            }

            //نوع حقوق - نوع استخدام
            if (model.RollCallSalariesCode.Count > 0)
            {
                foreach (var objRollCallSalaryCode in model.RollCallSalariesCode)
                {
                    rollCallDefinitionObj.RollCallSalaryCodes.Add(new RollCallSalaryCode()
                    {
                        EmploymentTypeCode = objRollCallSalaryCode.EmploymentTypeCode,
                        SalaryAccountCode = objRollCallSalaryCode.SalaryAccountCode,
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    });
                }
            }

            // رده شغلی مجاز
            if (model.RollCallJobCategoriesId != null)
            {
                foreach (var rollCallJobCategoryID in model.RollCallJobCategoriesId)
                {
                    rollCallDefinitionObj.RollCallJobCategories.Add(new RollCallJobCategory()
                    {
                        CodeCategoryJobCategory = rollCallJobCategoryID,
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    });
                }
            }

            //نوع استخدام دسترسی به کد
            if (model.EmploymentTypeCodesId != null)
                foreach (string employmentTypeCodeId in model.EmploymentTypeCodesId)
                {
                    rollCallDefinitionObj.RollCallEmploymentTypes.Add(new RollCallEmploymentType()
                    {
                        EmploymentTypeCode = Convert.ToDecimal(employmentTypeCodeId),
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    });
                }
            //موارد مشمول
            if (model.IncludedRollCallsId != null)
            {
                foreach (string includedRollCallDefinicationId in model.IncludedRollCallsId)
                {
                    rollCallDefinitionObj.IncludedRollCalls.Add(new IncludedRollCall()
                    {
                        IncludedDefinitionId = Convert.ToInt32(includedRollCallDefinicationId),
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    });
                }
                if (model.IncludedRollCallsId.Any(x => Convert.ToInt32(x) == EnumIncludedDefinition.MaximunOverTime.Id))
                {
                    rollCallDefinitionObj.IsValidSingleDelete = true;
                }
            }

            // مشمول کسر حقوق
            if (model.RewardDeductionIncludedId > 0)
                rollCallDefinitionObj.IncludedRollCalls.Add(new IncludedRollCall()
                {
                    IncludedDefinitionId = model.RewardDeductionIncludedId,
                    InsertUser = model.InsertUser,
                    InsertDate = DateTime.Now,
                    DomainName = model.DomainName,
                    IsActive = true,
                });
            //  مشمول کسر پاداش
            if (model.SalaryDeductionIncludedId > 0)
                rollCallDefinitionObj.IncludedRollCalls.Add(new IncludedRollCall()
                {
                    IncludedDefinitionId = model.SalaryDeductionIncludedId,
                    InsertUser = model.InsertUser,
                    InsertDate = DateTime.Now,
                    DomainName = model.DomainName,
                    IsActive = true,
                });
            // کد های سازگار
            if (model.CompatibleRollCallsId != null)
            {
                foreach (string compatibleRollCallId in model.CompatibleRollCallsId)
                {
                    rollCallDefinitionObj.CompatibleRollCalls_RollCallDefinitionId.Add(new CompatibleRollCall()
                    {
                        CompatibleRollCallId = Convert.ToInt32(compatibleRollCallId),
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        CompatibleRollCallType = EnumCompatibleRollCallType.Compatible.Id,
                        IsActive = true,
                    });
                }
            }
            // کد های قابل تبدیل
            if (model.InterchangeableRollCallsId != null)
                foreach (string interchangeableRollCallId in model.InterchangeableRollCallsId)
                {
                    rollCallDefinitionObj.CompatibleRollCalls_RollCallDefinitionId.Add(new CompatibleRollCall()
                    {
                        CompatibleRollCallId = Convert.ToInt32(interchangeableRollCallId),
                        InsertUser = model.InsertUser,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        CompatibleRollCallType = EnumCompatibleRollCallType.Interchangeable.Id,
                        IsActive = true,
                    });
                }
            //شهرمحل خدمت
            foreach (var objRollCallWorkCity in model.RollCallWorkCityModels)
            {

                rollCallDefinitionObj.RollCallWorkCities.Add(new RollCallWorkCity()
                {
                    WorkCityId = objRollCallWorkCity.WorkCityId,
                    StartDate = objRollCallWorkCity.StartDate.Date,
                    EndDate = objRollCallWorkCity.EndDate.Date,
                    InsertUser = model.InsertUser,
                    InsertDate = DateTime.Now,
                    DomainName = model.DomainName,
                    IsActive = true,
                });

            }
            //
          //  rollCallDefinitionObj.Code = rollCallDefinitionObj.Id.ToString();
            rollCallDefinitionObj.IsValidForDailyAbcenseInAnalyz = model.IsValidForDailyAbcenseInAnalyz;
            rollCallDefinitionObj.IsValidForDeleteAbsenceItem = model.IsValidForDeleteAbsenceItem;
            rollCallDefinitionObj.Id = int.Parse(model.Code);
            await _kscHrUnitOfWork.RollCallDefinitionRepository.AddAsync(rollCallDefinitionObj);
            await _kscHrUnitOfWork.SaveAsync();
            //
            //var rollCallDefinition = await GetOne(rollCallDefinitionObj.Id);
            //rollCallDefinition.Code = rollCallDefinition.Id.ToString();
            //await _kscHrUnitOfWork.SaveAsync();

            //
            return result;
        }
        public async Task<KscResult> UpdateRollCallDefinication(AddRollCallDefinicationModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try
            {


                if (await ExistsByTitle(model.Id, model.Title) == true)
                {
                    result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
                    return result;
                }
                if (await ExistsByCode(model.Id, model.Code) == true)
                {
                    result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
                    return result;
                }
                if (model.ValidityMinimumTime.ConvertStringToTimeSpan() > TimeSpan.Parse("23:59") || model.ValidityMinimumTime.ConvertStringToTimeSpan() < TimeSpan.Parse("00:01"))
                {
                    result.AddError("رکورد نامعتبر", "حداقل زمان مجاز صحیح نمی باشد");
                    return result;
                }
                if (model.ValidityMaximumTime.ConvertStringToTimeSpan() > TimeSpan.Parse("23:59") || model.ValidityMaximumTime.ConvertStringToTimeSpan() < TimeSpan.Parse("00:01"))
                {
                    result.AddError("رکورد نامعتبر", "حداکثر زمان مجاز صحیح نمی باشد");
                    return result;
                }
                bool IsNotValidateRollCallWorkTimeDayTypeModels = (model.RollCallWorkTimeDayTypeModels.Count == 0 && !model.IsValidForAllWorkTimeDayType);
                if (IsNotValidateRollCallWorkTimeDayTypeModels)
                {
                    result.AddError("رکورد نامعتبر", "حداقل یک رکورد روز - زمان کاری وارد نمایید.");
                    return result;
                }
                /// نوع کد - اضافه کار باشد - اجباری می شد
                if (model.RollCallSalariesCode.Count() == 0 && (model.RollCallConceptId == EnumRollCallConcept.OverTime.Id))
                {
                    result.AddError("رکورد نامعتبر", "حداقل یک رکورد کد های حقوقی  نوع استخدام - کد حقوق را وارد نمایید.");
                    return result;
                }
                /// نوع کد - اضافه کار نباشد - نباید مقدار داشته باشد
                //if (model.RollCallSalariesCode.Count() != 0 && (model.RollCallConceptId != EnumRollCallConcept.OverTime.Id))
                //{
                //    result.AddError("رکورد نامعتبر", "برای کدهای غیر از اضافه کار، امکان ثبت  کد های حقوقی  وجود ندارد.");
                //    return result;
                //}
                /// نوع کد - اضافه کار باشد و شامل میانگین باشد، باید مقدار داشته باشد
                if (model.OverTimePriority == null && (model.RollCallConceptId == EnumRollCallConcept.OverTime.Id)
                    && model.IncludedRollCallsId.Any(x => x == EnumIncludedDefinition.MaximunOverTime.Id.ToString()))
                {
                    result.AddError("رکورد نامعتبر", "ترتیب کسر از مازاد اضافه کاری را وارد نمایید");
                    return result;
                }
                if (model.OverTimePriority != null &&
                    (model.RollCallConceptId != EnumRollCallConcept.OverTime.Id
                    || !model.IncludedRollCallsId.Any(x => x == EnumIncludedDefinition.MaximunOverTime.Id.ToString())))
                {
                    result.AddError("رکورد نامعتبر", "ترتیب کسر از مازاد اضافه کاری نباید مقدار داشته باشد");
                    return result;
                }
                if (model.RollCallJobCategoriesId == null && !model.IsValidForAllCategoryCode)
                {
                    result.AddError("رکورد نامعتبر", "حداقل یک مورد رده شغلی مجاز را وارد نمایید.");
                    return result;
                }
                if (model.EmploymentTypeCodesId == null && !model.IsValidForAllEmploymentType)
                {
                    result.AddError("رکورد نامعتبر", "نوع استخدام دسترسی به کد را وارد نمایید.");
                    return result;
                }

                //if (model.IncludedRollCallsId == null)
                //{
                //    result.AddError("رکورد نامعتبر", "حداقل یک مورد موارد مشمول این کد را وارد نمایید.");
                //    return result;
                //}
                if (model.CompatibleRollCallsId == null)
                {
                    result.AddError("رکورد نامعتبر", "حداقل یک مورد کد سازگار را وارد نمایید.");
                    return result;
                }
                result = await CheckRollCallWorkCity(model);
                if (!result.Success) return result;
                //
                var objRollCallDefinication = await GetOne(model.Id);
                objRollCallDefinication.AccessLevelId = model.AccessLevelId;
                objRollCallDefinication.InsertCodeIsAutomatic = model.InsertCodeIsAutomatic;
                objRollCallDefinication.RollCallCategoryId = model.RollCallCategoryId;
                objRollCallDefinication.RollCallConceptId = model.RollCallConceptId;
                //objRollCallDefinication.IsActive = model.IsActive = true;
                objRollCallDefinication.IsValidForAllCategoryCode = model.IsValidForAllCategoryCode;
                objRollCallDefinication.IsValidForAllEmploymentType = model.IsValidForAllEmploymentType;
                objRollCallDefinication.IsValidForAllWorkTimeDayType = model.IsValidForAllWorkTimeDayType;
                //
                objRollCallDefinication.IsValidForTemporaryStartDate = model.IsValidForTemporaryStartDate;
                objRollCallDefinication.IsValidForTemporaryEndDate = model.IsValidForTemporaryEndDate;
                //
                objRollCallDefinication.TimesAllowedUsePerDay = model.TimesAllowedUsePerDay;
                objRollCallDefinication.TimesAllowedUsePerWeek = model.TimesAllowedUsePerWeek;

                objRollCallDefinication.TimesAllowedUsePerMonth = model.TimesAllowedUsePerMonth;
                objRollCallDefinication.ValidityDayNumberInYear = model.ValidityDayNumberInYear;
                objRollCallDefinication.OverTimePriority = model.OverTimePriority;
                //  objRollCallDefinication.IsValidSingleDelete = model.IsValidSingleDelete;

                objRollCallDefinication.UpdateDate = DateTime.Now;
                objRollCallDefinication.UpdateUser = model.UpdateUser;
                objRollCallDefinication.DomainName = model.DomainName;
                objRollCallDefinication.IsValidInShiftEnd = model.IsValidInShiftEnd;
                objRollCallDefinication.IsValidInShiftStart = model.IsValidInShiftStart;
                objRollCallDefinication.ValidityEndDate = model.ValidityEndDate;
                objRollCallDefinication.ValidityStartDate = model.ValidityStartDate;
                objRollCallDefinication.ValidityMaximumTime = model.ValidityMaximumTime;
                objRollCallDefinication.ValidityMaximumTimeMinute = model.ValidityMaximumTimeMinute;
                objRollCallDefinication.ValidityMinimumTime = model.ValidityMinimumTime;
                objRollCallDefinication.ValidityMinimumTimeMinute = model.ValidityMinimumTimeMinute;
                /// نوع روز کاری 
                if (model.RollCallWorkTimeDayTypeModels != null)
                    foreach (var objRollCallWorkTimeDayType in objRollCallDefinication.RollCallWorkTimeDayTypes)
                    {

                        if (!model.RollCallWorkTimeDayTypeModels.Any(r => r.WorkDayTypeId == objRollCallWorkTimeDayType.WorkDayTypeId && r.WorkTimeId == objRollCallWorkTimeDayType.WorkTimeId))
                        {
                            objRollCallWorkTimeDayType.IsActive = false;
                            objRollCallWorkTimeDayType.UpdateDate = DateTime.Now;
                            objRollCallWorkTimeDayType.UpdateUser = model.UpdateUser;
                        }
                    }
                foreach (var objRollCallWorkTimeDayType in model.RollCallWorkTimeDayTypeModels)
                {

                    if (!objRollCallDefinication.RollCallWorkTimeDayTypes.Any(x => x.WorkDayTypeId == objRollCallWorkTimeDayType.WorkDayTypeId && x.WorkTimeId == objRollCallWorkTimeDayType.WorkTimeId))
                    {
                        objRollCallDefinication.RollCallWorkTimeDayTypes.Add(new RollCallWorkTimeDayType()
                        {
                            WorkTimeId = objRollCallWorkTimeDayType.WorkTimeId,
                            WorkDayTypeId = objRollCallWorkTimeDayType.WorkDayTypeId,
                            InsertUser = model.InsertUser,
                            InsertDate = DateTime.Now,
                            DomainName = model.DomainName,
                            IsActive = true,
                        });
                    }
                    else
                    {
                        var objUpdate = objRollCallDefinication.RollCallWorkTimeDayTypes.SingleOrDefault(x => x.WorkDayTypeId == objRollCallWorkTimeDayType.WorkDayTypeId && x.WorkTimeId == objRollCallWorkTimeDayType.WorkTimeId);
                        objUpdate.IsActive = true;
                        objUpdate.UpdateDate = DateTime.Now;
                        objUpdate.UpdateUser = model.UpdateUser;
                    }
                }
                //نوع حقوق - نوع استخدام
                if (model.RollCallSalariesCode != null)
                    foreach (var objRollCallSalaryCodes in objRollCallDefinication.RollCallSalaryCodes)
                    {

                        if (!model.RollCallSalariesCode.Any(r => r.SalaryAccountCode == objRollCallSalaryCodes.SalaryAccountCode && r.EmploymentTypeCode == objRollCallSalaryCodes.EmploymentTypeCode))
                            objRollCallSalaryCodes.IsActive = false;
                    }
                foreach (var objRollCallSalaryCode in model.RollCallSalariesCode)
                {
                    if (!objRollCallDefinication.RollCallSalaryCodes.Any(x => x.SalaryAccountCode == objRollCallSalaryCode.SalaryAccountCode && x.EmploymentTypeCode == objRollCallSalaryCode.EmploymentTypeCode))
                        objRollCallDefinication.RollCallSalaryCodes.Add(new RollCallSalaryCode()
                        {
                            EmploymentTypeCode = objRollCallSalaryCode.EmploymentTypeCode,
                            SalaryAccountCode = objRollCallSalaryCode.SalaryAccountCode,
                            InsertUser = model.InsertUser,
                            InsertDate = DateTime.Now,
                            DomainName = model.DomainName,
                            IsActive = true,
                        });
                    else
                    {
                        var objUpdate = objRollCallDefinication.RollCallSalaryCodes.SingleOrDefault(x => x.SalaryAccountCode == objRollCallSalaryCode.SalaryAccountCode && x.EmploymentTypeCode == objRollCallSalaryCode.EmploymentTypeCode);
                        objUpdate.IsActive = true;
                        objUpdate.UpdateDate = DateTime.Now;
                        objUpdate.UpdateUser = model.UpdateUser;
                    }
                }

                // رده شغلی مجاز
                if (model.RollCallJobCategoriesId != null)
                    foreach (var objRollCallJobCategory in objRollCallDefinication.RollCallJobCategories)
                    {
                        if (!model.RollCallJobCategoriesId.Any(r => r == objRollCallJobCategory.CodeCategoryJobCategory))
                            objRollCallJobCategory.IsActive = false;
                    }
                if (model.RollCallJobCategoriesId != null)
                    foreach (var rollCallJobCategoryID in model.RollCallJobCategoriesId)
                    {
                        if (!objRollCallDefinication.RollCallJobCategories.Any(x => x.CodeCategoryJobCategory == rollCallJobCategoryID))
                            objRollCallDefinication.RollCallJobCategories.Add(new RollCallJobCategory()
                            {
                                CodeCategoryJobCategory = rollCallJobCategoryID,
                                InsertUser = model.InsertUser,
                                InsertDate = DateTime.Now,
                                DomainName = model.DomainName,
                                IsActive = true,
                            });
                        else
                        {
                            var objUpdate = objRollCallDefinication.RollCallJobCategories.SingleOrDefault(x => x.CodeCategoryJobCategory == rollCallJobCategoryID);
                            objUpdate.IsActive = true;
                            objUpdate.UpdateDate = DateTime.Now;
                            objUpdate.UpdateUser = model.UpdateUser;
                        }
                    }



                //نوع استخدام دسترسی به کد
                foreach (var objRollCallEmploymentType in objRollCallDefinication.RollCallEmploymentTypes)
                {
                    if (model.EmploymentTypeCodesId != null)
                    {
                        if (!model.EmploymentTypeCodesId.Any(r => r == objRollCallEmploymentType.EmploymentTypeCode.ToString()) || model.EmploymentTypeCodesId == null)
                        {
                            objRollCallEmploymentType.IsActive = false;
                            objRollCallEmploymentType.UpdateDate = DateTime.Now;
                            objRollCallEmploymentType.UpdateUser = model.UpdateUser;
                        }
                    }
                }
                if (model.EmploymentTypeCodesId != null)
                    foreach (var EmploymentTypeCodeId in model.EmploymentTypeCodesId)
                    {

                        if (!objRollCallDefinication.RollCallEmploymentTypes.Any(x => x.EmploymentTypeCode.ToString() == EmploymentTypeCodeId))
                        {
                            objRollCallDefinication.RollCallEmploymentTypes.Add(new RollCallEmploymentType()
                            {
                                EmploymentTypeCode = Convert.ToDecimal(EmploymentTypeCodeId),
                                InsertUser = model.InsertUser,
                                InsertDate = DateTime.Now,
                                DomainName = model.DomainName,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            var objUpdate = objRollCallDefinication.RollCallEmploymentTypes.SingleOrDefault(x => x.EmploymentTypeCode.ToString() == EmploymentTypeCodeId);
                            objUpdate.IsActive = true;
                            objUpdate.UpdateDate = DateTime.Now;
                            objUpdate.UpdateUser = model.UpdateUser;
                        }
                    }
                // مشمول کسر حقوق
                if (model.RewardDeductionIncludedId > 0)
                    model.IncludedRollCallsId = model.IncludedRollCallsId.Concat(new string[] { model.RewardDeductionIncludedId.ToString() }).ToArray();
                //  مشمول کسر پاداش
                if (model.SalaryDeductionIncludedId > 0)
                    model.IncludedRollCallsId = model.IncludedRollCallsId.Concat(new string[] { model.SalaryDeductionIncludedId.ToString() }).ToArray();
                //موارد مشمول

                foreach (var objIncludedRollCalls in objRollCallDefinication.IncludedRollCalls)
                {

                    if (!model.IncludedRollCallsId.Any(r => r == objIncludedRollCalls.IncludedDefinitionId.ToString()) || model.IncludedRollCallsId == null)
                    {
                        objIncludedRollCalls.IsActive = false;
                        objIncludedRollCalls.UpdateDate = DateTime.Now;
                        objIncludedRollCalls.UpdateUser = model.UpdateUser;
                    }
                }
                if (model.IncludedRollCallsId != null)
                    //
                    if (model.IncludedRollCallsId.Any(x => Convert.ToInt32(x) == EnumIncludedDefinition.MaximunOverTime.Id))
                    {
                        objRollCallDefinication.IsValidSingleDelete = true;
                    }
                    else
                    {
                        objRollCallDefinication.IsValidSingleDelete = false;
                    }
                //
                foreach (var includedRollCallId in model.IncludedRollCallsId)
                {

                    if (!objRollCallDefinication.IncludedRollCalls.Any(x => x.IncludedDefinitionId.ToString() == includedRollCallId))
                    {

                        objRollCallDefinication.IncludedRollCalls.Add(new IncludedRollCall()
                        {
                            IncludedDefinitionId = Convert.ToInt32(includedRollCallId),
                            InsertUser = model.InsertUser,
                            InsertDate = DateTime.Now,
                            DomainName = model.DomainName,
                            IsActive = true,
                        });
                    }
                    else
                    {
                        var objUpdate = objRollCallDefinication.IncludedRollCalls.SingleOrDefault(x => x.IncludedDefinitionId.ToString() == includedRollCallId);
                        objUpdate.IsActive = true;
                        objUpdate.UpdateDate = DateTime.Now;
                        objUpdate.UpdateUser = model.UpdateUser;
                    }
                }
                //
                // کد های سازگار
                if (model.CompatibleRollCallsId != null)
                    foreach (var objIncludedRollCalls in objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Where(x => x.CompatibleRollCallType == EnumCompatibleRollCallType.Compatible.Id))
                    {

                        if (!model.CompatibleRollCallsId.Any(r => r == objIncludedRollCalls.CompatibleRollCallId.ToString()))
                            objIncludedRollCalls.IsActive = false;
                    }
                if (model.CompatibleRollCallsId != null)
                    foreach (var compatibleRollCallId in model.CompatibleRollCallsId)
                    {

                        if (!objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Any(x => x.CompatibleRollCallId.ToString() == compatibleRollCallId && x.CompatibleRollCallType == EnumCompatibleRollCallType.Compatible.Id))
                        {
                            objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Add(new CompatibleRollCall()
                            {
                                CompatibleRollCallId = Convert.ToInt32(compatibleRollCallId),
                                CompatibleRollCallType = EnumCompatibleRollCallType.Compatible.Id,
                                InsertUser = model.InsertUser,
                                InsertDate = DateTime.Now,
                                DomainName = model.DomainName,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            var objUpdate = objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.SingleOrDefault(x => x.CompatibleRollCallId.ToString() == compatibleRollCallId && x.CompatibleRollCallType == EnumCompatibleRollCallType.Compatible.Id);
                            objUpdate.IsActive = true;
                            objUpdate.UpdateDate = DateTime.Now;
                            objUpdate.UpdateUser = model.UpdateUser;
                        }
                    }

                // کد های قابل تبدیل به یکدیگر
                foreach (var objIncludedRollCalls in objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Where(x => x.CompatibleRollCallType == EnumCompatibleRollCallType.Interchangeable.Id))
                {

                    if (!model.InterchangeableRollCallsId.Any(r => r == objIncludedRollCalls.CompatibleRollCallId.ToString()) || model.InterchangeableRollCallsId == null)
                    {
                        objIncludedRollCalls.IsActive = false;
                        objIncludedRollCalls.UpdateDate = DateTime.Now;
                        objIncludedRollCalls.UpdateUser = model.UpdateUser;
                    }
                }
                if (model.InterchangeableRollCallsId != null)
                    foreach (var compatibleRollCallId in model.InterchangeableRollCallsId)
                    {

                        if (!objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Any(x => x.CompatibleRollCallId.ToString() == compatibleRollCallId && x.CompatibleRollCallType == EnumCompatibleRollCallType.Interchangeable.Id))
                        {
                            objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Add(new CompatibleRollCall()
                            {
                                CompatibleRollCallId = Convert.ToInt32(compatibleRollCallId),
                                CompatibleRollCallType = EnumCompatibleRollCallType.Interchangeable.Id,
                                InsertUser = model.InsertUser,
                                InsertDate = DateTime.Now,
                                DomainName = model.DomainName,
                                IsActive = true,
                            });
                        }
                        else
                        {
                            var objUpdate = objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.FirstOrDefault(x => x.CompatibleRollCallId.ToString() == compatibleRollCallId && x.CompatibleRollCallType == EnumCompatibleRollCallType.Interchangeable.Id);
                            objUpdate.IsActive = true;
                            objUpdate.UpdateDate = DateTime.Now;
                            objUpdate.UpdateUser = model.UpdateUser;
                        }
                    }
                //شهرمحل خدمت

                if (model.RollCallWorkCityModels != null)
                    foreach (var objRollCallWorkCity in objRollCallDefinication.RollCallWorkCities)
                    {

                        if (!model.RollCallWorkCityModels.Any(r => r.WorkCityId == objRollCallWorkCity.WorkCityId && r.StartDate.Date == objRollCallWorkCity.StartDate.Date && r.EndDate.Date == objRollCallWorkCity.EndDate.Date))
                        {
                            objRollCallWorkCity.IsActive = false;
                            objRollCallWorkCity.UpdateDate = DateTime.Now;
                            objRollCallWorkCity.UpdateUser = model.UpdateUser;
                        }
                    }
                foreach (var objRollCallWorkCity in model.RollCallWorkCityModels)
                {

                    if (!objRollCallDefinication.RollCallWorkCities.Any(r => r.WorkCityId == objRollCallWorkCity.WorkCityId && r.StartDate.Date == objRollCallWorkCity.StartDate.Date && r.EndDate.Date == objRollCallWorkCity.EndDate.Date))
                    {
                        objRollCallDefinication.RollCallWorkCities.Add(new RollCallWorkCity()
                        {
                            WorkCityId = objRollCallWorkCity.WorkCityId,
                            StartDate = objRollCallWorkCity.StartDate.Date,
                            EndDate = objRollCallWorkCity.EndDate.Date,
                            InsertUser = model.InsertUser,
                            InsertDate = DateTime.Now,
                            DomainName = model.DomainName,
                            IsActive = true,
                        });
                    }
                    else
                    {
                        var objUpdate = objRollCallDefinication.RollCallWorkCities.SingleOrDefault(r => r.WorkCityId == objRollCallWorkCity.WorkCityId && r.StartDate == objRollCallWorkCity.StartDate && r.EndDate == objRollCallWorkCity.EndDate);
                        objUpdate.IsActive = true;
                        objUpdate.UpdateDate = DateTime.Now;
                        objUpdate.UpdateUser = model.UpdateUser;
                    }
                }
                //
                objRollCallDefinication.Code = objRollCallDefinication.Id.ToString();
                objRollCallDefinication.IsValidForDailyAbcenseInAnalyz = model.IsValidForDailyAbcenseInAnalyz;
                objRollCallDefinication.IsValidForDeleteAbsenceItem = model.IsValidForDeleteAbsenceItem;
                objRollCallDefinication.GenderTypeId = model.GenderTypeId;
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }
            return result;

        }

        public async Task<bool> ExistsByTitle(string name)
        {
            return await _kscHrUnitOfWork.RollCallDefinitionRepository.AnyAsync(x => x.Title == name);
        }

        public async Task<bool> ExistsByCode(string code)
        {
            return await _kscHrUnitOfWork.RollCallDefinitionRepository.AnyAsync(x => x.Code == code);
        }

        public async Task<RollCallDefinition> GetOne(int id)
        {
            return _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllIncluded().FirstOrDefault(i => i.Id == id);
        }

        public async Task<AddRollCallDefinicationModel> GetForEdit(int id)
        {
            var objRollCallDefinication = await GetOne(id);
            var model = _mapper.Map<AddRollCallDefinicationModel>(objRollCallDefinication);

            var includedDefinitionId = objRollCallDefinication.IncludedRollCalls.Where(x => x.IsActive).Select(x => x.IncludedDefinitionId);
            model.IncludedRollCallsId = includedDefinitionId.Select(i => i.ToString()).ToArray();
            model.RewardDeductionIncludedId = includedDefinitionId.FirstOrDefault(x => IncludedDefinitionConst.RewardDeductionIncludedId.Contains(x));
            model.SalaryDeductionIncludedId = includedDefinitionId.FirstOrDefault(x => IncludedDefinitionConst.SalaryDeductionIncludedId.Contains(x));
            model.RollCallWorkTimeDayTypeModels = _mapper.Map<List<RollCallWorkTimeDayTypeModel>>(objRollCallDefinication.RollCallWorkTimeDayTypes.Where(x => x.IsActive));
            var rollCallJobCategoriesId = objRollCallDefinication.RollCallJobCategories.Where(x => x.IsActive).Select(x => x.CodeCategoryJobCategory);
            model.RollCallJobCategoriesId = rollCallJobCategoriesId.Select(i => i.ToString()).ToArray();
            var rollCallEmploymentTypes = objRollCallDefinication.RollCallEmploymentTypes.Where(x => x.IsActive).Select(x => x.EmploymentTypeCode);
            model.EmploymentTypeCodesId = rollCallEmploymentTypes.Select(i => i.ToString()).ToArray();
            var compatibleRollCallsId = objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Where(x => x.IsActive && x.CompatibleRollCallType == EnumCompatibleRollCallType.Compatible.Id).Select(x => x.CompatibleRollCallId);
            model.CompatibleRollCallsId = compatibleRollCallsId.Select(i => i.ToString()).ToArray();
            //
            var interchangeableRollCallsId = objRollCallDefinication.CompatibleRollCalls_RollCallDefinitionId.Where(x => x.IsActive && x.CompatibleRollCallType == EnumCompatibleRollCallType.Interchangeable.Id).Select(x => x.CompatibleRollCallId);
            model.InterchangeableRollCallsId = interchangeableRollCallsId.Select(i => i.ToString()).ToArray();
            //
            model.RollCallSalariesCode = _mapper.Map<List<RollCallSalaryCodeModel>>(objRollCallDefinication.RollCallSalaryCodes.Where(x => x.IsActive));
            var ViewMisEmploymentTypes = _kscHrUnitOfWork.ViewMisEmploymentTypeRepository.GetAllQueryable().AsQueryable();
            var ViewMisSalaryCodes = _kscHrUnitOfWork.AccountCodeRepository.GetAllQueryable().AsQueryable();
            foreach (var item in model.RollCallSalariesCode)
            {
                item.EmploymentTypeTitle = ViewMisEmploymentTypes.FirstOrDefault(i => i.EmploymentTypeCode == item.EmploymentTypeCode)?.EmploymentTypeTitle;
                item.SalaryAccountTitle = ViewMisSalaryCodes.FirstOrDefault(i => i.Id == item.SalaryAccountCode)?.Title;
            }
            model.RollCallWorkCityModels = _mapper.Map<List<RollCallWorkCityModel>>(objRollCallDefinication.RollCallWorkCities.Where(x => x.IsActive));
            var workCities = _kscHrUnitOfWork.WorkCityRepository.GetAllQueryable().Include(x => x.City).Select(x => new { WorkCityId = x.Id, CityTitle = x.City.Title });
            foreach (var item in model.RollCallWorkCityModels)
            {
                var workCity = workCities.FirstOrDefault(x => x.WorkCityId == item.WorkCityId);
                if (workCity != null)
                    item.WorkCityTitle = workCity.CityTitle;
            }
            return model;
        }

        public async Task<bool> ExistsByTitle(int id, string name)
        {
            return (await _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllAsync()).Any(x => x.Id != id && x.Title == name);

        }
        public async Task<bool> ExistsByCode(int id, string code)
        {
            return (await _kscHrUnitOfWork.RollCallDefinitionRepository.GetAllAsync()).Any(x => x.Id != id && x.Code == code);

        }
        public List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence(DateTime date)
        {
            var query = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionIdByDate(date);
            var result = query.Select(x => new RollCallModelForEmployeeAttendAbsenceModel()
            {
                RollCallDefinitionId = x.Id,
                RollCallDefinitionTitle = x.Title,
                RollCallCategoryId = x.RollCallCategoryId,
                IsValidForAllWorkTimeDayType = x.IsValidForAllWorkTimeDayType,
                // RollCallWorkTimeDayTypes = x.RollCallWorkTimeDayTypes != null ? x.RollCallWorkTimeDayTypes.Select(x => new RollCallWorkTimeDayTypeModel() { WorkTimeId = x.WorkTimeId, WorkDayTypeId = x.WorkDayTypeId }).ToList() : null,
                IsValidForAllCategoryCode = x.IsValidForAllCategoryCode,
                IsValidForAllEmploymentType = x.IsValidForAllEmploymentType,
                IsValidForTemporaryEndDate = x.IsValidForTemporaryEndDate,
                IsValidForTemporaryStartDate = x.IsValidForTemporaryStartDate,
                IsValidInShiftStart = x.IsValidInShiftStart,
                IsValidInShiftEnd = x.IsValidInShiftEnd,
                IsValidSingleDelete = x.IsValidSingleDelete,
                TimesAllowedUsePerDay = x.TimesAllowedUsePerDay,
                TimesAllowedUsePerMonth = x.TimesAllowedUsePerMonth,
                TimesAllowedUsePerWeek = x.TimesAllowedUsePerWeek,
                //  RollCallEmploymentTypeCode = x.RollCallEmploymentTypes != null ? x.RollCallEmploymentTypes.Select(x => x.EmploymentTypeCode).ToList() : null

            }).ToList();
            return result;
        }
        public List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionByWorkTimeDayTypeEmploymentTypeId(int workTimeId, int workDayTypeId, int? employmentTypeId, DateTime date)
        {
            var query = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByWorkTimeDayTypeEmploymentTypeId(workTimeId, workDayTypeId, employmentTypeId, date);
            var result = query.Select(x => new RollCallModelForEmployeeAttendAbsenceModel()
            {
                RollCallDefinitionId = x.Id,
                RollCallDefinitionTitle = x.Title,
                RollCallDefinitionCode = x.Code,
                RollCallConceptId = x.RollCallConceptId,
                RollCallCategoryId = x.RollCallCategoryId,
                IsValidForAllCategoryCode = x.IsValidForAllCategoryCode,
                //CodeCategoryJobCategory = x.RollCallJobCategories != null ? x.RollCallJobCategories.Select(x => x.CodeCategoryJobCategory).ToList() : null,
                IsValidForTemporaryEndDate = x.IsValidForTemporaryEndDate,
                IsValidForTemporaryStartDate = x.IsValidForTemporaryStartDate,
                IsValidInShiftStart = x.IsValidInShiftStart,
                IsValidInShiftEnd = x.IsValidInShiftEnd,
                IsValidSingleDelete = x.IsValidSingleDelete,
                TimesAllowedUsePerDay = x.TimesAllowedUsePerDay,
                TimesAllowedUsePerWeek = x.TimesAllowedUsePerWeek,
                TimesAllowedUsePerMonth = x.TimesAllowedUsePerMonth,
            }).ToList();
            return result;
        }
        public List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence_()
        {
            var query = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByIncludedForAttendAbsenceAsnoTracking();
            var result = query.Select(x => new RollCallModelForEmployeeAttendAbsenceModel()
            {
                RollCallDefinitionId = x.Id,
                RollCallDefinitionTitle = x.Title,
                RollCallDefinitionCode = x.Code,
                ValidityStartDate = x.ValidityStartDate.Value,
                ValidityEndDate = x.ValidityEndDate.Value,
                RollCallCategoryId = x.RollCallCategoryId,
                RollCallConceptId = x.RollCallConceptId,
                IsValidForAllWorkTimeDayType = x.IsValidForAllWorkTimeDayType,
                //RollCallWorkTimeDayTypes = x.RollCallWorkTimeDayTypes != null ? x.RollCallWorkTimeDayTypes.Select(x => new RollCallWorkTimeDayTypeModel() { WorkTimeId = x.WorkTimeId, WorkDayTypeId = x.WorkDayTypeId }).ToList() : null,
                IsValidForAllCategoryCode = x.IsValidForAllCategoryCode,
                IsValidForAllEmploymentType = x.IsValidForAllEmploymentType,
                IsValidForTemporaryEndDate = x.IsValidForTemporaryEndDate,
                IsValidForTemporaryStartDate = x.IsValidForTemporaryStartDate,
                IsValidInShiftStart = x.IsValidInShiftStart,
                IsValidInShiftEnd = x.IsValidInShiftEnd,
                IsValidSingleDelete = x.IsValidSingleDelete,
                TimesAllowedUsePerDay = x.TimesAllowedUsePerDay,
                TimesAllowedUsePerWeek = x.TimesAllowedUsePerWeek,
                TimesAllowedUsePerMonth = x.TimesAllowedUsePerMonth,
                // RollCallEmploymentTypeCode = x.RollCallEmploymentTypes != null ? x.RollCallEmploymentTypes.Select(x => x.EmploymentTypeCode).ToList() : null,
                TrainingTypeId = x.TrainingTypeId,
                TrainingValidInShiftTime = x.TrainingValidInShiftTime,
                TrainingValidOutShiftTime = x.TrainingValidOutShiftTime,
                VaccinationCheck = x.VaccinationCheck
                //CompatibleRollCall = x.CompatibleRollCalls_RollCallDefinitionId != null ? x.CompatibleRollCalls_RollCallDefinitionId.Select(x => new CompatibleRollCallModel() { CompatibleRollCallId = x.CompatibleRollCallId, CompatibleRollCallType = x.CompatibleRollCallType }).ToList() : null

            }).ToList();
            return result;
        }
        public List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence()
        {
            IQueryable<RollCallDefinition> rollCallDefinition = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionAsNoTracking();
            IQueryable<RollCallWorkTimeDayType> rollCallWorkTimeDayType = _kscHrUnitOfWork.RollCallWorkTimeDayTypeRepository.GetRollCallWorkTimeDayTypeAsNoTracking().Where(x => x.IsActive);
            IQueryable<RollCallEmploymentType> rollCallEmploymentType = _kscHrUnitOfWork.RollCallEmploymentTypeRepository.GetRollCallEmploymentTypeAsNoTracking().Where(x => x.IsActive);
            var result = from roll in rollCallDefinition
                         join workday in rollCallWorkTimeDayType on roll.Id equals workday.RollCallDefinitionId into leftWorkday
                         from workdayResult in leftWorkday.DefaultIfEmpty()
                         join emplType in rollCallEmploymentType on roll.Id equals emplType.RollCallDefinitionId into leftEmplType
                         from emplTypeResult in leftEmplType.DefaultIfEmpty()
                         select new RollCallModelForEmployeeAttendAbsenceModel()
                         {
                             RollCallDefinitionId = roll.Id,
                             RollCallDefinitionTitle = roll.Title,
                             RollCallDefinitionCode = roll.Code,
                             ValidityStartDate = roll.ValidityStartDate.Value,
                             ValidityEndDate = roll.ValidityEndDate.Value,
                             RollCallCategoryId = roll.RollCallCategoryId,
                             RollCallConceptId = roll.RollCallConceptId,
                             IsValidForAllWorkTimeDayType = roll.IsValidForAllWorkTimeDayType,
                             WorkTimeId = workdayResult != null ? workdayResult.WorkTimeId : null,
                             WorkDayTypeId = workdayResult != null ? workdayResult.WorkDayTypeId : null,
                             IsValidForAllCategoryCode = roll.IsValidForAllCategoryCode,
                             IsValidForAllEmploymentType = roll.IsValidForAllEmploymentType,
                             IsValidForTemporaryEndDate = roll.IsValidForTemporaryEndDate,
                             IsValidForTemporaryStartDate = roll.IsValidForTemporaryStartDate,
                             IsValidInShiftStart = roll.IsValidInShiftStart,
                             IsValidInShiftEnd = roll.IsValidInShiftEnd,
                             IsValidSingleDelete = roll.IsValidSingleDelete,
                             TimesAllowedUsePerDay = roll.TimesAllowedUsePerDay,
                             TimesAllowedUsePerWeek = roll.TimesAllowedUsePerWeek,
                             TimesAllowedUsePerMonth = roll.TimesAllowedUsePerMonth,
                             EmploymentTypeCode = emplTypeResult != null ? emplTypeResult.EmploymentTypeCode : null,
                             TrainingTypeId = roll.TrainingTypeId,
                             TrainingValidInShiftTime = roll.TrainingValidInShiftTime,
                             TrainingValidOutShiftTime = roll.TrainingValidOutShiftTime,
                             VaccinationCheck = roll.VaccinationCheck
                         };
            return result.ToList();
        }
        public List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsenceFromInputList(List<RollCallModelForEmployeeAttendAbsenceModel> model, int workTimeId, int workDayTypeId, int? employmentTypeId, DateTime date)
        {
            var result = model.Where(x => x.ValidityStartDate.Date <= date.Date && x.ValidityEndDate.Date >= date.Date &&
                                                       (x.IsValidForAllEmploymentType || x.EmploymentTypeCode == employmentTypeId) &&
                            (x.IsValidForAllWorkTimeDayType == true || (x.WorkTimeId == workTimeId && x.WorkDayTypeId == workDayTypeId))).ToList();
            return result;
        }
        public List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence(TimeSettingDataModel timeSettingDataModel, int? employmentTypeId)
        {
            DateTime dateCheckEnd = timeSettingDataModel.TomorrowDateTime.AddDays(-2);
            DateTime dateCheckStart = timeSettingDataModel.TomorrowDateTime.AddDays(1);
            var query = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinitionByIncludedForAttendAbsence().Where(
              x => x.ValidityStartDate < dateCheckStart && x.ValidityEndDate > dateCheckEnd &&

              (x.IsValidForAllWorkTimeDayType || (x.RollCallWorkTimeDayTypes.Any(x => (x.WorkTimeId == timeSettingDataModel.WorkTimeId && x.WorkDayTypeId == timeSettingDataModel.WorkDayTypeId)
              || (x.WorkTimeId == timeSettingDataModel.TomorrowWorkTimeId && x.WorkDayTypeId == timeSettingDataModel.TomorrowWorkDayTypeId))))
               && (x.IsValidForAllEmploymentType || x.RollCallEmploymentTypes.Any(x => x.EmploymentTypeCode == employmentTypeId))
                )

                ;
            var result = query.Select(x => new RollCallModelForEmployeeAttendAbsenceModel()
            {
                RollCallDefinitionId = x.Id,
                RollCallDefinitionTitle = x.Title,
                RollCallDefinitionCode = x.Code,
                ValidityStartDate = x.ValidityStartDate.Value,
                ValidityEndDate = x.ValidityEndDate.Value,
                RollCallCategoryId = x.RollCallCategoryId,
                RollCallConceptId = x.RollCallConceptId,
                IsValidForAllWorkTimeDayType = x.IsValidForAllWorkTimeDayType,
                //RollCallWorkTimeDayTypes = x.RollCallWorkTimeDayTypes != null ? x.RollCallWorkTimeDayTypes.Select(x => new RollCallWorkTimeDayTypeModel() { WorkTimeId = x.WorkTimeId, WorkDayTypeId = x.WorkDayTypeId }).ToList() : null,
                IsValidForAllCategoryCode = x.IsValidForAllCategoryCode,
                IsValidForAllEmploymentType = x.IsValidForAllEmploymentType,
                IsValidForTemporaryEndDate = x.IsValidForTemporaryEndDate,
                IsValidForTemporaryStartDate = x.IsValidForTemporaryStartDate,
                IsValidInShiftStart = x.IsValidInShiftStart,
                IsValidInShiftEnd = x.IsValidInShiftEnd,
                IsValidSingleDelete = x.IsValidSingleDelete,
                TimesAllowedUsePerDay = x.TimesAllowedUsePerDay,
                TimesAllowedUsePerWeek = x.TimesAllowedUsePerWeek,
                TimesAllowedUsePerMonth = x.TimesAllowedUsePerMonth,
                // RollCallEmploymentTypeCode = x.RollCallEmploymentTypes != null ? x.RollCallEmploymentTypes.Select(x => x.EmploymentTypeCode).ToList() : null,
                TrainingTypeId = x.TrainingTypeId,
                TrainingValidInShiftTime = x.TrainingValidInShiftTime,
                TrainingValidOutShiftTime = x.TrainingValidOutShiftTime,
                VaccinationCheck = x.VaccinationCheck
                //CompatibleRollCall = x.CompatibleRollCalls_RollCallDefinitionId != null ? x.CompatibleRollCalls_RollCallDefinitionId.Select(x => new CompatibleRollCallModel() { CompatibleRollCallId = x.CompatibleRollCallId, CompatibleRollCallType = x.CompatibleRollCallType }).ToList() : null

            }).ToList();
            return result;
        }
        public RollCallForEmployeeAttendAbsenceModel GetRollCallsForEmployeeAttendAbsence(TimeSettingDataModel timeSettingDataModel, int? employmentTypeId, DateTime date)
        {
            RollCallForEmployeeAttendAbsenceModel result = new RollCallForEmployeeAttendAbsenceModel()
            {
                RollCallToday = new List<RollCallModelForEmployeeAttendAbsenceModel>(),
                RollCallTomorrow = new List<RollCallModelForEmployeeAttendAbsenceModel>()
            };
            var data = GetRollCallDefinitionForEmployeeAttendAbsence();
            result.RollCallToday = GetRollCallDefinitionForEmployeeAttendAbsenceFromInputList(data, timeSettingDataModel.WorkTimeId, timeSettingDataModel.WorkDayTypeId, employmentTypeId, date);
            result.RollCallTomorrow = GetRollCallDefinitionForEmployeeAttendAbsenceFromInputList(data, timeSettingDataModel.TomorrowWorkTimeId, timeSettingDataModel.TomorrowWorkDayTypeId, employmentTypeId, date);
            return result;
        }

        public FilterResult<RollCallDefinicationModel> GetRollCallDefinicationInCeiling(SearchRollCallDefinicationModel Filter)
        {

            var rollCallDefinicationModelQuery = _kscHrUnitOfWork.RollCallDefinitionRepository.GetRollCallDefinicationInCeiling(EnumIncludedDefinition.MaximunOverTime.Id);


            var query = rollCallDefinicationModelQuery.Select(a => new RollCallDefinicationModel
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                OverTimePriority = a.OverTimePriority
            }).ToList();

            query = query.OrderBy(a => a.Code).ToList();
            var result = _FilterHandler.GetFilterResult<RollCallDefinicationModel>(query, Filter, nameof(RollCallDefinition.Id));
            var modelResult = new FilterResult<RollCallDefinicationModel>
            {
                Data = _mapper.Map<List<RollCallDefinicationModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        public async Task<KscResult> CheckRollCallWorkCity(AddRollCallDefinicationModel model)

        {
            var result = new KscResult();
            int row = 0;
            foreach (var item in model.RollCallWorkCityModels)
            {
                row++;
                item.RowId = row;
            }
            foreach (var item in model.RollCallWorkCityModels)
            {
                if (item.StartDate.Date > item.EndDate.Date)
                {
                    result.AddError("رکورد نامعتبر", $"برای شهر{item.WorkCityTitle} در ردیف {item.RowId} تاریخ شروع بزرگتر از تاریخ پایان است");
                    return result;
                }
                if (model.ValidityStartDate.HasValue && item.StartDate.Date < model.ValidityStartDate.Value.Date)
                {
                    result.AddError("رکورد نامعتبر", $"برای شهر{item.WorkCityTitle} در ردیف {item.RowId} تاریخ شروع نباید کوچکتر از تاریخ شروع اعتبار کد حضور-غیاب باشد ");
                    return result;
                }
                if (model.ValidityEndDate.HasValue && item.EndDate.Date > model.ValidityEndDate.Value.Date)
                {
                    result.AddError("رکورد نامعتبر", $"برای شهر{item.WorkCityTitle} در ردیف {item.RowId} تاریخ پایان نباید بزرگتر از تاریخ پایان اعتبار کد حضور-غیاب باشد ");
                    return result;
                }
                var data = model.RollCallWorkCityModels.Where(x => x.RowId != item.RowId && x.WorkCityId == item.WorkCityId &&
               ((x.StartDate <= item.StartDate && x.EndDate >= item.StartDate) ||
                (x.StartDate <= item.EndDate && x.EndDate >= item.EndDate) ||
                (x.StartDate >= item.StartDate && x.EndDate <= item.EndDate)
                )
                );
                if (data.Any())
                {
                    var rows = item.RowId + "," + string.Join(",", data.Select(x => x.RowId));
                    result.AddError("رکورد نامعتبر", $"برای شهر{item.WorkCityTitle} در ردیف {rows} بازه زمانی تکراری است");
                    // result.AddError("رکورد نامعتبر", $"برای شهر{item.WorkCityTitle} بازه زمانی تکراری است");
                    return result;
                }


            }
            return result;
        }
    }
}
