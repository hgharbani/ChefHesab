using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.TeamWorkCategory;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Appication.Interfaces.MIS;
using KSC.Communication.Dto;
using Ksc.HR.Appication.Interfaces.ExternalService;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class TeamWorkCategoryService : ITeamWorkCategoryService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IMisUpdateService _misUpdateService;
        private readonly IIndustrialAccountingService _industrialAccountingService;

        public TeamWorkCategoryService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler
            , IMisUpdateService misUpdateService
            , IIndustrialAccountingService industrialAccountingService)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _misUpdateService = misUpdateService;
            _industrialAccountingService = industrialAccountingService;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Id != id && x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        public void ExistsByCode(int id, string code)
        {
            //return _kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.TeamWorkCategoryRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        /// <summary>
        ///  MIS بروزرسانی 
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="HRBusinessException"></exception>
        public void ConnectMIS(MISTeamWorkCategoryModel model)
        {
            var teamWorkCategoryType = _kscHrUnitOfWork.TeamWorkCategoryTypeRepository.GetById(model.TeamWorkCategoryTypeId);
            var teamWorkMangementCode = _kscHrUnitOfWork.TeamWorkMangementCodeRepository.GetById(model.TeamWorkMangementCodeId);
           
            UpdateTeamAndGroupInputModel updateTeamAndGroupInputModel = new UpdateTeamAndGroupInputModel()
            {
                Domain = model.DomainName,
                FUNCTION = "GROUP-MANAGE",
                NUM_GRP_EMGRP = model.Code,
                DES_GRP_EMGRP = model.Title,
                FLG_MAN_OPE_EMGRP = teamWorkCategoryType.Code,
                FK_CCRRX = model.CostCenter.ToString(),
                COD_MNGT = teamWorkMangementCode.Code.Trim(),
                FLG_GRP_CRUD = model.FLG_GRP_CRUD,

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
        public async Task<KscResult> AddTeamWorkCategory(AddTeamWorkCategoryModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            Exists(model.Id, model.Title);
            ExistsByCode(model.Id, model.Code);
            

            var TeamWorkCategory = _mapper.Map<TeamWorkCategory>(model);
            //TeamWorkCategory.InsertDate = DateTime.Now;
            //TeamWorkCategory.InsertUser = model.CurrentUserName;
            //TeamWorkCategory.IsActive = true;
            //TeamWorkCategory.DomainName = model.DomainName;

            _kscHrUnitOfWork.TeamWorkCategoryRepository.Add(TeamWorkCategory);

            ConnectMIS(new MISTeamWorkCategoryModel
            {
                Code = model.Code,
                Title = model.Title,
                TeamWorkCategoryTypeId = model.TeamWorkCategoryTypeId,
                CostCenter = model.CostCenter,
                TeamWorkMangementCodeId = model.TeamWorkMangementCodeId,
                FLG_GRP_CRUD = "1",
                DomainName=model.DomainName
            });

            
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateTeamWorkCategory(EditTeamWorkCategoryModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            Exists(model.Id, model.Title);
            ExistsByCode(model.Id, model.Code);
            

            var oneTeamWorkCategory = GetOne(model.Id);
            if (oneTeamWorkCategory == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            oneTeamWorkCategory.IsActive = model.IsActive;
            oneTeamWorkCategory.Code = model.Code;
            oneTeamWorkCategory.Title = model.Title;
            oneTeamWorkCategory.CostCenter = model.CostCenter;
            oneTeamWorkCategory.TeamWorkMangementCodeId = model.TeamWorkMangementCodeId;
            oneTeamWorkCategory.TeamWorkCategoryTypeId = model.TeamWorkCategoryTypeId;
            oneTeamWorkCategory.UpdateDate = DateTime.Now;
            oneTeamWorkCategory.UpdateUser = model.CurrentUserName;


            ConnectMIS(new MISTeamWorkCategoryModel
            {
                Code = model.Code,
                Title = model.Title,
                TeamWorkCategoryTypeId = model.TeamWorkCategoryTypeId,
                CostCenter = model.CostCenter,
                TeamWorkMangementCodeId = model.TeamWorkMangementCodeId,
                FLG_GRP_CRUD = "2",
                DomainName = model.DomainName
            });

            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveTeamWorkCategory(EditTeamWorkCategoryModel model)
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

            ConnectMIS(new MISTeamWorkCategoryModel
            {
                Code = item.Code,
                Title = item.Title,
                TeamWorkCategoryTypeId = item.TeamWorkCategoryTypeId,
                CostCenter = item.CostCenter,
                TeamWorkMangementCodeId = item.TeamWorkMangementCodeId,
                FLG_GRP_CRUD = "3",
                DomainName = model.DomainName
            });

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<TeamWorkCategoryModel> GetTeamWorkCategory()
        {
            var cities = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetAllQueryable();
            return _mapper.Map<List<TeamWorkCategoryModel>>(cities);

        }


        //public FilterResult<TeamWorkCategoryModel> GetTeamWorkCategoryByFilter(FilterRequest Filter)
        //{
        //    var queryCostCenter = _kscHrUnitOfWork.ViewMisCostCenterRepository.GetAllQueryable().AsQueryable();

        //    var query = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetAllQueryable().AsQueryable()
        //        .Include(a => a.TeamWorkCategoryType_TeamWorkCategoryTypeId)
        //        .Include(a => a.TeamWorkMangementCode_TeamWorkMangementCodeId)
        //        .Where(a => a.IsActive)
        //        .AsQueryable()
        //        .Select(a => new TeamWorkCategoryModel()
        //        {
        //            TeamWorkCategoryTypeId = a.TeamWorkCategoryTypeId,
        //            TeamWorkCategoryTypeTitle = a.TeamWorkCategoryType_TeamWorkCategoryTypeId.Title,
        //            TeamWorkMangementCodeId = a.TeamWorkMangementCodeId,
        //            TeamWorkMangementCodeTitle = a.TeamWorkMangementCode_TeamWorkMangementCodeId.Title,
        //            CostCenter = a.CostCenter,
        //            Id = a.Id,
        //            Code = a.Code,
        //            Title = a.Title,
        //            InsertDate = a.InsertDate,
        //            InsertUser = a.InsertUser
        //        });

        //    var result = _FilterHandler.GetFilterResult<TeamWorkCategoryModel>(query, Filter, "Id");

        //    var TeamWorkCategorysFiltred = result.Data
        //        .Join(queryCostCenter, r => r.CostCenter, c => c.CostCenterCode, (r, c) => new { finalResult = r, Costcenterdata = c })
        //        .Select(a => new TeamWorkCategoryModel()
        //        {
        //            TeamWorkCategoryTypeId = a.finalResult.TeamWorkCategoryTypeId,
        //            // TeamWorkCategoryTypeTitle = a.finalResult.TeamWorkCategoryType_TeamWorkCategoryTypeId.Title,
        //            TeamWorkCategoryTypeTitle = a.finalResult.TeamWorkCategoryTypeTitle,
        //            TeamWorkMangementCodeId = a.finalResult.TeamWorkMangementCodeId,
        //            // TeamWorkMangementCodeTitle = a.finalResult.TeamWorkMangementCode_TeamWorkMangementCodeId.Title,
        //            TeamWorkMangementCodeTitle = a.finalResult.TeamWorkMangementCodeTitle,
        //            CostCenter = a.finalResult.CostCenter,
        //            CostCenterTitle = a.Costcenterdata.CostCenterTitle,
        //            Id = a.finalResult.Id,
        //            Code = a.finalResult.Code,
        //            Title = a.finalResult.Title,
        //            InsertDate = a.finalResult.InsertDate,
        //            InsertUser = a.finalResult.InsertUser
        //        }).ToList();

        //    return new FilterResult<TeamWorkCategoryModel>()
        //    {
        //        Data = TeamWorkCategorysFiltred,
        //        Total = result.Total

        //    };
        //}

        public async Task<FilterResult<TeamWorkCategoryModel>> GetTeamWorkCategoryByFilter(TeamWorkCategoryModel Filter)
        {
            List<CostCenterDataForJoin> queryCostCenter = new List<CostCenterDataForJoin>();

            if (Filter.DomainName == "KSC")
            {
                var FilterCostCenter = new FilterRequest();
                queryCostCenter = (await _industrialAccountingService.GetCostCenterAsync(FilterCostCenter)).ToList()
                    .Select(x => new CostCenterDataForJoin
                    {
                        CostCenterCode = x.CostCenterCode,
                        CostCenterTitle = x.Title
                    }).ToList();
            }
            else
            {
                queryCostCenter = _kscHrUnitOfWork.ViewMisCostCenterRepository.GetAllQueryable()
                    .Select(x => new CostCenterDataForJoin
                    {
                        CostCenterCode = x.CostCenterCode,
                        CostCenterTitle = x.CostCenterTitle
                    }).ToList();
            }

            var query = _kscHrUnitOfWork.TeamWorkCategoryRepository.GetAllQueryable().AsQueryable()
                .Include(a => a.TeamWorkCategoryType_TeamWorkCategoryTypeId)
                .Include(a => a.TeamWorkMangementCode_TeamWorkMangementCodeId)
                .Where(a => a.IsActive)
                .AsQueryable()
                .Select(a => new TeamWorkCategoryModel()
                {
                    TeamWorkCategoryTypeId = a.TeamWorkCategoryTypeId,
                    TeamWorkCategoryTypeTitle = a.TeamWorkCategoryType_TeamWorkCategoryTypeId.Title,
                    TeamWorkMangementCodeId = a.TeamWorkMangementCodeId,
                    TeamWorkMangementCodeTitle = a.TeamWorkMangementCode_TeamWorkMangementCodeId.Title,
                    CostCenter = a.CostCenter,
                    Id = a.Id,
                    Code = a.Code,
                    Title = a.Title,
                    InsertDate = a.InsertDate,
                    InsertUser = a.InsertUser
                }).ToList();


            var TeamWorkCategorysFiltred = (from db in query
                                           join viewData in queryCostCenter on db.CostCenter equals viewData.CostCenterCode into temp
                                           from viewData in temp.DefaultIfEmpty()
                                           select new TeamWorkCategoryModel() 
                                           {
                                               TeamWorkCategoryTypeId = db.TeamWorkCategoryTypeId,
                                               TeamWorkCategoryTypeTitle = db.TeamWorkCategoryTypeTitle,
                                               TeamWorkMangementCodeId = db.TeamWorkMangementCodeId,
                                               TeamWorkMangementCodeTitle = db.TeamWorkMangementCodeTitle,
                                               CostCenter = db.CostCenter,
                                               CostCenterTitle = viewData?.CostCenterTitle,
                                               Id = db.Id,
                                               Code = db.Code,
                                               Title = db.Title,
                                               InsertDate = db.InsertDate,
                                               InsertUser = db.InsertUser
                                           }).ToList();
            var result = _FilterHandler.GetFilterResult<TeamWorkCategoryModel>(TeamWorkCategorysFiltred, Filter, "Id");
            return new FilterResult<TeamWorkCategoryModel>()
            {
                Data = result.Data,
                Total = result.Total

            };
        }


        public TeamWorkCategory GetOne(int id)
        {
            return _kscHrUnitOfWork.TeamWorkCategoryRepository.GetAllQueryable().First(a => a.Id == id && a.IsActive);
        }



        public EditTeamWorkCategoryModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditTeamWorkCategoryModel>(model);
        }

        public List<SearchTeamWorkCategoryModel> GetTeamWorkCategoryByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler
                .GetFilterResult<TeamWorkCategory>(
                _kscHrUnitOfWork.TeamWorkCategoryRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            var finalData = result.Data.Select(a => new SearchTeamWorkCategoryModel()
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
            }).ToList();
            return _mapper.Map<List<SearchTeamWorkCategoryModel>>(finalData);
        }


    }
}
