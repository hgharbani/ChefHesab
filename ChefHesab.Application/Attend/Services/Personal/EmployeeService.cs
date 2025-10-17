using AutoMapper;
using DNTPersianUtils.Core;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.Appication.Interfaces.OnCall;
using Ksc.HR.Appication.Interfaces.Personal;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.ScheduledLoger;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.Duty.Introduction;
using Ksc.HR.DTO.Emp;
using Ksc.HR.DTO.Emp.EmployeeVacationManagement;
using Ksc.HR.DTO.Emp.Family;
using Ksc.HR.DTO.EmployeeBase;
using Ksc.HR.DTO.Entities.Enumrations;
using Ksc.HR.DTO.MIS;
using Ksc.HR.DTO.OnCall.Employee;
using Ksc.HR.DTO.Pay.EmployeeOtherPayment;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeWorkGroups;
using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Report;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using Ksc.HR.DTO.WorkShift;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Resources.Personal;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.EmployeeBase;
using Ksc.HR.Share.Model.OtherPaymentStatus;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.Model.ShiftConcept;
using Ksc.HR.Share.Model.SystemSequenceControl;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.HR.Share.Model.Vacation;
/////using Ksc.IndustrialAccounting.Shared.Utility;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using KSC.DMS.Interfaces;
using KSCCommunicationAPI.Models.Class.ADManagementApiClass;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using KscHelper.Model;
using Ksc.Hr.DTO.CategoryCoefficient;
using Ksc.HR.Share.Model.JobPositionStatus;
using Ksc.HR.DTO.BaseInfo;

namespace Ksc.HR.Appication.Services.Personal
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        //private readonly IFilterHandler _filterHandler;
        private readonly IFilterHandler _FilterHandler;
        private readonly IEmployeeWorkGroupService _employeeWorkGroupService;
        private readonly IEmployeeTeamWorkService _employeeTeamWorkService;
        private readonly IMisUpdateService _misUpdateService;
        private readonly IDataPersonalForOtherSystemService _dataPersonalForOtherSystemService;
        private readonly IAttachmentService _attachmentService;
        private readonly IFileSystemStorageService _fileSystemStorageService;




        public EmployeeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IEmployeeWorkGroupService employeeWorkGroupService
            , IEmployeeTeamWorkService employeeTeamWorkService
            , IMisUpdateService misUpdateService
            , IDataPersonalForOtherSystemService dataPersonalForOtherSystemService
            , IAttachmentService attachmentService
