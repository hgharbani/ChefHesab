using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Share.Model.IncreaseSalary;
using Ksc.HR.Share.Model.Interdict;
using Ksc.HR.Share.Model.Rule;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IEmployeePromotionRepository : IRepository<EmployeePromotion, int>
    {
        //Task<List<EmployeeInterdictDetail>> Test();
        List<EmployeePromotion> GetInterdictForUpdateInLastLevel(int calPerianDate);
        IQueryable<EmployeePromotionInterdicts> GetDataForPromotionInterdict(int calculateDate, int promotionSendToSalaryId, int EnumWorkFlowStatusId);
        IQueryable<EmployeePromotionInterdicts> GetAmendmentPromotions(int calculateDate, int? employeeId, int promotionSendToSalaryId, int EnumWorkFlowStatusId);
        IQueryable<EmployeePromotion> GetAllByStatusId(int statusid);
         IQueryable<EmployeePromotionInterdicts> CalculateAmendmentPromotions(int calculateDate, int? employeeId, int promotionSendToSalaryId, int interdictId, int EnumWorkFlowStatusId);
         IQueryable<EmployeePromotionInterdicts> CalculateIssuancePromotions(int calculateDate, int? employeeId, int promotionSendToSalaryId, int interdictId, int EnumWorkFlowStatusId);

        IQueryable<EmployeePromotionInterdicts> GetIssuancePromotions(int calculateDate, int promotionSendToSalaryId, int EnumWorkFlowStatusId);
        IQueryable<EmployeePromotion> GetAllByRelatedGrid();
  
        EmployeePromotion GetPromotionConfirmRequest(int id);
      
        Task<EmployeePromotion> GetOne(int id);
        void UpdateRange(List<EmployeePromotion> list);
        void RemoveRange(List<EmployeePromotion> list);
        IQueryable<EmployeePromotion> GetAllByPromotionIds(int[] ids);
        IQueryable<EmployeePromotion> GetAllByRelatedGridForDetails(int? calculateDate, int? statusId);
        IQueryable<EmployeeInterdict> GetCaculteWorkYearsFirstLevel(int calculateDate);
        List<EmployeeInterdict> GetlatestInterdictByEmployeeId(List<EmployeeInterdict> interdicts, List<int> employeeId, List<int> employeeInterdictId);
        List<EmployeePromotionInterdicts> SetDataForPromotionInterdict(List<EmployeePromotionInterdicts> promotionInterdicts);
        bool IsExistExecuteDate(int employeeId, DateTime executeDate);
        bool IsExistIssuDate(int employeeId, DateTime IssuDate);
        Task<List<PromotionAccountEmploymentTypeInterdictDto>> GetRelatedInterdictWithPromotions(int calculateDate, int id1, int id2,bool IsWantModify);
        List<EmployeeInterdict> UpdateInterdictNumber(int calPerianDate);
        IQueryable<EmployeePromotion> GetPromotionsByCalDate(int calDate);
        IQueryable<EmployeePromotion> GetRemoveInterdictNumberto90(int calculateDate);
        List<EmployeeInterdict> GetInterdictForUpdateInLastLevelUpdateSum(int calPerianDate);

    }
}
