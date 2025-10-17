using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IPaymentAdditionalHeaderRepository : IRepository<PaymentAdditionalHeader, int>
    {
        IQueryable<PaymentAdditionalHeader> GetAllQueryable();
        //void RemovePaymentAdditionalHeaderAndIncluded(int yearmonth);//(SearchPaymentAdditionalHeaderModel model);


        int GetcountDaysWork(int monthTimeSheetYearMonth);
        IQueryable<MonthTimeSheet> GetKindGartenRight(int date);
        long GetMartialSettingAmount(int EmployeementTypeId, int startdate, int endDate, int accountCode);
        void RemovePaymentAdditionalHeaderAndIncluded(int yearmonth, int accountCodeId);
    }
}
