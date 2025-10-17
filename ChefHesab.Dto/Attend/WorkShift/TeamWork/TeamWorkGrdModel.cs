
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWork
{
    public class TeamWorkGrdModel : FilterRequest
    {

        public int? teamWorkCategoryId { get; set; }

    }
}
