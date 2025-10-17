using Ksc.HR.Share.General;
using Ksc.HR.Resources.BusinessTrip;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using Ksc.HR.Resources.Chart;


namespace Ksc.HR.DTO.WorkShift

{
    public class EditPaymentStatusModel
    {


        public int Id { get; set; } // Id (Primary key)


        ///     [Required(ErrorMessageResourceName = "RequiredTitleAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Transfer.Request))]
         [Display(Name = "عنوان")]
        public string Title { get; set; } // Title (length: 50)
        public string Code { get; set; } // Code (length: 50)

        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        [Display(Name = "فعال؟")] 
        public bool IsActive { get; set; } // IsActive




    }
}
