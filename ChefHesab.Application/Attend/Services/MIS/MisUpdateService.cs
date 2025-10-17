using AutoMapper;
using KSC.MIS.Service;
using KSC.Common.Filters.Contracts;
using System;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.DTO.MIS;
using KSC.Common;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.IO;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.Emp.Family;
using Ksc.HR.DTO.Emp.BookLet;
using Ksc.HR.DTO.EmployeeBase;
using Ksc.HR.Domain.Repositories;
using Ksc.IndustrialAccounting.Shared.Utility;
using Ksc.Accounting.Shared.Tools.Helpers;
using Microsoft.Extensions.Configuration;
using Ksc.HR.DTO.EmployeePromotion.Enumerations;
using Ksc.HR.DTO.Rule.EmployeePromotion;
using NetTopologySuite.Index.HPRtree;
using KSC.Identity.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Ksc.HR.Share.General;

namespace Ksc.HR.Appication.Services.MIS;

public class MisUpdateService : IMisUpdateService
{
    private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IFilterHandler _FilterHandler;
    private readonly JsonSerializerOptions _options;
    private readonly IdentityHttpClinetHeaderHandle handel;



    public MisUpdateService(
         IKscHrUnitOfWork kscHrUnitOfWork,
        IMapper mapper,
        IFilterHandler FilterHandler,

        IConfiguration config
        , IdentityHttpClinetHeaderHandle handle

        )
    {
        _kscHrUnitOfWork = kscHrUnitOfWork;
        _mapper = mapper;
        _FilterHandler = FilterHandler;
        _config = config;
        _options = new JsonSerializerOptions
        {
            IgnoreNullValues = false,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        handel = handle;
    }
    /// <summary>
    ///می باشد  MIS این متد برای بروزرسانی اطلاعات تیم و گروه کاربر و یا ایجاد اطلاعات تیم در سیستم پرسنلی 
    /// </summary>
    /// <returns></returns>
    public RPC017 UpdateTeamAndGroup(UpdateTeamAndGroupInputModel inputModel)
    {
        //bool loadEnviroment = true;
        Enviroment env;
        //if (inputModel.FUNCTION == "EMPLOYEE-TEAM" || inputModel.FUNCTION == "EMPLOYEE-GROUP")
        //{
        //    loadEnviroment = false;

        //}
        try
        {
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //                                    env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }

            ParamApi<RPC017> miscall = new ParamApi<RPC017>(
                 Enviroment: env,
                 DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML027",
                 ParamName: "RPC017",
                 Pheader: new PHeader()
                 {

                 },
                     InputModel: new RPC017
                     {
                         EMPLOYEE_TEAM = new EMPLOYEE_TEAM()
                         {
                             NUM_PRSN = inputModel.NUM_PRSN,
                             NUM_TEAM_EMPL = inputModel.NUM_TEAM_EMPL,
                             NUM_TEAM_LIST = inputModel.NUM_TEAM_LIST,
                             DAT_STR_TEAM = inputModel.DAT_STR_TEAM,
                             STR_TEAM_LIST = inputModel.STR_TEAM_LIST,
                             COD_GRP_LIST = inputModel.COD_GRP_LIST,
                             COD_TYP_LIST = inputModel.COD_TYP_LIST,
                             STR_WORK_LIST = inputModel.STR_WORK_LIST


                         },
                         GROUP_MANAGE = new GROUP_MANAGE()
                         {
                             COD_MNGT = inputModel.COD_MNGT,
                             DES_GRP_EMGRP = inputModel.DES_GRP_EMGRP,
                             FK_CCRRX = inputModel.FK_CCRRX,
                             FLG_GRP_CRUD = inputModel.FLG_GRP_CRUD,
                             FLG_MAN_OPE_EMGRP = inputModel.FLG_MAN_OPE_EMGRP,
                             NUM_GRP_EMGRP = inputModel.NUM_GRP_EMGRP,
                         },
                         TEAM_MANAGE = new TEAM_MANAGE()
                         {
                             COD_OVT = inputModel.COD_OVT,
                             DAT_END = inputModel.DAT_END,
                             DAT_STR = inputModel.DAT_STR,
                             DES_TEAM = inputModel.DES_TEAM,
                             FLG_TEAM_CRUD = inputModel.FLG_TEAM_CRUD,
                             NUM_GRP_EMGRP_TEAM = inputModel.NUM_GRP_EMGRP_TEAM,
                             NUM_TEAM = inputModel.NUM_TEAM,
                         },
                         FUNCTION = inputModel.FUNCTION
                     }
                 );
            var result = miscall.GetResultDevelop<RPC017>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;

            return userDataMIS;
        }
        catch (Exception ex)
        {
            RPC017 e = new RPC017();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            //throw new Exception(ex.Message);
        }
    }
    public ReturnData<T> ConnectHRToMIS<T>(T modelMis, string subprogram, string paramName)
    {
        ReturnData<T> MisResult = new ReturnData<T>();

        //Enviroment env;
        //#if DEBUG
        //        env = Enviroment.Development;
        //#else
        //                                    env = Enviroment.Load;
        //#endif
        var ApplicationMode = _config["ApplicationMode"];
        var env = Enviroment.Development;
        if (ApplicationMode == "load")
        {
            env = Enviroment.Load;
        }
        // var   env = Enviroment.Load;


        ParamApi<T> miscall = new ParamApi<T>(
             Enviroment: env,
             DomainEnum: KSC.MIS.Service.Domain.KSC,
             LibraryName: LibraryName.PER,
             Subprogram: subprogram,
             ParamName: paramName,

             Pheader: new PHeader()
             {

             },
                 InputModel: modelMis
             );
        MisResult = miscall.GetResultDevelop<T>(_config["MisServiceUrl"]);


        return MisResult;

    }
    public ReturnData<T> ConnectHRToMIS<T>(T modelMis, string subprogram, string paramName,string domain="KSC")
    {
        ReturnData<T> MisResult = new ReturnData<T>();

        //Enviroment env;
        //#if DEBUG
        //        env = Enviroment.Development;
        //#else
        //                                    env = Enviroment.Load;
        //#endif
        var ApplicationMode = _config["ApplicationMode"];
        var env = Enviroment.Development;
        if (ApplicationMode == "load")
        {
            env = Enviroment.Load;
        }
        // var   env = Enviroment.Load;


        ParamApi<T> miscall = new ParamApi<T>(
             Enviroment: env,
             DomainEnum: domain,
             LibraryName: LibraryName.PER,
             Subprogram: subprogram,
             ParamName: paramName,

             Pheader: new PHeader()
             {

             },
                 InputModel: modelMis
             );
        MisResult = miscall.GetResultDevelop<T>(_config["MisServiceUrl"]);


        return MisResult;

    }

    public KscResult SendTextFileToMis(string ServerPath, List<string> fileNames)
    {
        var result = new KscResult();
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
                SendFile sendFileModel = new SendFile { application = "PER", filename = item, enviroment = env };
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

                        result.AddError("خطا", "انتقال فایل انجام نشد");
                        return result;
                    }
                }
                else
                {

                    result.AddError("خطا", "ارتباط با سیستم  برقرار نشد");
                    return result;
                }
            }
        }
        return result;
    }

    public KscResult SendTextByteFileToMis(string ServerPath, List<Ksc.HR.DTO.Personal.MonthTimeSheet.File> fileNames)
    {

        var result = new KscResult();
        var uri = $"{ServerPath}";

        foreach (var item in fileNames)
        {
            using (var client = new HttpClient())
            {
                //                string env;
                //#if DEBUG
                //                env = "M";
                //#else
                //                                    env = "L";
                //#endif
                var ApplicationMode = _config["ApplicationMode"];
                var env = "M";//Enviroment.Development;
                if (ApplicationMode == "load")
                {
                    env = "L"; //Enviroment.Load;
                }

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                SendFile sendFileModel = new SendFile
                {
                    application = "PER",
                    filename = item.filename + ".txt",
                    file = item.file,
                    enviroment = env
                };
                var postTask = client.PostAsJsonAsync<SendFile>(uri, sendFileModel);
                postTask.Wait();
                var sss = System.Text.Json.JsonSerializer.Serialize(sendFileModel);
                var content = new StringContent(sss);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var ttt = client.PostAsync(uri, content).GetAwaiter().GetResult();
                var stringResult = ttt.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var modelObj = JsonConvert.DeserializeObject<ReturnData<string>>(stringResult);
                if (modelObj.IsSuccess == true)
                {

                }
                else
                {
                    result.AddError("خطا", "انتقال فایل انجام نشد");
                    return result;

                }

            }
        }
        return result;
    }

    public KscResult SendTextByteFileToMis(string ServerPath, List<Ksc.HR.DTO.Personal.MonthTimeSheet.File> fileNames, string domain)
    {

        var result = new KscResult();
        var uri = $"{ServerPath}";

        foreach (var item in fileNames)
        {
            using (var client = new HttpClient())
            {
                //                string env;
                //#if DEBUG
                //                env = "M";
                //#else
                //                                    env = "L";
                //#endif
                var ApplicationMode = _config["ApplicationMode"];
                var env = "M";//Enviroment.Development;
                if (ApplicationMode == "load")
                {
                    env = "L"; //Enviroment.Load; 
                }
                if (domain != "KSC")// برای شرکتهای هلدینگ
                {
                    env = domain;
                }
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                SendFile sendFileModel = new SendFile
                {
                    application = "PER",
                    filename = item.filename + ".txt",
                    file = item.file,
                    enviroment = env
                };
                var postTask = client.PostAsJsonAsync<SendFile>(uri, sendFileModel);
                postTask.Wait();
                var sss = System.Text.Json.JsonSerializer.Serialize(sendFileModel);
                var content = new StringContent(sss);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var ttt = client.PostAsync(uri, content).GetAwaiter().GetResult();
                var stringResult = ttt.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var modelObj = JsonConvert.DeserializeObject<ReturnData<string>>(stringResult);
                if (modelObj.IsSuccess == true)
                {

                }
                else
                {
                    result.AddError("خطا", "انتقال فایل انجام نشد");
                    return result;

                }

            }
        }
        return result;
    }





    //-------------------------for personal info-------------------------------
    #region //ثبت و ویرایش اطلاعات پرسنلی در MIS
    public RPC108 UpdatePersonalInfo(MISAddOrEditEmployeeBaseModel inputModel, string operation)
    {
        bool loadEnviroment = true;
        Enviroment env;
        if (inputModel.FUNCTION == "INSERT-EMPLOYEE")
        {
            loadEnviroment = false;

        }
        try
        {
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //                                    env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC108> miscall = new ParamApi<RPC108>(
                 Enviroment: env,
                 DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML108",
                 ParamName: "RPC108",
                 Pheader: new PHeader()
                 {
                     pFunction = inputModel.FUNCTION
                 },
                     InputModel: new RPC108
                     {
                         OPERATION = operation,

                         // FUNCTION = "INSERT-EMPLOYEE",
                         NUM_PRSN_EMPL = inputModel.NUM_PRSN_EMPL,// پرسنلی
                         COD_STA_PYM_EMPL = inputModel.COD_STA_PYM_EMPL.ToString(),// وضعیت اشتغال
                         DAT_STA_PYM_EMPL = inputModel.DAT_STA_PYM_EMPL,// تاریخ وضعیت اشتغال
                         NAM_PER_EMPL = inputModel.NAM_PER_EMPL,  // نام
                         NAM_FAM_EMPL = inputModel.NAM_FAM_EMPL,//نام خانوادگی
                         NAM_PER_LST_EMPL = inputModel.NAM_PER_LST_EMPL,// نام قبلی فرد
                         NAM_FAM_LST_EMPL = inputModel.NAM_FAM_LST_EMPL,//    نام خانوادگی قبلی فرد
                         COD_SEX_EMPL = inputModel.COD_SEX_EMPL.ToString(),// جنسیت
                         NAM_FTHR_EMPL = inputModel.NAM_FTHR_EMPL,// نام پدر
                         NUM_NNAL_EMPL = inputModel.NUM_NNAL_EMPL.Trim(),// کد ملی
                         NUM_CRT_EMPL = inputModel.NUM_CRT_EMPL, //شماره// شناسنامه
                         COD_NNLTY_EMPL = inputModel.COD_NNLTY_EMPL.ToString(),// ملیت
                         COD_RLGN_EMPL = inputModel.COD_RLGN_EMPL.ToString(),   //مذهب
                         DAT_BRT_EMPL = inputModel.DAT_BRT_EMPL,// تاریخ تولد شمسی
                         COD_CITY_BORN = inputModel.COD_CITY_BORN,// شهر محل تولد
                         DAT_ISSU_CRT_EMPL = inputModel.DAT_ISSU_CRT_EMPL,// تاریخ صدور شناسنامه(شمسی)
                         COD_CITY_DOC = inputModel.COD_CITY_DOC,// شهر صدور شناسنامه
                         COD_MLTRY_EMPL = inputModel.COD_MLTRY_EMPL.ToString(), //وضعیت نظام وظیفه
                         DAT_STR_MLTRY_EMPL = inputModel.DAT_STR_MLTRY_EMPL,  // تاریخ شروع سربازی(شمسی)
                         DAT_END_MLTRY_EMPL = inputModel.DAT_END_MLTRY_EMPL, //تاریخ پایان سربازی(شمسی)
                         COD_MRD_EMPL = inputModel.COD_MRD_EMPL.ToString(),         // وضعیت تاهل
                         DAT_MRD_EMPL = inputModel.DAT_MRD_EMPL, //تاریخ ازدواج(شمسی)
                         NUM_PNS_EMPL = inputModel.NUM_PNS_EMPL, //شماره بیمه
                         FK_TBPNS = inputModel.FK_TBPNS.ToString(),   // نوع بیمه
                         COD_INSU_COMP_EMPL = inputModel.COD_INSU_COMP_EMPL.ToString(), //شرکت بیمه گذار
                         NUM_MOBIL_EMPL = inputModel.NUM_MOBIL_EMPL,// تلفن همراه
                         COD_BLD_TYP_EMPL = inputModel.COD_BLD_TYP_EMPL.ToString(),// گروه خون
                         COD_ZIP_EMPL = inputModel.COD_ZIP_EMPL, //کد پستی
                         COD_CITY_RSDNC = inputModel.COD_CITY_RSDNC, //شهر محل سکونت
                         DES_ADR_EMPL = inputModel.DES_ADR_EMPL,// آدرس محل سکونت
                         USER_NAME = inputModel.USER_NAME, //
                     }

                 );
            var result = miscall.GetResultDevelop<RPC108>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;

            return userDataMIS;
        }
        catch (Exception ex)
        {
            RPC108 e = new RPC108();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            throw new Exception(ex.Message);
        }
    }
    #endregion
    //---------------------------------------------------------------------

    public RPC109 UpdateTextFilePersonalInfo(MISAddOrEditEmployeeBaseModel inputModel)
    {
        bool loadEnviroment = true;
        Enviroment env;
        //if (inputModel.FUNCTION == "INSERT-EMPLOYEE")
        //{
        //    loadEnviroment = false;

        //}
        try
        {
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //            env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC109> miscall = new ParamApi<RPC109>(
                 Enviroment: env,
                 DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML109",
                 ParamName: "RPC109",
                 Pheader: new PHeader()
                 {
                     pFunction = inputModel.FUNCTION
                 },
                     InputModel: new RPC109
                     {
                         //OPERATION = operation,

                     }

                 );
            var result = miscall.GetResultDevelop<RPC109>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;

            return userDataMIS;
        }
        catch (Exception ex)
        {
            RPC109 e = new RPC109();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            throw new Exception(ex.Message);
        }
    }

    public KscResult<byte[]> GetTextFileFromMis(string ServerPath, string fileName)
    {
        var result = new KscResult<byte[]>();
        var uri = $"{ServerPath}";
        try
        {
            var activeDirectoryLdapKind = _config["AuthenticationOption:ActiveDirectoryLdapKind"];
            string domain = Utility.GetDomainNameByLdapKind(int.Parse(activeDirectoryLdapKind));
            using (var client = new HttpClient())
            {
                string env;
                var ApplicationMode = _config["ApplicationMode"];
                if (ApplicationMode == "dev")
                {
                    env = "M";
                }
                else
                {
                    env = "L";
                }
                if (domain != "KSC")
                {
                    env = domain;
                }
                Fileinfo getFileModel = new Fileinfo { application = "PER", filename = fileName, enviroment = env };
                StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(getFileModel, _options), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Clear();
                //var handel = ServiceProvider.GetService<IdentityHttpClinetHeaderHandle>();
                handel.Handle(client.DefaultRequestHeaders);



                HttpResponseMessage httpResponseMessage =  client.PostAsync(uri, content).GetAwaiter().GetResult();
                //postTask.Wait();
                //var resultApi = postTask.Result;
                if (httpResponseMessage != null)
                {
                    var jsonString =  httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var modelObj = JsonConvert.DeserializeObject<ReturnData<byte[]>>(jsonString);
                    if (modelObj.IsSuccess == true)
                    {
                        result.Data = (Byte[])modelObj.Data;
                        //var str = new FileStream() {Name = "test"};

                    }
                    else
                    {

                        result.AddError("خطا", "انتقال فایل انجام نشد");
                        return result;
                    }
                }
                else
                {

                    result.AddError("خطا", "ارتباط با سیستم  برقرار نشد");
                    return result;
                }
            }

            return result;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }


    public RPC109 MISInfo(MISModel inputModel, string subProgram)
    {
        bool loadEnviroment = true;
        Enviroment env;
        try
        {
            //#if DEBUG
            //            env = Enviroment.Load; //env = Enviroment.Development; // 
            //#else
            //            env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC109> miscall = new ParamApi<RPC109>(
                 Enviroment: env,
                 //DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: subProgram,
                 ParamName: "RPC109",
                 Pheader: new PHeader()
                 {
                     pFunction = inputModel.FUNCTION
                 },
                     InputModel: new RPC109
                     {
                         //OPERATION = operation,

                     }

                 );
            var result = miscall.GetResultDevelop<RPC109>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;

            return userDataMIS;
        }
        catch (Exception ex)
        {
            RPC109 e = new RPC109();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            throw new Exception(ex.Message);
        }
    }


    public RPC109 UpdateTextFileFamilyInfo(MISAddOrEditFamilyModel inputModel)
    {
        bool loadEnviroment = true;
        Enviroment env;
        try
        {
            ////#if DEBUG
            ////            env = Enviroment.Development;
            ////#else
            //env = Enviroment.Load;
            ////#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC109> miscall = new ParamApi<RPC109>(
                 Enviroment: env,
                 DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML109",
                 ParamName: "RPC109",
                 Pheader: new PHeader()
                 {
                     pFunction = inputModel.FUNCTION
                 },
                     InputModel: new RPC109
                     {
                         //OPERATION = operation,

                     }

                 );
            var result = miscall.GetResultDevelop<RPC109>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;

            return userDataMIS;
        }
        catch (Exception ex)
        {
            RPC109 e = new RPC109();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            throw new Exception(ex.Message);
        }
    }

    public RPC109 UpdateTextFileBookletPersonInfo(MISAddOrEditBookletPersonModel inputModel)
    {
        bool loadEnviroment = true;
        Enviroment env;
        try
        {
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC109> miscall = new ParamApi<RPC109>(
                 Enviroment: env,
                 DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML109",
                 ParamName: "RPC109",
                 Pheader: new PHeader()
                 {
                     pFunction = inputModel.FUNCTION
                 },
                     InputModel: new RPC109
                     {
                         //OPERATION = operation,

                     }

                 );
            var result = miscall.GetResultDevelop<RPC109>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;

            return userDataMIS;
        }
        catch (Exception ex)
        {
            RPC109 e = new RPC109();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            throw new Exception(ex.Message);
        }
    }


    #region //ثبت و ویرایش اطلاعات حساب در MIS
    /// <summary>
    /// درصورتی که حساب یا بن کارت حالت فعال باشد اطلاعات MIS
    /// ویرایش شود
    /// </summary>
    /// <param name="inputModel"></param>
    /// <returns></returns>
    public ReturnData<RPC_ACN_BANK> UpdatePersonalAccount(AddEmployeeAccountBankDto inputModel)
    {
        Enviroment env;
        try
        {
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //                                    env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC_ACN_BANK> miscall = new ParamApi<RPC_ACN_BANK>(
                 Enviroment: env,
                 //DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML070",
                 ParamName: "RPC_ACN_BANK",
                 Pheader: new PHeader()
                 {

                 },
                     InputModel: new RPC_ACN_BANK
                     {
                         COD_BANK = inputModel.BankId.ToString(),
                         FLG_ACT = inputModel.IsActive == false ? 0 : 1,
                         NUM_ACN = inputModel.AccountNumber,
                         NUM_CARD = inputModel.CardNumber,
                         NUM_NNAL = inputModel.NationalCode,
                         TYP_ACN = inputModel.AccountBankTypeId.ToString(),
                         NUM_PRSN = inputModel.EmployeeNumber,

                     }

                 );
            var result = miscall.GetResultDevelop<RPC_ACN_BANK>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }

            return result;

            //var userDataMIS = result.Data;

            //return userDataMIS;
        }
        catch (Exception ex)
        {
            ReturnData<RPC_ACN_BANK> e = new ReturnData<RPC_ACN_BANK>();
            e.AddError(ex.Message);

            return e;
        }
    }
    #endregion


    #region اطلاعات پرسنلی




    public ReturnData<RPC_EMPLOYMENT> UpdateEmployeeConditionModel(EmployeeConditionModel model)
    {
        var inputModel = FillEmployeeConditionSyncModel(model);

        Enviroment env;
        try
        {
            var activeDirectoryLdapKind = _config["AuthenticationOption:ActiveDirectoryLdapKind"];

            model.DomainName = Utility.GetDomainNameByLdapKind(int.Parse(activeDirectoryLdapKind));
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //                                        env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC_EMPLOYMENT> miscall = new ParamApi<RPC_EMPLOYMENT>(
                 Enviroment: env,
                 DomainEnum: model.DomainName,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML080",
                 ParamName: "RPC_EMPLOYMENT",
                  Pheader: new PHeader()
                  {
                      pFunction = inputModel.P_function
                  },
                     InputModel: new RPC_EMPLOYMENT
                     {
                         NUM_PRSN = inputModel.EmployeeNumber.ToString(),
                         COD_STA_PYM = inputModel.PeymentStatusId.ToString(),
                         COD_CLASS = inputModel.EmploymentStatusId.ToString(),
                         COD_EXMP_ENEX = inputModel.EntryExitTypeId.ToString(),
                         //COD_EXPRT_LEV = inputModel.StudyFieldId.ToString(),
                         COD_EXPRT_LEV = inputModel.StudyFieldCode.ToString(),

                         COD_ISAR = inputModel.IsarStatusId.ToString(),
                         COD_LEV_EDUC = inputModel.EducationId.ToString(),
                         COD_OVT_VAC = inputModel.SacrificeOptionSettingId.ToString(),
                         //COD_TYP_WRK = inputModel.WorkTimeId.ToString(),
                         //COD_WRK_GRP = inputModel.WorkGroupCode.ToString(),
                         DAT_EMPLT = inputModel.EmploymentDate.ToString(),
                         //DAT_END_CNTRC = inputModel.ContractEndDate.ToString(),
                         //DAT_JOB_POS = inputModel.JobPositionStartDate.ToString(),
                         //DAT_STR_CNTRC = inputModel.ContractStartDate.ToString(),
                         DAT_STR_EDUC = inputModel.EducationDate.ToString(),
                         DAT_STR_SAV = inputModel.SavingTypeDate.ToString(),
                         //DAT_STR_TEAM = inputModel.TeamStartDate.ToString(),
                         FK_CITY_WORK = inputModel.WorkCityCodeMIS != null ? inputModel.WorkCityCodeMIS.ToString() : "",
                         FK_EMPLT = inputModel.EmploymentTypeId.ToString(),
                         //FK_JPOS = inputModel.JobPositionCode,
                         FK_SAVTB = inputModel.SavingTypeId.ToString(),
                         FLG_FLOAT_ATABI = inputModel.HasFloatTime.ToString(),
                         //NUM_TEAM = inputModel.TeamWorkCode.ToString(),
                         PCN_ISAR = inputModel.SacrificePercentage.ToString(),
                     }

                 );
            var result = miscall.GetResultDevelop<RPC_EMPLOYMENT>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }

            return result;
        }
        catch (Exception ex)
        {
            ReturnData<RPC_EMPLOYMENT> e = new ReturnData<RPC_EMPLOYMENT>();
            e.AddError(ex.Message);

            return e;
        }
    }









    /// <summary>
    /// بروزرسانی اطلاعات پرسنلی در MIS
    /// 
    /// </summary>
    /// <param name="inputModel"></param>
    /// <returns></returns>
    /// 



    public ReturnData<RPC_EMPLOYMENT> UpdateEmployeeCondition(EmployeeConditionSyncModel inputModel)
    {

        Enviroment env;
        try
        {
            //#if DEBUG
            //            env = Enviroment.Development;
            //#else
            //                                        env = Enviroment.Load;
            //#endif
            var ApplicationMode = _config["ApplicationMode"];
            env = Enviroment.Development;
            if (ApplicationMode == "load")
            {
                env = Enviroment.Load;
            }
            ParamApi<RPC_EMPLOYMENT> miscall = new ParamApi<RPC_EMPLOYMENT>(
                 Enviroment: env,
                 //DomainEnum: inputModel.Domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML080",
                 ParamName: "RPC_EMPLOYMENT",
                  Pheader: new PHeader()
                  {
                      pFunction = inputModel.P_function
                  },
                     InputModel: new RPC_EMPLOYMENT
                     {
                         NUM_PRSN = inputModel.EmployeeNumber.ToString(),
                         COD_STA_PYM = inputModel.PeymentStatusId.ToString(),
                         COD_CLASS = inputModel.EmploymentStatusId.ToString(),
                         COD_EXMP_ENEX = inputModel.EntryExitTypeId.ToString(),
                         COD_EXPRT_LEV = inputModel.StudyFieldCode.ToString(),
                         COD_ISAR = inputModel.IsarStatusId.ToString(),
                         COD_LEV_EDUC = inputModel.EducationId.ToString(),
                         COD_OVT_VAC = inputModel.SacrificeOptionSettingId.ToString(),
                         //COD_TYP_WRK = inputModel.WorkTimeId.ToString(),
                         //COD_WRK_GRP = inputModel.WorkGroupCode.ToString(),
                         DAT_EMPLT = inputModel.EmploymentDate.ToString(),
                         //DAT_END_CNTRC = inputModel.ContractEndDate.ToString(),
                         //DAT_JOB_POS = inputModel.JobPositionStartDate.ToString(),
                         //DAT_STR_CNTRC = inputModel.ContractStartDate.ToString(),
                         DAT_STR_EDUC = inputModel.EducationDate.ToString(),
                         DAT_STR_SAV = inputModel.SavingTypeDate.ToString(),
                         //DAT_STR_TEAM = inputModel.TeamStartDate.ToString(),
                         FK_CITY_WORK = inputModel.WorkCityCodeMIS.ToString(),
                         FK_EMPLT = inputModel.EmploymentTypeId.ToString(),
                         //FK_JPOS = inputModel.JobPositionCode,
                         FK_SAVTB = inputModel.SavingTypeId.ToString(),
                         FLG_FLOAT_ATABI = inputModel.HasFloatTime.ToString(),
                         //NUM_TEAM = inputModel.TeamWorkCode.ToString(),
                         PCN_ISAR = inputModel.SacrificePercentage.ToString(),
                     }

                 );
            var result = miscall.GetResultDevelop<RPC_EMPLOYMENT>(_config["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }

            return result;
        }
        catch (Exception ex)
        {
            ReturnData<RPC_EMPLOYMENT> e = new ReturnData<RPC_EMPLOYMENT>();
            e.AddError(ex.Message);

            return e;
        }
    }

    public EmployeeConditionSyncModel FillEmployeeConditionSyncModel(EmployeeConditionModel model)
    {
        EmployeeConditionSyncModel entity = new EmployeeConditionSyncModel();
        entity.P_function = model.P_function.ToUpper();
        entity.EmployeeNumber = int.Parse(model.EmployeeNumber);

        entity.PeymentStatusId = model.PaymentStatusId;

        if (model.EmploymentTypeId.HasValue)
            entity.EmploymentTypeId = model.EmploymentTypeId.Value;

        if (model.EmploymentStatusId.HasValue)
            entity.EmploymentStatusId = model.EmploymentStatusId.Value;

        if (model.EmploymentDate.HasValue)
            entity.EmploymentDate = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.EmploymentDate.Value).DateKey;

        if (model.ContractStartDate.HasValue)
            entity.ContractStartDate = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.ContractStartDate.Value).DateKey;

        if (model.ContractEndDate.HasValue)
            entity.ContractEndDate = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.ContractEndDate.Value).DateKey;

        if (model.WorkGroupId.HasValue)
        {
            var workGroup = _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupIncludWorkTime(model.WorkGroupId.Value).GetAwaiter().GetResult();
            entity.WorkTimeId = workGroup.WorkTimeId;
            entity.WorkGroupCode = workGroup.Code;
        }

        if (model.StudyFieldId.HasValue)
        {
            var studyField = _kscHrUnitOfWork.StudyFieldRepository.GetStudyFieldById(model.StudyFieldId.Value)
             ;
            entity.StudyFieldCode = int.Parse(studyField.FirstOrDefault().Code);
        }

        if (model.EntryExitTypeId.HasValue)
            entity.EntryExitTypeId = model.EntryExitTypeId.Value;


        entity.HasFloatTime = model.HasFloatTime == true ? 1 : 0;

        if (model.TeamWorkId.HasValue)
            entity.TeamWorkCode = int.Parse(_kscHrUnitOfWork.TeamWorkRepository.GetById(model.TeamWorkId.Value).Code);

        if (model.TeamStartDate.HasValue)
            entity.TeamStartDate = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.TeamStartDate.Value).DateKey;

        if (model.EducationId.HasValue)
            entity.EducationId = model.EducationId.Value;

        //if (model.StudyFieldId.HasValue)
        //    entity.StudyFieldId = model.StudyFieldId.Value;


        if (model.EducationDate.HasValue)
            entity.EducationDate = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.EducationDate.Value).DateKey;

        if (model.WorkCityId.HasValue)
        {
            var cityId = _kscHrUnitOfWork.WorkCityRepository.GetById(model.WorkCityId.Value).CityId;
            entity.WorkCityCodeMIS = _kscHrUnitOfWork.CityRepository.GetById(cityId).TAB_CITY_SP_MIS;
        }
        entity.JobPositionCode = model.JobPositionCode;

        if (model.JobPositionStartDate.HasValue)
            entity.JobPositionStartDate = _kscHrUnitOfWork.WorkCalendarRepository.GetByMiladiDate(model.JobPositionStartDate.Value).DateKey;

        if (model.SavingTypeId.HasValue)
            entity.SavingTypeId = model.SavingTypeId.Value;

        if (model.SavingTypeDate.HasValue)
            entity.SavingTypeDate = model.SavingTypeDate.Value;

        if (model.IsarStatusId.HasValue)
            entity.IsarStatusId = model.IsarStatusId.Value;

        if (model.SacrificeOptionSettingId.HasValue)
            entity.SacrificeOptionSettingId = model.SacrificeOptionSettingId.Value;

        if (model.SacrificePercentage.HasValue)
            entity.SacrificePercentage = model.SacrificePercentage.Value;

        return entity;
    }






    #endregion

}
