using Ksc.HR.Domain.Entities.Log;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Log
{
    public interface ILogDataRepository : IRepository<LogData, long>
    {
        //void InsertLog(string authenticateUser = "", string authorizeUser = "");
        IQueryable<LogData> GetAllRelated();

    }
}
