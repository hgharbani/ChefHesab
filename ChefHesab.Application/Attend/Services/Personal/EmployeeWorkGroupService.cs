using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.DTO.MIS;
using Ksc.HR.DTO.Personal.EmployeeWorkGroups;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.General;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeWorkGroupService : IEmployeeWorkGroupService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        // private readonly IEmployeeWorkGroupRepository _employeeWorkGroupRepository;
        private readonly IMisUpdateService _misUpdateService;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public EmployeeWorkGroupService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IMisUpdateService misUpdateService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            //  _employeeWorkGroupRepository = employeeWorkGroupRepository;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _misUpdateService= misUpdateService;
        }
        public FilterResult<EmployeeWorkGroupModel> GetEmployeeWorkGroups(EmployeeWorkGroupModel model)
        {
            var employeeWorkGroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupInclouded().AsNoTracking();
            var PersonShiftConcept = employeeWorkGroup.Where(a => a.EmployeeId == model.EmployeeId).Select(a => new EmployeeWorkGroupModel()
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                FromDate = a.StartDate,
                ToDate = a.EndDate,
                WorkGroupCode = a.WorkGroup.Code,
                WorkGroupId= a.WorkGroupId,
                WorkTimeTitle = a.WorkGroup.WorkTime.Title,
                IsActive = a.IsActive
                //ShiftBoardModels = a.WorkGroup.ShiftBoards.Select(c => new DTO.WorkShift.ShiftBoard.ShiftBoardModel()
                //{
                //    YyyyshamsiTitle = c.WorkCalendar.ShamsiDateV1,
                //    ShiftConceptDetailTitle = c.ShiftConceptDetail.Title,
                //}
                //).ToList()
            }).AsNoTracking();

            var result = _FilterHandler.GetFilterResult<EmployeeWorkGroupModel>(PersonShiftConcept, model, "EmployeeId");


            return new FilterResult<EmployeeWorkGroupModel>()
            {
                Data = _mapper.Map<List<EmployeeWorkGroupModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public async Task EmployeeWorkGroupTransferManagement(ResultEmployeeTransferModel model)
        {
            try
            {
                var employeeWorkGroupActive = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetActiveWorkGroupByEmployeeIdAsync(model.EmployeeId);
                if (employeeWorkGroupActive == null)
                {
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "گروه کاری فعال "));
                }
                if (employeeWorkGroupActive != null)
                {
                    employeeWorkGroupActive.IsActive = false;
                    if (employeeWorkGroupActive.EndDate == null)
                    {
                        employeeWorkGroupActive.EndDate = model.TransferChangeDate.Value.AddDays(-1);
                    }
                    employeeWorkGroupActive.UpdateUser = model.CurrentUserName;
                    employeeWorkGroupActive.UpdateDate = System.DateTime.Now;

                }
                EmployeeWorkGroup newEmployeeWorkGroup = new EmployeeWorkGroup();
                newEmployeeWorkGroup.EmployeeId = model.EmployeeId;
                newEmployeeWorkGroup.IsActive = true;
                newEmployeeWorkGroup.WorkGroupId = model.WorkGroupId.Value;
                newEmployeeWorkGroup.TransferRequestId = model.TransferRequestId;
                newEmployeeWorkGroup.InsertUser = model.CurrentUserName;
                newEmployeeWorkGroup.InsertDate = System.DateTime.Now;
                if (!model.IsTransferReturn)
                {
                    newEmployeeWorkGroup.StartDate = model.TransferChangeDate.Value;
                }
                else
                {
                    newEmployeeWorkGroup.StartDate = employeeWorkGroupActive.EndDate.Value.AddDays(1);
                }
                if (model.IsTemporaryTransfer && !model.IsTransferReturn)
                {
                    newEmployeeWorkGroup.EndDate = model.TransferReturnDate;
                }
                var lastWorkGroup = _kscHrUnitOfWork.WorkGroupRepository.GetById(model.LastWorkGroupId.Value);
                var newWorkGroup = _kscHrUnitOfWork.WorkGroupRepository.GetById(model.WorkGroupId.Value);
                if (lastWorkGroup.WorkTimeId != newWorkGroup.WorkTimeId)
                    newEmployeeWorkGroup.WorkTimeChange = true;
                //
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.EmployeeId);
                employee.WorkGroupId = model.WorkGroupId;
                employee.WorkGroupStartDate = newEmployeeWorkGroup.StartDate;

                //
                await _kscHrUnitOfWork.EmployeeWorkGroupRepository.AddAsync(newEmployeeWorkGroup);
                var workGroup =await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(model.WorkGroupId.Value);
                //MIS بروزرسانی 
                UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
                {
                    Domain = "KSC",
                    FUNCTION = "EMPLOYEE-GROUP",
                    NUM_PRSN = employee.EmployeeNumber,
                    COD_TYP_LIST = workGroup.WorkTime.Code.Trim(),
                    COD_GRP_LIST = workGroup.Code.Trim(),
                    STR_WORK_LIST = newEmployeeWorkGroup.StartDate.ToPersianDate().Replace("/", ""),
                   
                };
                var resultApiMis = _misUpdateService.UpdateTeamAndGroup(updateTeamAndGroupInputModel);
                if (resultApiMis.IsError)
                {
                    throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

    }
}
