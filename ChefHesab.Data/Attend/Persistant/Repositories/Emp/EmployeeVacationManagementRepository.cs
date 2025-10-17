using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;

using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using Ksc.HR.Share.Model.Vacation;
using Ksc.HR.Share.General;
using Ksc.HR.Domain.Entities.Chart;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class EmployeeVacationManagementRepository : EfRepository<EmployeeVacationManagement, long>, IEmployeeVacationManagementRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeVacationManagementRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeVacationManagement> GetAllRelated()
        {
            return _kscHrContext.EmployeeVacationManagements.AsQueryable().Include(a => a.Employee).Include(a => a.Vacation)
                ;
               

        }

        public IQueryable<EmployeeVacationManagement> GetByIdRelated(int id)
        {
            return _kscHrContext.EmployeeVacationManagements.Where(x => x.Id == id)
                .AsQueryable();

        }

        public IQueryable<EmployeeVacationManagement> GetByEmployeeId(int employeeId)
        {
            return _kscHrContext.EmployeeVacationManagements.Where(x => x.EmployeeId == employeeId)
                .AsQueryable();

        }

        public async Task<bool> AddBulkAsync(EmployeeVacationManagement entity)
        {
            try
            {
                List<EmployeeVacationManagement> list = new List<EmployeeVacationManagement>();
                list.Add(entity);
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });
                await _kscHrContext.BulkSaveChangesAsync();


                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> AddBulkAsync(List<EmployeeVacationManagement> list)
        {
            try
            {
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = false;
                });

                await _kscHrContext.BulkSaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> UpdateBulkAsync(EmployeeVacationManagement entity)
        {
            try
            {
                List<EmployeeVacationManagement> list = new List<EmployeeVacationManagement>();
                list.Add(entity);
                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });
                


                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public async Task<bool> SaveBulkAsync()
        {
            try
            {
                await _kscHrContext.BulkSaveChangesAsync();


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void UpdateRange(List<EmployeeVacationManagement> list)
        {
            _kscHrContext.EmployeeVacationManagements.UpdateRange(list);
        }


        public async Task<bool> UpdateBulkAsync(List<EmployeeVacationManagement> list)
        {
            try
            {
            
                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = false;
                    //option.UseTempDB = true;
                    //option.SetOutputIdentity = true;
                    //option.CalculateStats = true;
                    //option.BatchSize = 4000;
                });
                await _kscHrContext.BulkSaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public async Task<bool> UpdateBulkWithoutSaveAsync(List<EmployeeVacationManagement> list)
        {
            try
            {

                await _kscHrContext.BulkUpdateAsync(list, option =>
                {
                    option.IncludeGraph = false;
                    option.UseTempDB = true;
                    option.SetOutputIdentity = true;
                    option.CalculateStats = true;
                    option.BatchSize = 4000;
                });


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        /// <summary>
        /// استفاده شده سال جاری
        /// </summary>
        /// <returns></returns>
        public void UpdateUsedCurrentYear(int employeeId,double duration,string remark, string userName)
        {
            var UsedCurrentYear = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.UsedCurrentYear.Id.ToString());
            var EmployeeVacationLog = new EmployeeVacationManagementLog()
            {
                EmployeeVacationManagementId = UsedCurrentYear.Id,
                InsertDate = UsedCurrentYear.InsertDate,
                InsertUser = UsedCurrentYear.InsertUser,
                UpdateDate = DateTime.Now,
                UpdateUser = userName,
                ValueDuration = UsedCurrentYear.ValueDuration,
                Remark = UsedCurrentYear.Remark,

            };
            _kscHrContext.EmployeeVacationManagementLogs.Add(EmployeeVacationLog);


            var LastYearVacationRemaining = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.LastYearVacationRemaining.Id.ToString());
            var CurrentYearMerit = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.CurrentYearMerit.Id.ToString());
            var MeritVacationRemaining = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.MeritVacationRemaining.Id.ToString());
            var MeritVacationRemainingLog = new EmployeeVacationManagementLog()
            {
                EmployeeVacationManagementId = MeritVacationRemaining.Id,
                InsertDate = MeritVacationRemaining.InsertDate,
                InsertUser = MeritVacationRemaining.InsertUser,
                UpdateDate = DateTime.Now,
                UpdateUser = userName,
                ValueDuration = MeritVacationRemaining.ValueDuration,
                Remark = MeritVacationRemaining.Remark,

            };
            _kscHrContext.EmployeeVacationManagementLogs.Add(MeritVacationRemainingLog);
            UsedCurrentYear.Duration = UsedCurrentYear.Duration+ duration;
            UsedCurrentYear.ValueDuration = UsedCurrentYear.Duration.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            UsedCurrentYear.Remark = remark;
            if (UsedCurrentYear.Duration < 0)
            {
                throw new Exception("مقدار استفاده مرخصی نمیتواند منفی شود");

            }
            MeritVacationRemaining.Duration = (LastYearVacationRemaining.Duration + CurrentYearMerit.Duration) - UsedCurrentYear.Duration;
            MeritVacationRemaining.ValueDuration = MeritVacationRemaining.Duration.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            MeritVacationRemaining.Remark = remark;

        }




        /// <summary>
        /// استحقاق سال جاری
        /// </summary>
        /// <returns></returns>
        public void UpdateCurrentYearMerit(int employeeId, double duration, string remark, string userName)
        {
            var CurrentYearMerit = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.CurrentYearMerit.Id.ToString());
            var EmployeeVacationLog = new EmployeeVacationManagementLog()
            {
                EmployeeVacationManagementId = CurrentYearMerit.Id,
                InsertDate = CurrentYearMerit.InsertDate,
                InsertUser = CurrentYearMerit.InsertUser,
                UpdateDate = DateTime.Now,
                UpdateUser = userName,
                ValueDuration = CurrentYearMerit.ValueDuration,
                Remark = CurrentYearMerit.Remark,

            };
            _kscHrContext.EmployeeVacationManagementLogs.Add(EmployeeVacationLog);

            var UsedCurrentYear = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.UsedCurrentYear.Id.ToString());


            var LastYearVacationRemaining = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.LastYearVacationRemaining.Id.ToString());


            var MeritVacationRemaining = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.MeritVacationRemaining.Id.ToString());
            var MeritVacationRemainingLog = new EmployeeVacationManagementLog()
            {
                EmployeeVacationManagementId = MeritVacationRemaining.Id,
                InsertDate = MeritVacationRemaining.InsertDate,
                InsertUser = MeritVacationRemaining.InsertUser,
                UpdateDate = DateTime.Now,
                UpdateUser = userName,
                ValueDuration = MeritVacationRemaining.ValueDuration,
                Remark = MeritVacationRemaining.Remark,

            };
            _kscHrContext.EmployeeVacationManagementLogs.Add(MeritVacationRemainingLog);
            CurrentYearMerit.Duration =  duration;
            CurrentYearMerit.ValueDuration = CurrentYearMerit.Duration.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            CurrentYearMerit.Remark = remark;

            MeritVacationRemaining.Duration = (LastYearVacationRemaining.Duration + CurrentYearMerit.Duration) - UsedCurrentYear.Duration;
            MeritVacationRemaining.ValueDuration = MeritVacationRemaining.Duration.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            MeritVacationRemaining.Remark = remark;
        }







        /// <summary>
        /// مانده مرخصی سال قبل
        /// </summary>
        /// <returns></returns>
        public void UpdateLastYearVacationRemaining(int employeeId, double duration, string remark,string userName,bool isSurplusVacation=false)
        {
            var UsedCurrentYear = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a =>a.EmployeeId== employeeId&& a.Vacation.Code == EnumVacation.UsedCurrentYear.Id.ToString());


            var LastYearVacationRemaining = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.LastYearVacationRemaining.Id.ToString());
         
            

            var EmployeeVacationLog =new EmployeeVacationManagementLog() { 
            EmployeeVacationManagementId = LastYearVacationRemaining.Id,
            InsertDate = LastYearVacationRemaining.InsertDate,
            InsertUser = LastYearVacationRemaining.InsertUser,
            UpdateDate = DateTime.Now,
            UpdateUser = userName,
            ValueDuration=   LastYearVacationRemaining.ValueDuration,
            Remark = LastYearVacationRemaining.Remark,

            };
            _kscHrContext.EmployeeVacationManagementLogs.Add(EmployeeVacationLog);


          

            var CurrentYearMerit = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.CurrentYearMerit.Id.ToString());

            var MeritVacationRemaining = _kscHrContext.EmployeeVacationManagements.FirstOrDefault(a => a.EmployeeId == employeeId && a.Vacation.Code == EnumVacation.MeritVacationRemaining.Id.ToString());
            var MeritVacationRemainingLog = new EmployeeVacationManagementLog()
            {
                EmployeeVacationManagementId = MeritVacationRemaining.Id,
                InsertDate = MeritVacationRemaining.InsertDate,
                InsertUser = MeritVacationRemaining.InsertUser,
                UpdateDate = DateTime.Now,
                UpdateUser = userName,
                ValueDuration = MeritVacationRemaining.ValueDuration,
                Remark = MeritVacationRemaining.Remark,

            };
            _kscHrContext.EmployeeVacationManagementLogs.Add(MeritVacationRemainingLog);





            LastYearVacationRemaining.Duration = LastYearVacationRemaining.Duration + duration;
            LastYearVacationRemaining.ValueDuration = LastYearVacationRemaining.Duration.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            LastYearVacationRemaining.Remark = remark;

            

            MeritVacationRemaining.Duration = (LastYearVacationRemaining.Duration + CurrentYearMerit.Duration) - UsedCurrentYear.Duration;
            MeritVacationRemaining.ValueDuration = MeritVacationRemaining.Duration.Value.ConvertMinDobleWithPaddLeftToDaysHour(9 * 60);
            MeritVacationRemaining.Remark = remark;
        }

    }
}

