using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IStudentRewardSettingRepository : IRepository<StudentRewardSetting, int>
    {
        IQueryable<StudentRewardSetting> GetAllQueryableByInclude();
        StudentRewardSetting GetByRewardType_Level_Year(int RewardTypeId, int RewardLevelId, int Year);
    }
}
