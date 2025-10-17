using Ksc.HR.DTO.MIS;
using Ksc.HR.DTO.Pay.EmployeeOtherPayment.PrivatePortal;
using Ksc.HR.Share.Model;
using KSC.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.MIS;
public interface ISyncDataRewardMisFromTextFileService
{
    Task<KscResult> SyncRewardBaseHeaderAndDetailFromMis();
    Task<KscResult> SyncRewardInFromMis();
}
