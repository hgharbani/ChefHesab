using KSC.Common;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.OnCall.Employee;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using Ksc.HR.Share.Model;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Duty.Introduction;
using Ksc.HR.DTO.ODSViews.ViewMisEmployee;
using Microsoft.AspNetCore.Mvc;
using Ksc.HR.DTO.Personal.EmployeeWorkGroups;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Report;
using Ksc.HR.DTO.Pay.EmployeeOtherPayment;
using Ksc.HR.DTO.Emp.Family;
using Ksc.HR.DTO.Emp;
using Ksc.HR.DTO.BaseInfo;

namespace Ksc.HR.Appication.Interfaces.OnCall
{
    public interface IEmployeeService
    {


        Task<FilterResult<SearchEmployeeModel>> GetEmployeeByKendoFilter(FilterRequest Filter);
        FilterResult<SearchEmployeeModel> GetUserWindowsEmployeeList(SearchEmployeeModel Filter);
        /// <summary>
        /// فهرست تمامی پرسنل فعال
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        FilterResult<SearchEmployeeModel> GetAllUserWindowsEmployeeList(FilterRequest Filter);
        CALLING_RPC GetPersonalDataMis(InputMisApiModel model);

        EmployeeModel GetOneBySearchModel(SearchEmployeeModel model);

        Employee GetOne(int id);
        EditEmployeeModel GetForEdit(int id);

        EditEmployeeModel GetForEditByWfRequestId(int id);
        string GetSuperiorJobPositionCodeByProcssId(int employeeId, int procssId, string domain);
        EmployeeModel GetPersonnelDetails(int id, string domain);
        Task<EmployeeModel> GetPersonnelEmployeeDetails(int id);

        FilterResult<SearchEmployeeModel> GetUserWindowsEmployeeByKendoFilter(SearchEmployeeModel Filter);

        FilterResult<SearchEmployeeModel> GetUserWindowsEmployeeByKendoFilterSalaryUser(SearchEmployeeModel Filter);

        Task EmployeeTransferManagement(ResultEmployeeTransferModel model);
        /// <summary>
        /// تغییر تیم یا شیفت کاری
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<KscResult> EmployeeTransferByOfficialManagement(EmployeeTeamManagmentModel entity);
        EmployeeModel GetOneBySearchModelWindUser(string currentName);
        Task EmployeeTeamWork_WorkGroupTransferManagement(ResultEmployeeTransferModel model);
        FilterResult<IntroductionModel> GetUsersForIntroductionHR(SearchEmployeeModel Filter);
        /// <summary>
        ///گرفتن اطلاعات پرسنل خانم - فرجه شیردهی 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EmployeeBreastfeddingModel> GetEmployeeBreastFedding(int id);
        /// <summary>
        /// ثبت اطلاعات فرجه شیر دهی پرسنل خانم
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<KscResult> PostEmployeeBreastFedding(EmployeeBreastfeddingModel model);
        FilterResult<SearchEmployeeModel> GetAllPersonalByKendoFilter(SearchEmployeeModel Filter);
        EditEmployeeModel GetEmployeeIncludedWorkCity(int id);

        FilterResult<SearchEmployeeModel> GetCurrentPersonalTeamByKendoFilter(SearchEmployeeModel Filter);
        bool IsUserTmMa(string currentUser);

        bool IsUserTm(string currentUser);
        Task<FilterResult<SearchEmployeeModel>> GetEmployeeByKendoFilterForMission(SearchEmployeeModel Filter, List<string> roles);
        Task<FilterResult<SearchEmployeeModel>> GetStandbyEmployeeForMission(SearchEmployeeModel Filter);
        Task<FilterResult<SearchEmployeeModel>> GetEmployeeByKendoFilterForInterdict(SearchEmployeeModel Filter);



        FilterResult<SearchEmployeeModel> GetLeaderWithHisPersonalByKendoFilter(SearchEmployeeModel Filter);
        EmployeeModel GetOneByEmployeeId(int id);
        //Task<KscResult> AddEmployeeInfo(AddOrEditEmployeeBaseModel model);
       // Task<KscResult> UpdateEmployeeInfo(AddOrEditEmployeeBaseModel model);
        Task<KscResult> AddOrEditEmployeeInfo(AddOrEditEmployeeBaseModel model);
        KscResult AddEmployeeInfo(AddOrEditEmployeeBaseModel model);
        KscResult UpdateEmployeeInfo(AddOrEditEmployeeBaseModel model);
       // Task<IActionResult> GetByKendoFilterForStandbyEmployee(SearchEmployeeModel Filter);
        void Exists(int id, string employeeNumber);
        // Task<FilterResult<AddOrEditEmployeeBaseModel>> GetByKendoFilterForEmployeeBaseInfo(AddOrEditEmployeeBaseModel Filter);//  ,List<string> roles);

