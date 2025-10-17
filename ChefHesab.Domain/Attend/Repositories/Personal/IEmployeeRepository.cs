using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeRepository : IRepository<Employee, int>
    {
        int? GetEmployeeIdByEmployeeNumberAsync(string employeeNumber);
        IQueryable<Employee> GetEmployees(List<int> ids);
        IQueryable<Employee> GetEmployeeWithActiveFamilyForTravel(DateTime EndDate);
        IQueryable<Employee> GetEmployee();
        IQueryable<Employee> GetEmployeeIncludedTeamwork(List<int> ids);
        IQueryable<Employee> GetIncludedEmployee();
        IQueryable<Employee> GetEmployeeByRelatedMonthTimeSheet();

        IQueryable<Employee> GetIncludedEmployeeById(int id);

        //CALLING_RPC GetPersonalDataMis(InputMisApiModel model);
        Employee GetEmployeeByPersonalNum(string Nmp);
        Task<Employee> GetEmployeeByEmployeeId(int id);
        Employee GetEmployeeIncludedTeamWorkByEmployeeId(string employeeNumber);
        Employee GetEmployeeByEmployeeIdNoAsync(int id);
        IQueryable<Employee> GetEmployeesActivAndeHaveTeamAsNotracking();
        IQueryable<Employee> GetEmployeeWorkGroupWorkTimeByRelated();
        Task<Employee> GetEmployeeByIDMonthTimeSheet(int employeeID);
        IQueryable<Employee> GetEmployeeIByTeamCodes(List<string> TeamCods);
        IQueryable<Employee> GetEmployeeIncludedWorkCity();
        IQueryable<Employee> GetEmployees(List<string> EmployeeNumbers);
        IQueryable<Employee> GetLeaderWithHisPersonalByUserName(string currentUserName);
        Employee GetEmployeeIncludedTeamWorkByEmployeeId(int employeeid);
        IQueryable<Employee> GetEmployeeForMissionByAccesslevel(string currentUserName, bool AcessLevel, int dismiisalEmployeeId);
        IQueryable<Employee> GetForStandbyEmployee();
        Employee GetEmployeeCondition(int id);
        IQueryable<Employee> GetEmployeeIncludedCiteis(int id);
        IQueryable<Employee> GetEmployeeByPaymentStatusAccess(List<string> roles);

        IQueryable<Employee> GetByNationalCode(string nationalCode);
        IQueryable<Employee> GetEmployeeWithActiveFamily(DateTime EndDate);
        IQueryable<Employee> GetEmploymentPerson();
        IQueryable<Employee> GetDataEmployeeForTravel(List<string> employeeNumber);
        Task<Employee> GetEmployeeByEmployeeNumberAsync(string employeeNumber);
        Employee GetEmployeeIncludedTeamWork_WorkGroupByEmployeeId(int employeeid);
        IQueryable<Employee> GetDataEmployeeForTravelAsNoTracking(List<string> employeeNumber);
        Task<List<Employee>> GetEmployeeByEmployeeNumbersAsync(List<string> employeeNumber);
        List<Employee> GetEmployeeByEmployeeNumbersAsNoList(List<string> employeeNumber);
        IQueryable<Employee> GetEmployeeSIncludedTeamWork();
        IQueryable<Employee> GetEmploymentPersonCurrent();
        Employee GetEmployeeByEmployeeIdIncludedAbsence(int id);
        void UpdatePaymentStatusForStartMonth();
        IQueryable<Employee> GetEmployeeForTrafficCar();
        IQueryable<Employee> GetEmployeeForManagnet(int yearMonth, List<int> jobpositionIds);
        Employee GetEmployeeByPersonalNumIncluded(string Nmp);
        Employee GetEmployeeByEmployeeIdIncluded(int employeeId);
        string GetEmployeeJobPositionGrade(int employeeId);
        Employee GetEmployeeByEmployeeIdIncludedByDetailed(int employeeId);
        List<Employee> GetEmployeeWorkInJobposition(List<int> jobpositionIds);
        IQueryable<Employee> GetEmployeesActivAndeHaveTeamAsNotracking(List<int> employeesid);
        Employee GetEmployeeDetailsWithJobPositionAndStructureId(string employeeNumber);
        IQueryable<Employee> GetEmployee(List<int> ids);
    }
}
