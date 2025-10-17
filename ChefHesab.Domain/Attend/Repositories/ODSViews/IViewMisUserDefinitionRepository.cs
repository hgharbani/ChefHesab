using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.ODSViews
{
    public interface IViewMisUserDefinitionRepository : IRepository<ViewMisUserDefinition>
    {
        ViewMisUserDefinition GetMisUserDefinition(string winUser);
        Task<ViewMisUserDefinition> GetMisUserDefinitionAsync(string winUser);
        ViewMisUserDefinition GetUserDefinitionByEmployeeNumber(string employeeNumber);
        Task<ViewMisUserDefinition> GetUserDefinitionByMisUserAsync(string misUser);
    }
}
