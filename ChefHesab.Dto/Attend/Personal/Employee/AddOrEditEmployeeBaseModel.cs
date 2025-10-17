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
using DNTPersianUtils;
using Ksc.HR.Share.General;

namespace Ksc.HR.DTO.Personal.Employee
{

    public class AddOrEditEmployeeBaseModel
    {

        public AddOrEditEmployeeBaseModel()
        {
            AvailableInsuranceType = new List<SearchInsuranceTypeModel>();//نوع بیمه
            AvailableInsuranceList = new List<SearchInsuranceListModel>();//شرکت بیمه
            AvailableRegion = new List<SearchRegionDto>();//مذهب
            AvailableBloodType = new List<SearchBloodTypeDto>();//گروه خونی
            AvailableNationality = new List<SearchNationalityDto>();//ملیت
            AvailableMilitaryStatus = new List<SearchMilitaryStatusModel>();//وضعیت نظام وظیفه
            AvailableCity = new List<SearchCityModel>();//شهر
            AvailableProvince = new List<SearchProvinceModel>();//استان
            AvailableGender = new List<SearchGenderDto>();//جنسیت
            AvailableMaritalStatus = new List<SearchMaritalStatusDto>();//تاهل


        }
        public int? PersonalTypeId { get; set; }
        public bool IsColesed { get; set; }
        public List<SearchInsuranceTypeModel> AvailableInsuranceType { get; set; }
        public List<SearchInsuranceListModel> AvailableInsuranceList { get; set; }
        public List<SearchRegionDto> AvailableRegion { get; set; }//استان
        public List<SearchBloodTypeDto> AvailableBloodType { get; set; }//گروه خونی
        public List<SearchNationalityDto> AvailableNationality { get; set; }
        public List<SearchMilitaryStatusModel> AvailableMilitaryStatus { get; set; }
        public List<SearchCityModel> AvailableCity { get; set; }
        public List<SearchProvinceModel> AvailableProvince { get; set; }
        public List<SearchGenderDto> AvailableGender { get; set; }
        public List<SearchMaritalStatusDto> AvailableMaritalStatus { get; set; }


       
        public string Operation { get; set; }
        /// <summary>
        /// تعداد فایل  
        /// </summary>
        public int CountAttachmentFile { get; set; }
        public bool IsGenerated { get; set; }

        public int Id { get; set; }

