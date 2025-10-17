using Ksc.HR.DTO.Other;
using Ksc.HR.Share.model.KendoModel;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.OnCall.Employee
{
    public class SearchEmployeeModel : FilterRequest
    {
       
        public bool IsSalaryUser { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }

        public bool AddCurrentUser { get; set; }
        public string CurrentUserName { get; set; }
        public int Id { get; set; } // Id (Primary key)
        public int emplId { get; set; } 
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string Name { get; set; } // Name (length: 500)
        public string Family { get; set; } // Family (length: 500)
        public string DisplayMember { get { return $"{EmployeeNumber} {Name/*.Trim()*/} {Family/*.Trim()*/} "; } }
        public string TeamCode { get; set; } // TeamCode
        public List<string> RollName { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string NationalCode { get; set; }
        public string FatherName { get; set; } // FatherName (length: 500)
        public List<string> EmployeeNumbers { get; set; }

        public string FullName { get { return $"{Name/*.Trim()*/} {Family/*.Trim()*/} "; } }


        //// Reverse navigation

        ///// <summary>
        ///// Child WF_Requests where [Request].[EmployeeId] point to this entity (FK_WF_Request_Employee)
        ///// </summary>

        //public List<WF_Request> AvilableWF_RequestsModel { get; set; } // FK_OnCallType_RollCallDefinition


        public byte[] ImageByte { get; set; }
        public string Image
        {
            get
            {
                if (ImageByte == null)
                    return null;
                string base64String = Convert.ToBase64String(ImageByte, 0, ImageByte.Length);
                var ImageUrl = "data:image/png;base64," + base64String;
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

        public int? PaymentStatusId { get; set; }
        public string EmploymentTypeTitle { get; set; }
    }

    public class EmployeSelectList : SelectListItem
    {
        public int Id { get; set; }
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string Name { get; set; } // Name (length: 500)
        public string Family { get; set; } // Family (length: 500)
        public string DisplayMember { get { return $"{EmployeeNumber} {Name/*.Trim()*/} {Family/*.Trim()*/} "; } }
        public string TeamCode { get; set; } // TeamCode
    }

    public class EmployeFilter : SelectListItem
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string Name { get; set; } // Name (length: 500)
        public string Family { get; set; } // Family (length: 500)
        public string NationalCode { get; set; }
        public string FatherName { get; set; } // FatherName (length: 500)
        public string CurrentUserName { get; set; } // FatherName (length: 500)
        public List<int> PeymentStatusIds { get; set; } = new List<int>();
        public List<int> PersonalTypeIds { get; set; } = new List<int>();

        public int? JabPostionId { get; set; }
    }

    public class SearchEmployeeInfo
    {
        public SearchEmployeeInfo()
        {
            EmployeeNumbers = new List<string>();
        }
        public List<string> EmployeeNumbers { get; set; }
        public string EmployeeNumber { get; set; }
        public int EmployeeId { get; set; }
    }

    public class SearchEmployeeForInterDict:KendoRequest
    {
        public SearchEmployeeForInterDict()
        {
            EmployeeNumbers = new List<string>();
        }
        public List<string> EmployeeNumbers { get; set; }
        public string EmployeeNumber { get; set; }

        public bool IsSalaryUser { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }
        public string CurrentUserName { get; set; }
        public int Id { get; set; } // Id (Primary key)
        public int emplId { get; set; }
        public string Name { get; set; } // Name (length: 500)
        public string Family { get; set; } // Family (length: 500)
        public string TeamCode { get; set; } // TeamCode
        public List<string> RollName { get; set; }
    }
}
