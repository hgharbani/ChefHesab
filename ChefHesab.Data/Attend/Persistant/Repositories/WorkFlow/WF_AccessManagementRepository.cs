using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Ksc.HR.Data.Persistant.Repositories.WorkFlow
{
    public class WF_AccessManagementRepository : EfRepository<WF_AccessManagement, int>, IWF_AccessManagementRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_AccessManagementRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<WF_AccessManagement> GetAllByPersonId(string personId, string jopPositionCode)
        {
            string currentUserJobPositionCode = jopPositionCode;
            var result = _kscHrContext.WF_AccessManagements
                .Where(x =>
                x.IsDeleted != true &&
                x.IsActive &&
                (x.PersonId == personId || (!string.IsNullOrEmpty(currentUserJobPositionCode) && !string.IsNullOrEmpty(x.JobPositionCode) && x.JobPositionCode == currentUserJobPositionCode)) &&
                (x.EndDate == null || (x.EndDate != null && x.EndDate >= DateTime.Now)) &&
                (x.StartDate == null || (x.StartDate != null && x.StartDate <= DateTime.Now)));
            return result;
        }
        public IEnumerable<WF_AccessManagement> GetAccessListforResponser(string personId, string jopPositionCode)
        {
            // لیست دسترسیهای اجرا و پاسخ پرسنل
            var responserAccessList = GetResponserAccessManagementList(personId, jopPositionCode);
            if (responserAccessList == null || !responserAccessList.Any())
            {
                return null;
            }
            var ResponserRoles = responserAccessList.Select(x => x.RoleId).ToList();
            // لیست دسترسیهایی که نقش آنها زیر مجموعه نقشهای تعریف شده برای پاسخ دهنده است
            var accessListForResponser =
                GetAll()
                .Where(x =>
                x.IsActive &&
                (x.EndDate == null || (x.EndDate != null && x.EndDate >= DateTime.Now)) &&
                (x.StartDate == null || (x.StartDate != null && x.StartDate <= DateTime.Now))).ToList();

            var finalList = accessListForResponser.Where(x => x.Id == 0);

            foreach (var item in responserAccessList)
            {
                var result = accessListForResponser.Where(x => x.ProcessId == item.ProcessId);
                finalList = finalList.Concat(result);
            }
            return finalList.Distinct();

        }
        public IEnumerable<WF_AccessManagement> GetResponserAccessManagementList(string personId, string jopPositionCode)
        {
            // لیست نقش هایی که زیر مجموعه دارند
            // var ParentRoles =  _wfRoleRepository.GetParentRoles();
            var ParentRoles = _kscHrContext.WF_Roles.Where(x => x.IsActive);
            var p = ParentRoles.ToList();

            if (!ParentRoles.Any())
            {
                return null;
            }

            // لیست دسترسیهای اجرا و پاسخ پرسنل
            //return GetAllByPersonId(personId).Where(x => ParentRoles.ToList().Contains(x.WF_Role));
            return GetAllByPersonId(personId, jopPositionCode).ToList().Where(x => ParentRoles.ToList().Contains(x.WF_Role));
        }
        public IQueryable<WF_AccessManagement> GetValidWorkFlowAccessManagement()
        {
            return _kscHrContext.WF_AccessManagements
                .Where(x =>
                x.IsActive &&
                (x.EndDate == null || (x.EndDate != null && x.EndDate >= DateTime.Now)) &&
                (x.StartDate == null || (x.StartDate != null && x.StartDate <= DateTime.Now)));
        }
        public IEnumerable<WF_AccessManagement> GetAllByPersonIdProcessId(string personId, int processId)
        {
            var result = GetAllByPersonId(personId, null).Where(x => x.ProcessId == processId);
            return result;
        }

        public WF_AccessManagement GetWF_AccessManagementByProcessIdRoleId(int processId, int roleId)
        {
            var result = _kscHrContext.WF_AccessManagements.FirstOrDefault(x => x.ProcessId == processId && x.RoleId == roleId);
            return result;
        }
        public IQueryable<WF_AccessManagement> GetWF_AccessManagementByWinUser(string winUser)
        {
            var empl = _kscHrContext.ViewMisEmployee.FirstOrDefault(x => x.WinUser == winUser.Trim());
            string jobPositionCode = null;
            if (empl != null)
                jobPositionCode = empl.JobCategoryCode;
            var result = _kscHrContext.WF_AccessManagements.Include(a => a.WF_Process).Where(x => x.PersonId == winUser || (jobPositionCode!=null && x.JobPositionCode== jobPositionCode));
            return result;
        }
        public IQueryable<WF_AccessManagement> GetAllByPersonalId(string personId, string jopPositionCode) // متد جدید برای GetAllByPersonId
        {
            string currentUserJobPositionCode = jopPositionCode;
            var result = _kscHrContext.WF_AccessManagements
                .Where(x =>
                x.IsDeleted != true &&
                x.IsActive &&
                (x.PersonId == personId || (!string.IsNullOrEmpty(currentUserJobPositionCode) && !string.IsNullOrEmpty(x.JobPositionCode) && x.JobPositionCode == currentUserJobPositionCode)) &&
                (x.EndDate == null || (x.EndDate != null && x.EndDate >= DateTime.Now)) &&
                (x.StartDate == null || (x.StartDate != null && x.StartDate <= DateTime.Now)));
            return result;
        }
        public IQueryable<WF_AccessManagement> GetDataAccessListforResponser(string personId, string jopPositionCode)
        {
            // لیست دسترسیهای اجرا و پاسخ پرسنل
            var responserAccessList = GetResponserAccessManagementListNew(personId, jopPositionCode);
            if (responserAccessList == null || !responserAccessList.Any())
            {
                return null;
            }
            var ResponserRoles = responserAccessList.Select(x => x.RoleId);
            // لیست دسترسیهایی که نقش آنها زیر مجموعه نقشهای تعریف شده برای پاسخ دهنده است
            var accessListForResponser =
                _kscHrContext.WF_AccessManagements
                .Where(x =>
                x.IsActive &&
                (x.EndDate == null || (x.EndDate != null && x.EndDate >= DateTime.Now)) &&
                (x.StartDate == null || (x.StartDate != null && x.StartDate <= DateTime.Now)));

            var finalList = accessListForResponser.Where(x => x.Id == 0);

            foreach (var item in responserAccessList)
            {
                var result = accessListForResponser.Where(x => x.ProcessId == item.ProcessId);
                finalList = finalList.Concat(result);
            }
            return finalList.Distinct();

        }
        public IQueryable<WF_AccessManagement> GetResponserAccessManagementListNew(string personId, string jopPositionCode)
        {
            // لیست نقش هایی که زیر مجموعه دارند
            // var ParentRoles =  _wfRoleRepository.GetParentRoles();
            var ParentRoles = _kscHrContext.WF_Roles.Where(x => x.IsActive);
            var p = ParentRoles;

            if (!ParentRoles.Any())
            {
                return null;
            }

            // لیست دسترسیهای اجرا و پاسخ پرسنل
            //return GetAllByPersonId(personId).Where(x => ParentRoles.ToList().Contains(x.WF_Role));
            //return GetAllByPersonalId(personId, jopPositionCode).Where(x => ParentRoles.ToList().Contains(x.WF_Role));
            var personalData = GetAllByPersonalId(personId, jopPositionCode);
            var result = from person in personalData
                       join rol in ParentRoles on person.RoleId equals rol.Id
                       select person;
            return result;
        }
    }
}
