using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IChart_JobPositionStatusCategoryRepository : IRepository<Chart_JobPositionStatusCategory, int>
    {
        IQueryable<Chart_JobPositionStatusCategory> GetChart_JobPositionStatusCategoryById(int id);
        IQueryable<Chart_JobPositionStatusCategory> GetChart_JobPositionStatusCategorys();
    }
}
