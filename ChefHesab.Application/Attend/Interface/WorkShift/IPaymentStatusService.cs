

using Ksc.HR.Domain.Entities.BusinessTrip;

using Ksc.HR.Domain.Entities.Workshift;

using Ksc.HR.DTO.WorkFlow.BaseFile;
using Ksc.HR.DTO.WorkShift;
using KSC.Common;
using KSC.Common.Filters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift

{
    public interface IPaymentStatusService
    {
       FilterResult<SearchPaymentStatusModel> GetPaymentStatusByFilter(FilterRequest Filter);
        Task<KscResult> AddPaymentStatus(AddPaymentStatusModel model);
        /// <summary>
        /// آیا عنوان تکراری می باشد؟
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistsByTitle(int id, string name);
        /// <summary>
        /// آیا عنوان تکراری می باشد؟
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
       Task<bool> ExistsByTitle(string name);
        Task<PaymentStatus> GetOne(int id);
        List<SearchPaymentStatusModel> GetPaymentStatusByKendoFilter(FilterRequest Filter);

       Task<EditPaymentStatusModel> GetForEdit(int id);
        /// <summary>
        /// ویرایش  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<KscResult> UpdatePaymentStatus(EditPaymentStatusModel model);
        Task<List<AutomCompleteModel>> GetPaymentStatusAutoComplete();

        Task<List<AutomCompleteModel>> GetPaymentStatusAutoCompleteByAccess(List<string> roles);
    }
}
