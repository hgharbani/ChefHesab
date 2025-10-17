using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;
using System.ComponentModel.DataAnnotations;
//using Ksc.HR.Resources.Workshift;

namespace Ksc.HR.DTO.WorkShift.City
{
    public class CityTrainingModel
    {


        public int Id { get; set; }

        [DisplayName("عنوان")] 
        public string Title { get; set; } // Title (length: 200)


       


    }
}
