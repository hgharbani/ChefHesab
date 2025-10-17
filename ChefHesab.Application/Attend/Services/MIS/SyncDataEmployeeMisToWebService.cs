using AutoMapper;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.PersonalType;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSCCommunicationAPI.Models.Class.ADManagementApiClass;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using KscHelper.Model;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Microsoft.Extensions.Configuration;

namespace Ksc.HR.Appication.Services.MIS
{
    public class SyncDataEmployeeMisToWebService : ISyncDataEmployeeMisToWebService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IConfiguration _config;


        public SyncDataEmployeeMisToWebService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IConfiguration config)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _config = config;
        }
        public async Task<KscResult> InsertFromMisSyncData(InsertAndUpdateFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(model.EmployeeNumber);
                if (employee != null)
                {
                    result.AddError("", "شماره پرسنلی موجود می باشد");
                    return result;
                }
                int employeeNumber = int.Parse(model.EmployeeNumber);
                string nationalCode = model.NationalCode.Fa2En();
                employee = new Domain.Entities.Personal.Employee()
                {
                    EmployeeNumber = employeeNumber.ToString(),
                    Name = model.Name.FixPersianChars(),
                    Family = model.Family.FixPersianChars(),
                    PaymentStatusId = 1,
                    PersonalTypeId = EnumPersonalType.EmploymentPerson.Id,
                    NationalCode = nationalCode.Trim(),
                    Gender = int.Parse(model.Gender),
                    MaritalStatusId = int.Parse(model.MaritalStatusId),
                };
                await _kscHrUnitOfWork.EmployeeRepository.AddAsync(employee);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }
        public async Task<KscResult> UpdateFromMisSyncData(InsertAndUpdateFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var activeDirectoryLdapKind = _config["AuthenticationOption:ActiveDirectoryLdapKind"];
                int employeeNumber = int.Parse(model.EmployeeNumber);
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(employeeNumber.ToString());
                int genderTemp = int.Parse(model.Gender);
                int maritalStatusIdTemp = int.Parse(model.MaritalStatusId);
                if (employee == null)
                {
                    result.AddError("", "شماره پرسنلی موجود نمی باشد");
                    return result;
                }
                if (model.ProgramName == "S6PG301W")
                {
                    if (employee.Name != model.Name.FixPersianChars())
                        employee.Name = model.Name.FixPersianChars().Trim();
                    if (employee.Family != model.Family.FixPersianChars())
                        employee.Family = model.Family.FixPersianChars().Trim();
                    string nationalCode = model.NationalCode.Fa2En();
                    if (employee.NationalCode != nationalCode.Trim())
                        employee.NationalCode = nationalCode.Trim();
                    if (employee.Gender != genderTemp)
                        employee.Gender = genderTemp;
                    if (employee.MaritalStatusId != maritalStatusIdTemp)
                        employee.MaritalStatusId = maritalStatusIdTemp;
                }

                if (model.ProgramName == "S6PG302W")
                {
                    var dismissalStatusId = int.Parse(model.DismissalStatusCode);
                    if (employee.DismissalStatusId != dismissalStatusId && dismissalStatusId != 0)
                        employee.DismissalStatusId = dismissalStatusId;
                    int dismissalDateTemp = int.Parse(model.DismissalDate);
                    if (dismissalDateTemp != 0)
                    {
                        var dismissalDate = dismissalDateTemp.ToString().Fa2En().ToGorgianDate();
                        if (employee.DismissalDate != dismissalDate)
                        {
                            employee.DismissalDate = dismissalDate;
                            var teamWorkId = _kscHrUnitOfWork.TeamWorkRepository.FirstOrDefault(x => x.Code == model.TeamWorkCode).Id;
                            if (teamWorkId != 0)
                            {
                                var teamWorkEnd = _kscHrUnitOfWork.EmployeeTeamWorkRepository.FirstOrDefault(x => x.EmployeeId == employee.Id && x.TeamEndDate == null && x.TeamWorkId == teamWorkId);
                                teamWorkEnd.TeamEndDate = dismissalDate.Value.AddDays(-1);
                                teamWorkEnd.IsActive = false;
                                teamWorkEnd.UpdateDate = DateTime.Now;
                                teamWorkEnd.UpdateUser = model.UserName;
                                _kscHrUnitOfWork.EmployeeTeamWorkRepository.Update(teamWorkEnd);
                            }
                        }
                    }
                    int employmentDateTemp = int.Parse(model.EmploymentDate);
                    if (employmentDateTemp != 0)
                    {
                        var employmentDate = employmentDateTemp.ToString().Fa2En().ToGorgianDate();
                        if (employee.EmploymentDate != employmentDate)
                            employee.EmploymentDate = employmentDate;
                    }
                    int paymentStatusCode = int.Parse(model.PaymentStatusCode);
                    var paymentStatusId = _kscHrUnitOfWork.PaymentStatusRepository.FirstOrDefault(x => x.Code == paymentStatusCode.ToString()).Id;
                    if (employee.PaymentStatusId != paymentStatusId)
                        employee.PaymentStatusId = paymentStatusId;
                    int employmentTypeCode = int.Parse(model.EmploymentTypeCode);
                    if (employee.EmploymentTypeId != employmentTypeCode)
                        employee.EmploymentTypeId = employmentTypeCode;
                    model.CityWork = model.CityWork.Trim();
                    //var workCityCode = int.Parse(model.WorkCityCode);
                    //var provinceCode = int.Parse(model.ProvinceCode);
                    var cityQuery = _kscHrUnitOfWork.CityRepository.FirstOrDefault(x => x.TAB_CITY_SP_MIS == model.CityWork);
                    if (cityQuery == null)
                    {
                        result.AddError("", "شهر محل خدمت وجود ندارد");
                        return result;
                    }
                    var cityId = cityQuery.Id;
                    var workCityQuery = _kscHrUnitOfWork.WorkCityRepository.FirstOrDefault(x => x.CityId == cityId);
                    if (workCityQuery == null)
                    {
                        result.AddError("", "محل خدمت وجود ندارد");
                        return result;
                    }
                    var workCityId = workCityQuery.Id;
                    if (employee.WorkCityId != workCityId && workCityId != 0)
                        employee.WorkCityId = workCityId;
                    int entryExitTypeCode = int.Parse(model.EntryExitTypeCode);
                    var entryExitTypeId = _kscHrUnitOfWork.EntryExitTypeRepository.FirstOrDefault(x => x.Code == entryExitTypeCode.ToString()).Id;
                    if (employee.EntryExitTypeId != entryExitTypeId)
                        employee.EntryExitTypeId = entryExitTypeId;
                    if (paymentStatusCode == 1)
                    {
                        if (employee.WorkGroupStartDate != employee.EmploymentDate)
                            employee.WorkGroupStartDate = employee.EmploymentDate;
                        var teamWorkStartDate = model.TeamWorkStartDate.Fa2En().ToGorgianDate();//Fa2En().ToGregorianDateTime();
                        if (employee.TeamWorkStartDate != teamWorkStartDate)
                            employee.TeamWorkStartDate = teamWorkStartDate;
                        var teamWorkId = _kscHrUnitOfWork.TeamWorkRepository.FirstOrDefault(x => x.Code == model.TeamWorkCode).Id;
                        if (employee.TeamWorkId == null)
                        {
                            employee.TeamWorkId = teamWorkId;
                            var employeeTeamWork = new Domain.Entities.Personal.EmployeeTeamWork()
                            {
                                EmployeeId = employee.Id,
                                IsActive = true,
                                TeamWorkId = employee.TeamWorkId ?? 0,
                                TeamStartDate = employee.TeamWorkStartDate ?? DateTime.Now,
                                InsertDate = DateTime.Now,
                                InsertUser = model.UserName,
                            };
                            _kscHrUnitOfWork.EmployeeTeamWorkRepository.Add(employeeTeamWork);
                        }
                        int workTimeCode = int.Parse(model.WorkTimeCode);
                        var workTimeId = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimeByCode(workTimeCode.ToString()).Id;
                        var workGroupId = _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeIdAndCode(workTimeId, model.WorkGroupCode).Id;
                        if (employee.WorkGroupId == null)
                        {
                            employee.WorkGroupId = workGroupId;
                            var employeeWorkGroup = new Domain.Entities.Personal.EmployeeWorkGroup()
                            {
                                EmployeeId = employee.Id,
                                StartDate = employee.TeamWorkStartDate ?? DateTime.Now,
                                IsActive = true,
                                WorkGroupId = employee.WorkGroupId ?? 0,
                                InsertDate = DateTime.Now,
                                InsertUser = model.UserName,

                            };
                            _kscHrUnitOfWork.EmployeeWorkGroupRepository.Add(employeeWorkGroup);

                        }
                    }

                }

                if (model.ProgramName == "S6PG305W")
                {
                    int paymentStatusCode = int.Parse(model.PaymentStatusCode);
                    var paymentStatusId = _kscHrUnitOfWork.PaymentStatusRepository.FirstOrDefault(x => x.Code == paymentStatusCode.ToString()).Id;
                    if (employee.PaymentStatusId != paymentStatusId)
                        employee.PaymentStatusId = paymentStatusId;
                }
                if (model.ProgramName == "S6PG302W")
                {
                    await DeActiveWindowsUser(model.EmployeeNumber, "KSC");
                }
                _kscHrUnitOfWork.EmployeeRepository.Update(employee);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }
        public async Task DeActiveWindowsUser(string employeeNumber, string domain)
        {
            var activeDirectoryLdapKind = _config["AuthenticationOption:ActiveDirectoryLdapKind"];
            if (activeDirectoryLdapKind != "1")//فولاد نباشد
                return;
            try
            {
                if (string.IsNullOrWhiteSpace(employeeNumber))
                    return;

                var privateKey = await Utility.GetPrivatekeyFromADManagementApi(domain);
                var uri = "https://wapi.ksc.ir/ksccommunicationapi/api/ADManagementApi/ActiveUser";
                var userDefinition = _kscHrUnitOfWork.ViewMisUserDefinitionRepository.GetUserDefinitionByEmployeeNumber(employeeNumber);
                if (userDefinition == null || string.IsNullOrWhiteSpace(userDefinition.WinUser))
                {
                    return;
                }
                //employeeNumber
                string windowsUserName = userDefinition.WinUser.ToLower();
                //
                var userStatus = await Utility.UserStatus(windowsUserName, domain);
                if (userStatus.IsActive != true)
                {
                    return;
                }
                //

                using (var client = new HttpClient())
                {
                    ActiveUserInputDto activeUserInputDto = new ActiveUserInputDto()
                    {
                        Domain = domain,
                        IsActive = false,
                        Token = privateKey,
                        AllowedUser = Utility.AllowedUser,
                        UserName = windowsUserName
                    };

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var postTask = client.PostAsJsonAsync<ActiveUserInputDto>(uri, activeUserInputDto);
                    postTask.Wait();
                    var resultApi = postTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        var returnValue = resultApi.Content.ReadAsStringAsync();
                        var modelObj = JsonConvert.DeserializeObject<ReturnData<ActiveUserOutputDto>>(returnValue.Result);
                        if (modelObj == null)
                        {
                            throw new Exception("1-غیر فعالسازی کاربر ویندوز با خطا مواجه شد");
                        }
                        if (modelObj.IsSuccess == true)
                        {
                            if (modelObj.Data.IsActive)
                            {
                                throw new Exception("2-غیر فعالسازی کاربر ویندوز با خطا مواجه شد");
                            }
                        }
                        else
                        {

                            throw new Exception("3-غیر فعالسازی کاربر ویندوز با خطا مواجه شد");
                        }
                    }
                    else
                    {
                        throw new Exception("4-غیر فعالسازی کاربر ویندوز با خطا مواجه شد");

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "**");
            }
        }

    }
}
