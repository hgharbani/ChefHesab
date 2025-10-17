using Ksc.HR.Resources.Personal;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using KSC.Common.Filters.Models;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using System;
using System.Collections.Generic;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeModel:FilterRequest
    {

        public int ChildrentCount { get; set; }
        public int DependentsCount { get; set; }

        public string CurrentUserName { get; set; }

        public bool IsError { get; set; }
        public string MsgError { get; set; }

        //private string TOT_SHFT_PER_JPOS;
        //private string TOT_NRM_PER_JPOS;
        //private string TOT_PER_CNTRC_JPOS;
        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; } // NationalCode (length: 50)

        /// <summary>
        /// نام پدر
        /// </summary>
        public string FatherName { get; set; } // FatherName (length: 500)
        public int? PaymentStatusId { get; set; } // PaymentStatusId

        public string COD_CAT_JOB { get; set; }
        public string TOT_SHFT_PER_JPOS { get; set; } //ظرفیت خالی ساختار (شیفت)

        public string TOT_NRM_PER_JPOS { get; set; }//ظرفیت خالی ساختار (روزکار)

        public string NUM_PER_CNTRC_JPOS { get; set; }//ظرفیت خالی ساختار (موقت)

        public string WorkTimeCode { get; set; }
        public string NoeEstekhdam { get; set; }//نوع استخدام 
        public string JobPositionCode { get; set; }//کد پست 
        public string CallingCostCenter { get; set; }//کد مرکز هزینه 

        public string SubFunctionCode { get; set; }//کد واحد سازمانی

        public string COD_TYP_WRK_EMPL { get; set; }//زمان کار (روزکار/شیفت/دو نوبته و .....)

        public string SubFunctionTitle { get; set; }//عنوان واحد سازمانی

        public string IsOnCalling { get; set; }//نشانگر قابل فراخوان بودن پست

        public string JobPositionTitle { get; set; }//عنوان پست

        public string PersonalTypeTitle { get; set; }
        public int PersonalTypeId { get; set; }
        public string SuperiorJobPositionCode { get; set; }//کد پست مافوق

        public string TeamWorkCode { get; set; }//تیم کاری
        public string TeamWorkId { get; set; }//تیم کاری
        

        public string TeamWorkTitle { get; set; }//عنوان تیم کاری

        //public string COD_WRK_GRP_EMPL { get; set; }//کد گروه کاری (R/A/B/C,….)
        public DateTime? RegisterDate { get; set; }

        public string misEmployeeRegister { get; set; }

        public string CodeEmployeeRegister { get; set; }

        public string RegisterDateString { get { return RegisterDate.HasValue ? RegisterDate.ToLongPersianDateString() : ""; } }

        [Display(Name = nameof(Id), ResourceType = typeof(EmployeeResource))]
        public int Id { get; set; } // Id (Primary key)

        [Display(Name = nameof(EmployeeNumber), ResourceType = typeof(EmployeeResource))]
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)

        [Display(Name = nameof(Name), ResourceType = typeof(EmployeeResource))]
        public string Name { get; set; } // Name (length: 500)

        [Display(Name = nameof(FullName), ResourceType = typeof(EmployeeResource))]
        public string FullName { get { return Name + " " + Family; } }

        [Display(Name = nameof(Family), ResourceType = typeof(EmployeeResource))]
        public string Family { get; set; } // Family (length: 500)

        [Display(Name = nameof(TotalDayVocationLeave), ResourceType = typeof(EmployeeResource))] 
        public int TotalDayVocationLeave { get; set; }

        public List<SearchRollCallDefinicationModel> Sickleaves { get; set; }
        public List<SearchRollCallDefinicationModel> Vocationlistleaves { get; set; }

        public int ShiftConceptDetailsId { get; set; }
        public SearchShiftConceptModel ShiftCode { get; set; } = new SearchShiftConceptModel() { Id=1,Code="R-روزکار"};
        /// <summary>
        /// تاریخ ترک خدمت
        /// </summary>
        public string DAT_DSMSL_EMPL { get; set; }
        /// <summary>
        /// کد علت ترک کار
        /// </summary>
        public string COD_DSMSL_EMPL { get; set; }
        /// <summary>
        /// علت ترک کار
        /// </summary>
        public string Title_DSMSL_EMPL { get; set; }

        /// <summary>
        /// استفاده از منازل
        /// </summary>
        public bool COD_USE_HOUS_EMPL { get; set; }
        /// <summary>
        /// گروه کاری
        /// </summary>
        public string WorkGroupCode { get; set; }
        /// <summary>
        /// مانده مرخصی
        /// </summary>
        public string RemianingVacationInMinute { get; set; }
        /// <summary>
        /// نوع اسخدام
        /// </summary>
        public string EmployeeTypeTitle { get; set; }
        /// <summary>
        /// زمان کاری
        /// </summary>
        public string WorkTimeTitle { get; set; }
        /// <summary>
        /// وضعیت اشتغال به کار
        /// </summary>
        public string PaymentStatusTitle { get; set; }
        public int CountLeve { get; set; }
        public string TeamWork { get; set; }
        /// <summary>
        /// اضافه کار سقف
        /// </summary>
        public string MaximumDurationOverTime { get; set; }
        public int? Gender { get; set; }
        public int? MaritalStatusId { get; set; }

        //// Reverse navigation

        ///// <summary>
        ///// Child WF_Requests where [Request].[EmployeeId] point to this entity (FK_WF_Request_Employee)
        ///// </summary>

        //public List<WF_Request> AvilableWF_RequestsModel { get; set; } // FK_OnCallType_RollCallDefinition

    }

}
