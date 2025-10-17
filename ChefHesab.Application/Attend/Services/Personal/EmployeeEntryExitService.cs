using AutoMapper;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using DNTPersianUtils.Core;
using Ksc.HR.Domain.Repositories.Personal;
using KSC.Common;
using Ksc.HR.Domain.Entities.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Resources.Messages;
using KSC.MIS.Service;
using Ksc.HR.Share.Model;
using KSC.MIS.Service;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using Ksc.HR.Share.Extention;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using Ksc.HR.Share.General;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.Share.General;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.Report;
using Microsoft.Extensions.Configuration;
using Ksc.HR.DTO.WorkShift.TeamWork;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeEntryExitService : IEmployeeEntryExitService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IShiftConceptDetailService _shiftConceptDetailService;
        private readonly IConfiguration _configuration;
        public EmployeeEntryExitService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper
       , IFilterHandler FilterHandler
       , IShiftConceptDetailService shiftConceptDetailService
, IConfiguration configuration
       )
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _shiftConceptDetailService = shiftConceptDetailService;
            _configuration = configuration;
        }

        public async Task<KscResult> FindEntryExit(string startDate, string EndDate, string personalNumber)
        {
            var result = new KscResult();
            var DAT_STR_ASSPYString = int.Parse(startDate);
            var DAT_END_ASSPYString = int.Parse(EndDate);
            var workCalendars = _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarQuerable()
            .Where(a => a.DateKey >= DAT_STR_ASSPYString && a.DateKey <= DAT_END_ASSPYString).ToListAsync().GetAwaiter().GetResult();
            var miladiDate = workCalendars.Select(a => a.MiladiDateV1).ToList();
            var employeeEntryExist = _kscHrUnitOfWork.EmployeeEntryExitRepository
                .Any(a => miladiDate.Any(c => c == a.EntryExitDate)
                && a.PersonalNumber == personalNumber);
            if (employeeEntryExist)
            {
                //result.Id = "R";
                result.AddError("", "این شماره پرسنلی در این بازه دارای  ورود و خروج می باشد");
                return result;
            }
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(personalNumber);
            var workCalendarIds = workCalendars.Select(a => a.Id).ToList();
            var items = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetItemsHolidayByworkcalendarIds(employee.Id, workCalendarIds);

            //var normalDay = items.Where(x => rollCallHoliday.Any(r => r == x.RollCallDefinitionId) == false).Select(x => x.WorkCalendarId).ToList().Distinct();
            if (items.Any())
            {
                var dates = workCalendars.Where(x => items.Any(n => n.WorkCalendarId == x.Id)).Select(x => x.ShamsiDateV1).ToList();
                var inValidDate = string.Join("-", dates);
                result.AddError("", string.Format(" کارکرد تایید شده برای تاریخ {0} وجود دارد ، در صورت تایید کلیه موارد تایید کارکرد در این تاریخ ها حذف می شود، آیا مطمئن هستید؟", inValidDate));
                return result;
            }


            return result;
        }
        public FilterResult<SearchEmployeeEntryExitModel> GetEmployeeEntryExitByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetAllQueryable();
            FilterResult<EmployeeEntryExit> result = _FilterHandler.GetFilterResult<EmployeeEntryExit>(query, Filter, nameof(EmployeeEntryExit.Id));
            var modelResult = new FilterResult<SearchEmployeeEntryExitModel>
            {
                Data = _mapper.Map<List<SearchEmployeeEntryExitModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }


        public CALLING_RPC GetPersonalDataMis(InputMisApiModel model)
        //(string personalNumber, string domain,string func_Name)
        {

            try
            {
                ParamApi<CALLING_RPC> miscall = new ParamApi<CALLING_RPC>(
                     Enviroment: Enviroment.Load,
                     DomainEnum: model.domain.ToUpper(),
                     LibraryName: LibraryName.PER,
                     Subprogram: "S6XML025",
                     ParamName: "CALLING_RPC",
                     Pheader: new PHeader()
                     {

                     },
                         InputModel: new CALLING_RPC { NUM_PRSN_EMPL = model.NUM_PRSN_EMPL, FUNCTION = model.FUNCTION }
                     );
                var result = miscall.GetResultDevelop<CALLING_RPC>(_configuration["MisServiceUrl"]);
                if (result.IsSuccess == false)
                {
                    //throw new Exception(string.Join(",", result.Messages));
                    throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
                }

                var userDataMIS = result.Data;
                return userDataMIS;
            }
            catch (Exception ex)
            {
                CALLING_RPC e = new CALLING_RPC();
                e.IsError = true;
                e.MsgError = ex.Message;
                return e;
                //throw new Exception(ex.Message);
            }
        }

        public EmployeeEntryExitModel GetOneBySearchModel(SearchEmployeeEntryExitModel model)
        {
            var EmployeeEntryExit = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetAllQueryable()
                .First(x => x.PersonalNumber == model.PersonalNumber);
            var result = new EmployeeEntryExitModel
            {
                Id = EmployeeEntryExit.Id,
                EntryExitType = EmployeeEntryExit.EntryExitType,
            };

            return result;
        }

        public EmployeeEntryExit GetOne(int id)
        {
            return _kscHrUnitOfWork.EmployeeEntryExitRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public EditEmployeeEntryExitModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditEmployeeEntryExitModel>(model);
        }
        public List<EmployeeEntryExitModel> GetEmployeeEntryExit()
        {
            var model =
                new List<EmployeeEntryExitModel>() {
                    new EmployeeEntryExitModel() {
           Id=1,
            EntryDate = DateTime.Now,
            EntryDateString=DateTime.Now.ToLongPersianDateString() ,
            ExitDate = DateTime.Now,
            ExitDateString=DateTime.Now.ToLongPersianDateString() ,
            EntryTime = "07:15",
            ExitTime ="16:00",
            Selected=false,
            },{
                new EmployeeEntryExitModel(){
                    Id=2,
                EntryDate = DateTime.Now,
            EntryDateString=DateTime.Now.ToLongPersianDateString() ,
            ExitDate = DateTime.Now,
            ExitDateString=DateTime.Now.ToLongPersianDateString() ,
            EntryTime = "17:00",
            ExitTime ="20:00",
            Selected=true}
            },
                new EmployeeEntryExitModel()
            {
            Id=3,
               EntryDate = DateTime.Now,
               EntryDateString=DateTime.Now.ToLongPersianDateString() ,
            ExitDate = DateTime.Now.AddDays(1),
             ExitDateString=DateTime.Now.AddDays(1).ToLongPersianDateString() ,
            EntryTime = "08:15",
            ExitTime ="16:00",
            Selected=false,
            },
              new EmployeeEntryExitModel()
            {
            Id=4,
             EntryDate = DateTime.Now.AddDays(2),
                EntryDateString=DateTime.Now.AddDays(2).ToLongPersianDateString() ,
            ExitDate = DateTime.Now.AddDays(2),
             ExitDateString=DateTime.Now.AddDays(2).ToLongPersianDateString() ,

            EntryTime = "10:15",
            ExitTime ="16:00",
            Selected=false,
            }
                  };

            return model;
        }

        //این متد با گرفتن ورودی های تاریخ و شماره پرسنلی طبق منطق ورود خروج های معتبر را از جدول entryExit
        public List<EntryExitResult> GetEmployeeEntryExitByDate(EntryExitListSearchModel model)
        {
            //0000
            var onCall_Request = _kscHrUnitOfWork.OnCall_RequestRepository.WhereQueryable(x => x.RequestId == model.WfRequestId).FirstOrDefault();
            var wf_Request = _kscHrUnitOfWork.WF_RequestRepository.GetAllQueryable().FirstOrDefault(x => x.Id == model.WfRequestId);
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().FirstOrDefault(x => x.Id == wf_Request.EmployeeId);
            model.person = employee.EmployeeNumber;
            model.entryExitDate = onCall_Request.OnCallDate;
            var data = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitValidByDate(model.person, model.entryExitDate)
                .Select(x => new EntryExitList()
                {
                    type = x.EntryExitType,
                    time = x.EntryExitTime
                ,
                    EntryExitDate = x.EntryExitDate,
                    Id = x.Id
                }).ToList();
            var tdate = model.entryExitDate.AddDays(1);
            var tomorrow = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitValidByDate(model.person, tdate).OrderBy(x => x.EntryExitDate)
                 .FirstOrDefault();
            if (tomorrow != null && tomorrow.EntryExitType == 2)
            {
                EntryExitList temp = new EntryExitList()
                {
                    type = tomorrow.EntryExitType,
                    time = tomorrow.EntryExitTime
                    ,
                    EntryExitDate = tomorrow.EntryExitDate
                ,
                    Id = tomorrow.Id
                };
                data.Add(temp);
            }
            List<EntryExitResult> entryExitResult = new List<EntryExitResult>();
            for (int i = 0; i < data.Count; i++)
            {
                EntryExitResult row = new EntryExitResult();
                if (data[i].type == 1)
                {

                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (data[j].type == 2)
                        {
                            row.Id = data[i].Id;
                            row.EntryTime = data[i].time;
                            row.ExitTime = data[j].time;

                            row.EntryDateString = data[i].EntryExitDate.ToLongPersianDateString();//persian
                            row.ExitDateString = data[j].EntryExitDate.ToLongPersianDateString();//persian

                            row.EntryDate = data[i].EntryExitDate;//miladi
                            row.ExitDate = data[j].EntryExitDate;//miladi
                            entryExitResult.Add(row);

                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return entryExitResult;
        }
        public async Task<List<EntryExitResult>> GetEmployeeEntryExitForOnCall(EntryExitForOnCallSearchModel searchModel)
        {
            List<EntryExitResult> result = new List<EntryExitResult>();

            var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(searchModel.EmployeeId);
            if (employee == null)
            {
                return result;
                //throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات پرسنلی"));
            }
            //
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(searchModel.OnCallDate);
            if (workCalendar == null)
            {
                return result;
                // throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات تقویم"));
            }
            if (employee.WorkGroupId == null)
            {
                return result;
                // throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات گروه کاری"));
            }

            var employeeEntryExitYesterdayToTomorrow = GetEmployeeEntryExitForTimeSheet(searchModel.EmployeeId, searchModel.OnCallDate);
            if (employeeEntryExitYesterdayToTomorrow.TodayList.Count() == 0)
            {
                return result;
                // throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات ورود-خروج"));
            }
            List<EmployeeEntryExitViewModel> entryExitData = new List<EmployeeEntryExitViewModel>();
            var entryExitToday = employeeEntryExitYesterdayToTomorrow.TodayList.Where(x => x.EntryTime != null && x.ExitTime != null).OrderBy(x => x.EntryTime).ToList();
            if (entryExitToday.Count() != 0)
            {
                entryExitData.AddRange(entryExitToday);
            }
            var lastEntryExitToday = employeeEntryExitYesterdayToTomorrow.TodayList.OrderBy(x => x.EntryTime).Last();

            if (lastEntryExitToday.ExitTime == null && employeeEntryExitYesterdayToTomorrow.TomorrowList.Count() != 0 && employeeEntryExitYesterdayToTomorrow.TomorrowList.First().EntryTime == null)
            {
                var firstTomorrowList = employeeEntryExitYesterdayToTomorrow.TomorrowList.First();
                entryExitData.Add(new EmployeeEntryExitViewModel()
                {
                    EntryId = lastEntryExitToday.EntryId,
                    ExitId = firstTomorrowList.ExitId,
                    EntryTime = lastEntryExitToday.EntryTime,
                    ExitTime = firstTomorrowList.ExitTime,
                    EntryDate = searchModel.OnCallDate,
                    ExitDate = employeeEntryExitYesterdayToTomorrow.TomorrowDate
                });
            }
            if (entryExitData.Count() == 0)
            {
                return result;
                //throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات ورود-خروج"));
            }
            List<EmployeeEntryExitViewModel> dataListValid = new List<EmployeeEntryExitViewModel>();

            // گرفتن تنظیمات شیفت
            if (employee.WorkCityId == null)
            {
                return result;
                //throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات شهر محل کار پرسنل"));
            }

            //
            var employeeWorkGroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupByEmployeeIdDateIncludeByWorkGroup(employee.Id, searchModel.OnCallDate);
            if (employeeWorkGroup == null)

            {
                return result;
            }
            var shiftConceptDetail = await _kscHrUnitOfWork.ShiftConceptDetailRepository.GetShiftConceptDetailWithWOrkGroupIdDate(employeeWorkGroup.WorkGroupId, workCalendar.Id);
            if (shiftConceptDetail == null)
            {
                return result;
            }
            var timeShiftSetting = await _kscHrUnitOfWork.TimeShiftSettingRepository.GetShiftDateTimeSettingAsync(employee.Id, shiftConceptDetail.Id, employee.WorkCityId.Value, employeeWorkGroup.WorkGroupId, workCalendar.Id).ConfigureAwait(false);
            //
            if (shiftConceptDetail.OncallCheckFree || (timeShiftSetting.IsRestShift && timeShiftSetting.ShiftSettingFromShiftboard == false)) // هر زمانی قابل پذیرش باشد یا غیر شیفتی باشد و روز استراحتش باشد 
            {
                dataListValid.AddRange(entryExitData);
            }
            else
            {
                // ورود-خروج قبل از شروع شیفت معتبر باشد
                if (shiftConceptDetail.OncallCheckBeforeShiftStart)
                {
                    if (entryExitToday.Count() != 0)
                    {
                        var entryExitBeforeStartShiftData = entryExitToday.Where(x => x.ExitTime.ConvertStringToTimeSpan() <= timeShiftSetting.ShiftStartTime.ConvertStringToTimeSpan());
                        if (entryExitBeforeStartShiftData.Count() != 0)
                        {
                            dataListValid.AddRange(entryExitBeforeStartShiftData);
                        }
                    }
                    //
                }
                //
                //  ورود بعد از پایان شیفت معتبر باشد
                if (shiftConceptDetail.OncallCheckAfterShiftEnd)
                {
                    var entryExitAfterEndShiftData = entryExitData.Where(x => timeShiftSetting.ShiftEndDate.Date == timeShiftSetting.ShiftStartDate.Date
                    && x.EntryTime.ConvertStringToTimeSpan() > timeShiftSetting.ShiftEndTime.ConvertStringToTimeSpan());
                    if (entryExitAfterEndShiftData.Count() != 0)
                    {
                        dataListValid.AddRange(entryExitAfterEndShiftData);
                    }
                }
                //  ورود بعد از شروع شیفت معتبر باشد
                if (shiftConceptDetail.OncallCheckAfterShiftStart)
                {

                    var entryExitAfterStartShiftData = entryExitData.Where(x => x.EntryTime.ConvertStringToTimeSpan() >= timeShiftSetting.ShiftStartTime.ConvertStringToTimeSpan());
                    if (entryExitAfterStartShiftData.Count() != 0)
                    {
                        dataListValid.AddRange(entryExitAfterStartShiftData);
                    }
                }
            }
            if (dataListValid.Count() == 0)
            {
                return result;
                //   throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "اطلاعات ورود-خروج مربوط به اضافه کار"));
            }
            dataListValid = dataListValid.OrderBy(x => x.ExitDate).ThenBy(x => x.EntryTime).ToList();
            var onCallRequest = _kscHrUnitOfWork.OnCall_RequestRepository.GetById(searchModel.OnCallRequestId);
            //
            var dataListInValid = dataListValid.Where(x => x.EntryTime == x.ExitTime && x.EntryDate == x.ExitDate).ToList();
            dataListValid = dataListValid.Except(dataListInValid).ToList();
            //
            foreach (var item in dataListValid)
            {
                result.Add(new EntryExitResult()
                {
                    Id = item.EntryId,
                    EntryTime = item.EntryTime,
                    ExitTime = item.ExitTime,
                    EntryDateString = item.EntryDate.ToLongPersianDateString(),//persian
                    ExitDateString = item.ExitDate.ToLongPersianDateString(),//persian
                    EntryDate = item.EntryDate,
                    ExitDate = item.ExitDate,
                    Selected = onCallRequest.OnCallStartTime == item.EntryTime

                });
            }
            //

            //
            return result;
            //


        }
        #region

        #endregion


        //edit safe taeed farakhan

        public KscResult EditOnCallConfirm(List<EntryExitResult> model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            var entity = model.First();

            var onCall_Request = _kscHrUnitOfWork.OnCall_RequestRepository.GetRequestByRequestId(entity.WfRequestId);
            onCall_Request.OnCallStartDate = entity.EntryDate;
            onCall_Request.OnCallStartTime = int.Parse(entity.EntryTime).ToString("00:00");
            onCall_Request.OnCallEndDate = entity.EntryDate;
            onCall_Request.OnCallEndTime = int.Parse(entity.ExitTime).ToString("00:00");

            //if (onCall_Request.WF_Request.StatusId==1)
            //{
            //    //onCall_Request.WF_Request

            //    var inputMisApiModel = new InputMisApiModel
            //    {
            //        NUM_PRSN_EMPL = onCall_Request.WF_Request.Employee.EmployeeNumber.ToString(),
            //        FUNCTION = "EMPL_UPPER_POST",
            //        domain = "KSC"
            //    };
            //    CALLING_RPC outPutMIS = GetPersonalDataMis(inputMisApiModel);
            //}

            //var wF_RequestHistory = onCall_Request.WF_Request.WF_RequestHistories.First();
            //wF_RequestHistory.WF_RequestHistories.Add(new WF_RequestHistory()
            //{
            //    StatusId = 1,
            //    StartUser = entity.CurrentUserName,
            //    StartDate = DateTime.Now,
            //    InsertDate = DateTime.Now,
            //});
            //wF_RequestHistory.ParentId= _kscHrUnitOfWork.WF_RequestHistoryRepository.FindParentIdInRequestHistory(entity.WfRequestId)
            //    onCall_Request


            ////
            //if (ExistsByCode(model.Id, model.Code) == true)
            //{

            //    result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
            //    return result;
            //}

            //if (Exists(model.Id, model.Title) == true)
            //{

            //    result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
            //    return result;
            //}

            //var oneShiftConcept = GetOne(model.Id);
            //if (oneShiftConcept == null)
            //{
            //    result.AddError("رکورد حذف شده", "رکورد حذف شده است");
            //    return result;
            //}
            //oneShiftConcept.Code = model.Code;
            //oneShiftConcept.Title = model.Title;
            //oneShiftConcept.IsActive = model.IsActive;

            ////var ShiftConcept = _mapper.Map<ShiftConcept>(model);
            //oneShiftConcept.UpdateDate = DateTime.Now;
            //oneShiftConcept.UpdateUser = model.CurrentUserName;


            //if (_kscHrUnitOfWork.SaveChanges() > 0)
            //    return result;
            //result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;

        }



        #region   jobCategorySpecific
        public async Task<List<JobCategorySpecificModel>> GetJobCategorySpecificModels(string employeeId)
        {
            var datetimeDetails = DateTime.Now.GetPersianMonthStartAndEndDates();
            var persunnelNumber = await _kscHrUnitOfWork.ViewMisEmployeeRepository.GetMisEmployeesByWinUser(employeeId).ConfigureAwait(false);
            if (persunnelNumber == null)
            {
                return new List<JobCategorySpecificModel>();
            }
            var employeeEnumrable = await _kscHrUnitOfWork.EmployeeRepository.WhereAsync(a => a.EmployeeNumber == persunnelNumber.EmployeeNumber).ConfigureAwait(false);
            var employeeSelected = employeeEnumrable.FirstOrDefault();
            var EmployeEntryExitEnumbrable = await _kscHrUnitOfWork.EmployeeEntryExitRepository.WhereAsync(a => a.EntryExitDate >= datetimeDetails.StartDate &&
                 a.EntryExitDate <= datetimeDetails.EndDate && a.PersonalNumber == persunnelNumber.EmployeeNumber).ConfigureAwait(false);
            var EmployeEntryExit = EmployeEntryExitEnumbrable.ToList();

            var rollCallDefinitionSkiped = new List<int>() { 85, 86, 87 };
            var workcalendarQuery = await _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarQuerable().AsNoTracking()
                .Where(a => a.MiladiDateV1 >= datetimeDetails.StartDate && a.MiladiDateV1 <= datetimeDetails.EndDate).OrderBy(a => a.MiladiDateV1).Select(a => new JobCategorySpecificModel()
                {
                    WorkCalendarId = a.Id,
                    EntryExitDate = a.MiladiDateV1,
                    PersianEntryExitDate = a.ShamsiDateV1,
                    DayTitle = a.DayNameShamsi,
                    DayNumber = a.DayOfMonthShamsi,
                    MonthNumber = a.MonthNameShamsiV1,
                    IsExist = a.EmployeeAttendAbsenceItems.Any(a => a.EmployeeId == employeeSelected.Id),
                    jobCategorySpecificEntryExitModels = new List<JobCategorySpecificEntryExitModel>()
                }).ToListAsync().ConfigureAwait(false);

            foreach (var item in workcalendarQuery)
            {
                if (EmployeEntryExit.Any(a => a.EntryExitDate == item.EntryExitDate))
                {
                    item.IsExist = true;
                    item.EmployeeId = employeeSelected.Id.ToString();
                    var model = EmployeEntryExit.Where(a => a.EntryExitDate == item.EntryExitDate).Select(a => new JobCategorySpecificEntryExitModel()
                    {
                        WorkCalendarId = item.WorkCalendarId,
                        EntryExitTime = a.EntryExitTime,
                        EntryExitTimeType = a.EntryExitType,
                        EmployeeId = a.EmployeeId.ToString()
                    }).Take(8).ToList();
                    item.jobCategorySpecificEntryExitModels.AddRange(model);
                    for (int i = model.Count / 2; i < 4; i++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            var models = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTimeType = j
                            };
                            item.jobCategorySpecificEntryExitModels.Add(models);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTimeType = j
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                        }
                    }
                }
            }
            return workcalendarQuery;
        }

        public async Task<List<JobCategorySpecificModel>> ReportMonthlyEntryExitPersonel(SearchEmployeeEntryExitModel filter)
        {

            //var datetimeDetails = DateTime.Now.GetPersianMonthStartAndEndDates();
            filter.StartDate = filter.StartDateString.Fa2En().ToGregorianDateTime();
            filter.EndDate = filter.EndDateString.Fa2En().ToGregorianDateTime();
            var persunnelNumber = new ViewMisEmployee();
            if (!string.IsNullOrEmpty(filter.CurrentUser))
            {
                persunnelNumber = await _kscHrUnitOfWork.ViewMisEmployeeRepository
                    .GetMisEmployeesByWinUser(filter.CurrentUser).ConfigureAwait(false);

            }

            else
            {
                persunnelNumber = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumber(filter.PersonalNumber);

            }
            if (persunnelNumber == null && filter.EmployeeId < 1)
            {
                return new List<JobCategorySpecificModel>();
            }
            var employeeEnumrable = _kscHrUnitOfWork.EmployeeRepository.GetEmployee().AsNoTracking();
            if (persunnelNumber != null)
            {
                employeeEnumrable = employeeEnumrable.Where(a => a.EmployeeNumber == persunnelNumber.EmployeeNumber);
            }
            if (filter.EmployeeId > 0)
            {
                employeeEnumrable = employeeEnumrable.Where(a => a.Id == filter.EmployeeId);
            }
            var employeeSelected = employeeEnumrable.FirstOrDefault();
            var EmployeEntryExitEnumbrable = await _kscHrUnitOfWork.EmployeeEntryExitRepository.WhereAsync(a => a.EntryExitDate >= filter.StartDate &&
                 a.EntryExitDate <= filter.EndDate && a.PersonalNumber == employeeSelected.EmployeeNumber).ConfigureAwait(false);
            var EmployeEntryExit = EmployeEntryExitEnumbrable.ToList();
            var workcalendarQuery = await _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarQuerable().AsNoTracking()
                .Where(a => a.MiladiDateV1 >= filter.StartDate && a.MiladiDateV1 <= filter.EndDate).Select(a => new JobCategorySpecificModel()
                {
                    WorkCalendarId = a.Id,
                    EntryExitDate = a.MiladiDateV1,
                    PersianEntryExitDate = a.ShamsiDateV1,
                    DayTitle = a.DayNameShamsi,

                    DayNumber = a.DayOfMonthShamsi,
                    MonthNumber = a.MonthNameShamsiV1,
                    //TeamCode= employeeSelected.TeamWork.Code,
                    YearNumber = a.YyyyShamsi.ToString(),
                    IsExist = a.EmployeeAttendAbsenceItems.Any(c => c.EmployeeId == employeeSelected.Id),
                    IsHaveAttendItem = a.EmployeeAttendAbsenceItems.Any(c => c.EmployeeId == employeeSelected.Id),
                    jobCategorySpecificEntryExitModels = new List<JobCategorySpecificEntryExitModel>()
                }).ToListAsync().ConfigureAwait(false);
            var filterModelSearchEntry = new SearchEmployeeEntryExitModel()
            {
                EmployeeId = employeeSelected.Id,
                StartDate = filter.StartDate,
                EndDate = filter.EndDate
            };

            var EntryExitByEmployees = GetEntryExitByEmployee(filterModelSearchEntry);
            foreach (var item in workcalendarQuery)
            {
                item.EmployeeId = employeeSelected.Id.ToString();
                if (EntryExitByEmployees.Any(a => a.EntryExitDate == item.EntryExitDate))
                {
                    item.IsExist = true;

                    var EntryExitByEmployeesFirst = EntryExitByEmployees.First(a => a.EntryExitDate == item.EntryExitDate);


                    for (int i = 0; i < 6; i++)
                    {
                        var countModel = i + 1;
                        if (EntryExitByEmployeesFirst.EmployeeEntryExitViewModel.Count() >= countModel)
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                IsCreatedManual = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryIsCreatedManual,
                                IsDeleted = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryIsDeleted,
                                UpdateDate = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryUpdateDate,
                                UpdateUser = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryUpdateUser,
                                EntryExitTime = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryTime
                                ,
                                EntryExitTimeType = 1,
                                EntryExitId = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryId,
                                EmployeeId = item.EmployeeId
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                            var model2 = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitTime,
                                EntryExitId = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitId,
                                EntryExitTimeType = 2,
                                IsCreatedManual = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitIsCreatedManual,
                                IsDeleted = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitIsDeleted,
                                UpdateDate = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitUpdateDate,
                                UpdateUser = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitUpdateUser,
                                EmployeeId = item.EmployeeId
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model2);
                        }
                        else
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = "",

                                IsDeleted = false,
                                EntryExitTimeType = 1,
                                EmployeeId = item.EmployeeId
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                            var model2 = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = "",
                                EntryExitTimeType = 2,
                                EmployeeId = item.EmployeeId,

                                IsDeleted = false,


                            };
                            item.jobCategorySpecificEntryExitModels.Add(model2);

                        }

                    }
                }
                else
                {

                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = "",
                                EntryExitTimeType = j,
                                EmployeeId = item.EmployeeId,

                                IsDeleted = false,
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                        }
                    }
                }
            }
            return workcalendarQuery;
        }

        //for  eslah vorood khorooj
        public async Task<List<JobCategorySpecificModel>> GetJobCategorySpecificModelsForOneDate(int employeeId, DateTime dateTime)
        {

            var workcalendarQuery = await _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarQuerable().AsNoTracking()
                .Where(a => a.MiladiDateV1 == dateTime.Date).Select(a => new JobCategorySpecificModel()
                {
                    WorkCalendarId = a.Id,
                    EntryExitDate = a.MiladiDateV1,
                    PersianEntryExitDate = a.ShamsiDateV1,
                    DayTitle = a.DayNameShamsi,
                    DayNumber = a.DayOfMonthShamsi,
                    MonthNumber = a.MonthNameShamsiV1,
                    IsExist = a.EmployeeAttendAbsenceItems.Any(c => c.EmployeeId == employeeId && c.WorkCalendarId == a.Id),
                    jobCategorySpecificEntryExitModels = new List<JobCategorySpecificEntryExitModel>()
                }).ToListAsync().ConfigureAwait(false);
            var persunnelNumber = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().First(a => a.Id == employeeId);
            var EmployeEntryExit = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetAllQueryable().AsQueryable()
                .Where(a => a.EntryExitDate == dateTime.Date && a.PersonalNumber == persunnelNumber.EmployeeNumber).AsNoTracking();
            foreach (var item in workcalendarQuery)
            {
                if (EmployeEntryExit.Any(a => a.EntryExitDate == item.EntryExitDate))
                {
                    item.IsExist = true;
                    var model = EmployeEntryExit.Where(a => a.EntryExitDate == item.EntryExitDate).Select(a => new JobCategorySpecificEntryExitModel()
                    {
                        WorkCalendarId = item.WorkCalendarId,
                        EntryExitTime = int.Parse(a.EntryExitTime).ToString("0000"),
                        EntryExitTimeType = a.EntryExitType,
                        EntryExitId = a.Id
                    }).Take(8).ToList();
                    item.jobCategorySpecificEntryExitModels.AddRange(model);
                    for (int i = model.Count / 2; i < 4; i++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            var models = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTimeType = j
                            };
                            item.jobCategorySpecificEntryExitModels.Add(models);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTimeType = j
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                        }
                    }
                }
            }
            return workcalendarQuery;
        }

        public async Task<KscResult> AddJobCategorySpecificModels(List<JobCategorySpecificEntryExitModel> model, string username)
        {
            var result = new KscResult();

            try
            {
                var groupedList = model.GroupBy(a => a.WorkCalendarId).Select(a => new
                {
                    WorkCalendarId = a.Key,
                    ListWorkCalendar = a.OrderBy(c => c.WorkCalendarId).ToList()
                }).ToList();
                var workcalendarIds = model.Select(a => a.WorkCalendarId).ToList();
                var workcalendarQuery = _kscHrUnitOfWork.WorkCalendarRepository.GetAllAsNoTracking()
                    .Where(a => workcalendarIds.Contains(a.Id)).ToList();
                var employeeId = int.Parse(model.First().EmployeeId);

                var employeeEnumrable = await _kscHrUnitOfWork.EmployeeRepository
                   .WhereAsync(a => a.Id == employeeId).ConfigureAwait(false);
                var employee = employeeEnumrable.FirstOrDefault();


                var miladiDate = workcalendarQuery.Select(a => a.MiladiDateV1).ToList();
                var employeeEntryExist = _kscHrUnitOfWork.EmployeeEntryExitRepository.Where(a => miladiDate.Any(c => c == a.EntryExitDate) && a.PersonalNumber == employee.EmployeeNumber).ToList();
                var rollCallDefinitionSkipped = new List<int>() { 85, 86, 87 };
                var AttendAbccenceItems = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Where(c => workcalendarIds.Contains(c.WorkCalendarId) &&
                c.EmployeeId == employee.Id
                && !rollCallDefinitionSkipped.Contains(c.RollCallDefinitionId)).ToList();


                foreach (var modelGroupedItem in groupedList)
                {
                    var list = new List<EmployeeEntryExit>();
                    foreach (var item in modelGroupedItem.ListWorkCalendar)
                    {


                        var datetime = workcalendarQuery.FirstOrDefault(a => a.Id == item.WorkCalendarId);
                        if (datetime == null) continue;
                        var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(datetime.Id).GetAwaiter().GetResult();
                        if (systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id
                            || systemStatus == EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id)
                        {
                            result.AddError("خطا", "کارکرد بسته شده است");
                            return result;
                        }
                        var isAny = employeeEntryExist.Any(a => item.EntryExitId == a.Id);
                        var isHaveAttendAbccence = AttendAbccenceItems.Any(c => c.WorkCalendarId == modelGroupedItem.WorkCalendarId);
                        if (isHaveAttendAbccence)
                        {
                            throw new HRBusinessException(Validations.RegularExpression, (" تایید کارکرد شده است" + datetime.ShamsiDateV1 + "کاربر در تاریخ "));

                        }
                        if (isAny)
                        {

                            var getone = employeeEntryExist.First(a => item.EntryExitId == a.Id);

                            if (string.IsNullOrEmpty(item.EntryExitTime))
                            {
                                getone.DeletedRemoteIpAddress = item.RemoteIpAddress;

                                _kscHrUnitOfWork.EmployeeEntryExitRepository.Delete(getone);

                            }
                            else
                            {
                                var entryExistTrueFormat = item.EntryExitTime.Replace(":", "");
                                if (int.Parse(entryExistTrueFormat) > 0 && item.EntryExitTime != getone.EntryExitTime)
                                {
                                    getone.UpdateRemoteIpAddress = item.RemoteIpAddress;
                                    getone.EntryExitTime = item.EntryExitTime;
                                }

                            }
                            continue;


                        };
                        var truformated = item.EntryExitTime.Replace(":", "");
                        var listduplicated = list.FirstOrDefault(c => c.EntryExitTime.CompareTo(truformated) < 0);
                        if (listduplicated != null)
                        {

                            if (item.EntryExitTimeType == listduplicated.EntryExitType)
                            {
                                throw new HRBusinessException(Validations.RepetitiveId, (" در ست نمی باشد" + datetime.ShamsiDateV1 + "ساختار ورود و خروج "));
                            }

                        }
                        var warmFromated = list.FirstOrDefault(c => c.EntryExitTime.CompareTo(item.EntryExitTime.Replace(":", "")) >= 0);
                        if (warmFromated != null)
                        {
                            throw new HRBusinessException(Validations.RegularExpression, (" در ست نمی باشد" + datetime.ShamsiDateV1 + "ساختار ورود و خروج "));

                        }

                        var data = new EmployeeEntryExit()
                        {
                            EntryExitType = item.EntryExitTimeType,
                            EntryExitTime = item.EntryExitTime,
                            EntryExitDate = datetime.MiladiDateV1,
                            EmployeeId = employee.Id,
                            PersonalNumber = employee.EmployeeNumber,
                            IsCreatedManual = true,
                            IsDeleted = false,
                            CreateDateTime = DateTime.Now,
                            CreateUser = employee.EmployeeNumber,
                            RemoteIpAddress = item.RemoteIpAddress,
                            CreateRemoteIpAddress = item.RemoteIpAddress,


                        };

                        list.Add(data);

                    }
                    if (list.Any())
                    {
                        await _kscHrUnitOfWork.EmployeeEntryExitRepository.AddRangeAsync(list);
                    }

                }
                await _kscHrUnitOfWork.SaveAsync(checklog: false);
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("", "خطایی ناشناخته رخ داده است");
                return result;
            }


        }

        ///////////////////////////// //پر کردن گرید تایید کارکرد
        public async Task<FilterResult<EmployeeEntryExistAbsencesModel>> GetEntryExitTeamForConfirmTimeSheet(SearchEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeEntryExistAbsencesModel>();
            try
            {
                var finalList = new List<EmployeeEntryExistAbsencesModel>();
                if (string.IsNullOrEmpty(Filter.TeamWorkCode) && !Filter.EmployeeIds.Any())
                {
                    return result;
                }
                if (!string.IsNullOrEmpty(Filter.TeamWorkCode))
                {
                    result = await GetAllUserTeamEntryExit(Filter);
                }
                if (Filter.EmployeeIds.Any())
                {
                    result = await GetUsersTeamEntryExit(Filter);
                }

            }
            catch (Exception ex)
            {

            }

            return result;


        }

        private async Task<FilterResult<EmployeeEntryExistAbsencesModel>> GetUsersTeamEntryExit(SearchEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeEntryExistAbsencesModel>();
            var finalList = new List<EmployeeEntryExistAbsencesModel>();


            Filter.EntryExitDate = Filter.EntryExitDateString.Fa2En().ToGregorianDateTime();
            var activeEmployeeTeamWroks = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable().Include(x => x.TeamWork)
                .Where(a => Filter.EmployeeIds.Contains(a.EmployeeId)).AsNoTracking();


            if (Filter.EntryExitDate.HasValue)
            {
                activeEmployeeTeamWroks = activeEmployeeTeamWroks.Where(a => a.TeamStartDate.Date <= Filter.EntryExitDate.Value.Date && (a.TeamEndDate.HasValue == false || a.TeamEndDate.Value.Date >= Filter.EntryExitDate.Value.Date));
            }

            var activePersonsId = activeEmployeeTeamWroks.Select(a => a.EmployeeId).ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking();


            //var shiftConceptDetails = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereQueryable(a => c);
            var finalEmployeeList = EmployeeList.ToList().OrderBy(x => int.Parse(x.EmployeeNumber)).ToList();
            //-------------
            var workCalendarid = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(Filter.EntryExitDate.Value); //workCalendarid


            //------------------

            var userTeamList = finalEmployeeList.Select(a => a.Id).ToList();
            var EmployeeAttendAbsenceItemList = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemByRelated()
                .Where(a => userTeamList.Contains(a.EmployeeId) && a.WorkCalendarId == workCalendarid.Id).AsNoTracking().ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var userInsertMis = EmployeeAttendAbsenceItemList.Where(c => !string.IsNullOrEmpty(c.InsertUser)).Select(c => c.InsertUser.ToLower()).ToList();

            var ViewMisUsersInserts = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.Where(a => userInsertMis.Contains(a.WindowsUser.ToLower())).ToList();

            var employeeWorkGroups = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup()
                .Where(a => userTeamList.Contains(a.EmployeeId) &&
                a.StartDate <= Filter.EntryExitDate.Value &&
                (a.EndDate >= Filter.EntryExitDate.Value || a.EndDate.HasValue == false)).ToListAsync().ConfigureAwait(false);

            var WorkGroupIds = employeeWorkGroups.Select(a => a.WorkGroupId).ToList();
            var ShiftConceptDetails = await _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdDate(WorkGroupIds, workCalendarid.Id);
            var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(workCalendarid.Id);

            var EntryExitByEmployees = GetEntryExitByEmployeelist(userTeamList, Filter.EntryExitDate.Value);
            var employeeEducationTime = _kscHrUnitOfWork.EmployeeEducationTimeRepository.WhereAsync(a => userTeamList.Contains(a.EmployeeId.Value) && a.WorkCalendarId == workCalendarid.Id && a.TrainingTypeId == 1 && a.IsDeleted == false).GetAwaiter().GetResult().ToList();
            var ShiftconceptDetailsIdInItem = EmployeeAttendAbsenceItemList.Select(a => a.ShiftConceptDetailId).ToList();
            var ShiftconceptDetailsInItem = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereAsync(a => ShiftconceptDetailsIdInItem.Contains(a.Id)).GetAwaiter().GetResult().ToList();
            foreach (var entry in finalEmployeeList)
            {
                var UserInsertDataIsManager = false;
                var isUserInsertet = false;
                var isExistEmployeeAttendAbsenceItem = EmployeeAttendAbsenceItemList.Any(x => x.EmployeeId == entry.Id);
                var employeeAttendAbsenceItem = EmployeeAttendAbsenceItemList.FirstOrDefault(x => x.EmployeeId == entry.Id);
                //  var workGroup = employeeWorkGroups.FirstOrDefault(x => x.WorkGroupId == entry.WorkGroupId);
                var workGroup = employeeWorkGroups.FirstOrDefault(x => x.EmployeeId == entry.Id);
                if (workGroup == null)
                {
                    continue;
                }
                var ShiftConceptDetail = new SearchShiftConceptModel();
                if (isExistEmployeeAttendAbsenceItem)
                {
                    ShiftConceptDetail = ShiftconceptDetailsInItem.Where(a => a.Id == employeeAttendAbsenceItem.ShiftConceptDetailId)
                        .Select(a => new SearchShiftConceptModel()
                        {
                            Id = a.Id,
                            Code = a.Code,
                            Title = a.Title,
                        }).First();
                }
                else
                {
                    ShiftConceptDetail = ShiftConceptDetails.FirstOrDefault(a => a.WorkGroupCode == workGroup.WorkGroup.Code);
                }

                if (employeeAttendAbsenceItem != null)
                {
                    isUserInsertet = employeeAttendAbsenceItem.InsertUser != null ? employeeAttendAbsenceItem.InsertUser.ToLower() == Filter.CurrentUser.ToLower() : false;

                }

                var isHaveEmployeeEducationTime = employeeEducationTime.Any(a => a.EmployeeId == entry.Id);//مشخص کردن تایید کارکرد آموزش
                var rollcalcategoryCode = "";
                if (isHaveEmployeeEducationTime)
                {
                    rollcalcategoryCode = "8";
                }
                var model = new EmployeeEntryExistAbsencesModel()
                {
                    EmployeeId = entry.Id,
                    FullName = entry.Name + " " + entry.Family,
                    PersonalNumber = int.Parse(entry.EmployeeNumber.Trim()),
                    TeamWorkId = entry.TeamWorkId.Value,
                    WorkCalendarId = workCalendarid.Id,
                    IsExistEmployeeAttendAbsenceItem = isExistEmployeeAttendAbsenceItem,
                    RollCallDefinitionId = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinitionId : 0,
                    RollCallDefinitionCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.Code : null,
                    RollCallCategoryCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.RollCallCategory.Code : rollcalcategoryCode,
                    ColorCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.RollCallCategory.ColorCode : "#fff",
                    WorkTimeId = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.WorkTimeId : workGroup.WorkGroup.WorkTimeId,
                    WorkTimeTitle = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.WorkTime.Title : workGroup.WorkGroup.WorkTime.Title,
                    WorkGroupId = workGroup.WorkGroupId,
                    WorkGroupCode = workGroup.WorkGroup.Code,
                    ShiftConceptDetail = ShiftConceptDetail,
                    ShiftConceptDetailsId = ShiftConceptDetail.Id,
                    ShiftConceptDetailsCode = ShiftConceptDetail.Code,
                    ShiftConceptDetailsTitle = ShiftConceptDetail.Title,
                    OldShiftConceptDetail = ShiftConceptDetail,
                    OldShiftConceptDetailId = ShiftConceptDetail.Id,
                    OldShiftConceptDetailCode = ShiftConceptDetail.Code,
                    OldShiftConceptDetailTitle = ShiftConceptDetail.Title,
                    WorkCityId = entry.WorkCityId,
                    IsUserInsertData = isUserInsertet,
                    IsUserManagerTeam = false,
                    UserInsertDataIsManager = UserInsertDataIsManager,
                    IsOfficialAttendAbcense = Filter.IsOfficialAttendAbcense,
                    ActiveForAllUser = systemStatus == EnumSystemSequenceStatusDailyTimeSheet.ActiveForAllUser.Id,
                    ActiveForOfficialUser = systemStatus == EnumSystemSequenceStatusDailyTimeSheet.ActiveForOfficialUser.Id,
                    IshaveLongTearm = employeeAttendAbsenceItem == null ? false : employeeAttendAbsenceItem.EmployeeLongTermAbsenceId.HasValue,
                    IsValidForDeleteAbsenceItem = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.IsValidForDeleteAbsenceItem : true,

                };

                var filterModel = new EmployeeEntryExitManagementInputModel()
                {
                    EntryExitDate = Filter.EntryExitDate.Value,
                    EmployeeId = entry.Id
                };
                if (EntryExitByEmployees.Any(a => a.EmployeeId == entry.Id))
                {
                    var modifiedEntryExits = EntryExitByEmployees.First(a => a.EmployeeId == entry.Id).EmployeeEntryExitViewModel.Take(3).ToList();
                    model.Entry1 = modifiedEntryExits.Count() >= 1 ? modifiedEntryExits[0].EntryTime : "";
                    model.Exist1 = modifiedEntryExits.Count() >= 1 ? modifiedEntryExits[0].ExitTime : "";
                    model.Entry2 = modifiedEntryExits.Count() >= 2 ? modifiedEntryExits[1].EntryTime : "";
                    model.Exist2 = modifiedEntryExits.Count() >= 2 ? modifiedEntryExits[1].ExitTime : "";
                    model.Entry3 = modifiedEntryExits.Count() >= 3 ? modifiedEntryExits[2].EntryTime : "";
                    model.Exist3 = modifiedEntryExits.Count() >= 3 ? modifiedEntryExits[2].ExitTime : "";

                }
                finalList.Add(model);
            }
            var modelResult = new FilterResult<EmployeeEntryExistAbsencesModel>
            {
                Data = finalList,
                Total = result.Total
            };

            return modelResult;
        }

        private async Task<FilterResult<EmployeeEntryExistAbsencesModel>> GetAllUserTeamEntryExit(SearchEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeEntryExistAbsencesModel>();
            var finalList = new List<EmployeeEntryExistAbsencesModel>();
            var teamWorkDecimal = Convert.ToDecimal(Filter.TeamWorkCode);
            var query = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => a.TeamCode == teamWorkDecimal);
            var isUserManagerTeam = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository
                .Any(a => a.TeamCode == teamWorkDecimal && a.WindowsUser == Filter.CurrentUser && (a.DisplaySecurity == 1 || a.DisplaySecurity == 2));
            if (Filter.IsOfficialAttendAbcense == false)
            {
                query = query.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUser);
            }

            var TeamCodeSelected = query.Select(a => a.TeamCode).FirstOrDefault();
            if (TeamCodeSelected == null)
            {
                return result;
            }
            var TeamCodeString = TeamCodeSelected.ToString();
            Filter.EntryExitDate = Filter.EntryExitDateString.Fa2En().ToGregorianDateTime();
            var activeEmployeeTeamWroks = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable().Include(x => x.TeamWork)
                .Where(a => a.TeamWork.Code == TeamCodeString).AsNoTracking();


            if (Filter.EntryExitDate.HasValue)
            {
                activeEmployeeTeamWroks = activeEmployeeTeamWroks.Where(a => a.TeamStartDate.Date <= Filter.EntryExitDate.Value.Date && (a.TeamEndDate.HasValue == false || a.TeamEndDate.Value.Date >= Filter.EntryExitDate.Value.Date));
            }

            var activePersonsId = activeEmployeeTeamWroks.Select(a => a.EmployeeId).ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking();


            //var shiftConceptDetails = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereQueryable(a => c);
            var finalEmployeeList = EmployeeList.ToList().OrderBy(x => int.Parse(x.EmployeeNumber)).ToList();
            //-------------
            var workCalendarid = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(Filter.EntryExitDate.Value); //workCalendarid


            //------------------

            var userTeamList = finalEmployeeList.Select(a => a.Id).ToList();
            var EmployeeAttendAbsenceItemList = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .GetEmployeeAttendAbsenceItemByRelated()
                .Where(a => userTeamList.Contains(a.EmployeeId) && a.WorkCalendarId == workCalendarid.Id).AsNoTracking().ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var userInsertMis = EmployeeAttendAbsenceItemList.Where(c => !string.IsNullOrEmpty(c.InsertUser)).Select(c => c.InsertUser.ToLower()).ToList();

            var ViewMisUsersInserts = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.Where(a => userInsertMis.Contains(a.WindowsUser.ToLower())).ToList();

            var employeeWorkGroups = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup()
                .Where(a => userTeamList.Contains(a.EmployeeId) &&
                a.StartDate <= Filter.EntryExitDate.Value &&
                (a.EndDate >= Filter.EntryExitDate.Value || a.EndDate.HasValue == false)).ToListAsync().ConfigureAwait(false);

            var WorkGroupIds = employeeWorkGroups.Select(a => a.WorkGroupId).ToList();
            var ShiftConceptDetails = await _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdDate(WorkGroupIds, workCalendarid.Id);
            var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(workCalendarid.Id);

            var EntryExitByEmployees = GetEntryExitByEmployeelist(userTeamList, Filter.EntryExitDate.Value);
            var employeeEducationTime = _kscHrUnitOfWork.EmployeeEducationTimeRepository.WhereAsync(a => userTeamList.Contains(a.EmployeeId.Value) && a.WorkCalendarId == workCalendarid.Id && a.TrainingTypeId == 1 && a.IsDeleted == false).GetAwaiter().GetResult().ToList();
            var ShiftconceptDetailsIdInItem = EmployeeAttendAbsenceItemList.Select(a => a.ShiftConceptDetailId).ToList();
            var ShiftconceptDetailsInItem = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereAsync(a => ShiftconceptDetailsIdInItem.Contains(a.Id)).GetAwaiter().GetResult().ToList();
            //var currentUserDetail=_kscHrUnitOfWork.EmployeeRepository.GetEmployee().
            //var isMaTm = _kscHrUnitOfWork.EmployeeRepository.GetPersonalDataMis(new InputMisApiModel() { NUM_PRSN_EMPL = Filter.EmployeeNumber, FUNCTION = "FETCH_GENERAL", domain = "KSC" })
            foreach (var entry in finalEmployeeList)
            {
                var UserInsertDataIsManager = false;
                var isUserInsertet = false;
                var isExistEmployeeAttendAbsenceItem = EmployeeAttendAbsenceItemList.Any(x => x.EmployeeId == entry.Id);
                var employeeAttendAbsenceItem = EmployeeAttendAbsenceItemList.FirstOrDefault(x => x.EmployeeId == entry.Id);
                var employeeAttendAbsenceItemByEmployeeId = EmployeeAttendAbsenceItemList.Where(x => x.EmployeeId == entry.Id);
                //  var workGroup = employeeWorkGroups.FirstOrDefault(x => x.WorkGroupId == entry.WorkGroupId);
                var workGroup = employeeWorkGroups.FirstOrDefault(x => x.EmployeeId == entry.Id);
                if (workGroup == null)
                {
                    continue;
                }
                var ShiftConceptDetail = new SearchShiftConceptModel();
                if (isExistEmployeeAttendAbsenceItem)
                {
                    ShiftConceptDetail = ShiftconceptDetailsInItem.Where(a => a.Id == employeeAttendAbsenceItem.ShiftConceptDetailId)
                        .Select(a => new SearchShiftConceptModel()
                        {
                            Id = a.Id,
                            Code = a.Code,
                            Title = a.Title,
                        }).First();
                }
                else
                {
                    ShiftConceptDetail = ShiftConceptDetails.FirstOrDefault(a => a.WorkGroupCode == workGroup.WorkGroup.Code);
                }

                if (employeeAttendAbsenceItem != null)
                {
                    isUserInsertet = employeeAttendAbsenceItem.InsertUser != null ? employeeAttendAbsenceItem.InsertUser.ToLower() == Filter.CurrentUser.ToLower() : false;

                }
                if (employeeAttendAbsenceItem != null)
                {
                    // UserInsertDataIsManager = employeeAttendAbsenceItem.InsertUser != null ? ViewMisUsersInserts.Any(c => employeeAttendAbsenceItem.InsertUser.ToLower() == c.WindowsUser.ToLower() && (c.DisplaySecurity == 1 || c.DisplaySecurity == 2)):false;
                    UserInsertDataIsManager = isUserManagerTeam;
                }
                var isHaveEmployeeEducationTime = employeeEducationTime.Any(a => a.EmployeeId == entry.Id);//مشخص کردن تایید کارکرد آموزش
                var rollcalcategoryCode = "";
                if (isHaveEmployeeEducationTime)
                {
                    rollcalcategoryCode = "8";
                }
                var model = new EmployeeEntryExistAbsencesModel()
                {
                    EmployeeId = entry.Id,
                    FullName = entry.Name + " " + entry.Family,
                    PersonalNumber = int.Parse(entry.EmployeeNumber.Trim()),
                    TeamWorkId = entry.TeamWorkId.Value,
                    WorkCalendarId = workCalendarid.Id,
                    IsExistEmployeeAttendAbsenceItem = isExistEmployeeAttendAbsenceItem,
                    RollCallDefinitionId = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinitionId : 0,
                    RollCallDefinitionCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.Code : null,
                    RollCallCategoryCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.RollCallCategory.Code : rollcalcategoryCode,
                    ColorCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.RollCallCategory.ColorCode : "#fff",
                    WorkTimeId = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.WorkTimeId : workGroup.WorkGroup.WorkTimeId,
                    WorkTimeTitle = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.WorkTime.Title : workGroup.WorkGroup.WorkTime.Title,
                    WorkGroupId = workGroup.WorkGroupId,
                    WorkGroupCode = workGroup.WorkGroup.Code,
                    ShiftConceptDetail = ShiftConceptDetail,
                    ShiftConceptDetailsId = ShiftConceptDetail.Id,
                    ShiftConceptDetailsCode = ShiftConceptDetail.Code,
                    ShiftConceptDetailsTitle = ShiftConceptDetail.Title,
                    OldShiftConceptDetail = ShiftConceptDetail,
                    OldShiftConceptDetailId = ShiftConceptDetail.Id,
                    OldShiftConceptDetailCode = ShiftConceptDetail.Code,
                    OldShiftConceptDetailTitle = ShiftConceptDetail.Title,
                    WorkCityId = entry.WorkCityId,
                    IsUserInsertData = isUserInsertet,
                    IsUserManagerTeam = isUserManagerTeam,
                    UserInsertDataIsManager = UserInsertDataIsManager,
                    IsOfficialAttendAbcense = Filter.IsOfficialAttendAbcense,
                    IshaveLongTearm = employeeAttendAbsenceItem == null ? false : employeeAttendAbsenceItem.EmployeeLongTermAbsenceId.HasValue,

                    ActiveForAllUser = systemStatus == EnumSystemSequenceStatusDailyTimeSheet.ActiveForAllUser.Id,
                    ActiveForOfficialUser = systemStatus == EnumSystemSequenceStatusDailyTimeSheet.ActiveForOfficialUser.Id,
                    //IsValidForDeleteAbsenceItem= employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.IsValidForDeleteAbsenceItem : true,
                    IsValidForDeleteAbsenceItem = employeeAttendAbsenceItemByEmployeeId.Any() ?
                    !employeeAttendAbsenceItemByEmployeeId.Any(x => x.RollCallDefinition.IsValidForDeleteAbsenceItem == false) : true,

                };

                var filterModel = new EmployeeEntryExitManagementInputModel()
                {
                    EntryExitDate = Filter.EntryExitDate.Value,
                    EmployeeId = entry.Id
                };
                if (EntryExitByEmployees.Any(a => a.EmployeeId == entry.Id))
                {
                    var modifiedEntryExits = EntryExitByEmployees.First(a => a.EmployeeId == entry.Id).EmployeeEntryExitViewModel.Take(3).ToList();
                    model.Entry1 = modifiedEntryExits.Count() >= 1 ? modifiedEntryExits[0].EntryTime : "";
                    model.Exist1 = modifiedEntryExits.Count() >= 1 ? modifiedEntryExits[0].ExitTime : "";
                    model.Entry2 = modifiedEntryExits.Count() >= 2 ? modifiedEntryExits[1].EntryTime : "";
                    model.Exist2 = modifiedEntryExits.Count() >= 2 ? modifiedEntryExits[1].ExitTime : "";
                    model.Entry3 = modifiedEntryExits.Count() >= 3 ? modifiedEntryExits[2].EntryTime : "";
                    model.Exist3 = modifiedEntryExits.Count() >= 3 ? modifiedEntryExits[2].ExitTime : "";

                }
                finalList.Add(model);
            }
            var modelResult = new FilterResult<EmployeeEntryExistAbsencesModel>
            {
                Data = finalList,
                Total = result.Total
            };

            return modelResult;
        }

        ///////////////////////////// //پر کردن گرید تایید کارکرد
        public async Task<FilterResult<EmployeeEntryExistAbsencesModel>> GetEntryExitTeamForConfirmTimeSheetMobile(SearchEmployeeEntryExitModel Filter)
        {
            var result = new FilterResult<EmployeeEntryExistAbsencesModel>();
            try
            {
                var finalList = new List<EmployeeEntryExistAbsencesModel>();
                if (string.IsNullOrEmpty(Filter.TeamWorkCode))
                {
                    return result;
                }
                var teamWorkDecimal = Convert.ToDecimal(Filter.TeamWorkCode);
                var query = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => a.TeamCode == teamWorkDecimal);

                if (Filter.IsOfficialAttendAbcense == false)
                {
                    query = query.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUser);
                }

                var TeamCodeSelected = query.Select(a => a.TeamCode).FirstOrDefault();
                if (TeamCodeSelected == null)
                {
                    return result;
                }
                var TeamCodeString = TeamCodeSelected.ToString();
                Filter.EntryExitDate = Filter.EntryExitDateString.Fa2En().ToGregorianDateTime();
                var activeEmployeeTeamWroks = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable()
                    .Where(a => a.TeamWork.Code == TeamCodeString
                    && a.EmployeeId == Filter.EmployeeId)
                    .Include(x => x.TeamWork)
                    .AsNoTracking();

                if (Filter.EntryExitDate.HasValue)
                {
                    activeEmployeeTeamWroks = activeEmployeeTeamWroks.Where(a => a.TeamStartDate.Date <= Filter.EntryExitDate.Value.Date && (a.TeamEndDate.HasValue == false || a.TeamEndDate.Value.Date >= Filter.EntryExitDate.Value.Date));
                }

                var activePersonsId = activeEmployeeTeamWroks.Select(a => a.EmployeeId).ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking();

                var finalEmployeeList = EmployeeList.ToList().OrderBy(x => int.Parse(x.EmployeeNumber)).ToList();
                //-------------
                var workCalendarid = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(Filter.EntryExitDate.Value); //workCalendarid


                //------------------

                var userTeamList = finalEmployeeList.Select(a => a.Id).ToList();
                var EmployeeAttendAbsenceItemList = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                    .GetEmployeeAttendAbsenceItemByRelated()
                    .Where(a => userTeamList.Contains(a.EmployeeId) && a.WorkCalendarId == workCalendarid.Id).AsNoTracking().ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                var employeeWorkGroups = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup()
                    .Where(a => userTeamList.Contains(a.EmployeeId) &&
                    a.StartDate <= Filter.EntryExitDate.Value &&
                    (a.EndDate >= Filter.EntryExitDate.Value || a.EndDate.HasValue == false)).ToListAsync().ConfigureAwait(false);

                var WorkGroupIds = employeeWorkGroups.Select(a => a.WorkGroupId).ToList();
                var ShiftConceptDetails = await _shiftConceptDetailService.GetShiftConceptDetailWithWOrkGroupIdDate(WorkGroupIds, workCalendarid.Id);
                var systemStatus = await _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(workCalendarid.Id);

                var EntryExitByEmployees = GetEntryExitByEmployeelist(userTeamList, Filter.EntryExitDate.Value);
                var employeeEducationTime = _kscHrUnitOfWork.EmployeeEducationTimeRepository.WhereAsync(a => userTeamList.Contains(a.EmployeeId.Value) && a.WorkCalendarId == workCalendarid.Id && a.TrainingTypeId == 1 && a.IsDeleted == false).GetAwaiter().GetResult().ToList();
                var ShiftconceptDetailsIdInItem = EmployeeAttendAbsenceItemList.Select(a => a.ShiftConceptDetailId).ToList();
                var ShiftconceptDetailsInItem = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereAsync(a => ShiftconceptDetailsIdInItem.Contains(a.Id)).GetAwaiter().GetResult().ToList();
                //var currentUserDetail=_kscHrUnitOfWork.EmployeeRepository.GetEmployee().
                //var isMaTm = _kscHrUnitOfWork.EmployeeRepository.GetPersonalDataMis(new InputMisApiModel() { NUM_PRSN_EMPL = Filter.EmployeeNumber, FUNCTION = "FETCH_GENERAL", domain = "KSC" })
                foreach (var entry in finalEmployeeList)
                {

                    var isUserInsertet = false;
                    var isExistEmployeeAttendAbsenceItem = EmployeeAttendAbsenceItemList.Any(x => x.EmployeeId == entry.Id);
                    var employeeAttendAbsenceItem = EmployeeAttendAbsenceItemList.FirstOrDefault(x => x.EmployeeId == entry.Id);
                    var employeeAttendAbsenceItemByEmployeeId = EmployeeAttendAbsenceItemList.Where(x => x.EmployeeId == entry.Id);
                    var workGroup = employeeWorkGroups.FirstOrDefault(x => x.WorkGroupId == entry.WorkGroupId);
                    if (workGroup == null)
                    {
                        continue;
                    }
                    var ShiftConceptDetail = new SearchShiftConceptModel();
                    if (isExistEmployeeAttendAbsenceItem)
                    {
                        ShiftConceptDetail = ShiftconceptDetailsInItem.Where(a => a.Id == employeeAttendAbsenceItem.ShiftConceptDetailId)
                            .Select(a => new SearchShiftConceptModel()
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Title = a.Title,
                            }).First();
                    }
                    else
                    {
                        ShiftConceptDetail = ShiftConceptDetails.FirstOrDefault(a => a.WorkGroupCode == workGroup.WorkGroup.Code);
                    }

                    if (employeeAttendAbsenceItem != null)
                    {
                        isUserInsertet = employeeAttendAbsenceItem.InsertUser != null ? employeeAttendAbsenceItem.InsertUser.ToLower() == Filter.CurrentUser.ToLower() : false;

                    }
                    var isHaveEmployeeEducationTime = employeeEducationTime.Any(a => a.EmployeeId == entry.Id);//مشخص کردن تایید کارکرد آموزش
                    var rollcalcategoryCode = "";
                    if (isHaveEmployeeEducationTime)
                    {
                        rollcalcategoryCode = "8";
                    }
                    var model = new EmployeeEntryExistAbsencesModel()
                    {
                        EmployeeId = entry.Id,
                        FullName = entry.Name + " " + entry.Family,
                        PersonalNumber = int.Parse(entry.EmployeeNumber.Trim()),
                        TeamWorkId = entry.TeamWorkId.Value,
                        WorkCalendarId = workCalendarid.Id,
                        IsExistEmployeeAttendAbsenceItem = isExistEmployeeAttendAbsenceItem,
                        RollCallDefinitionId = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinitionId : 0,
                        RollCallDefinitionCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.Code : null,
                        RollCallCategoryCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.RollCallCategory.Code : rollcalcategoryCode,
                        ColorCode = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.RollCallDefinition.RollCallCategory.ColorCode : "#fff",
                        WorkTimeId = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.WorkTimeId : workGroup.WorkGroup.WorkTimeId,
                        WorkTimeTitle = employeeAttendAbsenceItem != null ? employeeAttendAbsenceItem.WorkTime.Title : workGroup.WorkGroup.WorkTime.Title,
                        WorkGroupId = workGroup.WorkGroupId,
                        WorkGroupCode = workGroup.WorkGroup.Code,
                        ShiftConceptDetail = ShiftConceptDetail,
                        OldShiftConceptDetail = ShiftConceptDetail,
                        WorkCityId = entry.WorkCityId,
                        IsUserInsertData = isUserInsertet,
                        IsOfficialAttendAbcense = Filter.IsOfficialAttendAbcense,
                        ActiveForAllUser = systemStatus == EnumSystemSequenceStatusDailyTimeSheet.ActiveForAllUser.Id,
                        ActiveForOfficialUser = systemStatus == EnumSystemSequenceStatusDailyTimeSheet.ActiveForOfficialUser.Id,
                        IsValidForDeleteAbsenceItem = employeeAttendAbsenceItemByEmployeeId.Any() ?
                    !employeeAttendAbsenceItemByEmployeeId.Any(x => x.RollCallDefinition.IsValidForDeleteAbsenceItem == false) : true,

                    };

                    var filterModel = new EmployeeEntryExitManagementInputModel()
                    {
                        EntryExitDate = Filter.EntryExitDate.Value,
                        EmployeeId = entry.Id
                    };

                    if (EntryExitByEmployees.Any(a => a.EmployeeId == entry.Id))
                    {
                        var modifiedEntryExits = EntryExitByEmployees.First(a => a.EmployeeId == entry.Id).EmployeeEntryExitViewModel.Take(3).ToList();
                        model.Entry1 = modifiedEntryExits.Count() >= 1 ? modifiedEntryExits[0].EntryTime : "";
                        model.Exist1 = modifiedEntryExits.Count() >= 1 ? modifiedEntryExits[0].ExitTime : "";
                        model.Entry2 = modifiedEntryExits.Count() >= 2 ? modifiedEntryExits[1].EntryTime : "";
                        model.Exist2 = modifiedEntryExits.Count() >= 2 ? modifiedEntryExits[1].ExitTime : "";
                        model.Entry3 = modifiedEntryExits.Count() >= 3 ? modifiedEntryExits[2].EntryTime : "";
                        model.Exist3 = modifiedEntryExits.Count() >= 3 ? modifiedEntryExits[2].ExitTime : "";

                    }
                    finalList.Add(model);
                }
                var modelResult = new FilterResult<EmployeeEntryExistAbsencesModel>
                {
                    Data = finalList,
                    Total = result.Total
                };

                return modelResult;
            }
            catch (Exception ex)
            {

            }

            return result;


        }


        //for  eslah vorood khorooj New
        public List<EmployeeEntryExitManagementModel> GetEmployeeEntryExitManagement(EmployeeEntryExitManagementInputModel inputModel)
        {
            var result = new List<EmployeeEntryExitManagementModel>();
            //_kscHrUnitOfWork.EmployeeEntryExitRepository.DisposeOperation();
            var employeeEntryExitByDate = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitByDate(inputModel.EmployeeId, inputModel.EntryExitDate).OrderBy(x => x.EntryExitTime).ToList();//;

            var persons = _kscHrUnitOfWork.ViewMisUserDefinitionRepository.GetAllQueryable()
                .Select(x => new { WinUser = x.WinUser.Trim().ToLower(), x.FirstName, x.LastName }).AsQueryable();

            var TmPerson = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetMisEmployeesByWinUser(inputModel.CurrentUserName).GetAwaiter().GetResult();

            inputModel.IsValidEditEntryExit = inputModel.IsValidEditEntryExit || TmPerson.JobCategoryCode.ToLower() == "tm";//ادیت ورود خروج برای مدیران  "tm" و کاربر اداری فعال است 
            foreach (var item in employeeEntryExitByDate)
            {
                var updateuser = item.UpdateUser?.ToLower();
                var deleteUser = item.DeletedUser?.ToLower();
                EmployeeEntryExitManagementModel data = new EmployeeEntryExitManagementModel();
                data.Id = item.Id;
                data.EntryExitType = item.EntryExitType;
                data.EntryExitTypeTitle = item.EntryExitType == 1 ? "ورود" : "خروج";
                data.EntryExitTime = item.EntryExitTime;
                data.IsDeleted = item.IsDeleted;
                data.IsCanBeInsert = inputModel.IsValidEditEntryExit;
                data.IsCreatedManual = item.IsCreatedManual;
                data.UpdateUser = persons.Where(x => x.WinUser == updateuser).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault() ?? item.UpdateUser;
                data.DeletedUser = persons.Where(x => x.WinUser == deleteUser).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault() ?? item.DeletedUser;
                //data.UpdateUser = item.UpdateUser;
                //data.DeletedUser = item.DeletedUser;
                if (item.EntryExitType == 1)
                {
                    if (result.Count() != 0 && result.Last().EntryExitType != 2)
                    {
                        EmployeeEntryExitManagementModel emptyRow = new EmployeeEntryExitManagementModel();
                        emptyRow.EntryExitType = 2;
                        emptyRow.IsCanBeInsert = inputModel.IsValidEditEntryExit;
                        result.Add(emptyRow);
                    }
                }
                else
                {
                    if (result.Count() == 0 || result.Last().EntryExitType != 1)
                    {
                        EmployeeEntryExitManagementModel emptyRow = new EmployeeEntryExitManagementModel();
                        emptyRow.EntryExitType = 1;
                        emptyRow.IsCanBeInsert = inputModel.IsValidEditEntryExit;
                        result.Add(emptyRow);

                    }
                }
                result.Add(data);

            }
            // در صورتیکه دسترسی به ویرایش ورود-خروج داشته یا کاربر اداری باشد
            if (inputModel.IsValidEditEntryExit)
            {
                var count = 20 - result.Count();
                for (var i = 0; i < count; i++)
                {
                    var item = new EmployeeEntryExitManagementModel();
                    if (i % 2 == 0 || (result.Any() && result.Last().EntryExitType == 2))
                    {
                        item.EntryExitType = 1;
                        item.IsCanBeInsert = inputModel.IsValidEditEntryExit;
                    }
                    else
                    {
                        item.EntryExitType = 2;
                        item.IsCanBeInsert = inputModel.IsValidEditEntryExit;
                    }
                    result.Add(item);
                }
            }
            return result;
        }



        //دکمه ویرایش ورود خروج

        public async Task<KscResult> EditEmployeeEntryExit(List<EmployeeEntryExitManagementModel> model)
        {
            // var flg = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetMisEmployeesByWinUser(model.use)

            var result = model.IsValid();
            if (!result.Success)
                return result;
            // جدید
            //var entryExitIds = model.Where(x => x.Id != 0).Select(x => x.Id).ToList();
            //if (entryExitIds.Count() != 0)
            //{
            //    var employeeEntryExitHasAttendAbsences = _kscHrUnitOfWork.EmployeeEntryExitAttendAbsenceRepository.GetEmployeeEntryExitHasAttendAbsences(entryExitIds);
            //    model = model.Where(x => employeeEntryExitHasAttendAbsences.Any(a => a == x.Id) == false).ToList();
            //}
            // پایان جدید

            //if
            //(employeeEntryExitAttendAbsence.Any(x => x.EmployeeEntryExitId == item.EntryId)
            //bool ismodify=false; //تغییر نداده

            try
            {
                foreach (var item in model)
                {

                    //var flag = employeeEntryExitAttendAbsence.Any(x => x.EmployeeEntryExitId == item.Id)
                    //if (flag==false)
                    //{

                    //}

                    if (item.Id > 0) //edit
                    {

                        result = EditEmployeeEntryExist(item);

                    }
                    else
                    {//
                        if (item.IsDeleted == false)// && item.IsCanBeInsert == true) 

                        {


                            var mm = item.EntryExistDate.Replace("/", "-").ToEnglishNumbers();
                            result = AddEmployeeEntryExist(item);
                        }
                    }

                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }

            return result;

        }
        public List<EmployeeEntryExitManagementModel> GetEmployeeEntryExitManagement1(EmployeeEntryExitManagementInputModel inputModel)
        {
            var result = new List<EmployeeEntryExitManagementModel>();
            var employeeEntryExitByDate = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitByDate(inputModel.EmployeeId, inputModel.EntryExitDate).ToList();
            for (int i = 0; i < employeeEntryExitByDate.Count(); i++)
            {
                EmployeeEntryExitManagementModel item = new EmployeeEntryExitManagementModel();
                item.Id = employeeEntryExitByDate[i].Id;
                item.EntryExitType = employeeEntryExitByDate[i].EntryExitType;
                item.EntryExitTime = employeeEntryExitByDate[i].EntryExitTime;
                item.IsDeleted = employeeEntryExitByDate[i].IsDeleted;
                item.IsCreatedManual = employeeEntryExitByDate[i].IsCreatedManual;

                if (i % 2 == 0)
                {
                    if (employeeEntryExitByDate[i].EntryExitType != 1)
                    {
                        EmployeeEntryExitManagementModel newItem = new EmployeeEntryExitManagementModel();
                        newItem.EntryExitType = 1;
                        result.Add(newItem);
                        result.Add(item);
                    }
                    else
                    {
                        if (result.LastOrDefault() != null && result.Last().EntryExitType != 2)
                        {
                            EmployeeEntryExitManagementModel newItem = new EmployeeEntryExitManagementModel();
                            newItem.EntryExitType = 2;
                            result.Add(newItem);
                        }
                        result.Add(item);
                    }
                }
                else
                {
                    if (employeeEntryExitByDate[i].EntryExitType != 2)
                    {
                        result.Add(item);
                        EmployeeEntryExitManagementModel newItem = new EmployeeEntryExitManagementModel();
                        newItem.EntryExitType = 2;
                        result.Add(newItem);
                    }
                    else
                    {
                        if (result.LastOrDefault() != null && result.Last().EntryExitType != 1)
                        {

                            EmployeeEntryExitManagementModel newItem = new EmployeeEntryExitManagementModel();
                            newItem.EntryExitType = 1;
                            result.Add(newItem);
                        }
                        result.Add(item);
                    }
                }


            }

            return result;
        }

        private KscResult AddEmployeeEntryExist(EmployeeEntryExitManagementModel model)
        {
            var FindUserDetails = _kscHrUnitOfWork.EmployeeRepository.GetById(model.EmployeeId);
            var miladiDate = (DateTime)model.EntryExistDate.ToGregorianDateTime();
            var employeeEntryExit = _kscHrUnitOfWork.EmployeeEntryExitRepository
                .GetEmployeeEntryExitValidByEmployeeIdDate(model.EmployeeId, miladiDate)
                .Any(x => x.EntryExitTime == model.EntryExitTime);
            var result = new KscResult();

            if (employeeEntryExit == true)
            {
                throw new HRBusinessException(Validations.RegularExpression, ("ساعت" + " " + model.EntryExitTime + " " + "تکراری می باشد"));

            }

            if (model.EntryExitType != 1 && model.EntryExitType != 2)
            {
                throw new HRBusinessException(Validations.RegularExpression, ("نوع ورود خروج ساعت" + " " + model.EntryExitTime + " " + "مشخص نشده است"));

            }

            //if (Exist())
            // if (employeeEntryExit == false)
            // {

            var item = new EmployeeEntryExit();
            item.IsDeleted = model.IsDeleted;
            item.IsCreatedManual = true;
            item.EntryExitTime = model.EntryExitTime;
            item.EntryExitType = model.EntryExitType;
            item.EntryExitDate = (DateTime)model.EntryExistDate.ToGregorianDateTime();
            item.PersonalNumber = FindUserDetails.EmployeeNumber;
            item.EmployeeId = model.EmployeeId;
            item.CreateDateTime = DateTime.Now;
            item.CreateUser = model.CurrentUserName;
            item.CreateAuthenticateUserName = model.AuthenticateUserName;
            // item.RemoteIpAddress = model.RemoteIpAddress;
            item.CreateRemoteIpAddress = model.RemoteIpAddress;
            _kscHrUnitOfWork.EmployeeEntryExitRepository.Add(item);
            //}
            return result;
        }


        private KscResult EditEmployeeEntryExist(EmployeeEntryExitManagementModel item)
        {
            var result = new KscResult();
            //if (Exist())
            var getone = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetById(item.Id);




            if (getone == null)
            {
                throw new Exception("رکورد حذف شده است");
            }
            if (getone.IsCreatedManual && item.IsDeleted == true)
            {
                _kscHrUnitOfWork.EmployeeEntryExitRepository.Delete(getone);

            }

            else
            {
                //if ( getone.EntryExitTime != item.EntryExitTime) //اگر دستی باشد ویرایش کند و قبلا این ای دی وجود داشته باشد امکان ادیت بدهد
                //{

                //getone.EntryExitTime = item.EntryExitTime;
                //getone.EntryExitType = item.EntryExitType;
                //}
                if (item.IsDeleted)
                {
                    getone.IsDeleted = item.IsDeleted;

                    getone.DeletedDate = DateTime.Now;
                    getone.DeletedUser = item.CurrentUserName;
                    getone.DeletedAuthenticateUserName = item.AuthenticateUserName;
                    getone.DeletedRemoteIpAddress = item.RemoteIpAddress;
                }
                else if (getone.EntryExitType != item.EntryExitType)
                {
                    getone.EntryExitType = item.EntryExitType;
                    getone.UpdateDate = DateTime.Now;
                    getone.IsDeleted = false;
                    getone.UpdateUser = item.CurrentUserName;
                    getone.UpdateAuthenticateUserName = item.AuthenticateUserName;
                    getone.UpdateRemoteIpAddress = item.RemoteIpAddress;
                }
                else
                {
                    getone.UpdateDate = DateTime.Now;
                    getone.IsDeleted = false;
                    getone.UpdateUser = item.CurrentUserName;
                    getone.UpdateAuthenticateUserName = item.AuthenticateUserName;
                    getone.UpdateRemoteIpAddress = item.RemoteIpAddress;
                }
            }

            getone.RemoteIpAddress = item.RemoteIpAddress;
            return result;
        }
        public async Task<List<JobCategorySpecificModel>> GetMonthlyEntryExitSpecificPersonel(SearchEmployeeEntryExitModel filter)
        {

            //var datetimeDetails = DateTime.Now.GetPersianMonthStartAndEndDates();

            var systemAttendDateItem = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate;
            var yearmonth = systemAttendDateItem;
            if (!string.IsNullOrEmpty(filter.YearMonth))
            {
                systemAttendDateItem = int.Parse(filter.YearMonth.Fa2En());
            }
            var workCalendarMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(systemAttendDateItem);
            filter.StartDate = workCalendarMonth.Item1;
            filter.EndDate = workCalendarMonth.Item2;
            var persunnelNumber = new ViewMisEmployee();
            if (!string.IsNullOrEmpty(filter.CurrentUser))
            {
                persunnelNumber = await _kscHrUnitOfWork.ViewMisEmployeeRepository
                    .GetMisEmployeesByWinUser(filter.CurrentUser).ConfigureAwait(false);

            }

            else
            {
                persunnelNumber = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumber(filter.PersonalNumber);

            }
            if (persunnelNumber == null && filter.EmployeeId < 1)
            {
                return new List<JobCategorySpecificModel>();
            }
            var employeeEnumrable = _kscHrUnitOfWork.EmployeeRepository.GetEmployee().AsNoTracking();
            if (persunnelNumber != null)
            {
                employeeEnumrable = employeeEnumrable.Where(a => a.EmployeeNumber == persunnelNumber.EmployeeNumber);
            }
            if (filter.EmployeeId > 0)
            {
                employeeEnumrable = employeeEnumrable.Where(a => a.Id == filter.EmployeeId);
            }
            var employeeSelected = employeeEnumrable.FirstOrDefault();
            var EmployeEntryExitEnumbrable = await _kscHrUnitOfWork.EmployeeEntryExitRepository.WhereAsync(a => a.EntryExitDate >= filter.StartDate &&
                 a.EntryExitDate <= filter.EndDate && a.PersonalNumber == employeeSelected.EmployeeNumber).ConfigureAwait(false);
            var EmployeEntryExit = EmployeEntryExitEnumbrable.ToList();
            var rollCallDefinitionSkipped = new List<int>() { 85, 86, 87 };
            var workcalendarQuery = await _kscHrUnitOfWork.WorkCalendarRepository.GetWorkCalendarQuerable().AsNoTracking()
                .Where(a => a.MiladiDateV1 >= filter.StartDate && a.MiladiDateV1 <= filter.EndDate).OrderBy(a => a.MiladiDateV1).Select(a => new JobCategorySpecificModel()
                {
                    WorkCalendarId = a.Id,
                    EntryExitDate = a.MiladiDateV1,
                    PersianEntryExitDate = a.ShamsiDateV1,
                    DayTitle = a.DayNameShamsi,

                    DayNumber = a.DayOfMonthShamsi,
                    MonthNumber = a.MonthNameShamsiV1,
                    //TeamCode= employeeSelected.TeamWork.Code,
                    YearNumber = a.YyyyShamsi.ToString(),
                    IsExist = a.EmployeeAttendAbsenceItems.Any(c => c.EmployeeId == employeeSelected.Id),
                    IsHaveAttendItem = a.EmployeeAttendAbsenceItems.Any(c => c.EmployeeId == employeeSelected.Id && !rollCallDefinitionSkipped.Contains(c.RollCallDefinitionId)),
                    jobCategorySpecificEntryExitModels = new List<JobCategorySpecificEntryExitModel>()
                }).ToListAsync().ConfigureAwait(false);
            var filterModelSearchEntry = new SearchEmployeeEntryExitModel()
            {
                EmployeeId = employeeSelected.Id,
                StartDate = filter.StartDate,
                EndDate = filter.EndDate
            };

            var EntryExitByEmployees = GetEntryExitByEmployee(filterModelSearchEntry);
            foreach (var item in workcalendarQuery)
            {
                item.EmployeeId = employeeSelected.Id.ToString();
                if (EntryExitByEmployees.Any(a => a.EntryExitDate == item.EntryExitDate))
                {
                    item.IsExist = true;

                    var EntryExitByEmployeesFirst = EntryExitByEmployees.First(a => a.EntryExitDate == item.EntryExitDate);


                    for (int i = 0; i < 6; i++)
                    {
                        var countModel = i + 1;
                        if (EntryExitByEmployeesFirst.EmployeeEntryExitViewModel.Count() >= countModel)
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                IsCreatedManual = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryIsCreatedManual,
                                IsDeleted = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryIsDeleted,
                                UpdateDate = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryUpdateDate,
                                UpdateUser = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryUpdateUser,
                                EntryExitTime = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryTime
                                ,
                                EntryExitTimeType = 1,
                                EntryExitId = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].EntryId,
                                EmployeeId = item.EmployeeId
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                            var model2 = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitTime,
                                EntryExitId = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitId,
                                EntryExitTimeType = 2,
                                IsCreatedManual = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitIsCreatedManual,
                                IsDeleted = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitIsDeleted,
                                UpdateDate = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitUpdateDate,
                                UpdateUser = EntryExitByEmployeesFirst.EmployeeEntryExitViewModel[i].ExitUpdateUser,
                                EmployeeId = item.EmployeeId
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model2);
                        }
                        else
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = "",

                                IsDeleted = false,
                                EntryExitTimeType = 1,
                                EmployeeId = item.EmployeeId
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                            var model2 = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = "",
                                EntryExitTimeType = 2,
                                EmployeeId = item.EmployeeId,

                                IsDeleted = false,


                            };
                            item.jobCategorySpecificEntryExitModels.Add(model2);

                        }

                    }
                }
                else
                {

                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            var model = new JobCategorySpecificEntryExitModel()
                            {
                                WorkCalendarId = item.WorkCalendarId,
                                EntryExitTime = "",
                                EntryExitTimeType = j,
                                EmployeeId = item.EmployeeId,

                                IsDeleted = false,
                            };
                            item.jobCategorySpecificEntryExitModels.Add(model);
                        }
                    }
                }
            }
            return workcalendarQuery;
        }

        #endregion

        public EmployeeEntryExitYesterdayToTomorrowModel GetEmployeeEntryExitForTimeSheet(int EmployeeId, DateTime date)
        {
            DateTime yesterDay = date.AddDays(-1);
            DateTime tomorrow = date.AddDays(1);
            var dataFromYesterdayToTomorrow = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitValidByEmployeeIdRangeDate(EmployeeId, yesterDay, tomorrow)
                            .Select(x => new EntryExitList()
                            {
                                type = x.EntryExitType,
                                time = x.EntryExitTime,
                                EntryExitDate = x.EntryExitDate,
                                Id = x.Id
                            }).ToList();
            #region Yesterday
            List<EmployeeEntryExitViewModel> entryExitYestrdayValidData = new List<EmployeeEntryExitViewModel>();
            List<EmployeeEntryExitViewModel> entryExitYestrdayList = new List<EmployeeEntryExitViewModel>();
            var yesterdayData = dataFromYesterdayToTomorrow.Where(x => x.EntryExitDate.Date == yesterDay.Date).ToList();
            entryExitYestrdayValidData = GetEntryExitPairModel(yesterdayData).ToList();


            #endregion
            #region Tomorrow
            List<EmployeeEntryExitViewModel> entryExitTomorrowValidData = new List<EmployeeEntryExitViewModel>();
            List<EmployeeEntryExitViewModel> entryExitTomorrowList = new List<EmployeeEntryExitViewModel>();
            var tomorrowData = dataFromYesterdayToTomorrow.Where(x => x.EntryExitDate.Date == tomorrow.Date).ToList();
            entryExitTomorrowValidData = GetEntryExitPairModel(tomorrowData).ToList();
            #endregion
            #region Today
            var todayData = dataFromYesterdayToTomorrow.Where(x => x.EntryExitDate.Date == date.Date).ToList();
            var entryExitToday = GetEntryExitPairModel(todayData).ToList();
            #endregion
            EmployeeEntryExitYesterdayToTomorrowModel model = new EmployeeEntryExitYesterdayToTomorrowModel()
            {
                TodayList = entryExitToday,
                YesterdayList = entryExitYestrdayValidData,
                TomorrowList = entryExitTomorrowValidData,
                TomorrowDate = tomorrow,
                YesterdayDate = yesterDay,
            };
            return model;

        }

        //•	اضافه کار  قهری پرسنل روزکار forcedTime Over
        public int? sumForcedOverTime1(EmployeeEntryExitManagementInputModel inputModel)
        {
            //•	اضافه کار  قهری پرسنل روزکار - 

            var miladiStartcurrentMonth = inputModel.EntryExitDate.GetPersianMonthStartAndEndDates().StartDate;

            //var StartcurrentMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetCurrentMonthStartInShamsi(inputModel.EntryExitDate);//1401/06/01
            //var miladiStartcurrentMonth = StartcurrentMonth != null ? StartcurrentMonth.MiladiDateV1 : (DateTime?)null;
            int? sumTimeDuration = 0;
            if (inputModel.EmployeeId > 0)
            {

                var rollCallDefinitionIds = _kscHrUnitOfWork.IncludedRollCallRepository.GetAllByRelated()
                 .Where(x => x.RollCallDefinitionId == EnumIncludedDefinition.ForcedOverTime.Id
                 && x.RollCallDefinition.RollCallCategoryId == EnumRollCallCategory.OverTime.Id
                 ).Select(w => w.RollCallDefinitionId).ToList();

                var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemByRelated()
                    .Where(x => x.EmployeeId == inputModel.EmployeeId && x.WorkCalendar.MiladiDateV1 >= miladiStartcurrentMonth
                    && x.WorkCalendar.MiladiDateV1 <= inputModel.EntryExitDate
                    && rollCallDefinitionIds.Contains(x.RollCallDefinitionId)
                    );

                //sumTimeDuration = employeeAttendAbsenceItem.Sum(x => x.TimeDuration.ConvertDurationToMinute());

                var timeDurations = employeeAttendAbsenceItem.Select(x => x.TimeDuration).ToList();

                foreach (var item in timeDurations)
                {
                    sumTimeDuration += item.ConvertDurationToMinute();
                }
            }
            return sumTimeDuration;
        }


        private static List<EmployeeEntryExitViewModel> GetEntryExitPairModel(List<EntryExitList> data)
        {
            List<EmployeeEntryExitViewModel> entryExitResult = new List<EmployeeEntryExitViewModel>();
            data = data.OrderBy(x => x.time).ToList();
            int j = 0;
            int k = 0;
            int count = data.Count();
            for (int i = 0; i < count; i = k)
            {
                EmployeeEntryExitViewModel row = new EmployeeEntryExitViewModel();
                k = i + 1;
                if (data[i].type == 1)
                {
                    j = i + 1;
                    row.EntryId = data[i].Id;
                    row.EntryTime = data[i].time;
                    row.EntryTimeToTimeSpan = data[i].time.ConvertStringToTimeSpan();
                    row.EntryDate = data[i].EntryExitDate.Date;
                    row.EntryIsCreatedManual = data[i].IsCreatedManual;
                    row.EntryIsDeleted = data[i].IsDeleted;
                    row.EntryUpdateDate = data[i].UpdateDate;
                    row.EntryUpdateUser = data[i].UpdateUser;


                    if (j != count && data[j].type == 2)
                    {
                        k = j + 1;
                        row.ExitTime = data[j].time;
                        row.ExitTimeToTimeSpan = data[j].time.ConvertStringToTimeSpan();
                        row.ExitDate = data[j].EntryExitDate.Date;
                        row.ExitId = data[j].Id;
                        row.ExitIsCreatedManual = data[j].IsCreatedManual;
                        row.ExitIsDeleted = data[j].IsDeleted;
                        row.ExitUpdateDate = data[j].UpdateDate;
                        row.ExitUpdateUser = data[j].UpdateUser;
                    }
                }
                else
                {
                    row.ExitId = data[i].Id;
                    row.ExitTime = data[i].time;
                    row.ExitTimeToTimeSpan = data[i].time.ConvertStringToTimeSpan();
                    row.ExitDate = data[i].EntryExitDate.Date;
                    row.IsCreatedManual = data[i].IsCreatedManual;
                    row.IsDeleted = data[i].IsDeleted;
                    row.UpdateDate = data[i].UpdateDate;
                    row.UpdateUser = data[i].UpdateUser;
                }
                row.RowGuid = Guid.NewGuid();
                entryExitResult.Add(row);
            }
            return entryExitResult;
        }

        public List<EmployeeEntryExitForAttendAbsenceItemModel> GetEntryExitByEmployeelist(List<int> employeeId, DateTime date)
        {
            List<EmployeeEntryExitForAttendAbsenceItemModel> entryExitListResult = new List<EmployeeEntryExitForAttendAbsenceItemModel>();
            var data = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetEmployeeEntryExitValidByDate(date).Where(x => employeeId.Contains(x.EmployeeId))
                           .Select(x => new EntryExitList()
                           {
                               EmployeeId = x.EmployeeId,
                               type = x.EntryExitType,
                               time = x.EntryExitTime,
                               EntryExitDate = x.EntryExitDate,
                               Id = x.Id
                           }).ToList();


            foreach (var item in data)
            {
                if (!entryExitListResult.Any(x => x.EmployeeId == item.EmployeeId))
                {
                    EmployeeEntryExitForAttendAbsenceItemModel entryExit = new EmployeeEntryExitForAttendAbsenceItemModel();
                    var dataByEmployeeId = data.Where(x => x.EmployeeId == item.EmployeeId).ToList();
                    // entryExit.EmployeeEntryExitViewModel = GetEntryExitPairModel(dataByEmployeeId).Where(y => y.EntryTime != null && y.ExitTime != null).OrderBy(x => x.EntryTime).ToList();
                    //entryExit.EmployeeEntryExitViewModel = GetEntryExitPairModel(dataByEmployeeId).OrderBy(x => x.EntryTime).ToList();
                    entryExit.EmployeeEntryExitViewModel = GetEntryExitPairModel(dataByEmployeeId).ToList();
                    entryExit.EmployeeId = item.EmployeeId;
                    entryExitListResult.Add(entryExit);
                }
            }
            return entryExitListResult;
        }


        public List<EmployeeEntryExitForAttendAbsenceItemModel> GetEntryExitByEmployee(SearchEmployeeEntryExitModel model)
        {
            List<EmployeeEntryExitForAttendAbsenceItemModel> entryExitListResult = new List<EmployeeEntryExitForAttendAbsenceItemModel>();
            var data = _kscHrUnitOfWork.EmployeeEntryExitRepository
                .GetEmployeeEntryExitValidByEmployeeIdRangeDateForReport(model.EmployeeId
                , model.StartDate.Value, model.EndDate.Value)
                           .Select(x => new EntryExitList()
                           {
                               EmployeeId = x.EmployeeId,
                               type = x.EntryExitType,
                               time = x.EntryExitTime,
                               EntryExitDate = x.EntryExitDate,
                               Id = x.Id,
                               IsDeleted = x.IsDeleted,
                               IsCreatedManual = x.IsCreatedManual,
                               UpdateDate = x.UpdateDate,
                               UpdateUser = x.UpdateUser


                           }).ToList().GroupBy(a => a.EntryExitDate).Select(a => new
                           {
                               EntryExitDate = a.Key,
                               EntryExitList = a.ToList()
                           }).ToList();
            foreach (var item in data)
            {
                EmployeeEntryExitForAttendAbsenceItemModel entryExit = new EmployeeEntryExitForAttendAbsenceItemModel();
                entryExit.EntryExitDate = item.EntryExitDate;
                entryExit.EmployeeId = model.EmployeeId;
                entryExit.EmployeeEntryExitViewModel = GetEntryExitPairModel(item.EntryExitList).ToList();
                entryExitListResult.Add(entryExit);

            }
            return entryExitListResult.ToList();
        }

        //GetItemDate

        public async Task<SearchEmployeeEntryExitModel> GetItemDate()  //تاریخ تایید کارکرد شده اول و پایان ماه را می دهد
        {
            var systemControlDate = await _kscHrUnitOfWork.SystemControlDateRepository.GetAllQueryable().AsQueryable()
                .FirstOrDefaultAsync();
            var attendAbsenceItemDate = systemControlDate.AttendAbsenceItemDate;
            var workcalendar = _kscHrUnitOfWork.WorkCalendarRepository.Where(a => a.YearMonthV1 == attendAbsenceItemDate)
                .First().MiladiDateV1.GetPersianMonthStartAndEndDates();

            var item = new SearchEmployeeEntryExitModel()
            {
                StartDate = workcalendar.StartDate,
                EndDate = workcalendar.EndDate
            };
            return item;
        }

        public List<EmployeeDontHaveExist> GEtPersenisNotHaveFamily(int yearmonth)
        {
            var result = new List<EmployeeDontHaveExist>();
            var workcalendar = _kscHrUnitOfWork.WorkCalendarRepository.Where(a => a.YearMonthV1 == yearmonth).ToList();
            var startEndYearMonth = workcalendar.First().MiladiDateV1.GetPersianMonthStartAndEndDates();
            var workcalendarId = workcalendar.Select(a => a.Id).ToList();


            var employees = _kscHrUnitOfWork.EmployeeRepository.GetEmployeesActivAndeHaveTeamAsNotracking()

                .Include(a => a.WorkGroup)
                .ThenInclude(a => a.WorkTime)
                .Where(a => a.WorkGroup.WorkTimeId == 1)
                .Include(a => a.EmployeeAttendAbsenceItems)
                .Where(a => a.EmployeeAttendAbsenceItems.Any(x => workcalendarId.Contains(x.WorkCalendarId)))
                .Include(a => a.TeamWork)
                 .AsNoTrackingWithIdentityResolution()
                .Select(a => new
                {
                    a.Id,
                    a.EmployeeNumber,
                    FullName = a.Name + " " + a.Family,
                    TeamWorkCode = a.TeamWork.Code,
                    WorkCalendarIds = a.EmployeeAttendAbsenceItems.Where(a => a.RollCallDefinitionId == 13 || a.RollCallDefinitionId == 13).Select(a => a.WorkCalendarId).ToList()
                }).ToList();

            var EmployeeIds = employees.Select(a => a.Id).ToList();
            var entrayExistEmployees = _kscHrUnitOfWork.EmployeeEntryExitRepository.GetAllQueryable()
            .Where(a => a.IsDeleted == false && EmployeeIds.Contains(a.EmployeeId) && a.EntryExitDate >= startEndYearMonth.StartDate && a.EntryExitDate <= startEndYearMonth.EndDate)
            .ToList();

            foreach (var item in employees)
            {


                var workCalenderDateonCal = workcalendar.Where(a => item.WorkCalendarIds.Contains(a.Id)).Select(a => a.MiladiDateV1).ToList();

                var entraysExists = entrayExistEmployees.Where(a => a.EmployeeId == item.Id ).ToList();

                var aa = entraysExists.GroupBy(a => a.EntryExitDate).Select(a => MapEntrayExist(a)).OrderBy(a => a.EntrayExistDate).ToList();

                foreach (var entrayexist in aa)
                {
                    

                    var nextDay = entrayexist.Count + 1;
                    var getnextDay = aa.FirstOrDefault(a => a.Count == nextDay);
                   

                    if (getnextDay == null)
                    {

                        break;
                    }
                  
                    if (entrayexist.IsEntray && getnextDay.FirstIsExist)
                    {
                        var isEntrayDateOncall = workCalenderDateonCal.Any(x => x == entrayexist.EntrayExistDate);
                        var isExistDateOncall = workCalenderDateonCal.Any(x => x == getnextDay.EntrayExistDate);
                        var itemResult = new EmployeeDontHaveExist()
                        {
                            EmployeeNumber = item.EmployeeNumber,
                            FullName = item.FullName,
                            TeamWorkCode = item.TeamWorkCode,
                            EntrayDate = entrayexist.EntrayExistDate.ToPersianDate(),
                            ExistDate = getnextDay.EntrayExistDate.ToPersianDate(),
                            IsExistDateOncall= isExistDateOncall,
                            IsEntrayDateOncall= isEntrayDateOncall

                        };
                        result.Add(itemResult);


                    }
                }

            }

            return result;
        }

        public int count { get; set; } = 0;
        private ModelEntrayExitDtoDorEmployee MapEntrayExist(IGrouping<DateTime, EmployeeEntryExit> a)
        {
            var result = new ModelEntrayExitDtoDorEmployee();
            count = count + 1;
            result.Count = count;
            result.EntrayExistDate = a.Key;
            result.LastEntrayExist = a.OrderByDescending(x => x.EntryExitTime).FirstOrDefault();
            result.IsEntray = result.LastEntrayExist.EntryExitType == 1;
            result.IsHaveExist = false;
          
            result.FirstEntryExist = a.OrderBy(x => x.EntryExitTime).FirstOrDefault();
            result.FirstIsExist = result.FirstEntryExist.EntryExitType == 2;
            return result;
        }


    }

    public class ModelEntrayExitDtoDorEmployee()
    {
        public DateTime EntrayExistDate { get; set; }
        public int Count { get; set; }
        public EmployeeEntryExit LastEntrayExist { get; set; }
        public EmployeeEntryExit FirstEntryExist { get; set; }

        public bool IsHaveExist { get; set; }
        public bool IsEntray { get; set; }
        public bool FirstIsExist { get; internal set; }
    }
}