        Task<EmployeeConditionModel> GetEmployeeCondition(int id);
        Task<KscResult> PostEmployeeCondition(AddOrEditEmployeeConditionModel model);


        Task<AddOrEditEmployeeBaseModel> GetForEditEmployeeInfo(int id,List<string> roles);
        void ExistsnationalCode(string nationalCode);
        string GetGeneratedEmployeeNum();
        bool GetCheckStepper();
        FilterResult<EmployeeModel> GetByFilterEmployeeInfo(EmployeFilter Filter);



        Task<EmployeeSacrificeDto> GetEmployeeSacrifice(int id);
        Task<KscResult> PostEmployeeSacrifice(EmployeeSacrificeDto model);
        void ExistsnationalCode(string nationalCode, string employeeNumber);
        Task<FilterResult<SearchEmployeeModel>> GetByFilterAsyncForRegistered(SearchEmployeeModel Filter, List<string> roles);

        Task<FilterResult<SearchEmployeeModel>> GetByFilterAsyncForRegisteredByAccess(SearchEmployeeModel Filter, List<string> roles);

        Task<KscResult> SyncEmployeeDataFromMis();
        KscResult UpploadPicToEmployeePic();
        Task<KscResult> UpdateEmployeeWorkShiftGroup(EmployeeWorkGroupModel model);
        Task<KscResult> SyncEmployeeAdressDataFromMis();

        FilterResult<EmployeeModel> GetByFilterEmployeeInfoByAccess(EmployeFilter Filter, List<string> roles);
        List<EmployeeInfoModel> GetListEmployeeInfoByNumber(SearchEmployeeInfo search);
        EmployeeInfoModel GetEmployeeInfoByNumber(string employeeNumber);
        void GetGeneratedEmployeeNum1();
        KscResult<EmployeeOrganizationHouseModel> GetExistEmployeeOrganizationHouse(string employeeNumber);
       Task<EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModel>> GetEmployeeOtherPaymentByFilter(OtherPaymentSearchPanelModel search);


        //===========================هزینه سفر و ازدواج و تولد==================================
        List<EmployeeOtherPaymentModelForTravel> GetPersonalEmployeeInTravel(SearchEmployeeTravelPayModel Filter);
        List<EmployeeOtherPaymentModelForTravel> GetPersonalManagmentInTravel(SearchEmployeeTravelPayModel Filter);
        Task<EmployeeOtherPaymentForMariedAndBirthDayModel> GetEmployeeOtherPaymentForMariedAndBirthDay(OtherPaymentSearchPanelModel search);
        EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModelForTravel> GetEmployeeOtherPaymentTravelManagmentByFilter(SearchEmployeeTravelPayModel search);
        EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModelForTravel> GetEmployeeOtherPaymentTravelEmployeeByFilter(SearchEmployeeTravelPayModel search);
        Task<KscResult> PostEmployeeWorkType(EmployeeWorkDayTypeModel model);
       Task<List<EmployeeDataForOtherSystemModel>> GetListEmployeeForOtherSystem();
        Task<KscResult> SyncEmployeeEfficiencyJobPositionDataFromMis(ConvertPageModel filter);
        Task<KscResult> ConvertEmployeePicture();
        Task DeActiveWindowsUser(string employeeNumber, string domain);
        Task<EmployeePictureModel> GetImageByEmployeeId(int employeeId);
        EmployeeModel GetEmployeeByNationalCode(string nationalCode);
        Task<EmployeeCategoryCoefficientDto> GetEmployeeCategoryCoefficient(int id);
        Task<KscResult> PostEmployeeCategoryCoefficient(EmployeeCategoryCoefficientDto model);
        EmployeeDetailsModel GetEmployeeDetails(int employeeId);
        bool IsActiveNationalCode(string nationalCode);
    }
}


