using Ksc.HR.DTO.MIS;
using Ksc.HR.DTO.Pay.EmployeeOtherPayment.PrivatePortal;
using Ksc.HR.Share.Model;
using System.Collections.Generic;

namespace Ksc.HR.Appication.Interfaces.MIS;
public interface IDataPersonalForOtherSystemService
{
    OutOfSalaryPaymentResultModel GetDataOutOfSalaryPaymentFromMis(OutOfSalaryPaymentFilterRequest filter);

    //List<DETAIL_PER> GetPersonalDataForOtherSystem(InputDataPersonalForOtherSystemModel model);
    RPC_PER GetPersonalDataForOtherSystem(InputDataPersonalForOtherSystemModel model);
    CALLING_RPC GetPersonalDataMis(InputMisApiModel model);
}
