using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories.EmployeeBase;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class TableDefinitionRepository : EfRepository<TableDefinition, int>, ITableDefinitionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public TableDefinitionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}
