using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Repositories.HRSystemStatusControl;
using Ksc.HR.Domain.Repositories.OnCall;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Repositories;
namespace Ksc.HR.Data.Persistant.Repositories.HRSystemStatusControl
{
    public class SystemControlDateRepository : EfRepository<SystemControlDate, int>, ISystemControlDateRepository
    {
        private readonly KscHrContext _kscHrContext;
        public SystemControlDateRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public SystemControlDate GetActiveData()
        {
            var query = GetAll().Where(x => x.IsActive == true).AsQueryable().AsNoTracking().FirstOrDefault();
            return query;
        } 
        public int GetSalaryYear()
        {
            return Convert.ToInt32(GetActiveData().SalaryDate.ToString().Substring(0, 4));
        } 
        public int GetSalaryYear(SystemControlDate model)
        {
            return Convert.ToInt32(model.SalaryDate.ToString().Substring(0, 4));
        }
        public SystemControlDate InsertMonthToAttendAbsenceItemDate()
        {
            var query = _kscHrContext.SystemControlDates.FirstOrDefault(x => x.IsActive == true);
            var date = Utility.GetDateTimeFromYearMonth(query.AttendAbsenceItemDate.ToString());
            date = date.AddMonths(1);
            query.AttendAbsenceItemDate = Utility.GetYearMonthShamsiByMiladiDate(date);
            return query;
        }

    }
}
