using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DayNightPercentEmplymentType;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IDayNightPercentEmplymentTypeRepository : IRepository<DayNightPercentEmplymentType, int>
    {
        IQueryable<DayNightPercentEmplymentTypeModel> GetDayNightPercentEmplymentType(int emplymentTypeId);
    }
}
