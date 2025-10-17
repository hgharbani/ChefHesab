using Ksc.HR.DTO.MIS;
using KSC.Common;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.MIS
{
    public interface ISyncDataJobPositionMisToWebService
    {

        Task<KscResult> InsertScoreFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);


        Task<KscResult> UpdateJobPositionFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> DeleteJobPositionFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> InsertJobPositionFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);


        Task<KscResult> InsertJobIdentityFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> UpdateJobIdentityFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> DeleteJobIdentityFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);

        Task<KscResult> InsertStructureFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> UpdateStructureFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> DeleteStructureFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model);
        Task<KscResult> InsertAndUpdateIncreasePercent(InsertAndUpdateJobPositionFromMisViewModel model);
    }
}