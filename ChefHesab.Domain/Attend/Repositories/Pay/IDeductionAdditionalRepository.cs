using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DeductionAdditional;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IDeductionAdditionalRepository : IRepository<DeductionAdditional, int>
    {
        bool CheckCanInsertDeductionAdditional(int accountCodeId, int yearMonth);
        IQueryable<DeductionAdditional> GetByYearMonth(int yearMonth);
        void InsertDeductionAdditional(DeductionAdditionalDto dto);
        void InsertDeductionAdditionalGeneral(DeductionAdditionalDto dto);
        bool IsExistDeductionAdditional(int accountCodeId, int yearMonth);
    }
}
