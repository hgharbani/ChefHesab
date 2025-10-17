using Ksc.HR.DTO.WorkShift.FloatTimeSetting;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IFloatTimeSettingService
    {
        List<FloatTimeSettingModel> GetFloatTimeSetting();
    }
}
