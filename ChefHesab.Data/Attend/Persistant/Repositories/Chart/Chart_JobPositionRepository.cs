using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Chart;

using Ksc.HR.Share.Chart.ChartJobPosition;
using Ksc.HR.Share.Model.Chart;
using Ksc.HR.Share.Model.JobPositionStatus;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.WorkShift;
using KSC.Common;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Extention;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class Chart_JobPositionRepository : EfRepository<Chart_JobPosition, int>, IChart_JobPositionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobPositionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_JobPosition> getActived()
        {
            return _kscHrContext.Chart_JobPosition.AsQueryable().Where(a => a.IsActive)
                .Include(a => a.Chart_JobPositionScores)
                .Include(a => a.Chart_JobPositionIncreasePercents);

        }

        public List<Tuple<Chart_JobPosition, int>> GetListRemainingJobPosition(List<int> jobpositionId)
        {
            var result = new List<Tuple<Chart_JobPosition, int>>();
            try
            {

                var findAllPost = GetChart_JobPositions()
                     .Where(x => jobpositionId.Contains(x.Id))
                     .ToList();

                var employeeActived = _kscHrContext.Employees.Where(x =>
                x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
                x.TeamWorkId != null && jobpositionId.Contains(x.JobPositionId.Value))
                    .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                    .ToList();

                var getReloCationRequested = _kscHrContext.Relocations
                   .Where(a => jobpositionId.Contains(a.DestinationJobPositionId.Value)
                               && (a.IsEnd == null || a.IsEnd.Value == false))

                   .ToList();


                foreach (var jobposition in findAllPost)
                {


                    var ReloCationRequested = getReloCationRequested
                        .Where(a => a.DestinationJobPositionId == jobposition.Id)
                        .ToList();
                    var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

                    var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == jobposition.Id).Count();
                    var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == jobposition.Id).Count();
                    var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == jobposition.Id);



                    var GetSumrequest = ReloCationRequested.Count();
                    var GetSumWorkDayrequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
                    var GetSumShiftRequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

                    var CapacityCount = 0;
                    var WorkingDayCount = 0;
                    var WorkingShiftCount = 0;
                    if (jobposition.WorkingDayCount == 0 && jobposition.WorkingShiftCount == 0)
                    {
                        CapacityCount = ((jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

                    }
                    else
                    {
                        CapacityCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount + jobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                        WorkingDayCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                        WorkingShiftCount = ((jobposition.WorkingShiftCount + jobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

                    }

                    var item = new Tuple<Chart_JobPosition, int>(jobposition, CapacityCount);
                    result.Add(item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public List<Tuple<Chart_JobPosition, int, int>> GetListRemainingJobPositionWithEmployeeCount(List<int> jobpositionId)
        {
            var result = new List<Tuple<Chart_JobPosition, int, int>>();
            try
            {

                var findAllPost = GetChart_JobPositions()
                     .Where(x => jobpositionId.Contains(x.Id))
                     .ToList();

                var employeeActived = _kscHrContext.Employees.Where(x =>
                x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
                x.TeamWorkId != null && jobpositionId.Contains(x.JobPositionId.Value))
                    .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                    .ToList();

                var getReloCationRequested = _kscHrContext.Relocations
                   .Where(a => jobpositionId.Contains(a.DestinationJobPositionId.Value)
                               && (a.IsEnd == null || a.IsEnd.Value == false))

                   .ToList();


                foreach (var jobposition in findAllPost)
                {


                    var ReloCationRequested = getReloCationRequested
                        .Where(a => a.DestinationJobPositionId == jobposition.Id)
                        .ToList();
                    var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

                    var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == jobposition.Id).Count();
                    var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == jobposition.Id).Count();
                    var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == jobposition.Id);



                    var GetSumrequest = ReloCationRequested.Count();
                    var GetSumWorkDayrequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
                    var GetSumShiftRequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

                    var CapacityCount = 0;
                    var WorkingDayCount = 0;
                    var WorkingShiftCount = 0;
                    if (jobposition.WorkingDayCount == 0 && jobposition.WorkingShiftCount == 0)
                    {
                        CapacityCount = ((jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

                    }
                    else
                    {
                        CapacityCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount + jobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                        WorkingDayCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                        WorkingShiftCount = ((jobposition.WorkingShiftCount + jobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

                    }

                    var item = new Tuple<Chart_JobPosition, int, int>(jobposition, CapacityCount, totalCountemployeee);
                    result.Add(item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// بدست آوردن ظرفیت باقیمانده یک پست
        /// item1=WorkDayCount
        /// item2=shiftWorkcount
        /// item3=CapacityCount
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public Tuple<int, int, int> GetRemainingCapacity(int jobPositionId)
        {
            var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

            var jobposition = GetChart_JobPositions()
                    .Where(x => x.Id == jobPositionId)
                    .Include(x => x.EmployeeJobPositions)
                    .ThenInclude(a => a.Employee)
                    .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception("پست غیر فعال است");
            }
            var getReloCationRequested = _kscHrContext.Relocations
                .Where(a => a.DestinationJobPositionId == jobPositionId && (a.IsEnd == null || a.IsEnd.Value == false))
                .ToList();

            var employeeActived = _kscHrContext.Employees.Where(x =>
              x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
              x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
              x.TeamWorkId != null && x.JobPositionId.Value == jobPositionId)
                  .Include(a => a.TeamWork)
                  .Include(a => a.WorkGroup)
                  .ThenInclude(a => a.WorkTime)
                  .ToList();


            var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode).Count();
            var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode).Count();
            var totalCountemployeee = employeeActived.Count();





            var GetSumrequest = getReloCationRequested.Count();
            var GetSumWorkDayrequest = getReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
            var GetSumShiftRequest = getReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);


            var CapacityCount = 0;
            var WorkingDayCount = 0;
            var WorkingShiftCount = 0;
            if (jobposition.WorkingDayCount == 0 && jobposition.WorkingShiftCount == 0)
            {
                CapacityCount = ((jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

            }
            else
            {
                CapacityCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount + jobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                WorkingDayCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                WorkingShiftCount = ((jobposition.WorkingShiftCount + jobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

            }


            return new Tuple<int, int, int>(WorkingDayCount, WorkingShiftCount, CapacityCount);

        }

        /// <summary>
        /// بدست اوردن افراد زیر مجموعه یک پست
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Employee> GetSubGroupEmployeeFromJobPositionId(int jobPositionId, bool isWantCheckRemain = false, bool isCanSeeAll = false
            , bool excludeEndStructure = true)
        {
            var employeeIdList = new List<Employee>();
            try
            {

                var jobposition = GetChart_JobPositions()
                     .Where(x => x.Id == jobPositionId)
                     .FirstOrDefault();
                if (jobposition.IsActive == false)
                {
                    throw new Exception("پست غیر فعال است");
                }
                var findAllChildrenPost = GetChart_JobPositions().AsNoTracking()
                     .Where(x => x.NewCodeRelation.Contains(jobposition.MisJobPositionCode) &&
                     x.Id != jobposition.Id && x.IsActive == true && x.JobIdentityId.HasValue
                     && x.EndDate.HasValue == false

                     )
                     .Include(a => a.Chart_JobIdentity)
                     .ThenInclude(a => a.Chart_JobCategory)
                     .ThenInclude(a => a.Chart_JobCategoryDefination)

                     .ToList();

                if (excludeEndStructure)
                {
                    findAllChildrenPost = findAllChildrenPost.Where(x => x.StructureEndDate.HasValue == false).ToList();
                }

                if (isCanSeeAll == false)
                {
                    findAllChildrenPost = findAllChildrenPost.Where(a => a.Chart_JobIdentity.Chart_JobCategory.AllowTransfer == true).ToList();
                }

                var allSubGroupjobPositionIds = findAllChildrenPost.Select(x => x.Id).ToList();

                var employeeActived = _kscHrContext.Employees.Where(x =>
                x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
                x.TeamWorkId != null && allSubGroupjobPositionIds.Contains(x.JobPositionId.Value))
                    .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                    .ToList();

                var getReloCationRequested = _kscHrContext.Relocations
                   .Where(a => allSubGroupjobPositionIds.Contains(a.DestinationJobPositionId.Value)
                               && (a.IsEnd == null || a.IsEnd.Value == false))

                   .ToList();


                foreach (var subgroupJobPositionId in allSubGroupjobPositionIds)
                {
                    var findjobposition = findAllChildrenPost
                        .FirstOrDefault(x => x.Id == subgroupJobPositionId);

                    var ReloCationRequested = getReloCationRequested
                        .Where(a => a.DestinationJobPositionId == subgroupJobPositionId)
                        .ToList();
                    var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

                    var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                    var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                    var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == subgroupJobPositionId);



                    var GetSumrequest = ReloCationRequested.Count();
                    var GetSumWorkDayrequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
                    var GetSumShiftRequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

                    var CapacityCount = 0;
                    var WorkingDayCount = 0;
                    var WorkingShiftCount = 0;
                    if (findjobposition.WorkingDayCount == 0 && findjobposition.WorkingShiftCount == 0)
                    {
                        CapacityCount = ((findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

                    }
                    else
                    {
                        CapacityCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount + findjobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                        WorkingDayCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                        WorkingShiftCount = ((findjobposition.WorkingShiftCount + findjobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

                    }


                    var getEmployees = employeeActived.Where(a => a.JobPositionId == subgroupJobPositionId).ToList();

                    if (isWantCheckRemain == true)
                    {
                        if (CapacityCount > 0)
                        {

                            employeeIdList.AddRange(getEmployees);
                        }
                    }
                    else
                    {
                        employeeIdList.AddRange(getEmployees);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeeIdList;

        }

        /// <summary>
        /// بدست اوردن افراد زیر مجموعه یک پست
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Employee> GetAllEmployeeFromJobPositionId(int jobPositionId, bool isWantCheckRemain = false, bool isCanSeeAll = false)
        {
            var employeeIdList = new List<Employee>();
            try
            {

                var jobposition = GetChart_JobPositions()
                     .Where(x => x.Id == jobPositionId)
                     .FirstOrDefault();
                if (jobposition.IsActive == false)
                {
                    throw new Exception("پست غیر فعال است");
                }
                var findAllChildrenPost = GetChart_JobPositions().AsNoTracking()
                     .Where(x => (x.NewCodeRelation.Contains(jobposition.MisJobPositionCode) || x.Id == jobPositionId) && x.IsActive == true && x.JobIdentityId.HasValue)
                     .Include(a => a.Chart_JobIdentity)
                     .ThenInclude(a => a.Chart_JobCategory)
                     .ThenInclude(a => a.Chart_JobCategoryDefination)

                     .ToList();

                if (isCanSeeAll == false)
                {
                    findAllChildrenPost = findAllChildrenPost.Where(a => a.Chart_JobIdentity.Chart_JobCategory.AllowTransfer == true).ToList();
                }

                var allSubGroupjobPositionIds = findAllChildrenPost.Select(x => x.Id).ToList();

                var employeeActived = _kscHrContext.Employees.Where(x =>
                x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
                x.TeamWorkId != null && allSubGroupjobPositionIds.Contains(x.JobPositionId.Value))
                    .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                    .ToList();

                var getReloCationRequested = _kscHrContext.Relocations
                   .Where(a => allSubGroupjobPositionIds.Contains(a.DestinationJobPositionId.Value)
                               && (a.IsEnd == null || a.IsEnd.Value == false))

                   .ToList();


                foreach (var subgroupJobPositionId in allSubGroupjobPositionIds)
                {
                    var findjobposition = findAllChildrenPost
                        .FirstOrDefault(x => x.Id == subgroupJobPositionId);

                    var ReloCationRequested = getReloCationRequested
                        .Where(a => a.DestinationJobPositionId == subgroupJobPositionId)
                        .ToList();
                    var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

                    var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                    var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                    var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == subgroupJobPositionId);



                    var GetSumrequest = ReloCationRequested.Count();
                    var GetSumWorkDayrequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
                    var GetSumShiftRequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

                    var CapacityCount = 0;
                    var WorkingDayCount = 0;
                    var WorkingShiftCount = 0;
                    if (findjobposition.WorkingDayCount == 0 && findjobposition.WorkingShiftCount == 0)
                    {
                        CapacityCount = ((findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

                    }
                    else
                    {
                        CapacityCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount + findjobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                        WorkingDayCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                        WorkingShiftCount = ((findjobposition.WorkingShiftCount + findjobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

                    }


                    var getEmployees = employeeActived.Where(a => a.JobPositionId == subgroupJobPositionId).ToList();

                    if (isWantCheckRemain == true)
                    {
                        if (CapacityCount > 0)
                        {

                            employeeIdList.AddRange(getEmployees);
                        }
                    }
                    else
                    {
                        employeeIdList.AddRange(getEmployees);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeeIdList;

        }

        /// <summary>
        /// بدست اوردن پست های زیر مجموعه یک پست
        /// پست هایی که ظرفیت دارند را نمایش میدهد
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Chart_JobPosition> GetSubGroupJobPostionFromJobPositionId(int jobPositionId, bool isCanSeeAll = false, bool excludeEndStructure = true)
        {
            var jobpositionIds = new List<Chart_JobPosition>();
            var jobposition = GetChart_JobPositions()
                 .Where(x => x.Id == jobPositionId)
                 .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception("پست غیر فعال است");
            }
            var findAllChildrenPost = GetChart_JobPositions().AsNoTracking()
                      .Where(x => x.NewCodeRelation.Contains(jobposition.MisJobPositionCode)
                      && x.Id != jobposition.Id && x.IsActive == true && x.JobIdentityId.HasValue
                       && x.EndDate.HasValue == false
                      )
                      .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryDefination)

                      .ToList();

            if (excludeEndStructure)
            {
                findAllChildrenPost = findAllChildrenPost.Where(x => x.StructureEndDate.HasValue == false).ToList();
            }

            if (isCanSeeAll == false)
            {
                findAllChildrenPost = findAllChildrenPost.Where(a => a.Chart_JobIdentity.Chart_JobCategory.AllowTransfer == true).ToList();
            }

            var allSubGroupjobPositionIds = findAllChildrenPost.Select(x => x.Id).ToList();

            var employeeActived = _kscHrContext.Employees.Where(x =>
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
            x.TeamWorkId != null && allSubGroupjobPositionIds.Contains(x.JobPositionId.Value))
                .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                .ToList();

            var getReloCationRequested = _kscHrContext.Relocations
               .Where(a => allSubGroupjobPositionIds.Contains(a.DestinationJobPositionId.Value)
                           && (a.IsEnd == null || a.IsEnd.Value == false))

               .ToList();


            foreach (var subgroupJobPositionId in allSubGroupjobPositionIds)
            {
                var findjobposition = findAllChildrenPost
                    .FirstOrDefault(x => x.Id == subgroupJobPositionId);

                var ReloCationRequested = getReloCationRequested
                    .Where(a => a.DestinationJobPositionId == subgroupJobPositionId)
                    .ToList();
                var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

                var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == subgroupJobPositionId);



                var GetSumrequest = ReloCationRequested.Count();
                var GetSumWorkDayrequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
                var GetSumShiftRequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

                var CapacityCount = 0;
                var WorkingDayCount = 0;
                var WorkingShiftCount = 0;
                if (findjobposition.WorkingDayCount == 0 && findjobposition.WorkingShiftCount == 0)
                {
                    CapacityCount = ((findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

                }
                else
                {
                    CapacityCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount + findjobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                    WorkingDayCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                    WorkingShiftCount = ((findjobposition.WorkingShiftCount + findjobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;
                }

                if (CapacityCount > 0)
                {
                    jobpositionIds.Add(findjobposition);
                }
            }

            return jobpositionIds;

        }



        /// <summary>
        /// بدست اوردن پست و پست های زیر مجموعه یک پست
        /// بدست اوردن پست های زیر مجموعه یک پست
        /// پست هایی که ظرفیت دارند را نمایش میدهد
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Chart_JobPosition> GetAllSubGroupJobPostionFromJobPositionId(int jobPositionId,
            bool checkStructureEnd = true
            , bool checkEndDate = false
            , bool checkIsActive = true
            , bool checkIdendtity = true
            )
        {
            var jobpositionIds = new List<Chart_JobPosition>();
            var jobposition = GetChart_JobPositions()
                 .Where(x => x.Id == jobPositionId)
                 .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception("پست غیر فعال است");
            }
            var findAllChildrenPost = GetChart_JobPositions().AsNoTracking()
                      .Where(x => x.NewCodeRelation.Contains(jobposition.MisJobPositionCode)
                      )
                      .WhereIf(checkStructureEnd==true,x=> x.StructureEndDate.HasValue == false)
                      .WhereIf(checkIsActive == true,x=> x.IsActive == true)
                      .WhereIf(checkIdendtity == true,x=> x.JobIdentityId.HasValue)
                      .WhereIf(checkEndDate==true,x=>x.EndDate.HasValue==false)
                      .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryDefination)

                      .ToList();



            return findAllChildrenPost;

        }


        /// <summary>
        /// بدست اوردن پست های زیر مجموعه یک پست
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<JobPositionDetailModel> GetDetailedSubGroupJobPostion(int jobPositionId, bool isCanSeeAll = false)
        {
            var jobpositionIds = new List<JobPositionDetailModel>();

            var jobpositionQuery = GetChart_JobPositions().AsNoTracking().Include(x => x.Chart_Structure);

            var jobposition = jobpositionQuery
                 .Where(x => x.Id == jobPositionId)
                 .Include(a => a.Chart_Structure)
                 .Include(a => a.Parent)
                 .ThenInclude(a => a.Chart_Structure)
                 .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception("پست غیر فعال است");
            }
            var DefinitionList = new List<int>() { 1,2,};



            var findAllChildrenPost = jobpositionQuery
                      .Where(x => x.NewCodeRelation.Contains(jobposition.MisJobPositionCode)
                      && x.Id != jobposition.Id && x.IsActive == true && x.JobIdentityId.HasValue
                  && x.StructureEndDate.HasValue == false
                      )
                      .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryDefination)
                       .Include(a => a.Chart_Structure)

                       .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryEducations)
                      .ThenInclude(a => a.EducationCategory)
                      .ToList();

            var AllCodeRelation = jobposition.NewCodeRelation.Split(',');
            var FindAsistance = new Chart_JobPosition();
            if (jobposition.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id)
            {
                FindAsistance = jobposition;
            }
            else
            {
                FindAsistance = jobpositionQuery
                    .Where(x => (AllCodeRelation.Contains(x.MisJobPositionCode)) &&
                    x.Id != jobposition.Id &&
                    x.IsActive == true &&
                    x.JobIdentityId.HasValue &&
                    x.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id)
                    .Include(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory)
                    .ThenInclude(a => a.Chart_JobCategoryDefination)
                     .Include(a => a.Chart_Structure)
                    .FirstOrDefault();
            }



            if (isCanSeeAll == false)
            {
                findAllChildrenPost = findAllChildrenPost.Where(a => a.Chart_JobIdentity.Chart_JobCategory.AllowTransfer==true).ToList();
            }

            var allSubGroupjobPositionIds = findAllChildrenPost.Select(x => x.Id).ToList();

            var employeeActived = _kscHrContext.Employees.Where(x =>
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
            x.TeamWorkId != null && allSubGroupjobPositionIds.Contains(x.JobPositionId.Value))
                .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                .ToList();

            var getReloCationRequested = _kscHrContext.Relocations
               .Where(a => allSubGroupjobPositionIds.Contains(a.DestinationJobPositionId.Value)
                           && (a.IsEnd == null || a.IsEnd.Value == false))

               .ToList();


            foreach (var subgroupJobPositionId in allSubGroupjobPositionIds)
            {
                var findjobposition = findAllChildrenPost
                    .FirstOrDefault(x => x.Id == subgroupJobPositionId);

                var ReloCationRequested = getReloCationRequested
                    .Where(a => a.DestinationJobPositionId == subgroupJobPositionId)
                    .ToList();
                var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

                var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == subgroupJobPositionId).Count();
                var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == subgroupJobPositionId);



                var GetSumrequest = ReloCationRequested.Count();
                var GetSumWorkDayrequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
                var GetSumShiftRequest = ReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

                var CapacityCount = 0;
                var WorkingDayCount = 0;
                var WorkingShiftCount = 0;
                if (findjobposition.WorkingDayCount == 0 && findjobposition.WorkingShiftCount == 0)
                {
                    CapacityCount = ((findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

                }
                else
                {
                    CapacityCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount + findjobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                    WorkingDayCount = ((findjobposition.WorkingDayCount + findjobposition.TemporaryCount + findjobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                    WorkingShiftCount = ((findjobposition.WorkingShiftCount + findjobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

                }
                var asistanceTitle = FindAsistance != null ? FindAsistance.Chart_Structure.MisJobPositionCode + "-" + FindAsistance.Chart_Structure.Title :
                jobposition.Parent.Chart_Structure.StructureTypeId == 1 ? "" : ""
                ;


                var item = new JobPositionDetailModel()
                {
                    JobPositionId = subgroupJobPositionId,
                    JobPositionCode = findjobposition.Title,
                    JobPositionTitle = findjobposition.MisJobPositionCode,

                    StructureId = findjobposition.StructureId,
                    StructureCode = findjobposition.Chart_Structure.MisJobPositionCode,
                    StructureTitle = findjobposition.Chart_Structure.Title,

                    JobIdentityTitle = findjobposition.Chart_JobIdentity.Title,
                    JobIdentityCode = findjobposition.Chart_JobIdentity.Code,

                    DefinitionPostTitle = findjobposition.Chart_JobIdentity.Chart_JobCategory.Title,
                    //Asistance= FindAsistance.Chart_Structure.MisJobPositionCode + "-"+ FindAsistance.Chart_Structure.Title,
                    Asistance = asistanceTitle,
                    //JobCategoryDefinationTitle = findjobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination.Title,


                    TotalCountEmployeee = totalCountemployeee,
                    WorkDayEmployee = workdayEmployee,
                    WorkShiftEmployee = workshiftEmployee,

                    SumRequestRelocation = GetSumrequest,
                    SumRequestRelocationForWorkDay = GetSumWorkDayrequest,
                    SumRequestRelocationForWorkShift = GetSumShiftRequest,

                    CapacityCount = findjobposition.CapacityCount,
                    WorkingDayCount = findjobposition.WorkingDayCount,
                    WorkingShiftCount = findjobposition.WorkingShiftCount,

                    RemainingCapacityCount = CapacityCount,
                    RemainingWorkingDayCount = WorkingDayCount,
                    RemainingWorkingShiftCount = WorkingShiftCount,
                    EducationTitle = findjobposition.Chart_JobIdentity.Chart_JobCategory
                    .Chart_JobCategoryEducations.FirstOrDefault()
                    .EducationCategory.Title,
                    JobCategoryDefinitionId = findjobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination.Id,
                };

                jobpositionIds.Add(item);

            }

            return jobpositionIds;

        }




        /// <summary>
        /// بدست اوردن جزییات کامل یک پست
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public JobPositionDetailModel GetDetailedJobPostion(int jobPositionId)
        {


            var jobpositionQuery = GetChart_JobPositions().AsNoTracking();

            var jobposition = jobpositionQuery
                 .Where(x => x.Id == jobPositionId)
                 .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryDefination)
                      .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryEducations)
                      .ThenInclude(a => a.EducationCategory)
                      .ThenInclude(x => x.Education)
                      .ThenInclude(x => x.EmployeeEducationDegrees)
                      .Include(a => a.Chart_JobPositionFields)
                      .ThenInclude(a => a.StudyField)
                       .Include(a => a.Chart_Structure)

                       .Include(a => a.Parent)
                       .ThenInclude(a => a.Chart_Structure)
                 .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception("پست غیر فعال است");
            }


            var AllCodeRelation = jobposition.NewCodeRelation.Split(',');

            var FindAsistance = new Chart_JobPosition();
            if (jobposition.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id)
            {
                FindAsistance = jobposition;
            }
            else
            {
                FindAsistance = jobpositionQuery
                    .Where(x => (AllCodeRelation.Contains(x.MisJobPositionCode)) &&
                    x.Id != jobposition.Id &&
                    x.IsActive == true &&

                    x.JobIdentityId.HasValue &&
                    x.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id
                     && x.StructureEndDate.HasValue == false
                     )
                    .Include(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory)
                    .ThenInclude(a => a.Chart_JobCategoryDefination)
                     .Include(a => a.Chart_Structure)



                    .FirstOrDefault();
            }


            var employeeActived = _kscHrContext.Employees.Where(x =>
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
            x.TeamWorkId != null && x.JobPositionId == jobPositionId)
                .Include(a => a.TeamWork)
                    .Include(a => a.WorkGroup)
                    .ThenInclude(a => a.WorkTime)
                .ToList();

            var getReloCationRequested = _kscHrContext.Relocations
               .Where(a => a.DestinationJobPositionId.Value == jobPositionId
                           && (a.IsEnd == null || a.IsEnd.Value == false))

               .ToList();



            var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);

            var workdayEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId == roozkarCode && a.JobPositionId == jobPositionId).Count();
            var workshiftEmployee = employeeActived.Where(a => a.WorkGroup.WorkTimeId != roozkarCode && a.JobPositionId == jobPositionId).Count();
            var totalCountemployeee = employeeActived.Count(a => a.JobPositionId == jobPositionId);



            var GetSumrequest = getReloCationRequested.Count();
            var GetSumWorkDayrequest = getReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId == roozkarCode);
            var GetSumShiftRequest = getReloCationRequested.Count(a => a.DestinationEmployeeWorkTimeId.HasValue && a.DestinationEmployeeWorkTimeId != roozkarCode);

            var CapacityCount = 0;
            var WorkingDayCount = 0;
            var WorkingShiftCount = 0;
            if (jobposition.WorkingDayCount == 0 && jobposition.WorkingShiftCount == 0)
            {
                CapacityCount = ((jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumrequest + totalCountemployeee)) ?? 0;

            }
            else
            {
                CapacityCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount + jobposition.WorkingShiftCount.Value) - (GetSumrequest + totalCountemployeee)) ?? 0;
                WorkingDayCount = ((jobposition.WorkingDayCount + jobposition.TemporaryCount + jobposition.ExtraCount) - (GetSumWorkDayrequest + workdayEmployee)) ?? 0;
                WorkingShiftCount = ((jobposition.WorkingShiftCount + jobposition.ExtraCount) - (GetSumShiftRequest + workshiftEmployee)) ?? 0;

            }

            var asistanceTitle = FindAsistance != null ? FindAsistance.Chart_Structure.MisJobPositionCode + "-" + FindAsistance.Chart_Structure.Title :
             jobposition.Parent.Chart_Structure.StructureTypeId == 1 ? "" : ""
             ;

            var item = new JobPositionDetailModel()
            {
                JobPositionId = jobPositionId == 0 | jobPositionId == null ? 0 : jobPositionId,
                JobPositionCode = jobposition.Title == null ? null : jobposition.Title,
                JobPositionTitle = jobposition.MisJobPositionCode == null ? null : jobposition.MisJobPositionCode,

                StructureId = jobposition.StructureId,
                StructureCode = jobposition.Chart_Structure.MisJobPositionCode,
                StructureTitle = jobposition.Chart_Structure.Title,

                JobIdentityTitle = jobposition.Chart_JobIdentity.Title,
                JobIdentityCode = jobposition.Chart_JobIdentity.Code,




                JobCategoryEducationId = jobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryEducations.FirstOrDefault() == null ? 0 : jobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryEducations.FirstOrDefault().Id,
                EducationTitle = jobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryEducations.FirstOrDefault() == null ? null : jobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryEducations.FirstOrDefault().EducationCategory.Title,
                StudyFieldTitle = jobposition.Chart_JobPositionFields.FirstOrDefault(a => a.IsActive == true) == null ? null : jobposition.Chart_JobPositionFields.FirstOrDefault(a => a.IsActive == true)?.StudyField.Title,
                //JobPositionFieldId = jobposition.Chart_JobPositionFields.Where(x => x.JobPositionId == jobPositionId).FirstOrDefault(a=>a.IsActive==true)== null ? 0 : jobposition.Chart_JobPositionFields.Where(x => x.JobPositionId == jobPositionId).FirstOrDefault(a => a.IsActive == true).Id,

                JobPositionFieldId = jobposition.Chart_JobPositionFields.Where(x => x.IsActive == true).FirstOrDefault(x => x.JobPositionId == jobPositionId) == null ? 0 : jobposition.Chart_JobPositionFields.Where(x => x.IsActive == true).FirstOrDefault(x => x.JobPositionId == jobPositionId).Id,

                DefinitionPostTitle = jobposition.Chart_JobIdentity.Chart_JobCategory.Title,
                Asistance = asistanceTitle,

                TotalCountEmployeee = totalCountemployeee,
                WorkDayEmployee = workdayEmployee,
                WorkShiftEmployee = workshiftEmployee,

                SumRequestRelocation = GetSumrequest,
                SumRequestRelocationForWorkDay = GetSumWorkDayrequest,
                SumRequestRelocationForWorkShift = GetSumShiftRequest,

                CapacityCount = jobposition.CapacityCount,
                WorkingDayCount = jobposition.WorkingDayCount,
                WorkingShiftCount = jobposition.WorkingShiftCount,

                RemainingCapacityCount = CapacityCount,
                RemainingWorkingDayCount = WorkingDayCount,
                RemainingWorkingShiftCount = WorkingShiftCount,
                EmployeeEducationDegreeId = jobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryEducations.FirstOrDefault().EducationCategory.Education.FirstOrDefault().EmployeeEducationDegrees.FirstOrDefault().Id,
                JobCategoryDefinitionId = jobposition.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination.Id,
                jobCategoryId = jobposition.Chart_JobIdentity.Chart_JobCategory.Id
            };

            return item;

        }

        public List<Tuple<Chart_JobPosition, Employee>> GetEmployeeParent(List<int> jobpositionIds)
        {
            var chartQuery = _kscHrContext.Chart_JobPosition.Where(a => a.IsActive).AsQueryable().AsNoTracking();
            var jobPositionFilterList = jobpositionIds.ToList();
            var charts = new List<Chart_JobPosition>();
            var getCurrentPosts = chartQuery.Where(a => jobPositionFilterList.Contains(a.Id))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)
                .ToList();
            charts.AddRange(getCurrentPosts);
            var JobPositionCodeParentList = getCurrentPosts.SelectMany(a => a.NewCodeRelation.Split(',')).Distinct().ToList();
            var ParentList = chartQuery.Where(a => JobPositionCodeParentList.Contains(a.MisJobPositionCode))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination).ToList();
            charts.AddRange(ParentList);

            var employees = _kscHrContext.Employees.Where(a => a.PaymentStatusId != 7).ToList();
            var employeeJobPositions1 = _kscHrContext.EmployeeJobPositions.Include(x => x.Employee).Where(a => a.IsActive && a.EndDate.HasValue == false && a.Employee.PaymentStatusId != 7).OrderByDescending(x => x.Id).ToList();



            var results = new List<Tuple<Chart_JobPosition, Employee>>();
            //int[] statuses = new[] { 2, 3, 12, 14 };
            int? parentId = null;

            try
            {
                foreach (int id in jobpositionIds)
                {
                    var chart = charts.FirstOrDefault(x => x.Id == id);
                    if (chart.IsActive == false)
                    {
                        throw new Exception($"پست {chart.MisJobPositionCode} غیر فعال است");
                    }


                    var allparent = chart.NewCodeRelation.Split(',').ToList();
                    var AllParent = charts.Where(x => allparent.Contains(x.MisJobPositionCode)
                    && x.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                        .OrderByDescending(a => a.LevelNumber)
                        .ToList();

                    var findFirstTM = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                    .Chart_JobCategoryDefination.Title == "TM").FirstOrDefault();

                    var emps = employees.Where(x => x.JobPositionId == findFirstTM.Id).FirstOrDefault();

                    var item = new Tuple<Chart_JobPosition, Employee>(findFirstTM, emps);


                    results.Add(item);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;
        }





        public Chart_JobPosition GetChart_JobPositionByMisCode(string misCode)
        {
            return _kscHrContext.Chart_JobPosition.FirstOrDefault(a => a.MisJobPositionCode == misCode);
        }
        public int? GetJobPositionIdByMisCode(string misCode)
        {
            return _kscHrContext.Chart_JobPosition.Where(a => a.MisJobPositionCode == misCode).Select(x => x.Id).FirstOrDefault();
        }
        public int GetJobPositionCode()
        {
            var maxCode = _kscHrContext.Chart_JobPosition.Max(x => x.Code);
            var intMaxCode = Convert.ToInt32(maxCode) + 1;
            return intMaxCode;
        }
        public int GetJobPositionOrderNo()
        {
            var maxCode = _kscHrContext.Chart_JobPosition.Max(x => x.OrderNo);
            var intMaxCode = Convert.ToInt32(maxCode) + 1;
            return intMaxCode;
        }
        public IQueryable<Chart_JobPosition> GetChart_JobPositionById(int id)
        {
            return _kscHrContext.Chart_JobPosition.Where(a => a.Id == id).AsQueryable();
        }
        /// <summary>
        /// بررسی ظرفیت یک پست بر اساس وضعیت کاربر
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="JobPositionId"></param>
        /// <returns></returns>
        public Tuple<bool, string> CheckCountRemaining(int EmployeeId, int JobPositionId)
        {
            var employee = _kscHrContext.Employees.Include(x => x.WorkGroup).ThenInclude(x => x.WorkTime).AsQueryable().AsNoTracking()
                 .Where(x => x.Id == EmployeeId)
                 .FirstOrDefaultAsync()
                 .GetAwaiter().GetResult();

            var jobposition = GetChart_JobPositions()
                    .Where(x => x.Id == JobPositionId)
                    .Include(x => x.EmployeeJobPositions)
                    .ThenInclude(a => a.Employee)
                    .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception($"پست {jobposition.MisJobPositionCode} غیر فعال است");
            }

            var roozkarCode = ((int)WorkTimeCodeEnums.RoozKar);
            var countInThisJob = jobposition.EmployeeJobPositions.Count(a => a.IsActive); // تعداد افرادی که به این شغل متصل هستند

            if (employee.WorkGroup != null)
            {
                if (roozkarCode == employee.WorkGroup.WorkTimeId) // روزکار
                {
                    var RozkarRemmingCount = (jobposition.ExtraCount + jobposition.WorkingDayCount) - jobposition.EmployeeJobPositions.Where(a => a.Employee.WorkGroupId == employee.WorkGroupId).Count();
                    if (RozkarRemmingCount <= 0)
                    {
                        return new Tuple<bool, string>(false, "ظرفیت پست انتخاب شده پر می باشد");

                    }
                }
                else // غیر از روزکار
                {
                    var ShiftRemmingCount = (jobposition.ExtraCount + jobposition.WorkingShiftCount) - jobposition.EmployeeJobPositions.Where(a => a.Employee.WorkGroupId == employee.WorkGroupId).Count();
                    if (ShiftRemmingCount <= 0)
                    {
                        return new Tuple<bool, string>(false, "ظرفیت پست انتخاب شده پر می باشد");
                    }
                }
            }
            else
            {
                return new Tuple<bool, string>(false, "نوع کارکرد بایستی مشخص باشد");
            }
            return new Tuple<bool, string>(true, "پست مجاز می باشد");


        }
        public IQueryable<Chart_JobPosition> GetHobCategoryDefinationIdByJobPositionId(int id)
        {
            return _kscHrContext.Chart_JobPosition

                .Where(a => a.Id == id)
                .Include(a => a.Chart_JobIdentity)
                .ThenInclude(x => x.Chart_JobCategory)
                .AsNoTracking();
        }

        public IQueryable<Chart_JobPosition> GetForDiagramIncluded()
        {
            return _kscHrContext.Chart_JobPosition
                .Include(a => a.Chart_JobPositions)
                .Include(a => a.Chart_JobPositions)

                .AsNoTracking();
        }


        public IQueryable<Chart_JobPosition> GetByIncluded()
        {
            return _kscHrContext.Chart_JobPosition

                .Include(a => a.Chart_JobIdentity)
                .ThenInclude(x => x.Chart_JobCategory)
                .AsNoTracking();
        }
        public IQueryable<Chart_JobPosition> GetChart_JobPositionsByStatusCategory(int StatusCategoryId)
        {
            return _kscHrContext.Chart_JobPosition.Include(a => a.EmployeeJobPositions)
                .ThenInclude(a => a.Employee).Include(a => a.Chart_JobPositionStatu)
                .Where(a => a.IsActive && a.Chart_JobPositionStatu.JobPostionStatusCategoryId == StatusCategoryId)
                .AsNoTracking();
        }

        public IQueryable<Chart_JobPosition> GetjobCategoryDefinationIdStatusIdByJobPositionId(int id)
        {
            return _kscHrContext.Chart_JobPosition
                .Where(a => a.Id == id)
                .Include(a => a.Chart_JobIdentity)
                .ThenInclude(x => x.Chart_JobCategory)
                .Include(x => x.Chart_JobPositionStatu)
                .AsNoTracking();
        }

        public IQueryable<Chart_JobPosition> GetChart_JobPositionIncludedById(int id)
        {
            return _kscHrContext.Chart_JobPosition

                .Where(a => a.Id == id)
                .Include(a => a.Chart_JobPositionStatu)
                .Include(a => a.JobPositionNature)
                .Include(a => a.Chart_JobIdentity)
                .Include(a => a.Chart_Structure)
                .Include(a => a.Chart_RewardSpecific)
                .AsNoTracking();
        }
        public IQueryable<Chart_JobPosition> GetChart_JobPositions()
        {
            var result = _kscHrContext.Chart_JobPosition.AsQueryable().AsNoTracking();
            return result;
        }
        public IQueryable<Chart_JobPosition> GetChart_JobPositionsIncludedParent()
        {
            var result = _kscHrContext.Chart_JobPosition.Include(a => a.Parent)
                .Include(x => x.Chart_Structure)
                .Include(x => x.JobPositionNatureSubGroup)
                .AsQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<Chart_JobPosition> GetChart_JobPositionsIncluded()
        {
            var result = _kscHrContext.Chart_JobPosition.Include(a => a.Parent)
                .Include(a => a.Chart_Structure)
                .ThenInclude(a => a.Chart_StructureType)
                .Include(a => a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_JobPositions)
                .Include(a => a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_Structure)
                .ThenInclude(a => a.Chart_StructureType)
                .AsQueryable().AsNoTracking();

            return result;
        }

        public void UpdateRange(List<Chart_JobPosition> jobPositions)
        {
            _kscHrContext.Chart_JobPosition.UpdateRange(jobPositions);
        }

        /// <summary>
        /// مشخص کردن مقام بالا سر پست ها
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<KscResult<Chart_JobPosition>> GetSuperiorJobPosition(int[] ids)
        {
            var chartQuery = _kscHrContext.Chart_JobPosition.Where(a => a.IsActive).AsQueryable().AsNoTracking();
            var jobPositionFilterList = ids.ToList();
            var charts = new List<Chart_JobPosition>();
            var getCurrentPosts = chartQuery.Where(a => jobPositionFilterList.Contains(a.Id))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)
                .ToList();
            charts.AddRange(getCurrentPosts);
            var JobPositionCodeParentList = getCurrentPosts.SelectMany(a => a.NewCodeRelation.Split(',')).Distinct().ToList();
            var ParentList = chartQuery.Where(a => JobPositionCodeParentList.Contains(a.MisJobPositionCode))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination).ToList();
            charts.AddRange(ParentList);

            var employees = _kscHrContext.Employees.Where(a => a.PaymentStatusId != 7).ToList();
            var employeeJobPositions1 = _kscHrContext.EmployeeJobPositions.Include(x => x.Employee)
                .Where(a => a.IsActive && a.EndDate.HasValue == false && a.Employee.PaymentStatusId != 7)
                .OrderByDescending(x => x.Id).ToList();



            var results = new List<KscResult<Chart_JobPosition>>();
            //int[] statuses = new[] { 2, 3, 12, 14 };
            int? parentId = null;

            try
            {
                foreach (int id in ids)
                {
                    var chart = charts.FirstOrDefault(x => x.Id == id);
                    var CurrentJobPosition = new KscResult<Chart_JobPosition>();
                    CurrentJobPosition.Data = chart;
                    CurrentJobPosition.Errors = new List<KscError>();

                    var allparent = chart.NewCodeRelation.Split(',').ToList();
                    var AllParent = charts.Where(x => allparent.Contains(x.MisJobPositionCode)
                    && x.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                        .OrderByDescending(a => a.LevelNumber)
                        .ToList();

                    foreach (var firtParent in AllParent)
                    {
                        var findedInList = results.Any(a => a.Data.Parent.Chart_JobIdentity.Chart_JobCategory
                      .Chart_JobCategoryDefination.Title == firtParent.Chart_JobIdentity.Chart_JobCategory
                      .Chart_JobCategoryDefination.Title);
                        if (findedInList)
                            continue;

                        var emps = employees.Where(x => x.JobPositionId == firtParent.Id).ToList();

                        if (!emps.Any())
                            continue;

                        CurrentJobPosition.Data.Parent = firtParent;

                        var employeeids = emps.Select(x => x.Id).ToList();

                        var empjobposition = employeeJobPositions1
                            .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                        CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;

                    }
                    if (results.Any() == false)
                    {
                        CurrentJobPosition.AddError("", "کد شغلی بالاسری وجود ندارد");
                    }














                    //var findFirstMA = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                    //.Chart_JobCategoryDefination.Title == "MA").FirstOrDefault();

                    //var findFirstTM = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                    //.Chart_JobCategoryDefination.Title == "TM").FirstOrDefault();

                    //if (findFirstMA != null)
                    //{
                    //    CurrentJobPosition.Data.Parent = findFirstMA;
                    //    var emps = employees.Where(x => x.JobPositionId == findFirstMA.Id).ToList();
                    //    var employeeids = emps.Select(x => x.Id).ToList();

                    //    var empjobposition = employeeJobPositions1
                    //        .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                    //    CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;
                    //}
                    //else if (findFirstTM != null)
                    //{
                    //    CurrentJobPosition.Data.Parent = findFirstTM;
                    //    var emps = employees.Where(x => x.JobPositionId == findFirstTM.Id).ToList();
                    //    var employeeids = emps.Select(x => x.Id).ToList();

                    //    var empjobposition = employeeJobPositions1
                    //        .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                    //    CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;
                    //}
                    //else
                    //{
                    //    CurrentJobPosition.AddError("", "کد شغلی بالاسری وجود ندارد");

                    //}

                    //if (chart.ParentId == null)
                    //{
                    //    CurrentJobPosition.AddError("", "کد شغلی بالاسری وجود ندارد");

                    //}
                    //else
                    //{
                    //    Chart_JobPosition parentmodel = null;
                    //    while (parentmodel == null)
                    //    {
                    //        var parent = charts.FirstOrDefault(x => x.Id == parentId);
                    //        if (parent == null)
                    //        {
                    //            continue;
                    //        }
                    //        //if (parent.MisJobPositionCode == "2101013500170")
                    //        //{
                    //        //    // parentId = parent.ParentId;
                    //        //}
                    //        var emps = employees.Where(x => x.JobPositionId == parent.Id).ToList();
                    //        if (!emps.Any())
                    //        {
                    //            parentId = parent.ParentId;
                    //        }
                    //        else
                    //        {
                    //            var defination = parent.Chart_JobIdentity?.Chart_JobCategory?.Chart_JobCategoryDefination;
                    //            if (defination != null)
                    //            {
                    //                //مدیر
                    //                if (defination.Title == "TM" && parent.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                    //                {
                    //                    CurrentJobPosition.Data.Parent = parentmodel = parent;

                    //                    if (!emps.Any())
                    //                    {
                    //                        parentId = parent.ParentId;
                    //                        //CurrentJobPosition.AddError("", "افرادی در این پست وجود ندارد");
                    //                        //return Superior;
                    //                    }
                    //                }
                    //                else if (defination.Title == "MA")
                    //                {
                    //                    if (parent.Chart_JobPositionStatu.JobPostionStatusCategoryId != 1 || !emps.Any())
                    //                    {
                    //                        parentId = parent.ParentId;
                    //                    }
                    //                    else
                    //                    {
                    //                        CurrentJobPosition.Data.Parent = parentmodel = parent;
                    //                        var employeeids = emps.Select(x => x.Id).ToList();
                    //                        var empjobposition = employeeJobPositions1
                    //                            .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                    //                        CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    parentId = parent.ParentId;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    //parentId = parent.ParentId.Value;

                    //}
                    results.Add(CurrentJobPosition);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;


        }



        public List<ChartSuperiorPositionWithEmployees> GetSuperiorJobPositionForCartabl(List<int> ids)
        {
            var chartQuery = _kscHrContext.Chart_JobPosition.Where(a => a.IsActive).AsQueryable().AsNoTracking();
            var jobPositionFilterList = ids.ToList();
            var charts = new List<Chart_JobPosition>();
            var getCurrentPosts = chartQuery.Where(a => jobPositionFilterList.Contains(a.Id))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)
                .ToList();
            charts.AddRange(getCurrentPosts);
            var JobPositionCodeParentList = getCurrentPosts.SelectMany(a => a.NewCodeRelation.Split(',')).Distinct().ToList();
            var ParentList = chartQuery.Where(a => JobPositionCodeParentList.Contains(a.MisJobPositionCode))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination).ToList();
            charts.AddRange(ParentList);

            var employees = _kscHrContext.Employees.Where(a => a.PaymentStatusId != 7).ToList();
            var employeeJobPositions1 = _kscHrContext.EmployeeJobPositions.Include(x => x.Employee)
                .Where(a => a.IsActive && a.EndDate.HasValue == false && a.Employee.PaymentStatusId != 7)
                .OrderByDescending(x => x.Id).ToList();



            var results = new List<ChartSuperiorPositionWithEmployees>();
            //int[] statuses = new[] { 2, 3, 12, 14 };
            int? parentId = null;

            try
            {
                foreach (int id in ids)
                {
                    var item = new ChartSuperiorPositionWithEmployees();

                    var chart = charts.FirstOrDefault(x => x.Id == id);

                    item.JobId = chart.Id;
                    item.MisJoibPositionCode = chart.MisJobPositionCode;
                    item.Titlte = chart.Title;
                    item.Superiors = new List<ChartSuperiorPositionWithEmployees>();
                    var allparent = chart.NewCodeRelation.Split(',').ToList();
                    var AllParent = charts.Where(x => allparent.Contains(x.MisJobPositionCode)
                    && x.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                        .OrderByDescending(a => a.LevelNumber)
                        .ToList();

                    foreach (var firtParent in AllParent)
                    {
                        var parent = new ChartSuperiorPositionWithEmployees();

                        var findedInList = item.Superiors.Any(a => a.Chart_JobCategoryDefinationTitle == firtParent.Chart_JobIdentity.Chart_JobCategory
                      .Chart_JobCategoryDefination.Title);

                        if (findedInList)
                            continue;

                        var emps = employees.Where(x => x.JobPositionId == firtParent.Id).FirstOrDefault();

                        if (emps == null)
                            continue;
                        parent.JobId = firtParent.Id;
                        parent.MisJoibPositionCode = firtParent.MisJobPositionCode;
                        parent.Titlte = firtParent.Title;
                        parent.Chart_JobCategoryDefinationTitle = firtParent.Chart_JobIdentity.Chart_JobCategory
                      .Chart_JobCategoryDefination.Title;



                        parent.EmployeeNumber = emps.EmployeeNumber;
                        parent.EmployeeId = emps.Id;
                        parent.EmployeeTitle = emps.Name + " " + emps.Family;

                        item.Superiors.Add(parent);
                    }

                    results.Add(item);













                    //var findFirstMA = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                    //.Chart_JobCategoryDefination.Title == "MA").FirstOrDefault();

                    //var findFirstTM = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                    //.Chart_JobCategoryDefination.Title == "TM").FirstOrDefault();

                    //if (findFirstMA != null)
                    //{
                    //    CurrentJobPosition.Data.Parent = findFirstMA;
                    //    var emps = employees.Where(x => x.JobPositionId == findFirstMA.Id).ToList();
                    //    var employeeids = emps.Select(x => x.Id).ToList();

                    //    var empjobposition = employeeJobPositions1
                    //        .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                    //    CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;
                    //}
                    //else if (findFirstTM != null)
                    //{
                    //    CurrentJobPosition.Data.Parent = findFirstTM;
                    //    var emps = employees.Where(x => x.JobPositionId == findFirstTM.Id).ToList();
                    //    var employeeids = emps.Select(x => x.Id).ToList();

                    //    var empjobposition = employeeJobPositions1
                    //        .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                    //    CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;
                    //}
                    //else
                    //{
                    //    CurrentJobPosition.AddError("", "کد شغلی بالاسری وجود ندارد");

                    //}

                    //if (chart.ParentId == null)
                    //{
                    //    CurrentJobPosition.AddError("", "کد شغلی بالاسری وجود ندارد");

                    //}
                    //else
                    //{
                    //    Chart_JobPosition parentmodel = null;
                    //    while (parentmodel == null)
                    //    {
                    //        var parent = charts.FirstOrDefault(x => x.Id == parentId);
                    //        if (parent == null)
                    //        {
                    //            continue;
                    //        }
                    //        //if (parent.MisJobPositionCode == "2101013500170")
                    //        //{
                    //        //    // parentId = parent.ParentId;
                    //        //}
                    //        var emps = employees.Where(x => x.JobPositionId == parent.Id).ToList();
                    //        if (!emps.Any())
                    //        {
                    //            parentId = parent.ParentId;
                    //        }
                    //        else
                    //        {
                    //            var defination = parent.Chart_JobIdentity?.Chart_JobCategory?.Chart_JobCategoryDefination;
                    //            if (defination != null)
                    //            {
                    //                //مدیر
                    //                if (defination.Title == "TM" && parent.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                    //                {
                    //                    CurrentJobPosition.Data.Parent = parentmodel = parent;

                    //                    if (!emps.Any())
                    //                    {
                    //                        parentId = parent.ParentId;
                    //                        //CurrentJobPosition.AddError("", "افرادی در این پست وجود ندارد");
                    //                        //return Superior;
                    //                    }
                    //                }
                    //                else if (defination.Title == "MA")
                    //                {
                    //                    if (parent.Chart_JobPositionStatu.JobPostionStatusCategoryId != 1 || !emps.Any())
                    //                    {
                    //                        parentId = parent.ParentId;
                    //                    }
                    //                    else
                    //                    {
                    //                        CurrentJobPosition.Data.Parent = parentmodel = parent;
                    //                        var employeeids = emps.Select(x => x.Id).ToList();
                    //                        var empjobposition = employeeJobPositions1
                    //                            .Where(x => employeeids.Contains(x.EmployeeId)).ToList();

                    //                        CurrentJobPosition.Data.EmployeeJobPositions = empjobposition;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    parentId = parent.ParentId;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    //parentId = parent.ParentId.Value;

                    //}
                    //results.Add(CurrentJobPosition);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;


        }






        public List<Tuple<Chart_JobPosition, Employee>> GetListSuperiorJobPosition(int id)
        {
            var chartQuery = _kscHrContext.Chart_JobPosition.Where(a => a.IsActive).AsQueryable().AsNoTracking();

            var charts = new List<Chart_JobPosition>();
            var getCurrentPosts = chartQuery.Where(a => a.Id == id)
                .Include(a => a.Parent)
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)
                .FirstOrDefault();
            if (getCurrentPosts.IsActive == false)
            {
                throw new Exception($"پست {getCurrentPosts.MisJobPositionCode} غیر فعال است");
            }
            if (getCurrentPosts.Parent.JobPoisitionStatusId == 10)
            {
                return new List<Tuple<Chart_JobPosition, Employee>>();
            }
            charts.Add(getCurrentPosts);
            var JobPositionCodeParentList = getCurrentPosts.NewCodeRelation.Split(',');
            var ParentList = chartQuery.Where(a => JobPositionCodeParentList.Contains(a.MisJobPositionCode))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination).ToList();
            charts.AddRange(ParentList);

            var employees = _kscHrContext.Employees.Where(a => a.PaymentStatusId != 7).ToList();

            var results = new List<Tuple<Chart_JobPosition, Employee>>();

            int? parentId = null;

            try
            {
                var chart = charts.FirstOrDefault(x => x.Id == id);
                if (chart.IsActive == false)
                {
                    throw new Exception($"پست {chart.MisJobPositionCode} غیر فعال است");
                }


                var allparent = chart.NewCodeRelation.Split(',').ToList();
                var AllParent = charts.Where(x => allparent.Contains(x.MisJobPositionCode)
                && x.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                    .OrderByDescending(a => a.LevelNumber)
                    .ToList();
                foreach (var firtParent in AllParent)
                {
                    var findedInList = results.Any(a => a.Item1.Chart_JobIdentity.Chart_JobCategory
                  .Chart_JobCategoryDefination.Title == firtParent.Chart_JobIdentity.Chart_JobCategory
                  .Chart_JobCategoryDefination.Title);
                    if (findedInList)
                        continue;

                    var emps = employees.Where(x => x.JobPositionId == firtParent.Id).FirstOrDefault();

                    if (emps == null)
                        continue;

                    var item = new Tuple<Chart_JobPosition, Employee>(firtParent, emps);
                    results.Add(item);



                }
                if (results.Any() == false)
                {
                    throw new Exception("مافوق  سازمانی پست مورد نظر پیدا نشد");
                }
                //var findFirstMA = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                //.Chart_JobCategoryDefination.Title == "MA").FirstOrDefault();

                //var findFirstTM = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                //.Chart_JobCategoryDefination.Title == "TM").FirstOrDefault();
                //if(findFirstMA==null && findFirstTM == null)
                //{
                //    throw new Exception("مافوق  سازمانی پست مورد نظر پیدا نشد");
                //}
                //if (findFirstMA != null)
                //{

                //    var emps = employees.Where(x => x.JobPositionId == findFirstMA.Id).First();

                //    var item = new Tuple<Chart_JobPosition, Employee>(findFirstMA, emps);
                //    results.Add(item);
                //}


                //if (findFirstTM != null)
                //{
                //    var emps = employees.Where(x => x.JobPositionId == findFirstTM.Id).First();

                //    var item = new Tuple<Chart_JobPosition, Employee>(findFirstTM, emps);
                //    results.Add(item);
                //}




            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;


        }



        public List<Tuple<Chart_JobPosition, Employee>> GetAllSuperiorJobPosition(int id)
        {
            var chartQuery = _kscHrContext.Chart_JobPosition.Where(a => a.IsActive).AsQueryable().AsNoTracking();

            var charts = new List<Chart_JobPosition>();
            var getCurrentPosts = chartQuery.Where(a => a.Id == id)
                .Include(a => a.Parent)
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)
                .FirstOrDefault();
            if (getCurrentPosts.IsActive == false)
            {
                throw new Exception($"پست {getCurrentPosts.MisJobPositionCode} غیر فعال است");
            }
            if (getCurrentPosts.Parent.JobPoisitionStatusId == 10)
            {
                return new List<Tuple<Chart_JobPosition, Employee>>();
            }
            charts.Add(getCurrentPosts);
            var JobPositionCodeParentList = getCurrentPosts.NewCodeRelation.Split(',');
            var ParentList = chartQuery.Where(a => JobPositionCodeParentList.Contains(a.MisJobPositionCode))
                .Include(a => a.Chart_JobPositionStatu).Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination).ToList();
            charts.AddRange(ParentList);

            var employees = _kscHrContext.Employees.Where(a => a.PaymentStatusId != 7).ToList();

            var results = new List<Tuple<Chart_JobPosition, Employee>>();

            int? parentId = null;

            try
            {
                var chart = charts.FirstOrDefault(x => x.Id == id);
                if (chart.IsActive == false)
                {
                    throw new Exception($"پست {chart.MisJobPositionCode} غیر فعال است");
                }


                var allparent = chart.NewCodeRelation.Split(',').ToList();
                var AllParent = charts.Where(x => allparent.Contains(x.MisJobPositionCode)
                && x.Chart_JobPositionStatu.JobPostionStatusCategoryId == 1)
                    .OrderByDescending(a => a.LevelNumber)
                    .ToList();
                foreach (var firtParent in AllParent)
                {
                    var findedInList = results.Any(a => a.Item1.MisJobPositionCode == firtParent.MisJobPositionCode);
                    if (findedInList)
                        continue;

                    var emps = employees.Where(x => x.JobPositionId == firtParent.Id).FirstOrDefault();

                    if (emps == null)
                        continue;

                    var item = new Tuple<Chart_JobPosition, Employee>(firtParent, emps);
                    results.Add(item);



                }
                if (results.Any() == false)
                {
                    throw new Exception("مافوق  سازمانی پست مورد نظر پیدا نشد");
                }
                //var findFirstMA = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                //.Chart_JobCategoryDefination.Title == "MA").FirstOrDefault();

                //var findFirstTM = AllParent.Where(a => a.Chart_JobIdentity.Chart_JobCategory
                //.Chart_JobCategoryDefination.Title == "TM").FirstOrDefault();
                //if(findFirstMA==null && findFirstTM == null)
                //{
                //    throw new Exception("مافوق  سازمانی پست مورد نظر پیدا نشد");
                //}
                //if (findFirstMA != null)
                //{

                //    var emps = employees.Where(x => x.JobPositionId == findFirstMA.Id).First();

                //    var item = new Tuple<Chart_JobPosition, Employee>(findFirstMA, emps);
                //    results.Add(item);
                //}


                //if (findFirstTM != null)
                //{
                //    var emps = employees.Where(x => x.JobPositionId == findFirstTM.Id).First();

                //    var item = new Tuple<Chart_JobPosition, Employee>(findFirstTM, emps);
                //    results.Add(item);
                //}




            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;


        }



        public List<Chart_JobPosition> GetChart_JobPositionsforInterdictCalculation(List<int?> JobPositionsId)
        {
            var result = _kscHrContext.Chart_JobPosition.AsQueryable().AsNoTracking()
                .Where(a => JobPositionsId.Contains(a.Id) && a.IsActive).Include(x => x.Chart_JobIdentity).ThenInclude(x => x.Chart_JobCategory)
                 .ThenInclude(x => x.Chart_JobCategoryDefination)
                 .ThenInclude(x => x.InterdictWeathers)
                 .Include(a => a.Chart_JobPositionScores)
                    .ThenInclude(a => a.Chart_JobPositionScoreType)
                    .Include(a => a.Chart_Structure)
                    .Include(a => a.Chart_JobIdentity)
                    .ThenInclude(a => a.Chart_JobCategory)
                 .ToList();
            ;
            return result;
        }

        public Chart_JobPosition GetChart_JobPositionParentByMisCode(string ParentMisCode)
        {
            return _kscHrContext.Chart_JobPosition.FirstOrDefault(a => a.ParentMisCode == ParentMisCode);
        }

        public Chart_JobPosition GetOneByRelations(int id)
        {
            return _kscHrContext
                .Chart_JobPosition
                .Include(x => x.Chart_JobPositionTeamWorks)
                .ThenInclude(x => x.TeamWork)
                .Include(x => x.Chart_JobPositionTeamWorks)
                .ThenInclude(x => x.UserDefinitionSecurityPriorityStatus)
                .FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// کارکنان فعال
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public bool IsActiveEmployee(int employeeId)
        {
            var jobposition = _kscHrContext.Chart_JobPosition;

            List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8}; //غیرفعال

            var query =
                        from c in _kscHrContext.Employees
                        join t in _kscHrContext.Chart_JobPosition
                        on c.JobPositionId equals t.Id
                        where !paymentStatusIds.Contains(c.PaymentStatusId ?? 0)

                        select new
                        {
                            t.JobPoisitionStatusId,
                            c.JobPositionId,
                            c.EmployeeNumber,
                            c.Id//employeeId
                        };
            var IsActive = query.Any(x => x.Id == employeeId);
            return IsActive;
        }

        public string FindCommonPost(int source, int destination)
        {
            var sourcePath = GetAllSuperiorJobPosition(source).Select(a => a.Item1).ToList();
            var destinationPath = GetAllSuperiorJobPosition(destination).Select(a => a.Item1).ToList();

            // برعکس کنیم که از ریشه شروع کنه
            sourcePath.Reverse();
            destinationPath.Reverse();

            var common = "";
            int? length = Math.Min(sourcePath.Count, destinationPath.Count);

            for (int i = 0; i < length; i++)
            {
                if (sourcePath[i].Id == destinationPath[i].Id)
                    common = sourcePath[i].MisJobPositionCode;
                else
                    break;
            }

            return common;
        }



    }
}
