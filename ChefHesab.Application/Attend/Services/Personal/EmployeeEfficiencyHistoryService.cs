using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Personal.EmployeeEfficiencyHistory;
using Ksc.HR.DTO.Personal.EmployeeTimeSheet;
using Ksc.HR.DTO.Report;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.Resources.Messages;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksc.HR.Share.Extention;
using System;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using DNTPersianUtils.Core;
using DNTPersianUtils;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using KSC.MIS.Service;
using System.Globalization;
using System.Text;
using Ksc.HR.Share.General;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.DTO.Stepper;
using Ksc.HR.Application.Interfaces;
using Ksc.HR.DTO.MIS;
using NetTopologySuite.Index.HPRtree;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeEfficiencyHistoryService : IEmployeeEfficiencyHistoryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        // private readonly IEmployeeWorkGroupRepository _employeeWorkGroupRepository;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IMisUpdateService _misUpdate;
        private readonly IStepper_ProcedureService _procedureService;

        public EmployeeEfficiencyHistoryService(IKscHrUnitOfWork kscHrUnitOfWork,
            IMapper mapper, IFilterHandler FilterHandler, IMisUpdateService misUpdate, IStepper_ProcedureService procedureService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _misUpdate = misUpdate;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _procedureService = procedureService;
        }


        //public FilterResult<EmployeeEfficiencyGridManageModel> GetEmployeeForEfficiency_ooo(ReportSearchModel Filter)
        //{
        //    var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewEmployeeTeamUserActiveRepository.GetAllAsNoTracking();

        //    if (Filter.FromTeam > 0)
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.FromTeam.ToString()) >= 0);
        //    if (Filter.ToTeam > 0)
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.ToTeam.ToString()) <= 0);

        //    if (Filter.EmployeeIds.Count() > 0)
        //    {
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => Filter.EmployeeIds.Contains(x.Id));
        //    }



        //    if (!Filter.IsSalaryUser)
        //    {
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == Filter.CurrentUserName);
        //    }

        //    Filter.YearMonth = int.Parse(Filter.YearMonthString.Fa2En());
        //    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(Filter.YearMonth);
        //    var PrevMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(Filter.YearMonth);
        //    var YearMonthDayShamsi_Prev = int.Parse(PrevMonth.Substring(0, 7).Replace("/", ""));


        //    var employeeIds = query_ViewMisEmployeeSecurity.Select(a => a.Id).ToList();

        //    var activeEmployeeTeamWorksINEmpIds = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActivePersonsIdByEmployeeIdsWithDate(employeeIds, workCalendar.Item1).ToList();

        //    var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activeEmployeeTeamWorksINEmpIds)
        //        .AsNoTracking();
        //    if (!string.IsNullOrEmpty(Filter.PersonalNumber))
        //    {
        //        query = query.Where(x => x.EmployeeNumber.Contains(Filter.PersonalNumber));
        //    }

        //    if (!string.IsNullOrEmpty(Filter.Name))
        //    {
        //        query = query.Where(x => x.Name.Contains(Filter.Name));
        //    }

        //    if (!string.IsNullOrEmpty(Filter.Family))
        //    {
        //        query = query.Where(x => x.Family.Contains(Filter.Family));
        //    }

        //    //filtered EmployeeEfficiencyHistory
        //    var empNumberInQuery = query.Select(x => x.EmployeeNumber).ToList();
        //    var query_view = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumbers(empNumberInQuery).ToList();
        //    if (!Filter.IsSalaryUser)
        //    {//اگر کاربر جاری، فرد عادی باشد رکورد اطلاعاتی خودش بایستی از لیست حذف شود
        //        var currentPerson = query_view.Where(x => x.WinUser != null).FirstOrDefault(x => x.WinUser.ToLower() == Filter.CurrentUserName);
        //        if (currentPerson != null)
        //        {
        //            query = query.Where(x => x.EmployeeNumber != currentPerson.EmployeeNumber);
        //        }
        //    }

        //    var timeSheetSetting = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
        //    var minRange = timeSheetSetting.MinEfficiency;
        //    var maxRange = timeSheetSetting.MaxEfficiency;



        //    var dataForYearMonth = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Any(x => x.YearMonth == Filter.YearMonth);
        //    var EmployeeTimeSetting = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
        //    var isActiveMonth = (EmployeeTimeSetting.AttendAbsenceItemDate == Filter.YearMonth) && dataForYearMonth;

        //    decimal defultEff = Convert.ToDecimal("0");

        //    var bindtomodel = query.ToList().Select(x => new EmployeeEfficiencyGridManageModel()
        //    {
        //        FullName = x.Name + " " + x.Family,
        //        EmployeeId = x.Id,
        //        EmployeeNumber = x.EmployeeNumber,
        //        TeamTitle = x.TeamWork.Code + "-" + x.TeamWork.Title,
        //        TeamCode = x.TeamWork.Code,
        //        IsActiveMonth = isActiveMonth,
        //        YearMonth = Filter.YearMonth.ToString(),
        //        EmploymentTypeId = x.EmploymentTypeId, // نوع استخدام
        //        CurrentUser = Filter.CurrentUserName,
        //        EfficiencyNew = defultEff,//0,
        //        EfficiencyOld = defultEff,//0,
        //        MinRange = minRange,
        //        MaxRange = maxRange,


        //    }).ToList();
        //    var employeeIdsFindedList = bindtomodel.Select(x => x.EmployeeId).ToList();

        //    var EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository
        //                  // .GetKendoGrid(employeeIdsFindedList, YearMonthDayShamsi_Prev, Filter.YearMonth)
        //                  //.GetAll()
        //                  //.AsQueryable()
        //                  .Where(a => employeeIdsFindedList.Contains(a.EmployeeId) &&
        //                              (a.YearMonth == YearMonthDayShamsi_Prev || a.YearMonth == Filter.YearMonth))
        //        .OrderByDescending(a => a.Id).ToList();

        //    foreach (var item in bindtomodel)
        //    {
        //        var findEfficiencyOld = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == YearMonthDayShamsi_Prev);
        //        if (findEfficiencyOld != null)
        //        {
        //            item.EfficiencyOld = findEfficiencyOld.Efficiency;
        //        }
        //        var findEfficiencyNew = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth);
        //        if (findEfficiencyNew != null)
        //        {
        //            item.EfficiencyNew = findEfficiencyNew.Efficiency;
        //        }
        //        var findView = query_view.FirstOrDefault(x => x.EmployeeNumber == item.EmployeeNumber);
        //        if (findView != null)
        //        {
        //            item.PostDesc = findView.JobPositionTitle; // عنوان پست
        //            item.GroupCode = Convert.ToInt32(findView.JobLevelCode); // گروه شغلی
        //        }
        //        item.IsEffective = 1;
        //        if (item.EmploymentTypeId == 6 && (item.GroupCode == 17 || item.GroupCode == 18 || item.GroupCode == 19 || item.GroupCode == 20))
        //            item.IsEffective = 0;

        //        item.IsChenged = EmployeeEfficiencyHistory.Any(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth && x.InsertUser != "System");
        //    }


        //    var resultFinalList = _FilterHandler.GetFilterResult<EmployeeEfficiencyGridManageModel>(bindtomodel, Filter, "TeamCode_Sort");

        //    return new FilterResult<EmployeeEfficiencyGridManageModel>()
        //    {
        //        Data = resultFinalList.Data.ToList(),
        //        Total = resultFinalList.Total

        //    };

        //}
        public FilterResult<EmployeeEfficiencyGridManageModel> GetEmployeeForEfficiency(ReportSearchModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewEmployeeTeamUserActiveRepository.GetAllAsNoTracking();
            #region جستجوی صفحه
            if (Filter.FromTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.FromTeam.ToString()) >= 0);
            if (Filter.ToTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.ToTeam.ToString()) <= 0);

            if (Filter.EmployeeIds.Count() > 0)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => Filter.EmployeeIds.Contains(x.Id));
            }
            #endregion
            #region شخصی سازی دیتا بر اساس فرد لاگین کرده
            if (!Filter.IsSalaryUser)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == Filter.CurrentUserName);
            }
            #endregion

            Filter.YearMonth = int.Parse(Filter.YearMonthString.Fa2En());
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(Filter.YearMonth);
            var PrevMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(Filter.YearMonth);
            var YearMonthDayShamsi_Prev = int.Parse(PrevMonth.Substring(0, 7).Replace("/", ""));


            var employeeIds = query_ViewMisEmployeeSecurity.Select(a => a.Id).ToList();

            var activeEmployeeTeamWorksINEmpIds = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActivePersonsIdByEmployeeIdsWithDate(employeeIds, workCalendar.Item1).ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activeEmployeeTeamWorksINEmpIds)
                .AsNoTracking();
            #region جستجوی صفحه
            if (!string.IsNullOrEmpty(Filter.PersonalNumber))
            {
                query = query.Where(x => x.EmployeeNumber.Contains(Filter.PersonalNumber));
            }

            if (!string.IsNullOrEmpty(Filter.Name))
            {
                query = query.Where(x => x.Name.Contains(Filter.Name));
            }

            if (!string.IsNullOrEmpty(Filter.Family))
            {
                query = query.Where(x => x.Family.Contains(Filter.Family));
            }
            #endregion
            //filtered EmployeeEfficiencyHistory
            var empNumberInQuery = query.Select(x => x.Id);


            ////var query_view = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumbers(empNumberInQuery).ToList();
            //var query_viewQuery = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetAllQueryable().AsNoTracking()
            //    .Join(empNumberInQuery, x => x.EmployeeId, y => y, (x, y) => new { ViewMisEmployee = x });
            //// var query_viewQuery = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumbers(empNumberInQuery);

            var query_viewQuery = _kscHrUnitOfWork.View_EmployeeRepository.GetAllQueryable().AsNoTracking()
                .Join(empNumberInQuery, x => x.EmployeeId, y => y, (x, y) => new { ViewMisEmployee = x });


            var query_view = query_viewQuery;


            if (!Filter.IsSalaryUser)
            {//اگر کاربر جاری، فرد عادی باشد رکورد اطلاعاتی خودش بایستی از لیست حذف شود
                var currentPerson = query_view.Where(x => x.ViewMisEmployee.WindowsUser != null).FirstOrDefault(x => x.ViewMisEmployee.WindowsUser.ToLower() == Filter.CurrentUserName);
                if (currentPerson != null)
                {
                    query = query.Where(x => x.EmployeeNumber != currentPerson.ViewMisEmployee.EmployeeNumber);
                }
            }

            var timeSheetSetting = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
            var minRange = timeSheetSetting.MinEfficiency;
            var maxRange = timeSheetSetting.MaxEfficiency;



            var dataForYearMonth = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Any(x => x.YearMonth == Filter.YearMonth);
            var EmployeeTimeSetting = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var isActiveMonth = (EmployeeTimeSetting.AttendAbsenceItemDate == Filter.YearMonth) && dataForYearMonth;

            decimal defultEff = Convert.ToDecimal("0");
            var qurylist = query.ToList();

            
            var model = from a in qurylist
                       join b in query_view on a.EmployeeNumber equals b.ViewMisEmployee.EmployeeNumber
                       select new EmployeeEfficiencyGridManageModel 
                       {
                           FullName = a.Name + " " + a.Family,
                           EmployeeId = a.Id,
                           EmployeeNumber = a.EmployeeNumber,
                           TeamTitle = a.TeamWork.Code + "-" + a.TeamWork.Title,
                           TeamCode = a.TeamWork.Code,
                           IsActiveMonth = isActiveMonth,
                           YearMonth = Filter.YearMonth.ToString(),
                           EmploymentTypeId = a.EmploymentTypeId, // نوع استخدام
                           CurrentUser = Filter.CurrentUserName,
                           EfficiencyNew = defultEff,//0,
                           EfficiencyOld = defultEff,//0,
                           MinRange = minRange,
                           MaxRange = maxRange,
                           PostDesc = b.ViewMisEmployee.JobPositionTitle,// عنوان پست
                           //GroupCode = b.ViewMisEmployee.JobLevelCode, // گروه شغلی
                       };
            var bindtomodel = model.ToList();

            var employeeIdsFindedList = bindtomodel.Select(x => x.EmployeeId).ToList();

            var queryForGroupCode = _kscHrUnitOfWork.EmployeeInterdictRepository.GetlatestByEmployeeIds(employeeIdsFindedList, true)
                .Select(x=> new { x.EmployeeId , x.CurrentJobGroupId });

            bindtomodel = (from a in bindtomodel
                           join b in queryForGroupCode on a.EmployeeId equals b.EmployeeId into g 
                           from b in g.DefaultIfEmpty()
                           select new EmployeeEfficiencyGridManageModel
                          {
                              FullName = a.FullName,
                              EmployeeId = a.EmployeeId,
                              EmployeeNumber = a.EmployeeNumber,
                              TeamTitle = a.TeamTitle,
                              TeamCode = a.TeamCode,
                              IsActiveMonth = a.IsActiveMonth,
                              YearMonth = a.YearMonth,
                              EmploymentTypeId = a.EmploymentTypeId, // نوع استخدام
                              CurrentUser = a.CurrentUser,
                              EfficiencyNew = a.EfficiencyNew,
                              EfficiencyOld = a.EfficiencyOld,
                              MinRange = a.MinRange,
                              MaxRange = a.MaxRange,
                              PostDesc = a.PostDesc,// عنوان پست
                              GroupCode = b?.CurrentJobGroupId, // گروه شغلی
                          }).ToList();

            var EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetAllQueryable()
                          .Where(a => employeeIdsFindedList.Any(y => y == a.EmployeeId) &&
                                      (a.YearMonth == YearMonthDayShamsi_Prev || a.YearMonth == Filter.YearMonth))
                          .Select(x => new { Id = x.Id, EmployeeId = x.EmployeeId, YearMonth = x.YearMonth, Efficiency = x.Efficiency, InsertUser = x.InsertUser })
                .OrderByDescending(a => a.Id).ToList();

            foreach (var item in bindtomodel)
            {
                var findEfficiencyOld = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == YearMonthDayShamsi_Prev);
                if (findEfficiencyOld != null)
                {
                    item.EfficiencyOld = findEfficiencyOld.Efficiency;
                }
                var findEfficiencyNew = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth);
                if (findEfficiencyNew != null)
                {
                    item.EfficiencyNew = findEfficiencyNew.Efficiency;
                }
                //var findView = query_view.FirstOrDefault(x => x.ViewMisEmployee.EmployeeNumber == item.EmployeeNumber);
                //if (findView != null)
                //{
                //    item.PostDesc = findView.ViewMisEmployee.JobPositionTitle; // عنوان پست
                //    item.GroupCode = findView.ViewMisEmployee.JobLevelCode; // گروه شغلی
                //}
                item.IsEffective = 1;
                if (item.EmploymentTypeId == 6 && (item.GroupCode == 17 || item.GroupCode == 18 || item.GroupCode == 19 || item.GroupCode == 20))
                    item.IsEffective = 0;

                item.IsChenged = EmployeeEfficiencyHistory.Any(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth && x.InsertUser != "System");
            }


            var resultFinalList = _FilterHandler.GetFilterResult<EmployeeEfficiencyGridManageModel>(bindtomodel, Filter, "TeamCode_Sort");

            return new FilterResult<EmployeeEfficiencyGridManageModel>()
            {
                Data = resultFinalList.Data.ToList(),
                Total = resultFinalList.Total

            };

        }

        //public FilterResult<EmployeeEfficiencyGridManageModel> GetEmployeeForEfficiencyReportMonth_ooo(ReportSearchModel Filter)
        //{
        //    var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewEmployeeTeamUserActiveRepository.GetAllAsNoTracking();

        //    if (Filter.FromTeam > 0)
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.FromTeam.ToString()) >= 0);
        //    if (Filter.ToTeam > 0)
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.ToTeam.ToString()) <= 0);

        //    if (Filter.EmployeeIds.Count() > 0)
        //    {
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => Filter.EmployeeIds.Contains(x.Id));
        //    }

        //    if (!Filter.IsSalaryUser)
        //    {
        //        query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == Filter.CurrentUserName);
        //    }
        //    var employeeIds = query_ViewMisEmployeeSecurity.Select(a => a.Id).ToList();
        //    Filter.YearMonth = Convert.ToInt32(Filter.YearMonthString);
        //    var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(Filter.YearMonth);

        //    var activeEmployeeTeamWorksINEmpIds = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActivePersonsIdByEmployeeIdsWithDate(employeeIds, workCalendar.Item1).ToList();

        //    var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activeEmployeeTeamWorksINEmpIds)
        //        .AsNoTracking();


        //    //filtered EmployeeEfficiencyHistory
        //    var empNumberInQuery = query.Select(x => x.EmployeeNumber).ToList();
        //    var query_view = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumbers(empNumberInQuery).ToList();
        //    if (!Filter.IsSalaryUser)
        //    {//اگر کاربر جاری، فرد عادی باشد رکورد اطلاعاتی خودش بایستی از لیست حذف شود
        //        var currentPerson = query_view.Where(x => x.WinUser != null).FirstOrDefault(x => x.WinUser.ToLower() == Filter.CurrentUserName);
        //        if (currentPerson != null)
        //        {
        //            query = query.Where(x => x.EmployeeNumber != currentPerson.EmployeeNumber);
        //        }
        //    }

        //    var timeSheetSetting = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
        //    var minRange = timeSheetSetting.MinEfficiency;
        //    var maxRange = timeSheetSetting.MaxEfficiency;


        //    Filter.YearMonth = int.Parse(Filter.YearMonthString.Fa2En());
        //    var PrevMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(Filter.YearMonth);
        //    var YearMonthDayShamsi_Prev = int.Parse(PrevMonth.Substring(0, 7).Replace("/", ""));

        //    var dataForYearMonth = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Any(x => x.YearMonth == Filter.YearMonth);
        //    var EmployeeTimeSetting = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
        //    var isActiveMonth = (EmployeeTimeSetting.AttendAbsenceItemDate == Filter.YearMonth) && dataForYearMonth;

        //    decimal defultEff = Convert.ToDecimal("0");

        //    var bindtomodel = query.ToList().Select(x => new EmployeeEfficiencyGridManageModel()
        //    {
        //        FullName = x.Name + " " + x.Family,
        //        EmployeeId = x.Id,
        //        EmployeeNumber = x.EmployeeNumber,
        //        TeamTitle = x.TeamWork.Code + "-" + x.TeamWork.Title,
        //        TeamCode = x.TeamWork.Code,
        //        IsActiveMonth = isActiveMonth,
        //        YearMonth = Filter.YearMonth.ToString(),
        //        EmploymentTypeId = x.EmploymentTypeId, // نوع استخدام
        //        CurrentUser = Filter.CurrentUserName,
        //        EfficiencyNew = defultEff,//0,
        //        EfficiencyOld = defultEff,//0,
        //        MinRange = minRange,
        //        MaxRange = maxRange,


        //    }).ToList();
        //    var employeeIdsFindedList = bindtomodel.Select(x => x.EmployeeId).ToList();

        //    var EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository
        //                   //.GetKendoGrid(employeeIdsFindedList, YearMonthDayShamsi_Prev, Filter.YearMonth)
        //                   // .GetAll().AsQueryable()
        //                   .Where(a => employeeIdsFindedList.Contains(a.EmployeeId) &&
        //                              (a.YearMonth == YearMonthDayShamsi_Prev || a.YearMonth == Filter.YearMonth))
        //         .OrderByDescending(a => a.Id).ToList();
        //    bindtomodel = bindtomodel.Where(item => EmployeeEfficiencyHistory.Any(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth && x.InsertUser != "System")).ToList();
        //    foreach (var item in bindtomodel)
        //    {
        //        var findEfficiencyOld = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == YearMonthDayShamsi_Prev);
        //        if (findEfficiencyOld != null)
        //        {
        //            item.EfficiencyOld = findEfficiencyOld.Efficiency;
        //        }
        //        var findEfficiencyNew = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth);
        //        if (findEfficiencyNew != null)
        //        {
        //            item.EfficiencyNew = findEfficiencyNew.Efficiency;
        //        }
        //        var findView = query_view.FirstOrDefault(x => x.EmployeeNumber == item.EmployeeNumber);
        //        if (findView != null)
        //        {
        //            item.PostDesc = findView.JobPositionTitle; // عنوان پست
        //            item.GroupCode = Convert.ToInt32(findView.JobLevelCode); // گروه شغلی
        //        }
        //        item.IsEffective = 1;
        //        if (item.EmploymentTypeId == 6 && (item.GroupCode == 17 || item.GroupCode == 18 || item.GroupCode == 19 || item.GroupCode == 20))
        //            item.IsEffective = 0;

        //        //item.IsChenged = EmployeeEfficiencyHistory.Any(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth && x.InsertUser != "System");
        //    }


        //    var resultFinalList = _FilterHandler.GetFilterResult<EmployeeEfficiencyGridManageModel>(bindtomodel, Filter, "TeamCode_Sort");

        //    return new FilterResult<EmployeeEfficiencyGridManageModel>()
        //    {
        //        Data = resultFinalList.Data.ToList(),
        //        Total = resultFinalList.Total

        //    };

        //}

        public FilterResult<EmployeeEfficiencyGridManageModel> GetEmployeeForEfficiencyReportMonth(ReportSearchModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewEmployeeTeamUserActiveRepository.GetAllAsNoTracking();

            if (Filter.FromTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.FromTeam.ToString()) >= 0);
            if (Filter.ToTeam > 0)
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => x.Code.CompareTo(Filter.ToTeam.ToString()) <= 0);

            if (Filter.EmployeeIds.Count() > 0)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => Filter.EmployeeIds.Contains(x.Id));
            }

            if (!Filter.IsSalaryUser)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == Filter.CurrentUserName);
            }
            var employeeIds = query_ViewMisEmployeeSecurity.Select(a => a.Id).ToList();
            Filter.YearMonth = Convert.ToInt32(Filter.YearMonthString);
            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.GetStartDateAndEndDateWithYearMonth(Filter.YearMonth);

            var activeEmployeeTeamWorksINEmpIds = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActivePersonsIdByEmployeeIdsWithDate(employeeIds, workCalendar.Item1).ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activeEmployeeTeamWorksINEmpIds)
                .AsNoTracking();


            //filtered EmployeeEfficiencyHistory
            var empNumberInQuery = query.Select(x => x.Id);

            var query_viewQuery = _kscHrUnitOfWork.View_EmployeeRepository.GetAllQueryable().AsNoTracking()
                .Join(empNumberInQuery, x => x.EmployeeId, y => y, (x, y) => new { ViewMisEmployee = x });


            var query_view = query_viewQuery;

            if (!Filter.IsSalaryUser)
            {//اگر کاربر جاری، فرد عادی باشد رکورد اطلاعاتی خودش بایستی از لیست حذف شود
                var currentPerson = query_view.Where(x => x.ViewMisEmployee.WindowsUser != null).FirstOrDefault(x => x.ViewMisEmployee.WindowsUser.ToLower() == Filter.CurrentUserName);
                if (currentPerson != null)
                {
                    query = query.Where(x => x.EmployeeNumber != currentPerson.ViewMisEmployee.WindowsUser);
                }
            }

            var timeSheetSetting = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
            var minRange = timeSheetSetting.MinEfficiency;
            var maxRange = timeSheetSetting.MaxEfficiency;


            Filter.YearMonth = int.Parse(Filter.YearMonthString.Fa2En());
            var PrevMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetPrevMonth(Filter.YearMonth);
            var YearMonthDayShamsi_Prev = int.Parse(PrevMonth.Substring(0, 7).Replace("/", ""));

            var dataForYearMonth = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Any(x => x.YearMonth == Filter.YearMonth);
            var EmployeeTimeSetting = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var isActiveMonth = (EmployeeTimeSetting.AttendAbsenceItemDate == Filter.YearMonth) && dataForYearMonth;

            decimal defultEff = Convert.ToDecimal("0");
            var qurylist = query.ToList();


            var model = from a in qurylist
                        join b in query_view on a.EmployeeNumber equals b.ViewMisEmployee.EmployeeNumber
                        select new EmployeeEfficiencyGridManageModel
                        {
                            FullName = a.Name + " " + a.Family,
                            EmployeeId = a.Id,
                            EmployeeNumber = a.EmployeeNumber,
                            TeamTitle = a.TeamWork.Code + "-" + a.TeamWork.Title,
                            TeamCode = a.TeamWork.Code,
                            IsActiveMonth = isActiveMonth,
                            YearMonth = Filter.YearMonth.ToString(),
                            EmploymentTypeId = a.EmploymentTypeId, // نوع استخدام
                            CurrentUser = Filter.CurrentUserName,
                            EfficiencyNew = defultEff,//0,
                            EfficiencyOld = defultEff,//0,
                            MinRange = minRange,
                            MaxRange = maxRange,
                            PostDesc = b.ViewMisEmployee.JobPositionTitle,// عنوان پست
                            //GroupCode = b.ViewMisEmployee.JobLevelCode, // گروه شغلی
                        };
            var bindtomodel = model.ToList();

            var employeeIdsFindedList = bindtomodel.Select(x => x.EmployeeId).ToList();

            var queryForGroupCode = _kscHrUnitOfWork.EmployeeInterdictRepository.GetlatestByEmployeeIds(employeeIdsFindedList, true)
                .Select(x => new { x.EmployeeId, x.CurrentJobGroupId });

            bindtomodel = (from a in bindtomodel
                           join b in queryForGroupCode on a.EmployeeId equals b.EmployeeId into g
                           from b in g.DefaultIfEmpty()
                           select new EmployeeEfficiencyGridManageModel
                           {
                               FullName = a.FullName,
                               EmployeeId = a.EmployeeId,
                               EmployeeNumber = a.EmployeeNumber,
                               TeamTitle = a.TeamTitle,
                               TeamCode = a.TeamCode,
                               IsActiveMonth = a.IsActiveMonth,
                               YearMonth = a.YearMonth,
                               EmploymentTypeId = a.EmploymentTypeId, // نوع استخدام
                               CurrentUser = a.CurrentUser,
                               EfficiencyNew = a.EfficiencyNew,
                               EfficiencyOld = a.EfficiencyOld,
                               MinRange = a.MinRange,
                               MaxRange = a.MaxRange,
                               PostDesc = a.PostDesc,// عنوان پست
                               GroupCode = b?.CurrentJobGroupId, // گروه شغلی
                           }).ToList();

            var EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository
                           //.GetKendoGrid(employeeIdsFindedList, YearMonthDayShamsi_Prev, Filter.YearMonth)
                           // .GetAll().AsQueryable()
                           .Where(a => employeeIdsFindedList.Contains(a.EmployeeId) &&
                                      (a.YearMonth == YearMonthDayShamsi_Prev || a.YearMonth == Filter.YearMonth))
                 .OrderByDescending(a => a.Id).ToList();

            bindtomodel = bindtomodel.Where(item => EmployeeEfficiencyHistory.Any(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth && x.InsertUser != "System")).ToList();
            
            foreach (var item in bindtomodel)
            {
                var findEfficiencyOld = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == YearMonthDayShamsi_Prev);
                if (findEfficiencyOld != null)
                {
                    item.EfficiencyOld = findEfficiencyOld.Efficiency;
                }
                var findEfficiencyNew = EmployeeEfficiencyHistory.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth);
                if (findEfficiencyNew != null)
                {
                    item.EfficiencyNew = findEfficiencyNew.Efficiency;
                }
                //var findView = query_view.FirstOrDefault(x => x.EmployeeNumber == item.EmployeeNumber);
                //if (findView != null)
                //{
                //    item.PostDesc = findView.JobPositionTitle; // عنوان پست
                //    item.GroupCode = Convert.ToInt32(findView.JobLevelCode); // گروه شغلی
                //}
                item.IsEffective = 1;
                if (item.EmploymentTypeId == 6 && (item.GroupCode == 17 || item.GroupCode == 18 || item.GroupCode == 19 || item.GroupCode == 20))
                    item.IsEffective = 0;

                //item.IsChenged = EmployeeEfficiencyHistory.Any(x => x.EmployeeId == item.EmployeeId && x.YearMonth == Filter.YearMonth && x.InsertUser != "System");
            }


            var resultFinalList = _FilterHandler.GetFilterResult<EmployeeEfficiencyGridManageModel>(bindtomodel, Filter, "TeamCode_Sort");

            return new FilterResult<EmployeeEfficiencyGridManageModel>()
            {
                Data = resultFinalList.Data.ToList(),
                Total = resultFinalList.Total

            };

        }



        public KscResult SaveEmployeeEfficiency(EmployeeEfficiencyGridManageModel models, string currentUser, bool IsSalaryUser)
        {
            var result = new KscResult();

            var YearMonth = int.Parse(models.YearMonth);

            var stepperValidition = _kscHrUnitOfWork.EmployeeEfficiencyMonthRepository.GetByYearMonth(YearMonth)
                .Any(x => x.SystemSequenceStatusId == EnumSystemSequenceStatusEfficiencyMonth.InActiveEfficiency.Id);
            if (stepperValidition == true)
            {
                result.AddError("خطا", "ضریب کارایی ماهیانه بسته است");
                return result;
            }

            var workCalendar = _kscHrUnitOfWork.WorkCalendarRepository.FirstOrDefault(c => c.YearMonthV1 == YearMonth);
            var systemStatusIds = new List<int?>() {
                EnumSystemSequenceStatusDailyTimeSheet.ActiveForOfficialUser.Id,
                EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id,
                EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id,
                EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id
            };
            //var systemStatusOld = _kscHrUnitOfWork.WorkCalendarRepository.GetDailyTimeSheetStatus(workCalendar.Id).GetAwaiter().GetResult();
            var systemStatus = _kscHrUnitOfWork.WorkCalendarRepository.GetMonthTimeSheetStatus(YearMonth, systemStatusIds).GetAwaiter().GetResult();
            var systemControlDate = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            if (systemControlDate.AttendAbsenceItemDate != YearMonth)
            {
                result.AddError("خطا", "امکان ذخیره فقط برای ماه تایید کارکرد وجود دارد.");
                return result;
            }

            var dataForYearMonth = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Any(x => x.YearMonth == YearMonth);
            if (dataForYearMonth == false)
            {
                result.AddError("خطا", "ضریب کارایی برای ماه تایید کارکرد ایجاد نشده است");
                return result;
            }



            if (IsSalaryUser == false)
            {
                //if (systemStatusOld == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id
                //  || systemStatusOld == EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id)
                if (systemStatus == true) // در همان سال ماه تمام روزهای ماه برای تمام کاربران بسته شده است
                {
                    result.AddError("خطا", "تمام روزهای ماه برای تمام کاربران بسته شده است");
                    return result;
                }
            }
            var validRang = _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActive();
            if (validRang.MinEfficiency > models.EfficiencyNew || validRang.MaxEfficiency < models.EfficiencyNew)
            {
                result.AddError("خطا", $"ضریب کارایی بایستی بین {validRang.MinEfficiency} تا {validRang.MaxEfficiency}  باشد.");
                return result;
            }

            var employeeEfficiencies = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetByEmployeeId(models.EmployeeId);
            var latestData = employeeEfficiencies.Where(x => x.YearMonth == YearMonth).OrderByDescending(x => x.Id).FirstOrDefault();
            if (latestData != null)
            {
                if (latestData.Efficiency == models.EfficiencyNew)
                {
                    result.AddError("خطا", "ضریب کارایی تغییری نکرده است");
                    return result;
                }
            }
            //var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeId(models.EmployeeId).GetAwaiter().GetResult();
            //if (employee.Efficiency == models.EfficiencyNew)
            //{
            //    result.AddError("خطا", "ضریب کارایی تغییری نکرده است");
            //    return result;
            //}

            //employee.Efficiency = models.EfficiencyNew;

            foreach (var item in employeeEfficiencies.ToList())
            {
                item.IsLatest = false;
            }

            var EmployeeEfficiencyHistory = new EmployeeEfficiencyHistory()
            {
                Efficiency = models.EfficiencyNew,
                EmployeeId = models.EmployeeId,
                YearMonth = YearMonth,
                InsertDate = DateTime.Now,
                InsertUser = currentUser,
                IsLatest = true,
                RemoteIpAddress = models.RemoteIpAddress,
            };

            _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Add(EmployeeEfficiencyHistory);
            var status = _kscHrUnitOfWork.Save();
            if (status > 0)
            {
                return result;
            }
            result.AddError("", "عملیات نا موفق بود");
            return result;
        }

        public FilterResult<EmployeeEfficiencyHistoryModel> GetReportEmployeeEfficiencyHistoryData(SearchEmployeeEfficiencyHistoryModel Filter)
        {
            var query = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetAllQueryable().AsQueryable();
            if (Filter.EmployeeId > 0)
                query = query.Where(x => x.EmployeeId == Filter.EmployeeId);

            var result = _FilterHandler.GetFilterResult<EmployeeEfficiencyHistory>(query, Filter, nameof(EmployeeEfficiencyHistory.Id));
            var finaldata = _mapper.Map<List<EmployeeEfficiencyHistoryModel>>(result.Data);
            foreach (var item in finaldata)
            {
                item.InsertDateShamsi = item.InsertDate.HasValue ? item.InsertDate.Value.ToShortPersianDateTimeString() : "";
            }
            var modelResult = new FilterResult<EmployeeEfficiencyHistoryModel>
            {
                Data = finaldata,
                Total = result.Total
            };
            return modelResult;
        }

        public async Task<KscResult> EmployeeEfficiencyLatestSendToMIS(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();

            try
            {

                var YearMonth = int.Parse(model.DateTimeSheet);

                var query_EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetLatestData(YearMonth).ToList();

                #region پر کردن مدل
                var MONTHLY_Efficiency = new StringBuilder();
                foreach (var item in query_EmployeeEfficiencyHistory)
                {
                    MONTHLY_Efficiency.AppendLine(item.YearMonth + "|" + item.Employee.EmployeeNumber + "|" + item.Efficiency);
                }
                //نوشتن در فایلها تمام میشود
                var content_MONTHLY_Efficiency = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_Efficiency.ToString()));

                #endregion

                #region Copy File
                var FileforMIS = new List<File>{
                    new File { filename = "MONTHLY_Efficiency" , file = content_MONTHLY_Efficiency },
                };

                result = _misUpdate.SendTextByteFileToMis(Utility.ServerPathSendFileStream, FileforMIS);


                #endregion
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }

            return result;
        }
        public async Task<KscResult> EmployeeEfficiencyLatestSendToMISStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            try
            {
                var YearMonth = int.Parse(model.Yearmonth);
                if (!model.IsBackStep)
                {
                    var query_EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetLatestData(YearMonth).ToList();

                    #region پر کردن مدل
                    var MONTHLY_Efficiency = new StringBuilder();
                    foreach (var item in query_EmployeeEfficiencyHistory)
                    {
                        MONTHLY_Efficiency.AppendLine(item.YearMonth + "|" + item.Employee.EmployeeNumber + "|" + item.Efficiency.ToString().Replace("/", "."));
                    }
                    //نوشتن در فایلها تمام میشود
                    var content_MONTHLY_Efficiency = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(MONTHLY_Efficiency.ToString()));

                    #endregion

                    #region Copy File
                    var FileforMIS = new List<File>{
                    new File { filename = "MONTHLY_Efficiency" , file = content_MONTHLY_Efficiency },
                };
                    result = _misUpdate.SendTextByteFileToMis(Utility.ServerPathSendFileStream, FileforMIS);
                    if (result.Success)
                    {
                        var data = new EFCNC_PCN()
                        {
                            DAT_PYM_EPAYO = model.Yearmonth
                        };
                        ReturnData<EFCNC_PCN> misResult = _misUpdate.ConnectHRToMIS<EFCNC_PCN>(data, "S6XML031", "EFCNC_PCN", model.DomainName);
                        if (misResult.IsSuccess)
                        {
                            model.Result = "انتقال فایل ضریب کارایی ماه به MIS با موفقیت ثبت شد";
                            model.ResultCount = query_EmployeeEfficiencyHistory.Count();
                            result = await _procedureService.InsertStepProcedure(model);
                        }
                        else
                        {
                            result.AddError("", misResult.Messages[0]);
                            return result;
                        }
                    }
                }
                else
                {
                    result = await _procedureService.InsertStepProcedure(model);
                }
                await _kscHrUnitOfWork.SaveAsync();
                #endregion
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }

            return result;
        }
        /// <summary>
        /// تغیر وضعیت ضریب کارایی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> EmployeeEfficiencyChangeStatus(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();

            try
            {

                var yearmonth = int.Parse(model.Yearmonth);
                //var date = Utility.GetDateTimeFromYearMonth(model.Yearmonth);
                //date = date.AddMonths(1);
                //yearmonth = Utility.GetYearMonthShamsiByMiladiDate(date);
                var systemSequenceStatusId = EnumSystemSequenceStatusEfficiencyMonth.InActiveEfficiency.Id;
                if (model.IsBackStep)
                {
                    systemSequenceStatusId = EnumSystemSequenceStatusEfficiencyMonth.ActiveEfficiency.Id;
                    model.Result = EnumSystemSequenceStatusEfficiencyMonth.ActiveEfficiency.Name;
                }
                else
                {
                    model.Result = EnumSystemSequenceStatusEfficiencyMonth.InActiveEfficiency.Name;
                }
                //int yearmonth = Int32.Parse(model.Yearmonth);
                var efficiencymodel = _kscHrUnitOfWork.EmployeeEfficiencyMonthRepository.FirstOrDefault(x => x.YearMonth == yearmonth);
                if (efficiencymodel == null)
                {
                    _kscHrUnitOfWork.EmployeeEfficiencyMonthRepository.Add(new EmployeeEfficiencyMonth()
                    {
                        InsertDate = DateTime.Now,
                        InsertUser = model.CurrentUser,
                        InsertAuthenticateUserName = model.AuthenticateUserName,
                        SystemSequenceStatusId = systemSequenceStatusId,
                        YearMonth = yearmonth,
                        IsActive = true,
                    });
                }
                else
                {
                    efficiencymodel.UpdateDate = DateTime.Now;
                    efficiencymodel.UpdateUser = model.CurrentUser;
                    efficiencymodel.SystemSequenceStatusId = systemSequenceStatusId;
                    efficiencymodel.UpdateAuthenticateUserName = model.AuthenticateUserName;
                }
                result = await _procedureService.InsertStepProcedure(model);
                if (result.Success)
                    await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }

            return result;
        }


        public async Task<KscResult> EmployeeEfficiencyLatestSendToMIS_Old(SearchMonthTimeSheetModel model)
        {
            var result = new KscResult();

            try
            {

                var YearMonth = int.Parse(model.DateTimeSheet);

                var query_EmployeeEfficiencyHistory = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetLatestData(YearMonth).ToList();

                #region پر کردن مدل

                string path = @"\\srv2359\sharefiles\mis\per\";//http://wapi.ksc.ir/MisService/api/IT/SendFileStream
                                                               //Byte[] bytes = File.ReadAllBytes(path);
                                                               //String file = Convert.ToBase64String(bytes);
                                                               ////string path = @"d:\\MISTXT\";
                System.IO.StreamWriter MONTHLY_Efficiency = new System.IO.StreamWriter(path + "MONTHLY_Efficiency.TXT"); //d:\\MISTXT\MONTHLY_SHIFT.txt

                foreach (var item in query_EmployeeEfficiencyHistory)
                {
                    MONTHLY_Efficiency.WriteLine(item.YearMonth + "|" + item.Employee.EmployeeNumber + "|" + item.Efficiency);
                }

                MONTHLY_Efficiency.Close();


                #endregion

                #region Copy File
                List<string> fileNames = new() { "MONTHLY_Efficiency", };

                var ServerPath = Utility.ServerPathSendFile; //"https://wapi.ksc.ir/MisService/api/IT/SendFile";
                var uri = $"{ServerPath}";
                foreach (var item in fileNames)
                {
                    using (var client = new HttpClient())
                    {
                        string env;
#if DEBUG
                        env = "M";
#else
                        env = "L";
#endif

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        SendFile sendFileModel = new SendFile { application = "PER", filename = item + ".txt", enviroment = env };
                        var postTask = client.PostAsJsonAsync<SendFile>(uri, sendFileModel);
                        postTask.Wait();


                        var resultApi = postTask.Result;
                        if (resultApi.IsSuccessStatusCode)
                        {
                            var returnValue = resultApi.Content.ReadAsStringAsync();

                            var modelObj = JsonConvert.DeserializeObject<ReturnData<string>>(returnValue.Result);
                            if (modelObj.IsSuccess == true)
                            {

                            }
                            else
                            {
                                throw new HRBusinessException(Validations.RepetitiveId, "انتقال فایل انجام نشد");

                                //result.AddError("خطا", "انتقال فایل انجام نشد");
                                //return result;
                            }
                        }
                        else
                        {
                            throw new HRBusinessException(Validations.RepetitiveId, "ارتباط با سیستم  برقرار نشد");
                            //result.AddError("خطا", "ارتباط با سیستم  برقرار نشد");
                            //return result;
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// ایجاد ضرایب کارایی افراد برای ماه بعد -WorkCalendar انتقال داده شده
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public KscResult CreateEmployeeEfficiency(int yearMonth)
        {
            var result = new KscResult();
            var nexmonth = _kscHrUnitOfWork.WorkCalendarRepository.GetNextMonth(yearMonth);
            var YearMonthShamsi_Next = int.Parse($"{nexmonth.Substring(0, 7).Replace("/", "")}");

            var today = DateTime.Now;
            var EmployeeEfficiencyHistories = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetLatestData(yearMonth).ToList();
            List<EmployeeEfficiencyHistory> newDatas = new List<EmployeeEfficiencyHistory>();
            foreach (var item in EmployeeEfficiencyHistories)
            {
                EmployeeEfficiencyHistory employee = new EmployeeEfficiencyHistory();
                employee.YearMonth = YearMonthShamsi_Next;
                employee.Efficiency = item.Efficiency;
                employee.EmployeeId = item.EmployeeId;
                employee.IsLatest = true;
                employee.InsertDate = today;
                employee.InsertUser = "System";
                newDatas.Add(employee);

                item.IsLatest = false;
            }

            _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.AddRange(newDatas);
            var status = _kscHrUnitOfWork.Save();
            if (status > 0)
            {
                return result;
            }
            result.AddError("", "عملیات نا موفق بود");
            return result;
        }
        public async Task<KscResult> CreateEmployeeEfficiencyStep(UpdateStatusByYearMonthProcedureModel model)
        {
            var result = new KscResult();
            try
            {
                var yearMonth = Convert.ToInt32(model.Yearmonth);
                if (_kscHrUnitOfWork.Stepper_ProcedureStatusRepository.CheckAllStepsDoneByYearMonthParent(yearMonth, EnumProcedureStep.SallaryBill.Id) == false)
                {
                    result.AddError("خطا", "فرایند محاسبه حقوق ودستمزد انجام نشده است");
                    return result;
                }
                var nextMonth = _kscHrUnitOfWork.WorkCalendarRepository.GetNextMonth(yearMonth);
                var YearMonthShamsi_Next = int.Parse($"{nextMonth.ToString().Substring(0, 7).Replace("/", "")}");

                var today = DateTime.Now;
                var EmployeeEfficiencyHistories = _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.GetLatestData(yearMonth).ToList();
                List<EmployeeEfficiencyHistory> newDatas = new List<EmployeeEfficiencyHistory>();
                if (_kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.Any(x => x.YearMonth == YearMonthShamsi_Next && x.InsertUser == "System"))
                {
                    result.AddError("خطا", "ضریب کارایی این ماه قبلا ایجاد شده");
                    return result;
                }
                foreach (var item in EmployeeEfficiencyHistories)
                {
                    EmployeeEfficiencyHistory employee = new EmployeeEfficiencyHistory();
                    employee.YearMonth = YearMonthShamsi_Next;
                    employee.Efficiency = item.Efficiency;
                    employee.EmployeeId = item.EmployeeId;
                    employee.IsLatest = true;
                    employee.InsertDate = today;
                    employee.InsertUser = "System";
                    newDatas.Add(employee);
                    item.IsLatest = false;
                }

                _kscHrUnitOfWork.EmployeeEfficiencyHistoryRepository.AddRange(newDatas);
                model.Result = "ایجاد ضریب کارایی ماه با موفقیت ثبت شد";
                model.ResultCount = newDatas.Count();
                result = await _procedureService.InsertStepProcedure(model);
                if (result.Success)
                {
                    var status = _kscHrUnitOfWork.Save();
                    if (status > 0)
                    {
                        return result;
                    }
                }
                result.AddError("", "عملیات نا موفق بود");
            }
            catch (Exception ex)
            {

                result.AddError("", "خطا در عملیات");
            }
            return result;

        }


    }
}
