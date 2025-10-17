using KSC.Domain;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Share.Model.BusinessTrip;
using Ksc.HR.Share.Model.Mission;

namespace Ksc.HR.Domain.Repositories.BusinessTrip
{
    public interface IMission_RequestRepository : IRepository<Mission_Request, int>
    {
        bool DeleteById(int id, int statusId);
        Mission_Request GetMissionConfirmRequest(int id);
        IQueryable<Mission_Request> GetIncludedWF_Request();
        IQueryable<Mission_Request> GetMission_Requests();
        Task<Mission_Request> GetOne(int id);
        Mission_Request GetMissionByWFRequestId(int wFRequestId);
        IQueryable<Mission_Request> GetMission_RequestsByEmployeeID(int employeeId);
        Mission_Request GetMissionByWFRequestId_EmployeeNumber(int wFRequestId, string employeenumber);
        bool DeleteById(int id, int statusId, bool isValidDelete);
        Mission_Request GetOneSync(int id);
        Mission_Request GetMissionEmployeeReportByWFRequestId(int wFRequestId);
        IQueryable<Mission_Request> GetMissionByListWFRequestId(List<int> wFRequestId);
        int GetMissionDurationCount(Mission_Request model,int employeeId);
        int GetMissionDurationCurrentMonthCount(Mission_Request model, int employeeId);
        Task<Mission_Request> GetMissionByWFRequestIdAsync(int wFRequestId);
        IQueryable<Mission_Request> GetMissionsForSendToMis(int yearmonth,int statusID);
        IQueryable<Mission_Request> GetAllMissionConfirmRequest();
        bool DeleteByIdInExeption(int id);
		MissionCountDataModel GetMissionCountCurrentMonthCount(Mission_Request model, int employeeId);
        IQueryable<Mission_Request> GetMissionsIsPaid();
        void MissionDateManagement(MissionDateManagementInputModel model, DateTime missionEndDate, ref DateTime? notPayStartDate, ref DateTime? notPayEndDate);
        IQueryable<Mission_Request> GetMissionsForStepperSendToMis(int yearmonth, int statusID);
    }
}
