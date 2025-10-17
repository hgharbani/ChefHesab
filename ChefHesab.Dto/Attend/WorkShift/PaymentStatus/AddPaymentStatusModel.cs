using Ksc.HR.Resources.Chart;
using Ksc.HR.Resources.Transfer;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.WorkShift

{
    public class AddPaymentStatusModel
    {

        
        public int Id { get; set; } // Id (Primary key)

        [Display(Name = "عنوان")]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public string Title { get; set; } // Title (length: 50)
        public string Code { get; set; } // Code (length: 50)

        public DateTime? InsertDate { get; set; } // InsertDate
        
        public string InsertUser { get; set; } // InsertUser (length: 50)

        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public bool IsActive { get; set; } // IsActive



    }
}
