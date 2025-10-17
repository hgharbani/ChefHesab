using Ksc.HR.DTO.Emp;
using Ksc.HR.DTO.EmployeeBase.EmploymentStatus;
using Ksc.HR.DTO.EmployeeBase.EmploymentType;
using Ksc.HR.DTO.EmployeeBase.IsarStatus;
using Ksc.HR.DTO.EmployeeBase.SavingType;
using Ksc.HR.DTO.Personal.EmployeeTeamWork;
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.EntryExitType;
using Ksc.HR.DTO.WorkShift.FloatTimeSetting;
using Ksc.HR.DTO.WorkShift.WorkGroup;
using Ksc.HR.DTO.WorkShift.WorkTime;
using Ksc.HR.Resources.Emp;
using Ksc.HR.Resources.Personal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeConditionModel
    {
        public EmployeeConditionModel()
        {
            AvailableEmploymentStatus = new List<SearchEmploymentStatusModel>();
            AvailableEmploymentType = new List<SearchEmploymentTypeModel>();
            AvailableEntryExitType = new List<EntryExitTypeModel>();
            AvailableFloatTimeSetting = new List<FloatTimeSettingModel>();
            AvailableWorkGroup = new List<SearchWorkGroupModel>();
        }
        public string DomainName { get; set; }
        public string P_function { get; set; } 
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EmploymentTypeId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        /// <summary>
        /// نوع استخدام
        /// </summary>
        public int? EmploymentTypeId { get; set; } // EmploymentTypeId
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EmploymentStatusId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        /// <summary>
        /// وضعیت استخدام
        /// </summary>
        public int? EmploymentStatusId { get; set; } // EmploymentStatusId
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EmploymentDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public DateTime? EmploymentDate { get; set; } // EmploymentDate

        public int LastGroupNumber { get; set; }
        /// <summary>
        /// تاریخ شروع قرارداد
        /// </summary>
       // [Display(Name = nameof(ContractStartDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public DateTime? ContractStartDate { get; set; } // ContractStartDate
        /// <summary>
        /// تاریخ پایان قرارداد
        /// </summary>
       // [Display(Name = nameof(ContractEndDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        public DateTime? ContractEndDate { get; set; } // ContractEndDate
        /// <summary>
        /// کارکرد شناور
        /// </summary>
        [Display(Name = nameof(HasFloatTime), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public bool HasFloatTime { get; set; }
        /// <summary>
        /// زمان شناور
        /// </summary>
        [Display(Name = nameof(FloatTimeSettingId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? FloatTimeSettingId { get; set; } // FloatTimeSettingId
        [Display(Name = nameof(WorkCityId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        /// <summary>
        /// شهر محل خدمت
        /// </summary>
        public int? WorkCityId { get; set; } // WorkCityId
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(WorkGroupId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        /// <summary>
        /// گروه کاری
        /// </summary>
        public int? WorkGroupId { get; set; } // WorkGroupId
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EntryExitTypeId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        /// <summary>
        /// نوع ثبت ورود و خروج
        /// </summary>
        public int? EntryExitTypeId { get; set; } // EntryExitTypeId
        /// <summary>
        /// نوع پس انداز
        /// </summary>
        [Display(Name = nameof(SavingTypeId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? SavingTypeId { get; set; } // SavingTypeId

        [Display(Name = nameof(SavingTypeDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        public int? SavingTypeDate { get; set; } // SavingTypeId

        public string CurrentUserName { get; set; }
        public int PaymentStatusId { get; set; }
        public string WorkCityCityTitle { get; set; }

        public int EmployeeSacrificeId { get; set; }
        public int EmployeeEducationDegreeId { get; set; }
        public int EmployeeTeamWorkId { get; set; }
        /// <summary>
        /// کد پست سازمانی
        /// </summary>
        [Required(ErrorMessage = "لطفا کد پست سازمانی را وارد نمایید")]
        [Display(Name = "کد پست سازمانی")]
        public string JobPositionCode { get; set; } //JobPositionCode

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        [Required(ErrorMessage = "لطفا تاریخ شروع  به کار در پست سازمانی را وارد نمایید")]
        [Display(Name = "تاریخ شروع  به کار در پست سازمانی")]
        public DateTime? JobPositionStartDate { get; set; } // EmploymentDate

        public List<SearchEmploymentTypeModel> AvailableEmploymentType { get; set; }
        public List<SearchEmploymentStatusModel> AvailableEmploymentStatus { get; set; }
        //public List<SearchCityModel> AvailableCity { get; set; }
        public List<SearchWorkGroupModel> AvailableWorkGroup { get; set; }
        public List<SearchSavingTypeModel> AvailableSavingType { get; set; }
        public List<EntryExitTypeModel> AvailableEntryExitType { get; set; }
        public List<FloatTimeSettingModel> AvailableFloatTimeSetting { get; set; }

        [Display(Name = nameof(IsarStatusId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? IsarStatusId { get; set; }
        public List<SearchIsarStatusDto> IsarStatuses { get; set; }

        [Display(Name = nameof(SacrificeOptionSettingId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? SacrificeOptionSettingId { get; set; }




        [Display(Name = nameof(SacrificePercentage), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Range(0, 100, ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? SacrificePercentage { get; set; }

        [Display(Name = "مدرک تحصیلی")]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? EducationId { get; set; }

        [Display(Name = "رشته تحصیلی")]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]

        public int? StudyFieldId { get; set; }

        [Display(Name = nameof(EducationDate), ResourceType = typeof(EmployeeEducationDegreeResource))]
        public DateTime? EducationDate { get; set; }

        /// <summary>
        /// گروه کاری
        /// </summary>
        [Required(ErrorMessage = "لطفا تیم کاری  را وارد نمایید")]
        [Display(Name = "تیم کاری ")]
        public int? TeamWorkId { get; set; } // TeamWorkId

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        [Required(ErrorMessage = "لطفا تاریخ شروع تیم  را وارد نمایید")]
        [Display(Name = "تاریخ شروع تیم ")]
        public DateTime? TeamStartDate { get; set; } // TeamStartDate
        public string EmployeeNumber { get; set; }
        public string TeamWorkTitle { get; set; }
        public string EducationTitle { get; set; }
        public string StudyFieldTitle { get; set; }
        public string JobPositionTitle { get; set; }
        public string StudyFieldCode { get; set; }
        public string DismissalDateShamsi { get; set; }
        public string DismissalStatusTitle { get; set; }
        public string JobIdentityDisplay { get; set; }
        public string StructureDisplay { get; set; }
    }

    public class AddOrEditEmployeeConditionModel
    {
        public AddOrEditEmployeeConditionModel()
        {
            AvailableEmploymentStatus = new List<SearchEmploymentStatusModel>();
            AvailableEmploymentType = new List<SearchEmploymentTypeModel>();

        }
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// نوع استخدام
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EmploymentTypeId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? EmploymentTypeId { get; set; } // EmploymentTypeId



        /// <summary>
        /// وضعیت استخدام
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EmploymentStatusId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]

        public int? EmploymentStatusId { get; set; } // EmploymentStatusId


        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EmploymentDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public DateTime? EmploymentDate { get; set; } // EmploymentDate



        public List<SearchEmploymentTypeModel> AvailableEmploymentType { get; set; }
        public List<SearchEmploymentStatusModel> AvailableEmploymentStatus { get; set; }
        public string CurrentUserName { get; set; }
        public string EmployeeNumber { get; set; }

        public int WorkCityId { get; set; }
    }


    public class EmployeeWorkDayTypeModel
    {
        public EmployeeWorkDayTypeModel()
        {
            AvailableEntryExitType = new List<EntryExitTypeModel>();
            AvailableFloatTimeSetting = new List<FloatTimeSettingModel>();
            AvailableWorkGroup = new List<SearchWorkGroupModel>();
        }
        public int Id { get; set; }

        /// <summary>
        /// کارکرد شناور
        /// </summary>
        [Display(Name = nameof(HasFloatTime), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public bool HasFloatTime { get; set; }

        /// <summary>
        /// زمان شناور
        /// </summary>
        [Display(Name = nameof(FloatTimeSettingId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? FloatTimeSettingId { get; set; } // FloatTimeSettingId

        [Display(Name = nameof(WorkCityId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        /// <summary>
        /// شهر محل خدمت
        /// </summary>
        public int? WorkCityId { get; set; } // WorkCityId


 


        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(EntryExitTypeId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        /// <summary>
        /// نوع ثبت ورود و خروج
        /// </summary>
        public int? EntryExitTypeId { get; set; } // EntryExitTypeId

        public string CurrentUserName { get; set; }

        public int PaymentStatusId { get; set; }

        public string WorkCityCityTitle { get; set; }

        public List<SearchWorkGroupModel> AvailableWorkGroup { get; set; }
        public List<SearchSavingTypeModel> AvailableSavingType { get; set; }
        public List<EntryExitTypeModel> AvailableEntryExitType { get; set; }
        public List<FloatTimeSettingModel> AvailableFloatTimeSetting { get; set; }
        public string EmployeeNumber { get; set; }
        public string DomainName { get; set; }

       
    }
}