        [Display(Name = nameof(EmployeeNumber), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)//شماره پرسنلی

        [Display(Name = nameof(Gender), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int Gender { get; set; }

        [Display(Name = nameof(PhoneNumber), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [RegularExpression(@"^([0]|\+91[\-\s]?)?[789]\d{9}$", ErrorMessage = "شماره موبایل را درست وارد نمایید")]
        public string PhoneNumber { get; set; }//شماره تلفن

        [Display(Name = "  تعداد فرزند ")]
        public int NumberOfChildren { get; set; } // ChildCount (length: 50) //تعداد فرزند

        [Display(Name = "  کد پستی   ")]
        public string HomeZipCode { get; set; } // PostCode (length: 50) //کد پستی


        [Display(Name = "  وضعیت نظام وظیفه ")]
        //[Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? MilitaryStatusId { get; set; }//وضعیت نظام وظیفه


        public string Address { get; set; } // PostCode (length: 50) //ادرس
        [Display(Name = nameof(BirthDate), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]

        public DateTime? BirthDate { get; set; }//تاریخ تولد
        public string BirthDateString { get { return BirthDate.HasValue ? BirthDate.Value.ToPersianDate() : ""; } }


        [Display(Name = nameof(FatherName), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string FatherName { get; set; } // نام پدر
        [Display(Name = " مدرک تحصیلی   ")]
        public string Education { get; set; }  

        [Display(Name = " آدرس   ")]
        public string HomeAddress { get; set; }  // آدرس محل سکونت

        [Display(Name = nameof(BirthCityId), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]

        public int? BirthCityId { get; set; } // شهر محل تولد

        [Display(Name = nameof(CertificateCityId), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? CertificateCityId { get; set; } //شهر محل صدور شناسنامه 

        [Display(Name = "  شهر محل سکونت   ")]
        public int? HomeCityId { get; set; }  // شهر محل سکونت
                                              //for edit province

        // public int? BirthCityProvinceId { get; set; } // استان محل تولد
        // public int? CertificateCityProvinceId { get; set; } // استان محل صدور شناسنامه
        //  public int? HomeCityProvinceId { get; set; } // استان محل سکونت

        public string HomeCityTitle { get; set; }
        public string CertificateCityTitle { get; set; }
        public string BirthCityTitle { get; set; }


        [Display(Name = nameof(NationalCode), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string NationalCode { get; set; } // کد ملی


        [Display(Name = nameof(MaritalStatusId), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? MaritalStatusId { get; set; } //وضعیت تاهل 

        [Display(Name = " سایر افراد تحت تکفل ")]
        public int? NumberOfDependents { get; set; } //         /// تعداد سایر افراد تحت تکفل

        [Display(Name = nameof(InsuranceTypeId), ResourceType = typeof(EmployeeResource))]
        public int? InsuranceTypeId { get; set; }//نوع بیمه
        public string InsuranceTypeText { get; set; }//نوع بیمه

        [Display(Name = nameof(InsuranceListId), ResourceType = typeof(EmployeeResource))]
        public int? InsuranceListId { get; set; }//شرکت بیمه گزار
        public string InsuranceListText { get; set; }//شرکت بیمه گزار

        [Display(Name = nameof(CertificateNumber), ResourceType = typeof(EmployeeResource))]

        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string CertificateNumber { get; set; }// شماره شناسنامه

        [Display(Name = "ملیت")]
        public int? NationalityId { get; set; }    //ملیت  


        [Display(Name = nameof(BloodTypeId), ResourceType = typeof(EmployeeResource))]
        public int? BloodTypeId { get; set; }// گروه خون

        [Display(Name = nameof(RegionId), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int? RegionId { get; set; } // مذهب

        public string GuidPicId { get; set; }

        [Display(Name = nameof(MarriedDate), ResourceType = typeof(EmployeeResource))]
        public DateTime? MarriedDate { get; set; }         /// تاریخ ازدواج
        public string MarriedDateString { get { return MarriedDate.HasValue ? MarriedDate.Value.ToPersianDate() : ""; } }

        [Display(Name = nameof(MilitaryStartDate), ResourceType = typeof(EmployeeResource))]
        public DateTime? MilitaryStartDate { get; set; } //         /// تاریخ شروع سربازی
        public string MilitaryStartDateString { get { return MilitaryStartDate.HasValue ? MilitaryStartDate.Value.ToPersianDate() : ""; } }


        [Display(Name = nameof(MilitaryEndDate), ResourceType = typeof(EmployeeResource))]
        public DateTime? MilitaryEndDate { get; set; } // /// تاریخ پایان سربازی
        public string MilitaryEndDateString { get { return MilitaryEndDate.HasValue ? MilitaryEndDate.Value.ToPersianDate() : ""; } }


        [Display(Name = nameof(Name), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string Name { get; set; } // Name (length: 500) //نام

        // [Display(Name = nameof(FullName), ResourceType = typeof(EmployeeResource))]
        //public string FullName { get { return Name + " " + Family; } }//نام خانوادگی

        [Display(Name = nameof(Family), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string Family { get; set; } // Family (length: 500)
        public int PaymentStatusId { get; set; } // وضعیت پرداخت

        /// <summary>
        /// وضعیت اشتغال به کار
        /// </summary>
       [Display(Name = nameof(PaymentStatusId), ResourceType = typeof(EmployeeResource))]
        public string PaymentStatusTitle { get; set; }

    
        public int? EmploymentTypeId { get; set; }
        public string EmploymentTypeTitle { get; set; }

        [Display(Name = nameof(CertificateDate), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public DateTime? CertificateDate { get; set; } //         /// تاریخ صدور شناسنامه
        public string CertificateDateString { get { return CertificateDate.HasValue ? CertificateDate.Value.ToPersianDate():""; } }
        
        [Display(Name = nameof(PreiveName), ResourceType = typeof(EmployeeResource))]
        public string PreiveName { get; set; } //نام قبلی
        
        [Display(Name = nameof(PreiveFamily), ResourceType = typeof(EmployeeResource))]
        public string PreiveFamily { get; set; } //نام خانوادگی قبلی




        public int? EmploymentStatusId { get; set; } // وضعیت استخدام

        [Display(Name = "شماره بیمه")]
        public string InsuranceNumber { get; set; } // InsuranceNumber (length: 50)/// شماره بیمه و بازنشستگی
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string RemoteIpAddress { get; set; }
        public string DomainName { get; set; }
        public byte[] ImageByte { get; set; }

        public IFormFile FileToUpload { get; set; }
        public string FileName { get; set; }
        public string FileGuid { get; set; }
        public string imageStream { get; set; }
        public string imageFormat { get; set; }
        public string Image
        {
            get
            {
                if (ImageByte == null)
                    return null;
                string base64String = Convert.ToBase64String(ImageByte, 0, ImageByte.Length);
                var ImageUrl = "data:image/"+ imageFormat + ";base64," + base64String;
                return ImageUrl;
            }
            set
            {
            }
        }
        public string ImageBase64
        {
            get
            {
                if (ImageByte == null)
                    return null;
                string base64String = Convert.ToBase64String(ImageByte, 0, ImageByte.Length);
                return base64String;
            }
            set
            {
            }
        }




    }
}
