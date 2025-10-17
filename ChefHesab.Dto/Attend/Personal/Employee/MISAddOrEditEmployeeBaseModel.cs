using Ksc.HR.Resources.Personal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.EmployeeBase;
using System.ComponentModel;
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.DTO.Personal.MaritalStatus;
using Microsoft.AspNetCore.Http;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class MISAddOrEditEmployeeBaseModel
    {
        public string Operation { get; set; }

        public string Domain { get; set; }
        /// <summary>
        /// MIS نام فانکشن برای استفاده از روتین در
        /// </summary>
        public string FUNCTION { get; set; }
        public string NUM_PRSN_EMPL { get; set; }//EmployeeNumber شماره پرسنلی
        public int COD_STA_PYM_EMPL { get; set; }// PaymentStatus وضعیت اشتغال
        public string DAT_STA_PYM_EMPL { get; set; }// PaymentStatus :YearMonth تاریخ وضعیت اشتغال
        public string NAM_PER_EMPL { get; set; }//Name    نام
        public string NAM_FAM_EMPL { get; set; }// Family نام خانوادگی
        public string NAM_PER_LST_EMPL { get; set; }//PreiveName نام قبلی فرد
        public string NAM_FAM_LST_EMPL { get; set; }//PreiveFamily    نام خانوادگی قبلی فرد
        public int COD_SEX_EMPL { get; set; }//Gender  جنسیت
        public string NAM_FTHR_EMPL { get; set; }//   FatherName نام پدر
        public string NUM_NNAL_EMPL { get; set; }//   NationalCode کد ملی
        public string NUM_CRT_EMPL { get; set; }//CertificateNumber شماره شناسنامه
        public int COD_NNLTY_EMPL { get; set; }//   NationalityId ملیت
        public int COD_RLGN_EMPL { get; set; } // RegionId    مذهب
        public string DAT_BRT_EMPL { get; set; } //       BirthDate تاریخ تولد شمسی
        public string COD_CITY_BORN { get; set; } //  Birth:City TAB_CITY_SP_MIS شهر محل تولد
        public string COD_RGN_CITY_BORN { get; set; } // Birth: Province

        public string COD_CNTRY_CITY_BORN { get; set; } //  Birth: Country 
        public string DAT_ISSU_CRT_EMPL { get; set; } //   CertificateDate تاریخ صدور شناسنامه(شمسی)

        public string COD_CITY_DOC { get; set; } // Certificate: City TAB_CITY_SP_MIS شهر صدور شناسنامه

        public string COD_RGN_CITY_DOC { get; set; } //Certificate: Province 

        public string COD_CNTRY_CITY_DOC { get; set; } //   Certificate: Country

        public int COD_MLTRY_EMPL { get; set; } //   MilitaryStatusId وضعیت نظام وظیفه

        public string DAT_STR_MLTRY_EMPL { get; set; } //  MilitaryStartDate   تاریخ شروع سربازی(شمسی)

        public string DAT_END_MLTRY_EMPL { get; set; } //  MilitaryEndDate تاریخ پایان سربازی(شمسی)

        public int COD_MRD_EMPL { get; set; } //  MaritalStatusId وضعیت تاهل

        public string DAT_MRD_EMPL { get; set; } // MarriedDate تاریخ ازدواج(شمسی)

        public string NUM_PNS_EMPL { get; set; } //  InsuranceNumber شماره بیمه

        public int FK_TBPNS { get; set; } // FK_TBPNS InsuranceType   نوع بیمه

        public int COD_INSU_COMP_EMPL { get; set; } // InsuranceListId شرکت بیمه گذار

        public string NUM_MOBIL_EMPL { get; set; } // PhoneNumber تلفن همراه

        public int COD_BLD_TYP_EMPL { get; set; } // BloodTypeId گروه خون
        public string COD_ZIP_EMPL { get; set; } //  HomeZipCode کد پستی
        public string COD_CITY_RSDNC { get; set; } //  HomeCity-	TAB_CITY_SP_MIS شهر محل سکونت
        public string COD_RGN_CITY_RSDNC { get; set; } // HomeCity: Province
        public string COD_CNTRY_CITY_RSDNC { get; set; } //  Home:Country
        public string DES_ADR_EMPL { get; set; } //   HomeAddress آدرس محل سکونت
        public string USER_NAME { get; set; } //




    }

}
