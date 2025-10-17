using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Common;
using Ksc.HR.DTO.WorkShift;
using EFCore.BulkExtensions;

using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Share.WorkShift;



namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class EmployeeJobPositionRepository : EfRepository<EmployeeJobPosition, int>, IEmployeeJobPositionRepository
    {
        private readonly KscHrContext _kscHrContext;

        public EmployeeJobPositionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            
            _kscHrContext = KscHrContext;
        }
        public async Task<bool> AddBulkAsync(List<EmployeeJobPosition> list)
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
        public IQueryable<EmployeeJobPosition> GetAllRelated()
        {
            return _kscHrContext.EmployeeJobPositions
                .Include(x => x.Employee).ThenInclude(x => x.PaymentStatus)
                .Include(x => x.Chart_JobPosition)
                .AsQueryable()
             .AsNoTracking();

        }

        public IQueryable<EmployeeJobPosition> GetByIdRelated(int id)
        {
            return _kscHrContext.EmployeeJobPositions.Where(x => x.Id == id)
                .Include(x => x.Chart_JobPosition)
                .AsQueryable();

        }

        public IQueryable<EmployeeJobPosition> GetByEmployeeIdRelated(int? employeeId)
        {
            return _kscHrContext.EmployeeJobPositions.Where(x => x.EmployeeId == employeeId && x.EndDate == null)
                .Include(x => x.Chart_JobPosition)
                .ThenInclude(x => x.Chart_JobIdentity)
                .ThenInclude(x => x.Chart_JobCategory)
            .AsQueryable();

        }  
        public IQueryable<EmployeeJobPosition> GetHistoriesRelocationByEmployeeIdRelated(int? employeeId)
        {
            return _kscHrContext.EmployeeJobPositions.Where(x => x.EmployeeId == employeeId && x.EndDate.HasValue)
                .Include(x => x.Chart_JobPosition)
                .ThenInclude(x => x.Chart_JobPositionIncreasePercents.Where(a=>a.IsActive))
            .AsQueryable();

        }
        public EmployeeJobPosition GetLastJobPositionEmployeeId(int? employeeId)
        {
            return _kscHrContext.EmployeeJobPositions.Where(x => employeeId == x.EmployeeId && x.IsActive && !x.EndDate.HasValue).AsNoTracking()
                    .OrderByDescending(a => a.StartDate)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory).ThenInclude(a => a.Chart_JobCategoryDefination)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobPositionFields).ThenInclude(a => a.StudyField)
                    .FirstOrDefault();


        }

        public List<EmployeeJobPosition> Get2LastRelatedJobPositionsEmployeeId(int? employeeId)
        {
            return _kscHrContext.EmployeeJobPositions.Where(x => employeeId == x.EmployeeId ).AsNoTracking()
                    .OrderByDescending(a => a.StartDate)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory).ThenInclude(a => a.Chart_JobCategoryDefination)
                    .Include(a => a.Chart_JobPosition).ThenInclude(a => a.Chart_JobPositionFields).ThenInclude(a => a.StudyField)
                    .Take(2).ToList();


        }

        public IQueryable<EmployeeJobPosition> GetEmployeeJobPositionByEmployeeId(int? employeeId)
        {
            return _kscHrContext.EmployeeJobPositions.Where(x => x.EmployeeId == employeeId )            
                .AsQueryable();

        }
        public async Task<KscResult> AddEmployeeJobPosition(EmployeeJobPosition model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;


            try
            {
                //await chackForSave(model, result);
                //comment
                #region checkForSave
                var employee = _kscHrContext.Employees//.GetEmployeeWorkGroupWorkTimeByRelated()
                    .Include(x => x.WorkGroup)
                    .AsNoTracking()
                    .Where(x => x.Id == model.EmployeeId)
               .FirstOrDefault();

                var jobposition = _kscHrContext.Chart_JobPosition//.GetChart_JobPositions()
                    .Where(x => x.Id == model.JobPositionId)
                    .Include(x => x.EmployeeJobPositions)
                    .ThenInclude(a => a.Employee)
                    .FirstOrDefault();


                var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);
                var countInThisJob = jobposition.EmployeeJobPositions.Count(); // تعداد افرادی که به این شغل متصل هستند

                if (employee.WorkGroup != null)
                {
                    if (roozkarCode == employee.WorkGroup.WorkTimeId) // روزکار
                    {
                        var RozkarRemmingCount = jobposition.WorkingDayCount - jobposition.EmployeeJobPositions.Where(a => a.Employee.WorkGroupId == employee.WorkGroupId).Count();
                        if (RozkarRemmingCount <= 0)
                        {
                            result.AddError("خطا", "ظرفیت پست انتخاب شده پر می باشد");
                            return result;
                        }
                    }
                    else // غیر از روزکار
                    {
                        var ShiftRemmingCount = jobposition.WorkingShiftCount - jobposition.EmployeeJobPositions.Where(a => a.Employee.WorkGroupId == employee.WorkGroupId).Count();
                        if (ShiftRemmingCount <= 0)
                        {
                            result.AddError("خطا", "ظرفیت پست انتخاب شده پر می باشد");
                            return result;
                        }
                    }
                }
                else
                {
                    result.AddError("خطا", "نوع کارکرد بایستی مشخص باشد");
                    return result;
                }
                #endregion

                //var Obj = _mapper.Map<EmployeeJobPosition>(model);
                var Obj = new EmployeeJobPosition
                {
                    EmployeeId = model.EmployeeId,
                    JobPositionId = model.JobPositionId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsActive = true,
                    InsertDate = model.InsertDate,
                    InsertUser = model.InsertUser,
                };

                await _kscHrContext.EmployeeJobPositions.AddAsync(Obj);


            }
            catch (Exception ex)
            {

                result.AddError("خطا", ex.Message);
                return result;
            }

            return result;
        }

        public KscResult UpdateEmployeeJobPosition(EmployeeJobPosition model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneEmployeeJobPosition = _kscHrContext.EmployeeJobPositions
                .Where(x => x.EmployeeId == model.EmployeeId && x.JobPositionId == model.JobPositionId)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
            if (oneEmployeeJobPosition == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            //await chackForSave(model, result);
            //comment
            #region checkForSave
            var employee = _kscHrContext.Employees//.GetEmployeeWorkGroupWorkTimeByRelated()
                    .Include(x => x.WorkGroup)
                    .AsNoTracking()
                    .Where(x => x.Id == model.EmployeeId)
               .FirstOrDefault();

            var jobposition = _kscHrContext.Chart_JobPosition//.GetChart_JobPositions()
                .Where(x => x.Id == model.JobPositionId)
                .Include(x => x.EmployeeJobPositions)
                .ThenInclude(a => a.Employee)
                .FirstOrDefault();


            var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);
            var countInThisJob = jobposition.EmployeeJobPositions.Count(); // تعداد افرادی که به این شغل متصل هستند

            if (employee.WorkGroup != null)
            {
                if (roozkarCode == employee.WorkGroup.WorkTimeId) // روزکار
                {
                    var RozkarRemmingCount = jobposition.WorkingDayCount - jobposition.EmployeeJobPositions.Where(a => a.Employee.WorkGroupId == employee.WorkGroupId).Count();
                    if (RozkarRemmingCount <= 0)
                    {
                        result.AddError("خطا", "ظرفیت پست انتخاب شده پر می باشد");
                        return result;
                    }
                }
                else // غیر از روزکار
                {
                    var ShiftRemmingCount = jobposition.WorkingShiftCount - jobposition.EmployeeJobPositions.Where(a => a.Employee.WorkGroupId == employee.WorkGroupId).Count();
                    if (ShiftRemmingCount <= 0)
                    {
                        result.AddError("خطا", "ظرفیت پست انتخاب شده پر می باشد");
                        return result;
                    }
                }
            }
            else
            {
                result.AddError("خطا", "نوع کارکرد بایستی مشخص باشد");
                return result;
            }
            #endregion

            //var EmployeeJobPositionObj = _mapper.Map<EmployeeJobPosition>(model); // انچه از صفحه داریم با مپر
            var EmployeeJobPositionObj = new EmployeeJobPosition
            {
                Id = oneEmployeeJobPosition.Id,
                EmployeeId = oneEmployeeJobPosition.EmployeeId,

                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive,
                JobPositionId = model.JobPositionId,
                UpdateUser = model.UpdateUser,

                UpdateDate = model.UpdateDate,
                InsertDate = oneEmployeeJobPosition.InsertDate,
                InsertUser = oneEmployeeJobPosition.InsertUser,
            };


            _kscHrContext.EmployeeJobPositions.Update(EmployeeJobPositionObj); // ذخیره داده ای که مپ شده


            return result;
        }




        #region // تغییر سوابق شغلی
        //پارامترها:::>>>> UpdateUser,, EmployeeInterdictId,StartDate,JobPositionId

        public async Task<KscResult> UpdateEmployeeJobPositionForInterdict(EmployeeJobPosition model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            try { 
            var oneEmployeeJobPosition = _kscHrContext.EmployeeJobPositions.Where(x => x.EmployeeId == model.EmployeeId
            && x.EndDate == null && x.IsActive).FirstOrDefault();//آخرین رکورد فرد 

            if (oneEmployeeJobPosition == null)
            {
                result.AddError("سوابق شغلی وجود ندارد", "سوابق شغلی وجود ندارد");
                return result;
            }


            //آپدیت رکورد آخر فرد
            oneEmployeeJobPosition.UpdateDate = DateTime.Now;
            oneEmployeeJobPosition.EndDate = model.StartDate.AddDays(-1);//تاریخ پایان با یک روز قبل پر می شود
            oneEmployeeJobPosition.UpdateUser = model.UpdateUser;
            _kscHrContext.EmployeeJobPositions.Update(oneEmployeeJobPosition); // 
      
            //اضافه کردن رکورد جدید
            var EmployeeJobPositionObj = new EmployeeJobPosition
            {

                EmployeeId = model.EmployeeId,  
                JobPositionId = model.JobPositionId,
                EndDate = null,
                StartDate = model.StartDate,
                EmployeeInterdictId = model.EmployeeInterdictId,//شناسه حکم روی رکورد جدید فرد ثبت می شود
                IsActive = true,
                InsertDate = DateTime.Now,
                InsertUser = model.UpdateUser,
                UpdateUser = null,
                UpdateDate = null,

            };
            await _kscHrContext.EmployeeJobPositions.AddAsync(EmployeeJobPositionObj); // ذخیره داده ای که مپ شده

            //آپدیت شغل در جدول employee

            var employee =  _kscHrContext.Employees.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
            if (employee != null)
            {
                employee.JobPositionId= model.JobPositionId;
                employee.JobPositionStartDate = model.StartDate;
                employee.UpdateUser = model.UpdateUser;
                employee.UpdateDate = DateTime.Now;
            }
            _kscHrContext.Employees.Update(employee); // 

            // await _kscHrContext.SaveAsync();


        }
            catch (Exception ex)
            {

                result.AddError("خطا", ex.Message);
                return result;
            }
            return result;
        }
        #endregion


        public List<EmployeeJobPosition> GetEmployeeJobPositionByEmployeeNumbersAsNoList(List<string> employeeId)
        {
            var result = _kscHrContext.EmployeeJobPositions.AsNoTracking().Where(x => employeeId.Contains(x.EmployeeId.ToString())).ToList();
            return result;
        }
    }
}
