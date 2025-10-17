using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Emp;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface  IEmployeePromotionSuggestionRepository : IRepository<EmployeePromotion, int>
    {
        List<EmployeePromotion> GetCurrentMonthEmployeeiesPredictionDateUpgrate(List<int> employeeIds, int YearMonth);
        DateTime PredictionDateUpgrate(int employeeId);

        int CheckMeritAddMonth(double? cofficint);
        int GetTotalHistory(EmployeeDateInformation model, DateTime? EmployeementDate, double? scoreEducation, int sumMonthTimeSheet5464);
        double Fixcofficint(double? cofficint);
        double GetScoreEducationBetweenPostAndEmployee(int? postEducationCategoryId, int? employeeEducationCategoryId);
        DateTime GetTotalDayForEmployeeDatePromotion(int jobEducationEpherience, int employeeTotalHistory, double educationScore);
        DateTime FixNextUpgeadeBaseDate(DateTime nextUpgradeBaseDate, DateTime? LastUpgradeDate, DateTime? JobPositionStartDate);
        int? GetPerformanceRate(List<MeritRating> MeritRating, IEnumerable<double?> employeeMeritRatingCoefficint, int DiffCurrentGroupAndNexGroup = 0);
        double GetPerformanceRating(int DiffCurrentGroupandNexGroup, double MeritRatingCoefficint);
        DateTime FixfinalUpgradeDate(DateTime finalUpgradeDate);
        Tuple<DateTime?, DateTime?, bool, bool> SelectBaseUpgradeDate(DateTime nextUpgradeBaseDate, DateTime? LastUpgradeDate, DateTime? JobPositionStartDate);
        DateTime CheckOneYearDiffBetweenUpgradeDateAndNextUpgradeDate(DateTime? nextUpgradeBaseDate, DateTime? lastOrJobDate, bool isNexUpgradeDate, bool islastOrJobDate);
        List<EmployeePromotion> GetEmployeeJoinedToNextLevel(List<int> employeeIds, int YearMonth);
    }
}
