using AutoMapper;
using Ksc.Hr.Application.Interfaces;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.DTO.View_PresentEmployees;
using Ksc.Hr.Domain.Entities;
using KSC.Common.Filters.Models;
using KSC.Common.Filters.Contracts;
using Ksc.HR.Domain.Repositories;
using KSC.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;
using KSC.Common.Filters.Handlers;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.MaritalStatus;
using System.Collections.Generic;

using Ksc.Material.Common.Tools;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Domain.Entities.Workshift;




namespace Ksc.Hr.Application.Services
{


    public class View_PresentEmployeesService : IView_PresentEmployeesService
    {

        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _filterHandler;
        private readonly IView_PresentEmployeesRepository _repository;

        public View_PresentEmployeesService(IKscHrUnitOfWork kscHrUnitOfWork,
            IMapper mapper,
            IFilterHandler filterHandler,
            IView_PresentEmployeesRepository repository
            )
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _filterHandler = filterHandler;
            _repository = repository;
            _mapper = mapper;



        }

        public FilterResult<View_PresentEmployeesDto> GetAllContractInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();
            if (filter.ContractorID > 0)
            {
                query = query.Where(x => x.ContractorID == filter.ContractorID);
            }
            if (filter.JobPositionId > 0)
            {
                var getAllSubGroup = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllSubGroupJobPostionFromJobPositionId(filter.JobPositionId.Value);
                var costCenters = getAllSubGroup.Select(a => a.CostCenter).ToList();
                query = query.Where(a => costCenters.Contains(a.CostCenter));
            }
            query = query.Where(a => a.IsKscEmployee == false && a.IsPresent == true);
          

            var result = _filterHandler.GetFilterResult<View_PresentEmployees>(query, filter, "EmployeeNumber");


