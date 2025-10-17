using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class StudentRewardSettingRepository : EfRepository<StudentRewardSetting, int>, IStudentRewardSettingRepository
    {
        private readonly KscHrContext _kscHrContext;

        public StudentRewardSettingRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;

        }

        public IQueryable<StudentRewardSetting> GetAllQueryableByInclude()
        {
            return _kscHrContext.StudentRewardSettings.Include(x => x.RewardLevel).Include(x => x.RewardType);
        }
        public StudentRewardSetting GetByRewardType_Level_Year(int RewardTypeId, int RewardLevelId, int Year)
        {
            return GetAllQueryableByInclude().FirstOrDefault(x => x.RewardTypeId == RewardTypeId && x.Year == Year && x.RewardLevelId == RewardLevelId && x.IsActive);
        }  
        public StudentRewardSetting GetByRewardType_Level(int RewardTypeId, int RewardLevelId, int Year)
        {
            return GetAllQueryableByInclude().FirstOrDefault(x => x.RewardTypeId == RewardTypeId && x.Year == Year && x.RewardLevelId == RewardLevelId && x.IsActive);
        }
    }
}
