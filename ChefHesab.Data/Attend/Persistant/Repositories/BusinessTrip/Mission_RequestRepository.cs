using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Repositories.BusinessTrip;
using DNTPersianUtils.Core;
using Ksc.HR.Share.Model.BusinessTrip;
using Ksc.HR.Share.Model.Mission;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Data.Persistant.Repositories.BusinessTrip
{
    public class Mission_RequestRepository : EfRepository<Mission_Request, int>, IMission_RequestRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Mission_RequestRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Mission_Request> GetIncludedWF_Request()
        {
            return _kscHrContext.Mission_Requests.Include(a => a.WF_Request)
                .AsQueryable();
        }
        /// <summary>
        /// تعداد روز های ماموریت رفته
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int GetMissionDurationCount(Mission_Request model, int employeeId)
        {
            var persianYearMonth = model.MissionStartDate.GetPersianMonthStartAndEndDates();
            //var missionsData = GetIncludedWF_Request().Where(x => x.Id != model.Id && x.WF_Request.StatusId != 2 && x.WF_Request.StatusId != 51 && x.WF_Request.StatusId != 52 && x.WF_Request.EmployeeId == employeeId && x.MissionStartDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate);
            var missionsData = GetIncludedWF_Request().Where(x => x.Id != model.Id
            //&& x.MissionEndDate <= model.MissionStartDate
            && !EnumCancelMissionStatus.List.Any(i => i == x.WF_Request.StatusId) && x.WF_Request.EmployeeId == employeeId
            &&
            ((x.MissionStartDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate) // شروع و پایان در یک ماه
            || (x.MissionStartDate < persianYearMonth.StartDate && x.MissionEndDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate)// شروع ماه قبل و پایان  ماه جاری ماموریت
            || (x.MissionStartDate >= persianYearMonth.StartDate && x.MissionStartDate <= persianYearMonth.EndDate && x.MissionEndDate > persianYearMonth.EndDate)// شروع ماه جاری ماموریت و پایان  ماه بعد
            ));
            if (model.MissionTypeId > 0)
                missionsData = missionsData.Where(x => x.MissionTypeId == model.MissionTypeId);
            if (model.MissionLocationId > 0)
                missionsData = missionsData.Where(x => x.MissionLocationId == model.MissionLocationId);
            //
            var result = missionsData.ToList().Select(x => new
            {
                MissionStartDate = x.MissionStartDate < persianYearMonth.StartDate ? persianYearMonth.StartDate : x.MissionStartDate,
                MissionEndDate = x.MissionEndDate > persianYearMonth.EndDate ? persianYearMonth.EndDate.AddDays(1) : x.MissionEndDate,
            });
            //
            return (int)result.ToList().Sum(x => x.MissionEndDate.Subtract(x.MissionStartDate).TotalDays);
        }
        public int GetMissionDurationCurrentMonthCount1(Mission_Request model, int employeeId)
        {
            var persianYearMonth = model.MissionStartDate.GetPersianMonthStartAndEndDates();
            //var missionsData = GetIncludedWF_Request().Where(x => x.Id != model.Id && x.WF_Request.StatusId != 2 && x.WF_Request.StatusId != 51 && x.WF_Request.StatusId != 52 && x.WF_Request.EmployeeId == employeeId && x.MissionStartDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate);
            var missionsData = GetIncludedWF_Request().Where(x => x.Id != model.Id

            && !EnumCancelMissionStatus.List.Any(i => i == x.WF_Request.StatusId) && x.WF_Request.EmployeeId == employeeId
            &&
            ((x.MissionStartDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate) // شروع و پایان در یک ماه
            || (x.MissionStartDate < persianYearMonth.StartDate && x.MissionEndDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate)// شروع ماه قبل و پایان  ماه جاری ماموریت
            || (x.MissionStartDate >= persianYearMonth.StartDate && x.MissionStartDate <= persianYearMonth.EndDate && x.MissionEndDate > persianYearMonth.EndDate)// شروع ماه جاری ماموریت و پایان  ماه بعد
            ));
            if (model.MissionTypeId > 0)
                missionsData = missionsData.Where(x => x.MissionTypeId == model.MissionTypeId);
            if (model.MissionLocationId > 0)
                missionsData = missionsData.Where(x => x.MissionLocationId == model.MissionLocationId);
            //
            var result = missionsData.ToList().Select(x => new
            {
                MissionStartDate = x.MissionStartDate < persianYearMonth.StartDate ? persianYearMonth.StartDate : x.MissionStartDate,
                MissionEndDate = x.MissionEndDate > persianYearMonth.EndDate ? persianYearMonth.EndDate.AddDays(1) : x.MissionEndDate,
            });
            //
            int countmision = (int)result.ToList().Sum(x => x.MissionEndDate.Subtract(x.MissionStartDate).TotalDays) + (int)((model.MissionEndDate > persianYearMonth.EndDate ? persianYearMonth.EndDate.AddDays(1) : model.MissionEndDate).Subtract(model.MissionStartDate).TotalDays);
            return countmision;
        }
        public int GetMissionDurationCurrentMonthCount(Mission_Request model, int employeeId)
        {
            var persianYearMonth = model.MissionStartDate.GetPersianMonthStartAndEndDates();
            var missionDataCurrentMonth = GetMissionDataCurrentMonth(model, employeeId, persianYearMonth);
            int countmision = GetCountMisionByMissionCurrentMonthData(model, missionDataCurrentMonth, persianYearMonth);
            return countmision;
        }
        public List<MissionCurrentMonthModel> GetMissionDataCurrentMonth(Mission_Request model, int employeeId, PersianMonth persianYearMonth)
        {
            //persianYearMonth نسبت به تاریخ شروع ماموریت گرفته شده است

            //	var persianYearMonth = model.MissionStartDate.GetPersianMonthStartAndEndDates();
            var missionsData = GetIncludedWF_Request().Where(x => x.Id != model.Id

        && !EnumCancelMissionStatus.List.Any(i => i == x.WF_Request.StatusId) && x.WF_Request.EmployeeId == employeeId
        &&
        ((x.MissionStartDate >= persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate) // شروع و پایان در یک ماه
        || (x.MissionStartDate < persianYearMonth.StartDate && x.MissionEndDate > persianYearMonth.StartDate && x.MissionEndDate <= persianYearMonth.EndDate)// شروع ماه قبل و پایان  ماه جاری ماموریت
        || (x.MissionStartDate >= persianYearMonth.StartDate && x.MissionStartDate <= persianYearMonth.EndDate && x.MissionEndDate > persianYearMonth.EndDate)// شروع ماه جاری ماموریت و پایان  ماه بعد
        ));
            if (model.MissionTypeId > 0)
                missionsData = missionsData.Where(x => x.MissionTypeId == model.MissionTypeId);
            if (model.MissionLocationId > 0)
                missionsData = missionsData.Where(x => x.MissionLocationId == model.MissionLocationId);
            //
            var result = missionsData.ToList().Select(x => new MissionCurrentMonthModel
            {
                MissionStartDate = x.MissionStartDate < persianYearMonth.StartDate ? persianYearMonth.StartDate : x.MissionStartDate,
                MissionEndDate = x.MissionEndDate > persianYearMonth.EndDate ? persianYearMonth.EndDate.AddDays(1) : x.MissionEndDate,
                StatusId = x.WF_Request.StatusId,
                MissionId = x.Id,
                MissionNumber = x.MissionNumber,
                MissionPaymentDayCount = x.MissionPaymentDayCount,
                NotPayStartDate = x.NotPayStartDate,
                NotPayEndDate = x.NotPayEndDate,
                MissionStartDateReal = x.MissionStartDate,
                MissionEndDateReal = x.MissionEndDate


            }).ToList();
            return result;
            //
            //int countmision = (int)result.ToList().Sum(x => x.MissionEndDate.Subtract(x.MissionStartDate).TotalDays) + (int)((model.MissionEndDate > persianYearMonth.EndDate ? persianYearMonth.EndDate.AddDays(1) : model.MissionEndDate).Subtract(model.MissionStartDate).TotalDays);
            //return countmision;
        }
        public int GetCountMisionByMissionCurrentMonthData(Mission_Request model, List<MissionCurrentMonthModel> MissionCurrentMonthList, PersianMonth persianYearMonth)
        {
            int countmision = (int)MissionCurrentMonthList.ToList().Sum(x => x.MissionEndDate.Subtract(x.MissionStartDate).TotalDays)
                + (int)((model.MissionEndDate > persianYearMonth.EndDate ? persianYearMonth.EndDate.AddDays(1) : model.MissionEndDate)
                .Subtract(model.MissionStartDate).TotalDays);
            return countmision;
        }
        public MissionCountDataModel GetMissionCountCurrentMonthCount(Mission_Request model, int employeeId)
        {
            var persianYearMonth = model.MissionStartDate.GetPersianMonthStartAndEndDates();
            var missionDataCurrentMonth = GetMissionDataCurrentMonth(model, employeeId, persianYearMonth);
            int countmisionCurrentMonth = GetCountMisionByMissionCurrentMonthData(model, missionDataCurrentMonth, persianYearMonth);
            //
            missionDataCurrentMonth = missionDataCurrentMonth.Where(x => !string.IsNullOrWhiteSpace(x.MissionNumber)).ToList();
            int countmisionByMissionNumber = (int)missionDataCurrentMonth.Sum(x => x.MissionEndDate.Subtract(x.MissionStartDate).TotalDays);
            var currentMonthMissionByMissionNumberCountByPayment1 = missionDataCurrentMonth
                .Where(x => x.MissionPaymentDayCount.HasValue && x.MissionPaymentDayCount != 0);
            int sumData = 0;
            foreach (var item in currentMonthMissionByMissionNumberCountByPayment1)
            {
                var missionEndDate = item.MissionEndDateReal.AddDays(-1);
                var notInSameMonth = Utility.GetYearMonthShamsiByMiladiDate(missionEndDate) != Utility.GetYearMonthShamsiByMiladiDate(item.MissionStartDateReal);
                if (notInSameMonth)//|| (item.NotPayEndDate.HasValue && item.MissionStartDate > item.NotPayEndDate)) //شروع و پایان ماموریت در یک ماه نباشد
                {
                    if (item.NotPayStartDate.HasValue && item.MissionStartDate.Date < item.NotPayStartDate.Value.Date)
                    {
                        sumData += (int)(item.NotPayStartDate.Value.Date.Subtract(item.MissionStartDate.Date).TotalDays);
                    }
                    else if (item.NotPayEndDate.HasValue && item.MissionStartDate.Date > item.NotPayEndDate.Value.Date)
                        sumData += (int)(item.MissionEndDate.Subtract(item.MissionStartDate).TotalDays);
                    else if (item.NotPayStartDate.HasValue && item.MissionStartDate.Date == item.NotPayStartDate.Value.Date)
                        sumData += (int)(item.MissionEndDate.Subtract(item.NotPayEndDate.Value).TotalDays);
                }
                else
                {
                    sumData += item.MissionPaymentDayCount.Value;
                }
            }

            int currentMonthMissionByMissionNumberCountByPayment = (int)missionDataCurrentMonth
               .Where(x => x.MissionNumber != null && x.MissionPaymentDayCount.HasValue && x.MissionPaymentDayCount != 0)
               .Sum(x => x.MissionEndDate.Subtract(x.MissionStartDate).TotalDays);
            MissionCountDataModel result = new MissionCountDataModel
            {
                MissionCountCurrentMonth = countmisionCurrentMonth,
                CurrentMonthMissionByMissionNumberCount = countmisionByMissionNumber,
                CurrentMonthMissionByMissionNumberCountByPayment = sumData//currentMonthMissionByMissionNumberCountByPayment
            };

            return result;
        }
        public IQueryable<Mission_Request> GetAllMissionConfirmRequest()
        {
            return GetIncludedWF_Request()
                .Include(x => x.WF_Request)
                .ThenInclude(x => x.WF_RequestHistories)
                .Include(a => a.City)
                .ThenInclude(x => x.Province)
                .ThenInclude(x => x.Country)
                .Include(x => x.Employee)
                .Include(x => x.Mission_Location)
                .Include(x => x.Mission_Type)
                .Include(x => x.Mission_Goal)
                .Include(x => x.EntryDayTimeSetting)
                .Include(x => x.ExitDayTimeSetting);
        }
        public Mission_Request GetMissionConfirmRequest(int id)
        {
            return GetAllMissionConfirmRequest()
                .FirstOrDefault(i => i.WfRequestId == id);
        }
        public async Task<Mission_Request> GetOne(int id)
        {
            return await GetIncludedWF_Request().FirstAsync(a => a.Id == id);
        }
        public Mission_Request GetOneSync(int id)
        {
            return GetMission_Requests().FirstOrDefault(a => a.Id == id);
        }
        public IQueryable<Mission_Request> GetMission_Requests()
        {
            return _kscHrContext.Mission_Requests
                .Include(a => a.EntryDayTimeSetting)
                .Include(a => a.Mission_Location)
                .Include(a => a.Mission_Type)
                .Include(a => a.Employee)
                .Include(a => a.City)
                .Include(a => a.Mission_Goal)
                .Include(a => a.WF_Request).ThenInclude(a => a.Employee)
                .Include(a => a.WF_Request).ThenInclude(a => a.WF_RequestHistories)
                .Include(a => a.WF_Request).ThenInclude(a => a.WF_Status)
                .AsQueryable();
        }


        public bool DeleteById(int id, int statusId)
        {
            var mission_Request = GetIncludedWF_Request().FirstOrDefault(a => a.Id == id);
            if (mission_Request != null)
            {
                if (mission_Request.WF_Request.StatusId != statusId)
                    return false;
                var wf_Request = _kscHrContext.WF_Requests.AsQueryable().Include(x => x.WF_RequestHistories)
                                                       .FirstOrDefault(x => x.Id == mission_Request.WfRequestId);
                //
                if (wf_Request.WF_RequestHistories.Count() != 1)
                    return false;
                //
                _kscHrContext.Mission_Requests.Remove(mission_Request);
                _kscHrContext.WF_RequestHistories.RemoveRange(wf_Request.WF_RequestHistories);
                _kscHrContext.WF_Requests.Remove(wf_Request);
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool DeleteById(int id, int statusId, bool isValidDeleteOtherStatus)
        {
            var mission_Request = GetIncludedWF_Request().FirstOrDefault(a => a.Id == id);
            if (mission_Request != null)
            {
                if (mission_Request.WF_Request.StatusId != statusId && isValidDeleteOtherStatus == false)
                    return false;
                var wf_Request = _kscHrContext.WF_Requests.AsQueryable().Include(x => x.WF_RequestHistories)
                                                       .FirstOrDefault(x => x.Id == mission_Request.WfRequestId);
                //
                if (mission_Request.WF_Request.StatusId == statusId && wf_Request.WF_RequestHistories.Count() != 1)
                    return false;
                //
                //
                if (isValidDeleteOtherStatus && wf_Request.WF_RequestHistories.Count() != 2)
                    return false;
                //
                _kscHrContext.Mission_Requests.Remove(mission_Request);
                _kscHrContext.WF_RequestHistories.RemoveRange(wf_Request.WF_RequestHistories);
                _kscHrContext.WF_Requests.Remove(wf_Request);
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool DeleteByIdInExeption(int id)
        {
            var mission_Request = GetIncludedWF_Request().FirstOrDefault(a => a.Id == id);
            if (mission_Request != null)
            {

                var wf_Request = _kscHrContext.WF_Requests.AsQueryable().Include(x => x.WF_RequestHistories)
                                                       .FirstOrDefault(x => x.Id == mission_Request.WfRequestId);
                //
                var wF_RequestHistories = _kscHrContext.WF_RequestHistories.Where(s => s.RequestId == mission_Request.WfRequestId);
                //
                _kscHrContext.Mission_Requests.Remove(mission_Request);
                _kscHrContext.WF_RequestHistories.RemoveRange(wF_RequestHistories);
                _kscHrContext.WF_Requests.Remove(wf_Request);
                return true;
            }
            else
            {
                return false;
            }

        }
        public Mission_Request GetMissionByWFRequestId(int wFRequestId)
        {
            return _kscHrContext.Mission_Requests.Where(x => x.WfRequestId == wFRequestId).Include(x => x.Mission_Type).First();
        }
        public Mission_Request GetMissionEmployeeReportByWFRequestId(int wFRequestId)
        {
            return _kscHrContext.Mission_Requests.AsQueryable().Include(x => x.WF_Request).ThenInclude(x => x.WF_Status).Include(x => x.WF_Request).ThenInclude(x => x.Employee).ThenInclude(x => x.TeamWork).Include(x => x.City).Include(x => x.Mission_Type).Include(x => x.Mission_Goal).Include(x => x.Employee).Where(x => x.WfRequestId == wFRequestId).FirstOrDefault();
        }
        public Mission_Request GetMissionByWFRequestId_EmployeeNumber(int wFRequestId, string employeenumber)
        {
            return _kscHrContext.Mission_Requests.AsQueryable().Include(x => x.WF_Request).ThenInclude(x => x.Employee).Include(x => x.WF_Request).ThenInclude(x => x.WF_RequestHistories).Include(x => x.City).Include(x => x.Mission_Type).Where(x => x.WfRequestId == wFRequestId && x.WF_Request.Employee.EmployeeNumber == employeenumber).FirstOrDefault();
        }
        public IQueryable<Mission_Request> GetMission_RequestsByEmployeeID(int employeeId)
        {
            return GetIncludedWF_Request().Where(x => x.WF_Request.EmployeeId == employeeId);
        }
        public IQueryable<Mission_Request> GetMissionByListWFRequestId(List<int> wFRequestId)
        {
            return _kscHrContext.Mission_Requests.Where(x => wFRequestId.Any(w => w == x.WfRequestId)).Include(x => x.City);
        }
        public async Task<Mission_Request> GetMissionByWFRequestIdAsync(int wFRequestId)
        {
            return await _kscHrContext.Mission_Requests.FirstOrDefaultAsync(x => x.WfRequestId == wFRequestId);
        }
        public IQueryable<Mission_Request> GetMissionsForSendToMis(int yearmonth, int statusID)
        {
            return GetAllMissionConfirmRequest().Include(x => x.WF_Request.Employee).Where(x => x.PeymentDate == yearmonth && x.WF_Request.StatusId == statusID).AsQueryable();
        }
        public IQueryable<Mission_Request> GetMissionsForStepperSendToMis(int yearmonth, int statusID)
        {
            return GetAllMissionConfirmRequest().Include(x => x.WF_Request.Employee).Where(x => (x.PeymentDate == yearmonth || (x.PeymentDate < yearmonth && x.PaidDate.HasValue == false)) && x.WF_Request.StatusId == statusID).AsQueryable();
        }

        /// <summary>
        /// ماموریتهایی که در حقوق-دستمزد تایید نهایی شده و  برای پرداخت از طریق استپر ارسال شده اند
        /// </summary>
        /// <returns></returns>
        public IQueryable<Mission_Request> GetMissionsIsPaid()
        {
            return _kscHrContext.Mission_Requests.Where(x => x.IsPaid == true || x.PaidDate != null);
        }
        //public IQueryable<Mission_Request> GetMission_RequestStatus(int mission_RequestId)
        //{

        //    var Mission_RequestItem = GetIncludedWF_Request()
        //            .Where(a => a.Id == mission_RequestId).AsQueryable().AsNoTracking();//,[ProcessId]  ,[StatusId]


        //}
        public void MissionDateManagement(MissionDateManagementInputModel model, DateTime missionEndDate, ref DateTime? notPayStartDate, ref DateTime? notPayEndDate)
        {
            int missionDurationFromStartDate = model.TotalDays;
            int missionDurationAtEndDate = 0;
            DateTime? startDayInMonthFromEndMission = null;
            var endDateFromStartMission = model.MissionStartDate.GetPersianMonthStartAndEndDates().EndDate;
            if (Utility.GetYearMonthShamsiByMiladiDate(missionEndDate) != Utility.GetYearMonthShamsiByMiladiDate(model.MissionStartDate))
            {

                missionDurationFromStartDate = (int)(endDateFromStartMission.Subtract(model.MissionStartDate)).TotalDays + 1;
                //

                var startDateFromEndMission = missionEndDate.GetPersianMonthStartAndEndDates().StartDate;
                startDayInMonthFromEndMission = startDateFromEndMission.Date;
                missionDurationAtEndDate = (int)(model.MissionEndDate.Subtract(startDateFromEndMission)).TotalDays;


            }

            int dayNotPayFromStartDate = 0;
            int dayNotPayFromStartDateInMissionEndDate = 0;
            var Mission_Request = new Mission_Request()
            {
                Id = model.MissionRequestId,
                MissionStartDate = model.MissionStartDate,
                MissionEndDate = model.MissionEndDate,
                MissionLocationId = model.MissionLocationId,
            };
            var missionCountDataByStartDate = GetMissionCountCurrentMonthCount(Mission_Request, employeeId: model.EmployeeId);
            if (missionCountDataByStartDate.CurrentMonthMissionByMissionNumberCountByPayment + missionDurationFromStartDate >= model.MissionMinimumDay.Value)
            {
                if (missionCountDataByStartDate.CurrentMonthMissionByMissionNumberCountByPayment >= model.MissionMinimumDay.Value)
                    dayNotPayFromStartDate = missionDurationFromStartDate;
                else
                    dayNotPayFromStartDate = missionCountDataByStartDate.CurrentMonthMissionByMissionNumberCountByPayment + missionDurationFromStartDate - model.MissionMinimumDay.Value + 1;
            }
            if (missionDurationAtEndDate != 0)
            {
                var startDateFromEndMission = model.MissionEndDate.GetPersianMonthStartAndEndDates().StartDate;
                var missionCountDataByEndDate = GetMissionCountCurrentMonthCount(new Mission_Request() { Id = model.MissionRequestId, MissionStartDate = startDateFromEndMission, MissionEndDate = model.MissionEndDate }, employeeId: model.EmployeeId);
                if (missionCountDataByEndDate.CurrentMonthMissionByMissionNumberCountByPayment + missionDurationAtEndDate >= model.MissionMinimumDay.Value)
                {
                    if (missionCountDataByEndDate.CurrentMonthMissionByMissionNumberCountByPayment >= model.MissionMinimumDay.Value)
                        dayNotPayFromStartDateInMissionEndDate = missionDurationAtEndDate;
                    else
                        dayNotPayFromStartDateInMissionEndDate = missionCountDataByEndDate.CurrentMonthMissionByMissionNumberCountByPayment + missionDurationAtEndDate - model.MissionMinimumDay.Value + 1;
                }

            }
            if (dayNotPayFromStartDate != 0)
            {
                if (Utility.GetYearMonthShamsiByMiladiDate(missionEndDate) == Utility.GetYearMonthShamsiByMiladiDate(model.MissionStartDate))
                {
                    notPayStartDate = model.MissionEndDate.AddDays(dayNotPayFromStartDate * (-1));
                    notPayEndDate = missionEndDate;
                }
                else
                {
                    notPayStartDate = endDateFromStartMission.AddDays((dayNotPayFromStartDate - 1) * (-1));

                    if (dayNotPayFromStartDateInMissionEndDate == 0)
                    {
                        notPayEndDate = endDateFromStartMission;
                    }
                    else
                    {
                        notPayEndDate = startDayInMonthFromEndMission.Value.AddDays(dayNotPayFromStartDateInMissionEndDate - 1);
                    }

                }
            }
            else
            {
                if (dayNotPayFromStartDateInMissionEndDate != 0)
                {
                    notPayStartDate = startDayInMonthFromEndMission;
                    notPayEndDate = startDayInMonthFromEndMission.Value.AddDays(dayNotPayFromStartDateInMissionEndDate - 1);
                }
            }

        }
    }
}
