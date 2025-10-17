using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.TeamWork;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.DTO.ODSViews.ViewMisEmployeeSecurity;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Share.Extention;
using System.Windows.Markup;
using KSC.Communication.Dto;
namespace Ksc.HR.Appication.Services.WorkShift
{
    public class TeamWorkService : ITeamWorkService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IMisUpdateService _misUpdateService;

        public TeamWorkService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler
            , IMisUpdateService misUpdateService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _misUpdateService = misUpdateService;
        }


        public List<TeamWorkModel> GetTeamWork()
        {
            var cities = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable();
            return _mapper.Map<List<TeamWorkModel>>(cities);

        }
        public FilterResult<TeamWorkModel> GetTeamWorkByFilter(TeamWorkGrdModel Filter)
        {
            var query = _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable().AsQueryable()
                .Include(a => a.TeamWorkCategory)
                .Include(a => a.OverTimeDefinition)
                //.Where(a => a.IsActive)
                .AsQueryable();
            if (Filter.teamWorkCategoryId.HasValue && Filter.teamWorkCategoryId > 0)
                query = query.Where(x => x.TeamWorkCategoryId == Filter.teamWorkCategoryId);

            var result = _FilterHandler.GetFilterResult<TeamWork>(query, Filter, "Id");

            var TeamWorksFiltred = result.Data.Select(a => new TeamWorkModel()
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Title,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                DomainName = a.DomainName,
                HasCommode = a.HasCommode,
                IsActive = a.IsActive,
                OverTimeDefinitionId = a.OverTimeDefinitionId,
                OverTimeDefinitionTitle = a.OverTimeDefinition != null ? a.OverTimeDefinition.Title : "",
                TeamWorkCategoryId = a.TeamWorkCategoryId,
                TeamWorkCategoryTitle = a.TeamWorkCategory != null ? a.TeamWorkCategory.Title : "",
                UpdateDate = a.UpdateDate,
                UpdateUser = a.UpdateUser,
                ValidityEndDate = a.ValidityEndDate,
                ValidityStartDate = a.ValidityStartDate,
            }).ToList();

            return new FilterResult<TeamWorkModel>()
            {
                Data = TeamWorksFiltred,
                Total = result.Total

            };
        }

        public FilterResult<TeamWorkCostCenterViewModel> GetAllTeamWorksWithCostCenters(FilterRequest request)
        {
            var teamWorks = _kscHrUnitOfWork.TeamWorkRepository.GetAll();

            var teamWorkCategories = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetAll();

            var query = (from tw in teamWorks
                         join twc in teamWorkCategories
                         on tw.TeamWorkCategoryId equals twc.Id
                         select new TeamWorkCostCenterViewModel
                         {
                             Id = tw.Id,
                             Title = tw.Title,
                             Code = tw.Code,
                             CostCenter = twc.CostCenter.ToString()
                         }).ToList();

            var result = _FilterHandler.GetFilterResult<TeamWorkCostCenterViewModel>(query, request, nameof(TeamWorkCostCenterViewModel.Id));

            return new FilterResult<TeamWorkCostCenterViewModel>()
            {
                Data = result.Data.ToList(),
                Total = result.Total
            };
        }


        public void Exists(int id, string title, int teamWorkCategoryId)
        {
            //return _kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Id != id && x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Id != id
            && x.Title == title
            && x.TeamWorkCategoryId == teamWorkCategoryId
            && x.IsActive == true
            ))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeResource.Title));
        }
        public void Exists(string title, int teamWorkCategoryId)
        {
            // return _kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Title == title
            && x.TeamWorkCategoryId == teamWorkCategoryId
            && x.IsActive == true
            ))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeResource.Title));
        }

        public void ExistsByCode(int id, string code, int teamWorkCategoryId)
        {
            // return _kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Id != id
            && x.Code == code
            && x.TeamWorkCategoryId == teamWorkCategoryId
            && x.IsActive == true
            ))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void CheckValidityEndDate(DateTime date)
        {
            // return _kscHrUnitOfWork.TeamWorkRepository.Any(x => x.Id != id && x.Code == code);
            if (Utility.IsSmallerThanNow(date))
                throw new HRBusinessException(Validations.RepetitiveId, "تاریخ خاتمه تیم باید از تاریخ جاری سیستم بزرگتر مساوی باشد");
        }

        //public bool IsEqualTeamWorkCategory(string code)
        //{
        //    var findTeamWorkCategory = code.Substring(0, 3);
        //    return _kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Id != id && x.Code == code);

        //}

        public void IsEqualTeamWorkCategory(string code, int TeamWorkCategoryId)
        {
            if (code.Length <= 3)
                throw new HRBusinessException(Validations.RepetitiveId, "کد بایستی بیشتر از 3 رقم باشد");
            if (code.Length > 4)
                throw new HRBusinessException(Validations.RepetitiveId, "کد بایستی 4 رقم باشد");
            var TeamWorkCategoryCodeOnPage = code.Substring(0, 3);
            //var findTeamWorkCategoryInDB = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetOneByCode(findTeamWorkCategory);
            var findTeamWorkCategoryInDB = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetById(TeamWorkCategoryId);

            if (findTeamWorkCategoryInDB.Code != TeamWorkCategoryCodeOnPage)
                throw new HRBusinessException(Validations.RepetitiveId,
                   String.Format("سه رقم اول کد بایستی {0} باشد", findTeamWorkCategoryInDB.Code));


        }
        /// <summary>
        ///  MIS بروزرسانی 
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="HRBusinessException"></exception>
        public void ConnectMIS(MISTeamWorkModel model)
        {
            var teamWorkCategory = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetById(model.TeamWorkCategoryId);
            var overTimeDefinition = _kscHrUnitOfWork.OverTimeDefinitionRepository.GetById(model.OverTimeDefinitionId.Value);

            UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
            {
                Domain = model.DomainName,
                FUNCTION = "TEAM-MANAGE",
                NUM_GRP_EMGRP_TEAM = teamWorkCategory.Code.Trim(),
                NUM_TEAM = model.Code,
                DES_TEAM = model.Title,
                DAT_STR = model.ValidityStartDate.ToPersianDate().Replace("/", ""),
                DAT_END = model.ValidityEndDate.HasValue ? model.ValidityEndDate.ToPersianDate().Replace("/", "") : "0",
                COD_OVT = overTimeDefinition.Code.Trim(),
                FLG_TEAM_CRUD = model.FLG_TEAM_CRUD,

            };
            var resultApiMis = _misUpdateService.UpdateTeamAndGroup(updateTeamAndGroupInputModel);
            if (resultApiMis.IsError)
            {
                throw new HRBusinessException("", String.Format("خطای MIS-{0}", resultApiMis.MsgError));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddTeamWork(AddTeamWorkModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            if (model.ValidityStartDate.HasValue == false)
                throw new HRBusinessException(Validations.RepetitiveId, "تاریخ شروع باید مقدار داشته باشد");

            if (model.ValidityEndDate.HasValue)
            {
                CheckValidityEndDate(model.ValidityEndDate.Value);
                if (model.ValidityStartDate > model.ValidityEndDate)
                    throw new HRBusinessException(Validations.RepetitiveId, "تاریخ شروع و پایان بررسی شود");
            }

            Exists(model.Id, model.Title, model.TeamWorkCategoryId);
            ExistsByCode(model.Id, model.Code, model.TeamWorkCategoryId);
            IsEqualTeamWorkCategory(model.Code, model.TeamWorkCategoryId);

            var TeamWork = _mapper.Map<TeamWork>(model);
            //TeamWork.InsertDate = DateTime.Now;
            //TeamWork.InsertUser = model.CurrentUserName;
            //TeamWork.IsActive = true;
            //TeamWork.DomainName = model.DomainName;

            _kscHrUnitOfWork.TeamWorkRepository.Add(TeamWork);

            ConnectMIS(new MISTeamWorkModel
            {
                Code = model.Code,
                Title = model.Title,
                OverTimeDefinitionId = model.OverTimeDefinitionId,
                TeamWorkCategoryId = model.TeamWorkCategoryId,
                ValidityStartDate = model.ValidityStartDate,
                ValidityEndDate = model.ValidityEndDate,
                FLG_TEAM_CRUD = "1",
                DomainName = model.DomainName,
            });

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateTeamWork(EditTeamWorkModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            if (model.ValidityStartDate.HasValue == false)
                throw new HRBusinessException(Validations.RepetitiveId, "تاریخ شروع باید مقدار داشته باشد");

            if (model.ValidityEndDate.HasValue)
            {
                CheckValidityEndDate(model.ValidityEndDate.Value);
                if (model.ValidityStartDate > model.ValidityEndDate)
                    throw new HRBusinessException(Validations.RepetitiveId, "تاریخ شروع و پایان بررسی شود");
            }

            Exists(model.Id, model.Title, model.TeamWorkCategoryId);
            ExistsByCode(model.Id, model.Code, model.TeamWorkCategoryId);
            IsEqualTeamWorkCategory(model.Code, model.TeamWorkCategoryId);

            var oneTeamWork = GetOne(model.Id);
            if (oneTeamWork == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            oneTeamWork.Code = model.Code;
            oneTeamWork.Title = model.Title;
            oneTeamWork.OverTimeDefinitionId = model.OverTimeDefinitionId;
            oneTeamWork.TeamWorkCategoryId = model.TeamWorkCategoryId;
            oneTeamWork.ValidityStartDate = model.ValidityStartDate;
            oneTeamWork.ValidityEndDate = model.ValidityEndDate;
            oneTeamWork.IsActive = model.IsActive;
            oneTeamWork.UpdateDate = DateTime.Now;
            oneTeamWork.UpdateUser = model.CurrentUserName;

            ConnectMIS(new MISTeamWorkModel
            {
                Code = model.Code,
                Title = model.Title,
                OverTimeDefinitionId = model.OverTimeDefinitionId,
                TeamWorkCategoryId = model.TeamWorkCategoryId,
                ValidityStartDate = model.ValidityStartDate,
                ValidityEndDate = model.ValidityEndDate,
                FLG_TEAM_CRUD = "2",
                DomainName = model.DomainName
            });

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
        public async Task<KscResult> RemoveTeamWork(EditTeamWorkModel model)
        {
            var result = new KscResult();
            //if (!result.Success)
            //    return result;
            var item = GetOne(model.Id);
            if (item == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }

            item.IsActive = false;
            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;

            ConnectMIS(new MISTeamWorkModel
            {
                Code = item.Code,
                Title = item.Title,
                OverTimeDefinitionId = item.OverTimeDefinitionId,
                TeamWorkCategoryId = item.TeamWorkCategoryId,
                ValidityStartDate = item.ValidityStartDate,
                ValidityEndDate = item.ValidityEndDate,
                FLG_TEAM_CRUD = "3",
                DomainName = model.DomainName
            });

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public TeamWork GetOne(int id)
        {
            return _kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable().First(a => a.Id == id);//&& a.IsActive
        }

        public EditTeamWorkModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditTeamWorkModel>(model);
        }

        public List<SearchTeamWorkModel> GetWorkByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler
                .GetFilterResult<TeamWork>(_kscHrUnitOfWork.TeamWorkRepository.WhereQueryable(a => a.IsActive).AsQueryable()
                , Filter, "Id");
            return _mapper.Map<List<SearchTeamWorkModel>>(result.Data.ToList());
        }


        public FilterResult<SearchTeamWorkModel> GetAllActiveWorkByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<TeamWork>(_kscHrUnitOfWork.TeamWorkRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");

            return new FilterResult<SearchTeamWorkModel>()
            {
                Data = _mapper.Map<List<SearchTeamWorkModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public FilterResult<SearchTeamWorkModel> GetActiveTeamWorkAsync(SearchTeamWorkModel Filter)
        {
            var result = _FilterHandler.GetFilterResult<TeamWork>(_kscHrUnitOfWork.TeamWorkRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var TeamWorkList = result.Data.ToList();
            if (Filter.Id > 0)
            {
                var teamWorkData = _kscHrUnitOfWork.TeamWorkRepository.GetById(Filter.Id);
                TeamWorkList.Add(teamWorkData);

            }
            var data = _mapper.Map<List<SearchTeamWorkModel>>(TeamWorkList);
            foreach (var item in data)
            {
                item.Title = item.Code + " - " + item.Title;
            }
            return new FilterResult<SearchTeamWorkModel>()
            {
                Data = data,
                Total = result.Total

            };
        }

        //public List<SearchTeamWorkModel> GetTeamWorkByKendoFilter(FilterRequest Filter)
        //{
        //    //var teamworksBywinUser = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsQueryable().Where(a => a.WindowsUser.ToLower() == "m.monajem").AsNoTracking();
        //    //var teamCodes= teamworksBywinUser.Select(a => a.TeamCode.ToString()).ToList();
        //    //var query =_kscHrUnitOfWork.TeamWorkRepository.GetAllQueryable()
        //    //    .Where(a => a.IsActive && teamCodes.Contains(a.Code)).AsQueryable();
        //    var query = _kscHrUnitOfWork.TeamWorkRepository.GetTeamWorksByWinUser("m.monajem");
        //    var result = _FilterHandler.GetFilterResult<TeamWork>(query, Filter, "Id");
        //    var finalData = result.Data.Select(a => new SearchTeamWorkModel()
        //    {
        //        Id = a.Id,
        //        Title = a.Code + " " + a.Title,
        //        Code = a.Code,
        //    }).ToList();
        //    return _mapper.Map<List<SearchTeamWorkModel>>(finalData);
        //}

        public List<SearchTeamWorkModel> GetTeamWorkByKendoFilterByUser(SearchTeamWorkModel Filter)
        {
            var query = _kscHrUnitOfWork.TeamWorkRepository.GetTeamWorksByWinUser(Filter.CurrentUserName);
            var result = _FilterHandler.GetFilterResult<TeamWork>(query, Filter, "Id");
            var finalData = result.Data.Select(a => new SearchTeamWorkModel()
            {
                Id = a.Id,
                Title = a.Code + " " + a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchTeamWorkModel>>(finalData);
        }
        /// <summary>
        /// تمامی تیم هایی که کاربر دسترسی دارد
        /// به همراه تیم هایی که کاربری به آن ها تخصیص داده نشده
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public FilterResult<SearchViewMisEmployeeSecurityModel> GetAllTeamWithCheckAccessKendoFilter(SearchViewMisEmployeeSecurityModel Filter)
        {
            var queryMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsNoTracking();
            if (Filter.IsOfficialAttendAbcense == true)
            {
                var TeamWorkId = queryMisEmployeeSecurity.Where(x => x.DisplaySecurity == 1).Select(a => a.TeamWorkId).ToList();
                var TeamWork = _kscHrUnitOfWork.TeamWorkRepository.WhereQueryable(a => TeamWorkId.Contains(a.Id)).ToList().Select(t => new
                {
                    t.Id,
                    TeamCode = t.Code.ToDecimal(),
                    TeamTitle = t.Title
                }).ToList();

                queryMisEmployeeSecurity = (from t in TeamWork
                                            join v in queryMisEmployeeSecurity.Where(x => x.DisplaySecurity == 1) on t.Id equals v.TeamWorkId into g
                                            from s in g.DefaultIfEmpty()
                                            select new ViewMisEmployeeSecurity
                                            {
                                                TeamCode = t.TeamCode,
                                                TeamTitle = t.TeamTitle,
                                                TeamWorkId = g.Count() > 0 ? g.FirstOrDefault().TeamWorkId : 0,
                                                AuthenticationSecurity = g.Count() > 0 ? g.FirstOrDefault().AuthenticationSecurity : 0,
                                                TeamWorkIsActive = g.Count() > 0 ? g.FirstOrDefault().TeamWorkIsActive : false,
                                                Id = g.Count() > 0 ? g.FirstOrDefault().Id : "",
                                                EmployeeNumber = g.Count() > 0 ? g.FirstOrDefault().EmployeeNumber : 0,
                                                WindowsUser = g.Count() > 0 ? g.FirstOrDefault().WindowsUser : "",
                                                DisplaySecurity = g.Count() > 0 ? g.FirstOrDefault().DisplaySecurity : 0,
                                            }).AsQueryable();
            }

            var FilteredQuery = queryMisEmployeeSecurity;
            Filter.FixSortandFilter<ViewMisEmployeeSecurity>(new Dictionary<string, string>
                {
                    { "TeamCode", "eq" },
                    { "EmployeeNumber", "eq" },
                    { "AuthenticationSecurity", "eq" },
                    { "DisplaySecurity", "eq" }
                });
            //var test = FilteredQuery.Where(a => a.TeamCode == 6201);
            if (Filter.IsOfficialAttendAbcense == false)
            {
                FilteredQuery = FilteredQuery.Where(a => a.WindowsUser.ToLower() == Filter.CurrentUserName);
            }
            var TeamList = FilteredQuery.ToList();
            var teamList = FilteredQuery.ToList().DistinctBy(a => a.TeamCode);
            var TeamCodes = TeamList.GroupBy(a => a.TeamCode).Select(a => a.Key).ToList();
            FilterResult<ViewMisEmployeeSecurity> result = _FilterHandler.GetFilterResult<ViewMisEmployeeSecurity>(teamList, Filter, nameof(ViewMisEmployeeSecurity.TeamCode));

            var GetTeamCodeDisplaySecurityOne = queryMisEmployeeSecurity.Where(a => TeamCodes.Any(x => x == a.TeamCode) && a.DisplaySecurity == 1).ToList();
            var employeesNumber = GetTeamCodeDisplaySecurityOne.Select(a => a.EmployeeNumber.ToString()).ToList();
            var EmployeeList = _kscHrUnitOfWork.EmployeeRepository.GetAllQueryable().AsQueryable().AsNoTracking().Where(a => employeesNumber.Contains(a.EmployeeNumber)).ToList();
            foreach (var item in result.Data.ToList())
            {
                var teamCode = GetTeamCodeDisplaySecurityOne.FirstOrDefault(c => c.TeamCode == item.TeamCode);
                if (teamCode == null)
                    continue;
                var employee = EmployeeList.First(a => a.EmployeeNumber == teamCode.EmployeeNumber.ToString());
                item.TeamTitle = teamCode.TeamCode + " - " + teamCode.TeamTitle + " (" + employee.Name.Trim() + " " + employee.Family.Trim() + "-" + employee.EmployeeNumber.Trim() + ")";
            }

            var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
            {
                Data = _mapper.Map<List<SearchViewMisEmployeeSecurityModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;
        }



        /// <summary>
        /// تمامی تیم هایی که کاربر دسترسی دارد
        /// به همراه تیم هایی که کاربری به آن ها تخصیص داده نشده
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public FilterResult<SearchViewMisEmployeeSecurityModel> GetAllTeamMapper(int[] ids)
        {
            var queryMisEmployeeSecurity = _kscHrUnitOfWork.ViewMisEmployeeSecurityRepository.GetAllQueryable().AsNoTracking();
            var TeamWorkId = queryMisEmployeeSecurity.Where(x => x.DisplaySecurity == 1).Select(a => a.TeamWorkId).ToList();
            var TeamWork = _kscHrUnitOfWork.TeamWorkRepository.WhereQueryable(a => TeamWorkId.Contains(a.Id)).ToList().Select(t => new
            {
                t.Id,
                TeamCode = t.Code.ToDecimal(),
                TeamTitle = t.Title
            }).ToList();


            var teams = (from t in TeamWork
                         join v in queryMisEmployeeSecurity.Where(x => x.DisplaySecurity == 1) on t.Id equals v.TeamWorkId into g
                         from s in g.DefaultIfEmpty()
                         select new ViewMisEmployeeSecurity
                         {
                             TeamCode = t.TeamCode,
                             TeamTitle = t.TeamTitle,
                             TeamWorkId = g.Count() > 0 ? g.First().TeamWorkId : 0,
                             AuthenticationSecurity = g.Count() > 0 ? g.First().AuthenticationSecurity : 0,
                             TeamWorkIsActive = g.Count() > 0 ? g.First().TeamWorkIsActive : false,
                             Id = g.Count() > 0 ? g.First().Id : "",
                             EmployeeNumber = g.Count() > 0 ? g.First().EmployeeNumber : 0,
                             WindowsUser = g.Count() > 0 ? g.First().WindowsUser : "",
                             DisplaySecurity = g.Count() > 0 ? g.First().DisplaySecurity : 0,
                         }).AsQueryable().DistinctBy(a => a.TeamCode).ToList();
            var result = new List<SearchViewMisEmployeeSecurityModel>();
            ;


            if (ids != null && ids.Any())
            {
                var index = 0;
                foreach (var team in teams)
                {
                    if (ids.Contains(int.Parse(team.Id)))
                    {
                        result.Add(_mapper.Map<SearchViewMisEmployeeSecurityModel>(team));
                    }
                    index++;
                }
            }

            var modelResult = new FilterResult<SearchViewMisEmployeeSecurityModel>
            {
                Data = result,
                Total = result.Count()
            };
            return modelResult;
        }


    }
}
