using EFCore.BulkExtensions;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.DTO.Emp.EmployeeDateInformation;
using Ksc.HR.Share.Model.EmployeeDateInformation;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.Model.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTPersianUtils.Core;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeDateInformationRepository : EfRepository<EmployeeDateInformation, int>, IEmployeeDateInformationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeDateInformationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public EmployeeDateInformation GetByEmployeeId(int employeeId) => _kscHrContext.EmployeeDateInformation.FirstOrDefault(a => a.EmployeeId == employeeId);
        public async Task<bool> AddBulkAsync(List<EmployeeDateInformation> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                    option.UseTempDB = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public void UpdateRange(List<EmployeeDateInformation> list)
        {
            _kscHrContext.EmployeeDateInformation.UpdateRange(list);
        }

        public async Task<bool> UpdateLastUpgradeDate(List<UpdateLastUpgradeDateModel> list, string username)
        {
            try
            {

                var allDate = _kscHrContext.EmployeeDateInformation.AsEnumerable().Where(x => list.Any(a => a.EmployeeId == x.EmployeeId));
                foreach (var item in allDate)
                {
                    item.UpdateDate = DateTime.Now;
                    item.UpdateUser = username;
                    item.LastUpgradeDate = list.Find(a => a.EmployeeId == item.EmployeeId).PromotionDate;
                }
                // _kscHrContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateLastUpgradeDate(List<UpdateDateModel> list, string username)
        {
            try
            {

                var allDate = GetAllQueryable().Where(x => list.Any(a => a.EmployeeId == x.EmployeeId));
                foreach (var item in allDate)
                {
                    var data = list.Find(a => a.EmployeeId == item.EmployeeId);
                    item.UpdateDate = DateTime.Now;
                    item.UpdateUser = username;
                    item.LastUpgradeDate = data.LastUpgradeDate;
                    item.UpgrateCategoryJobDate = data.UpgrateCategoryJobDate;
                    item.UpgrateGroupDateByChangePost = data.UpgrateGroupDateByChangePost;
                }
                // _kscHrContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IQueryable<EmployeeDateInformation> GetAllRelated() => _kscHrContext.EmployeeDateInformation.Include(x => x.Employee);

        public void UpdateLastUpgradeDate(int yearMonth)
        {
            var ListUpdate = new List<EmployeeDateInformationModel>();
            var counter = 0;
            try
            {
         
                var EmployeeInterdictQuery = _kscHrContext.EmployeeInterdicts

                       .Include(a => a.Chart_JobPosition)
                    .ThenInclude(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory)
                    .ThenInclude(a => a.Chart_JobCategoryDefination).AsNoTracking();
                var WorkCalendars = _kscHrContext.WorkCalendars.AsQueryable().Where(a=>a.YearMonthV1== yearMonth).OrderBy(a=>a.MiladiDateV1).FirstOrDefault();
                var filteredInterdict = EmployeeInterdictQuery.Where(a => a.InterdictTypeId == EnumInterdictType.NotifPost.Id)
                    .Where(a => a.IssuanceDate.HasValue&& a.IssuanceDate.Value >= WorkCalendars.MiladiDateV1)
                .ToList();

                var employeesId = filteredInterdict.Select(a => a.EmployeeId).ToList();

                var employee = _kscHrContext.Employees.Where(a => employeesId.Contains(a.Id))
                    .Where(x =>
                                x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                                x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.TeamWorkId != null
                          )
                    .AsNoTracking()
                    .Include(a => a.EmployeeInterdicts)
                    .ThenInclude(a => a.Chart_JobPosition)
                    .ThenInclude(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory)
                    .ThenInclude(a => a.Chart_JobCategoryDefination)
                    .Select(a => new
                    {
                        a.Id,
                        a.EmployeeNumber,
                        a.Name,
                        a.Family,
                        
                        EmployeeInterdicts = a.EmployeeInterdicts
                         .Where(a => a.InterdictTypeId == EnumInterdictType.NotifPost.Id)
                        .OrderByDescending(a => a.ExecuteDate)
                        .FirstOrDefault()
                    }).ToList();
                var employeeinterdictFilter = EmployeeInterdictQuery.Where(a => employeesId.Contains(a.EmployeeId))
                .ToList();

                var employeeDateInformation = _kscHrContext.EmployeeDateInformation
                    .Where(a => employeesId.Contains(a.EmployeeId)).ToList();


                foreach (var item in employee)
                {
                    counter += 1;
                    var interdict = item.EmployeeInterdicts;
                    var prevInterdict = employeeinterdictFilter.Where(a => a.EmployeeId == item.Id)
                        .Where(a => a.ExecuteDate < interdict.ExecuteDate).OrderByDescending(a => a.ExecuteDate)
                        .FirstOrDefault();
                    if (prevInterdict == null) continue;

                    var findEmployeeDateInformation = employeeDateInformation.FirstOrDefault(a => a.EmployeeId == item.Id);

                    if (findEmployeeDateInformation != null)
                    {
                        var currentJobGroup = interdict.CurrentJobGroupId;
                        var prevGroup = prevInterdict.CurrentJobGroupId;
                        if (currentJobGroup != prevGroup)
                        {
                            findEmployeeDateInformation.UpgrateGroupDateByChangePost = interdict.ExecuteDate;

                        }
                        var currentDefinition = interdict.Chart_JobPosition.Chart_JobIdentity.Chart_JobCategory.JobCategoryDefinationId;
                        var prevDefinition = prevInterdict.Chart_JobPosition.Chart_JobIdentity.Chart_JobCategory.JobCategoryDefinationId;
                        if (currentDefinition != prevDefinition)
                        {
                            findEmployeeDateInformation.UpgrateCategoryJobDate = interdict.ExecuteDate;
                        }
                        if (currentDefinition != prevDefinition || currentJobGroup != prevGroup)
                        {                           
                            _kscHrContext.EmployeeDateInformation.Update(findEmployeeDateInformation);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
