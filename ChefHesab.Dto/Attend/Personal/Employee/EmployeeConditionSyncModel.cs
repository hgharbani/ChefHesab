using Ksc.HR.Resources.Emp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeConditionSyncModel
    {

        public string P_function { get; set; }
        /// <summary>
        /// شماره پرسنلی int8
        /// NUM_PRSN
        /// </summary>
        public int EmployeeNumber { get; set; }


        /// <summary>
        /// کد وضعیت اشتغال
        /// COD_STA_PYM
        /// </summary>
        public int PeymentStatusId { get; set; }


        /// <summary>
        /// نوع استخدام int2
        /// FK_EMPLT
        /// </summary>
        public int EmploymentTypeId { get; set; } 


        /// <summary>
        /// وضعیت استخدام int2
        /// COD_CLASS
        /// </summary>
        public int EmploymentStatusId { get; set; } 


        /// <summary>
        /// تاریخ استخدام 14020101
        /// DAT_EMPLT
        /// </summary>
        public int EmploymentDate { get; set; } 


        /// <summary>
        /// تاریخ شروع قرارداد 14020101
        /// DAT_STR_CNTRC
        /// </summary>
        public int ContractStartDate { get; set; } 
       
        /// <summary>
        /// تاریخ پایان قرارداد 14030101
        /// DAT_END_CNTRC
        /// </summary>
        public int ContractEndDate { get; set; } 

        /// <summary>
        /// زمان کاری int2
        /// COD_TYP_WRK
        /// </summary>
        public int WorkTimeId { get; set; }

        /// <summary>
        /// گروه کاری R - A - ...
        /// COD_WRK_GRP
        /// </summary>
        public string WorkGroupCode { get; set; } // WorkGroupId

        /// <summary>
        /// نوع ثبت ورود و خروج int2
        /// COD_EXMP_ENEX
        /// </summary>
        public int EntryExitTypeId { get; set; }


        /// <summary>
        /// کارکرد شناور int1
        /// FLG_FLOAT_ATABI
        /// </summary>
        public int HasFloatTime { get; set; }


        /// <summary>
        /// گروه کاری int4
        /// NUM_TEAM
        /// </summary>
        public int TeamWorkCode { get; set; }



        /// <summary>
        /// تاریخ شروع  در تیم int8
        /// DAT_STR_TEAM
        /// </summary>
        public int TeamStartDate { get; set; } // TeamStartDate


        /// <summary>
        /// مدرک تحصیلی  int2
        /// COD_LEV_EDUC
        /// </summary>
        public int EducationId { get; set; }

        /// <summary>
        /// رشته تحصیلی
        /// COD_EXPRT_LEV
        /// </summary>
       // public int StudyFieldId { get; set; }

        /// <summary>
        /// رشته تحصیلی کد
        /// COD_EXPRT_LEV
        /// </summary>
        public int StudyFieldCode{ get; set; }
        /// <summary>
        /// شهر محل خدمت String10
        /// FK_CITY_WORK
        /// </summary>
        public string WorkCityCodeMIS { get; set; }


        /// <summary>
        /// کد پست سازمانی
        /// FK_JPOS
        /// </summary>
        public string JobPositionCode { get; set; } //JobPositionCode

        /// <summary>
        /// تاریخ احراز پست سازمانی int8
        /// DAT_JOB_POS
        /// </summary>
        public int JobPositionStartDate { get; set; } // EmploymentDate

        /// <summary>
        /// کد نوع پس انداز  int2
        /// FK_SAVTB
        /// </summary>
        public int SavingTypeId { get; set; } // SavingTypeId

        /// <summary>
        /// مبنا پس انداز سال ماه  INT6
        /// DAT_STR_SAV
        /// </summary>
        public int SavingTypeDate { get; set; }


        /// <summary>
        /// کد وضعیت ایثارگری INT2
        /// COD_ISAR
        /// </summary>
        public int IsarStatusId { get; set; }


        /// <summary>
        /// کد ایثارگری INT1
        /// COD_OVT_VAC
        /// </summary>
        public int SacrificeOptionSettingId { get; set; }



        /// <summary>
        /// درصد جانبازی INT 3.2
        /// PCN_ISAR
        /// </summary>
        public int SacrificePercentage { get; set; }


        /// <summary>
        /// تاریخ اخذ مدرک int8
        /// DAT_STR_EDUC
        /// </summary>
        public int EducationDate { get; set; }
    }
}
