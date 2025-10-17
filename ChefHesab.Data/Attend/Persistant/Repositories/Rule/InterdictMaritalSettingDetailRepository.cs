using DNTPersianUtils.Core;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Share.Model.Rule;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class InterdictMaritalSettingDetailRepository : EfRepository<InterdictMaritalSettingDetail, int>, IInterdictMaritalSettingDetailRepository
    {
        private readonly KscHrContext _kscHrContext;

        public InterdictMaritalSettingDetailRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;
        }
        public IQueryable<InterdictMaritalSettingDetail> GetAllByInterdictMaritalSettingId(int interdictMaritalSettingId)
        {
            var result = _kscHrContext.InterdictMaritalSettingDetails.Include(x => x.MaritalStatus).Include(x => x.AccountCode).Where(x => x.InterdictMaritalSettingId == interdictMaritalSettingId && x.IsActive == true).AsQueryable();
            return result;
        }
        public IQueryable<InterdictMaritalSettingDetail> GetInterdictMaritalSettingDetailsByFilterInterdict(SearchInterdictDetailDto model)
        {
            var accountcodes = model.AccountCodeIds;
            var employeemodel = _kscHrContext.Employees.Find(model.EmployeeId);

            var result = _kscHrContext.InterdictMaritalSettingDetails.Include(x => x.AccountCode).Include(x => x.InterdictMaritalSetting)
            .Where(x =>
            
            x.InterdictMaritalSetting.EmploymentTypeId == model.EmploymentTypeId &&
            x.InterdictMaritalSetting.StartDate <= model.YearMonth && x.InterdictMaritalSetting.EndDate >= model.YearMonth
            &&  x.IsActive == true
            && x.InterdictMaritalSetting.IsConfirmed &&
            (x.MaritalStatusId == employeemodel.MaritalStatusId || x.MaritalStatusId.HasValue == false)
            && accountcodes.Any(a => a == x.AccountCodeId)
            ).OrderBy(x=>x.AccountCodeId); 
            return result;
        }

       
    }
}
