using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.ShiftBoard;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Repositories.WorkShift;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class ShiftBoardService : IShiftBoardService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ShiftBoardService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public bool Exists(int id, string title)
        {
            return _kscHrUnitOfWork.ShiftBoardRepository.Any(x => x.Id != id && x.IsActive == true);
        }
        public bool Exists(string title)
        {
            return _kscHrUnitOfWork.ShiftBoardRepository.Any(x => x.IsActive == true);
        }
        public bool ExistsCode(string code)
        {
            return _kscHrUnitOfWork.ShiftBoardRepository.Any(x => x.IsActive == true);
        }


        public List<WorkShiftViewModel> GetShift(searchMonthShift model)
        {
            List<WorkShiftViewModel> result = new List<WorkShiftViewModel>();

            var date_int = int.Parse(model.date);
            var cities = _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable()
                .AsQueryable()
                .Include(a => a.WorkGroup)
                .Include(a => a.WorkCalendar)
                .Include(a => a.ShiftConceptDetail)
                .Include(a => a.ShiftConceptDetail.ShiftConcept)
                 .Where(x =>
                 x.WorkCalendar.YearMonthV1 == date_int)
                 .ToList();


            foreach (var item in cities)
            {
                WorkShiftViewModel temp = new WorkShiftViewModel();
                temp.ShiftName = item.WorkGroup.Code;
                temp.YYYYMM = item.WorkCalendar.YearMonthV1.ToString();

                result.Add(temp);
            }
            return result;
            //return _mapper.Map<List<WorkShiftViewModel>>(cities);

        }
        public List<WorkShiftViewModel> GetShiftName(searchMonthShift model)
        {
            List<WorkShiftViewModel> result = new List<WorkShiftViewModel>();

            var date_int = int.Parse(model.date);
            var shiftBoards = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByYearMothNoTracking(date_int);
            var shiftNames = shiftBoards.Select(x => new { ShiftName = x.WorkGroup.Code }).Distinct();
            result = shiftNames.Select(x => new WorkShiftViewModel() { ShiftName = x.ShiftName }).ToList();
            return result;

        }
        public WorkShiftViewModel GetMonthShift(searchMonthShift model)
        {
            WorkShiftViewModel result = new WorkShiftViewModel()
            {
                ShiftName = model.shift
                                                                 ,
                YYYYMM = model.date
                                                                 ,
                DaysShifts = new List<WorkShiftItemViewModel>()
            };
            var date_int = int.Parse(model.date);
            var cities = _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable()
                .AsQueryable()
                .Include(a => a.WorkGroup)
                .Include(a => a.WorkCalendar)
                .Include(a => a.ShiftConceptDetail)
                .Include(a => a.ShiftConceptDetail.ShiftConcept)
                 .Where(x =>
                 x.WorkGroup.Code.Contains(model.shift) &&
                 x.WorkCalendar.YearMonthV1 == date_int)
                 .AsEnumerable()
                 .OrderBy(x => x.WorkCalendar.DayOfMonthShamsi);
            foreach (var item in cities)
            {
                result.DaysShifts.Add(new WorkShiftItemViewModel
                {
                    DayId = item.WorkCalendar.DayOfMonthShamsi,
                    ShiftTime = item.ShiftConceptDetail.ShiftConcept.Title,
                    ColorId = Utility.GetColorIdByShiftConceptCode(item.ShiftConceptDetail.ShiftConcept.Code),
                });
            }
            return result;
            //return _mapper.Map<List<WorkShiftViewModel>>(cities);

        }

        public List<ShiftBoardModel> GetShiftBoard()
        {
            var cities = _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable();
            return _mapper.Map<List<ShiftBoardModel>>(cities);

        }
        public FilterResult<ShiftBoardModel> GetShiftBoardByFilter(FilterRequest Filter)
        {
            //var Data = _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable()
            //    .AsQueryable().Include(a => a.ShiftConceptDetail)
            //    .Include(a => a.WorkGroup)
            //    .Include(a => a.WorkCalendar)
            //    .Where(a => a.IsActive)
            //    .GroupBy(g => new
            //    {
            //        g.WorkGroupId,
            //        g.WorkGroup.Code,

            //    }).Select(a => new ShiftBoardModel()
            //    {
            //        WorkGroupId = a.Key.WorkGroupId,
            //        WorkGroupTitle = a.Key.Code,
            //        YyyyshamsiTitle = a.Max(x => x.WorkCalendar.Yyyyshamsi).ToString(),
            //        YearMonthV1Title = a.Max(x => x.WorkCalendar.YearMonthV1).ToString()
            //    })
            //    .AsQueryable();

            var Data = _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable()
              .AsQueryable().Include(a => a.ShiftBoards)
              .Where(a => a.IsActive)
               .Select(a => new ShiftBoardModel()
               {
                   WorkGroupId = a.Id,
                   WorkGroupTitle = a.Code,
                   YyyyshamsiTitle = a.ShiftBoards.Any() ? a.ShiftBoards.Max(x => x.WorkCalendar.YyyyShamsi).ToString() : "",
                   YearMonthV1Title = a.ShiftBoards.Any() ? a.ShiftBoards.Max(x => x.WorkCalendar.YearMonthV1).ToString() : "",
                   WorkTimeTitle = a.WorkTime.Title,
                   RepetitionPeriod = a.RepetitionPeriod
               })
              .AsQueryable();



            var result = _FilterHandler.GetFilterResult<ShiftBoardModel>(Data, Filter, "WorkGroupTitle");
            return new FilterResult<ShiftBoardModel>()
            {
                Data = _mapper.Map<List<ShiftBoardModel>>(result.Data.ToList()),
                Total = result.Total

            };
            //return new FilterResult<ShiftBoardModel>()
            //{
            //    Data = Data.Skip(Filter.Skip).Take(Filter.Take).ToList(),
            //    Total = Data.Count()

            //};

        }


        public ShiftBoard GetOne(int id)
        {
            return _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }



        public List<SearchShiftBoardModel> GetWorkByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftBoard>(_kscHrUnitOfWork.ShiftBoardRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchShiftBoardModel>>(result.Data.ToList());
        }

        public CreateShiftBoardModel GetDataCreateShiftBorad(CreateWorkShiftInputModel inputModel)
        {
            var group = inputModel.WorkGroupId;
            var date = inputModel.Date;
            var model = new CreateShiftBoardModel()
            {
                CreateShiftBoardListModel = new List<CreateShiftBoardListModel>()

                   ,
                PeriodStartDate = date,
                WorkGroupId = group,
                RepetitionPeriod = inputModel.RepetitionPeriod,
                ShiftConceptDetailList = new List<SearchShiftConceptDetailModel>()
            };

            var miladiDate = date.ToGregorianDateTime();
            var year = PersianCulture.GetPersianYear(miladiDate);
            if (year == null)
            {
                model.HasError = true;
                model.ErrorMessage = "در تبدیل سال خطا رخ داده است";
                return model;
            }
            //
            var workGroup = _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().AsQueryable().Include(a => a.WorkTime).FirstOrDefault(a => a.Id == group);
            if (workGroup == null)
            {
                model.HasError = true;
                model.ErrorMessage = "اطلاعات گروه کاری وجود ندارد";
                return model;
            }
            //
            var shiftConceptDetailList = _kscHrUnitOfWork.ShiftConceptDetailRepository.WhereQueryable(x => x.IsActive)

               .Select(d => new SearchShiftConceptDetailModel()
               {
                   Id = d.Id,
                   Code = d.Code,
                   Title = d.Title
               }).OrderBy(o => o.Code).ToList();
            model.ShiftConceptDetailList.Add(new SearchShiftConceptDetailModel { Title = "انتخاب کنید" });
            model.ShiftConceptDetailList.AddRange(shiftConceptDetailList);
            //

            var shiftBoard = _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable().AsQueryable().Include(x => x.WorkCalendar).Include(x => x.ShiftConceptDetail);
            if (shiftBoard.Any(x => x.WorkGroupId == group && x.WorkCalendar.YyyyShamsi == year))
            {
                model.ShiftBoardIsAlready = true;
                model.HasError = true;
                model.ErrorMessage = "اطلاعات لوحه شیفت وجود دارد،برای مشاهده آن به نمایش لوحه شیفت مراجعه کنید";
                return model;
            }
            else
            {

                // if (workGroup.WorkTime.RepetitionPeriod == 0)
                if (!workGroup.RepetitionPeriod.HasValue || workGroup.RepetitionPeriod == 0)
                {
                    if (inputModel.RepetitionPeriod == 0)
                    {
                        model.HasRepetitionPeriod = false;
                        model.HasError = true;
                        model.ErrorMessage = "دوره تکرار برای زمان کاری مشخص نشده است";
                        return model;
                    }
                    else
                    {
                        workGroup.RepetitionPeriod = inputModel.RepetitionPeriod;
                    }
                }
                if (miladiDate == null)
                {
                    model.HasError = true;
                    model.ErrorMessage = "در گرفتن تاریخ میلادی خطا رخ داده است";
                    return model;
                }
                var tempDate = miladiDate.Value;
                var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQueryable();
                if (!workCalendar.Any(x => x.YyyyShamsi == year))
                {
                    model.HasError = true;
                    model.ErrorMessage = "اطلاعات تقویم برای تاریخ انتخاب شده وجود ندارد";
                    return model;
                }
                var endDate = tempDate.AddDays(-1);
                //   var startDate = tempDate.AddDays(-1 * workGroup.WorkTime.RepetitionPeriod);
                var startDate = tempDate.AddDays(-1 * workGroup.RepetitionPeriod.Value);
                var workCalendarSelected = workCalendar.Where(x => x.MiladiDateV1 >= startDate && x.MiladiDateV1 <= endDate).ToList().OrderBy(x => x.MiladiDateV1);
                //

                //
                List<CreateShiftBoardListModel> list = new List<CreateShiftBoardListModel>();
                // لوحه شیفت سال قبل وجود داشته باشد
                if (shiftBoard.Any(x => x.WorkGroupId == group && x.WorkCalendar.YyyyShamsi == year - 1))
                {
                    //
                    foreach (var dateCalendar in workCalendarSelected)
                    {
                        var shiftInDate = shiftBoard.SingleOrDefault(x => x.WorkGroupId == group && x.WorkCalendarId == dateCalendar.Id);
                        if (shiftInDate == null)
                        {
                            model.HasError = true;
                            model.ErrorMessage = string.Format("اطلاعات لوحه شیفت برای تاریخ {0} وجود ندارد", dateCalendar.ShamsiDateV1);
                            return model;
                        }
                        CreateShiftBoardListModel shiftBoarddata = new CreateShiftBoardListModel();
                        shiftBoarddata.ShiftBoardId = shiftInDate.Id;
                        shiftBoarddata.ShiftDate = shiftInDate.WorkCalendar.ShamsiDateV1;
                        shiftBoarddata.ShiftDay = shiftInDate.WorkCalendar.DayNameShamsi;
                        shiftBoarddata.ShiftConceptDetailId = shiftInDate.ShiftConceptDetailId;
                        list.Add(shiftBoarddata);
                    }
                }
                else // لوحه شیفت سال قبل وجود نداشته باشد
                {
                    //
                    foreach (var dateCalendar in workCalendarSelected)
                    {
                        CreateShiftBoardListModel shiftBoarddata = new CreateShiftBoardListModel();
                        shiftBoarddata.ShiftDate = dateCalendar.ShamsiDateV1;
                        shiftBoarddata.ShiftDay = dateCalendar.DayNameShamsi;
                        list.Add(shiftBoarddata);
                    }
                }
                //
                //if (list.Count != workGroup.WorkTime.RepetitionPeriod)
                if (list.Count != workGroup.RepetitionPeriod)
                {
                    model.HasError = true;
                    model.ErrorMessage = "تعداد ردیف های بدست آمده از لوحه شیفت دوره قبل با دوره تکرار برابر نمی باشد";
                    return model;
                }
                model.CreateShiftBoardListModel = list;
            }
            return model;
        }

        public async Task<KscResult> CreateShiftBoard(CreateShiftBoardModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            //
            // var workGroup = _kscHrUnitOfWork.WorkGroupRepository.GetById(model.WorkGroupId);
            var workGroup = _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().AsQueryable().Include(a => a.WorkTime).FirstOrDefault(a => a.Id == model.WorkGroupId);
            if (workGroup == null)
                result.AddError("خطا", " اطلاعات گروه کاری وجود ندارد");
            //
            var miladiDate = model.PeriodStartDate.ToGregorianDateTime();
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetAllQueryable();
            var workCalendarStartDate = workCalendar.SingleOrDefault(x => x.MiladiDateV1 == miladiDate);
            if (workCalendarStartDate == null)
                result.AddError("خطا", " اطلاعات تقویم کاری برای تاریخ شروع لوحه شیفت وجود ندارد");
            //
            //if (model.CreateShiftBoardListModel.Count() != workGroup.WorkTime.RepetitionPeriod)
            if (workGroup.RepetitionPeriod == null && model.RepetitionPeriod != 0)
            {
                workGroup.RepetitionPeriod = model.RepetitionPeriod;
            }
            if (model.CreateShiftBoardListModel.Count() != workGroup.RepetitionPeriod)
                result.AddError("خطا", " دوره تکرار برای زمان کاری مشخص نشده است");
            //
            if (model.CreateShiftBoardListModel.Count() != model.CreateShiftBoardListModel.Count(x => x.ShiftConceptDetailId != 0))
                result.AddError("خطا", " برای همه ردیف ها باید شیفت را مشخص کنید");
            var year = PersianCulture.GetPersianYear(miladiDate);
            if (result.Errors != null)
            {
                return result;
            }
            var shiftBoardByYear = _kscHrUnitOfWork.ShiftBoardRepository.WhereQueryable(x => x.WorkGroupId == model.WorkGroupId && x.WorkCalendar.YyyyShamsi == year);
            //

            //
            if (shiftBoardByYear.Any())
            {
                result.AddError("خطا", "  لوحه شیفت وجود دارد");
            }
            else
            {
                var workCalendarSelected = workCalendar.Where(x => x.MiladiDateV1 >= miladiDate && x.YyyyShamsi == year).OrderBy(x => x.MiladiDateV1).ToList();
                int index = 0;
                //var repetitionPeriod = workGroup.WorkTime.RepetitionPeriod - 1;
                var repetitionPeriod = workGroup.RepetitionPeriod.Value - 1;
                List<ShiftBoard> shiftBoardList = new List<ShiftBoard>();
                foreach (var calendar in workCalendarSelected)
                {

                    ShiftBoard newShiftBoard = new ShiftBoard();
                    newShiftBoard.WorkGroupId = workGroup.Id;
                    newShiftBoard.WorkCalendarId = calendar.Id;
                    newShiftBoard.ShiftConceptDetailId = model.CreateShiftBoardListModel[index].ShiftConceptDetailId;
                    newShiftBoard.IsActive = true;
                    newShiftBoard.InsertDate = System.DateTime.Now;
                    newShiftBoard.InsertUser = model.CurrentUserName;
                    newShiftBoard.DomainName = model.DomainName;
                    if (calendar.MiladiDateV1 == miladiDate)
                    {
                        newShiftBoard.IsPeriodStartDate = true;
                    }
                    // _kscHrUnitOfWork.ShiftBoardRepository.Add(newShiftBoard);
                    shiftBoardList.Add(newShiftBoard);
                    if (index == repetitionPeriod)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = index + 1;
                    }
                }
                if (shiftBoardList.Count() > 0)
                {
                    _kscHrUnitOfWork.ShiftBoardRepository.AddReangeShiftBoard(shiftBoardList);
                }
            }
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public FilterResult<ShiftBoardListModel> GeShiftBoradGrid(ShiftBoardGridSearchModel Filter)
        {
            var miladiDate = Filter.Date.ToGregorianDateTime();
            var year = PersianCulture.GetPersianYear(miladiDate);
            var Data = _kscHrUnitOfWork.ShiftBoardRepository.GetAllQueryable()
                .AsQueryable().Include(a => a.ShiftConceptDetail)
                .Include(a => a.WorkGroup)
                .Include(a => a.WorkCalendar)
                .Where(a => a.IsActive && a.WorkGroupId == Filter.WorkGroupId && a.WorkCalendar.YyyyShamsi == year)
               .Select(a => new ShiftBoardListModel()
               {
                   Id = a.Id,
                   ShiftDate = a.WorkCalendar.ShamsiDateV1,
                   ShiftDay = a.WorkCalendar.DayNameShamsi,
                   ShiftMonth = a.WorkCalendar.MonthNameShamsiV1,
                   ShiftConceptDetailTitle = a.ShiftConceptDetail.Code + "-" + a.ShiftConceptDetail.Title,

               })
                .AsQueryable();
            //var result = _FilterHandler.Filter<ShiftBoardListModel>(Data, Filter, nameof(ShiftBoard.Id));
            //var count = _FilterHandler.Count<ShiftBoardListModel>(Data, Filter);
            //return new FilterResult<ShiftBoardListModel>()
            //{
            //    Data = result.ToList(),
            //    Total = count

            //};
            var result = _FilterHandler.GetFilterResult<ShiftBoardListModel>(Data, Filter, nameof(ShiftBoard.Id));
            return new FilterResult<ShiftBoardListModel>()
            {
                Data = _mapper.Map<List<ShiftBoardListModel>>(result.Data.ToList()),
                Total = result.Total

            };


        }
        #region //برای لوح شیفت به این توابع نیاز نیست

        //public EditShiftBoardModel GetForEdit(int id)
        //{
        //    var data = GetOne(id);
        //    EditShiftBoardModel model = new EditShiftBoardModel();
        //    model.IsActive = data.IsActive;
        //    model.ShiftConceptDetailId = data.ShiftConceptDetailId;
        //    model.WorkCalendarId = data.WorkCalendarId;
        //    model.WorkGroupId = data.WorkGroupId;
        //    model.IsPeriodStartDate = data.IsPeriodStartDate;
        //  //  model.LastWorkGroupTitle = data.WorkGroup.Where(x => x.IsActive).Select(s => s.Code).ToList();
        //    var invalidCode = _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.ShiftBoardId != data.Id || (x.ShiftBoardId == id && x.IsActive == false)).Select(x => x.Code);
        //    //model.LatinAlphabetListEdit = model.LatinAlphabetList.Except(invalidCode).ToList();
        //    return model;
        //    //return _mapper.Map<EditShiftBoardModel>(model);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public KscResult AddShiftBoard(AddShiftBoardModel model)
        //{
        //    var result = model.IsValid();
        //    if (!result.Success)
        //        return result;

        //    //if (Exists(model.Title) == true)
        //    //{

        //    //    result.AddError("رکورد نامعتبر", "عنوان وارد شده موجود می باشد");
        //    //    return result;
        //    //}
        //    //if (ExistsCode(model.Code) == true)
        //    //{

        //    //    result.AddError("رکورد نامعتبر", "کد وارد شده موجود می باشد");
        //    //    return result;
        //    //}
        //    //if (model.RepetitionPeriod == 0)
        //    //{

        //    //    result.AddError("رکورد نامعتبر", "دوره تکرار باید مقدار داشته باشد");
        //    //    return result;
        //    //}
        //    if (model.WorkGroupTitle == null)
        //    {

        //        result.AddError("رکورد نامعتبر", "گروه کاری باید مقدار داشته باشد");
        //        return result;
        //    }
        //    if (_kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().Any(x => model.WorkGroupTitle.Any(y => y == x.Code)))
        //    {
        //        result.AddError("رکورد نامعتبر", "کد گروه کاری نباید تکراری باشد");
        //        return result;
        //    }
        //    var ShiftBoard = _mapper.Map<ShiftBoard>(model);
        //    ShiftBoard.InsertDate = DateTime.Now;
        //    ShiftBoard.InsertUser = model.InsertUser;
        //    ShiftBoard.IsActive = true;
        //    foreach (var item in model.WorkGroupTitle)
        //    {
        //        WorkGroup workGroup = new WorkGroup();
        //        workGroup.Code = item.ToString();
        //        workGroup.InsertDate = DateTime.Now;
        //        workGroup.InsertUser = model.InsertUser;
        //        workGroup.IsActive = true;
        //        // _kscHrUnitOfWork.WorkGroupRepository.Add(workGroup);
        //        ShiftBoard.WorkGroup.Add(workGroup);
        //    }
        //    _kscHrUnitOfWork.ShiftBoardRepository.Add(ShiftBoard);
        //    if (_kscHrUnitOfWork.SaveChanges() > 0)
        //        return result;
        //    result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
        //    return result;
        //}

        //public KscResult UpdateShiftBoard(EditShiftBoardModel model)
        //{

        //    var result = model.IsValid();
        //    if (!result.Success)
        //        return result;
        //    var oneShiftBoard = GetOne(model.Id);
        //    if (oneShiftBoard == null)
        //    {
        //        result.AddError("رکورد حذف شده", "رکورد حذف شده است");
        //        return result;
        //    }
        //    //
        //    if (model.WorkGroupTitle == null)
        //    {

        //        result.AddError("رکورد نامعتبر", "گروه کاری باید مقدار داشته باشد");
        //        return result;
        //    }

        //    if (_kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().Any(x => x.ShiftBoardId != model.Id && model.WorkGroupTitle.Any(y => y == x.Code)))
        //    {
        //        result.AddError("رکورد نامعتبر", "کد گروه کاری نباید تکراری باشد");
        //        return result;
        //    }
        //    //

        //    //

        //    oneShiftBoard.ShiftConceptDetailId = model.ShiftConceptDetailId;
        //    oneShiftBoard.WorkCalendarId = model.WorkCalendarId;
        //    oneShiftBoard.WorkGroupId = model.WorkGroupId;
        //    oneShiftBoard.UpdateDate = DateTime.Now;
        //    oneShiftBoard.UpdateUser = model.UpdateUser;

        //    //var workGroupByShiftBoardId = _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.ShiftBoardId == model.Id);
        //    //var deletedWorkGroup = workGroupByShiftBoardId.Where(x => !model.WorkGroupTitle.Contains(x.Code));
        //    //foreach (var item in deletedWorkGroup)
        //    //{
        //    //    item.IsActive = false;
        //    //    _kscHrUnitOfWork.WorkGroupRepository.Update(item);
        //    //}
        //    //foreach (var item in model.WorkGroupTitle)
        //    //{
        //    //    if (!workGroupByShiftBoardId.Any(x => x.Code == item))
        //    //    {
        //    //        WorkGroup workGroup = new WorkGroup();
        //    //        workGroup.Code = item;
        //    //        workGroup.InsertDate = DateTime.Now;
        //    //        workGroup.InsertUser = model.InsertUser;
        //    //        workGroup.IsActive = true;
        //    //        workGroup.ShiftBoardId = oneShiftBoard.Id;
        //    //        _kscHrUnitOfWork.WorkGroupRepository.Add(workGroup);
        //    //    }
        //    //}
        //    if (_kscHrUnitOfWork.SaveChanges() > 0)
        //        return result;
        //    result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
        //    return result;

        //}
        //public KscResult RemoveShiftBoard(EditShiftBoardModel model)
        //{

        //    var result = new KscResult();
        //    //if (!result.Success)
        //    //    return result;
        //    var item = GetOne(model.Id);
        //    if (item == null)
        //    {
        //        result.AddError("رکورد حذف شده", "رکورد حذف شده است");
        //        return result;
        //    }

        //    item.IsActive = false;
        //    item.UpdateDate = DateTime.Now;
        //    item.UpdateUser = model.UpdateUser;


        //    //_service.Remove(item);
        //    if (_kscHrUnitOfWork.SaveChanges() > 0) return result;
        //    result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
        //    return result;
        //}
        #endregion
        #region اطلاعات لوحه شیفت در یک بازه تاریخ
        public List<ShiftBoardByRangeDateModel> GetShiftBoardByRangeDate(ShiftBoardByRangeDateInputModel model)
        {
            List<ShiftBoardByRangeDateModel> result = new List<ShiftBoardByRangeDateModel>();
            if (model.DateFrom > model.DateTo)
                return result;
            //
            model.DateFrom = model.DateFrom.Date;
            model.DateTo = model.DateTo.Date;
            var data = _kscHrUnitOfWork.ViewShiftBoardRepository.GetShiftBoard().Where(x => x.MiladiDate >= model.DateFrom && x.MiladiDate <= model.DateTo);
            return _mapper.Map<List<ShiftBoardByRangeDateModel>>(data);
        }
        #endregion
    }
}
