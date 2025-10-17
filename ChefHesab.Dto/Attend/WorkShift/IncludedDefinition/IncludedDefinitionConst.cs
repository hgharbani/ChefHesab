using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.WorkShift.IncludedDefinition
{
    public class IncludedDefinitionConst
    {
        public static List<int> SalaryDeductionIncludedId
        {
            get
            {
                return new List<int> { EnumIncludedDefinition.SalarySingleDeduction.Id, EnumIncludedDefinition.SalaryAggregateDeduction.Id };
            }
        }
        public static List<int> RewardDeductionIncludedId
        {
            get
            {
                return new List<int> { EnumIncludedDefinition.HourlyDeductionReward.Id, EnumIncludedDefinition.DailyDeductionReward.Id };
            }
        }
        //public static List<int> SingleOptionIncludedCodeForRollCalldefinition
        //{
        //    get
        //    {
        //        return new List<int> { EnumIncludedDefinition.MaximunOverTime.Id,
        //                                  EnumIncludedDefinition.AverageOverTime.Id,
        //                                  EnumIncludedDefinition.ShiftAdvantages.Id,
        //                                  EnumIncludedDefinition.InsurancePayment.Id,
        //                                  EnumIncludedDefinition.TaxPayment.Id,
        //                                  EnumIncludedDefinition. Workingdays.Id,
        //        };
        //    }
        //}
    }
}