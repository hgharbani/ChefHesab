using KSC.Domain;
namespace Ksc.Hr.Domain.Entities
{
    public class V_JobPosition : IEntityBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int hasChildren { get; set; }
        public string StructureTitle { get; set; }
        public decimal? CostCenter { get; set; }
        public int? CapacityCount { get; set; }
        public double? CofficientProximityProduct { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ExtraCount { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public string InsuranceJobCode { get; set; }
        public bool? IsOnCall { get; set; }
        public bool? IsStrategic { get; set; }
        public int? JobIdentityId { get; set; }
        public int? JobPoisitionStatusId { get; set; }
        public int? ParentId { get; set; }
        public int? PermissionExistCommodityCount { get; set; }
        public double? RewardSpecificEfficincy { get; set; }
        public int? SpecificRewardId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StructureEndDate { get; set; }
        public int StructureId { get; set; }
        public string ParentCode { get; set; }
        public int? SubstituteCount { get; set; }
        public int? TemporaryCount { get; set; }
        public int? EmployeeCount { get; set; }
        
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public int? WorkingDayCount { get; set; }
        public int? WorkingDayOutsourcingCount { get; set; }
        public int? WorkingShiftCount { get; set; }
        public int? WorkingShiftOutsourcingCount { get; set; }
        public bool IsActive { get; set; }
        public int? JoinedRelocationCount { get; set; }
        public int? TransferRelocationCount { get; set; }
        public string ParentCodeChain { get; set; }
        public int? LevelNumber { get; set; }
        public string Asistanse { get; set; }
    }
}

