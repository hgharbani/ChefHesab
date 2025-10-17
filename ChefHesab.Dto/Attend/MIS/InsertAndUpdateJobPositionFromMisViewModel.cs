using Ksc.Hr.DTO.CategoryCoefficient;
using Ksc.HR.DTO.Chart.ChartStructure;
using Ksc.HR.DTO.Chart.JobPositionNature;
using Ksc.HR.DTO.Chart.RewardSpecific;
using Ksc.HR.DTO.Chart;
using Ksc.HR.DTO.ExternalDTO.IndustrialAccounting;
using Ksc.Hr.DTO.ProductionEfficiency;
using Ksc.HR.Resources.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace Ksc.HR.DTO.MIS
{
    public class ScoreViewModel
    {
        public int TypeId { get; set; }
        public string Score { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class InsertAndUpdateScoreViewModel
    {
        public InsertAndUpdateScoreViewModel()
        {
            ScoreViewModel = new List<ScoreViewModel>();
        }
        public List<ScoreViewModel> ScoreViewModel { get; set; }
        public string MisJobPositionCode { get; set; }

        public string TypeId { get; set; }
        public string Score { get; set; }

        public string StartDate { get; set; } 
        public string EndDate { get; set; } 
    }

    public class InsertAndUpdateJobPositionViewModel
    {

        public string Code { get; set; } // 
        public string Title { get; set; } // -
        public string StructureCode { get; set; } // -
        public string CostCenter { get; set; }//-
        public string LevelNumber { get; set; }
        public string JobIdentityCode { get; set; } // -
        public string JobPoisitionStatus { get; set; } // -
        public string StartDate { get; set; } // -
        public string EndDate { get; set; } // -
        public string StructureEndDate { get; set; } //-
        public string CapacityCount { get; set; } //  -
        public string WorkingDayCount { get; set; } //  -
        public string WorkingShiftCount { get; set; } // -
        public string WorkingDayOutsourcingCount { get; set; } // -
        public string WorkingShiftOutsourcingCount { get; set; } // -
        public string PermissionExistCommodityCount { get; set; } // - 
        public string TemporaryCount { get; set; } // -
        public string SubstituteCount { get; set; } // - 
        public string ExtraCount { get; set; } // - 
        public string IsOnCall { get; set; } //-
        public string IsStrategic { get; set; } // - 
        public string InsuranceJobCode { get; set; } // -
        public string RewardSpecificEfficincy { get; set; } // - 
        public string CofficientProximityProduct { get; set; } // - 
        public string ParentCode { get; set; } // - 
        public string SpecificRewardCode { get; set; } // - 
        public string JobPositionNatureCode { get; set; }//-
        public string Description { get; set; } // -
        public string MisJobPositionCode { get; set; }

        public string FlagIncrease { get; set; }
        public string CoefficientYearsDay { get; set; } // ParentId  
        public string CoefficientYearsShift { get; set; } // ParentId  
        public string IncreaseStartDate { get; set; } // IncreaseStartDate
        public string IncreaseEndDate { get; set; } // IncreaseEndDate 



        // public InsertAndUpdateIncreasePercentViewModel InsertAndUpdateIncreasePercentViewModel { get; set; }
    }

    public class InsertAndUpdateJobIdentityViewModel
    {
        // public int Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Title { get; set; } // Title
        public string JobCategoryCode { get; set; } // JobCategoryId
        public string StartDate { get; set; } // StartDate
        public string EndDate { get; set; } // EndDate
    }

    public class InsertAndUpdateStructureViewModel
    {
        // public int Id { get; set; } // Id (Primary key)
        // public int StructureTypeId { get; set; } // StructureTypeId
        public string MisJobPositionCode { get; set; }
        public string Title { get; set; } // Title
        public string ParentCode { get; set; } // ParentId  
        public string StartDate { get; set; } // StartDate
        public string EndDate { get; set; } // EndDate 
    }
    public class InsertAndUpdateIncreasePercentViewModel
    {
        // public int Id { get; set; } // Id (Primary key)
        // public int StructureTypeId { get; set; } // StructureTypeId
        public string MisJobPositionCode { get; set; }
        public string CoefficientYearsDay { get; set; } // ParentId  
        public string CoefficientYearsShift { get; set; } // ParentId  
        public string StartDate { get; set; } // StartDate
        public string EndDate { get; set; } // EndDate 
    }


    public class InsertAndUpdateJobPositionFromMisViewModel
    {
        public string Operation { get; set; }
        public string UserName { get; set; }
        public InsertAndUpdateJobPositionViewModel InsertAndUpdateJobPositionViewModel { get; set; }
        public InsertAndUpdateJobIdentityViewModel InsertAndUpdateJobIdentityViewModel { get; set; }
        public InsertAndUpdateStructureViewModel InsertAndUpdateStructureViewModel { get; set; }
        public InsertAndUpdateScoreViewModel InsertAndUpdateScoreViewModel { get; set; }
        public EnumInsertAndUpdateJobPositionType TypeTable { get; set; }
        public InsertAndUpdateIncreasePercentViewModel InsertAndUpdateIncreasePercentViewModel { get; set; }
    }
}
