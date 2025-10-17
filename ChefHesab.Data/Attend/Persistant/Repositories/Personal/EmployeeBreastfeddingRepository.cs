using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.Share.Model.RollCallDefinication;
using Ksc.HR.Domain.Repositories;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public partial class EmployeeBreastfeddingRepository : EfRepository<EmployeeBreastfedding, int>, IEmployeeBreastfeddingRepository
    {

        private readonly KscHrContext _kscHrContext;
        public EmployeeBreastfeddingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public void ChangeDeActiveEmployeeBreastfeddingsById(int id)
        {
            _kscHrContext.EmployeeBreastfedding.Where(x => x.EmployeeId == id).ToList().ForEach(i => i.IsActive = false);
        }
    }
}

