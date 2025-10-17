using Ksc.HR.DTO.MIS;
using KSC.Common;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.MIS;
public interface ISyncDataEmployeeMisToWebService
{
    Task<KscResult> UpdateFromMisSyncData(InsertAndUpdateFromMisViewModel model);
    Task<KscResult> InsertFromMisSyncData(InsertAndUpdateFromMisViewModel model);
}
