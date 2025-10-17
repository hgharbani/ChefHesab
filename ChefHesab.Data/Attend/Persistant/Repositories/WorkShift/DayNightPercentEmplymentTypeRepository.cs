using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DayNightPercentEmplymentType;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class DayNightPercentEmplymentTypeRepository : EfRepository<DayNightPercentEmplymentType, int>, IDayNightPercentEmplymentTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public DayNightPercentEmplymentTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<DayNightPercentEmplymentTypeModel> GetDayNightPercentEmplymentType(int emplymentTypeId)
        {
            var query = from empl in _kscHrContext.DayNightPercentEmplymentTypes.Where(x => x.EmplymentTypeId == emplymentTypeId && x.IsActive)
                        join time in _kscHrContext.DayNightSettingTimes.Where(x => x.IsActive)
                        on empl.DayNightSettingTimeId equals time.Id
                        select new DayNightPercentEmplymentTypeModel()
                        {
                            StartTime = time.StartTime,
                            EndTime = time.EndTime,
                            OnCallPercent = empl.OnCallPercent,
                            IsDay = time.IsDay
                        };
            return query;

        }

    }
}
