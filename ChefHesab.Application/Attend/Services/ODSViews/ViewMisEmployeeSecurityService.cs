using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmployeeSecurity;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Extention;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Appication.Services.ODSViews
{
    public class ViewMisEmployeeSecurityService : IViewMisEmployeeSecurityService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisEmployeeSecurityService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public FilterResult<SearchViewMisEmployeeSecurityModel> GetViewEmployeeByKendoFilterForTeamWork(SearchViewMisEmployeeSecurityModel Filter)
        {
            var query = from t in _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable()
                        join v in _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking() on t.Id equals v.TeamCode into g
                        from v in g.DefaultIfEmpty()
                        select v;
            Filter.FixSortandFilter<ViewMisEmployeeSecurity>(new Dictionary<string, string>
                {
                    { "TeamCode", "eq" },
                    { "EmployeeNumber", "eq" },
                    { "AuthenticationSecurity", "eq" },
                    { "DisplaySecurity", "eq" }
                });
            var teamCurentUser = query.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            //var result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurity>(query,Filter, "TeamCode");
            FilterResult<ViewMisEmployeeSecurity> result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurity>(teamCurentUser, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));
            var TeamCodes = result.Data.Select(a => a.TeamCode).ToList();
            var GetTeamCodeDisplaySecurityOne = query.Where(a => TeamCodes.Contains(a.TeamCode) && a.DisplaySecurity == 1 && a.TeamWorkIsActive).ToList();
            var employeesNumber = GetTeamCodeDisplaySecurityOne.Select(a => a.EmployeeNumber.ToString()).ToList();
            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => employeesNumber.Contains(a.EmployeeNumber)).ToList();
            foreach (var item in result.Data.ToList())
            {
                var teamCode = GetTeamCodeDisplaySecurityOne.First(c => c.TeamCode == item.TeamCode);
                var employee = EmployeeList.First(a => a.EmployeeNumber == teamCode.EmployeeNumber.ToString());
                item.TeamTitle = teamCode.TeamTitle + "(" + employee.Name + " " + employee.Family + "-" + employee.EmployeeNumber + ")";
            }

            var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
            {
                Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }

        //by leila
        public FilterResult<SearchViewMisEmployeeSecurityModel> GetViewMisEmployeeSecurityByFilter(SearchViewMisEmployeeSecurityModel Filter)
        {
            try
            {
                var viewmis = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository
                    .GetAllQueryable()
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(a => a.TeamWorkIsActive)
                    ;

                var query = (from v in viewmis
                             select new SearchViewMisEmployeeSecurityModel()
                             {
                                 WindowsUser = v.WindowsUser,
                                 EmployeeNumber = v.EmployeeNumber.ToString(),
                                 TeamCode = v.TeamCode,
                                 DisplaySecurity = v.DisplaySecurity,
                                 TeamTitle = v.TeamCode + "-" + v.TeamTitle,
                                 FullName = v.FullName,
                                 Id = v.TeamWorkId.ToString(),
                             }).ToList();
                
                Filter.FixSortandFilter<ViewMisEmployeeSecurity>(new Dictionary<string, string>
                {
                    { "TeamCode", "eq" },
                    { "EmployeeNumber", "eq" },
                    { "AuthenticationSecurity", "eq" },
                    { "DisplaySecurity", "eq" }
                });//♥
                if (Filter.IsOfficialAttendAbcense == false)
                {
                    query = query.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName).ToList();
                }
                var listDisplay1 = query.Where(c => c.DisplaySecurity == 1).ToList();
                //var employeenumbers = listDisplay1.Where(x => x.EmployeeNumber != null).Select(a => a.EmployeeNumber.ToString()).ToList();
                //var Employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => employeenumbers.Contains(a.EmployeeNumber)).ToList();
                foreach (var item in query)
                {
                    var currentTeam = listDisplay1.FirstOrDefault(c => c.TeamCode == item.TeamCode);
                    //if (currentTeam != null )
                    //{
                    //        item.TeamTitle = $"{item.TeamTitle} {currentTeam.FullName.Trim()}-({currentTeam.EmployeeNumber})";

                    //    }

                    if (currentTeam != null)
                    {
                        if (item.EmployeeNumber != null)
                        {

                            item.TeamTitle = $"{item.TeamTitle} ({currentTeam.FullName.Trim()}-{currentTeam.EmployeeNumber})";
                        }
                        else
                        {
                            item.TeamTitle = $"{item.TeamTitle} ({currentTeam.FullName.Trim()})";


                        }
                    }

                }
                var distincList = query.DistinctBy(a => a.TeamCode).ToList();
                FilterResult<SearchViewMisEmployeeSecurityModel> result = _FilterHandler.GetFilterResult<SearchViewMisEmployeeSecurityModel>(distincList, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));

                var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
                {
                    Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
                    Total = result.Total
                };
                return modelResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public FilterResult<SearchViewMisEmployeeSecurityModel> GetViewMisEmployeeSecurityByKendoFilter(SearchViewMisEmployeeSecurityModel Filter)
        {
            try
            {
                var viewmis = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository
                    .GetAllQueryable()
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(a=>a.TeamWorkIsActive)
                    ;

            var query = (from v in viewmis
                         select new SearchViewMisEmployeeSecurityModel()
                         {
                             WindowsUser = v.WindowsUser,
                             EmployeeNumber = v.EmployeeNumber.ToString(),
                             TeamCode = v.TeamCode,
                             DisplaySecurity = v.DisplaySecurity,
                             TeamWorkIsActive = v.TeamWorkIsActive,
                             TeamTitle = v.TeamCode + "-" + v.TeamTitle,
                             FullName=v.FullName,
                         }).ToList();

            Filter.FixSortandFilter<ViewMisEmployeeSecurity>(new Dictionary<string, string>
                {
                    { "TeamCode", "eq" },
                    { "EmployeeNumber", "eq" },
                    { "AuthenticationSecurity", "eq" },
                    { "DisplaySecurity", "eq" }
                });
            if (Filter.IsOfficialAttendAbcense == false)
            {
                query = query.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName).ToList();
            }
            var listDisplay1 = query.Where(c => c.DisplaySecurity == 1 && c.TeamWorkIsActive).ToList();
            //var employeenumbers = listDisplay1.Where(x => x.EmployeeNumber != null).Select(a => a.EmployeeNumber.ToString()).ToList();
            //var Employee = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => employeenumbers.Contains(a.EmployeeNumber)).ToList();
            foreach (var item in query)
            {
                var currentTeam = listDisplay1.FirstOrDefault(c => c.TeamCode == item.TeamCode);
                    //if (currentTeam != null )
                    //{
                    //        item.TeamTitle = $"{item.TeamTitle} {currentTeam.FullName.Trim()}-({currentTeam.EmployeeNumber})";

                    //    }

                    if (currentTeam != null)
                    {
                        if (item.EmployeeNumber!=null) { 

                        item.TeamTitle = $"{item.TeamTitle} ({currentTeam.FullName.Trim()}-{currentTeam.EmployeeNumber})";
                        }
                        else
                        {
                            item.TeamTitle = $"{item.TeamTitle} ({currentTeam.FullName.Trim()})";


                        }
                    }

                }
            var distincList = query.DistinctBy(a => a.TeamCode).ToList();
            FilterResult<SearchViewMisEmployeeSecurityModel> result = _FilterHandler.GetFilterResult<SearchViewMisEmployeeSecurityModel>(distincList, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));

            var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
            {
                Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public FilterResult<SearchViewMisEmployeeSecurityModel> GetViewMisEmployeeSecurityByKendoFilterSalaryUser(SearchViewMisEmployeeSecurityModel Filter)
        {
            var query = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().Where(a =>a.TeamWorkIsActive).AsNoTracking();
            var FilteredQuery = query;
            Filter.FixSortandFilter<ViewMisEmployeeSecurity>(new Dictionary<string, string>
                {
                    { "TeamCode", "eq" },
                    { "EmployeeNumber", "eq" },
                    { "AuthenticationSecurity", "eq" },
                    { "DisplaySecurity", "eq" }
                });
            if (Filter.IsSalaryUser == false)
            {
                FilteredQuery = FilteredQuery.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }
            var teamList = FilteredQuery.ToList().DistinctBy(a => a.TeamCode);
            var TeamCodes = teamList.Select(a => a.TeamCode).ToList();
            //var result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurity>(query,Filter, "TeamCode");
            FilterResult<ViewMisEmployeeSecurity> result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurity>(teamList, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));

            var GetTeamCodeDisplaySecurityOne = query.Where(a => TeamCodes.Contains(a.TeamCode) && a.DisplaySecurity == 1 && a.TeamWorkIsActive).ToList();
            var employeesNumber = GetTeamCodeDisplaySecurityOne.Select(a => a.EmployeeNumber.ToString()).ToList();
            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => employeesNumber.Contains(a.EmployeeNumber)).ToList();
            foreach (var item in result.Data.ToList())
            {
                var teamCode = GetTeamCodeDisplaySecurityOne.FirstOrDefault(c => c.TeamCode == item.TeamCode);
                if (teamCode == null) continue;
                if (teamCode.EmployeeNumber != null) { 
                       var employee = EmployeeList.First(a => a.EmployeeNumber == teamCode.EmployeeNumber.ToString());
                item.TeamTitle = teamCode.TeamCode + " - " + teamCode.TeamTitle + " (" + employee.Name.Trim() + " " + employee.Family.Trim() + "-" + employee.EmployeeNumber.Trim() + ")";
                }
                else
                {
                    item.TeamTitle = teamCode.TeamCode + " - " + teamCode.TeamTitle + " (" + teamCode.FullName.Trim() + ")";
                }
                ;
            }

            var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
            {
                Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }


        public async Task<decimal?> GetFirstDisplayTeamUser(string CurentUserName)
        {
            try
            {


                var query = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().AsNoTracking();


                var teamCurentUser = await query.FirstOrDefaultAsync(a => a.DisplaySecurity == 1 && a.WindowsUser.ToLower() == CurentUserName.ToLower() && a.TeamWorkIsActive);


                //var result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurity>(query,Filter, "TeamCode");

                return teamCurentUser != null ? teamCurentUser.TeamCode : 0;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #region EmployeeTeamByOfficialManagment - مدیریت تیم و شیفت 

        //public FilterResult<SearchViewMisEmployeeSecurityModel> GetAllViewMisEmployeeSecurityByKendoFilter(FilterRequest Filter)
        //{
        //    var allMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.WhereQueryable(x => x.DisplaySecurity == 1).AsNoTracking();
        //    var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable();
        //    var queryMisEmployeeSecurity = (from t in teamWork
        //                                    join v in allMisEmployeeSecurity on t.Id equals v.TeamWorkId into g
        //                                    from s in g.DefaultIfEmpty()
        //                                    select new ViewMisEmployeeSecurityModel
        //                                    {
        //                                        TeamCode = t.Code,
        //                                        TeamTitle = t.Title,
        //                                        TeamWorkId = t.Id,
        //                                        AuthenticationSecurity = (s.AuthenticationSecurity != null ? s.AuthenticationSecurity : 0),
        //                                        TeamWorkIsActive = (s.TeamWorkIsActive != null ? s.TeamWorkIsActive : false),
        //                                        Id = (s.Id != null ? s.Id : ""),
        //                                        EmployeeNumber = (s.EmployeeNumber != null ? s.EmployeeNumber : 0),
        //                                        WindowsUser = (s.WindowsUser != null ? s.WindowsUser : ""),
        //                                        DisplaySecurity = (s.DisplaySecurity != null ? s.DisplaySecurity : 0),
        //                                    }).AsQueryable();

        //    Filter.FixSortandFilter<ViewMisEmployeeSecurityModel>(new Dictionary<string, string>
        //        {
        //            { "TeamCode", "eq" },
        //            { "EmployeeNumber", "eq" },
        //            { "AuthenticationSecurity", "eq" },
        //            { "DisplaySecurity", "eq" }
        //        });
        //    FilterResult<ViewMisEmployeeSecurityModel> result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurityModel>(queryMisEmployeeSecurity, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));
        //    var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
        //    {
        //        Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
        //        Total = result.Total
        //    };
        //    return modelResult;
        //}
        public FilterResult<SearchViewMisEmployeeSecurityModel> GetAllViewMisEmployeeSecurityByKendoFilter(FilterRequest Filter)
        {
            var teamWork = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable();
            var queryMisEmployeeSecurity = (from t in teamWork
                                            select new ViewMisEmployeeSecurityModel
                                            {
                                                TeamCode = t.Code,
                                                TeamTitle = t.Title,
                                                TeamWorkIsActive = t.IsActive
                                            });

            Filter.FixSortandFilter<ViewMisEmployeeSecurityModel>(new Dictionary<string, string>
                {
                    { "TeamCode", "eq" },
                               });
            FilterResult<ViewMisEmployeeSecurityModel> result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurityModel>(queryMisEmployeeSecurity, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));
            var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
            {
                Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }
        #endregion



        /// <summary>
        /// بدست آوردن کدهای هزینه ای که فرد دسترسی دارد
        /// فرد باید Tm باشد
        /// </summary>
        /// <param name="winUser"></param>
        /// <returns></returns>
        public List<decimal> GetcostCenterUserAccess(SearchUserInCostCenters filter)
        {
            var GetTeamAccess = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
                .Where(a => a.WindowsUser.ToLower() == filter.WindowsUser && a.TeamWorkIsActive);

            var employeeTeamWork = GetTeamAccess.Select(a => a.TeamWorkId);

            var GetCostCenters = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable()
                .Where(a => employeeTeamWork.Contains(a.Id))
                .Include(a => a.TeamWorkCategory)
                .Select(a=>a.TeamWorkCategory.CostCenter).Distinct().ToList();


            var EmployeeNumber = GetTeamAccess.FirstOrDefault().EmployeeNumber.ToString();
            var employeequery = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().FirstOrDefault(a => a.EmployeeNumber == EmployeeNumber);

            var IsTmUser = _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllQueryable().Where(a => a.Id == employeequery.JobPositionId)
                .Include(a => a.Chart_JobIdentity).ThenInclude(a => a.Chart_JobCategory).ThenInclude(a => a.Chart_JobCategoryDefination)
                .Any(a => a.Chart_JobIdentity.Chart_JobCategory.Chart_JobCategoryDefination.Title == "TM");

            if (IsTmUser)
            {
                
                return GetCostCenters;
            }
            return new List<decimal>();
        }

        /// <summary>
        /// بدست آوردن افراد زیر مجموعه ای که فرد دارد
        /// حتما باید جز مرکز هزینه مد نظر باشد
        /// </summary>
        /// <param name="costCentrer"></param>
        /// <param name="winuser"></param>
        /// <returns></returns>
        public List<UserWindowTeamAccess> GetEmployeeWorkInCostCenter(SearchUserInCostCenters filter)
        {
            var GetTeamAccess = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable()
                .Where(a =>a.TeamWorkIsActive);
            if (filter.IsAdmin == false)
            {
                GetTeamAccess = GetTeamAccess.Where(a => a.WindowsUser == filter.WindowsUser);

            }
            var employeeTeamWork = GetTeamAccess.Select(a => a.TeamWorkId);

            var TeamWorkIds = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable()
                .Where(a => employeeTeamWork.Contains(a.Id) && filter.CostCenter.Contains(a.TeamWorkCategory.CostCenter))
                .Include(a => a.TeamWorkCategory)
                .Select(a => a.Id).ToList();


          var getEmployeeAccessed=  _kscHrUnitOfWork.EmployeeRepository.GetEmployeesActivAndeHaveTeamAsNotracking()
                
                .Include(a=>a.TeamWork).Where(a=> TeamWorkIds.Contains(a.TeamWorkId.Value)).Select(a=>new UserWindowTeamAccess()
                {
                    EmployeeNumber = a.EmployeeNumber,
                    NameFamily = a.Name +" "+a.Family,
                    TeamCode=a.TeamWork.Code,
                    TeamTitle=a.TeamWork.Title,
                    CostCenter = a.TeamWork.TeamWorkCategory.CostCenter,

                    JobPositionId =a.JobPositionId,

                }).ToList();
         
            var jobPositionIds= getEmployeeAccessed.Select(a=>a.JobPositionId).ToList();


            var jobPositionsFinded=    _kscHrUnitOfWork.Chart_JobPositionRepository.GetAllQueryable().Where(a => jobPositionIds.Contains(a.Id)).Include(a=>a.Chart_Structure).ToList();

            foreach(var item in getEmployeeAccessed)
            {
                var jobPosition = jobPositionsFinded.FirstOrDefault(a => a.Id == item.JobPositionId);
                if (jobPosition != null)
                {
                    item.StructureTitle = jobPosition.Chart_Structure.Title;
                    item.StructureCode = jobPosition.Chart_Structure.Code;
                }
            }

            return getEmployeeAccessed;
        }
    }
}
