using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_RoleRepository : IRepository<WF_Role, int>
    {
        public IEnumerable<WF_Role> GetParentRoles();
    }
}
