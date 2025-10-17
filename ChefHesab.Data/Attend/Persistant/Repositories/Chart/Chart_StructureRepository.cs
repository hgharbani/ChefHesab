using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Chart;
using Ksc.HR.Share.Model.JobPositionStatus;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.WorkShift;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ksc.HR.Data.Persistant.Repositories.Rule.IncreaseSalaryHeaderRepository;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class Chart_StructureRepository : EfRepository<Chart_Structure, int>, IChart_StructureRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_StructureRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_Structure> GetByMisStructureCode(string misStructureCode)
        {
            return _kscHrContext.Chart_Structure.Where(x=>x.MisJobPositionCode== misStructureCode).AsQueryable();
        }

        public int GetStructureCode()
        {
            var maxCode = _kscHrContext.Chart_Structure.Max(x => x.Code);
            var intMaxCode = Convert.ToInt32(maxCode) + 1;
            return intMaxCode;
        }
        public IQueryable<Chart_Structure> GetAllInclude()
        {
            return _kscHrContext.Chart_Structure.Include(a=>a.Chart_StructureType)
                .Include(a => a.Parent)
                .Include(a=>a.Chart_Structures).AsQueryable();
        }
        public IQueryable<Chart_Structure> GetAllIncludeForDiagram()
        {
            return _kscHrContext.Chart_Structure.Include(a => a.Chart_StructureType)
              .Include(a=>a.Chart_JobPositions)
                .Include(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a=> a.Chart_JobPositions)

                .Include(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a=> a.Chart_JobPositions)

                .Include(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a=> a.Chart_JobPositions)

                .Include(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a=> a.Chart_JobPositions)

                .Include(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a => a.Chart_Structures).ThenInclude(a => a.Chart_Structures)
                .ThenInclude(a=> a.Chart_JobPositions)

                .AsQueryable();
        }


       

        /// <summary>
        /// بدست اوردن پست های مدیریت یک واحد
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Chart_JobPosition> GetJobPostionFromStrucureId(int structureId, bool isCanSeeAll = false)
        {
            var jobpositionIds = new List<Chart_JobPosition>();
            var DefinitionList = new List<string>() { "TM", "MA" };
            var jobpositions =GetAllQueryable().Where(a=>a.Id== structureId && a.IsActive==true)

                .Include(a=>a.Chart_JobPositions)
                .ThenInclude(a => a.Chart_JobIdentity)
                 .ThenInclude(a => a.Chart_JobCategory)
                 .ThenInclude(a => a.Chart_JobCategoryDefination)
                 .SelectMany(a=>a.Chart_JobPositions).Where(x=>x.IsActive).OrderByDescending(a => a.LevelNumber)
                 .AsQueryable();
                 

                 
            if (isCanSeeAll==false)
            {
                jobpositions = jobpositions.Where(a => DefinitionList.Contains(a.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination.Title));
            }
            return jobpositions.ToList();

        }

        /// <summary>
        /// بدست اوردن مدیران یک واحد 
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Employee> GetManagementFromStrucureId(int structureId)
        {
            var jobpositionManagement= GetJobPostionFromStrucureId(structureId);
            var jobPositionIds = jobpositionManagement.Select(a => a.Id);
            var employeesManagement = _kscHrContext.Employees.AsQueryable().Where(a=> jobPositionIds.Contains(a.Id)).ToList();


            return employeesManagement;

        }

        /// <summary>
        /// بدست واحد های غیر از خودش 
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public List<Chart_Structure> GetOtherStrucureId(int jobPositionId)
        {
            var jobpositionIds = new List<Chart_JobPosition>();
            var jobposition = _kscHrContext.Chart_JobPosition
                 .Where(x => x.Id == jobPositionId)
                 .FirstOrDefault();
            if (jobposition.IsActive == false)
            {
                throw new Exception($"پست {jobposition.MisJobPositionCode} غیر فعال است");
            }
            var DefinitionList = new List<string>() { "TM", "MA", "RE" };
            var findAllChildrenPost = _kscHrContext.Chart_JobPosition.AsNoTracking()
                      .Where(x => x.NewCodeRelation.Contains(jobposition.MisJobPositionCode)  && x.IsActive == true && x.JobIdentityId.HasValue)
                      .Include(a => a.Chart_JobIdentity)
                      .ThenInclude(a => a.Chart_JobCategory)
                      .ThenInclude(a => a.Chart_JobCategoryDefination)

                      .ToList();
      
            var allSubGroupjobPositionIds = findAllChildrenPost.Select(x => x.Id).ToList();

            var getOtherJobPosition = _kscHrContext.Chart_JobPosition.AsNoTracking()
                .Where(a => !allSubGroupjobPositionIds.Contains(a.Id))
                .Include(a => a.Chart_JobIdentity)
                .ThenInclude(a => a.Chart_JobCategory)
                .ThenInclude(a => a.Chart_JobCategoryDefination)
      
                      
                .Select(a=>a.Chart_Structure).ToList();

            return getOtherJobPosition;



        }


        /// <summary>
        /// بدست اوردن پست های مدیریت یک واحد
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public Chart_Structure GetAssistance(int structureId)
        {
            var assistance = new Chart_Structure();
            var Structurequery = GetAllQueryable().AsNoTracking();
            var FindStructure = Structurequery.FirstOrDefault(a => a.Id == structureId);
            var codeRelation = FindStructure.NewCodeRelation.Split(",");
            if(FindStructure.StructureTypeId== ((int)StructureTypeEnum.Moavenat))
            {
                return FindStructure;
            }
            else
            {
                assistance = Structurequery.FirstOrDefault(a=> codeRelation.Contains(a.MisJobPositionCode) && a.StructureTypeId== ((int)StructureTypeEnum.Moavenat));
            
            }


            return assistance;

        }

        /// <summary>
        /// بدست اوردن معاونت یک واحد از جاب پوزیشن
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        public Chart_Structure GetAssistanceFromJobPositionwithStructureId(int structureId)
        {
            var assistance = new Chart_Structure();
            var Structurequery = GetAllQueryable().AsNoTracking();
            var FindStructure = Structurequery.Where(a => a.Id == structureId)
                .Include(a=>a.Parent)
                
                .Include(a=>a.Chart_JobPositions).FirstOrDefault();
            if (FindStructure.IsActive == false)
            {
                throw new Exception($"واحد {FindStructure.MisJobPositionCode} غیر فعال است");
            }
            var jobposition=FindStructure.Chart_JobPositions.FirstOrDefault(a=>a.IsActive);
            var AllCodeRelation = jobposition.NewCodeRelation.Split(",");
            var FindAsistance = new Chart_JobPosition();
            if (jobposition.JobPoisitionStatusId == EnumJobPositionStatus.Moavenat.Id)
            {
                FindAsistance = jobposition;
            }
            else
            {

                FindAsistance = _kscHrContext.Chart_JobPosition
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

            if (FindStructure.Parent.StructureTypeId == 1)
            {
                return new Chart_Structure();
            }
          
            if (FindAsistance == null) throw new Exception("مافوق  سازمانی پست مورد نظر پیدا نشد");

            assistance = Structurequery.FirstOrDefault(a =>a.Id== FindAsistance.StructureId);
            return assistance;

        }

        public Tuple<List<Chart_Structure>, List<Chart_JobPosition>, List<Employee>> GetSubgroupStructureAndJobPositionAndEmployee(int structureId)
        {
            

            var findStructure = _kscHrContext.Chart_Structure.FirstOrDefault(a => a.Id == structureId);
            var getAllActiveSubGroup = _kscHrContext.Chart_Structure
                .Where(a => a.IsActive.Value == true)
                .Where(a => a.NewCodeRelation.Contains(findStructure.MisJobPositionCode))
                .Include(a => a.Chart_JobPositions)
                .Where(a => a.Chart_JobPositions.Any(x => x.IsActive == true))
                .ToList();

            var subgroupStructure = getAllActiveSubGroup.ToList();
            var subgroupJobPosition = getAllActiveSubGroup.SelectMany(a=>a.Chart_JobPositions).ToList();
            var JobPositionIds = subgroupJobPosition.Select(a=>a.Id).ToList();
            var employee = _kscHrContext.Employees.Where(x =>
           x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
           x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id 
           && x.TeamWorkId != null)
                .Where(a => JobPositionIds.Contains(a.JobPositionId.Value))
                .ToList();
            var result = new Tuple<List<Chart_Structure>,
                List<Chart_JobPosition>,
                List<Employee>>
                (subgroupStructure, subgroupJobPosition, employee);
            return result;


        }


    }








    public enum StructureTypeEnum
    {
        Moavenat=2,
        Bakhsh=3
    }
}
