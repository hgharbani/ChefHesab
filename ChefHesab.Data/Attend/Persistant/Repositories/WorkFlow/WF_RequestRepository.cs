using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Share.Model.WF_Request;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using KSC.Common;
using Ksc.HR.Domain.Entities.Chart;

namespace Ksc.HR.Data.Persistant.Repositories.WorkFlow
{
    public class WF_RequestRepository : EfRepository<WF_Request, int>, IWF_RequestRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_RequestRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<WF_Request> GetAllRequestByRelated()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Status).Include(x => x.WF_Requests).Include(x => x.WF_Process).Include(x => x.Employee).ThenInclude(x => x.TeamWork).Include(x => x.Employee).ThenInclude(x => x.WorkGroup).Include(x => x.OnCall_Requests).Include(x => x.WF_RequestHistories).Include(x => x.ParentRequest);
        }
        public IQueryable<WF_Request> GetAllRequestByRelatedEntity()
        {
            var query = _kscHrContext.WF_Requests.Include(x => x.WF_Status).Include(x => x.WF_Requests).Include(x => x.WF_Process).Include(x => x.Employee).ThenInclude(x => x.TeamWork).Include(x => x.Employee).ThenInclude(x => x.WorkGroup).Include(x => x.OnCall_Requests).Include(x => x.ParentRequest);
            return query;
        }
        public IQueryable<WF_Request> GetAllRequestByAllRelated()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Status).ThenInclude(x => x.WF_StatusProcessManagements).Include(x => x.WF_Process).Include(x => x.Employee).ThenInclude(x => x.TeamWork).Include(x => x.Employee).ThenInclude(x => x.WorkGroup).Include(x => x.OnCall_Requests).Include(x => x.WF_RequestHistories);
        }
        public async Task<WF_Request> GetAllRequestByEmployeeIdProcessId(int employeeId, int processId)
        {

            //return (await GetAllAsync()).Where(x => x.EmployeeId == employeeId && x.ProcessId == processId).AsQueryable().Include(x=>x.Transfer_Requests);
            return await _kscHrContext.WF_Requests
                .AsQueryable()
                .Include(x => x.Transfer_Requests)
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.ProcessId == processId);

        }
        public IEnumerable<WF_Request> GetRequestsIncludEmployee()
        {
            return GetAll().AsQueryable().Include(x => x.Employee);
        }
        public async Task<WF_Request> GetRequestIncludHistoryIncludChildRequest(int id)
        {
            var query = _kscHrContext.WF_Requests.Include(x => x.WF_Requests).Include(x => x.WF_RequestHistories).ThenInclude(x => x.Parent);
            return await query.FirstAsync(x => x.Id == id);
        }
        public async Task<WF_Request> GetRequestIncludChildRequestAsNotracking(int id)
        {
            var query = _kscHrContext.WF_Requests.Include(x => x.WF_Requests).Include(x => x.WF_Process).Include(x => x.ParentRequest).AsNoTracking();
            return await query.FirstAsync(x => x.Id == id);
        }
        public IQueryable<WF_Request> GetAllRequestForOutBox()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Status).Include(x => x.WF_Process).Include(x => x.Employee).Include(x => x.WF_RequestHistories);
        }
        public async Task<WF_Request> GetAllRequestById(int id)
        {
            var query = GetAll().AsQueryable().Include(x => x.WF_Status).Include(x => x.WF_Process)
                               .Include(x => x.Employee).Include(x => x.WF_RequestHistories).AsNoTracking();
            return await query.FirstAsync(x => x.Id == id);
        }
        public IEnumerable<WF_Request> GetRequestsIncludStatusEmployee()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Status).Include(x => x.Employee).Include(x => x.WF_Process);
        }
        public IQueryable<WF_Request> GetActiveRequest()
        {
            return _kscHrContext.WF_Requests.Where(x => x.IsActive);
        }
        public IQueryable<WF_Request> GetActiveRequestsIncludStatusEmployee()
        {
            return _kscHrContext.WF_Requests.Where(x => x.IsActive).Include(x => x.WF_Status).Include(x => x.Employee).Include(x => x.WF_Process);
        }
        public void UpdateRange(List<WF_Request> wfRequest)
        {
            _kscHrContext.WF_Requests.UpdateRange(wfRequest);
        }
        public async Task<PromotionConfirmHelper> PromotionConfirmInWorkFlowRequest(ChangeGroupStatusInRequestEndModel model)
        {
            var res = new KscResult();
            var result = new PromotionConfirmHelper();
            try
            {

                var oldHistories = new List<WF_RequestHistory>();
                var newHistories = new List<WF_RequestHistory>();
                var newRequests = new List<WF_Request>();

                var workFlowManagement = await _kscHrContext
                    .WF_WorkFlowManagements
                           .FirstOrDefaultAsync(x => x.IsActive && x.ProcessId == model.ProcessId && !x.IsManual
                                   && x.CurrentStatusId == model.CurrentStatusId && x.NextStatusId == model.NextStatusId);

                if (workFlowManagement == null)
                {
                    res.AddError("مدیریت تغییرات درخواست یافت نشد", $"CurrentStatusId:{model.CurrentStatusId}");
                    res.Success = false;

                }
                foreach (var requestId in model.RequestIds)
                {
                    var request = await GetByIdAsync(requestId);
                    if (request.StatusId != model.CurrentStatusId)
                    {
                        res.AddError("وضعیت درخواست تغییر یافته است", $"requestId:{request.Id}");
                        res.Success = false;
                    }
                    request.StatusId = model.NextStatusId;
                    request.UpdateUser = model.CurrentUserNumber;
                    request.SuperiorJobPositionCode = null;
                    request.SuperiorJobPositionCodeTemp = null;
                    var lastRequestHistory = await _kscHrContext.WF_RequestHistories
                        .Where(x => x.RequestId == requestId && x.EndDate == null).OrderByDescending(x => x.Id).FirstOrDefaultAsync();



                    if (lastRequestHistory == null)
                    {
                        res.AddError("تاریخچه تغییرات درخواست یافت نشد", $"requestId:{request.Id}");
                        res.Success = false;

                    }
                    if (lastRequestHistory != null)
                    {
                        lastRequestHistory.EndDate = DateTime.Now;
                        lastRequestHistory.EndUser = model.CurrentUserNumber;
                        lastRequestHistory.EndUserAuthenticateUserName = model.AuthenticateUserName;
                        oldHistories.Add(lastRequestHistory);


                        var newHistory = new WF_RequestHistory
                        {
                            InsertDate = DateTime.Now,
                            StartDate = DateTime.Now,
                            StatusId = model.NextStatusId,
                            RequestId = requestId,
                            ParentId = (lastRequestHistory != null) ? lastRequestHistory.Id : null,
                            StartUser = model.CurrentUserNumber,
                            StartUserAuthenticateUserName = model.AuthenticateUserName,
                            WorkFlowActionId = workFlowManagement.WorkFlowActionId,
                        };


                        newHistories.Add(newHistory);
                        newRequests.Add(request);
                    }

                }
                


                //_kscHrContext.WF_Requests.UpdateRange(newRequests);
                //await _kscHrContext.WF_RequestHistories.AddRangeAsync(newHistories);
                //_kscHrContext.WF_RequestHistories.UpdateRange(oldHistories);

                result.NewHistories = newHistories;
                result.OldHistories = oldHistories;
                result.NewRequests = newRequests;
                result.Result = res;

                return result;

            }
            catch (Exception ex)
            {
                res.Success = false;


            }
            return result;

        }
        public async Task ChangeWorkFlowRequestStatus(ChangeGroupStatusInRequestEndModel model)
        {
            try
            {

                var workFlowManagement = _kscHrContext.WF_WorkFlowManagements
                           .FirstOrDefault(x => x.IsActive && x.ProcessId == model.ProcessId //&& !x.IsManual
                                   && x.CurrentStatusId == model.CurrentStatusId && x.NextStatusId == model.NextStatusId);

                if (workFlowManagement == null)
                {

                    throw new Exception("مدیریت تغییرات درخواست یافت نشد");
                }
                foreach (var requestId in model.RequestIds)
                {
                    var request = await GetByIdAsync(requestId);
                    if (request.StatusId != model.CurrentStatusId)
                    {
                        throw new Exception("وضعیت درخواست تغییر یافته است");
                    }
                    request.StatusId = model.NextStatusId;
                    request.SuperiorJobPositionCode = null;
                    request.SuperiorJobPositionCodeTemp = null;
                    var lastRequestHistory = _kscHrContext.WF_RequestHistories
                        .Where(x => x.RequestId == requestId && x.EndDate == null).ToList().LastOrDefault();



                    if (lastRequestHistory == null)
                    {

                        throw new Exception("تاریخچه تغییرات درخواست یافت نشد");

                    }
                    lastRequestHistory.EndDate = DateTime.Now;
                    lastRequestHistory.EndUser = model.CurrentUserNumber;
                    lastRequestHistory.EndUserAuthenticateUserName = model.AuthenticateUserName;

                    var newHistory = new WF_RequestHistory
                    {
                        InsertDate = DateTime.Now,
                        StartDate = DateTime.Now,
                        StatusId = model.NextStatusId,
                        RequestId = requestId,
                        ParentId = lastRequestHistory.Id,
                        StartUser = model.CurrentUserNumber,
                        StartUserAuthenticateUserName = model.AuthenticateUserName,
                        WorkFlowActionId = workFlowManagement.WorkFlowActionId,
                    };

                    request.WF_RequestHistories.Add(newHistory);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }


}
