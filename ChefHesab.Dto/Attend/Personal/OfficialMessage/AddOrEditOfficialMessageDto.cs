using Ksc.HR.Resources.Personal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.Personal.OfficialMessage
{
    public class AddOrEditOfficialMessageDto
    {
        public AddOrEditOfficialMessageDto()
        {
        }
        public int Id { get; set; }
        [Display(Name = nameof(StartDate), ResourceType = typeof(OfficialMessageResource))]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime StartDate { get; set; }

        [Display(Name = nameof(EndDate), ResourceType = typeof(OfficialMessageResource))]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime EndDate { get; set; }

        [Display(Name = nameof(Messages), ResourceType = typeof(OfficialMessageResource))]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Messages { get; set; }

        [Display(Name = nameof(InsertUser), ResourceType = typeof(OfficialMessageResource))]
      
        public string InsertUser { get; set; }

    }
}

