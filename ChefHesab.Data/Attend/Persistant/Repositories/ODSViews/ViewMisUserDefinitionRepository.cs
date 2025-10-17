using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.ODSViews
{
    public class ViewMisUserDefinitionRepository : EfRepository<ViewMisUserDefinition>, IViewMisUserDefinitionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisUserDefinitionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public ViewMisUserDefinition GetMisUserDefinition (string winUser)
        {
          return _kscHrContext.ViewMisUserDefinition.Where(x => x.WinUser.ToLower().Trim() == winUser.ToLower().Trim()).FirstOrDefault();
        }
        public async Task< ViewMisUserDefinition> GetMisUserDefinitionAsync(string winUser)
        {
          return await _kscHrContext.ViewMisUserDefinition.FirstOrDefaultAsync(x => x.WinUser.ToLower().Trim() == winUser.ToLower().Trim());
        }
        public async Task< ViewMisUserDefinition> GetUserDefinitionByMisUserAsync(string misUser)
        {
          return await _kscHrContext.ViewMisUserDefinition.FirstOrDefaultAsync(x => x.MisUser.ToLower().Trim() == misUser.ToLower().Trim());
        }
        public ViewMisUserDefinition GetUserDefinitionByEmployeeNumber(string employeeNumber)
        {
            decimal personalNumber = decimal.Parse(employeeNumber);
            return _kscHrContext.ViewMisUserDefinition.Where(x => x.PersonalNumber== personalNumber).FirstOrDefault();
        }
    }
}
