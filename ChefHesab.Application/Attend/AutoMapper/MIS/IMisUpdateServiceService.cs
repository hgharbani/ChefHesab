using Ksc.HR.DTO.Emp.BookLet;
using Ksc.HR.DTO.Emp.Family;
using Ksc.HR.DTO.EmployeeBase;
using Ksc.HR.DTO.MIS;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using KSC.Common;
using KSC.MIS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.MIS
{
    public interface IMisUpdateService
    {
        ReturnData<T> ConnectHRToMIS<T>(T modelMis, string subprogram, string paramName);
        KscResult<byte[]> GetTextFileFromMis(string ServerPath, string fileName);
        KscResult SendTextByteFileToMis(string ServerPath, List<File> fileNames);
        KscResult SendTextFileToMis(string ServerPath, List<string> fileNames);
        RPC017 UpdateTeamAndGroup(UpdateTeamAndGroupInputModel inputModel);

        RPC108 UpdatePersonalInfo(MISAddOrEditEmployeeBaseModel inputModel,string operation);
        RPC109 UpdateTextFilePersonalInfo(MISAddOrEditEmployeeBaseModel inputModel);

        RPC109 UpdateTextFileFamilyInfo(MISAddOrEditFamilyModel inputModel);

        RPC109 UpdateTextFileBookletPersonInfo(MISAddOrEditBookletPersonModel inputModel);

        /// <summary>
        /// درصورتی که حساب یا بن کارت حالت فعال باشد اطلاعات MIS
        /// ویرایش شود
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        ReturnData<RPC_ACN_BANK> UpdatePersonalAccount(AddEmployeeAccountBankDto inputModel);


        ReturnData<RPC_EMPLOYMENT> UpdateEmployeeCondition(EmployeeConditionSyncModel inputModel);


        RPC109 MISInfo(MISModel inputModel, string subProgram);
        ReturnData<RPC_EMPLOYMENT> UpdateEmployeeConditionModel(EmployeeConditionModel model);
        ReturnData<T> ConnectHRToMIS<T>(T modelMis, string subprogram, string paramName, string domain);
        KscResult SendTextByteFileToMis(string ServerPath, List<File> fileNames, string domain);
    }
}