            var finalresult = result.Data.Select(a => new View_PresentEmployeesDto()
            {
                Id = a.Id,
                EmployeeNumber = a.EmployeeNumber,
                Name = a.Name,
                Family = a.Family,
                CostCenter = a.CostCenter,
                ContractCode = a.ContractCode,
                ContractDescription = a.ContractDescription,
                Contractor = a.Contractor,
                ContractorID = a.ContractorID,
                WorkDay = a.WorkDay,

            }).ToList();
            return new FilterResult<View_PresentEmployeesDto>()
            {
                Data = finalresult,
                Total = result.Total
            };

        }

        public FilterResult<View_PresentEmployeesDto> GetAllKscEmployeeInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();
            if (filter.TeamWorkId > 0)
            {
                query = query.Where(x => x.TeamWorkId == filter.TeamWorkId);
            }
            if (filter.CostCenter > 0)
            {
                query = query.Where(x => x.CostCenter == filter.CostCenter);
            }
            if (filter.JobPositionId>0)
            {
                var jobposition = _kscHrUnitOfWork.Chart_JobPositionRepository
                  .GetById(filter.JobPositionId.Value);
                var childjobposition = _kscHrUnitOfWork.Chart_JobPositionRepository
                    .GetAllSubGroupJobPostionFromJobPositionId(filter.JobPositionId.Value);

                var misjobPositionCodes = childjobposition.Select(a => a.MisJobPositionCode).ToList();
                misjobPositionCodes.Add(jobposition.MisJobPositionCode);

                query = query.Where(x => misjobPositionCodes.Contains(x.MisJobPositionCode));
            }
            query = query.Where(a => a.IsKscEmployee == true && a.IsPresent == true);
            var result = _filterHandler.GetFilterResult<View_PresentEmployees>(query, filter, "EmployeeNumber");


            var finalresult = result.Data.Select(a => new View_PresentEmployeesDto()
            {
                Id = a.Id,
                EmployeeNumber = a.EmployeeNumber,
                Name = a.Name,
                Family = a.Family,
                CostCenter = a.CostCenter,
                TeamWorkCode = a.TeamWorkCode,
                TeamWorkTitle = a.TeamWorkTitle,
                MisJobPositionCode = a.MisJobPositionCode,
                JobPositionTitle = a.JobPositionTitle,
                WorkDay = a.WorkDay,

            }).ToList();
            return new FilterResult<View_PresentEmployeesDto>()
            {
                Data = finalresult,
                Total = result.Total
            };
        }

        public FilterResult<CostCenterPresentationDto> GetConstCenterGroupedKscInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();

            query = query.Where(a => a.IsKscEmployee == true && a.IsPresent == true);

            var finalRequest = query.GroupBy(a => new { a.CostCenter})
                .Select(a => new CostCenterPresentationDto()
                {

                    CostCenter = a.Key.CostCenter.Value,
                    ShiftWorkEmployeeCount = a.Count(a => a.WorkDay == false),
                    WorkDayEmployeeCount = a.Count(a => a.WorkDay == true),

                }).ToList();

            var result = _filterHandler.GetFilterResult<CostCenterPresentationDto>(finalRequest, filter, "SumEmployeeInCompany");
            return result;
        }
        public FilterResult<JobPositionDetailsDto> GetTopChartKscInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();

            var chartQuery = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllQueryable().Where(a=>a.IsActive).AsNoTracking();
            var statusIds=new List<int>() { 9,2};
            var getAllTopChart = chartQuery.Where(a => a.JobPoisitionStatusId.HasValue && statusIds.Contains(a.JobPoisitionStatusId.Value)).Select(a => new
            {
                a.Id,
                a.MisJobPositionCode,
                a.Title,
                newcodeParent = a.NewCodeRelation + "," + a.MisJobPositionCode.ToString(),
                a.CostCenter

            }).ToList();

            var getAllNewcodeRelationParent = getAllTopChart.Select(a => a.newcodeParent).ToList();

            var FindAllChild= chartQuery.Where(a=> getAllNewcodeRelationParent.Any(x=>a.NewCodeRelation.StartsWith(x))).ToList();
            var FindAllChildMisJobPositionCode = FindAllChild.Select(x=>x.MisJobPositionCode).ToList();


            var parentMisJobCode = getAllTopChart.Select(a => a.MisJobPositionCode).ToList();
            FindAllChildMisJobPositionCode.AddRange(parentMisJobCode);

            var allPresentUser = query.Where(a => a.IsKscEmployee == true && a.IsPresent == true&& FindAllChildMisJobPositionCode.Contains(a.MisJobPositionCode)).ToList();

            var results = new List<JobPositionDetailsDto>();
            if (!string.IsNullOrEmpty(filter.CurrentUser))
            {
                var userdefinition = _kscHrUnitOfWork.UserDefinitionRepository.Where(a => a.WindowsUser == filter.CurrentUser && a.IsActive).AsQueryable().Include(a => a.Employee).FirstOrDefault();
                getAllTopChart = getAllTopChart.Where(a => a.Id == userdefinition.Employee.JobPositionId.Value).ToList();
            }
           
            foreach (var item in getAllTopChart)
            {
                var parent = item.newcodeParent.Replace($",{item.MisJobPositionCode}","");
              if (getAllTopChart.Any(x=>x.newcodeParent== parent)) continue ;


                var findChidlPost= FindAllChild.Where(a=>a.NewCodeRelation.StartsWith(item.newcodeParent)).ToList();
                var getAllChildJobCode = findChidlPost.Select(a => a.MisJobPositionCode).ToList();
                getAllChildJobCode.Add(item.MisJobPositionCode);

                var ShiftWorkEmployeeCount = allPresentUser.Where(x => x.WorkDay == false).Where(x => getAllChildJobCode.Contains(x.MisJobPositionCode)).GroupBy(x=>x.EmployeeNumber).Count();
                var WorkDayEmployeeCount = allPresentUser.Where(x => x.WorkDay == true).Where(x => getAllChildJobCode.Contains(x.MisJobPositionCode)).GroupBy(x => x.EmployeeNumber).Count();

                var result= new JobPositionDetailsDto() {
                    JobpositionId = item.Id,
                    JobPositionTitle = item.Title,
                    MisJobpositionCode=item.MisJobPositionCode,
                    ShiftWorkEmployeeCount = ShiftWorkEmployeeCount,
                    WorkDayEmployeeCount = WorkDayEmployeeCount,
                    ChildMisJobpositionCode = getAllChildJobCode,
                    CostCenter= item.CostCenter,

                };
                results.Add(result);


            }
            results = results.Where(a => a.SumEmployeeInCompany > 0).ToList();
            var finalRequest = _filterHandler.GetFilterResult<JobPositionDetailsDto>(results, filter, "SumEmployeeInCompany");
            return finalRequest;
        }

        public KscResult userIsTopChartManagement(SearchPresentationDto filter)
        {
            var results = new KscResult();
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();

            var chartQuery = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllQueryable().Where(a => a.IsActive).AsNoTracking();
            var statusIds = new List<int>() { 9, 2 };
            var getAllTopChart = chartQuery.Where(a => a.JobPoisitionStatusId.HasValue && statusIds.Contains(a.JobPoisitionStatusId.Value)).Select(a => new
            {
                a.Id,
                a.MisJobPositionCode,
                a.Title,
                newcodeParent = a.NewCodeRelation + "," + a.MisJobPositionCode.ToString(),


            }).ToList();

           var jobpostionIds= getAllTopChart.Select(a=>a.Id).ToList();
            var employees= _kscHrUnitOfWork.EmployeeRepository.GetEmployeeWorkInJobposition(jobpostionIds);
            var userdefinition = _kscHrUnitOfWork.UserDefinitionRepository.FirstOrDefault(a => a.WindowsUser == filter.CurrentUser && a.IsActive);
            if (employees.Any(a => a.Id == userdefinition.EmployeeId))
            {
                return results;
            }
            results.AddError("", "کاربر دسترسی ندارد");
            return results;
        }



        public FilterResult<JobPositionDetailsDto> GetNexLevelTopChartKscInCompany(SearchPresentationDto filter)
        {
            if (filter.JobPositionId == 0) return new FilterResult<JobPositionDetailsDto>();
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();

            var chartQuery = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllQueryable().Where(a => a.IsActive).AsNoTracking();
            var statusIds = new List<int>() { 9, 2 };
            
            var findchart=chartQuery.Where(a => a.Id==filter.JobPositionId).Select(a => new
            {
                a.Id,
                a.MisJobPositionCode,
                a.Title,
                newcodeParent = a.NewCodeRelation + "," + a.MisJobPositionCode.ToString(),
            }).First();

            var getAllTopChart = chartQuery.Where(a => a.ParentId==findchart.Id).Select(a => new
            {
                a.Id,
                a.MisJobPositionCode,
                a.Title,
                newcodeParent = a.NewCodeRelation + "," + a.MisJobPositionCode.ToString(),


            }).ToList();

            var getAllNewcodeRelationParent = getAllTopChart.Select(a => a.newcodeParent).ToList();

            var FindAllChild = chartQuery.Where(a => getAllNewcodeRelationParent.Any(x => a.NewCodeRelation.StartsWith(x))).ToList();
            var FindAllChildMisJobPositionCode = FindAllChild.Select(x => x.MisJobPositionCode).ToList();

            var parentMisJobCode = getAllTopChart.Select(a => a.MisJobPositionCode).ToList();
            FindAllChildMisJobPositionCode.AddRange(parentMisJobCode);

            var allPresentUser = query.Where(a => a.IsKscEmployee == true && a.IsPresent == true && FindAllChildMisJobPositionCode.Contains(a.MisJobPositionCode)).ToList();

            var results = new List<JobPositionDetailsDto>();
            foreach (var item in getAllTopChart)
            {

                var findChidlPost = FindAllChild.Where(a => a.NewCodeRelation.StartsWith(item.newcodeParent)).ToList();
                var getAllChildJobCode = findChidlPost.Select(a => a.MisJobPositionCode).ToList();
                getAllChildJobCode.Add(item.MisJobPositionCode);
                var ShiftWorkEmployeeCount = allPresentUser.Where(x => x.WorkDay == false).Where(x => getAllChildJobCode.Contains(x.MisJobPositionCode)).GroupBy(x => x.EmployeeNumber).Count();
                var WorkDayEmployeeCount = allPresentUser.Where(x => x.WorkDay == true).Where(x => getAllChildJobCode.Contains(x.MisJobPositionCode)).GroupBy(x => x.EmployeeNumber).Count();

                var result = new JobPositionDetailsDto()
                {
                    JobpositionId=item.Id,
                    JobPositionTitle = item.Title,
                    MisJobpositionCode = item.MisJobPositionCode,
                    ShiftWorkEmployeeCount = ShiftWorkEmployeeCount,
                    WorkDayEmployeeCount = WorkDayEmployeeCount,
                    ChildMisJobpositionCode= getAllChildJobCode


                };
                results.Add(result);


            }
            results = results.Where(a => a.SumEmployeeInCompany > 0).ToList();
            var finalRequest = _filterHandler.GetFilterResult<JobPositionDetailsDto>(results, filter, "SumEmployeeInCompany");
            return finalRequest;
        }
        public FilterResult<ContractPresentationDto> GetContractGroupedInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();

          
            query = query.Where(a => a.IsKscEmployee == false && a.IsPresent == true);
            if (!string.IsNullOrEmpty(filter.CurrentUser))
            {
                var userdefinition = _kscHrUnitOfWork.UserDefinitionRepository
                    .WhereQueryable(a => a.WindowsUser == filter.CurrentUser && a.IsActive)
                    .Include(a=>a.Employee)
                    .FirstOrDefault();
                filter.JobPositionId= userdefinition.Employee.JobPositionId??0;
            }
            if (filter.JobPositionId > 0)
            {
                var getAllSubGroup = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllSubGroupJobPostionFromJobPositionId(filter.JobPositionId.Value);
                var costCenters = getAllSubGroup.Select(a => a.CostCenter).ToList();
                query = query.Where(a => costCenters.Contains(a.CostCenter));
            }
            var finalRequest = query.GroupBy(a => new { a.ContractorID, a.Contractor })
                .Select(a => new ContractPresentationDto()
                {
                    ContractorID = a.Key.ContractorID.Value,
                    Contractor = a.Key.Contractor,
                    ShiftWorkEmployeeCount = a.Count(a => a.WorkDay == false),
                    WorkDayEmployeeCount = a.Count(a => a.WorkDay == true),

                }).ToList();

            var result = _filterHandler.GetFilterResult<ContractPresentationDto>(finalRequest, filter, "SumEmployeeInCompany");
            return result;

        }

        public FilterResult<TeamWorkPresentation> GetTeamGroupedKscInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();
            
            query = query.Where(a => a.IsKscEmployee == true && a.IsPresent == true);

            var finalRequest = query.GroupBy(a => new { a.TeamWorkId, a.TeamWorkCode, a.TeamWorkTitle })
                .Select(a => new TeamWorkPresentation()
                {
                    TeamWorkId = a.Key.TeamWorkId.Value,
                    TeamWrokCode = int.Parse(a.Key.TeamWorkCode),
                    TeamWrokTitle = a.Key.TeamWorkTitle,
                    ShiftWorkEmployeeCount = a.Count(a => a.WorkDay == false),
                    WorkDayEmployeeCount = a.Count(a => a.WorkDay == true),
                    
                }).ToList();

            var result = _filterHandler.GetFilterResult<TeamWorkPresentation>(finalRequest, filter, "SumEmployeeInCompany");
            return result;

        }
        public FilterResult<WorkGroupPresentation> GetWorkGroupCountAllKscEmployeeInCompany(SearchPresentationDto filter)
        {
            var query = _kscHrUnitOfWork.View_PresentEmployeesRepository.GetAllQueryable().AsNoTracking();
            if (filter.TeamWorkId > 0)
            {
                query = query.Where(x => x.TeamWorkId == filter.TeamWorkId);
            }
            if (filter.CostCenter > 0)
            {
                query = query.Where(x => x.CostCenter == filter.CostCenter);
            }
            if (!string.IsNullOrEmpty(filter.CurrentUser))
            {
                var userdefinition = _kscHrUnitOfWork.UserDefinitionRepository.Where(a => a.WindowsUser == filter.CurrentUser && a.IsActive).AsQueryable().Include(a => a.Employee).FirstOrDefault();
                filter.JobPositionId = userdefinition.Employee.JobPositionId ?? 0;
            }
           

            if (filter.JobPositionId > 0)
            {
                var jobposition = _kscHrUnitOfWork.Chart_JobPositionRepository
                  .GetById(filter.JobPositionId.Value);
                var childjobposition = _kscHrUnitOfWork.Chart_JobPositionRepository
                    .GetAllSubGroupJobPostionFromJobPositionId(filter.JobPositionId.Value);

                var misjobPositionCodes = childjobposition.Select(a => a.MisJobPositionCode).ToList();
                misjobPositionCodes.Add(jobposition.MisJobPositionCode);

                query = query.Where(x => misjobPositionCodes.Contains(x.MisJobPositionCode));
            }


            var chartQuery = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllQueryable().Where(a => a.IsActive).AsNoTracking();
            var statusIds = new List<int>() { 9, 2 };
            var getAllTopChart = chartQuery.Where(a => a.JobPoisitionStatusId.HasValue && statusIds.Contains(a.JobPoisitionStatusId.Value)).Select(a => new
            {
                a.Id,
                a.MisJobPositionCode,
                a.Title,
                newcodeParent = a.NewCodeRelation + "," + a.MisJobPositionCode.ToString(),


            }).ToList();


            var getAllNewcodeRelationParent = getAllTopChart.Select(a => a.newcodeParent).ToList();
            var jobpositionIds = getAllTopChart.Select(a => a.Id).ToList();

            var FindAllChild = chartQuery.Where(a => getAllNewcodeRelationParent.Any(x => a.NewCodeRelation.StartsWith(x))).ToList();
            var FindAllChildMisJobPositionCode = FindAllChild.Select(x => x.Id).ToList();
            jobpositionIds.AddRange(FindAllChildMisJobPositionCode);

            var final = query.Where(a => a.IsKscEmployee == true && a.IsPresent == true&& jobpositionIds.Contains(a.JobPositionId.Value)).ToList();
            var employeeNumber = final.Select(a => a.EmployeeNumber).Distinct().ToList();

            var employeeList = _kscHrUnitOfWork.EmployeeRepository.GetEmployeesActivAndeHaveTeamAsNotracking()
                .Include(a=>a.WorkGroup)
                .ThenInclude(a=>a.WorkTime)
                .Select(a => new
                {
                    WorkGroupCode= a.WorkGroup.WorkTime.Title + "-" + a.WorkGroup.Code,
                    a.EmployeeNumber
                }).ToList();

            var employeeListIncompany = employeeList.Where(a => employeeNumber.Contains(a.EmployeeNumber))
                .GroupBy(a => a.WorkGroupCode).Select(a => new WorkGroupPresentation()
                {
                    Title = a.Key,
                    TotalCount = employeeList.Count(x => x.WorkGroupCode == a.Key),
                    InCompanyCount = a.Count(),
                    EmployeeNumbers = a.Select(x => x.EmployeeNumber).ToList()
                });

            var topchartEmployee = new List<string>();



            foreach (var item in getAllTopChart)
            {

                
                var findChidlPost = FindAllChild.Where(a => a.NewCodeRelation.StartsWith(item.newcodeParent)).ToList();
                var getAllChildJobCode = findChidlPost.Select(a => a.MisJobPositionCode).Distinct().ToList();
                getAllChildJobCode.Add(item.MisJobPositionCode);
                var ShiftWorkEmployeeCount = final.Where(x => x.WorkDay == false)
                    .Where(x => getAllChildJobCode.Contains(x.MisJobPositionCode)).Select(x=>x.EmployeeNumber).Distinct().ToList();
                var WorkDayEmployeeCount = final.Where(x => x.WorkDay == true)
                    .Where(x => getAllChildJobCode.Contains(x.MisJobPositionCode)).Select(x=>x.EmployeeNumber).Distinct().ToList();
                topchartEmployee.AddRange(WorkDayEmployeeCount);



            }

          var  topgridemployee = employeeListIncompany.Where(x=>x.Title.Contains("R")).SelectMany(a => a.EmployeeNumbers).ToList();
         var    topgridemployeeFiltered = topchartEmployee.Where(a => topgridemployee.Any(x=>x==a)).ToList();
            var finalEmployee = _kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable()
                .Include(a => a.Employees)
                .Include(a => a.WorkTime)
                .AsNoTrackingWithIdentityResolution()
                .Select(a => new WorkGroupPresentation()
                {
                    Title = a.WorkTime.Title + "-" + a.Code,
                    TotalCount = a.Employees.Count(x => x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                    x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id
                    && x.TeamWorkId != null),
                    InCompanyCount = a.Employees.Count(x => x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                    x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id &&
                    x.TeamWorkId != null && employeeNumber.Contains(x.EmployeeNumber))
                }).Where(a => a.TotalCount > 0);



            var result = _filterHandler.GetFilterResult<WorkGroupPresentation>(finalEmployee, filter, "InCompanyCount");
            return result;
        }
    }
}


