using KSC.Common.Filters.Models;
using Microsoft.AspNetCore.Routing.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.City
{
    public class CityFilterRequest: FilterRequest
    {
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
    }
}
