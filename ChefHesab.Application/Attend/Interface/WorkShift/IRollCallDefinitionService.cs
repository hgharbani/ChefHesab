using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IRollCallDefinitionService
    {
        FilterResult<RollCallDefinicationModel> GetRollCallDefinicationByModel(SearchRollCallDefinicationModel Filter);
        /// <summary>
        /// جستجو تمامی رکورد ها
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        List<SearchRollCallDefinicationModel> GetAllRollCallDefinicationRollCallDefinicationByModel(SearchRollCallDefinicationModel Filter);
        /// <summary>
        /// ثبت کد حضور وغیاب
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<KscResult> AddRollCallDefinication(AddRollCallDefinicationModel model);
        /// <summary>
        /// آیا عنوان تکراری می باشد؟
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistsByTitle(int id, string name);

        /// <summary>
        /// آیا عنوان تکراری می باشد؟
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistsByTitle(string name);
        /// <summary>
        /// آیا کد تکراری می باشد؟
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistsByCode(string code);

        /// <summary>
        /// نمایش کد حضور وغیاب 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RollCallDefinition> GetOne(int id);
        /// <summary>
        /// نمایش برای ویرایش کد حضور وغیاب
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AddRollCallDefinicationModel> GetForEdit(int id);

        /// <summary>
        /// ویرایش کد حضور وغیاب
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<KscResult> UpdateRollCallDefinication(AddRollCallDefinicationModel model);
        List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence(DateTime date);
        //List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionByWorkTimeDayTypeEmploymentTypeId(int workTimeId, int workDayTypeId, int? employmentTypeId, DateTime date);
        List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence();
        List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsenceFromInputList(List<RollCallModelForEmployeeAttendAbsenceModel> model, int workTimeId, int workDayTypeId, int? employmentTypeId, DateTime date);
        FilterResult<SearchRollCallDefinicationModel> GetRollCallDefinicationForAnalysAsync(EmployeeAttendAbsenceAnalysisModel Filter);
        List<SearchRollCallDefinicationModel> GeTemporaryRollCallDefinition(RollCallDefinicationFilterRequest Filter);
        List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence(TimeSettingDataModel timeSettingDataModel, int? employmentTypeId);
        List<RollCallModelForEmployeeAttendAbsenceModel> GetRollCallDefinitionForEmployeeAttendAbsence_();
        RollCallForEmployeeAttendAbsenceModel GetRollCallsForEmployeeAttendAbsence(TimeSettingDataModel timeSettingDataModel, int? employmentTypeId, DateTime date);
        FilterResult<RollCallDefinicationModel> GetRollCallDefinicationInCeiling(SearchRollCallDefinicationModel Filter);
    }
}