, IFileSystemStorageService fileSystemStorageService

            )
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            //_filterHandler = FilterHandler;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _employeeWorkGroupService = employeeWorkGroupService;
            _employeeTeamWorkService = employeeTeamWorkService;
            _misUpdateService = misUpdateService;
            _dataPersonalForOtherSystemService = dataPersonalForOtherSystemService;
            _attachmentService = attachmentService;
            _fileSystemStorageService = fileSystemStorageService;
        }


        //public async Task<FilterResult<AddOrEditEmployeeBaseModel>> GetByKendoFilterForEmployeeBaseInfo(FilterRequest Filter)
        //{
        //    try
        //    {


        //        var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployee().AsNoTracking();
        //        FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
        //        var modelResult = new FilterResult<AddOrEditEmployeeBaseModel>
        //        {
        //            Data = _mapper.Map<List<AddOrEditEmployeeBaseModel>>(result.Data),
        //            Total = result.Total
        //        };
        //        return modelResult;

        //    }
        //    catch (Exception)
        //    {
        //        return new FilterResult<AddOrEditEmployeeBaseModel>();

        //    }
        //}




        //ثبت و get اطلاعات پرسنلی
        #region



        //  برای ثبت اطلاعات پرسنلی MIS بروزرسانی 
        public void ConnectMIS(AddOrEditEmployeeBaseModel model, string operation)
        {
            var employeePaymentStatus = _kscHrUnitOfWork.EmployeePaymentStatusRepository.FirstOrDefault(x => x.PaymentStatusId == model.PaymentStatusId);

            var systemControlDate = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var yearMonth = systemControlDate.AttendAbsenceItemDate;



            //var homecityCode = _kscHrUnitOfWork.CityRepository.GetAllDataWithCityId(model.HomeCityId.Value)
            //      .Select(x => new { cityCode = x.Code });
            if (true)
            {

            }
            var homeCityQuery = model.HomeCityId.HasValue ? _kscHrUnitOfWork.CityRepository.GetAllDataWithCityId(model.HomeCityId.Value).FirstOrDefault() : new City();
            var birthCityQuery = model.BirthCityId.HasValue ? _kscHrUnitOfWork.CityRepository.GetAllDataWithCityId(model.BirthCityId.Value).FirstOrDefault() : new City();
            var certificateCityQuery = model.CertificateCityId.HasValue ? _kscHrUnitOfWork.CityRepository.GetAllDataWithCityId(model.CertificateCityId.Value).FirstOrDefault() : new City();
            var insuranceListIdQuery = model.InsuranceListId.HasValue ? _kscHrUnitOfWork.InsuranceListRepository.GetAllIncludeInsuranceTypeById(model.InsuranceTypeId.Value).FirstOrDefault() : new InsuranceList();


            MISAddOrEditEmployeeBaseModel MISupdateEmployeeBaseModelModel = new MISAddOrEditEmployeeBaseModel()
            {
                // [ODSDB].[PER].[EMPLOYEE_D04F025]
                Domain = model.DomainName,
                FUNCTION = "INSERT-EMPLOYEE",
                NUM_PRSN_EMPL = model.EmployeeNumber,// پرسنلی
                COD_STA_PYM_EMPL = 1,// وضعیت اشتغال
                //DAT_STA_PYM_EMPL = employeePaymentStatus != null ? employeePaymentStatus.YearMonth.ToString() : "",// تاریخ وضعیت اشتغال
                DAT_STA_PYM_EMPL = yearMonth.ToString(),// تاریخ وضعیت اشتغال

                NAM_PER_EMPL = model.Name,  // نام
                NAM_FAM_EMPL = model.Family,//نام خانوادگی
                NAM_PER_LST_EMPL = !string.IsNullOrEmpty(model.PreiveName) ? model.PreiveName.Trim() : "",// نام قبلی فرد
                NAM_FAM_LST_EMPL = !string.IsNullOrEmpty(model.PreiveFamily) ? model.PreiveFamily.Trim() : "",//    نام خانوادگی قبلی فرد
                COD_SEX_EMPL = model.Gender,// جنسیت
                NAM_FTHR_EMPL = model.FatherName,// نام پدر
                NUM_NNAL_EMPL = model.NationalCode.Trim(),// کد ملی
                NUM_CRT_EMPL = model.CertificateNumber, //شماره// شناسنامه
                COD_NNLTY_EMPL = model.NationalityId.HasValue ? model.NationalityId.Value : 0, // ملیت
                COD_RLGN_EMPL = model.RegionId.HasValue ? model.RegionId.Value : 0,   //مذهب
                DAT_BRT_EMPL = model.BirthDate.HasValue ? model.BirthDate.ToPersianDate().Replace("/", "") : "0",// تاریخ تولد شمسی
                COD_CITY_BORN = birthCityQuery.TAB_CITY_SP_MIS,// شهر محل تولد
                DAT_ISSU_CRT_EMPL = model.CertificateDate.HasValue ? model.CertificateDate.ToPersianDate().Replace("/", "") : "0",// تاریخ صدور شناسنامه(شمسی)
                COD_CITY_DOC = certificateCityQuery.TAB_CITY_SP_MIS,// شهر صدور شناسنامه
                COD_MLTRY_EMPL = model.MilitaryStatusId.HasValue ? model.MilitaryStatusId.Value : 0, //وضعیت نظام وظیفه
                DAT_STR_MLTRY_EMPL = model.MilitaryStartDate.HasValue ? model.MilitaryStartDate.ToPersianDate().Replace("/", "") : "0",  // تاریخ شروع سربازی(شمسی)
                DAT_END_MLTRY_EMPL = model.MilitaryEndDate.HasValue ? model.MilitaryEndDate.ToPersianDate().Replace("/", "") : "0", //تاریخ پایان سربازی(شمسی)
                COD_MRD_EMPL = model.MaritalStatusId.HasValue ? model.MaritalStatusId.Value : 0,         // وضعیت تاهل
                DAT_MRD_EMPL = model.MarriedDate.HasValue ? model.MarriedDate.ToPersianDate().Replace("/", "") : "0", //تاریخ ازدواج(شمسی)
                NUM_PNS_EMPL = !string.IsNullOrEmpty(model.InsuranceNumber) ? model.InsuranceNumber.Trim() : "", //شماره بیمه
                FK_TBPNS = model.InsuranceTypeId.HasValue ? model.InsuranceTypeId.Value : 0,   // نوع بیمه


                //    ,
                //[AgreementRow]
                COD_INSU_COMP_EMPL = !string.IsNullOrEmpty(insuranceListIdQuery.AgreementRow) ? Convert.ToInt32(insuranceListIdQuery.AgreementRow.Trim()) : 0,//شرکت بیمه گذار

                NUM_MOBIL_EMPL = model.PhoneNumber != null ? model.PhoneNumber : "0",// تلفن همراه
                COD_BLD_TYP_EMPL = model.BloodTypeId.HasValue ? model.BloodTypeId.Value : 0,// گروه خون
                COD_ZIP_EMPL = model.HomeZipCode != null ? model.HomeZipCode : "0", //کد پستی
                COD_CITY_RSDNC = !string.IsNullOrEmpty(homeCityQuery.TAB_CITY_SP_MIS) ? homeCityQuery.TAB_CITY_SP_MIS : "0", //شهر محل سکونت
                DES_ADR_EMPL = !string.IsNullOrEmpty(model.HomeAddress) ? model.HomeAddress.Trim() : "",// آدرس محل سکونت
                USER_NAME = model.InsertUser, //

            };
            var resultApiMis = _misUpdateService.UpdatePersonalInfo(MISupdateEmployeeBaseModelModel, operation);
            if (resultApiMis.IsError)
            {
                throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
            }

        }

        //get
        public FilterResult<EmployeeModel> GetByFilterEmployeeInfo(EmployeFilter Filter)
        {

            var data = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet().Include(x => x.Families).AsNoTracking();
            var checkStepper = _kscHrUnitOfWork.Stepper_StatusSystemMonthRepository
                                .CheckYearMonth_SystemControlId(
                                yearMonth: _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate,//140211,
                                systemSequenceControlId: EnumSystemSequenceControl.HRSystem.Id
                                , systemSequenceStatusId: EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id
                                );


            if (!string.IsNullOrEmpty(Filter.EmployeeNumber))
            {
                data = data.Where(a => a.EmployeeNumber.Contains(Filter.EmployeeNumber));
            }

            if (!string.IsNullOrEmpty(Filter.Name))
            {
                data = data.Where(a => a.Name.Contains(Filter.Name));
            }
            if (!string.IsNullOrEmpty(Filter.Family))
            {
                data = data.Where(a => a.Family.Contains(Filter.Family));
            }

            if (!string.IsNullOrEmpty(Filter.NationalCode))
            {
                data = data.Where(a => a.NationalCode.Contains(Filter.NationalCode));
            }
            if (Filter.PeymentStatusIds.Any())
            {
                data = data.Where(a => a.PaymentStatusId.HasValue && Filter.PeymentStatusIds.Contains(a.PaymentStatusId.Value));
            }
            var query = data.Select(x => new EmployeeModel
            {
                Id = x.Id,
                EmployeeNumber = x.EmployeeNumber,//شماره پرسنلی
                PaymentStatusTitle = x.PaymentStatus.Title,//وضعیت استخدام
                Name = x.Name,
                Family = x.Family,
                NationalCode = !string.IsNullOrEmpty(x.NationalCode) ? x.NationalCode.Trim() : "",
                FatherName = !string.IsNullOrEmpty(x.FatherName) ? x.FatherName.Trim() : "",
                PaymentStatusId = x.PaymentStatusId,
                DependentsCount = x.Families.Count(y =>
                                                   y.EndDateDependent == null &&
                                                   y.IsContinuesDependent == true),
                ChildrentCount = x.Families.Count(y =>
                                                  y.EndDateDependent == null &&
                                                  y.IsContinuesDependent == true &&
                                                  (y.DependenceTypeId == 3 || y.DependenceTypeId == 4))
            });

            var result = _FilterHandler.GetFilterResult<EmployeeModel>(query, Filter, "Id");
            return new FilterResult<EmployeeModel>()
            {
                Data = _mapper.Map<List<EmployeeModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public FilterResult<EmployeeModel> GetByFilterEmployeeInfoByAccess(EmployeFilter Filter, List<string> roles)
        {

            //var data = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet().Include(x => x.Families).AsNoTracking();
            var data = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet();
            /////
            //
            var paymentStatusAccess = _kscHrUnitOfWork.PaymentStatusAccessRepository.GetPaymentStatusAccessesByRoles(roles);

            var paymentStatusId = paymentStatusAccess.Select(x => x.PaymentStatusId).Distinct().ToList();
            if (!paymentStatusId.Any(x => x == null)) //ادمین نباشد
            {
                data = data.Where(x => paymentStatusId.Any(p => p == x.PaymentStatusId));
            }
            if (Filter.PersonalTypeIds.Count() > 0)
            {
                var types = Filter.PersonalTypeIds;
                data = data.Where(x => x.PersonalTypeId != null && types.Any(p => p == x.PersonalTypeId.Value));
            }

            data = data.Include(x => x.Families).AsNoTracking();
            //
            //////


            if (!string.IsNullOrEmpty(Filter.EmployeeNumber))
            {
                data = data.Where(a => a.EmployeeNumber.Contains(Filter.EmployeeNumber));
            }

            if (!string.IsNullOrEmpty(Filter.Name))
            {
                data = data.Where(a => a.Name.Contains(Filter.Name));
            }
            if (!string.IsNullOrEmpty(Filter.Family))
            {
                data = data.Where(a => a.Family.Contains(Filter.Family));
            }

            if (!string.IsNullOrEmpty(Filter.NationalCode))
            {
                data = data.Where(a => a.NationalCode.Contains(Filter.NationalCode));
            }
            if (Filter.JabPostionId.HasValue)
            {
                data = data.Where(a => a.JobPositionId == Filter.JabPostionId);
            }

            if (Filter.PeymentStatusIds.Any())
            {
                data = data.Where(a => a.PaymentStatusId.HasValue && Filter.PeymentStatusIds.Contains(a.PaymentStatusId.Value));
            }
            data = data.Include(a => a.JobPosition).AsQueryable().AsNoTracking();
            var query = data.Select(x => new EmployeeModel
            {
                PersonalTypeTitle = (x.PersonalType != null) ? x.PersonalType.Title : "",

                PersonalTypeId = (x.PersonalTypeId != null) ? x.PersonalTypeId.Value : 0,
                Id = x.Id,
                EmployeeNumber = x.EmployeeNumber,//شماره پرسنلی
                PaymentStatusTitle = x.PaymentStatus.Title,//وضعیت استخدام
                Name = x.Name,
                Family = x.Family,
                NationalCode = !string.IsNullOrEmpty(x.NationalCode) ? x.NationalCode.Trim() : "",
                FatherName = !string.IsNullOrEmpty(x.FatherName) ? x.FatherName.Trim() : "",
                PaymentStatusId = x.PaymentStatusId,
                JobPositionCode = x.JobPosition.MisJobPositionCode,
                JobPositionTitle = x.JobPosition.Title,
                DependentsCount = x.Families.Count(y =>
                                                   y.EndDateDependent == null &&
                                                   y.IsContinuesDependent == true),
                ChildrentCount = x.Families.Count(y =>
                                                  y.EndDateDependent == null &&
                                                  y.IsContinuesDependent == true &&
                                                  (y.DependenceTypeId == 3 || y.DependenceTypeId == 4))
            });

            var result = _FilterHandler.GetFilterResult<EmployeeModel>(query, Filter, "Id");
            return new FilterResult<EmployeeModel>()
            {
                Data = _mapper.Map<List<EmployeeModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }


        public void Exists(int id, string employeeNumber)
        {
            if (_kscHrUnitOfWork.EmployeeRepository.Any(x => x.Id != id == true && x.EmployeeNumber == employeeNumber))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Personal.EmployeeResource.EmployeeNumber));
        }
        public void ExistsnationalCode(string nationalCode)
        {
            if (_kscHrUnitOfWork.EmployeeRepository.Any(x => x.NationalCode == nationalCode))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Personal.EmployeeResource.NationalCode));
        }
        public void ExistsnationalCode(string nationalCode, string employeeNumber)
        {
            if (_kscHrUnitOfWork.EmployeeRepository.Any(x => x.NationalCode == nationalCode && x.EmployeeNumber != employeeNumber
            && !(x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id && x.PaymentStatusId == EnumPaymentStatus.DismiisalEmployee.Id)
            ))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Personal.EmployeeResource.NationalCode));
        }
        public async Task<KscResult> AddOrEditEmployeeInfo(AddOrEditEmployeeBaseModel model)
        {
            var fileGuId = "";
            var fileName = "";
            var result = model.IsValid();
            if (!result.Success)
                return result;

            try
            {
                if (model.Id > 0) //edit
                {

                    result = UpdateEmployeeInfo(model);

                }
                else
                {//add
                    result = AddEmployeeInfo(model);

                }

            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }

            return result;

        }
        //Add اطلاعات پرسنلی  
        public KscResult AddEmployeeInfo(AddOrEditEmployeeBaseModel model)
        {
            var result = model.IsValid();


            if (!result.Success)
                return result;




            Ksc.HR.Share.General.Utility.IsValidNationalCode(model.NationalCode);

            Exists(model.Id, model.EmployeeNumber);

            var employeeByMeli = _kscHrUnitOfWork.EmployeeRepository.GetByNationalCode(model.NationalCode).FirstOrDefault();
            //var query= _kscHrUnitOfWork.EmployeeRepository.WhereQueryable(x=>x.Id!=model.Id && x.NationalCode==model.NationalCode);
            //query.Any(x=>!(x.PersonalTypeId == 1 && x.PaymentStatusId == 7))
            //برای پرسنلی ترک خدمتی که تایپ 1 دارند بتوان شماره پرسنلی جدید با کدملی قبلی ثبت کرد

            ExistsnationalCode(model.NationalCode, model.EmployeeNumber);





            var checkStepper = _kscHrUnitOfWork.Stepper_StatusSystemMonthRepository
      .CheckYearMonth_SystemControlId(
      yearMonth: _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate,//140211,
      systemSequenceControlId: EnumSystemSequenceControl.HRSystem.Id
      , systemSequenceStatusId: EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id
      );

            if (checkStepper)
            {
                result.AddError("رکورد نامعتبر", "سیستم پرسنلی بسته است");
                return result;
            }



            if (model.MilitaryStatusId == 0)
            {
                model.MilitaryStatusId = null;
            }
            if (model.BloodTypeId == 0)
            {
                model.BloodTypeId = null;
            }
            if (model.RegionId == 0)
            {
                model.RegionId = null;
            }
            if (model.NationalityId == 0)
            {
                model.NationalityId = null;
            }
            if (model.MaritalStatusId == 0)
            {
                model.MaritalStatusId = null;
            }
            if (model.InsuranceNumber != null && model.InsuranceTypeId == null)
            {
                throw new Exception("نوع بیمه باید مقدار داشته باشد.");

            }

            if (model.InsuranceTypeId != null && model.InsuranceListId == null)
            {
                throw new Exception("شرکت بیمه گزار باید مقدار داشته باشد.");

            }
            if (model.Gender == EnumPersonalForm.men.Id && model.MilitaryStatusId == null)
            {
                throw new Exception("وضعیت نظام وظیفه باید مقدار داشته باشد.");

            }

            if (model.MaritalStatusId == null)
            {
                throw new Exception("وضعیت تاهل باید مقدار داشته باشد.");

            }
            if (model.NationalityId == null)
            {
                throw new Exception("ملیت باید مقدار داشته باشد.");

            }
            if (model.RegionId == null)
            {
                throw new Exception("مذهب باید مقدار داشته باشد.");

            }
            try
            {
                model.IsColesed = checkStepper; //چک استپر
                model.HomeAddress = !string.IsNullOrEmpty(model.HomeAddress) ? model.HomeAddress.Trim() : "";
                var employee = _mapper.Map<Employee>(model);
                employee.PaymentStatusId = EnumPaymentStatus.NewEmployee.Id;//==1
                employee.PersonalTypeId = EnumPersonalType.EmploymentPerson.Id;

                employee.IsGenerated = true;
                employee.EmployeeNumber = model.EmployeeNumber;
                var systemControlDate = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
                var yearMonth = systemControlDate.AttendAbsenceItemDate;
                employee.GuidPicId = model.GuidPicId;







                employee.EmployeePaymentStatus.Add(new EmployeePaymentStatus()
                {
                    PaymentStatusId = EnumPaymentStatus.NewEmployee.Id,
                    YearMonth = yearMonth,
                    InsertDate = DateTime.Now,
                    InsertUser = employee.InsertUser,
                });
                _kscHrUnitOfWork.EmployeeRepository.Add(employee);

                ConnectMIS(model, "I");

                //var operation = "I";
                //if (employee.EmployeeNumber) operation = "U";


                if (_kscHrUnitOfWork.SaveAsync().GetAwaiter().GetResult() > 0)
                {

                    if (!string.IsNullOrEmpty(model.GuidPicId))
                    {
                        var file = _attachmentService.AttachFile(new KSC.DMS.Dto.AddApplicationFileDto
                        {
                            ApplicationName = "HR",
                            EntityName = "Employee",
                            EntityKey = employee.Id.ToString(),
                            Title = model.Name + ' ' + model.Family,
                            Description = "عکس پرسنلی",
                            File = model.FileGuid,
                            FileName = model.FileName,
                            UserName = model.InsertUser,
                        });
                        var findpictureinhr = _kscHrUnitOfWork.EmployeePictureRepository.GetAllQueryable().Where(a => a.EmployeeId == employee.Id).ToList();


                        var pictureFiles = (_attachmentService.GetAttachments("HR", "Employee", new List<string> { employee.Id.ToString() })).GetAwaiter().GetResult();
                        var pictureFile = pictureFiles.FirstOrDefault(a => a.Name.Contains(model.GuidPicId.ToString()));
                        if (pictureFile != null)
                        {
                            var modelEmployeePicture = new EmployeePicture()
                            {
                                Image = pictureFile.Content,
                                PersonalNumber = employee.EmployeeNumber,
                                EmployeeId = employee.Id,
                                InsertDate = DateTime.Now,
                                InsertUser = model.UpdateUser
                            };

                            foreach (var item in findpictureinhr)
                            {
                                item.IsActive = false;
                            }
                            _kscHrUnitOfWork.EmployeePictureRepository.Add(modelEmployeePicture);

                        }
                    }
                }
                model.PaymentStatusId = employee.PaymentStatusId.Value;
                //برای ایجاد رکورد مرخصی با مقادیر خالی برای پرسنل جدید
                #region    

                var vacationBase = _kscHrUnitOfWork.VacationRepository.WhereQueryable(x => x.ShowInManagement == true);
                foreach (var item in vacationBase)
                {
                    var employeeVacationManagement = new EmployeeVacationManagement()
                    {
                        EmployeeId = employee.Id,
                        VacationId = item.Id,// آیدی نوع مرخصی
                        ValueDuration = "+ 0000:0:00",
                        Duration = 0,
                    };

                    _kscHrUnitOfWork.EmployeeVacationManagementRepository.AddAsync(employeeVacationManagement);

                }
                _kscHrUnitOfWork.SaveAsync();


                //await _kscHrUnitOfWork.SaveAsync();

                #endregion

            }//try

            catch (Exception ex)
            {
                result.AddError("خطا", $"خطا در ثبت صورت گرفته است، مجددا ثبت کنید" + "(" + ex.Message + ")");
                return result;

            }
            return result;

        }

        //GetCheckStepper

        public bool GetCheckStepper()
        {

            var checkStepper = _kscHrUnitOfWork.Stepper_StatusSystemMonthRepository
          .CheckYearMonth_SystemControlId(
          yearMonth: _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate,//140211,
          systemSequenceControlId: EnumSystemSequenceControl.HRSystem.Id
          , systemSequenceStatusId: EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id
          );

            return checkStepper;
        }




        //جنریت شماره پرسنلی

        public string GetGeneratedEmployeeNum()
        {
            var systemControlDate = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var yearMonth = systemControlDate.AttendAbsenceItemDate;
            int year = int.Parse(yearMonth.ToString().Substring(0, 4));
            //LastEmployeeNum== ****(1402)+***(seqNum)+*
            var mamoor = _kscHrUnitOfWork.EmployeeRepository.WhereQueryable(x => int.Parse(x.EmployeeNumber.Substring(0, 1)) == 2);

            var LastEmployeeNum = _kscHrUnitOfWork.EmployeeRepository
                 .GetAll().Where(x => x.IsGenerated == true
                 && int.Parse(x.EmployeeNumber.Substring(0, 1)) != 2 //مشاورین با عدد 2 شروع میشود و دستی وارد میشود
                 && x.EmployeeNumber.Length == 8).Max(x => x.EmployeeNumber).ToString();

            var seq = int.Parse(LastEmployeeNum.Substring(4, 3)); //tebghe ghabli
                                                                  // var seq = int.Parse(LastEmployeeNum.Substring(4, 4));//khodam
                                                                  // var seqNum = seq + 1;
            int seqNumTemp = 0;

            //
            int YearlastNUM = int.Parse(LastEmployeeNum.Substring(0, 4));
            if (year > YearlastNUM)
            {
                // var seqNew = "000";
                seqNumTemp = 1;//seqNew + 1;//برای سال جدید دوباره سه رقم وسط از 001 شروع شود
            }
            else
            {
                if (seq != 999)
                    seqNumTemp = seq + 1;
                else
                    seqNumTemp = seq;

            }

            //
            string seqNum = seqNumTemp.ToString();


            if (seqNumTemp < 10)
                seqNum = "00" + seqNumTemp.ToString();
            else
            if (seqNumTemp < 100)
                seqNum = "0" + seqNumTemp.ToString();





            //----------------------formula for X------------------------
            var EndX = 0;
            var b = int.Parse(year.ToString().Substring(1, 1)) * 7;
            var c = int.Parse(year.ToString().Substring(2, 1)) * 6;
            var d = int.Parse(year.ToString().Substring(3, 1)) * 5;
            var sumYear = b + c + d;


            var e = int.Parse(seqNum.ToString().Substring(0, 1)) * 4;
            var z = int.Parse(seqNum.ToString().Substring(1, 1)) * 3;
            var f = int.Parse(seqNum.ToString().Substring(2, 1)) * 2;
            var sumSeqNum = e + z + f;


            var Remain = (sumYear + sumSeqNum) % 11;
            var t = (12 - Remain);
            if (t <= 9 && t >= 0)//
            {
                EndX = t;
            }
            else if (t == 11 || t == 12)
            {
                EndX = 1;
            }

            else if (t == 10)
            {
                EndX = 0;
            }
            //--------------------------------------------------------

            var employeeNum = $"{year}{seqNum}{EndX}";



            return employeeNum;
        }
        //Edit اطلاعات پرسنلی get
        public async Task<AddOrEditEmployeeBaseModel> GetForEditEmployeeInfo(int id, List<string> roles)
        {
            var employeeByPaymentStatusAccess = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPaymentStatusAccess(roles);
            if (employeeByPaymentStatusAccess.Any(x => x.Id == id) == false)
            {
                return null;
            }
            var model = await GetOneRelated(id);
            var employeeData = _mapper.Map<AddOrEditEmployeeBaseModel>(model);
            employeeData.Education = model.EmployeeEducationDegrees.OrderByDescending(x => x.InsertDate).Select(x => x.Education.Title).FirstOrDefault();
            employeeData.EmploymentTypeId = model.EmploymentTypeId;
            employeeData.EmploymentTypeTitle = model.EmploymentTypeId.HasValue || model.EmploymentTypeId > 0 ? model.EmploymentType.Title : "مشخص نشده";
            //employeeData.BirthCityTitle = model.
            //employeeData.CertificateCityTitle = model.CertificateCity.Title;
            //employeeData.HomeCityTitle = model.HomeCity.Title;


            return employeeData;
        }

        public async Task<EmployeePictureModel> GetImageByEmployeeId(int employeeId)
        {
            var employeePicture = await _kscHrUnitOfWork.EmployeePictureRepository.GetAllQueryable()
                .OrderByDescending(a => a.InsertDate).FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (employeePicture == null)
            {
                return new EmployeePictureModel();
            }


            var model = new EmployeePictureModel()
            {
                Id = employeePicture.Id,
                EmployeeId = employeePicture.EmployeeId,
                PersonalNumber = employeePicture.PersonalNumber,
                Image = employeePicture.Image
            };


            return model;
        }

        public async Task<Employee> GetOneRelated(int id)
        {
            var result = await _kscHrUnitOfWork
                .EmployeeRepository
                .GetEmployeeIncludedCiteis(id)
                .Include(a => a.PaymentStatus)
                .Include(a => a.EmploymentType)
                .Include(x => x.EmployeeEducationDegrees)
                .ThenInclude(x => x.Education)
                .Include(a => a.InsuranceList)
                .FirstOrDefaultAsync();
            return result;
        }

        public KscResult UpdateEmployeeInfo(AddOrEditEmployeeBaseModel model)
        {

            var result = model.IsValid();

            if (!result.Success)
                return result;
            Ksc.HR.Share.General.Utility.IsValidNationalCode(model.NationalCode);
            var employeeByMeli = _kscHrUnitOfWork.EmployeeRepository.GetByNationalCode(model.NationalCode).FirstOrDefault();
            //برای پرسنلی ترک خدمتی که تایپ 1 دارند بتوان شماره پرسنلی جدید با کدملی قبلی ثبت کرد

            ExistsnationalCode(model.NationalCode, model.EmployeeNumber);


            if (model.MilitaryStatusId == 0)
            {
                model.MilitaryStatusId = null;
            }
            if (model.BloodTypeId == 0)
            {
                model.BloodTypeId = null;
            }
            if (model.RegionId == 0)
            {
                model.RegionId = null;
            }
            if (model.NationalityId == 0)
            {
                model.NationalityId = null;
            }
            if (model.MaritalStatusId == 0)
            {
                model.MaritalStatusId = null;
            }

            var checkStepper = _kscHrUnitOfWork.Stepper_StatusSystemMonthRepository
      .CheckYearMonth_SystemControlId(
      yearMonth: _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().AttendAbsenceItemDate,//140211,
      systemSequenceControlId: EnumSystemSequenceControl.HRSystem.Id
      , systemSequenceStatusId: EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id
      );

            if (checkStepper)
            {
                result.AddError("رکورد نامعتبر", "سیستم پرسنلی بسته است");
                return result;
            }


            if (model.Gender == EnumPersonalForm.Women.Id && model.MilitaryStatusId != null)
            {
                model.MilitaryStatusId = null;
            }
            if (model.InsuranceNumber != null && model.InsuranceTypeId == null)
            {
                throw new Exception("نوع بیمه باید مقدار داشته باشد.");

            }
            if (model.InsuranceTypeId != null && model.InsuranceListId == null)
            {
                throw new Exception("شرکت بیمه گزار باید مقدار داشته باشد.");

            }
            if (model.Gender == EnumPersonalForm.men.Id && model.MilitaryStatusId == null)
            {
                throw new Exception("وضعیت نظام وظیفه باید مقدار داشته باشد.");

            }

            if (model.MaritalStatusId == null)
            {
                throw new Exception("وضعیت تاهل باید مقدار داشته باشد.");

            }
            if (model.NationalityId == null)
            {
                throw new Exception("ملیت باید مقدار داشته باشد.");

            }
            if (model.RegionId == null)
            {
                throw new Exception("مذهب باید مقدار داشته باشد.");

            }

            Exists(model.Id, model.EmployeeNumber);


            var oneEmployeeInfo = GetOne(model.Id);
            if (oneEmployeeInfo == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            try
            {
                model.IsColesed = checkStepper;
                oneEmployeeInfo.Name = model.Name.Trim();
                oneEmployeeInfo.FatherName = model.FatherName.Trim();
                oneEmployeeInfo.Family = model.Family.Trim();
                oneEmployeeInfo.Gender = model.Gender;
                oneEmployeeInfo.NationalCode = model.NationalCode;
                oneEmployeeInfo.CertificateNumber = model.CertificateNumber;

                oneEmployeeInfo.BirthDate = model.BirthDate;
                oneEmployeeInfo.BirthCityId = model.BirthCityId;
                oneEmployeeInfo.CertificateCityId = model.CertificateCityId;
                oneEmployeeInfo.HomeCityId = model.HomeCityId;
                //oneEmployeeInfo.PersonalTypeId=model.PersonalTypeId;
                oneEmployeeInfo.RegionId = model.RegionId;
                oneEmployeeInfo.NationalityId = model.NationalityId;
                oneEmployeeInfo.MilitaryStatusId = model.MilitaryStatusId == 0 ? null : model.MilitaryStatusId;
                oneEmployeeInfo.MilitaryEndDate = model.MilitaryEndDate;
                oneEmployeeInfo.MilitaryStartDate = model.MilitaryStartDate;
                oneEmployeeInfo.MaritalStatusId = model.MaritalStatusId;
                oneEmployeeInfo.MarriedDate = model.MarriedDate;

                oneEmployeeInfo.NumberOfChildren = model.NumberOfChildren;
                oneEmployeeInfo.NumberOfDependents = model.NumberOfDependents;
                oneEmployeeInfo.InsuranceNumber = model.InsuranceNumber;
                oneEmployeeInfo.InsuranceListId = model.InsuranceListId;

                oneEmployeeInfo.CertificateDate = model.CertificateDate;
                oneEmployeeInfo.PreiveName = model.PreiveName;
                oneEmployeeInfo.PreiveFamily = model.PreiveFamily;

                oneEmployeeInfo.PhoneNumber = model.PhoneNumber;
                //oneEmployeeInfo.BloodTypeId = model.BloodTypeId;
                oneEmployeeInfo.BloodTypeId = model.BloodTypeId == 0 ? null : model.BloodTypeId;
                oneEmployeeInfo.RegionId = model.RegionId == 0 ? null : model.RegionId;
                oneEmployeeInfo.NationalityId = model.NationalityId == 0 ? null : model.NationalityId;
                oneEmployeeInfo.MaritalStatusId = model.MaritalStatusId == 0 ? null : model.MaritalStatusId;


                oneEmployeeInfo.HomeZipCode = model.HomeZipCode;
                oneEmployeeInfo.HomeAddress = !string.IsNullOrEmpty(model.HomeAddress) ? model.HomeAddress.Trim() : "";
                oneEmployeeInfo.UpdateDate = System.DateTime.Now;
                oneEmployeeInfo.UpdateUser = model.UpdateUser;
                oneEmployeeInfo.RemoteIpAddress = model.RemoteIpAddress;
                if (!string.IsNullOrEmpty(model.GuidPicId))
                {
                    oneEmployeeInfo.GuidPicId = model.GuidPicId;
                    var file = _attachmentService.AttachFile(new KSC.DMS.Dto.AddApplicationFileDto
                    {
                        ApplicationName = "HR",
                        EntityName = "Employee",
                        EntityKey = oneEmployeeInfo.Id.ToString(),
                        Title = oneEmployeeInfo.Name + ' ' + oneEmployeeInfo.Family,
                        Description = "عکس پرسنلی",
                        File = model.FileGuid,
                        FileName = model.FileName,
                        UserName = model.InsertUser,
                    });
                    var findpictureinhr = _kscHrUnitOfWork.EmployeePictureRepository.GetAllQueryable().Where(a => a.EmployeeId == oneEmployeeInfo.Id).ToList();


                    var pictureFiles = (_attachmentService.GetAttachments("HR", "Employee", new List<string> { oneEmployeeInfo.Id.ToString() })).GetAwaiter().GetResult();
                    var pictureFile = pictureFiles.FirstOrDefault(a => a.Name.Contains(model.GuidPicId.ToString()));
                    if (pictureFile != null)
                    {
                        var modelEmployeePicture = new EmployeePicture()
                        {
                            Image = pictureFile.Content,
                            PersonalNumber = oneEmployeeInfo.EmployeeNumber,
                            EmployeeId = oneEmployeeInfo.Id,
                            InsertDate = DateTime.Now,
                            InsertUser = model.UpdateUser,
                            IsActive = true,
                        };

                        foreach (var item in findpictureinhr)
                        {
                            item.IsActive = false;
                        }
                        _kscHrUnitOfWork.EmployeePictureRepository.Add(modelEmployeePicture);

                    }
                }
                _kscHrUnitOfWork.EmployeeRepository.Update(oneEmployeeInfo);


                if (oneEmployeeInfo.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id)
                {
                    ConnectMIS(model, "U");

                }

                if (_kscHrUnitOfWork.SaveAsync().GetAwaiter().GetResult() > 0)
                {
                    //if (!string.IsNullOrEmpty(model.GuidPicId) && model.GuidPicId != oneEmployeeInfo.GuidPicId)
                    //{

                    //}

                }
                model.PaymentStatusId = oneEmployeeInfo.PaymentStatusId.Value;




            }//try

            catch (Exception ex)
            {
                result.AddError("خطا", $"خطا در ثبت صورت گرفته است، مجددا ثبت کنید" + "(" + ex.Message + ")");

                return result;

            }
            return result;


        }


        #endregion
        public async Task<FilterResult<SearchEmployeeModel>> GetEmployeeByKendoFilter(FilterRequest Filter)
        {
            try
            {
                var query = _kscHrUnitOfWork.EmployeeRepository.GetEmploymentPerson()
                    .Include(a => a.TeamWork)
                    .Include(a => a.PaymentStatus).AsNoTracking();
                FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
                var modelResult = new FilterResult<SearchEmployeeModel>
                {
                    Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                    Total = result.Total
                };
                return modelResult;

            }
            catch (Exception ex)
            {
                return new FilterResult<SearchEmployeeModel>();

            }
        }
        //GetStandbyEmployeeForMission

        //همکار جانشین

        public async Task<FilterResult<SearchEmployeeModel>> GetStandbyEmployeeForMission(SearchEmployeeModel Filter)
        {
            try
            {

                var query = _kscHrUnitOfWork.EmployeeRepository.GetForStandbyEmployee().AsNoTracking();

                FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));

                var modelResult = new FilterResult<SearchEmployeeModel>
                {
                    Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data).Distinct(),
                    Total = result.Total
                };
                return modelResult;

            }
            catch (Exception)
            {
                return new FilterResult<SearchEmployeeModel>();

            }
        }
        public async Task<FilterResult<SearchEmployeeModel>> GetByFilterAsyncForRegistered(SearchEmployeeModel Filter, List<string> roles)
        {
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployee()
                      .AsNoTracking().OrderBy(a => a.PaymentStatusId);
                Filter.FixSortandFilter<Employee>(new Dictionary<string, string>
                {
                    { "NationalCode", "eq" },
                });
                FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(employee, Filter, nameof(Employee.PaymentStatusId));

                IQueryable<Employee> employeeByEmplId;
                if (Filter.emplId != 0 && !result.Data.Any(c => c.Id == Filter.emplId))
                {
                    employeeByEmplId = employee.Where(x => x.Id == Filter.emplId);

                    result.Data = result.Data.Union(employeeByEmplId);
                }

                var modelResult = new FilterResult<SearchEmployeeModel>
                {
                    Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data).Distinct(),
                    Total = result.Total
                };
                foreach (var item in modelResult.Data)
                {
                    item.FatherName = !string.IsNullOrEmpty(item.FatherName) ? item.FatherName : "";
                }
                return modelResult;

            }
            catch (Exception ex)
            {
                return new FilterResult<SearchEmployeeModel>();

            }
        }
        public async Task<FilterResult<SearchEmployeeModel>> GetByFilterAsyncForRegisteredByAccess(SearchEmployeeModel Filter, List<string> roles)
        {
            try
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployee();

                //
                var paymentStatusAccess = _kscHrUnitOfWork.PaymentStatusAccessRepository.GetPaymentStatusAccessesByRoles(roles);
                var paymentStatusId = paymentStatusAccess.Select(x => x.PaymentStatusId).Distinct().ToList();

                if (!paymentStatusId.Any(x => x == null)) //ادمین نباشد
                {
                    employee = employee.Where(x => paymentStatusId.Any(p => p == x.PaymentStatusId));
                }

                employee = employee.AsNoTracking().OrderBy(a => a.PaymentStatusId);
                //
                Filter.FixSortandFilter<Employee>(new Dictionary<string, string>
                {
                    { "NationalCode", "eq" },
                });
                FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(employee, Filter, nameof(Employee.PaymentStatusId));

                IQueryable<Employee> employeeByEmplId;
                if (Filter.emplId != 0 && !result.Data.Any(c => c.Id == Filter.emplId))
                {
                    employeeByEmplId = employee.Where(x => x.Id == Filter.emplId);

                    result.Data = result.Data.Union(employeeByEmplId);
                }

                var modelResult = new FilterResult<SearchEmployeeModel>
                {
                    Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data).Distinct(),
                    Total = result.Total
                };
                var employeeTypeIds = modelResult.Data.Select(a => a.EmploymentTypeId).ToList();
                var employeeType = _kscHrUnitOfWork.EmploymentTypeRepository.Where(a => employeeTypeIds.Contains(a.Id)).ToList();
                foreach (var item in modelResult.Data)
                {
                    var empType = employeeType.FirstOrDefault(a => a.Id == item.EmploymentTypeId);
                    item.FatherName = !string.IsNullOrEmpty(item.FatherName) ? item.FatherName : "";
                    item.EmploymentTypeTitle = empType != null ? empType.Title : "";
                }
                return modelResult;

            }
            catch (Exception ex)
            {
                return new FilterResult<SearchEmployeeModel>();

            }
        }
        //mission


        public async Task<FilterResult<SearchEmployeeModel>> GetEmployeeByKendoFilterForMission(SearchEmployeeModel Filter, List<string> roles)
        {
            try
            {

                var AcessLevel = _kscHrUnitOfWork.Mission_TypeAccessLevelRepository.CheckAccessLevelUser(roles);
                IQueryable<Employee> employee;
                //if (AcessLevel) //AcessLevel=True --> Admin ->نمایش تمام کاربران
                //{
                //    employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployee().Where(x => x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id);
                //}
                //else  //AcessLevel=false --> No Admin ->نمایش افراد تحت سرپرستی و خود شخص

                //{

                //    employee = _kscHrUnitOfWork.EmployeeRepository.GetLeaderWithHisPersonalByUserName(Filter.CurrentUserName);
                //}
                employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeForMissionByAccesslevel(Filter.CurrentUserName, AcessLevel, EnumPaymentStatus.DismiisalEmployee.Id);

                FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(employee, Filter, nameof(Employee.Id));
                //
                IQueryable<Employee> employeeByEmplId;
                if (Filter.emplId != 0)
                {
                    employeeByEmplId = employee.Where(x => x.Id == Filter.emplId);
                    result.Data = result.Data.Union(employeeByEmplId);
                }
                //
                var modelResult = new FilterResult<SearchEmployeeModel>
                {
                    Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data).Distinct(),
                    Total = result.Total
                };
                return modelResult;

            }
            catch (Exception)
            {
                return new FilterResult<SearchEmployeeModel>();

            }
        }

        //کل پرسنل  Interdict
        public async Task<FilterResult<SearchEmployeeModel>> GetEmployeeByKendoFilterForInterdict(SearchEmployeeModel Filter)
        {
            try
            {

                IQueryable<Employee> employee;

                employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployee().OrderByDescending(x => x.EmployeeNumber);
                // .Where(x => x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id);


                FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(employee, Filter, nameof(Employee.Id));
                //
                //IQueryable<Employee> employeeByEmplId;
                //if (Filter.emplId != 0)
                //{
                //    employeeByEmplId = employee.Where(x => x.Id == Filter.emplId);
                //    result.Data = result.Data.Union(employeeByEmplId);
                //}
                //
                var modelResult = new FilterResult<SearchEmployeeModel>
                {
                    Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data).Distinct(),
                    Total = result.Total
                };
                return modelResult;

            }
            catch (Exception)
            {
                return new FilterResult<SearchEmployeeModel>();

            }
        }

        //تیم خود یوزرویندوزی را پیدا میکند
        public FilterResult<SearchEmployeeModel> GetAllPersonalByKendoFilter11(SearchEmployeeModel Filter)
        {
            var query_ViewMisEmployee = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetAllQueryable().AsQueryable().AsNoTracking();

            if (Filter.IsOfficialAttendAbcense == false)
            {
                query_ViewMisEmployee = query_ViewMisEmployee.Where(a => a.WinUser.ToLower() == Filter.CurrentUserName);
            }

            var teamCodesUserWindows = query_ViewMisEmployee
                .Select(a => a.TeamCode).Distinct().ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIByTeamCodes(teamCodesUserWindows).AsQueryable().AsNoTracking();

            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        public FilterResult<SearchEmployeeModel> GetAllManagerByKendoFilter(SearchEmployeeModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking();

            if (Filter.IsOfficialAttendAbcense == false)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }

            var teamCodesUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamCode.ToString()).Distinct().ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIByTeamCodes(teamCodesUserWindows).AsQueryable().AsNoTracking();

            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }

        public FilterResult<SearchEmployeeModel> GetAllPersonalByKendoFilter(SearchEmployeeModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().Where(a => a.TeamWorkIsActive)
               .AsNoTracking();

            if (Filter.IsOfficialAttendAbcense == false)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }

            var teamCodesUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamCode.ToString()).Distinct().ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIByTeamCodes(teamCodesUserWindows).AsQueryable().AsNoTracking().ToList();
            if (Filter.AddCurrentUser == true && Filter.IsOfficialAttendAbcense == false)
            {
                var selectuser = query_ViewMisEmployeeSecurity.Select(a => a.EmployeeNumber).First();
                var currentUser = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().FirstOrDefault(a => a.EmployeeNumber == selectuser.Value.ToString());
                if (currentUser == null)
                {
                    query.Add(currentUser);
                }
            }

            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));

            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }

        //افراد تیم یک شخص بعلاوه خودش
        public FilterResult<SearchEmployeeModel> GetLeaderWithHisPersonalByKendoFilter(SearchEmployeeModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking();

            if (Filter.IsOfficialAttendAbcense == false)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.WindowsUser != null && a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }

            var teamCodesUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamCode.ToString()).Distinct().ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIByTeamCodes(teamCodesUserWindows)
                .Where(x => x.PaymentStatusId != 7).ToList();
            if (query.Count() != 0)
            {
                var person = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetAllQueryable().FirstOrDefault(a => a.WinUser != null && a.PaymentStatusCode != 7 && Filter.CurrentUserName.ToLower() == a.WinUser.ToLower());
                var personel = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNum(person.EmployeeNumber);
                query.Add(personel);
            }


            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        public FilterResult<SearchEmployeeModel> GetCurrentPersonalTeamByKendoFilter(SearchEmployeeModel filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking();
            var employeedata = GetOne(filter.Id);
            query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.TeamWorkId == employeedata.TeamWorkId);
            var teamCodesUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamCode.ToString()).Distinct().ToList();
            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIByTeamCodes(teamCodesUserWindows).Where(a => a.EmployeeNumber != employeedata.EmployeeNumber).AsQueryable().AsNoTracking();
            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }


        public FilterResult<SearchEmployeeModel> GetUserWindowsEmployeeByKendoFilter(SearchEmployeeModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking();

            if (Filter.IsOfficialAttendAbcense == false)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }

            var teamCodesUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamCode.ToString()).Distinct().ToList();

            var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository
                .GetAll().AsQueryable().Include(x => x.TeamWork)
                .Where(a => a.IsActive == true && teamCodesUserWindows.Contains(a.TeamWork.Code))
                .Select(a => a.EmployeeId).ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking()
                .Where(x => x.PaymentStatusId != 7);


            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }

        public FilterResult<SearchEmployeeModel> GetUserWindowsEmployeeByKendoFilterSalaryUser(SearchEmployeeModel Filter)
        {
            var query_ViewMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking();

            if (Filter.IsSalaryUser == false)
            {
                query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }

            var teamCodesUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamCode.ToString()).Distinct().ToList();

            var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository
                .GetAll().AsQueryable().Include(x => x.TeamWork)
                .Where(a => // a.IsActive == true &&
                teamCodesUserWindows.Contains(a.TeamWork.Code))
                .Select(a => a.EmployeeId).ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking()
                .Where(x => x.PaymentStatusId != 7);


            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }


        #region EmployeeTeamByOfficialManagment - مدیریت تیم و شیفت 

        public FilterResult<SearchEmployeeModel> GetAllUserWindowsEmployeeList(FilterRequest Filter)
        {
            // var teamCodesUserWindows = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
            //.Select(a => a.TeamCode.ToString()).ToList();

            // var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable().AsQueryable().Include(x => x.TeamWork)
            //     .Where(a => a.IsActive == true && teamCodesUserWindows.Contains(a.TeamWork.Code)).Select(a => a.EmployeeId).ToList();

            // //var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsQueryable()
            // //    .Where(a => activePersonsId.Contains(a.Id)).AsNoTracking();

            // var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsNoTracking();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmploymentPerson().AsNoTracking();

            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        #endregion
        /// <summary>
        /// این اصلاح شود
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CALLING_RPC GetPersonalDataMis(InputMisApiModel model)
        {
            return _dataPersonalForOtherSystemService.GetPersonalDataMis(model);


        }

        public EmployeeModel GetOneBySearchModel(SearchEmployeeModel model)
        {
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamWorkByEmployeeId(model.EmployeeNumber);

            var result = new EmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Family = employee.Family,
                EmployeeNumber = employee.EmployeeNumber,
                TeamWorkCode = employee.TeamWork.Code
            };

            return result;
        }

        public EmployeeModel GetOneByEmployeeId(int id)
        {
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamWorkByEmployeeId(id);

            var result = new EmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Family = employee.Family,
                EmployeeNumber = employee.EmployeeNumber,
                TeamWorkCode = employee.TeamWork.Code
            };

            return result;
        }
        public Employee GetOne(int id)
        {
            return _kscHrUnitOfWork.EmployeeRepository.GetById(id);
        }


        public EditEmployeeModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditEmployeeModel>(model);
        }


        #region EmployeeCondition -شرایط استخدام
        public async Task<EmployeeConditionModel> GetEmployeeCondition(int id)
        {
            var model = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeCondition(id);
            var employeeData = _mapper.Map<EmployeeConditionModel>(model);
            employeeData.WorkCityCityTitle = model.WorkCity==null?"": model.WorkCity.City.Title + "-" + model.WorkCity.Company.Title;
            employeeData.DismissalDateShamsi = model.DismissalDate.HasValue ? model.DismissalDate.Value.Date.ToPersianDate() : "";
            employeeData.DismissalStatusTitle = model.DismissalStatusId.HasValue ? model.Dismissal_Status.Title : "";
            if (model.JobPositionId.HasValue)
            {
                var jobposition = _kscHrUnitOfWork.Chart_JobPositionRepository.GetChart_JobPositionById(model.JobPositionId.Value)
                    .Include(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory)
                    .ThenInclude(a => a.Chart_JobCategoryDefination)
                    .Include(a => a.Chart_Structure)
                    .First();

                employeeData.JobPositionCode = jobposition.MisJobPositionCode;
                employeeData.JobPositionTitle = jobposition.Title;
                var Chart_JobCategoryDefinationTitle = jobposition.Chart_JobIdentity?.Chart_JobCategory.Chart_JobCategoryDefination.Title;
                var Chart_JobIdentityTitle = jobposition.Chart_JobIdentity?.Title;
                employeeData.JobIdentityDisplay = Chart_JobCategoryDefinationTitle + "-" + Chart_JobIdentityTitle;

                var Chart_StructureTitle = jobposition.Chart_Structure.Title;
                var StructureMisJobPositionCode = jobposition.Chart_Structure.MisJobPositionCode;
                employeeData.StructureDisplay = StructureMisJobPositionCode + "-" + Chart_StructureTitle;
            }
            if (model.TeamWorkId.HasValue)
            {
                var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable().First(a => a.Id == model.TeamWorkId.Value);

                employeeData.TeamWorkTitle = teamWork.Code + "-" + teamWork.Title;

            }

            var LastEmployeeInterdict = _kscHrUnitOfWork.EmployeeInterdictRepository.GetLastInterdictEmployee(id);
            employeeData.LastGroupNumber= LastEmployeeInterdict==null?0: LastEmployeeInterdict.CurrentJobGroupId.Value;
            return employeeData;
        }
        public async Task<KscResult> PostEmployeeConditionold(EmployeeConditionModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try
            {
                if (model.HasFloatTime == true && !model.FloatTimeSettingId.HasValue)
                {
                    result.AddError("", "لطفا تایم شناور را انتخاب کنید");
                    return result;
                }
                var employee = GetOne(model.Id);
                employee.EmploymentTypeId = model.EmploymentTypeId;
                employee.EmploymentStatusId = model.EmploymentStatusId;
                employee.EmploymentDate = model.EmploymentDate;
                employee.ContractStartDate = model.ContractStartDate;
                employee.ContractEndDate = model.ContractEndDate;
                employee.HasFloatTime = model.HasFloatTime;
                employee.FloatTimeSettingId = model.FloatTimeSettingId;
                employee.WorkCityId = model.WorkCityId;
                employee.EntryExitTypeId = model.EntryExitTypeId;
                employee.SavingTypeId = model.SavingTypeId;
                employee.SavingTypeDate = model.SavingTypeDate;
                //var employeeData = _mapper.Map(model, employee);
                // EmployeeWorkGroup
                //وضعیت جدید الاستخدام
                if (employee.PaymentStatusId == EnumPaymentStatus.NewEmployee.Id)
                {
                    if (!_kscHrUnitOfWork.EmployeeWorkGroupRepository.Any(x => x.EmployeeId == model.Id))
                    {
                        EmployeeWorkGroup newEmployeeWorkGroup = new EmployeeWorkGroup();
                        newEmployeeWorkGroup.EmployeeId = model.Id;
                        newEmployeeWorkGroup.IsActive = true;
                        newEmployeeWorkGroup.WorkGroupId = model.WorkGroupId.Value;
                        newEmployeeWorkGroup.InsertUser = model.CurrentUserName;
                        newEmployeeWorkGroup.InsertDate = System.DateTime.Now;
                        newEmployeeWorkGroup.StartDate = model.EmploymentDate.Value;
                        await _kscHrUnitOfWork.EmployeeWorkGroupRepository.AddAsync(newEmployeeWorkGroup);
                    }
                    else
                    {
                        EmployeeWorkGroup updateEmployeeWorkGroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetActiveWorkGroupByEmployeeId(model.Id);
                        updateEmployeeWorkGroup.WorkGroupId = model.WorkGroupId.Value;
                        updateEmployeeWorkGroup.UpdateUser = model.CurrentUserName;
                        updateEmployeeWorkGroup.UpdateDate = System.DateTime.Now;
                        updateEmployeeWorkGroup.StartDate = model.EmploymentDate.Value;
                        _kscHrUnitOfWork.EmployeeWorkGroupRepository.Update(updateEmployeeWorkGroup);
                    }
                    employee.WorkGroupId = model.WorkGroupId;

                }//

                _kscHrUnitOfWork.EmployeeRepository.Update(employee);

                //if (result.Success == true)
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }




        public async Task<KscResult> PostEmployeeCondition(AddOrEditEmployeeConditionModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try
            {

                var employee = GetOne(model.Id);
                model.EmployeeNumber = employee.EmployeeNumber;

                result = await CheckForSaveJobPosition(model, result);
                if (!result.Success)
                {
                    return result;
                }
                employee.EmploymentTypeId = model.EmploymentTypeId;
                employee.EmploymentStatusId = model.EmploymentStatusId;
                employee.EmploymentDate = model.EmploymentDate;
                employee.WorkCityId = model.WorkCityId;

                _kscHrUnitOfWork.EmployeeRepository.Update(employee);

                #region SyncMIS
                var condotionModel = new EmployeeConditionModel()
                {
                    EmployeeNumber = model.EmployeeNumber,
                    EmploymentTypeId = model.EmploymentTypeId,
                    EmploymentStatusId = model.EmploymentStatusId,
                    EmploymentDate = model.EmploymentDate,
                    WorkCityId = model.WorkCityId,
                    P_function = "Employment",
                    PaymentStatusId = employee.PaymentStatusId.HasValue ? employee.PaymentStatusId.Value : 0
                };
                var updateMIS = _misUpdateService.UpdateEmployeeConditionModel(condotionModel);
                if (updateMIS.IsSuccess == false)
                {
                    result.AddError("خطا MIS", $"خطا MIS - {string.Join(",", updateMIS.Messages)}");
                    return result;
                }
                #endregion

                //if (result.Success == true)
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }




        // جداول وابسته که باید اپدیت شود
        #region 


        private async Task<KscResult> CheckForSaveJobPosition(AddOrEditEmployeeConditionModel model, KscResult result)
        {
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeWorkGroupWorkTimeByRelated().AsNoTracking()
                    .Where(x => x.Id == model.Id)
                    .FirstOrDefaultAsync()
                    .GetAwaiter().GetResult();


            //other check

            if (model.EmploymentTypeId == 0)
            {
                result.AddError("", "نوع استخدام باید مقدار داشته باشد.");
                return result;
            }

            if (model.EmploymentStatusId == 0)
            {
                result.AddError("", "وضعیت استخدام باید مقدار داشته باشد.");
                return result;
            }

            if (model.EmploymentDate == null)
            {
                result.AddError("", "تاریخ استخدام باید مقدار داشته باشد.");
                return result;
            }
            if (model.WorkCityId == 0)
            {
                result.AddError("", "هر محل خدمت را وارد نمایید");
                return result;
            }



            return result;
        }

        public async Task<KscResult> AddEmployeeEducationDegree(EmployeeConditionModel model)
        {
            var result = new KscResult();


            if (!result.Success)
                return result;
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.Id);
            var employeeEducationDegreeActive = _kscHrUnitOfWork.EmployeeEducationDegreeRepository.GetActiveByEmployeeID(model.Id);
            if (employeeEducationDegreeActive.Any()
                && employee.PaymentStatusId == EnumPaymentStatus.NewEmployee.Id
                && employee.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id)
            {
                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "مدارک فعال دارید"));

            }
            EmployeeEducationDegree newEmployeeEducationDegree = new EmployeeEducationDegree
            {
                EmployeeId = model.Id,
                IsActive = true,
                EducationDate = model.EducationDate,
                StudyFieldId = model.StudyFieldId.Value,
                EducationId = model.EducationId.Value,
                InsertUser = model.CurrentUserName,
                InsertDate = System.DateTime.Now
            };

            await _kscHrUnitOfWork.EmployeeEducationDegreeRepository.AddAsync(newEmployeeEducationDegree);


            return result;
        }

        //-----------------------/مدارک تحصیلی --------------
        public async Task<KscResult> PostEmployeeEducationDegree(EmployeeConditionModel model)
        {
            var result = new KscResult();

            if (model.EmployeeEducationDegreeId > 0)
            {
                var searchModel = _kscHrUnitOfWork.EmployeeEducationDegreeRepository.GetById(model.EmployeeEducationDegreeId);
                searchModel.EducationId = model.EducationId.Value;//مدرک
                searchModel.StudyFieldId = model.StudyFieldId.Value;//رشته
                searchModel.EducationDate = model.EducationDate; //تاریخ اخذ مدرک
                searchModel.UpdateDate = DateTime.Now;
                searchModel.UpdateUser = model.CurrentUserName;
                _kscHrUnitOfWork.EmployeeEducationDegreeRepository.Update(searchModel);
            }
            else
            {
                result = await AddEmployeeEducationDegree(model);
            }

            return result;
        }


        //-----------------------------teamwork مدیریت تیم//------------------------------------

        public async Task<KscResult> PostEmployeeTeamWork(EmployeeConditionModel model)
        {
            var result = new KscResult();

            if (model.EmployeeTeamWorkId > 0)
            {
                var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.Id);
                if (employee.PaymentStatusId == EnumPaymentStatus.NewEmployee.Id)
                {
                    result = await UpdateEmployeeTeamWorkManagement(model);

                }
            }
            else
            {
                result = await AddEmployeeTeamWorkManagement(model);

            }
            return result;
        }
        public async Task<KscResult> AddEmployeeTeamWorkManagement(EmployeeConditionModel model)
        {
            var result = new KscResult();


            var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.Id);
            var employeeTeamWorkActive = await _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActiveTeamWorkByEmployeeIdAsync(model.Id);
            if (employeeTeamWorkActive != null && employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
            {

                result.AddError("خطا", "تیم کاری فعال دارید");
                return result;

            }
            if (model.TeamStartDate < employee.EmploymentDate)
            {
                result.AddError("", "تاریخ شروع  کوچک تر از تاریخ استخدام می باشد");
                return result;
            }
            EmployeeTeamWork newEmployeeTeamWork = new EmployeeTeamWork();
            newEmployeeTeamWork.EmployeeId = model.Id;
            newEmployeeTeamWork.IsActive = true;
            newEmployeeTeamWork.TeamWorkId = model.TeamWorkId.Value;
            newEmployeeTeamWork.TeamStartDate = model.TeamStartDate.Value;
            newEmployeeTeamWork.InsertUser = model.CurrentUserName;
            newEmployeeTeamWork.InsertDate = System.DateTime.Now;
            employee.TeamWorkId = model.TeamWorkId;
            employee.TeamWorkStartDate = newEmployeeTeamWork.TeamStartDate;
            await _kscHrUnitOfWork.EmployeeTeamWorkRepository.AddAsync(newEmployeeTeamWork);
            //_kscHrUnitOfWork.EmployeeRepository.Update(employee);


            return result;
        }
        public async Task<KscResult> UpdateEmployeeTeamWorkManagement(EmployeeConditionModel model)
        {
            var result = new KscResult();


            var employee = _kscHrUnitOfWork.EmployeeRepository.GetById(model.Id);
            if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
            {
                //throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تیم کاری فعال دارید"));
                result.AddError("", "تیم کاری فعال دارید");
                return result;
            }
            if (model.TeamStartDate < employee.EmploymentDate)
            {
                result.AddError("", "تاریخ شروع  کوچک تر از تاریخ استخدام می باشد");
                return result;
            }
            EmployeeTeamWork updateEmployeeTeamWork = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetById(model.EmployeeTeamWorkId);
            updateEmployeeTeamWork.TeamWorkId = model.TeamWorkId.Value;
            updateEmployeeTeamWork.TeamStartDate = model.TeamStartDate.Value;
            updateEmployeeTeamWork.UpdateUser = model.CurrentUserName;
            updateEmployeeTeamWork.UpdateDate = System.DateTime.Now;
            employee.TeamWorkId = model.TeamWorkId;
            employee.TeamWorkStartDate = updateEmployeeTeamWork.TeamStartDate;
            _kscHrUnitOfWork.EmployeeTeamWorkRepository.Update(updateEmployeeTeamWork);
            //_kscHrUnitOfWork.EmployeeRepository.Update(employee);

            return result;
        }

        #endregion

        #endregion

        public EditEmployeeModel GetEmployeeIncludedWorkCity(int id)
        {
            var employeemodel = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedWorkCity().FirstOrDefault(x => x.Id == id);
            return _mapper.Map<EditEmployeeModel>(employeemodel);
        }
        public EditEmployeeModel GetForEditByWfRequestId(int id)
        {
            var onCall_Request = _kscHrUnitOfWork.OnCall_RequestRepository.WhereQueryable(x => x.RequestId == id).FirstOrDefault();
            var wf_Request = _kscHrUnitOfWork.WF_RequestRepository.GetAllQueryable().FirstOrDefault(x => x.Id == id);
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().Include(x => x.WorkGroup).FirstOrDefault(x => x.Id == wf_Request.EmployeeId);

            var model = _mapper.Map<EditEmployeeModel>(employee);
            model.WorkTimeId = employee.WorkGroup.WorkTimeId;
            return model;
        }
        public string GetSuperiorJobPositionCodeByProcssId(int employeeId, int procssId, string domain)
        {
            var jobCategoryRanges = _kscHrUnitOfWork.WF_JobCategoryRangeRepository.GetAllQueryable().AsQueryable().Include(x => x.WF_ValidJobCategories).FirstOrDefault(x => x.WorkFlowProcessId == procssId);
            // var jobCategoryRanges = process.WF_JobCategoryRanges.FirstOrDefault();
            Employee employee = GetOne(employeeId);
            var person = _dataPersonalForOtherSystemService.GetPersonalDataMis(new InputMisApiModel() { NUM_PRSN_EMPL = employee.EmployeeNumber, FUNCTION = "FETCH_GENERAL", domain = domain });
            var jobPosition = person.DETAIL.FK_JPOS_EMPL;
            string SuperiorJobPosition = "";
            //            COD_CAT_JOB
            //COD_JOB_STA_JPOS

            //            Superiorperson.DETAIL.
            bool check = true;
            int i = 0;

            while (check)
            {
                i++;
                var Superiorperson = _dataPersonalForOtherSystemService.GetPersonalDataMis(new InputMisApiModel() { USER_FK_JPOS = jobPosition, FUNCTION = "JPOS_UPPER_POST", domain = domain });
                if (Superiorperson == null || Superiorperson.DETAIL == null || string.IsNullOrEmpty(Superiorperson.DETAIL.COD_JOB_ORGL))
                {
                    check = false;
                    break;
                }
                jobPosition = Superiorperson.DETAIL.COD_JOB_ORGL;
                if (jobCategoryRanges.LowestJobCategoryCode != Superiorperson.DETAIL.COD_CAT_JOB && jobCategoryRanges.HighestJobCategoryCode != Superiorperson.DETAIL.COD_CAT_JOB)
                {
                    break;
                }
                else
                if (!jobCategoryRanges.WF_ValidJobCategories.Any(x => x.IsActive && x.JobStatusCode == Superiorperson.DETAIL.COD_JOB_STA_JPOS))
                {
                    break;
                }
                else
                {
                    check = false;
                    SuperiorJobPosition = jobPosition;
                }
                if (i > 15)
                    check = false;
            }
            return SuperiorJobPosition;
        }

        public EmployeeModel GetPersonnelDetails(int id, string domain)
        {
            var emp = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeIdIncludedAbsence(id);

            var DataModel = new InputMisApiModel()
            {
                NUM_PRSN_EMPL = emp.EmployeeNumber,
                FUNCTION = "FETCH_GENERAL",
                domain = domain
            };

            var outPutMIS = _dataPersonalForOtherSystemService.GetPersonalDataMis(DataModel);
            var model = new EmployeeModel();
            model.Id = id;
            if (outPutMIS.IsError == true)
            {
                model.MsgError = outPutMIS.MsgError;
                model.IsError = outPutMIS.IsError;
                return model;
            }

            model.MsgError = outPutMIS.MsgError;
            model.IsError = outPutMIS.IsError;

            model.EmployeeNumber = emp.EmployeeNumber;
            model.Family = outPutMIS.DETAIL.NAM_FAM_EMPL;
            model.Name = outPutMIS.DETAIL.NAM_PER_EMPL;
            model.TeamWorkCode = outPutMIS.DETAIL.NUM_TEAM_EMPL;
            model.TeamWorkTitle = outPutMIS.DETAIL.DES_TEAM_EMTEM;
            model.JobPositionTitle = outPutMIS.DETAIL.DES_POS_JPOS;
            model.SubFunctionTitle = outPutMIS.DETAIL.DES_SUBF;
            model.IsOnCalling = outPutMIS.DETAIL.FLG_ONCAL_JPOS;
            model.NoeEstekhdam = outPutMIS.DETAIL.DES_EMPLT;
            model.JobPositionCode = outPutMIS.DETAIL.FK_JPOS_EMPL;
            model.CallingCostCenter = outPutMIS.DETAIL.COD_CC_CCRRX;
            model.SubFunctionCode = outPutMIS.DETAIL.COD_SUBF;
            //   model.WorkTimeCode = outPutMIS.DETAIL.COD_TYP_WRK_EMPL;
            model.WorkTimeCode = emp.WorkGroup.WorkTimeId.ToString();
            model.SuperiorJobPositionCode = outPutMIS.DETAIL.COD_JOB_ORGL;
            model.misEmployeeRegister = outPutMIS.DETAIL.DAT_EMPLT_EMPL;
            model.CodeEmployeeRegister = outPutMIS.DETAIL.COD_STA_PYM_EMPL;
            model.misEmployeeRegister = int.Parse(outPutMIS.DETAIL.DAT_EMPLT_EMPL).ToString("####/##/##");
            model.RegisterDate = model.misEmployeeRegister.ToGregorianDateTime();
            model.TotalDayVocationLeave = emp.EmployeeLongTermAbsences.Sum(a => a.AbsenceDayCount);
            model.Sickleaves = GetSearchEmployeeModels(2, id);
            model.Vocationlistleaves = GetSearchEmployeeModels(3, id);

            model.TOT_SHFT_PER_JPOS = outPutMIS.DETAIL.TOT_SHFT_PER_JPOS;
            model.TOT_NRM_PER_JPOS = outPutMIS.DETAIL.TOT_NRM_PER_JPOS;
            model.NUM_PER_CNTRC_JPOS = outPutMIS.DETAIL.NUM_PER_CNTRC_JPOS;
            model.COD_CAT_JOB = outPutMIS.DETAIL.COD_CAT_JOB;
            int DAT_DSMSL_EMPL = int.Parse(outPutMIS.DETAIL.DAT_DSMSL_EMPL);
            if (DAT_DSMSL_EMPL > 0)
                model.DAT_DSMSL_EMPL = DAT_DSMSL_EMPL.ToString("####/##/##");
            model.COD_DSMSL_EMPL = outPutMIS.DETAIL.COD_DSMSL_EMPL;
            int Dismissal_StatusId;
            if (int.TryParse(outPutMIS.DETAIL.COD_DSMSL_EMPL, out Dismissal_StatusId))
            {
                if (_kscHrUnitOfWork.Dismissal_StatusRepository.Any(a => a.Id == Dismissal_StatusId))
                    model.Title_DSMSL_EMPL = !string.IsNullOrEmpty(outPutMIS.DETAIL.COD_DSMSL_EMPL) ? _kscHrUnitOfWork.Dismissal_StatusRepository.GetById(Dismissal_StatusId).Title : "";
            }
            model.COD_USE_HOUS_EMPL = outPutMIS.DETAIL.COD_USE_HOUS_EMPL == "1";
            //
            return model;
        }


        public async Task<EmployeeModel> GetPersonnelEmployeeDetails(int id)
        {
            var emp = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByRelatedMonthTimeSheet().FirstOrDefault(x => x.Id == id);
            if (emp == null)
                throw new HRBusinessException(Validations.RepetitiveId, "لطفا پرسنل راانتخاب کنید ");

            //var DataModel = new InputMisApiModel()
            //{
            //    NUM_PRSN_EMPL = emp.EmployeeNumber,
            //    FUNCTION = "FETCH_GENERAL",
            //    domain = "KSC"
            //};

            //var outPutMIS = _kscHrUnitOfWork.EmployeeRepository.GetPersonalDataMis(DataModel);
            var model = new EmployeeModel();
            //model.Id = id;
            //if (outPutMIS.IsError == true)
            //{
            //    model.MsgError = outPutMIS.MsgError;
            //    model.IsError = outPutMIS.IsError;
            //    return model;
            //}

            //model.MsgError = outPutMIS.MsgError;
            //model.IsError = outPutMIS.IsError;

            model.EmployeeNumber = emp.EmployeeNumber;
            model.Family = emp.Family;
            model.Name = emp.Name;
            model.Gender = emp.Gender;
            model.MaritalStatusId = emp.MaritalStatusId;
            model.TeamWorkCode = emp.TeamWork?.Code;
            model.TeamWorkTitle = emp.TeamWork?.Title;
            model.WorkGroupCode = emp.WorkGroup?.Code;

            var MeritVacationRemaining = EnumVacation.MeritVacationRemaining.Id.ToString();
            var remianingVacationInLastMonth = _kscHrUnitOfWork.EmployeeVacationManagementRepository.GetAllQueryable().AsNoTracking()
                .FirstOrDefault(a => a.EmployeeId == emp.Id && a.Vacation.Code == MeritVacationRemaining);

            if (remianingVacationInLastMonth != null)
            {
                var timeSheetSettingActive = await _kscHrUnitOfWork.TimeSheetSettingRepository.GetTimeSheetSettingActiveAsync();
                var vacationEntitlementTimePerMonth = timeSheetSettingActive.VacationEntitlementTimePerMonth;
                model.RemianingVacationInMinute = remianingVacationInLastMonth.ValueDuration;
            }
            model.PaymentStatusTitle = emp.PaymentStatus.Title;
            if (emp.EmploymentTypeId.HasValue)
                model.EmployeeTypeTitle = _kscHrUnitOfWork.ViewMisEmploymentTypeRepository.GetEmployeTypeTitle(emp.EmploymentTypeId.Value);
            model.WorkTimeTitle = emp.WorkGroup?.WorkTime?.Title;
            model.MaximumDurationOverTime = emp.TeamWork?.OverTimeDefinition?.MaximumDuration;
            return model;
        }
        private List<SearchRollCallDefinicationModel> GetSearchEmployeeModels(int rollCallCategory, int employeeId)
        {
            var result = _kscHrUnitOfWork.RollCallDefinitionRepository.GetIncluded().AsNoTracking().Where(a => a.RollCallCategoryId == rollCallCategory).Select(a => new SearchRollCallDefinicationModel()
            {
                Title = a.Title,
                Total = a.EmployeeLongTermAbsences.Where(a => a.EmployeeId == employeeId).Sum(a => a.AbsenceDayCount),
            }).ToList();
            return result;
        }

        #region EmployeeTransferByOfficialManagement - تغییر تیم یا شیفت کاری
        public async Task<KscResult> EmployeeTransferByOfficialManagement(EmployeeTeamManagmentModel entity)
        {
            var result = entity.IsValid();
            try
            {
                if (!result.Success)
                    return result;
                if (entity.TeamWorkId.HasValue == false && entity.WorkGroupId.HasValue == false)
                {
                    result.AddError("خطا", "لطفا اطلاعات را وارد نمایید");
                    return result;
                }

                if (entity.TeamWorkId.HasValue == true && entity.TeamTransferReturnDate.HasValue == false)
                {
                    result.AddError("خطا", "تاریخ احراز تیم را انتخاب نمایید");
                    return result;
                }

                if (entity.WorkGroupId.HasValue == true && entity.ShiftTransferReturnDate.HasValue == false)
                {
                    result.AddError("خطا", "تاریخ احراز شیفت را انتخاب نمایید");
                    return result;
                }

                List<Task> taskList = new List<Task>();
                IEnumerable<int> employeesId = entity.EmployeesId;
                //entity.EmployeesId.ToList();
                foreach (var item in employeesId)
                {
                    ResultEmployeeTransferModel model = new ResultEmployeeTransferModel()
                    {
                        EmployeeId = item,
                        TeamWorkId = entity.TeamWorkId,
                        TransferChangeDate = entity.TeamTransferReturnDate,
                        TransferChangeDateShift = entity.ShiftTransferReturnDate,
                        TransferRequestTypeId = entity.TransferRequestTypeId,
                        CurrentUserName = entity.InsertUser,
                        WorkGroupId = entity.WorkGroupId,
                        DomainName = entity.DomainName,
                        //LastWorkGroupId=
                        //TransferRequestId=
                    };
                    await EmployeeTeamWork_WorkGroupTransferManagement_Kartable(model);
                }
                await _kscHrUnitOfWork.SaveAsync();
                return result;

            }
            catch (Exception ex)
            {

                result.AddError("خطا", ex.Message);
                return result;
            }
        }
        #endregion
        public async Task EmployeeTransferManagement(ResultEmployeeTransferModel model)
        {
            try
            {
                await EmployeeTeamWork_WorkGroupTransferManagement(model);
                // if (model.TransferRequestTypeId == EnumRequestType.TeamTransfer.Id)
                //{

                //    await _employeeTeamWorkService.EmployeeTeamWorkTransferManagement(model);
                //}
                //else if (model.TransferRequestTypeId == EnumRequestType.ShiftTransfer.Id)
                //{
                //    //await _employeeWorkGroupService.EmployeeWorkGroupTransferManagement(model);
                //}


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// مدیریت تخصیص تیم و شیفت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task EmployeeTeamWork_WorkGroupTransferManagement_Kartable(ResultEmployeeTransferModel model)
        {
            KscResult result = new KscResult();
            try
            {

                Employee employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamWork_WorkGroupByEmployeeId(model.EmployeeId);
                if (employee == null)
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, Resources.Personal.EmployeeResource.EmployeeNumber));

                if (employee.EmploymentDate.HasValue == false)
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, Resources.Personal.EmployeeResource.EmploymentDate));

                if (employee.EmploymentDate > model.TransferChangeDate)
                    throw new HRBusinessException(Validations.GreaterThan_FieldValue, String.Format(Validations.GreaterThan_FieldValue, "تاریخ احراز", Resources.Personal.EmployeeResource.EmploymentDate));

                string NUM_TEAM_EMPL = string.Empty,
                NUM_TEAM_LIST = string.Empty,
                DAT_STR_TEAM = string.Empty,
                COD_TYP_LIST = string.Empty,
                COD_GRP_LIST = string.Empty,
                STR_WORK_LIST = string.Empty,
                FUNCTION = string.Empty;
                var IsChange = false;
                //
                if (model.TransferRequestId == null)
                    ExistTransferRequestValidation(model);
                // تیم کاری
                if (model.TeamWorkId.HasValue == true)
                {
                    if (employee.TeamWorkStartDate >= model.TransferChangeDate)
                    {
                        //throw new Exception("تاریخ تغییر وارده برای ثبت اطلاعات مجاز نمی باشد.");
                        throw new Exception(String.Format("کاربر از تاریخ {0} در تیم {1} می باشد،امکان جابه جایی وجود ندارد", employee.TeamWorkStartDate.ToPersianDate(), employee.TeamWork.Code));

                    }
                    var employeeTeamWorkActive = await _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActiveTeamWorkByEmployeeIdAsync(model.EmployeeId);
                    if (employeeTeamWorkActive != null && employeeTeamWorkActive.TeamWorkId == model.TeamWorkId)
                    {  // nothing تکراری ثبت نشود
                        //var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetById(model.TeamWorkId.Value);
                        //NUM_TEAM_LIST = teamWork.Code;
                    }
                    else
                    {
                        IsChange = true;
                        if (employeeTeamWorkActive == null)
                        {// برو تیم جدید براش ثبت کن
                         //بعدش employee رو اپدیت کن
                            if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تیم کاری فعال "));
                        }
                        else
                        {

                            //if(employeeTeamWorkActive.TeamEndDate==model.TransferReturnDate)
                            employeeTeamWorkActive.IsActive = false;
                            if (employeeTeamWorkActive.TeamEndDate == null)
                            {
                                employeeTeamWorkActive.TeamEndDate = model.TransferChangeDate.Value.AddDays(-1);
                            }
                            if (model.IsTransferReturn) // برگشت جابه جایی
                            {
                                employeeTeamWorkActive.TeamEndDate = model.TransferReturnDate;
                            }
                            employeeTeamWorkActive.UpdateUser = model.CurrentUserName;
                            employeeTeamWorkActive.UpdateDate = System.DateTime.Now;

                        }
                        EmployeeTeamWork newEmployeeTeamWork = new EmployeeTeamWork();
                        newEmployeeTeamWork.EmployeeId = model.EmployeeId;
                        newEmployeeTeamWork.IsActive = true;
                        newEmployeeTeamWork.TeamWorkId = model.TeamWorkId.Value;
                        newEmployeeTeamWork.TransferRequestId = model.TransferRequestId;
                        newEmployeeTeamWork.InsertUser = model.CurrentUserName;
                        newEmployeeTeamWork.InsertDate = System.DateTime.Now;
                        if (!model.IsTransferReturn)
                        {
                            newEmployeeTeamWork.TeamStartDate = model.TransferChangeDate.Value;
                        }
                        else
                        {
                            newEmployeeTeamWork.TeamStartDate = employeeTeamWorkActive.TeamEndDate.Value.AddDays(1);
                        }
                        employee.TeamWorkId = model.TeamWorkId;
                        employee.TeamWorkStartDate = newEmployeeTeamWork.TeamStartDate;
                        await _kscHrUnitOfWork.EmployeeTeamWorkRepository.AddAsync(newEmployeeTeamWork);
                        //
                        var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetById(model.TeamWorkId.Value);
                        NUM_TEAM_LIST = teamWork.Code;
                        if (employeeTeamWorkActive != null)
                        {
                            NUM_TEAM_EMPL = employeeTeamWorkActive.TeamWork.Code;//تیم فعلی
                            DAT_STR_TEAM = employeeTeamWorkActive.TeamStartDate.ToPersianDate().Replace("/", "");//تاریخ شروع تیم فعلی
                        }
                    }
                }
                // شیفت کاری
                if (model.WorkGroupId.HasValue == true)
                {
                    if (employee.WorkGroupStartDate >= model.TransferChangeDateShift && !model.IsTransferReturn)
                    {
                        //  throw new Exception("تاریخ تغییر وارده برای ثبت اطلاعات مجاز نمی باشد.");
                        throw new Exception(String.Format("کاربر از تاریخ {0} در {1} می باشد،امکان جابه جایی وجود ندارد", employee.WorkGroupStartDate.ToPersianDate(), employee.WorkGroup.WorkTime.Title + "-" + employee.WorkGroup.Code));

                    }
                    model.LastWorkGroupId = employee.WorkGroupId;

                    var employeeWorkGroupActive = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetActiveWorkGroupByEmployeeIdAsync(model.EmployeeId);
                    if (employeeWorkGroupActive != null && employeeWorkGroupActive.WorkGroupId == model.WorkGroupId)
                    {
                        //nothing داده تکراری ذخیره نشود

                        //var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(model.WorkGroupId.Value);
                        //COD_TYP_LIST = workGroup.WorkTime.Code.Trim();
                        //COD_GRP_LIST = workGroup.Code.Trim();

                    }
                    else
                    {
                        IsChange = true;
                        if (employeeWorkGroupActive == null)
                        {
                            // شیفت جدید بایستی ثبت شود
                            if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "گروه کاری فعال "));
                        }
                        if (employeeWorkGroupActive != null)
                        {
                            employeeWorkGroupActive.IsActive = false;
                            if (employeeWorkGroupActive.EndDate == null)
                            {
                                employeeWorkGroupActive.EndDate = model.TransferChangeDateShift.Value.AddDays(-1);
                            }
                            else
                            {
                                //
                                if (!model.IsTransferReturn)
                                    throw new Exception("تا برگشت جابه جایی موقت امکان تغییر شیفت وجود ندارد.");
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
                            newEmployeeWorkGroup.StartDate = model.TransferChangeDateShift.Value;
                        }
                        else
                        {
                            newEmployeeWorkGroup.StartDate = employeeWorkGroupActive.EndDate.Value.AddDays(1);
                        }
                        if (model.IsTemporaryTransfer && !model.IsTransferReturn)
                        {
                            newEmployeeWorkGroup.EndDate = model.TransferReturnDate;
                        }

                        var lastWorkGroup = model.LastWorkGroupId.HasValue ? _kscHrUnitOfWork.WorkGroupRepository.GetById(model.LastWorkGroupId.Value) : null;
                        var newWorkGroup = _kscHrUnitOfWork.WorkGroupRepository.GetById(model.WorkGroupId.Value);
                        if (lastWorkGroup != null && (lastWorkGroup.WorkTimeId != newWorkGroup.WorkTimeId))
                            newEmployeeWorkGroup.WorkTimeChange = true;
                        employee.WorkGroupId = model.WorkGroupId;
                        employee.WorkGroupStartDate = newEmployeeWorkGroup.StartDate;
                        //
                        //
                        var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking();
                        var hasItemInAnotherWorkTimeId = employeeAttendAbsenceItem
                            .Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 >=
                         newEmployeeWorkGroup.StartDate && x.WorkTimeId != newWorkGroup.WorkTimeId);
                        if (hasItemInAnotherWorkTimeId.Any())
                        {
                            var returnResult = CheckHasItemInAnotherWorkTimeId(hasItemInAnotherWorkTimeId, newWorkGroup, employee, true);
                            if (!returnResult.Success)
                            {
                                if (returnResult.ErrorNumber == 101)
                                {
                                    throw new Exception(string.Join(",", returnResult.Errors));

                                }
                                var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                                throw new Exception($" در تاریخهای {string.Join(",", dates)} در گروه کاری دیگری کارکرد تایید شده دارد");
                            }
                        }

                        //
                        //
                        await _kscHrUnitOfWork.EmployeeWorkGroupRepository.AddAsync(newEmployeeWorkGroup);
                        var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(model.WorkGroupId.Value);
                        COD_TYP_LIST = workGroup.WorkTime.Code.Trim();
                        COD_GRP_LIST = workGroup.Code.Trim();
                        STR_WORK_LIST = newEmployeeWorkGroup.StartDate.ToPersianDate().Replace("/", "");
                        //
                    }
                }

                //if (model.IsTemporaryTransfer && !model.IsTransferReturn)
                //{
                //    newEmployeeTeamWork.TeamEndDate = model.TransferReturnDate;
                //}

                //
                if (IsChange == true)
                {
                    if (model.TeamWorkId.HasValue && model.WorkGroupId.HasValue)
                        FUNCTION = "EMPLOYEE-TEAM-GROUP";
                    else if (model.TeamWorkId.HasValue)
                        FUNCTION = "EMPLOYEE-TEAM";
                    else if (model.WorkGroupId.HasValue)
                        FUNCTION = "EMPLOYEE-GROUP";
                    //MIS بروزرسانی 
                    UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
                    {
                        Domain = model.DomainName,
                        FUNCTION = FUNCTION,
                        NUM_PRSN = employee.EmployeeNumber,
                        NUM_TEAM_EMPL = NUM_TEAM_EMPL,
                        NUM_TEAM_LIST = NUM_TEAM_LIST,
                        DAT_STR_TEAM = DAT_STR_TEAM,
                        STR_TEAM_LIST = model.TransferChangeDate.ToPersianDate().Replace("/", ""),
                        ///
                        COD_TYP_LIST = COD_TYP_LIST,
                        COD_GRP_LIST = COD_GRP_LIST,
                        STR_WORK_LIST = model.TransferChangeDateShift.ToPersianDate().Replace("/", ""),
                    };
                    var resultApiMis = _misUpdateService.UpdateTeamAndGroup(updateTeamAndGroupInputModel);
                    if (resultApiMis.IsError)
                    {
                        throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                    }
                }
                else
                {
                    throw new Exception("تغییری ایجاد نشد");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// تغییر تیم و شیف کاری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task EmployeeTeamWork_WorkGroupTransferManagement(ResultEmployeeTransferModel model)
        {
            KscResult result = new KscResult();
            try
            {



                Employee employee = await _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(model.EmployeeId);
                if (employee == null)
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, Resources.Personal.EmployeeResource.EmployeeNumber));

                if (employee.EmploymentDate.HasValue == false)
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, Resources.Personal.EmployeeResource.EmploymentDate));

                if (employee.EmploymentDate > model.TransferChangeDate)
                    throw new HRBusinessException(Validations.GreaterThan_FieldValue, String.Format(Validations.GreaterThan_FieldValue, "تاریخ احراز", Resources.Personal.EmployeeResource.EmploymentDate));

                string NUM_TEAM_EMPL = string.Empty,
                NUM_TEAM_LIST = string.Empty,
                DAT_STR_TEAM = string.Empty,
                COD_TYP_LIST = string.Empty,
                COD_GRP_LIST = string.Empty,
                STR_WORK_LIST = string.Empty,
                FUNCTION = string.Empty;
                var IsChange = false;
                //
                if (model.TransferRequestId == null)
                    ExistTransferRequestValidation(model);
                // تیم کاری
                if (model.TransferRequestTypeId != EnumRequestType.ShiftTransfer.Id)
                {
                    if (employee.TeamWorkStartDate >= model.TransferChangeDate)
                    {
                        //throw new Exception("تاریخ تغییر وارده برای ثبت اطلاعات مجاز نمی باشد.");
                        throw new Exception(String.Format("کاربر از تاریخ {0} در تیم {1} می باشد،امکان جابه جایی وجود ندارد", employee.TeamWorkStartDate.ToPersianDate(), employee.TeamWork.Code));

                    }
                    var employeeTeamWorkActive = await _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetActiveTeamWorkByEmployeeIdAsync(model.EmployeeId);
                    if (employeeTeamWorkActive != null && employeeTeamWorkActive.TeamWorkId == model.TeamWorkId)
                    {  // nothing تکراری ثبت نشود
                        var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetById(model.TeamWorkId.Value);
                        NUM_TEAM_LIST = teamWork.Code;
                    }
                    else
                    {
                        IsChange = true;
                        if (employeeTeamWorkActive == null)
                        {// برو تیم جدید براش ثبت کن
                         //بعدش employee رو اپدیت کن
                            if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "تیم کاری فعال "));
                        }
                        else
                        {

                            //if(employeeTeamWorkActive.TeamEndDate==model.TransferReturnDate)
                            employeeTeamWorkActive.IsActive = false;
                            if (employeeTeamWorkActive.TeamEndDate == null)
                            {
                                employeeTeamWorkActive.TeamEndDate = model.TransferChangeDate.Value.AddDays(-1);
                            }
                            if (model.IsTransferReturn) // برگشت جابه جایی
                            {
                                employeeTeamWorkActive.TeamEndDate = model.TransferReturnDate;
                            }
                            employeeTeamWorkActive.UpdateUser = model.CurrentUserName;
                            employeeTeamWorkActive.UpdateDate = System.DateTime.Now;

                        }
                        EmployeeTeamWork newEmployeeTeamWork = new EmployeeTeamWork();
                        newEmployeeTeamWork.EmployeeId = model.EmployeeId;
                        newEmployeeTeamWork.IsActive = true;
                        newEmployeeTeamWork.TeamWorkId = model.TeamWorkId.Value;
                        newEmployeeTeamWork.TransferRequestId = model.TransferRequestId;
                        newEmployeeTeamWork.InsertUser = model.CurrentUserName;
                        newEmployeeTeamWork.InsertDate = System.DateTime.Now;
                        if (!model.IsTransferReturn)
                        {
                            newEmployeeTeamWork.TeamStartDate = model.TransferChangeDate.Value;
                        }
                        else
                        {
                            newEmployeeTeamWork.TeamStartDate = employeeTeamWorkActive.TeamEndDate.Value.AddDays(1);
                        }
                        employee.TeamWorkId = model.TeamWorkId;
                        employee.TeamWorkStartDate = newEmployeeTeamWork.TeamStartDate;
                        await _kscHrUnitOfWork.EmployeeTeamWorkRepository.AddAsync(newEmployeeTeamWork);
                        //
                        var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetById(model.TeamWorkId.Value);
                        NUM_TEAM_LIST = teamWork.Code;
                        if (employeeTeamWorkActive != null)
                        {
                            NUM_TEAM_EMPL = employeeTeamWorkActive.TeamWork.Code;//تیم فعلی
                            DAT_STR_TEAM = employeeTeamWorkActive.TeamStartDate.ToPersianDate().Replace("/", "");//تاریخ شروع تیم فعلی
                        }
                    }
                }
                // شیفت کاری
                if (model.TransferRequestTypeId != EnumRequestType.TeamTransfer.Id)
                {
                    if (employee.WorkGroupStartDate >= model.TransferChangeDate && !model.IsTransferReturn)
                    {
                        //  throw new Exception("تاریخ تغییر وارده برای ثبت اطلاعات مجاز نمی باشد.");
                        throw new Exception(String.Format("کاربر از تاریخ {0} در {1} می باشد،امکان جابه جایی وجود ندارد", employee.WorkGroupStartDate.ToPersianDate(), employee.WorkGroup.WorkTime.Title + "-" + employee.WorkGroup.Code));

                    }
                    model.LastWorkGroupId = employee.WorkGroupId;

                    var employeeWorkGroupActive = await _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetActiveWorkGroupByEmployeeIdAsync(model.EmployeeId);
                    if (employeeWorkGroupActive != null && employeeWorkGroupActive.WorkGroupId == model.WorkGroupId)
                    {
                        //nothing داده تکراری ذخیره نشود

                        var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(model.WorkGroupId.Value);
                        COD_TYP_LIST = workGroup.WorkTime.Code.Trim();
                        COD_GRP_LIST = workGroup.Code.Trim();

                    }
                    else
                    {
                        IsChange = true;
                        if (employeeWorkGroupActive == null)
                        {
                            // شیفت جدید بایستی ثبت شود
                            if (employee.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id)
                                throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, "گروه کاری فعال "));
                        }
                        if (employeeWorkGroupActive != null)
                        {
                            employeeWorkGroupActive.IsActive = false;
                            if (employeeWorkGroupActive.EndDate == null)
                            {
                                employeeWorkGroupActive.EndDate = model.TransferChangeDate.Value.AddDays(-1);
                            }
                            else
                            {
                                //
                                if (!model.IsTransferReturn)
                                    throw new Exception("تا برگشت جابه جایی موقت امکان تغییر شیفت وجود ندارد.");
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

                        var lastWorkGroup = model.LastWorkGroupId.HasValue ? _kscHrUnitOfWork.WorkGroupRepository.GetById(model.LastWorkGroupId.Value) : null;
                        var newWorkGroup = _kscHrUnitOfWork.WorkGroupRepository.GetById(model.WorkGroupId.Value);
                        if (lastWorkGroup != null && (lastWorkGroup.WorkTimeId != newWorkGroup.WorkTimeId))
                            newEmployeeWorkGroup.WorkTimeChange = true;
                        employee.WorkGroupId = model.WorkGroupId;
                        employee.WorkGroupStartDate = newEmployeeWorkGroup.StartDate;
                        //
                        //
                        var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking();
                        var hasItemInAnotherWorkTimeId = employeeAttendAbsenceItem
                            .Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 >=
                         newEmployeeWorkGroup.StartDate && x.WorkTimeId != newWorkGroup.WorkTimeId);
                        if (hasItemInAnotherWorkTimeId.Any())
                        {
                            var returnResult = CheckHasItemInAnotherWorkTimeId(hasItemInAnotherWorkTimeId, newWorkGroup, employee, true);
                            if (!returnResult.Success)
                            {
                                if (returnResult.ErrorNumber == 101)
                                {
                                    throw new Exception(string.Join(",", returnResult.Errors));

                                }
                                var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                                throw new Exception($" در تاریخهای {string.Join(",", dates)} در گروه کاری دیگری کارکرد تایید شده دارد");
                            }
                        }

                        //
                        //
                        await _kscHrUnitOfWork.EmployeeWorkGroupRepository.AddAsync(newEmployeeWorkGroup);
                        var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations(model.WorkGroupId.Value);
                        COD_TYP_LIST = workGroup.WorkTime.Code.Trim();
                        COD_GRP_LIST = workGroup.Code.Trim();
                        STR_WORK_LIST = newEmployeeWorkGroup.StartDate.ToPersianDate().Replace("/", "");
                        //
                    }
                }

                //if (model.IsTemporaryTransfer && !model.IsTransferReturn)
                //{
                //    newEmployeeTeamWork.TeamEndDate = model.TransferReturnDate;
                //}

                //
                if (IsChange == true)
                {
                    FUNCTION = model.TransferRequestTypeId == EnumRequestType.ShiftTransfer.Id ? "EMPLOYEE-GROUP" : model.TransferRequestTypeId == EnumRequestType.TeamTransfer.Id ? "EMPLOYEE-TEAM" : "EMPLOYEE-TEAM-GROUP";
                    if (string.IsNullOrEmpty(STR_WORK_LIST))
                        STR_WORK_LIST = model.TransferChangeDate.ToPersianDate().Replace("/", "");
                    //MIS بروزرسانی 
                    UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
                    {
                        Domain = model.DomainName,
                        FUNCTION = FUNCTION,
                        NUM_PRSN = employee.EmployeeNumber,
                        NUM_TEAM_EMPL = NUM_TEAM_EMPL,
                        NUM_TEAM_LIST = NUM_TEAM_LIST,
                        DAT_STR_TEAM = DAT_STR_TEAM,
                        STR_TEAM_LIST = model.TransferChangeDate.ToPersianDate().Replace("/", ""),
                        ///
                        COD_TYP_LIST = COD_TYP_LIST,
                        COD_GRP_LIST = COD_GRP_LIST,
                        STR_WORK_LIST = STR_WORK_LIST,
                    };
                    var resultApiMis = _misUpdateService.UpdateTeamAndGroup(updateTeamAndGroupInputModel);
                    if (resultApiMis.IsError)
                    {
                        throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        //public KscResult CheckDate(DateTime date)
        //{
        //    var result= new KscResult();    
        //    var hasemployeeAttendAbsence = employeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 >= date)
        //               .Select(x => new { DateKey = x.WorkCalendar.DateKey, Date = x.WorkCalendar.ShamsiDateV1 });

        //    if (hasemployeeAttendAbsence.Any())
        //    {
        //        var dates = hasemployeeAttendAbsence.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
        //        result.AddError("خطا", $" تا تاریخ {string.Join(",", dates.Last())}  در شیفت " + employeeWorkGroup.WorkGroup.WorkTime.Title + "  تایید کارکرد دارد. برای  تغییر شیفت کاری از دکمه جابه جایی شیفت و تیم استفاده کنید. ");
        //        return result;
        //    }
        //}
        /// <summary>
        /// تغییر شیفت کای 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<KscResult> UpdateEmployeeWorkShiftGroup(EmployeeWorkGroupModel model)
        {
            KscResult result = new KscResult();
            try
            {
                model.FromDate = !string.IsNullOrWhiteSpace(model.PersianFromDate) ? model.PersianFromDate.ToGorgianDateKscResult().Data.Date : null;
                model.ToDate = !string.IsNullOrWhiteSpace(model.PersianToDate) ? model.PersianToDate.ToGorgianDateKscResult().Data.Date : null;
                Employee employee = await _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(model.EmployeeId);
                if (employee == null)
                    throw new HRBusinessException(Validations.NotFoundId, String.Format(Validations.NotFound, Resources.Personal.EmployeeResource.EmployeeNumber));

                var employeeWorkGroup = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetEmployeeWorkGroupIncludeTransferRequest().First(x => x.Id == model.Id);
                if (!employeeWorkGroup.IsActive)
                {
                    result.AddError("", "شیفت کاری فعال نمی باشد");
                    return result;
                }
                //

                //
                if (model.ToDate.HasValue && model.FromDate.Value.Date > model.ToDate.Value.Date)
                {
                    result.AddError("", "تاریخ شروع شیفت نباید بزرگتر از تاریخ پایان شیفت باشد");
                    return result;
                }
                //
                var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking();
                var selectedWorkGroup = _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByWorkTimeRelations_N(model.WorkGroupId);
                var newWorkGroup = _kscHrUnitOfWork.WorkGroupRepository.GetById(model.WorkGroupId);
                if (employeeWorkGroup.WorkGroup.WorkTimeId != selectedWorkGroup.WorkTimeId) // در صورتیکه زمان کاری تغییر کند
                {
                    //var employeeAttendAbsenceItem = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetEmployeeAttendAbsenceItemIncludWorkCalendarAsNoTracking();
                    var hasemployeeAttendAbsence = employeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 >= model.FromDate);
                    //.Select(x => new { DateKey = x.WorkCalendar.DateKey, Date = x.WorkCalendar.ShamsiDateV1 });

                    if (hasemployeeAttendAbsence.Any())
                    {
                        var returnResult = CheckHasItemInAnotherWorkTimeId(hasemployeeAttendAbsence, newWorkGroup, employee);
                        if (!returnResult.Success)
                        {
                            if (returnResult.ErrorNumber == 101)
                            {
                                result.AddError("خطا", (string.Join(",", returnResult.Errors)));
                                return result;
                            }
                            var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                            result.AddError("خطا", $" تا تاریخ {string.Join(",", dates.Last())}  در شیفت " + employeeWorkGroup.WorkGroup.WorkTime.Title + "  تایید کارکرد دارد. برای  تغییر شیفت کاری از دکمه جابه جایی شیفت و تیم استفاده کنید. ");
                            return result;
                        }

                    }
                }
                else
                {
                    var hascurrentemployeeAttendAbsence = employeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 >= model.FromDate.Value && x.WorkTimeId != employeeWorkGroup.WorkGroup.WorkTimeId);
                    // .Select(x => new { DateKey = x.WorkCalendar.DateKey, Date = x.WorkCalendar.ShamsiDateV1, WorkTimeId = x.WorkTimeId });

                    if (hascurrentemployeeAttendAbsence.Any())
                    {

                        var returnResult = CheckHasItemInAnotherWorkTimeId(hascurrentemployeeAttendAbsence, newWorkGroup, employee);
                        if (!returnResult.Success)
                        {
                            if (returnResult.ErrorNumber == 101)
                            {
                                result.AddError("خطا", (string.Join(",", returnResult.Errors)));
                                return result;
                            }
                            var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => new { Date = x.Date, WorkTimeId = x.WorkTimeId }).ToList().Distinct();
                            var lastWorkTimeTitle = _kscHrUnitOfWork.WorkTimeRepository.GetById(dates.Last().WorkTimeId).Title;
                            result.AddError("خطا", $" تا تاریخ {string.Join(",", dates.Last().Date)}  در شیفت " + lastWorkTimeTitle + "  تایید کارکرد دارد.  ");
                            return result;
                        }
                    }
                }
                if (employeeWorkGroup.StartDate.Date < model.FromDate.Value.Date)
                {
                    var hascurrentemployeeAttendAbsence = employeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 >= employeeWorkGroup.StartDate && x.WorkCalendar.MiladiDateV1 < model.FromDate.Value);
                    //.Select(x => new { DateKey = x.WorkCalendar.DateKey, Date = x.WorkCalendar.ShamsiDateV1 });

                    if (hascurrentemployeeAttendAbsence.Any())
                    {
                        var returnResult = CheckHasItemInAnotherWorkTimeId(hascurrentemployeeAttendAbsence, newWorkGroup, employee, true);
                        if (!returnResult.Success)
                        {
                            if (returnResult.ErrorNumber == 101)
                            {
                                result.AddError("خطا", (string.Join(",", returnResult.Errors.Select(x => x.Message))));
                                return result;
                            }
                            var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                            result.AddError("خطا", $" تا تاریخ {string.Join(",", dates.Last())}  در شیفت " + employeeWorkGroup.WorkGroup.WorkTime.Title + "  تایید کارکرد دارد. برای  تغییر شیفت کاری از دکمه جابه جایی شیفت و تیم استفاده کنید. ");
                            return result;
                        }
                    }
                }

                if (model.ToDate.HasValue && employeeWorkGroup.EndDate.HasValue && employeeWorkGroup.EndDate.Value.Date > model.ToDate.Value.Date)
                {
                    var hascurrentemployeeAttendAbsence = employeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId && x.WorkCalendar.MiladiDateV1 > model.ToDate.Value.Date);
                    //.Select(x => new { DateKey = x.WorkCalendar.DateKey, Date = x.WorkCalendar.ShamsiDateV1 });

                    if (hascurrentemployeeAttendAbsence.Any())
                    {
                        var returnResult = CheckHasItemInAnotherWorkTimeId(hascurrentemployeeAttendAbsence, newWorkGroup, employee);
                        if (!returnResult.Success)
                        {
                            if (returnResult.ErrorNumber == 101)
                            {
                                result.AddError("خطا", (string.Join(",", returnResult.Errors)));
                                return result;
                            }
                            var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                            result.AddError("خطا", $" تا تاریخ {string.Join(",", dates.Last())}  در شیفت " + employeeWorkGroup.WorkGroup.WorkTime.Title + "  تایید کارکرد دارد. برای  تغییر شیفت کاری از دکمه جابه جایی شیفت و تیم استفاده کنید. ");
                            return result;
                        }
                    }
                }
                //
                var transresult = ExistTransferRequestByEmployeeId(model.EmployeeId, EnumRequestType.ShiftTransfer.Id);
                if (!transresult.Success)
                {
                    result.AddErrors(transresult.Errors);
                    return result;
                }
                //_kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.Any(x=>x.StartDate>=model.FromDate && x.EndDate<=)

                if (!employeeWorkGroup.EndDate.HasValue && model.ToDate.HasValue)
                {
                    result.AddError("خطا", "تاریخ پایان امکان ویرایش ندارد");
                    return result;
                }
                var employeeWorkGroupDeActive = _kscHrUnitOfWork.EmployeeWorkGroupRepository.GetDeActiveWorkGroupByEmployeeId(model.EmployeeId);
                if (employeeWorkGroupDeActive.Any())
                {
                    var endDate = employeeWorkGroup.StartDate.AddDays(-1);
                    var lastemployeeWorkGroup = employeeWorkGroupDeActive.Where(x => x.EndDate.HasValue).OrderBy(x => x.Id).ToList().LastOrDefault(x => x.EndDate.Value.Date == endDate.Date);
                    if (lastemployeeWorkGroup == null)
                    {
                        result.AddError("خطا", "اطلاعات آخرین گروه کاری غیر فعال یافت نشد");
                        return result;
                    }
                    //
                    if (lastemployeeWorkGroup.WorkGroup.WorkTimeId != selectedWorkGroup.WorkTimeId)
                    {
                        var lastemployeeWorkGroupEndDate = model.FromDate.Value.AddDays(-1);
                        if (lastemployeeWorkGroup.EndDate < lastemployeeWorkGroupEndDate)
                        {
                            var hascurrentemployeeAttendAbsence = employeeAttendAbsenceItem.Where(x => x.EmployeeId == model.EmployeeId &&
                            x.WorkCalendar.MiladiDateV1 > lastemployeeWorkGroup.EndDate.Value.Date && x.WorkCalendar.MiladiDateV1 <= lastemployeeWorkGroupEndDate);

                            if (hascurrentemployeeAttendAbsence.Any())
                            {
                                var returnResult = CheckHasItemInAnotherWorkTimeId(hascurrentemployeeAttendAbsence, lastemployeeWorkGroup.WorkGroup, employee);
                                if (!returnResult.Success)
                                {
                                    if (returnResult.ErrorNumber == 101)
                                    {
                                        result.AddError("خطا", (string.Join(",", returnResult.Errors)));
                                        return result;
                                    }
                                    var dates = returnResult.ItemForChangeShiftModel.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                                    var lastWorkTimeTitle = _kscHrUnitOfWork.WorkTimeRepository.GetById(lastemployeeWorkGroup.WorkGroup.WorkTimeId).Title;

                                    result.AddError("خطا", $" تا تاریخ {string.Join(",", dates.Last())}  در شیفت " + employeeWorkGroup.WorkGroup.WorkTime.Title + "  تایید کارکرد دارد. برای  تغییر شیفت کاری از دکمه جابه جایی شیفت و تیم استفاده کنید. ");
                                    return result;
                                }
                            }
                        }
                    }
                    //
                    if (model.FromDate.Value.Date <= lastemployeeWorkGroup.StartDate.Date)
                    {
                        result.AddError("", "تاریخ شروع شیفت انتخابی نباید کوچکتر یا مساوی از تاریخ شروع شیفت قبلی باشد");
                        return result;
                    }
                    //
                    lastemployeeWorkGroup.EndDate = model.FromDate.Value.AddDays(-1);
                    if (lastemployeeWorkGroup.EndDate < lastemployeeWorkGroup.StartDate.Date)
                    {
                        result.AddError("", "خطایی در تنظیم تاریخ شیفت قبلی بوجود آمده است");
                        return result;
                    }

                }

                if (employeeWorkGroup.TransferRequestId.HasValue && employeeWorkGroup.EndDate.HasValue &&
                    employeeWorkGroup.Transfer_Request.WF_Request.StatusId == EnumWorkFlowStatus.TemporaryTransferConfirm.Id)
                {
                    if (!model.ToDate.HasValue)
                    {
                        result.AddError("خطا", $"با توجه به اینکه درخواست جابجایی موقت وجود دارد،باید تاریخ پایان را مقدار دهید");
                        return result;
                    }
                    if (employeeWorkGroup.Transfer_Request.RequestdWorkGroup.WorkTimeId != selectedWorkGroup.WorkTimeId)
                    {
                        result.AddError("خطا", $"کاربر در شیفت {employeeWorkGroup.Transfer_Request.RequestdWorkGroup.WorkTime.Title}-{employeeWorkGroup.Transfer_Request.RequestdWorkGroup.Code} درخواست جابه جایی موقت دارد");
                        return result;
                    }
                    employeeWorkGroup.Transfer_Request.TransferReturnDate = model.ToDate.Value;
                }
                employeeWorkGroup.WorkGroupId = model.WorkGroupId;
                // employee
                employee.WorkGroupId = model.WorkGroupId;
                employee.WorkGroupStartDate = model.FromDate.Value;
                employee.UpdateUser = model.UserName;
                employee.UpdateDate = DateTime.Now;
                //
                employeeWorkGroup.StartDate = model.FromDate.Value;
                employeeWorkGroup.EndDate = model.ToDate;
                //MIS بروزرسانی 
                string NUM_TEAM_EMPL = string.Empty,
                NUM_TEAM_LIST = string.Empty,
                DAT_STR_TEAM = string.Empty,
                COD_TYP_LIST = string.Empty,
                COD_GRP_LIST = string.Empty,
                STR_WORK_LIST = string.Empty,
                FUNCTION = string.Empty;
                COD_TYP_LIST = selectedWorkGroup.WorkTime.Code.Trim();
                COD_GRP_LIST = selectedWorkGroup.Code.Trim();
                STR_WORK_LIST = employeeWorkGroup.StartDate.ToPersianDate().Replace("/", "");
                FUNCTION = "EMPLOYEE-GROUP";

                UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
                {
                    Domain = model.DomainName,
                    FUNCTION = FUNCTION,
                    NUM_PRSN = employee.EmployeeNumber,
                    NUM_TEAM_EMPL = NUM_TEAM_EMPL,
                    NUM_TEAM_LIST = NUM_TEAM_LIST,
                    DAT_STR_TEAM = DAT_STR_TEAM,
                    //STR_TEAM_LIST = model.TransferChangeDate.ToPersianDate().Replace("/", ""),
                    ///
                    COD_TYP_LIST = COD_TYP_LIST,
                    COD_GRP_LIST = COD_GRP_LIST,
                    STR_WORK_LIST = STR_WORK_LIST,
                };
                var resultApiMis = _misUpdateService.UpdateTeamAndGroup(updateTeamAndGroupInputModel);
                if (resultApiMis.IsError)
                {
                    var message = String.Format("خطای MIS-{0}", resultApiMis.MsgError);
                    result.AddError("خطا", message);

                }

                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return result;

        }

        private void ExistTransferRequestValidation(ResultEmployeeTransferModel model)
        {
            var transferRequest = _kscHrUnitOfWork.Transfer_RequestRepository.GetTransferRequestByRelated().Where(x => x.WF_Request.EmployeeId == model.EmployeeId);
            if (transferRequest.Any())
            {
                if (model.TransferRequestTypeId == EnumRequestType.ShiftTransfer.Id || (model.TransferRequestId == null && model.WorkGroupId.HasValue))
                {
                    ExistTransferRequestByTransferRequestTypeId(transferRequest, EnumRequestType.ShiftTransfer.Id);
                }
                if (model.TransferRequestTypeId == EnumRequestType.TeamTransfer.Id || (model.TransferRequestId == null && model.TeamWorkId.HasValue))
                {
                    ExistTransferRequestByTransferRequestTypeId(transferRequest, EnumRequestType.TeamTransfer.Id);
                }
            }

        }

        private void ExistTransferRequestByTransferRequestTypeId(IQueryable<Transfer_Request> transferRequest, int transferRequestTypeId)
        {
            transferRequest = transferRequest.Where(x => x.Transfer_RequestReasonType.TransferRequestTypeId == transferRequestTypeId);
            if (transferRequest.Any())
            {
                var lastRequest = transferRequest.OrderByDescending(o => o.Id).First();
                var statusProcessManagement = _kscHrUnitOfWork.WF_StatusProcessManagementRepository.GetActiveWorkFlowStatusProcessManagementByProcessIdAndStatusId(lastRequest.WF_Request.ProcessId, lastRequest.WF_Request.StatusId);
                if (statusProcessManagement != null)
                    if (statusProcessManagement.IsFirstStatus || statusProcessManagement.IsLastStatus == false)
                        throw new Exception("درخواست جابجایی وجود دارد،باید از طرق کارتابل جابجایی تیم و شیفت ، درخواست بررسی شود");
            }

        }
        private KscResult ExistTransferRequestByEmployeeId(int employeeId, int transferRequestTypeId)
        {
            var result = new KscResult();
            var transferRequest = _kscHrUnitOfWork.Transfer_RequestRepository.GetTransferRequestByRelated()
                .Where(x => x.WF_Request.EmployeeId == employeeId &&
                            x.Transfer_RequestReasonType.TransferRequestTypeId == transferRequestTypeId
            );
            if (transferRequest.Any())
            {
                var lastRequest = transferRequest.OrderByDescending(o => o.Id).First();
                if (lastRequest.WF_Request.StatusId == EnumWorkFlowStatus.TransferShiftTeamCreate.Id)
                    result.AddError("", "درخواست جابجایی وجود دارد،باید از طرق کارتابل جابجایی تیم و شیفت ، درخواست بررسی شود");
            }
            return result;
        }

        public EmployeeModel GetOneBySearchModelWindUser(string currentName)
        {
            var teamCodesUserWindows = _kscHrUnitOfWork.ViewMisUserDefinitionRepository
               .WhereQueryable(a => a.WinUser.ToLower() == currentName.ToLower())
               .Select(a => new
               {
                   a.PersonalNumber,

               }
               ).FirstOrDefault();
            if (teamCodesUserWindows == null)
            {
                return new EmployeeModel();
            }

            var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamWorkByEmployeeId(teamCodesUserWindows.PersonalNumber.ToString());

            var result = new EmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Family = employee.Family,
                EmployeeNumber = employee.EmployeeNumber,
                TeamWorkTitle = employee.TeamWork.Title,
                TeamWorkCode = employee.TeamWork.Code,


            };

            return result;
        }

        public FilterResult<SearchEmployeeModel> GetUserWindowsEmployeeList(SearchEmployeeModel Filter)
        {
            var teamCodesUserWindows = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
                .Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName)
                .Select(a => a.TeamCode.ToString()).ToList();

            var activePersonsId = _kscHrUnitOfWork.EmployeeTeamWorkRepository.GetAllQueryable()
                .AsQueryable().Include(x => x.TeamWork)
                .Where(a => a.IsActive == true && teamCodesUserWindows.Contains(a.TeamWork.Code))
                .Select(a => a.EmployeeId).ToList();

            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeIncludedTeamwork(activePersonsId).AsQueryable().AsNoTracking();

            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var modelResult = new FilterResult<SearchEmployeeModel>
            {
                Data = _mapper.Map<List<SearchEmployeeModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }

        public FilterResult<IntroductionModel> GetUsersForIntroductionHR(SearchEmployeeModel Filter)
        {


            var query = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeWorkGroupWorkTimeByRelated().AsQueryable().AsNoTracking();
            if (Filter.EmployeeNumbers.Any())
            {
                query = query.Where(a => Filter.EmployeeNumbers.Contains(a.EmployeeNumber));
            }
            else
            {
                return new FilterResult<IntroductionModel>();
            }
            FilterResult<Employee> result = _FilterHandler.GetFilterResult<Employee>(query, Filter, nameof(Employee.Id));
            var finalList = result.Data.Select(a => new IntroductionModel()
            {
                Name = a.Name,
                Family = a.Family,
                WorkTimeTitle = a?.WorkGroup?.WorkTime.Title,
                PersonId = a.EmployeeNumber

            }).ToList();

            var modelResult = new FilterResult<IntroductionModel>
            {
                Data = finalList,
                Total = result.Total
            };
            return modelResult;
        }


        #region EmployeeBreastfedding - فرجه شیر دهی
        public async Task<EmployeeBreastfeddingModel> GetEmployeeBreastFedding(int id)
        {
            var employeedata = await _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeBreastfeddingModel>(employeedata);
        }
        public async Task<KscResult> PostEmployeeBreastFedding(EmployeeBreastfeddingModel model)
        {
            var result = model.IsValid();
            try
            {
                if (!result.Success)
                    return result;
                if (model.BreastfeddingStartDate > model.BreastfeddingEndDate)
                {
                    result.AddError("", "تاریخ شروع بزرگتر از تاریخ پایان می باشد");
                    return result;
                }
                var employeedata = await _kscHrUnitOfWork.EmployeeRepository.GetByIdAsync(model.EmployeeId);
                if (!(employeedata.Gender == EnumEmployeeGenderType.Woman.Id && (employeedata.MaritalStatusId == EnumEmployeeMaritalStatus.Married.Id || employeedata.MaritalStatusId == EnumEmployeeMaritalStatus.Lone.Id)))
                    throw new HRBusinessException("", EmployeeResource.ErrorBreastfeddingEmployee);
                if (employeedata.IsBreastfeddingInStartShift == model.IsBreastfeddingInStartShift && employeedata.BreastfeddingStartDate == model.BreastfeddingStartDate && employeedata.BreastfeddingEndDate == model.BreastfeddingEndDate)
                    throw new HRBusinessException("", Errors.DuplicationError);
                employeedata = _mapper.Map(model, employeedata);
                _kscHrUnitOfWork.EmployeeRepository.Update(employeedata);
                _kscHrUnitOfWork.EmployeeBreastfeddingRepository.ChangeDeActiveEmployeeBreastfeddingsById(employeedata.Id);
                var employeeBreastfedding = _mapper.Map<EmployeeBreastfedding>(model);
                employeeBreastfedding.InsertDate = DateTime.Now;
                employeeBreastfedding.InsertUser = model.CurrentUser;
                employeeBreastfedding.IsActive = true;
                await _kscHrUnitOfWork.EmployeeBreastfeddingRepository.AddAsync(employeeBreastfedding);
                await _kscHrUnitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {

                result.AddError("خطا", ex.Message);
                return result;
            }
        }
        #endregion

        public bool IsUserTmMa(string currentUser)
        {
            return _kscHrUnitOfWork.ViewMisEmployeeRepository.Any(c => (c.JobCategoryCode == "TM" || c.JobCategoryCode == "MA") && c.WinUser == currentUser);

        }

        public bool IsUserTm(string currentUser)
        {
            return _kscHrUnitOfWork.ViewMisEmployeeRepository.Any(c => (c.JobCategoryCode == "TM") && c.WinUser == currentUser);

        }


        #region EmployeeSacrifice
        public async Task<EmployeeSacrificeDto> GetEmployeeSacrifice(int id)
        {
            var model = await _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeId(id);
            var employeeData = _mapper.Map<EmployeeSacrificeDto>(model);

            return employeeData;
        }
        public async Task<KscResult> PostEmployeeSacrifice(EmployeeSacrificeDto model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try
            {

                var employee = GetOne(model.Id);

                //if (EqualsObject.ValueEquals<Employee, EmployeeSacrificeDto>(employee, model,
                //  x => x.Id, x => x.SacrificeOptionSettingId, x => x.SacrificePercentage))
                //{
                //    result.AddError("رکورد یکسان", "رکورد تغییر نکرده است");
                //    return result;
                //}

                employee.SacrificeOptionSettingId = model.SacrificeOptionSettingId;
                employee.SacrificePercentage = model.SacrificePercentage;
                employee.IsarStatusId = model.IsarStatusId;

                _kscHrUnitOfWork.EmployeeRepository.Update(employee);
                #region SyncMIS
                var condotionModel = new EmployeeConditionModel()
                {
                    EmployeeNumber = employee.EmployeeNumber,
                    SacrificeOptionSettingId = model.SacrificeOptionSettingId,
                    SacrificePercentage = model.SacrificePercentage,
                    IsarStatusId = model.IsarStatusId,
                    P_function = "IsarStatus",
                    PaymentStatusId = employee.PaymentStatusId.HasValue ? employee.PaymentStatusId.Value : 0
                };

                var updateMIS = _misUpdateService.UpdateEmployeeConditionModel(condotionModel);
                if (updateMIS.IsSuccess == false)
                {
                    result.AddError("خطا MIS", $"خطا MIS - {string.Join(",", updateMIS.Messages)}");
                    return result;
                }
                #endregion

                await _kscHrUnitOfWork.SaveAsync();

            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }
        #endregion

        #region EmployeeCategoryCoefficient
        public async Task<EmployeeCategoryCoefficientDto> GetEmployeeCategoryCoefficient(int id)
        {
            var model = await _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeId(id);
            var employeeData = _mapper.Map<EmployeeCategoryCoefficientDto>(model);

            return employeeData;
        }
        public async Task<KscResult> PostEmployeeCategoryCoefficient(EmployeeCategoryCoefficientDto model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try
            {

                var employee = GetOne(model.Id);

                employee.CategoryCoefficientId = model.CategoryCoefficientId;

                _kscHrUnitOfWork.EmployeeRepository.Update(employee);


                await _kscHrUnitOfWork.SaveAsync();

            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }
        #endregion
        public KscResult UpploadPicToEmployeePic()
        {
            var result = new KscResult();
            try
            {
                string path = @"E:\image\2\New folder";
                var ImgeFiles = Directory.GetFiles(path);
                var AllListEmployee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().ToList();
                foreach (var imgFile in ImgeFiles)
                {

                    var fileName = Path.GetFileName(imgFile).Split(".")[0];
                    var type = Path.GetFileName(imgFile).Split(".")[1];
                    var employee = AllListEmployee.FirstOrDefault(a => a.EmployeeNumber == fileName);
                    if (employee == null)
                    {

                        //File.AppendAllText(@"E:\11\2.txt", fileName + Environment.NewLine);
                        continue;
                    }


                    var fileNameGuid = Guid.NewGuid().ToString();

                    if (type.Contains("jpg") || type.Contains("jpeg"))
                    {

                        fileName = $"{fileNameGuid}.jpg";
                    }
                    else
                    {
                        fileName = $"{fileNameGuid}.png";
                    }
                    var fileGuId = _fileSystemStorageService.SaveFile(new KSC.DMS.Dto.FileUploadDto
                    {
                        Content = System.IO.File.ReadAllBytes(imgFile),
                        Name = fileName,
                        Size = System.IO.File.ReadAllBytes(imgFile).Length
                    });

                    employee.GuidPicId = fileNameGuid;
                    var file = _attachmentService.AttachFile(new KSC.DMS.Dto.AddApplicationFileDto
                    {
                        ApplicationName = "HR",
                        EntityName = "Employee",
                        EntityKey = employee.Id.ToString(),
                        Title = employee.Name + ' ' + employee.Family,
                        Description = "عکس پرسنلی",
                        File = fileGuId,
                        FileName = fileName,
                        UserName = "sys",
                    });
                    _kscHrUnitOfWork.EmployeeRepository.Update(employee);
                    //_kscHrUnitOfWork.EmployeePictureRepository.Add(new EmployeePicture()
                    //{
                    //    EmployeeId= employee.Id,
                    //    PersonalNumber = employee.EmployeeNumber,
                    //    Image = File.ReadAllBytes(imgFile),
                    //    InsertDate=DateTime.Now,
                    //    InsertUser="Sys"
                    //});
                }

                _kscHrUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                result.AddError("", "");
            }

            return result;
        }

        private string ImageToBase64(string imagePath)
        {
            var imgbyte = System.IO.File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imgbyte);
            return base64String;
        }
        private ItemForChangeShiftResultModel CheckHasItemInAnotherWorkTimeId(IQueryable<EmployeeAttendAbsenceItem> employeeAttendAbsenceItem, WorkGroup newWorkGroup, Employee employee, bool onlyCheck = false)
        {
            ItemForChangeShiftResultModel result = new ItemForChangeShiftResultModel() { ItemForChangeShiftModel = new List<ItemForChangeShiftModel>() };
            var hasItemInAnotherWorkTimeId = employeeAttendAbsenceItem.Select(x => new ItemForChangeShiftModel
            {
                DateKey = x.WorkCalendar.DateKey,
                Date = x.WorkCalendar.ShamsiDateV1,
                WorkCalendarId = x.WorkCalendarId,
                EmployeeAttendAbsenceItemId = x.Id,
                MissionRequestId = x.MissionRequestId,
                EmployeeLongTermAbsenceId = x.EmployeeLongTermAbsenceId,
                EvaluationDevelopmentId = x.EvaluationDevelopmentId,
                WorkTimeId = x.WorkTimeId,
                SystemSequenceStatusId = x.WorkCalendar.SystemSequenceStatusId

            }).ToList();
            result.ItemForChangeShiftModel.AddRange(hasItemInAnotherWorkTimeId);

            if (hasItemInAnotherWorkTimeId.Count() != hasItemInAnotherWorkTimeId.Count(x => x.EmployeeLongTermAbsenceId != null || x.MissionRequestId != null || x.EvaluationDevelopmentId != null))
            {
                result.AddError("خطا", "خطا");
                return result;
                //var dates = hasItemInAnotherWorkTimeId.OrderBy(x => x.DateKey).Select(x => x.Date).ToList().Distinct();
                //throw new Exception($" در تاریخهای {string.Join(",", dates)} در گروه کاری دیگری کارکرد تایید شده دارد");
            }
            var inActiveSystemSequenceStatus = hasItemInAnotherWorkTimeId.Where(x => x.SystemSequenceStatusId != null &&
             !ConstSystemSequenceStatusDailyTimeSheet.ValidSystemStatusForInsertMissionInItem.Any(s => s == x.SystemSequenceStatusId));

            if (inActiveSystemSequenceStatus.Any())
            {
                var dates = string.Join("-", hasItemInAnotherWorkTimeId.Select(x => x.Date).ToList());
                result.AddError("خطا", $"در تاریخهای {dates} سیستم کارکرد بسته می باشد");
                result.ErrorNumber = 101;
                return result;
            }
            if (onlyCheck)
            {
                return result;
            }
            // 
            List<EmployeeAttendAbsenceItemChangeShiftModel> employeeAttendAbsenceItemModel = new List<EmployeeAttendAbsenceItemChangeShiftModel>();
            if (hasItemInAnotherWorkTimeId.Any())
            {
                var newWorkTime = _kscHrUnitOfWork.WorkTimeRepository.GetById(newWorkGroup.WorkTimeId);
                var timeShiftSettingByWorkCityIdModel = _kscHrUnitOfWork.TimeShiftSettingRepository.GetTimeShiftSettingByWorkCityId(employee.WorkCityId.Value).Where(x => !x.IsTemporaryTime && x.WorktimeId == newWorkGroup.WorkTimeId).ToList();
                if (newWorkTime.ShiftSettingFromShiftboard)
                {
                    var WorkCalendarIds = hasItemInAnotherWorkTimeId.Select(x => x.WorkCalendarId).Distinct();
                    var shiftBoard = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardIncludeShiftConceptDetail();
                    employeeAttendAbsenceItemModel = shiftBoard.Where(x => x.WorkGroupId == newWorkGroup.Id && WorkCalendarIds.Any(c => c == x.WorkCalendarId)).ToList()
                        .Select(x => new EmployeeAttendAbsenceItemChangeShiftModel()
                        {
                            ShiftConceptDetailId = x.ShiftConceptDetailId,
                            StartTime = timeShiftSettingByWorkCityIdModel.First(t => t.ShiftConceptId == x.ShiftConceptDetail.ShiftConceptId).ShiftStartTime,
                            EndTime = timeShiftSettingByWorkCityIdModel.First(t => t.ShiftConceptId == x.ShiftConceptDetail.ShiftConceptId).ShiftEndtTime,
                            WorkCalendarId = x.WorkCalendarId,
                            TotalWorkHourInDay = timeShiftSettingByWorkCityIdModel.First(t => t.ShiftConceptId == x.ShiftConceptDetail.ShiftConceptId).TotalWorkHourInDay
                        }).ToList();
                }
                else
                {
                    var timeShiftSetting = timeShiftSettingByWorkCityIdModel.First(x => x.ShiftConceptId == EnumShiftConcept.FixedDay.Id);
                    var shiftConceptDetailId = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable().First(x => x.IsActive && x.ShiftConceptId == EnumShiftConcept.FixedDay.Id).Id;
                    employeeAttendAbsenceItemModel.Add(new EmployeeAttendAbsenceItemChangeShiftModel()
                    {
                        ShiftConceptDetailId = shiftConceptDetailId,
                        StartTime = timeShiftSetting.ShiftStartTime,
                        EndTime = timeShiftSetting.ShiftEndtTime,
                        TotalWorkHourInDay = timeShiftSetting.TotalWorkHourInDay


                    });
                }
                foreach (var item in hasItemInAnotherWorkTimeId)
                {
                    int shiftConceptDetailId = 0;
                    string startTime = string.Empty;
                    string endTime = string.Empty;
                    string totalWorkHourInDay = string.Empty;
                    int workCalendarId = 0;
                    if (!newWorkTime.ShiftSettingFromShiftboard)
                    {
                        var itemModel = employeeAttendAbsenceItemModel.First();
                        shiftConceptDetailId = itemModel.ShiftConceptDetailId;
                        startTime = itemModel.StartTime;
                        endTime = itemModel.EndTime;
                        workCalendarId = item.WorkCalendarId;
                        totalWorkHourInDay = itemModel.TotalWorkHourInDay;
                    }
                    else
                    {
                        var itemModel = employeeAttendAbsenceItemModel.First(x => x.WorkCalendarId == item.WorkCalendarId);
                        shiftConceptDetailId = itemModel.ShiftConceptDetailId;
                        startTime = itemModel.StartTime;
                        endTime = itemModel.EndTime;
                        workCalendarId = itemModel.WorkCalendarId;
                        totalWorkHourInDay = itemModel.TotalWorkHourInDay;
                    }
                    var employeeAttendAbsenceItemData = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository.GetById(item.EmployeeAttendAbsenceItemId);
                    employeeAttendAbsenceItemData.WorkTimeId = newWorkGroup.WorkTimeId;
                    employeeAttendAbsenceItemData.ShiftConceptDetailId = shiftConceptDetailId;
                    employeeAttendAbsenceItemData.ShiftConceptDetailIdInShiftBoard = shiftConceptDetailId;
                    employeeAttendAbsenceItemData.StartTime = startTime;
                    employeeAttendAbsenceItemData.EndTime = endTime;
                    employeeAttendAbsenceItemData.TimeDuration = totalWorkHourInDay;

                }
            }
            return result;
        }

        public EmployeeInfoModel GetEmployeeInfoByNumber(string employeeNumber)
        {
            var personel = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByPersonalNumIncluded(employeeNumber);
            var model = new EmployeeInfoModel()
            {
                FirstName = personel.Name,
                LastName = personel.Family,
                NationalCode = personel.NationalCode,
                EmployeeNumber = personel.EmployeeNumber,
                BirthDate = personel.BirthDate,
                PhoneNumber = personel.PhoneNumber,
                ShiftTitle = personel.WorkGroup.Code,
                Address = personel.HomeAddress,
                EmploymentTypeId = personel.EmploymentTypeId,
                TeamWorkId = personel.TeamWork.Title,
                TeamWorkCode = personel.TeamWork.Code,
                TeamWorkTitle = personel.TeamWork.Title,
                JobPositionId = personel.JobPositionId,
                JobPositionCode = personel.JobPositionCode,
                WorkCityId = personel.WorkCityId,
                WorkGroupId = personel.WorkGroupId,
                WorktimeId = personel.WorkGroup.WorkTimeId,
                
            };
            return model;
        }


        public bool IsActiveNationalCode(string nationalCode)
        {
            bool result = false;

            var personel = _kscHrUnitOfWork.EmployeeRepository.GetByNationalCode(nationalCode)
                .Any(x => x.PaymentStatusId == EnumPaymentStatus.CurrentEmployee.Id);

            var family = _kscHrUnitOfWork.FamilyRepository.GetByNationalCode(nationalCode)
                .Include(x => x.Employee).Any(x => x.Employee.PaymentStatusId == EnumPaymentStatus.CurrentEmployee.Id)
                ;

            result = personel || family;

            return result;
        }

        public EmployeeDetailsModel GetEmployeeDetails(int employeeId)
        {
            var personel = _kscHrUnitOfWork.EmployeeRepository.GetEmployeeByEmployeeIdIncludedByDetailed(employeeId);

            //var chart = new Chart_JobPosition();
            //chart = _kscHrUnitOfWork.Chart_JobPositionRepository.GetChart_JobPositionById(personel.JobPositionId ?? 0).AsNoTracking().FirstOrDefault();
            var result = new EmployeeDetailsModel()
            {
                Id = personel.Id,
                Name = personel.Name,
                Family = personel.Family,
                EmployeeNumber = int.Parse(personel.EmployeeNumber),
                EmploymentTypeId = personel.EmploymentTypeId.Value,
                EmploymentTypeTitle = personel.EmploymentType.Title,
                EmploymentDate = personel.EmploymentDate,
                EmploymentDatePersian = personel.EmploymentDate.ToPersianDateTimeString("yyyy/MM/dd"),
                PaymentStatusId = personel.PaymentStatusId,
                PaymentStatusTitle = personel.PaymentStatus.Title,
                WorkGroupId = personel.WorkGroupId,
                WorktimeId = personel.WorkGroup.WorkTimeId,
                WorktimeTitle = personel.WorkGroup.WorkTime.Title,
                WorkCityId = personel.WorkCityId,
                JobPositionId = personel.JobPositionId.Value,
                TeamCode = personel.TeamWork.Code,
                TeamTitle = personel.TeamWork.Title,
                TeamId = personel.TeamWorkId ?? 0,
                IsarStatusTitle = personel.IsarStatus?.Title
                //StructureId = chart.StructureId,

            };

            //اطلاعات پست فرد
            GetJobPositioinDetail(result);

            // پیدا کردن گروه فرد از آخرین حکم
            GetLastInterDictDetails(personel, result);

            //اطلاعات مدرک تحصیلی
            GetEducationAndStudyFieldEmployee(result);



            return result;
        }

        /// <summary>
        ///اطلاعات مدرک تحصیلی
        /// </summary>
        /// <param name="result"></param>
        private void GetEducationAndStudyFieldEmployee(EmployeeDetailsModel result)
        {
            var educationdegree = _kscHrUnitOfWork.EmployeeEducationDegreeRepository.GetActiveByEmployeeIDForDetail(result.Id);
            result.StudyFieldTitle = educationdegree.StudyField.Title;
            result.EducationTitle = educationdegree.Education.EducationCategory.Title;
            result.EducationId = educationdegree.EducationId;
            result.StudyFieldId = educationdegree.StudyFieldId;
        }

        /// <summary>
        /// پیدا کردن گروه فرد از آخرین حکم 
        /// </summary>
        /// <param name="personel"></param>
        /// <param name="result"></param>
        private void GetLastInterDictDetails(Employee personel, EmployeeDetailsModel result)
        {
            var findLastInterdict = _kscHrUnitOfWork.EmployeeInterdictRepository.GetlatestByEmployeeIdForDetail(personel.Id);

            result.GroupNumber = findLastInterdict.CurrentJobGroupId.ToString();
        }

        /// <summary>
        ///اطلاعات پست فرد
        /// </summary>
        /// <param name="result"></param>
        private void GetJobPositioinDetail(EmployeeDetailsModel result)
        {
            var currentJobPosition = _kscHrUnitOfWork.Chart_JobPositionRepository
                .GetDetailedJobPostion(result.JobPositionId);
            if (currentJobPosition != null)
            {
                result.JobPositionCode = currentJobPosition.JobPositionCode;
                result.JobPositionTitle = currentJobPosition.JobPositionTitle;
                result.DefinitionPostTitle = currentJobPosition.DefinitionPostTitle;
                result.JobIdentityTitle = currentJobPosition.JobIdentityTitle;
                result.JobIdentityCode = currentJobPosition.JobIdentityCode;

                result.StructureId = currentJobPosition.StructureId;
                result.StructureCode = currentJobPosition.StructureCode;
                result.StructureTitle = currentJobPosition.StructureTitle;
                //
                result.StudyFieldJobPositionTitle = currentJobPosition.StudyFieldTitle;
                result.EducationJobPositionTitle = currentJobPosition.EducationTitle;
                result.EmploymeEducationDegreeId = currentJobPosition.EmployeeEducationDegreeId;
                result.JobCategoryEducationId = currentJobPosition.JobCategoryEducationId;
                //پیدا کردن معاونت

                result.Asistance = currentJobPosition.Asistance;

                result.JobPositionFieldId = currentJobPosition.JobPositionFieldId;

                result.JobCategoryDefinitionId = currentJobPosition.JobCategoryDefinitionId;


            }
        }



        public List<EmployeeInfoModel> GetListEmployeeInfoByNumber(SearchEmployeeInfo search)
        {
            var personels = _kscHrUnitOfWork.EmployeeRepository.GetEmployees(search.EmployeeNumbers).AsNoTracking().ToList();
            var models = new List<EmployeeInfoModel>();
            var jobpositionids = personels.Select(a => a.JobPositionId).ToList();
            var jobpositions = _kscHrUnitOfWork.Chart_JobPositionRepository.getActived().Where(a => jobpositionids.Contains(a.Id)).ToList();
            foreach (var personel in personels)
            {
                var jobposition = jobpositions.FirstOrDefault(a => a.Id == personel.JobPositionId);
                var model = new EmployeeInfoModel()
                {
                    FirstName = personel.Name,
                    LastName = personel.Family,
                    NationalCode = personel.NationalCode,
                    EmployeeNumber = personel.EmployeeNumber,
                    BirthDate = personel.BirthDate,
                    PhoneNumber = personel.PhoneNumber,
                    ShiftTitle = personel.WorkGroup?.Code,
                    JobPositionCode = jobposition == null ? "" : jobposition.MisJobPositionCode,
                    TeamWorkCode = personel.TeamWork == null ? null : personel.TeamWork.Code,
                    CostCenter = jobposition == null ? 0 : jobposition.CostCenter
                };
                models.Add(model);
            }

            return models;
        }
        public async Task<List<EmployeeDataForOtherSystemModel>> GetListEmployeeForOtherSystem()
        {
            var personels = await _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().ToListAsync();
            var models = _mapper.Map<List<EmployeeDataForOtherSystemModel>>(personels);

            //foreach (var personel in personels)
            //{
            //    var model = new EmployeeDataModel()
            //    {
            //        FirstName = personel.Name,
            //        LastName = personel.Family,
            //        NationalCode = personel.NationalCode,
            //        EmployeeNumber = personel.EmployeeNumber,
            //        BirthDate = personel.BirthDate,
            //    };
            //    models.Add(model);
            //}

            return models;
        }
        #region MisEmployee
        /// <summary>
        /// انتقال اطلاعات از MIS به HR
        /// </summary>
        /// <returns></returns>
        public async Task<KscResult> SyncEmployeeDataFromMis()
        {
            var result = new KscResult<byte[]>();
            int errorcode = 0;
            try
            {
                #region  File

                var FileforMIS = "EMPLOYEE-DATA.TXT";
                var model = new MISAddOrEditEmployeeBaseModel()
                {
                    Domain = "KSC",
                    FUNCTION = "EMPLOYEE-DATA-FILE"
                };
                var resultApiMis = _misUpdateService.UpdateTextFilePersonalInfo(model);
                if (resultApiMis.IsError)
                {
                    throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                }
                result = _misUpdateService.GetTextFileFromMis(Utility.ServerPathGetFileStream, FileforMIS);

                if (result.Success)
                {

                    var data = (Byte[])result.Data;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var _stream = new StreamReader(new MemoryStream(data), System.Text.Encoding.GetEncoding("windows-1256"));
                    //_stream = new StreamReader(, Encoding.GetEncoding("windows-1256");
                    string line;
                    while ((line = _stream.ReadLine()) != null)
                    {
                        errorcode = 0;
                        Employee employeemodel;
                        var _splited = line.Split('|').ToArray();
                        string employeenumber = _splited[0].Any() ? _splited[0] : null;
                        errorcode++;
                        if (employeenumber == null)
                            continue;
                        int paymentStatusId = Convert.ToInt32(_splited[1].ToString());
                        errorcode++;
                        int paymentStatusyearmonth = Convert.ToInt32(_splited[2].ToString());
                        errorcode++;
                        var name = _splited[3].Any() ? !string.IsNullOrWhiteSpace(_splited[3]) || !string.IsNullOrEmpty(_splited[3]) ? _splited[3].ToString().Trim() : null : null;
                        errorcode++;
                        var family = _splited[4].Any() ? !string.IsNullOrWhiteSpace(_splited[4]) || !string.IsNullOrEmpty(_splited[4]) ? _splited[4].ToString().Trim() : null : null;
                        errorcode++;
                        var preiveName = _splited[5].Any() ? !string.IsNullOrWhiteSpace(_splited[5]) || !string.IsNullOrEmpty(_splited[5]) ? _splited[5].ToString().Trim() : null : null;
                        errorcode++;
                        var preiveFamily = _splited[6].Any() ? !string.IsNullOrWhiteSpace(_splited[6]) || !string.IsNullOrEmpty(_splited[6]) ? _splited[6].ToString().Trim() : null : null;
                        errorcode++;
                        int? gender = _splited[7].Any() ? Convert.ToInt32(_splited[7].ToString().Trim()) : null;
                        errorcode++;
                        var fatherName = _splited[8].Any() ? !string.IsNullOrWhiteSpace(_splited[8]) || !string.IsNullOrEmpty(_splited[8]) ? _splited[8].ToString().Trim() : null : null;
                        errorcode++;
                        var nationalCode = _splited[9].Any() ? !string.IsNullOrWhiteSpace(_splited[9]) || !string.IsNullOrEmpty(_splited[9]) ? _splited[9].ToString().Trim() : null : null;
                        errorcode++;
                        var certificateNumber = _splited[10].Any() ? !string.IsNullOrWhiteSpace(_splited[10]) || !string.IsNullOrEmpty(_splited[10]) ? _splited[10].ToString().Trim() : null : null;
                        errorcode++;
                        int? nationalityId = _splited[11].Any() ? (!string.IsNullOrEmpty(_splited[11].ToString()) ? Convert.ToInt32(_splited[11].ToString().Trim()) : null) : null;
                        errorcode++;
                        int? regionId = _splited[12].Any() ? (!string.IsNullOrEmpty(_splited[12].ToString()) ? Convert.ToInt32(_splited[12].ToString().Trim()) : null) : null;
                        errorcode++;
                        var birthDate = _splited[13].Any() ? !string.IsNullOrEmpty(_splited[13].ToString()) ? DateTimeExtensions.ToGorgianDate(_splited[13].ToString().Trim()) : null : null;
                        errorcode++;
                        var birthCityId = _splited[14].Any() ? _kscHrUnitOfWork.CityRepository.FirstOrDefault(x => x.TAB_CITY_SP_MIS == _splited[14].ToString().Trim())?.Id ?? null : null;
                        errorcode++;
                        var certificateDate = _splited[15].Any() ? (!string.IsNullOrEmpty(_splited[15].ToString().Trim()) ? DateTimeExtensions.ToGorgianDate(_splited[15].ToString().Trim()) : null) : null;
                        errorcode++;
                        var certificateCityId = _splited[16].Any() ? (_kscHrUnitOfWork.CityRepository.FirstOrDefault(x => x.TAB_CITY_SP_MIS == _splited[16].ToString().Trim())?.Id ?? null) : null;
                        errorcode++;
                        var militaryStatusId = _splited[17].Any() ? (_kscHrUnitOfWork.MilitaryStatusRepository.FirstOrDefault(x => x.Id.ToString() == _splited[17].ToString().Trim())?.Id ?? null) : null;
                        errorcode++;
                        DateTime? militaryStartDate = _splited[18].Any() ? (!string.IsNullOrEmpty(_splited[18].ToString().Trim()) ? DateTimeExtensions.ToGorgianDate(_splited[18].ToString().Trim()) : null) : null;
                        errorcode++;
                        DateTime? militaryEndDate = _splited[19].Any() ? (!string.IsNullOrEmpty(_splited[19].ToString().Trim()) ? DateTimeExtensions.ToGorgianDate(_splited[19].ToString().Trim()) : null) : null;
                        var maritalStatusId = _splited[20].Any() ? (_kscHrUnitOfWork.MaritalStatusRepository.FirstOrDefault(x => x.Id.ToString() == _splited[20].ToString().Trim())?.Id ?? null) : null;
                        errorcode++;
                        var marriedDate = _splited[21].Any() ? (!string.IsNullOrEmpty(_splited[21].ToString().Trim()) ? DateTimeExtensions.ToGorgianDate(_splited[21].ToString().Trim()) : null) : null;
                        errorcode++;
                        var insuranceNumber = _splited[22].Any() ? !string.IsNullOrWhiteSpace(_splited[22]) || !string.IsNullOrEmpty(_splited[22]) ? (_splited[22].ToString().Trim() ?? null) : null : null;
                        errorcode++;
                        int? insuranceType = _splited[23].Any() ? (Convert.ToInt32(_splited[23].ToString().Trim())) : null;
                        errorcode++;
                        var insuranceListId = _splited[24].Any() ? (_kscHrUnitOfWork.InsuranceListRepository.FirstOrDefault(x => x.AgreementRow == _splited[24].ToString().Trim() && x.InsuranceTypeId == insuranceType)?.Id ?? null) : null;
                        errorcode++;
                        var phoneNumber = _splited[25].Any() ? !string.IsNullOrWhiteSpace(_splited[25]) || !string.IsNullOrEmpty(_splited[25]) ? _splited[25].ToString().Trim() : null : null;
                        errorcode++;
                        var bloodTypeId = _splited[26].Any() ? _kscHrUnitOfWork.BloodTypeRepository.FirstOrDefault(x => x.Id.ToString() == _splited[26].ToString().Trim())?.Id ?? null : null;
                        errorcode++;
                        var homeZipCode = _splited[27].Any() ? !string.IsNullOrWhiteSpace(_splited[27]) || !string.IsNullOrEmpty(_splited[27]) ? _splited[27].ToString().Trim() : null : null;
                        errorcode++;
                        var homeCityId = _splited[28].Any() ? (_kscHrUnitOfWork.CityRepository.FirstOrDefault(x => x.TAB_CITY_SP_MIS == _splited[28].ToString().Trim())?.Id ?? null) : null;
                        errorcode++;
                        //var homeAddress = _splited[29].Any() ? _splited[29].ToString() ?? "" : null;
                        //errorcode++;
                        if (_kscHrUnitOfWork.EmployeeRepository.Any(x => x.EmployeeNumber == employeenumber))
                        {
                            employeemodel = _kscHrUnitOfWork.EmployeeRepository.FirstOrDefault(x => x.EmployeeNumber == employeenumber);
                            employeemodel.PaymentStatusId = Convert.ToInt32(paymentStatusId);
                            var employeePaymentStatus = _kscHrUnitOfWork.EmployeePaymentStatusRepository.GetAllEmployeePaymentStatusByRelated(employeemodel.Id).FirstOrDefault();
                            if (employeePaymentStatus == null)
                            {
                                if (paymentStatusId == 1)
                                    _kscHrUnitOfWork.EmployeePaymentStatusRepository.Add(new EmployeePaymentStatus()
                                    {
                                        EmployeeId = employeemodel.Id,
                                        YearMonth = paymentStatusyearmonth,
                                        PaymentStatusId = paymentStatusId,
                                        InsertDate = DateTime.Now,
                                        InsertUser = "MisSync",
                                    });
                            }
                            else
                            {
                                employeePaymentStatus.YearMonth = paymentStatusyearmonth;
                                employeePaymentStatus.PaymentStatusId = paymentStatusId;
                                employeePaymentStatus.InsertDate = DateTime.Now;
                                employeePaymentStatus.InsertUser = "MisSync";
                            }
                            employeemodel.Name = name;
                            employeemodel.Family = family;
                            employeemodel.PreiveName = preiveName;
                            employeemodel.PreiveFamily = preiveFamily;
                            employeemodel.Gender = gender;
                            employeemodel.FatherName = fatherName;
                            employeemodel.NationalCode = nationalCode;
                            employeemodel.CertificateNumber = certificateNumber;
                            employeemodel.NationalityId = nationalityId;
                            employeemodel.RegionId = regionId;
                            employeemodel.BirthDate = birthDate;
                            employeemodel.BirthCityId = birthCityId;
                            employeemodel.CertificateDate = certificateDate;
                            employeemodel.CertificateCityId = certificateCityId;
                            employeemodel.MilitaryStatusId = militaryStatusId;
                            employeemodel.MilitaryStartDate = militaryStartDate;
                            employeemodel.MilitaryEndDate = militaryEndDate;
                            employeemodel.MaritalStatusId = maritalStatusId;
                            employeemodel.MarriedDate = marriedDate;
                            employeemodel.InsuranceNumber = insuranceNumber;
                            //employeemodel.InsuranceType = _kscHrUnitOfWork.CityRepository.FirstOrDefault(x => x.Code == _splited[23].ToString().Trim())?.Id ?? null;
                            employeemodel.InsuranceListId = insuranceListId;
                            employeemodel.PhoneNumber = phoneNumber;
                            employeemodel.BloodTypeId = bloodTypeId;
                            employeemodel.HomeZipCode = homeZipCode;
                            employeemodel.HomeCityId = homeCityId;
                            //employeemodel.HomeAddress = homeAddress;
                            employeemodel.UpdateUser = "MisSync";
                            employeemodel.UpdateDate = DateTime.Now;

                        }
                        else
                        {
                            employeemodel = new Employee()
                            {
                                EmployeeNumber = employeenumber,
                                Name = name,
                                Family = family,
                                PreiveName = preiveName,
                                PreiveFamily = preiveFamily,
                                Gender = gender,
                                FatherName = fatherName,
                                NationalCode = nationalCode,
                                CertificateNumber = certificateNumber,
                                NationalityId = nationalityId,
                                RegionId = regionId,
                                BirthDate = birthDate,
                                BirthCityId = birthCityId,
                                CertificateDate = certificateDate,
                                CertificateCityId = certificateCityId,
                                MilitaryStatusId = militaryStatusId,
                                MilitaryStartDate = militaryStartDate,
                                MilitaryEndDate = militaryEndDate,
                                MaritalStatusId = maritalStatusId,
                                MarriedDate = marriedDate,
                                InsuranceNumber = insuranceNumber,
                                InsuranceListId = insuranceListId,
                                PhoneNumber = phoneNumber,
                                BloodTypeId = bloodTypeId,
                                HomeZipCode = homeZipCode,
                                HomeCityId = homeCityId,
                                //HomeAddress = homeAddress,
                                InsertUser = "MisSync",
                                InsertDate = DateTime.Now

                            };
                            if (paymentStatusId == 1)
                                employeemodel.EmployeePaymentStatus.Add(new EmployeePaymentStatus()
                                {
                                    EmployeeId = employeemodel.Id,
                                    YearMonth = paymentStatusyearmonth,
                                    PaymentStatusId = paymentStatusId,
                                    InsertDate = DateTime.Now,
                                    InsertUser = "MisSync",
                                });
                            employeemodel.PersonalTypeId = EnumPersonalType.EmploymentPerson.Id;
                            _kscHrUnitOfWork.EmployeeRepository.Add(employeemodel);
                        }
                    }
                }

                await _kscHrUnitOfWork.SaveAsync();
                #endregion
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message + errorcode);
            }

            return result;
        }
        /// <summary>
        /// انتقال اطلاعات  آدرس از MIS به HR
        /// </summary>
        /// <returns></returns>
        public async Task<KscResult> SyncEmployeeAdressDataFromMis()
        {
            var result = new KscResult<byte[]>();
            int errorcode = 0;
            string employeenumber = string.Empty;

            try
            {
                #region  File

                var FileforMIS = "EMPLOYEE-ADDRESS.TXT";
                var model = new MISAddOrEditEmployeeBaseModel()
                {
                    Domain = "KSC",
                    FUNCTION = "EMPLOYEE-DATA-ADDRESS"
                };
                var resultApiMis = _misUpdateService.UpdateTextFilePersonalInfo(model);
                if (resultApiMis.IsError)
                {
                    throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                }
                result = _misUpdateService.GetTextFileFromMis(Utility.ServerPathGetFileStream, FileforMIS);

                if (result.Success)
                {

                    var data = (Byte[])result.Data;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var _stream = new StreamReader(new MemoryStream(data), System.Text.Encoding.GetEncoding("windows-1256"));
                    //_stream = new StreamReader(, Encoding.GetEncoding("windows-1256");
                    string line;
                    while ((line = _stream.ReadLine()) != null)
                    {
                        errorcode = 0;
                        employeenumber = "";
                        Employee employeemodel;
                        if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                            continue;
                        var _splited = line.Split('|').ToArray();
                        employeenumber = _splited[0].Any() ? _splited[0] : null;
                        //employeenumber = employeenumber;
                        errorcode++;
                        if (employeenumber == null)
                            continue;
                        var homeAddress = _splited[1].Any() ? String.IsNullOrEmpty(_splited[1]) || String.IsNullOrWhiteSpace(_splited[1]) ? null : _splited[1].ToString() ?? "" : null;
                        errorcode++;

                        if (_kscHrUnitOfWork.EmployeeRepository.Any(x => x.EmployeeNumber == employeenumber))
                        {
                            employeemodel = _kscHrUnitOfWork.EmployeeRepository.FirstOrDefault(x => x.EmployeeNumber == employeenumber);
                            employeemodel.HomeAddress = homeAddress;
                            employeemodel.UpdateUser = "MisSync";
                            employeemodel.UpdateDate = DateTime.Now;

                        }
                    }
                }
                await _kscHrUnitOfWork.SaveAsync();
                #endregion
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message + errorcode + "-" + employeenumber);
            }

            return result;
        }




        /// <summary>
        /// سینک ردیف کارانه- گروه شغلی
        /// </summary>
        /// <returns></returns>
        public async Task<KscResult> SyncEmployeeEfficiencyJobPositionDataFromMis(ConvertPageModel filter)
        {
            var result = new KscResult<byte[]>();
            int errorcode = 0;
            string employeenumber = string.Empty;

            try
            {
                #region  File

                var FileforMIS = "Production-JobGroup.TXT";
                //var model = new MISAddOrEditEmployeeBaseModel()
                //{
                //    Domain = "KSC",
                //    FUNCTION = "EMPLOYEE-DATA-ADDRESS"
                //};
                //var resultApiMis = _misUpdateService.UpdateTextFilePersonalInfo(model);
                //if (resultApiMis.IsError)
                //{
                //    throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
                //}
                //result = _misUpdateService.GetTextFileFromMis(Utility.ServerPathGetFileStream, FileforMIS);

                // if (result.Success)
                //{

                // var data = (Byte[])result.Data;

                Byte[] data = System.IO.File.ReadAllBytes(@$"{filter.FileAddress}");

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var _stream = new StreamReader(new MemoryStream(data), System.Text.Encoding.GetEncoding("windows-1256"));
                //_stream = new StreamReader(, Encoding.GetEncoding("windows-1256");
                string line;
                var employees = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().ToList();
                while ((line = _stream.ReadLine()) != null)
                {
                    errorcode = 0;
                    employeenumber = "";
                    Employee employeemodel;
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                        continue;
                    var _splited = line.Split('|').ToArray();
                    employeenumber = _splited[0].Any() ? _splited[0] : null;
                    //employeenumber = employeenumber;
                    errorcode++;
                    if (employeenumber == null)
                        continue;
                    int? productionEfficiencyId = _splited[1].Any() ? String.IsNullOrEmpty(_splited[1]) || String.IsNullOrWhiteSpace(_splited[1]) ? null : Convert.ToInt32(_splited[1].ToString()) : null;
                    int? jobGroupId = _splited[2].Any() ? String.IsNullOrEmpty(_splited[2]) || String.IsNullOrWhiteSpace(_splited[1]) ? null : Convert.ToInt32(_splited[2].ToString()) : null;
                    errorcode++;


                    if (jobGroupId < 1 || jobGroupId > 20)
                    {
                        errorcode++;
                        continue;
                    }

                    if (employees.Any(x => x.EmployeeNumber == employeenumber))
                    {
                        employeemodel = employees.FirstOrDefault(x => x.EmployeeNumber == employeenumber);
                        if (productionEfficiencyId.HasValue)
                        {
                            if (productionEfficiencyId.Value > 0)
                            {
                                productionEfficiencyId = productionEfficiencyId.Value;
                            }
                            else
                            {
                                productionEfficiencyId = null;
                            }
                        }
                        else
                        {
                            productionEfficiencyId = null;
                        }
                        //employeemodel.ProductionEfficiencyId = !productionEfficiencyId.HasValue || productionEfficiencyId.Value==0:null?;

                        employeemodel.JobGroupId = jobGroupId;
                        employeemodel.UpdateUser = "MisSync";
                        employeemodel.UpdateDate = DateTime.Now;

                    }
                }
                // }
                await _kscHrUnitOfWork.SaveAsync(checklog: false);
                #endregion
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message + errorcode + "-" + employeenumber);
            }

            return result;
        }

        #endregion


        public void GetGeneratedEmployeeNum1()
        {
            int seq = 0;
            // var seqNum = seq + 1;
            int seqNumTemp = 0;

            //
            //  int YearlastNUM = 1402;
            int year = 1403;
            System.IO.StreamWriter empl = new System.IO.StreamWriter(string.Format((@"d:\\emploeeNumbers.txt")));
            string employeeNum = "14022551";
            while (seqNumTemp != 999)
            {
                int YearlastNUM = int.Parse(employeeNum.Substring(0, 4));
                seq = int.Parse(employeeNum.Substring(4, 3));
                if (year > YearlastNUM)
                {
                    // var seqNew = "000";
                    seqNumTemp = 1;//seqNew + 1;
                }
                else
                {
                    if (seq != 999)
                        seqNumTemp = seq + 1;
                    else
                        seqNumTemp = seq;

                }

                //
                string seqNum = seqNumTemp.ToString();


                if (seqNumTemp < 10)
                    seqNum = "00" + seqNumTemp.ToString();
                else
                if (seqNumTemp < 100)
                    seqNum = "0" + seqNumTemp.ToString();





                //----------------------formula for X------------------------
                var EndX = 0;
                var b = int.Parse(year.ToString().Substring(1, 1)) * 7;
                var c = int.Parse(year.ToString().Substring(2, 1)) * 6;
                var d = int.Parse(year.ToString().Substring(3, 1)) * 5;
                var sumYear = b + c + d;


                var e = int.Parse(seqNum.ToString().Substring(0, 1)) * 4;
                var z = int.Parse(seqNum.ToString().Substring(1, 1)) * 3;
                var f = int.Parse(seqNum.ToString().Substring(2, 1)) * 2;
                var sumSeqNum = e + z + f;


                var Remain = (sumYear + sumSeqNum) % 11;
                var t = (12 - Remain);
                if (t <= 9 && t >= 0)//
                {
                    EndX = t;
                }
                else if (t == 11 || t == 12)
                {
                    EndX = 1;
                }

                else if (t == 10)
                {
                    EndX = 0;
                }
                //--------------------------------------------------------

                employeeNum = $"{year}{seqNum}{EndX}";
                empl.WriteLine(employeeNum);


            }
            empl.Close();
        }

        public KscResult<EmployeeOrganizationHouseModel> GetExistEmployeeOrganizationHouse(string employeeNumber)
        {
            KscResult<EmployeeOrganizationHouseModel> result = new KscResult<EmployeeOrganizationHouseModel>() { Data = new EmployeeOrganizationHouseModel() };

            var DataModel = new InputMisApiModel()
            {
                NUM_PRSN_EMPL = employeeNumber,
                FUNCTION = "FETCH_GENERAL",
                domain = "KSC"
            };

            var outPutMIS = _dataPersonalForOtherSystemService.GetPersonalDataMis(DataModel);

            if (outPutMIS.IsError == true)
            {
                result.AddError("", "خطایی در دریافت اطلاعات رخ داده است");
                return result;
            }

            result.Data.HasOrganizationHouse = outPutMIS.DETAIL.COD_USE_HOUS_EMPL == "1";
            return result;

        }

        #region EmployeeOtherPayment
        //=========================هزینه سفر=============================

        /// <summary>
        /// پرداخت متفرقه برای هزینه سفر
        /// </summary>
        public List<EmployeeOtherPaymentModelForTravel> GetPersonalEmployeeInTravel(SearchEmployeeTravelPayModel Filter)
        {
            var resultModel = new List<EmployeeOtherPaymentModelForTravel>();
            var result = new KscResult<List<EmployeeHaveTravelPayModel>>();
            var queryList = new EmployeeHaveTravelPayParentModel();

            var startSettingDate = int.Parse(Filter.StartDate.ToEnglishNumbers());
            var endSettingDate = int.Parse(Filter.EndDate.ToEnglishNumbers());

            Filter.StartDate = $"{Filter.StartDate.ToEnglishNumbers()}01";
            Filter.EndDate = $"{Filter.EndDate.ToEnglishNumbers()}31";


            var StartYearShamsiDate = int.Parse(Filter.StartDate.Substring(0, 4));
            var EndYearShamsiDate = int.Parse(Filter.EndDate.Substring(0, 4));

            if (StartYearShamsiDate != EndYearShamsiDate)
            {
                throw new Exception("بازه زمانی باید در یک سال باشد ");
            }



            var StartShamsiDate = int.Parse(Filter.StartDate);
            var EndShamsiDate = int.Parse(Filter.EndDate);
            //
            var setting = _kscHrUnitOfWork.OtherPaymentSettingRepository
                .GetOtherPaymentSettingByotherPaymentTypeForTravel(startSettingDate, endSettingDate, EnumPaymentType.TravelEmployee.Id);
            //
            var parametersValue = _kscHrUnitOfWork.OtherPaymentSettingParameterValueRepository.GetAllQueryableBySettingId(setting.Id).Where(x => x.IsActive);
            var countValidDaysForVacationPayment = parametersValue.FirstOrDefault(x => x.OtherPaymentSettingParameterId == EnumOtherPaymentSettingParameter.CountValidDaysForVacationPayment.Id);
            if (countValidDaysForVacationPayment == null)
            {
                throw new Exception("تعداد روزهای مرخصی استحقاقی برای سفر در تنظیمات مقدار ندارد ");
            }

            var MinimumPersonForTravelPayment = parametersValue.FirstOrDefault(x => x.OtherPaymentSettingParameterId == EnumOtherPaymentSettingParameter.MinimumPersonForTravelPayment.Id);
            if (MinimumPersonForTravelPayment == null)
            {
                throw new Exception("حداقل تعداد برای افراد مجرد در محاسبه هزینه سفر در تنظیمات مقدار ندارد ");
            }


            var MaximumPersonForTravelPayment = parametersValue.FirstOrDefault(x => x.OtherPaymentSettingParameterId == EnumOtherPaymentSettingParameter.MaximumPersonForTravelPayment.Id);
            if (MaximumPersonForTravelPayment == null)
            {
                throw new Exception("حداکثر تعداد برای افراد متاهل و افراد تحت تکفلشان در محاسبه هزینه سفر در تنظیمات مقدار ندارد ");
            }
            //


            var getEmployeeWorkDayType = _kscHrUnitOfWork.EmployeeAttendAbsenceItemRepository
                .SpGetEmployeeLeaveStatus(StartShamsiDate, EndShamsiDate);

            var allHolidayDate = _kscHrUnitOfWork.WorkCalendarRepository
                .GetRangeDateKeyMonthWorkCalendar(StartShamsiDate, EndShamsiDate)
                .Where(a => a.IsHoliday)
                .Select(a => a.DateKey)
                .ToList();
            var GetList = (from item in getEmployeeWorkDayType
                           group item by new { item.EmployeeId } into final
                           select new
                           {
                               final.Key.EmployeeId,
                               list = final.OrderBy(a => a.EmployeeNumber).ToList(),

                           }).ToList();
            var employeenumbers = getEmployeeWorkDayType.Select(a => a.EmployeeNumber).ToList();

            var EmployeeWorkTime = _kscHrUnitOfWork.EmployeeRepository
                .GetDataEmployeeForTravel(employeenumbers.Distinct().ToList())
                .Select(a => new
                {
                    a.Id,
                    isOfficialholiday = a.WorkGroup.WorkTime.OfficialUnOfficialHolidayFromWorkCalendar
                }).ToList();


            Filter.CountDay = int.Parse(countValidDaysForVacationPayment.ParameterValue);
            Filter.MinLeve = Filter.CountDay;

            //
            var FinalList = new List<EmployeeHaveTravelPayModel>();
            foreach (var item in GetList)
            {
                var skiptHoliday = EmployeeWorkTime.First(a => item.EmployeeId == a.Id).isOfficialholiday;

                var searchList = GetPersoneIsCanInLeveList(Filter, item.list, skiptHoliday, allHolidayDate);

                if (string.IsNullOrEmpty(searchList.PersonnelNumber))
                    continue;
                FinalList.Add(searchList);
            }
            var employeeIds = FinalList.Select(x => x.EmployeeId).ToList();
            var salarymodel = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var salaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetSalaryYear(salarymodel);
            if (setting != null)
            {
                //var PayedEmployeeList = GetPayedEmployee(setting.ValidityStartYearMonth, setting.ValidityEndYearMonth, EnumPaymentType.TravelEmployee.Id);
                var PayedEmployeeList = GetPayedEmployeeForTravel(setting.ValidityStartYearMonth);
                if (PayedEmployeeList.Count > 0)
                {
                    //مقایسه دو لیست PayedEmployeeList و FinalList

                    var deleted = FinalList.Where(a => PayedEmployeeList.Any(x => x.EmployeeId == a.EmployeeId)).ToList();
                    var FinalResult = FinalList.Where(a => !deleted.Contains(a)).ToList();
                    employeeIds = FinalResult.Select(x => x.EmployeeId).ToList();
                }
                var listTravel = new List<ListTravelDto>();

                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployees(employeeIds);



                var kvalueunit = _kscHrUnitOfWork.KUnitSettingRepository.GetKUnitByYear(int.Parse(Filter.StartDate.Substring(0, 4)));



                var salarymonth = Utility.GetPersianMonth(salarymodel.SalaryDate.ToString());
                var blacklist = _kscHrUnitOfWork.EmployeeBlackListRepository.GetAllByDate(salarymonth.StartDate, salarymonth.EndDate, new int[] { EnumPaymentType.TravelEmployee.Id });

                //


                var minimumPersonForTravelPayment = int.Parse(MinimumPersonForTravelPayment.ParameterValue);
                var maximumPersonForTravelPayment = int.Parse(MaximumPersonForTravelPayment.ParameterValue);
                //
                resultModel = GetStaticalFamilyEmployeeReport(employee, setting, kvalueunit, minimumPersonForTravelPayment, maximumPersonForTravelPayment);
                var viewMisEmployee =
                    _kscHrUnitOfWork
                    .View_EmployeeRepository
                    .GetAllQueryable()
                    .Where(x => x.JobCategoryCode != "TM" && !string.IsNullOrEmpty(x.MisJobPositionCode))
                    .Select(a => new { a.EmployeeId, a.JobCategoryCode, a.CostCenter });
   

                if (resultModel != null)
                {
                    var query = (from e in resultModel
                                 join m in viewMisEmployee
                                      on e.Id equals m.EmployeeId

                                 select new EmployeeOtherPaymentModelForTravel
                                 {
                                     Id = e.Id,
                                     EmployeeNumber = e.EmployeeNumber,
                                     CostCenterCode = m.CostCenter.ToString(),
                                     FullName = e.FullName,
                                     YearMonth = e.YearMonth,
                                     FamilyCount = e.FamilyCount,
                                     EmploymentDate = e.EmploymentDate,
                                     Price = e.Price,
                                     GenderTitle = e.GenderTitle,
                                     MaritalStatusTitle = e.MaritalStatusTitle,
                                     TotalCount = e.TotalCount,
                                     JobCategoryCode = m.JobCategoryCode,
                                     IsBlackList = blacklist.Any(a => a.EmployeeId == e.Id)

                                 }).ToList();

                    return query;

                }

            }


            return resultModel;
        }

        /// <summary>
        /// پرداخت متفرقه برای هزینه سفر مدیران
        /// </summary>
        public List<EmployeeOtherPaymentModelForTravel> GetPersonalManagmentInTravel(SearchEmployeeTravelPayModel Filter)
        {
            var resultModel = new List<EmployeeOtherPaymentModelForTravel>();
            var result = new KscResult<List<EmployeeHaveTravelPayModel>>();
            var queryList = new EmployeeHaveTravelPayParentModel();

            var startSettingDate = int.Parse(Filter.StartDate.ToEnglishNumbers());
            var endSettingDate = int.Parse(Filter.EndDate.ToEnglishNumbers());

            Filter.StartDate = $"{Filter.StartDate.ToEnglishNumbers()}01";
            Filter.EndDate = $"{Filter.EndDate.ToEnglishNumbers()}31";

            var StartShamsiDate = int.Parse(Filter.StartDate);
            var EndShamsiDate = int.Parse(Filter.EndDate);
            //
            var StartYearShamsiDate = int.Parse(Filter.StartDate.Substring(0, 4));
            var EndYearShamsiDate = int.Parse(Filter.EndDate.Substring(0, 4));

            if (StartYearShamsiDate != EndYearShamsiDate)
            {
                throw new Exception("بازه زمانی باید در یک سال باشد ");
            }


            var stratDate = int.Parse(Filter.StartDate.ToEnglishNumbers());
            var endDate = int.Parse(Filter.EndDate.ToEnglishNumbers());

            var setting = _kscHrUnitOfWork.OtherPaymentSettingRepository
                .GetOtherPaymentSettingByotherPaymentTypeForTravel(startSettingDate, endSettingDate, EnumPaymentType.TravelManagment.Id);
            //
            var parametersValue = _kscHrUnitOfWork.OtherPaymentSettingParameterValueRepository.GetAllQueryableBySettingId(setting.Id).Where(x => x.IsActive);

            var MaximumPersonForTravelPayment = parametersValue.FirstOrDefault(x => x.OtherPaymentSettingParameterId == EnumOtherPaymentSettingParameter.CountMangmentForTravelPayment.Id);
            if (MaximumPersonForTravelPayment == null)
            {
                throw new Exception("حداکثر تعداد برای افراد متاهل و افراد تحت تکفلشان در محاسبه هزینه سفر در تنظیمات مقدار ندارد ");
            }

            //لیست 1
            var viewMisEmployee = _kscHrUnitOfWork
                                  .ViewMisEmployeeRepository
                                  .GetAllQueryable()
                                  .Where(x => x.JobCategoryCode == "TM").Distinct();

            var ids = viewMisEmployee.Select(x => x.EmployeeId).ToList();
            var employeeIds = new List<int>();
            var salarymodel = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var salaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetSalaryYear(salarymodel);
            if (setting != null)
            {
                //لیست 2
                // var PayedEmployeeList = GetPayedManagmentEmployee(ids, setting.ValidityStartYearMonth, setting.ValidityEndYearMonth, EnumPaymentType.TravelEmployee.Id);
                //var PayedEmployeeList = GetPayedManagmentEmployee(ids, stratDate, endDate, EnumPaymentType.TravelEmployee.Id);
                var PayedEmployeeList = GetPayedEmployeeForTravel(StartYearShamsiDate);

                employeeIds = viewMisEmployee.Select(x => x.EmployeeId).ToList();

                var listTravel = new List<ListTravelDto>();

                var employee = _kscHrUnitOfWork.EmployeeRepository.GetEmployees(employeeIds);



                var kvalueunit = _kscHrUnitOfWork.KUnitSettingRepository.GetKUnitByYear(StartYearShamsiDate);
                //


                //   var minimumPersonForTravelPayment = int.Parse(MinimumPersonForTravelPayment.ParameterValue);
                var maximumPersonForTravelPayment = int.Parse(MaximumPersonForTravelPayment.ParameterValue);
                //


                if (employee != null)
                {
                    //این رو باید از ستینگ بخونم عدد 7 رو
                    var price = Convert.ToDecimal(maximumPersonForTravelPayment * (kvalueunit * setting.KPercent));

                    var salarymonth = Utility.GetPersianMonth(salarymodel.SalaryDate.ToString());
                    var blacklist = _kscHrUnitOfWork.EmployeeBlackListRepository.GetAllByDate(salarymonth.StartDate, salarymonth.EndDate, new int[] { EnumPaymentType.TravelManagment.Id });

                    var query = (from e in employee
                                 join m in viewMisEmployee
                                      on e.Id equals m.EmployeeId

                                 select new EmployeeOtherPaymentModelForTravel
                                 {
                                     Id = e.Id,
                                     EmployeeNumber = e.EmployeeNumber,
                                     CostCenterCode = m.CostCenterCode,
                                     FullName = $"{e.Name} {e.Family}",
                                     // YearMonth = e.YearMonth,
                                     //FamilyCount = e.FamilyCount,
                                     //EmploymentDate = e.EmploymentDate,
                                     Price = price,
                                     GenderTitle = e.GenderType.Title,
                                     MaritalStatusTitle = e.MaritalStatus.Title,
                                     TotalCount = maximumPersonForTravelPayment,
                                     JobCategoryCode = m.JobCategoryCode,
                                     IsBlackList = blacklist.Any(a => a.EmployeeId == e.Id)

                                 }).ToList();

                    return query;

                }

            }


            return resultModel;
        }

        /// <summary>
        /// پرداخت متفرقه برای هزینه سفر مدیران
        /// </summary>
        public EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModelForTravel> GetEmployeeOtherPaymentTravelEmployeeByFilter(SearchEmployeeTravelPayModel search)
        {
            var modelResult = new EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModelForTravel>();
            if (search.OtherPaymentTypeId == null || search.OtherPaymentTypeId == 0)
            {
                modelResult.ErrorMessage = "نوع پرداختی را وارد کنید";

            }
            if (search.AccountCodeId == null || search.AccountCodeId == 0)
            {
                modelResult.ErrorMessage = "کد حساب را وارد کنید";

            }
            try
            {
                int? yearMonthStartReport = Convert.ToInt32(search.StartDate.ToEnglishNumbers());
                int? yearMonthEndReport = Convert.ToInt32(search.EndDate.ToEnglishNumbers());

                var salaryDate = search.SalaryDate;
                //var otherPaymentHeader = _kscHrUnitOfWork.OtherPaymentHeaderRepository
                //     .GetOtherPaymentHeaderByPaymentYearMonth(yearMonthStartReport.Value, yearMonthEndReport.Value, search.AccountCodeId.Value, salaryDate, search.OtherPaymentTypeId.Value);

                var otherPaymentHeader = _kscHrUnitOfWork.EmployeeOtherPaymentRepository
                    .IsExistEmployeeOtherPaymentInMonthYear(yearMonthStartReport.Value, yearMonthEndReport.Value, search.OtherPaymentTypeId.Value, salaryDate);

                List<EmployeeOtherPaymentModelForTravel> allpayments = new List<EmployeeOtherPaymentModelForTravel>();
                if (otherPaymentHeader.Any())
                {
                    modelResult.CanInsert = false;
                    var otherPaymentHeaderIds = otherPaymentHeader.Select(x => x.Id).Distinct().ToList();
                    var employeeData = _kscHrUnitOfWork.EmployeeOtherPaymentRepository.GetEmployeeOtherPaymentByHeaderIds(otherPaymentHeaderIds);
                    allpayments = employeeData.Select(x => new EmployeeOtherPaymentModelForTravel
                    {

                        EmployeeNumber = x.Employee.EmployeeNumber,
                        FullName = x.Employee.Name + " " + x.Employee.Family,
                        MaritalStatusTitle = x.Employee.MaritalStatus != null ? x.Employee.MaritalStatus.Title : "",
                        //FamilyCount = familyCount,
                        GenderId = x.Employee.Gender,
                        GenderTitle = x.Employee.Gender.HasValue ? x.Employee.GenderType.Title : "",
                        EmploymentDate = x.Employee.EmploymentDate.HasValue ? x.Employee.EmploymentDate.Value.ToPersianDate() : "",


                        TotalCount = x.PaymentPersonCount.HasValue ? x.PaymentPersonCount.Value : 0,
                        Price = x.PaymentAmount,
                        IsBlackList = x.IsBlacklist
                    }).ToList();
                    //
                }
                else
                {
                    modelResult.CanInsert = true;
                    allpayments = GetPersonalEmployeeInTravel(search);
                }
                var resultemployee = _FilterHandler.GetFilterResult<EmployeeOtherPaymentModelForTravel>(allpayments, search, "EmployeeNumber");

                modelResult.Data = resultemployee.Data;
                modelResult.Total = resultemployee.Total;
                modelResult.SumPrice = $"{allpayments.Sum(x => x.Price):N0}";
                modelResult.SumCountRow = allpayments.Where(x => !x.IsBlackList).Sum(x => x.TotalCount);

                modelResult.SalaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().SalaryDate;

            }
            catch (Exception ex)
            {

                modelResult.ErrorMessage = ex.Message;
            }
            return modelResult;

        }

        /// <summary>
        /// پرداخت متفرقه برای هزینه سفر
        /// </summary>
        public EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModelForTravel> GetEmployeeOtherPaymentTravelManagmentByFilter(SearchEmployeeTravelPayModel search)
        {
            var modelResult = new EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModelForTravel>();

            if (search.OtherPaymentTypeId == null || search.OtherPaymentTypeId == 0)
            {
                modelResult.ErrorMessage = "نوع پرداختی را وارد کنید";

            }
            if (search.AccountCodeId == null || search.AccountCodeId == 0)
            {
                modelResult.ErrorMessage = "کد حساب را وارد کنید";

            }

            try
            {
                //
                string yearStartReport = search.StartDate.ToEnglishNumbers().Substring(0, 4);
                var salaryDate = search.SalaryDate;
                var otherPaymentHeader = _kscHrUnitOfWork.OtherPaymentHeaderRepository
                     .GetOtherPaymentHeaderByPaymentYear(yearStartReport, search.AccountCodeId.Value, salaryDate, search.OtherPaymentTypeId.Value);
                //
                int startDate = int.Parse(search.StartDate.ToEnglishNumbers());
                int endDate = int.Parse(search.EndDate.ToEnglishNumbers());
                var isExistEmployeeOtherPaymentInMonthYear = _kscHrUnitOfWork.EmployeeOtherPaymentRepository
                           .IsExistEmployeeOtherPaymentInMonthYear(startDate, endDate, search.OtherPaymentTypeId.Value);
                //
                List<EmployeeOtherPaymentModelForTravel> allpayments = new List<EmployeeOtherPaymentModelForTravel>();
                // if (otherPaymentHeader.Any())
                if (isExistEmployeeOtherPaymentInMonthYear)
                {
                    modelResult.CanInsert = false;
                    var otherPaymentHeaderIds = otherPaymentHeader.Select(x => x.Id).Distinct().ToList();
                    var employeeData = _kscHrUnitOfWork.EmployeeOtherPaymentRepository.GetEmployeeOtherPaymentByHeaderIds(otherPaymentHeaderIds);
                    allpayments = employeeData.Select(x => new EmployeeOtherPaymentModelForTravel
                    {

                        EmployeeNumber = x.Employee.EmployeeNumber,
                        FullName = x.Employee.Name + " " + x.Employee.Family,
                        MaritalStatusTitle = x.Employee.MaritalStatus != null ? x.Employee.MaritalStatus.Title : "",
                        //FamilyCount = familyCount,
                        GenderId = x.Employee.Gender,
                        GenderTitle = x.Employee.Gender.HasValue ? x.Employee.GenderType.Title : "",
                        EmploymentDate = x.Employee.EmploymentDate.HasValue ? x.Employee.EmploymentDate.Value.ToPersianDate() : "",


                        TotalCount = x.PaymentPersonCount.HasValue ? x.PaymentPersonCount.Value : 0,
                        Price = x.PaymentAmount,
                        IsBlackList = x.IsBlacklist
                    }).ToList();
                    //
                }
                else
                {
                    modelResult.CanInsert = true;
                    allpayments = GetPersonalManagmentInTravel(search);
                }
                //
                var resultemployee = _FilterHandler.GetFilterResult<EmployeeOtherPaymentModelForTravel>(allpayments, search, "EmployeeNumber");

                modelResult.Data = resultemployee.Data;
                modelResult.Total = resultemployee.Total;
                modelResult.SumPrice = $"{allpayments.Sum(x => x.Price):N0}";
                modelResult.SumCountRow = allpayments.Where(x => !x.IsBlackList).Sum(x => x.TotalCount);
                modelResult.SalaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData().SalaryDate;

            }
            catch (Exception ex)
            {

                modelResult.ErrorMessage = ex.Message;
            }
            return modelResult;

        }

        private List<EmployeeOtherPaymentModelForTravel> GetStaticalFamilyManagmentReport(IQueryable<Employee> employee,
    OtherPaymentSetting setting, long? kUnit
    , int minimumPersonForTravelPaymentValue, int maximumPersonForTravelPaymentValue
    )
        {
            var data = new List<EmployeeOtherPaymentModelForTravel>();



            if (setting != null)
            {

                foreach (var item in employee)
                {
                    var ee = new EmployeeOtherPaymentModelForTravel();

                    var familyCount = item.Families
                        .Where(x => x.EndDateDependent.HasValue &&
                        x.EndDateDependent.Value > DateTime.Now ||
                        x.EndDateDependent == null
                        )
                        .Count();
                    ee.Id = item.Id;
                    ee.EmployeeNumber = item.EmployeeNumber;
                    ee.FullName = item.Name + " " + item.Family;
                    ee.MaritalStatusTitle = item.MaritalStatus != null ? item.MaritalStatus.Title : "";
                    ee.FamilyCount = familyCount;
                    ee.GenderId = item.Gender;
                    ee.GenderTitle = item.Gender.HasValue ? item.GenderType.Title : "";
                    ee.EmploymentDate = item.EmploymentDate.HasValue ? item.EmploymentDate.Value.ToPersianDate() : "";

                    var totalCount = (familyCount + 1 == 1) ?
                           minimumPersonForTravelPaymentValue : (familyCount + 1 > maximumPersonForTravelPaymentValue) ?
       maximumPersonForTravelPaymentValue : familyCount + 1;
                    ee.TotalCount = totalCount;
                    ee.Price = Convert.ToDecimal(totalCount * (kUnit * setting.KPercent));
                    data.Add(ee);
                }


            }
            return data;

        }

        private List<EmployeeOtherPaymentModelForTravel> GetStaticalFamilyEmployeeReport(IQueryable<Employee> employee,
            OtherPaymentSetting setting, long? kUnit
            , int minimumPersonForTravelPaymentValue, int maximumPersonForTravelPaymentValue
            )
        {
            var data = new List<EmployeeOtherPaymentModelForTravel>();



            if (setting != null)
            {
                List<int> invalidDependentExitDate = new List<int> { EnumDependentExitDateReason.Settlement.Id, EnumDependentExitDateReason.Death.Id };

                foreach (var item in employee)
                {
                    var ee = new EmployeeOtherPaymentModelForTravel();
                    //  var families = _kscHrUnitOfWork.FamilyRepository.GetAllQueryable().OrderBy(x => x.StartDateDependent).Where(fa => (fa.DependenceTypeId == EnumDependenceType.Wife.Id && fa.EndDateDependent == null) || (fa.DependenceTypeId == EnumDependenceType.Husband.Id || fa.DependenceTypeId == EnumDependenceType.Shohar.Id && fa.EndDateDependent.HasValue && !invalidDependentExitDate.Any(p => p == fa.DependentExitDateReasonId)));
                    List<int> invalidDependency = new List<int>() { EnumDependenceType.Father.Id, EnumDependenceType.Mother.Id };
                    var familyData = item.Families.Where(x => x.DependentExitDateReasonId == null || !invalidDependentExitDate.Any(p => p == x.DependentExitDateReasonId));
                    var familyCount = familyData
                      .Where(x =>

                      x.EndDateDependent == null ||
                      (
                        (x.EndDateDependent.HasValue && x.EndDateDependent.Value > DateTime.Now)
                       || (x.DependenceTypeId == EnumDependenceType.Daughter.Id && x.IsContinuesDependent)
                      )
                      ).Count();
                    if (item.Gender == EnumEmployeeGenderType.Woman.Id)
                    {
                        var families = familyData.Where(x => invalidDependency.Any(i => i == x.DependenceTypeId) == false);
                        familyCount = families.Where(x => x.EndDateDependent == null ||
                      ((
                        (x.EndDateDependent.HasValue && x.EndDateDependent.Value > DateTime.Now)
                       || (x.DependenceTypeId == EnumDependenceType.Daughter.Id && x.IsContinuesDependent)
                      )
                      || ((x.DependenceTypeId == EnumDependenceType.Husband.Id || x.DependenceTypeId == EnumDependenceType.Shohar.Id)
                         &&
                         x.EndDateDependent.HasValue && !invalidDependentExitDate.Any(p => p == x.DependentExitDateReasonId))
                      )
                      ).Count();
                        // .Where(x => (x.EndDateDependent.HasValue &&
                        // x.EndDateDependent.Value > DateTime.Now ||
                        // x.EndDateDependent == null) ||
                        // (
                        // (x.DependenceTypeId == EnumDependenceType.Husband.Id || x.DependenceTypeId == EnumDependenceType.Shohar.Id)
                        // &&
                        //x.EndDateDependent.HasValue && !invalidDependentExitDate.Any(p => p == x.DependentExitDateReasonId)
                        // )
                        // ).Count();
                    }

                    ee.Id = item.Id;
                    ee.EmployeeNumber = item.EmployeeNumber;
                    ee.FullName = item.Name + " " + item.Family;
                    ee.MaritalStatusTitle = item.MaritalStatus != null ? item.MaritalStatus.Title : "";
                    ee.FamilyCount = familyCount;
                    ee.GenderId = item.Gender;
                    ee.GenderTitle = item.Gender.HasValue ? item.GenderType.Title : "";
                    ee.EmploymentDate = item.EmploymentDate.HasValue ? item.EmploymentDate.Value.ToPersianDate() : "";

                    var totalCount = (familyCount + 1 == 1) ?
                           minimumPersonForTravelPaymentValue : (familyCount + 1 > maximumPersonForTravelPaymentValue) ?
       maximumPersonForTravelPaymentValue : familyCount + 1;
                    ee.TotalCount = totalCount;
                    ee.Price = Convert.ToDecimal(totalCount * (kUnit * setting.KPercent));
                    data.Add(ee);
                }


            }
            return data;

        }

        private EmployeeHaveTravelPayModel GetPersoneIsCanInLeveList(SearchEmployeeTravelPayModel Filter, List<SpGetEmployeeLeaveStatusReturnModel> models, bool skiptHoliday, List<int> dateKeys)
        {
            var FinalList = new EmployeeHaveTravelPayModel();
            var count = 0;
            var countLeve = models.ToList().Count(x => x.IsLeave == 1);
            var isLEaveInHoliday = false;

            foreach (var x in models.ToList())
            {
                var isSkipedHolayKey = dateKeys.Any(a => a == x.DateKey) && skiptHoliday;

                if (x.IsLeave == 1 || (x.IsLeave == 0 && isSkipedHolayKey))
                {
                    count++;
                    if (count == Filter.CountDay)
                    {
                        if (Filter.MinLeve > 0 && Filter.MaxLeve == 0)
                        {
                            if (countLeve >= Filter.MinLeve)
                            {
                                count = 0;
                                var itemList = new EmployeeHaveTravelPayModel()
                                {
                                    PersonnelNumber = x.EmployeeNumber,
                                    Datetime = x.DateKey.ToString(),
                                    EmployeeId = x.EmployeeId
                                };
                                return itemList;
                                break;
                            }
                        }
                        else if (Filter.MinLeve == 0 && Filter.MaxLeve > 0)
                        {
                            if (countLeve <= Filter.MaxLeve)
                            {
                                count = 0;
                                var itemList = new EmployeeHaveTravelPayModel()
                                {
                                    PersonnelNumber = x.EmployeeNumber,
                                    Datetime = x.DateKey.ToString(),
                                    EmployeeId = x.EmployeeId
                                };
                                return itemList;
                                break;
                            }
                        }
                        else
                        {
                            if (countLeve >= Filter.MinLeve && countLeve <= Filter.MaxLeve)
                            {
                                count = 0;
                                var itemList = new EmployeeHaveTravelPayModel()
                                {
                                    PersonnelNumber = x.EmployeeNumber,
                                    Datetime = x.DateKey.ToString(),
                                    EmployeeId = x.EmployeeId
                                };
                                return itemList;
                                break;
                            }
                        }

                    }
                }
                else
                {

                    count = 0;
                }
            }
            return FinalList;
        }

        /// <summary>
        /// گرفتن لیست کارمندانی که در این بازه زمانی پرداخت شده
        /// که باید از لیست پرداختی کم بشه
        /// </summary>
        /// <param name="payType">نوع پرداختی:هزینه سفر</param>
        private List<EmployeeHaveTravelPayModel> GetPayedEmployee(int StartDate, int? EndDate, int payType)
        {
            var employeePayment = new List<EmployeeHaveTravelPayModel>();
            int endFinalDate = (EndDate == null) ? 0 : EndDate.Value;
            //var sDate = Convert.ToInt32(StartDate.ToEnglishNumbers());
            //var eDate = Convert.ToInt32(EndDate.ToEnglishNumbers());
            var isExist = _kscHrUnitOfWork.EmployeeOtherPaymentRepository.IsExistEmployeeOtherPaymentInMonthYear(StartDate, endFinalDate, payType);
            if (isExist == true)
            {

                employeePayment = _kscHrUnitOfWork
                    .EmployeeOtherPaymentRepository
                    .GetEmployeeOtherPaymentInMonthYear(StartDate, endFinalDate, payType)
                    .Select(x => new EmployeeHaveTravelPayModel
                    {
                        EmployeeId = x.EmployeeId
                    })
                    .ToList();
            }


            return employeePayment;
        }

        /// <summary>
        /// گرفتن لیست کارمندانی که در این بازه زمانی پرداخت شده
        /// که باید از لیست پرداختی کم بشه
        /// </summary>
        /// <param name="payType">نوع پرداختی:هزینه سفر</param>
        private List<EmployeeHaveTravelPayModel> GetPayedManagmentEmployee(List<int> ids, int StartDate, int? EndDate, int payType)
        {
            var employeePayment = new List<EmployeeHaveTravelPayModel>();
            int endFinalDate = (EndDate == null) ? 0 : EndDate.Value;

            var isExist = _kscHrUnitOfWork.EmployeeOtherPaymentRepository.IsExistEmployeeOtherPaymentInMonthYear(StartDate, endFinalDate, payType);
            if (isExist == true)
            {

                employeePayment = _kscHrUnitOfWork
                    .EmployeeOtherPaymentRepository
                    .GetEmployeeOtherPaymentManagmentInMonthYear(StartDate, endFinalDate, payType, ids)
                    .Select(x => new EmployeeHaveTravelPayModel
                    {
                        EmployeeId = x.EmployeeId
                    })
                    .ToList();
            }


            return employeePayment;
        }

        //============================================================

        //public async Task<EmployeeOtherPaymentForMariedAndBirthDayModel> GetEmployeeOtherPaymentForMariedAndBirthDayOld(OtherPaymentSearchPanelModel search)
        //{
        //    List<int> invalidPaymentStatus = new List<int> { EnumPaymentStatus.DismiisalEmployee.Id, EnumPaymentStatus.NewEmployee.Id, EnumPaymentStatus.LeavedEmployee.Id };
        //    var personels = _kscHrUnitOfWork.ViewEmployeeForOtherPaymentMariedAndBirthDayRepository.Where(x => !invalidPaymentStatus.Any(p => p == x.PaymentStatusId));
        //    var viewmisemployee = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetAllQueryable();
        //    int? yearMonthStartReport = Convert.ToInt32(search.YearMonthStartReport.ToEnglishNumbers());
        //    int? yearMonthEndReport = Convert.ToInt32(search.YearMonthEndReport.ToEnglishNumbers());
        //    var families = _kscHrUnitOfWork.ViewFamilyForOtherPaymentMariedAndBirthDayRepository.GetAllQueryable().OrderBy(x => x.StartDateDependent).Where(fa => (fa.DependenceTypeId == EnumDependenceType.Wife.Id && fa.EndDateDependent == null) || (fa.DependenceTypeId == EnumDependenceType.Husband.Id && fa.EndDateDependent.HasValue && fa.DependentExitDateReasonId.HasValue == false));
        //    var salarymodel = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
        //    var salaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetSalaryYear(salarymodel);
        //    var kvalueunit = _kscHrUnitOfWork.KUnitSettingRepository.GetKUnitByYear(salaryDate);
        //    var salarymonth = Utility.GetPersianMonth(salarymodel.SalaryDate.ToString());
        //    var blacklist = _kscHrUnitOfWork.EmployeeBlackListRepository.GetAllByDate(salarymonth.StartDate, salarymonth.EndDate, new int[] { search.OtherPaymentTypeId.Value });
        //    var query =  (from p in personels
        //                       join m in viewmisemployee
        //                       on p.EmployeeNumber equals m.EmployeeNumber
        //                       join f in families
        //                       on p.Id equals f.EmployeeId
        //                       into family
        //                       from fa in family.DefaultIfEmpty()
        //                       where (fa.StartDateDependent == (families.Where(a => a.EmployeeId == p.Id).Min(a => a.StartDateDependent)))
        //                       && (p.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id && p.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id)
        //                       && m.CostCenterCode != null
        //                       //&& !blacklist.Any(a => a.EmployeeId == p.Id)
        //                       select new
        //                       {
        //                           p.Id,
        //                           p.EmployeeNumber,
        //                           m.CostCenterCode,
        //                           NameFamily = p.Name + " " + p.Family,
        //                           p.BirthDate,
        //                           p.MonthBirthDate,
        //                           p.MarriedDate,
        //                           p.MonthMarriedDate,
        //                           p.MaritalStatusId,
        //                           fa,
        //                           IsBlackList = blacklist.Any(a => a.EmployeeId == p.Id)
        //                       });
        //    var settings = _kscHrUnitOfWork.OtherPaymentSettingRepository.GetAllByFilterYearMonthAccountCode(yearMonthStartReport, yearMonthEndReport, search.AccountCodeId.Value).ToList();
        //    int monthstart = Convert.ToInt32(search.YearMonthStartReport.ToEnglishNumbers().Substring(4, 2));
        //    var settingbirthday = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.BirthDay.Id);
        //    var settingmaried = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.MariedDay.Id);
        //    var settingbirthdaymaried = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.FamilyBirthDay.Id);
        //    int monthend = Convert.ToInt32(search.YearMonthEndReport.ToEnglishNumbers().Substring(4, 2));
        //    var result = query.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList()
        //         .Select(p => new
        //         {
        //             p.Id,
        //             p.EmployeeNumber,
        //             p.CostCenterCode,
        //             p.NameFamily,
        //             p.BirthDate,
        //             MonthBirthDate = p.MonthBirthDate,
        //             HasBirthDate = (settingbirthday != null && (p.BirthDate.HasValue && p.MonthBirthDate >= monthstart &&
        //                                                      p.MonthBirthDate <= monthend)),
        //             HasMarriedDate = (settingmaried != null && p.MaritalStatusId == 1 && p.MarriedDate.HasValue
        //                                                        && p.MonthMarriedDate >= monthstart && p.MonthMarriedDate <= monthend),
        //             HasBirthdaymaried = p.fa != null ? ((settingbirthdaymaried != null && (p.MaritalStatusId == 1 && p.fa.BirthDate != null && p.fa.MonthBirthDate >= monthstart && p.fa.MonthBirthDate <= monthend))) : false,
        //             p.MarriedDate,
        //             p.MaritalStatusId,
        //             FamilyBirthDate = p.fa != null ? p.fa.BirthDate : (DateTime?)null,
        //             IsBlackList = p.IsBlackList
        //         }).OrderBy(x => x.EmployeeNumber).ToList();
        //    result = result.Where(x =>
        //     (
        //          x.HasBirthDate
        //       || x.HasMarriedDate
        //       || x.HasBirthdaymaried
        //       )).ToList();
        //    var allpayments = result
        //      .Select(x => new EmployeeOtherPaymentModel()
        //      {
        //          Id = x.Id,
        //          FullName = x.NameFamily,
        //          HasBirthDate = x.HasBirthDate,
        //          BirthDate = x.HasBirthDate ? x.BirthDate : null,
        //          HasMarriedDate = x.HasMarriedDate,
        //          MarriedDate = x.HasMarriedDate ? x.MarriedDate : null,
        //          EmployeeNumber = x.EmployeeNumber,
        //          CostCenterCode = x.CostCenterCode,
        //          HasBirthdayMaried = x.HasBirthdaymaried,
        //          FamilyBirthDate = x.HasBirthdaymaried ? x.FamilyBirthDate : null,
        //          BirthDateUnit = (x.HasBirthDate ? (settingbirthday.KPercent * kvalueunit) : 0),
        //          BirthDateMariedUnit = (x.HasBirthdaymaried ? (settingbirthdaymaried.KPercent * kvalueunit) : 0),
        //          MarriedDateUnit = (x.HasMarriedDate ? (settingmaried.KPercent * kvalueunit) : 0),
        //          Price = (x.HasBirthDate ? (settingbirthday.KPercent * kvalueunit) : 0) + (x.HasBirthdaymaried ? (settingbirthdaymaried.KPercent * kvalueunit) : 0) + (x.HasMarriedDate ? (settingmaried.KPercent * kvalueunit) : 0),
        //          Count = (x.HasBirthDate ? 1 : 0) + (x.HasBirthdaymaried ? 1 : 0) + (x.HasMarriedDate ? 1 : 0),
        //          IsBlackList = x.IsBlackList
        //      }).OrderBy(x => x.EmployeeNumber).Distinct();

        //    var model = new EmployeeOtherPaymentForMariedAndBirthDayModel()
        //    {
        //        EmployeeOtherPayments = allpayments.ToList(),
        //        OtherPaymentTypeIds = settings.Select(x => x.OtherPaymentTypeId).ToList(),
        //        SalaryDate = salarymodel.SalaryDate
        //    };
        //    return model;
        //}
        public async Task<EmployeeOtherPaymentForMariedAndBirthDayModel> GetEmployeeOtherPaymentForMariedAndBirthDay(OtherPaymentSearchPanelModel search)
        {
            List<int> invalidPaymentStatus = new List<int> { EnumPaymentStatus.DismiisalEmployee.Id, EnumPaymentStatus.NewEmployee.Id, EnumPaymentStatus.LeavedEmployee.Id };
            var personels = _kscHrUnitOfWork.EmployeeRepository.GetEmploymentPerson().Where(x => !invalidPaymentStatus.Any(p => p == x.PaymentStatusId));
            var viewmisemployee = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetAllQueryable();
            int? yearMonthStartReport = Convert.ToInt32(search.YearMonthStartReport.ToEnglishNumbers());
            int? yearMonthEndReport = Convert.ToInt32(search.YearMonthEndReport.ToEnglishNumbers());
            int yearstart = Convert.ToInt32(search.YearMonthStartReport.Substring(0, 4).ToEnglishNumbers());
            int yearend = Convert.ToInt32(search.YearMonthEndReport.Substring(0, 4).ToEnglishNumbers());
            if (yearstart != yearend)
            {
                throw new Exception("بازه سال انتخاب شده در یک سال باشد");
                //return null;
            }
            List<int> invalidDependentExitDate = new List<int> { EnumDependentExitDateReason.Settlement.Id, EnumDependentExitDateReason.Death.Id };
            var families = _kscHrUnitOfWork.FamilyRepository.GetAllQueryable().OrderBy(x => x.StartDateDependent).Where(fa => (fa.DependenceTypeId == EnumDependenceType.Wife.Id && fa.EndDateDependent == null) || ((fa.DependenceTypeId == EnumDependenceType.Husband.Id || fa.DependenceTypeId == EnumDependenceType.Shohar.Id) && fa.EndDateDependent.HasValue && !invalidDependentExitDate.Any(p => p == fa.DependentExitDateReasonId)));
            var salarymodel = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var salaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetSalaryYear(salarymodel);
            var kvalueunit = _kscHrUnitOfWork.KUnitSettingRepository.GetKUnitByYear(salaryDate);
            var salarymonth = Utility.GetPersianMonth(salarymodel.SalaryDate.ToString());
            var settings = _kscHrUnitOfWork.OtherPaymentSettingRepository.GetAllByFilterYearMonthAccountCode(yearMonthStartReport, yearMonthEndReport, search.AccountCodeId.Value).ToList();
            var blacklist = _kscHrUnitOfWork.EmployeeBlackListRepository.GetAllByDate(salarymonth.StartDate, salarymonth.EndDate, settings.Select(x => x.OtherPaymentTypeId).ToArray());
            var typesemployment_query = _kscHrUnitOfWork.EmployeeOtherPaymentTypeRepository.GetAllByYear(yearstart).Select(x => new { x.EmployeeOtherPayment.EmployeeId, x.OtherPaymentTypeId });
            var typesemployment = typesemployment_query.ToList();
            var query = await (from p in personels
                               join m in viewmisemployee
                               on p.EmployeeNumber equals m.EmployeeNumber
                               join f in families
                               on p.Id equals f.EmployeeId
                               into family
                               from fa in family.DefaultIfEmpty()
                               where (fa.StartDateDependent == (families.Where(a => a.EmployeeId == p.Id).Min(a => a.StartDateDependent)))
                               && (p.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id && p.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id)
                               && m.CostCenterCode != null
                               //  && !typesemployment.Any(a => a.EmployeeOtherPayment.EmployeeId == p.Id)
                               select new
                               {
                                   p.Id,
                                   p.EmployeeNumber,
                                   m.CostCenterCode,
                                   NameFamily = p.Name + " " + p.Family,
                                   p.BirthDate,
                                   MonthBirthDate = p.BirthDate.HasValue ? p.BirthDate.ConvertToMonthPersianYearMonthDay() : null,
                                   p.MarriedDate,
                                   EmploymentDate = p.EmploymentDate,
                                   //YearEmploymentDate = p.EmploymentDate.Value.ToPersianYearMonthDay(false).Year,
                                   MonthMarriedDate = p.MarriedDate.HasValue ? p.MarriedDate.ConvertToMonthPersianYearMonthDay() : null,
                                   p.MaritalStatusId,
                                   FamilyBirthDate = fa != null ? fa.BirthDate : (DateTime?)null,
                                   MonthFamilyBirthDate = fa != null ? (Utility.ConvertToMonthPersianYearMonthDay(fa.BirthDate)) : null,
                                   IsBlackList = blacklist.Any(a => a.EmployeeId == p.Id),
                                   //typesemployments = typesemployment.Where(a => a.EmployeeOtherPayment.EmployeeId == p.Id).ToList()
                               }).ToListAsync();
            int monthstart = Convert.ToInt32(search.YearMonthStartReport.ToEnglishNumbers().Substring(4, 2));
            var settingbirthday = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.BirthDay.Id);
            var settingmaried = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.MariedDay.Id);
            var settingbirthdaymaried = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.FamilyBirthDay.Id);
            int monthend = Convert.ToInt32(search.YearMonthEndReport.ToEnglishNumbers().Substring(4, 2));
            ////// اینجا خیلیییییی کنده!!!!!!! چ خبره؟؟؟؟؟؟؟؟؟؟
            var result_p = query.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
            var result = result_p.Select(p => new
            {
                p.Id,
                p.EmployeeNumber,
                p.CostCenterCode,
                p.NameFamily,
                p.BirthDate,

                HasBirthDate = (settingbirthday != null && (p.BirthDate.HasValue && p.MonthBirthDate >= monthstart &&
                                                              p.MonthBirthDate <= monthend)) && !typesemployment.Any(a => a.EmployeeId == p.Id && a.OtherPaymentTypeId == EnumPaymentType.BirthDay.Id),
                HasMarriedDate = (settingmaried != null && p.MaritalStatusId == 1 && p.MarriedDate.HasValue
                                                            //&& ((p.EmploymentDate.Year == salaryDate) ? (p.MonthMarriedDate <= p.EmploymentDate.Month) : true)
                                                            && p.MonthMarriedDate >= monthstart && p.MonthMarriedDate <= monthend)
                                                                && !typesemployment.Any(a => a.EmployeeId == p.Id && a.OtherPaymentTypeId == EnumPaymentType.MariedDay.Id)
                                                                                                                                       ,
                HasBirthdaymaried = p.FamilyBirthDate != null ? ((!typesemployment.Any(a => a.EmployeeId == p.Id && a.OtherPaymentTypeId == EnumPaymentType.FamilyBirthDay.Id) && settingbirthdaymaried != null && (p.MaritalStatusId == 1 && p.FamilyBirthDate != null && p.MonthFamilyBirthDate >= monthstart && p.MonthFamilyBirthDate <= monthend))) : false,
                p.MarriedDate,
                EmploymentDate = p.EmploymentDate,
                p.MaritalStatusId,
                FamilyBirthDate = p.FamilyBirthDate,
                IsBlackList = p.IsBlackList
            }).OrderBy(x => x.EmployeeNumber).ToList();
            //result = result.Where(a => a.EmployeeNumber == "422738").ToList();
            ////////////
            result = result.Where(x =>
             (
                  x.HasBirthDate
               || x.HasMarriedDate
               || x.HasBirthdaymaried
               )).ToList();
            var allpayments = result
              .Select(x => new EmployeeOtherPaymentModel()
              {
                  Id = x.Id,
                  FullName = x.NameFamily,
                  EmploymentDate = x.EmploymentDate,
                  HasBirthDate = x.HasBirthDate,
                  BirthDate = x.HasBirthDate ? x.BirthDate : null,
                  HasMarriedDate = x.HasMarriedDate,
                  MarriedDate = x.HasMarriedDate ? x.MarriedDate : null,
                  EmployeeNumber = x.EmployeeNumber,
                  CostCenterCode = x.CostCenterCode,
                  HasBirthdayMaried = x.HasBirthdaymaried,
                  FamilyBirthDate = x.HasBirthdaymaried ? x.FamilyBirthDate : null,
                  BirthDateUnit = (x.HasBirthDate ? (settingbirthday.KPercent * kvalueunit) : 0),
                  BirthDateMariedUnit = (x.HasBirthdaymaried ? (settingbirthdaymaried.KPercent * kvalueunit) : 0),
                  MarriedDateUnit = (x.HasMarriedDate ? (settingmaried.KPercent * kvalueunit) : 0),
                  Price = (x.HasBirthDate ? (settingbirthday.KPercent * kvalueunit) : 0) + (x.HasBirthdaymaried ? (settingbirthdaymaried.KPercent * kvalueunit) : 0) + (x.HasMarriedDate ? (settingmaried.KPercent * kvalueunit) : 0),
                  Count = (x.HasBirthDate ? 1 : 0) + (x.HasBirthdaymaried ? 1 : 0) + (x.HasMarriedDate ? 1 : 0),
                  IsBlackList = x.IsBlackList
              }).Where(x => x.Count > 0).OrderBy(x => x.EmployeeNumber).Distinct();

            var model = new EmployeeOtherPaymentForMariedAndBirthDayModel()
            {
                EmployeeOtherPayments = allpayments.ToList(),
                OtherPaymentTypeIds = settings.Select(x => x.OtherPaymentTypeId).ToList(),
                SalaryDate = salarymodel.SalaryDate
            };
            return model;
        }
        public async Task<EmployeeOtherPaymentForMariedAndBirthDayModel> GetEmployeeOtherPaymentForMariedAndBirthDay1111(OtherPaymentSearchPanelModel search)
        {
            List<int> invalidPaymentStatus = new List<int> { EnumPaymentStatus.DismiisalEmployee.Id, EnumPaymentStatus.NewEmployee.Id, EnumPaymentStatus.LeavedEmployee.Id };
            var personels = _kscHrUnitOfWork.ViewEmployeeForOtherPaymentMariedAndBirthDayRepository.Where(x => !invalidPaymentStatus.Any(p => p == x.PaymentStatusId));
            var viewmisemployee = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetAllQueryable();
            int? yearMonthStartReport = Convert.ToInt32(search.YearMonthStartReport.ToEnglishNumbers());
            int? yearMonthEndReport = Convert.ToInt32(search.YearMonthEndReport.ToEnglishNumbers());
            int yearstart = Convert.ToInt32(search.YearMonthStartReport.Substring(0, 4).ToEnglishNumbers());
            int yearend = Convert.ToInt32(search.YearMonthEndReport.Substring(0, 4).ToEnglishNumbers());
            if (yearstart != yearend)
            {
                throw new Exception("بازه سال انتخاب شده در یک سال باشد");
                //return null;
            }
            List<int> invalidDependentExitDate = new List<int> { EnumDependentExitDateReason.Settlement.Id, EnumDependentExitDateReason.Death.Id };
            var families = _kscHrUnitOfWork.ViewFamilyForOtherPaymentMariedAndBirthDayRepository.GetAllQueryable().OrderBy(x => x.StartDateDependent).Where(fa => (fa.DependenceTypeId == EnumDependenceType.Wife.Id && fa.EndDateDependent == null) || ((fa.DependenceTypeId == EnumDependenceType.Husband.Id || fa.DependenceTypeId == EnumDependenceType.Shohar.Id) && fa.EndDateDependent.HasValue && !invalidDependentExitDate.Any(p => p == fa.DependentExitDateReasonId)));
            var salarymodel = _kscHrUnitOfWork.SystemControlDateRepository.GetActiveData();
            var salaryDate = _kscHrUnitOfWork.SystemControlDateRepository.GetSalaryYear(salarymodel);
            var kvalueunit = _kscHrUnitOfWork.KUnitSettingRepository.GetKUnitByYear(salaryDate);
            var salarymonth = Utility.GetPersianMonth(salarymodel.SalaryDate.ToString());
            var settings = _kscHrUnitOfWork.OtherPaymentSettingRepository.GetAllByFilterYearMonthAccountCode(yearMonthStartReport, yearMonthEndReport, search.AccountCodeId.Value).ToList();
            var blacklist = _kscHrUnitOfWork.EmployeeBlackListRepository.GetAllByDate(salarymonth.StartDate, salarymonth.EndDate, settings.Select(x => x.OtherPaymentTypeId).ToArray());
            var typesemployment = _kscHrUnitOfWork.EmployeeOtherPaymentTypeRepository.GetAllByYear(yearstart).Select(x => new { x.EmployeeOtherPayment.EmployeeId, x.OtherPaymentTypeId });
            var query = (from p in personels
                         join m in viewmisemployee
                         on p.EmployeeNumber equals m.EmployeeNumber
                         join f in families
                         on p.Id equals f.EmployeeId
                         into family
                         from fa in family.DefaultIfEmpty()
                         where (fa != null && fa.StartDateDependent == (families.Where(a => a.EmployeeId == p.Id).Min(a => a.StartDateDependent)))
                         && (p.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id && p.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id)
                         && m.CostCenterCode != null
                         //  && !typesemployment.Any(a => a.EmployeeOtherPayment.EmployeeId == p.Id)
                         select new
                         {
                             p.Id,
                             p.EmployeeNumber,
                             m.CostCenterCode,
                             NameFamily = p.Name + " " + p.Family,
                             p.BirthDate,
                             p.MarriedDate,
                             p.EmploymentDate,
                             p.MaritalStatusId,
                             p.MonthBirthDate,
                             fa,
                             IsBlackList = blacklist.Any(a => a.EmployeeId == p.Id),
                             //typesemployments = typesemployment.Where(a => a.EmployeeOtherPayment.EmployeeId == p.Id).ToList()
                         });
            //var query_group = (from item in query
            //                   group item by new
            //                   {
            //                       Id = item.Id,
            //                       IsBlackList = item.IsBlackList,
            //                       EmployeeNumber = item.EmployeeNumber,
            //                       item.CostCenterCode,
            //                       NameFamily = item.NameFamily,
            //                       item.BirthDate,
            //                       item.MarriedDate,
            //                       item.MonthBirthDate,
            //                       item.EmploymentDate,
            //                       item.MaritalStatusId,

            //                   } into newgroup
            //                   select new
            //                   {
            //                       newgroup.Key.Id,
            //                       EmployeeNumber = newgroup.Key.EmployeeNumber,
            //                       newgroup.Key.CostCenterCode,
            //                       NameFamily = newgroup.Key.NameFamily,
            //                       newgroup.Key.BirthDate,
            //                       newgroup.Key.MarriedDate,
            //                       newgroup.Key.MonthBirthDate,
            //                       newgroup.Key.EmploymentDate,
            //                       newgroup.Key.MaritalStatusId,
            //                       IsBlackList = newgroup.Key.IsBlackList,
            //                       newgroup.FirstOrDefault().fa,
            //                   });

            int monthstart = Convert.ToInt32(search.YearMonthStartReport.ToEnglishNumbers().Substring(4, 2));
            var settingbirthday = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.BirthDay.Id);
            var settingmaried = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.MariedDay.Id);
            var settingbirthdaymaried = settings.FirstOrDefault(x => x.OtherPaymentTypeId == EnumPaymentType.FamilyBirthDay.Id);
            int monthend = Convert.ToInt32(search.YearMonthEndReport.ToEnglishNumbers().Substring(4, 2));
            bool hasSettingbirthday = settingbirthday != null;
            var result = query.GroupBy(x => x.Id).Select(x => x.FirstOrDefault())//.ToList()
                 .Select(p => new
                 {
                     p.Id,
                     p.EmployeeNumber,
                     p.CostCenterCode,
                     p.NameFamily,
                     p.BirthDate,
                     MonthBirthDate = p.MonthBirthDate,
                     HasBirthDate = hasSettingbirthday && (p.BirthDate.HasValue && p.MonthBirthDate >= monthstart &&
                                                             p.MonthBirthDate <= monthend) &&
                                                             !typesemployment.Any(a => a.EmployeeId == p.Id && a.OtherPaymentTypeId == EnumPaymentType.BirthDay.Id),

                     HasMarriedDate = (settingmaried != null && p.MaritalStatusId == 1 && p.MarriedDate.HasValue && p.MarriedDate >= p.EmploymentDate
                                                            && p.MonthBirthDate >= monthstart && p.MonthBirthDate <= monthend) && !typesemployment.Any(a => a.EmployeeId == p.Id && a.OtherPaymentTypeId == EnumPaymentType.MariedDay.Id),
                     HasBirthdaymaried = p.fa != null ? ((!typesemployment.Any(a => a.EmployeeId == p.Id && a.OtherPaymentTypeId == EnumPaymentType.FamilyBirthDay.Id) && settingbirthdaymaried != null && (p.MaritalStatusId == 1 && (p.fa.BirthDate != null && p.fa.MonthBirthDate >= monthstart && p.fa.MonthBirthDate <= monthend)))) : false,
                     p.MarriedDate,
                     p.MaritalStatusId,
                     FamilyBirthDate = p.fa != null ? p.fa.BirthDate : (DateTime?)null,
                     IsBlackList = p.IsBlackList
                 });
            result = result.Where(x =>
             (
                  x.HasBirthDate
               || x.HasMarriedDate
               || x.HasBirthdaymaried
               )).AsQueryable();
            var allpayments = result
              .Select(x => new EmployeeOtherPaymentModel()
              {
                  Id = x.Id,
                  FullName = x.NameFamily,
                  HasBirthDate = x.HasBirthDate,
                  BirthDate = x.HasBirthDate ? x.BirthDate : null,
                  HasMarriedDate = x.HasMarriedDate,
                  MarriedDate = x.HasMarriedDate ? x.MarriedDate : null,
                  EmployeeNumber = x.EmployeeNumber,
                  CostCenterCode = x.CostCenterCode,
                  HasBirthdayMaried = x.HasBirthdaymaried,
                  FamilyBirthDate = x.HasBirthdaymaried ? x.FamilyBirthDate : null,
                  BirthDateUnit = (x.HasBirthDate ? (settingbirthday.KPercent * kvalueunit) : 0),
                  BirthDateMariedUnit = (x.HasBirthdaymaried ? (settingbirthdaymaried.KPercent * kvalueunit) : 0),
                  MarriedDateUnit = (x.HasMarriedDate ? (settingmaried.KPercent * kvalueunit) : 0),
                  Price = (x.HasBirthDate ? (settingbirthday.KPercent * kvalueunit) : 0) + (x.HasBirthdaymaried ? (settingbirthdaymaried.KPercent * kvalueunit) : 0) + (x.HasMarriedDate ? (settingmaried.KPercent * kvalueunit) : 0),
                  Count = (x.HasBirthDate ? 1 : 0) + (x.HasBirthdaymaried ? 1 : 0) + (x.HasMarriedDate ? 1 : 0),
                  IsBlackList = x.IsBlackList
              }).Where(x => x.Count > 0).OrderBy(x => x.EmployeeNumber).Distinct().ToList();

            var model = new EmployeeOtherPaymentForMariedAndBirthDayModel()
            {
                EmployeeOtherPayments = allpayments,
                OtherPaymentTypeIds = settings.Select(x => x.OtherPaymentTypeId).ToList(),
                SalaryDate = salarymodel.SalaryDate
            };
            return model;
        }

        /// <summary>
        /// پرداخت متفرقه برای تولد- ازدواج
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModel>> GetEmployeeOtherPaymentByFilter(OtherPaymentSearchPanelModel search)
        {
            var data = await GetEmployeeOtherPaymentForMariedAndBirthDay(search);
            var allpayments = data.EmployeeOtherPayments;
            FilterResult<EmployeeOtherPaymentModel> resultemployee = _FilterHandler.GetFilterResult<EmployeeOtherPaymentModel>(allpayments, search, "EmployeeNumber");

            var modelResult = new EmployeeOtherPaymentFilterResult<EmployeeOtherPaymentModel>
            {
                Data = resultemployee.Data,
                Total = resultemployee.Total,
                SumPrice = $"{allpayments.Where(x => !x.IsBlackList).Sum(x => x.Price):N0}",
                SumCountRow = allpayments.Where(x => !x.IsBlackList).Sum(x => x.Count),
                SalaryDate = data.SalaryDate,
                CanInsert = true && resultemployee.Total > 0
            };
            return modelResult;

        }

        #endregion


        public async Task<KscResult> PostEmployeeWorkType(EmployeeWorkDayTypeModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try
            {
                if (model.HasFloatTime == true && !model.FloatTimeSettingId.HasValue)
                {
                    result.AddError("", "لطفا تایم شناور را انتخاب کنید");

                }

                if (!model.EntryExitTypeId.HasValue)
                {
                    result.AddError("", "نوع ثبت ورود و خروج  باید مقدار داشته باشد.");

                }
                if (!model.WorkCityId.HasValue)
                {
                    result.AddError("", "شهر محل خدمت  باید مقدار داشته باشد.");

                }

                if (!result.Success)
                {
                    return result;
                }

                var employee = GetOne(model.Id);
                model.EmployeeNumber = employee.EmployeeNumber;


                employee.HasFloatTime = model.HasFloatTime;
                //در صورتی که کارکرد شناور تیک دار نباشد زمانش باید خالی باشد
                if (model.HasFloatTime == false)
                {
                    employee.FloatTimeSettingId = null;
                }
                else
                {
                    employee.FloatTimeSettingId = model.FloatTimeSettingId;
                }

                employee.WorkCityId = model.WorkCityId;
                employee.EntryExitTypeId = model.EntryExitTypeId;

                _kscHrUnitOfWork.EmployeeRepository.Update(employee);

                #region SyncMIS
                var condotionModel = new EmployeeConditionModel()
                {
                    EmployeeNumber = model.EmployeeNumber,
                    HasFloatTime = model.HasFloatTime,
                    FloatTimeSettingId = model.FloatTimeSettingId,
                    EntryExitTypeId = model.EntryExitTypeId,
                    WorkCityId = model.WorkCityId,
                    P_function = "WorkCity",
                    PaymentStatusId = employee.PaymentStatusId.HasValue ? employee.PaymentStatusId.Value : 0
                };
                condotionModel.DomainName = model.DomainName;
                var updateMIS = _misUpdateService.UpdateEmployeeConditionModel(condotionModel);
                if (updateMIS.IsSuccess == false)
                {
                    result.AddError("خطا MIS", $"خطا MIS - {string.Join(",", updateMIS.Messages)}");
                    return result;
                }
                #endregion

                if (result.Success == true)
                    await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError("", ex.Message);
            }
            return result;
        }

        private List<EmployeeHaveTravelPayModel> GetPayedEmployeeForTravel(int year)
        {
            var employeePayment = new List<EmployeeHaveTravelPayModel>();


            employeePayment = _kscHrUnitOfWork
                .EmployeeOtherPaymentRepository
                .GetAll().AsQueryable().Include(x => x.OtherPaymentHeader)
                .Where(x => x.OtherPaymentHeader.PaymentYearMonth >= year
                            && x.OtherPaymentHeader.AccountCodeId == EnumSalaryAcountCode.Trip.Id && x.IsBlacklist == false)
                .Select(x => new EmployeeHaveTravelPayModel
                {
                    EmployeeId = x.EmployeeId
                })
                .ToList();



            return employeePayment;
        }


        public async Task<KscResult> ConvertEmployeePicture()
        {
            var result = new KscResult();

            var employeePictureList = new List<EmployeePicture>();
            var employeesList = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().Where(a => !string.IsNullOrEmpty(a.GuidPicId)).ToList();
            var employeeIdsString = employeesList.Select(a => a.Id.ToString()).ToList();


            try
            {
                foreach (var employee in employeesList)
                {
                    var pictureFiles = (await _attachmentService.GetAttachments("HR", "Employee", new List<string> { employee.Id.ToString() }));
                    var pictureFile = pictureFiles.FirstOrDefault(a => a.Name.Contains(employee.GuidPicId.ToString()));
                    if (pictureFile != null)
                    {
                        var model = new EmployeePicture()
                        {
                            Image = pictureFile.Content,
                            PersonalNumber = employee.EmployeeNumber,
                            EmployeeId = employee.Id,
                            InsertDate = DateTime.Now,
                            InsertUser = "system",
                            IsActive = true,
                        };

                        employeePictureList.Add(model);
                    }
                }
                _kscHrUnitOfWork.EmployeePictureRepository.AddRange(employeePictureList);
                _kscHrUnitOfWork.Save(checklog: false);
                return result;

            }
            catch (Exception ex)
            {
                result.AddError("", "");
                return result;
            }



        }
        ///
        public async Task DeActiveWindowsUser(string employeeNumber, string domain)
        {
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
                        if (modelObj.IsSuccess == true)
                        {
                            if (modelObj.Data.IsActive)
                            {
                                throw new Exception("غیر فعالسازی کاربر ویندوز با خطا مواجه شد");
                            }
                        }
                        else
                        {

                            throw new Exception("غیر فعالسازی کاربر ویندوز با خطا مواجه شد");
                        }
                    }
                    else
                    {
                        throw new Exception("غیر فعالسازی کاربر ویندوز با خطا مواجه شد");

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public EmployeeModel GetEmployeeByNationalCode(string nationalCode)
        {
            var employee = _kscHrUnitOfWork.EmployeeRepository.GetByNationalCode(nationalCode)
                  .Select(x => new EmployeeModel
                  {
                      Id = x.Id,
                      EmployeeNumber = x.EmployeeNumber,//شماره پرسنلی
                      PaymentStatusTitle = x.PaymentStatus.Title,//وضعیت استخدام
                      Name = x.Name,
                      Family = x.Family,
                      NationalCode = !string.IsNullOrEmpty(x.NationalCode) ? x.NationalCode.Trim() : "",
                      FatherName = !string.IsNullOrEmpty(x.FatherName) ? x.FatherName.Trim() : "",
                      PaymentStatusId = x.PaymentStatusId

                  }).FirstOrDefault();

            return employee;
        }



    }
}
