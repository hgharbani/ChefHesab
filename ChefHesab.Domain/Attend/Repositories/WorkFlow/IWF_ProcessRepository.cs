using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_ProcessRepository : IRepository<WF_Process, int>
    {
        IQueryable<WF_Process> GetAllProcess();
        IQueryable<WF_Process> GetProcessByParent(int parentId);
        IQueryable<WF_Process> GetProcessIncludeParent();
    }
}
