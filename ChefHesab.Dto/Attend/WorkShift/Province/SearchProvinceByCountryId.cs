using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.Province
{
    public class SearchProvinceByCountryId : FilterRequest
    {
        public int CountryId { get; set; }


    }
}
