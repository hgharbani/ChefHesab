using AutoMapper;
using KSC.Common.Filters.Contracts;
using Ksc.HR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.IndustrialAccounting.Shared.Utility;
using NetTopologySuite.Index.HPRtree;

namespace Ksc.HR.Appication.Services.MIS
{
    public class SyncDataJobPositionMisToWebService : ISyncDataJobPositionMisToWebService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        public SyncDataJobPositionMisToWebService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
        }

        public async Task<KscResult> InsertJobPositionFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var jobpositionModel = model.InsertAndUpdateJobPositionViewModel;
                var isExist = _kscHrUnitOfWork.Chart_JobPositionRepository.GetChart_JobPositionByMisCode(jobpositionModel.MisJobPositionCode);
                if (isExist != null)
                {
                    result.AddError("", "کد تکراری میباشد");
                    return result;
                }
                var jobPosition = new Chart_JobPosition();
                if (jobpositionModel.JobIdentityCode != null)
                {
                    var jobIdentity = _kscHrUnitOfWork.Chart_JobIdentityRepository
                        .GetChart_JobIdentityByCode(jobpositionModel.JobIdentityCode);
                    if (jobIdentity != null)
                    {
                        var jobIdentityId = jobIdentity.Select(x => x.Id).FirstOrDefault();
                        jobPosition.JobIdentityId = jobIdentityId;

                    }
                }

                if (jobpositionModel.JobPoisitionStatus != null)
                {
                    var jobStatus = Convert.ToInt32(jobpositionModel.JobPoisitionStatus);
                    jobPosition.JobPoisitionStatusId = jobStatus;
                }

                if (jobpositionModel.JobPositionNatureCode != null)
                {
                    var obPositionNatureId = Convert.ToInt32(jobpositionModel.JobPositionNatureCode);
                    jobPosition.JobPositionNatureId = obPositionNatureId;
                }
                if (jobpositionModel.StructureCode != null)
                {
                    var Structure = _kscHrUnitOfWork.Chart_StructureRepository
                        .GetByMisStructureCode(jobpositionModel.StructureCode);

                    if (Structure != null)
                    {
                        var structureId = Structure.Select(x => x.Id).FirstOrDefault();
                        if (structureId != 0)
                        {
                            jobPosition.StructureId = structureId;
                        }

                    }
                }


                if (jobpositionModel.Code == null)
                {

                    var code = _kscHrUnitOfWork.Chart_JobPositionRepository
                        .GetJobPositionCode();
                    if (code != 0)
                    {
                        jobPosition.Code = (code.ToString() != null) ? code.ToString().Fa2En() : code.ToString();

                    }
                }


                if (jobpositionModel.ParentCode != null)
                {

                    var parent = _kscHrUnitOfWork.Chart_JobPositionRepository
                        .GetChart_JobPositionByMisCode(jobpositionModel.ParentCode);

                    if (parent != null)
                    {
                        var parentId = parent.Id;
                        jobPosition.ParentId = parentId;



                        if (jobpositionModel.LevelNumber == null)
                        {
                            var levelParent = (parent.LevelNumber == null) ? 0 : parent.LevelNumber + 1;
                            jobPosition.LevelNumber = levelParent;
                        }

                    }

                }

                //if (jobpositionModel.LevelNumber != null)
                //{
                //    jobPosition.LevelNumber = Convert.ToInt32(jobpositionModel.LevelNumber);

                //}
                //else
                //{
                //    jobPosition.LevelNumber = 0;
                //}

                var orderNo = _kscHrUnitOfWork.Chart_JobPositionRepository.GetJobPositionOrderNo();

                jobPosition.OrderNo = orderNo;
                {
                    jobPosition.ParentMisCode = jobpositionModel.ParentCode;
                    jobPosition.CapacityCount = (jobpositionModel.CapacityCount != null) ? Convert.ToInt32(jobpositionModel.CapacityCount) : 0;
                    //jobPosition.CategoryCoefficientId = 1;
                    //jobPosition.Code = jobpositionModel.Code;
                    //jobPosition.CodeRelations = jobpositionModel.CodeRelations;
                    jobPosition.CofficientProximityProduct = (jobpositionModel.CofficientProximityProduct == null) ? 0 : Convert.ToDouble(jobpositionModel.CofficientProximityProduct);
                    jobPosition.CostCenter = (jobpositionModel.CostCenter == null) ? 0 : Convert.ToDecimal(jobpositionModel.CostCenter);
                    jobPosition.Description = (jobpositionModel.Description != null) ? jobpositionModel.Description.FixPersianChars() : jobpositionModel.Description;
                    jobPosition.EndDate = (jobpositionModel.EndDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(jobpositionModel.EndDate);
                    jobPosition.ExtraCount = (jobpositionModel.ExtraCount == null) ? 0 : Convert.ToInt32(jobpositionModel.ExtraCount);
                    jobPosition.InsertDate = DateTime.Now;
                    jobPosition.InsertUser = model.UserName;
                    jobPosition.InsuranceJobCode = (jobpositionModel.InsuranceJobCode != null) ? jobpositionModel.InsuranceJobCode.Fa2En() : jobpositionModel.InsuranceJobCode;
                    jobPosition.IsActive = true;
                    jobPosition.IsOnCall = (jobpositionModel.IsOnCall == null || jobpositionModel.IsOnCall == "0") ? false : true;
                    jobPosition.IsStrategic = (jobpositionModel.IsStrategic == null || jobpositionModel.IsStrategic == "0") ? false : true;
                    // jobPosition.LevelNumber = 0;

                    jobPosition.MisJobPositionCode = (jobpositionModel.MisJobPositionCode != null) ? jobpositionModel.MisJobPositionCode.Fa2En() : jobpositionModel.MisJobPositionCode;
                    jobPosition.PermissionExistCommodityCount = (jobpositionModel.PermissionExistCommodityCount == null) ? 0 : Convert.ToInt32(jobpositionModel.PermissionExistCommodityCount);
                    //jobPosition.ProductionEfficiencyId = 1;
                    jobPosition.RewardSpecificEfficincy = (jobpositionModel.RewardSpecificEfficincy == null) ? 0 : Convert.ToDouble(jobpositionModel.RewardSpecificEfficincy);
                    //jobPosition.SpecificRewardId = 6;
                    jobPosition.SpecificRewardId = (jobpositionModel.SpecificRewardCode == null) ? null : Convert.ToInt32(jobpositionModel.SpecificRewardCode);
                    jobPosition.StartDate = (jobpositionModel.StartDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(jobpositionModel.StartDate);
                    jobPosition.WorkingShiftOutsourcingCount = (jobpositionModel.WorkingShiftOutsourcingCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingShiftOutsourcingCount);
                    jobPosition.WorkingShiftCount = (jobpositionModel.WorkingShiftCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingShiftCount);
                    jobPosition.WorkingDayOutsourcingCount = (jobpositionModel.WorkingDayOutsourcingCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingDayOutsourcingCount);
                    jobPosition.WorkingDayCount = (jobpositionModel.WorkingDayCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingDayCount);
                    jobPosition.Title = (jobpositionModel.Title != null) ? jobpositionModel.Title.FixPersianChars() : jobpositionModel.Title;
                    jobPosition.TemporaryCount = (jobpositionModel.TemporaryCount == null) ? 0 : Convert.ToInt32(jobpositionModel.TemporaryCount);
                    jobPosition.SubstituteCount = (jobpositionModel.SubstituteCount == null) ? 0 : Convert.ToInt32(jobpositionModel.SubstituteCount);
                    jobPosition.StructureEndDate = (jobpositionModel.StructureEndDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(jobpositionModel.StructureEndDate);
                    jobPosition.Chart_JobPositionIncreasePercents = new List<Chart_JobPositionIncreasePercent>();


                };
                //var flagIncrease = (jobpositionModel.FlagIncrease == null || jobpositionModel.FlagIncrease == "0") ? false : true;
                //if (flagIncrease)
                //{
                //    jobPosition.Chart_JobPositionIncreasePercents = new List<Chart_JobPositionIncreasePercent>();
                //    var AddIncreaseList = new List<Chart_JobPositionIncreasePercent>();
                //    var listError = new List<KscError>();
                //    try
                //    {

                //        var splitedStartDate = jobpositionModel.IncreaseStartDate.Split('|');

                //        var finalEnDate = new List<string>();
                //        var splitedEndDate = string.IsNullOrEmpty(jobpositionModel.IncreaseEndDate) ? null : jobpositionModel.IncreaseEndDate.Split('|');
                //        for (var a = 0; a < splitedEndDate.Length - 1; a++)
                //        {
                //            finalEnDate.Add(splitedEndDate[a]);
                //        }
                //        if (finalEnDate.Any())
                //        {
                //            if (finalEnDate.Count(a => a == null || a == "0" || a == "") > 1)
                //            {
                //                throw new HRBusinessException(Validations.RepetitiveId, "این پست نمیتواند دو رکورد فعال داشته باشد");
                //            }
                //            if (finalEnDate.Count(a => a == null || a == "0" || a == "") == 0)
                //            {
                //                throw new HRBusinessException(Validations.RepetitiveId, "این پست باید یک  رکورد فعال داشته باشد");
                //            }
                //        }


                //        var CoefficientYearsDaySplited = jobpositionModel.CoefficientYearsDay.Split('|');
                //        var CoefficientYearsShiftsplited = jobpositionModel.CoefficientYearsShift.Split('|');

                //        for (int i = 0; i < splitedStartDate.Length - 1; i++)
                //        {
                //            var sss = new Chart_JobPositionIncreasePercent();
                //            if (CoefficientYearsDaySplited[i] == null || CoefficientYearsDaySplited[i] == "0" || CoefficientYearsDaySplited[i] == "")
                //            {
                //                listError.Add(new KscError("", $"امتیاز روزکار وارد نشده", 0));
                //                continue;
                //            }
                //            if (CoefficientYearsShiftsplited[i] == null || CoefficientYearsShiftsplited[i] == "0" || CoefficientYearsShiftsplited[i] == "")
                //            {
                //                listError.Add(new KscError("", $"امتیاز شیفت وارد نشده", 0));
                //                continue;
                //            }
                //            if (splitedStartDate[i] == null || splitedStartDate[i] == "0" || splitedStartDate[i] == "")
                //            {
                //                listError.Add(new KscError("", $"تاریخ شروع ردیف {i} درست وارد نشده است", 0));
                //                continue;
                //            }

                //            sss.CoefficientYearsDay = double.Parse(CoefficientYearsDaySplited[i]);
                //            sss.CoefficientYearsShift = double.Parse(CoefficientYearsShiftsplited[i]);
                //            sss.StartDate = (splitedStartDate[i] == null || splitedStartDate[i] == "0" || splitedStartDate[i] == "") ? null : DateTimeExtension.ShamsiToMiladiMIS(splitedStartDate[i]);
                //            sss.EndDate = (finalEnDate[i] == null || finalEnDate[i] == "0" || finalEnDate[i] == "") ? null : DateTimeExtension.ShamsiToMiladiMIS(splitedEndDate[i]);
                //            sss.IsActive = (finalEnDate[i] == null || finalEnDate[i] == "0" || finalEnDate[i] == "") ? true : false;
                //            AddIncreaseList.Add(sss);
                //        }

                //        if (listError.Count > 0)
                //        {
                //            result.AddErrors(listError);
                //            return result;
                //        }
                //        else if (AddIncreaseList.Count > 0)
                //        {
                //            foreach (var score in AddIncreaseList)
                //            {
                //                jobPosition.Chart_JobPositionIncreasePercents.Add(score);
                //            }

                //        }
                //        else
                //        {
                //            throw new HRBusinessException(Validations.RepetitiveId, "عملیات بعلت خالی بودن لیست سنوات افزوده ذخیره نشد");
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
                //    }
                //}


                await _kscHrUnitOfWork.Chart_JobPositionRepository.AddAsync(jobPosition);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

        public async Task<KscResult> UpdateHistoryJobPositionFromMisSyncData(Chart_JobPosition model)
        {
            var result = new KscResult();
            try
            {
                var jobpositionModel = model;

                var jobPosition = new Chart_JobPositionHistory();


                {
                    //jobPosition.JobIdentityId = jobpositionModel.JobIdentityId.Value;
                    jobPosition.JobPoisitionStatusId = jobpositionModel.JobPoisitionStatusId;
                    jobPosition.JobPositionNatureId = jobpositionModel.JobPositionNatureId;
                    jobPosition.StructureId = jobpositionModel.StructureId;
                    jobPosition.ParentId = jobpositionModel.ParentId;
                    jobPosition.JobPositionId = jobpositionModel.Id;

                    jobPosition.CapacityCount = (jobpositionModel.CapacityCount != null) ? Convert.ToInt32(jobpositionModel.CapacityCount) : 0;
                    //jobPosition.CategoryCoefficientId = 1;
                    jobPosition.Code = (jobpositionModel.Code != null) ? jobpositionModel.Code.Fa2En() : jobpositionModel.Code;
                    //jobPosition.CodeRelations = jobpositionModel.CodeRelations;
                    jobPosition.CofficientProximityProduct = (jobpositionModel.CofficientProximityProduct == null) ? 0 : Convert.ToDouble(jobpositionModel.CofficientProximityProduct);
                    jobPosition.CostCenter = (jobpositionModel.CostCenter == null) ? 0 : Convert.ToDecimal(jobpositionModel.CostCenter);
                    jobPosition.Description = (jobpositionModel.Description != null) ? jobpositionModel.Description.FixPersianChars() : jobpositionModel.Description;
                    jobPosition.EndDate = jobpositionModel.EndDate;
                    jobPosition.ExtraCount = (jobpositionModel.ExtraCount == null) ? 0 : Convert.ToInt32(jobpositionModel.ExtraCount);
                    jobPosition.UpdateDate = DateTime.Now;
                    jobPosition.UpdateUser = model.UpdateUser;
                    jobPosition.InsuranceJobCode = (jobpositionModel.InsuranceJobCode != null) ? jobpositionModel.InsuranceJobCode.Fa2En() : jobpositionModel.InsuranceJobCode;
                    jobPosition.IsActive = true;
                    jobPosition.IsOnCall = jobpositionModel.IsOnCall;
                    jobPosition.IsStrategic = jobpositionModel.IsStrategic;


                    //jobPosition.MisJobPositionCode = jobpositionModel.MisJobPositionCode;
                    jobPosition.PermissionExistCommodityCount = (jobpositionModel.PermissionExistCommodityCount == null) ? 0 : Convert.ToInt32(jobpositionModel.PermissionExistCommodityCount);
                    //jobPosition.ProductionEfficiencyId = 1;
                    jobPosition.RewardSpecificEfficincy = (jobpositionModel.RewardSpecificEfficincy == null) ? 0 : Convert.ToDouble(jobpositionModel.RewardSpecificEfficincy);
                    //jobPosition.SpecificRewardId = 6;
                    jobPosition.SpecificRewardId = (jobpositionModel.SpecificRewardId == null) ? null : Convert.ToInt32(jobpositionModel.SpecificRewardId);

                    jobPosition.StartDate = jobpositionModel.StartDate;
                    jobPosition.WorkingShiftOutsourcingCount = (jobpositionModel.WorkingShiftOutsourcingCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingShiftOutsourcingCount);
                    jobPosition.WorkingShiftCount = (jobpositionModel.WorkingShiftCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingShiftCount);
                    jobPosition.WorkingDayOutsourcingCount = (jobpositionModel.WorkingDayOutsourcingCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingDayOutsourcingCount);
                    jobPosition.WorkingDayCount = (jobpositionModel.WorkingDayCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingDayCount);
                    jobPosition.Title = (jobpositionModel.Title != null) ? jobpositionModel.Title.FixPersianChars() : jobpositionModel.Title;
                    jobPosition.TemporaryCount = (jobpositionModel.TemporaryCount == null) ? 0 : Convert.ToInt32(jobpositionModel.TemporaryCount);
                    jobPosition.SubstituteCount = (jobpositionModel.SubstituteCount == null) ? 0 : Convert.ToInt32(jobpositionModel.SubstituteCount);
                    jobPosition.StructureEndDate = jobpositionModel.StructureEndDate;
                    jobPosition.JobIdentityId = jobpositionModel.JobIdentityId;
                };

                await _kscHrUnitOfWork.JobPositionHistoryRepository.AddAsync(jobPosition);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

        public async Task<KscResult> UpdateJobPositionFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var jobpositionModel = model.InsertAndUpdateJobPositionViewModel;
                var jobPosition = _kscHrUnitOfWork.Chart_JobPositionRepository.GetChart_JobPositionByMisCode(jobpositionModel.MisJobPositionCode);
                if (jobPosition == null)
                {
                    result.AddError("", "کد موجود نمیباشد");
                    return result;
                }
                else
                {
                    await UpdateHistoryJobPositionFromMisSyncData(jobPosition);
                }
                if (jobpositionModel.JobIdentityCode != null)
                {
                    var jobIdentity = _kscHrUnitOfWork.Chart_JobIdentityRepository
                        .GetChart_JobIdentityByCode(jobpositionModel.JobIdentityCode);
                    if (jobIdentity != null)
                    {
                        var jobIdentityId = jobIdentity.Select(x => x.Id).FirstOrDefault();
                        jobPosition.JobIdentityId = jobIdentityId;

                    }
                }

                if (jobpositionModel.JobPoisitionStatus != null)
                {
                    var jobStatus = Convert.ToInt32(jobpositionModel.JobPoisitionStatus);
                    jobPosition.JobPoisitionStatusId = jobStatus;
                }

                if (jobpositionModel.JobPositionNatureCode != null)
                {
                    var obPositionNatureId = Convert.ToInt32(jobpositionModel.JobPositionNatureCode);
                    jobPosition.JobPositionNatureId = obPositionNatureId;
                }
                if (jobpositionModel.StructureCode != null)
                {
                    var Structure = _kscHrUnitOfWork.Chart_StructureRepository
                        .GetByMisStructureCode(jobpositionModel.StructureCode);

                    if (Structure != null)
                    {
                        var structureId = Structure.Select(x => x.Id).FirstOrDefault();
                        if (structureId != 0)
                        {
                            jobPosition.StructureId = structureId;
                        }

                    }
                }

                if (jobpositionModel.ParentCode != null)
                {
                    var parent = _kscHrUnitOfWork.Chart_JobPositionRepository
    .GetChart_JobPositionByMisCode(jobpositionModel.ParentCode);
                    //var parent = _kscHrUnitOfWork.Chart_JobPositionRepository
                    //    .GetChart_JobPositionParentByMisCode(jobpositionModel.ParentCode);

                    if (parent != null)
                    {
                        var parentId = parent.Id;
                        jobPosition.ParentId = parentId;


                        if (jobpositionModel.LevelNumber == null)
                        {
                            var levelParent = (parent.LevelNumber == null) ? 0 : parent.LevelNumber + 1;
                            jobPosition.LevelNumber = levelParent;
                        }

                    }

                }

                //if (jobpositionModel.LevelNumber != null)
                //{
                //    jobPosition.LevelNumber = Convert.ToInt32(jobpositionModel.LevelNumber);

                //}
                //else
                //{
                //    jobPosition.LevelNumber = 0;
                //}

                //if (jobpositionModel.SpecificRewardCode != null)
                //{
                //    jobPosition.SpecificRewardId = _kscHrUnitOfWork.Chart_JobPositionRepository.GetSpecificReward(jobpositionModel.SpecificRewardCode);
                //}


                {
                    jobPosition.CapacityCount = (jobpositionModel.CapacityCount != null) ? Convert.ToInt32(jobpositionModel.CapacityCount) : 0;
                    //jobPosition.CategoryCoefficientId = 1;
                    jobPosition.CofficientProximityProduct = (jobpositionModel.CofficientProximityProduct == null) ? 0 : Convert.ToDouble(jobpositionModel.CofficientProximityProduct);
                    jobPosition.CostCenter = (jobpositionModel.CostCenter == null) ? 0 : Convert.ToDecimal(jobpositionModel.CostCenter);
                    jobPosition.Description = (jobpositionModel.Description != null) ? jobpositionModel.Description.FixPersianChars() : jobpositionModel.Description;
                    jobPosition.EndDate = (jobpositionModel.EndDate == null) ? jobPosition.EndDate : DateTimeExtension.ShamsiToMiladiMIS(jobpositionModel.EndDate);
                    jobPosition.ExtraCount = (jobpositionModel.ExtraCount == null) ? 0 : Convert.ToInt32(jobpositionModel.ExtraCount);
                    jobPosition.UpdateDate = DateTime.Now;

                    jobPosition.UpdateUser = model.UserName;
                    jobPosition.InsuranceJobCode = (jobpositionModel.InsuranceJobCode != null) ? jobpositionModel.InsuranceJobCode.Fa2En() : jobpositionModel.InsuranceJobCode;
                    jobPosition.IsActive = true;
                    jobPosition.IsOnCall = (jobpositionModel.IsOnCall == null || jobpositionModel.IsOnCall == "0") ? false : true;
                    jobPosition.IsStrategic = (jobpositionModel.IsStrategic == null || jobpositionModel.IsStrategic == "0") ? false : true;
                    // jobPosition.LevelNumber = 0;

                    jobPosition.MisJobPositionCode = (jobpositionModel.MisJobPositionCode != null) ? jobpositionModel.MisJobPositionCode.Fa2En() : jobpositionModel.MisJobPositionCode;
                    jobPosition.PermissionExistCommodityCount = (jobpositionModel.PermissionExistCommodityCount == null) ? 0 : Convert.ToInt32(jobpositionModel.PermissionExistCommodityCount);
                    //jobPosition.ProductionEfficiencyId = 1;
                    jobPosition.RewardSpecificEfficincy = (jobpositionModel.RewardSpecificEfficincy == null) ? 0 : Convert.ToDouble(jobpositionModel.RewardSpecificEfficincy);
                    jobPosition.SpecificRewardId = (jobpositionModel.SpecificRewardCode == null) ? null : Convert.ToInt32(jobpositionModel.SpecificRewardCode);

                    jobPosition.StartDate = (jobpositionModel.StartDate == null) ? jobPosition.StartDate : DateTimeExtension.ShamsiToMiladiMIS(jobpositionModel.StartDate);
                    jobPosition.WorkingShiftOutsourcingCount = (jobpositionModel.WorkingShiftOutsourcingCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingShiftOutsourcingCount);
                    jobPosition.WorkingShiftCount = (jobpositionModel.WorkingShiftCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingShiftCount);
                    jobPosition.WorkingDayOutsourcingCount = (jobpositionModel.WorkingDayOutsourcingCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingDayOutsourcingCount);
                    jobPosition.WorkingDayCount = (jobpositionModel.WorkingDayCount == null) ? 0 : Convert.ToInt32(jobpositionModel.WorkingDayCount);
                    jobPosition.Title = (jobpositionModel.Title != null) ? jobpositionModel.Title.FixPersianChars() : jobpositionModel.Title;
                    jobPosition.TemporaryCount = (jobpositionModel.TemporaryCount == null) ? 0 : Convert.ToInt32(jobpositionModel.TemporaryCount);
                    jobPosition.SubstituteCount = (jobpositionModel.SubstituteCount == null) ? 0 : Convert.ToInt32(jobpositionModel.SubstituteCount);
                    jobPosition.StructureEndDate = (jobpositionModel.StructureEndDate == null) ? jobPosition.StructureEndDate : DateTimeExtension.ShamsiToMiladiMIS(jobpositionModel.StructureEndDate);
                };

                _kscHrUnitOfWork.Chart_JobPositionRepository.Update(jobPosition);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

        public async Task<KscResult> DeleteJobPositionFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var jobpositionModel = model.InsertAndUpdateJobPositionViewModel;
                var jobPosition = _kscHrUnitOfWork.Chart_JobPositionRepository.GetChart_JobPositionByMisCode(jobpositionModel.MisJobPositionCode);
                if (jobPosition == null)
                {
                    result.AddError("", "کد موجود نمیباشد");
                    return result;
                }

                jobPosition.IsActive = false;

                _kscHrUnitOfWork.Chart_JobPositionRepository.Update(jobPosition);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }


        public async Task<KscResult> InsertJobIdentityFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var jobIdentityModel = model.InsertAndUpdateJobIdentityViewModel;

                var isExist = _kscHrUnitOfWork.Chart_JobIdentityRepository.GetChart_JobIdentityByCode(jobIdentityModel.Code).Any();
                if (isExist == true)
                {
                    result.AddError("", "کد تکراری میباشد");
                    return result;
                }

                var jobIdentity = new Chart_JobIdentity();
                var jobCategory = _kscHrUnitOfWork
                                        .JobCategoryRepository
                                        .GetJobCategoryByCode(jobIdentityModel.JobCategoryCode)
                                        .FirstOrDefault();

                if (jobCategory != null)
                {
                    jobIdentity.JobCategoryId = jobCategory.Id;
                }

                {
                    jobIdentity.Code = (jobIdentityModel.Code != null) ? jobIdentityModel.Code.Fa2En() : jobIdentityModel.Code;
                    jobIdentity.InsertDate = DateTime.Now;
                    jobIdentity.InsertUser = model.UserName;
                    jobIdentity.IsActive = true;
                    jobIdentity.Title = (jobIdentityModel.Title != null) ? jobIdentityModel.Title.FixPersianChars() : jobIdentityModel.Title;
                    jobIdentity.StartDate = (jobIdentityModel.StartDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(jobIdentityModel.StartDate);
                    jobIdentity.EndDate = (jobIdentityModel.EndDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(jobIdentityModel.EndDate);


                };

                await _kscHrUnitOfWork.Chart_JobIdentityRepository.AddAsync(jobIdentity);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

        public async Task<KscResult> UpdateJobIdentityFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {

                var jobIdentityModel = model.InsertAndUpdateJobIdentityViewModel;

                var jobIdentity = _kscHrUnitOfWork.Chart_JobIdentityRepository.GetChart_JobIdentityByCode(jobIdentityModel.Code).FirstOrDefault();
                if (jobIdentity == null)
                {
                    result.AddError("", "کد موجود نمیباشد");
                    return result;
                }

                var jobCategory = _kscHrUnitOfWork
                                        .JobCategoryRepository
                                        .GetJobCategoryByCode(jobIdentityModel.JobCategoryCode)
                                        .FirstOrDefault();

                if (jobCategory != null)
                {
                    jobIdentity.JobCategoryId = jobCategory.Id;
                }

                {
                    jobIdentity.Code = (jobIdentityModel.Code != null) ? jobIdentityModel.Code.Fa2En() : jobIdentityModel.Code;
                    jobIdentity.UpdateDate = DateTime.Now;
                    jobIdentity.UpdateUser = model.UserName;
                    jobIdentity.IsActive = true;
                    jobIdentity.Title = (jobIdentityModel.Title != null) ? jobIdentityModel.Title.FixPersianChars() : jobIdentityModel.Title;
                    jobIdentity.StartDate = (jobIdentityModel.StartDate == null) ? jobIdentity.StartDate : DateTimeExtension.ShamsiToMiladiMIS(jobIdentityModel.StartDate);
                    jobIdentity.EndDate = (jobIdentityModel.EndDate == null) ? jobIdentity.EndDate : DateTimeExtension.ShamsiToMiladiMIS(jobIdentityModel.EndDate);

                };

                _kscHrUnitOfWork.Chart_JobIdentityRepository.Update(jobIdentity);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }
        public async Task<KscResult> DeleteJobIdentityFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {

                var jobIdentityModel = model.InsertAndUpdateJobIdentityViewModel;

                var jobIdentity = _kscHrUnitOfWork.Chart_JobIdentityRepository.GetChart_JobIdentityByCode(jobIdentityModel.Code).FirstOrDefault();
                if (jobIdentity == null)
                {
                    result.AddError("", "کد موجود نمیباشد");
                    return result;
                }

                jobIdentity.UpdateDate = DateTime.Now;
                jobIdentity.UpdateUser = model.UserName;
                jobIdentity.IsActive = true;



                _kscHrUnitOfWork.Chart_JobIdentityRepository.Update(jobIdentity);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }


        public async Task<KscResult> InsertStructureFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var structureModel = model.InsertAndUpdateStructureViewModel;
                var structure = new Chart_Structure();

                var isExist = _kscHrUnitOfWork.Chart_StructureRepository.GetByMisStructureCode(structureModel.MisJobPositionCode).Any();
                if (isExist == true)
                {
                    result.AddError("", "کد تکراری میباشد");
                    return result;
                }
                if (structureModel.ParentCode != null)
                {
                    var parent = _kscHrUnitOfWork.Chart_StructureRepository.GetByMisStructureCode(structureModel.ParentCode).FirstOrDefault();
                    if (parent != null)
                    {
                        var parentId = parent.Id;
                        structure.ParentId = parentId;
                    }

                }

                {

                    var code = _kscHrUnitOfWork.Chart_StructureRepository.GetStructureCode();

                    structure.Code = code.ToString();


                    //structure.Code = structureModel.Code;
                    //structure.CodeRelations = structureModel.CodeRelations;
                    structure.EndDate = (structureModel.EndDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(structureModel.EndDate);
                    structure.InsertDate = DateTime.Now;
                    structure.InsertUser = model.UserName;
                    structure.IsActive = true;
                    //structure.LevelNumber = structureModel.LevelNumber;
                    structure.Title = structureModel.Title;
                    //structure.MaxCode = structureModel.MaxCode;
                    //structure.MinCode = structureModel.MinCode;
                    structure.MisJobPositionCode = structureModel.MisJobPositionCode;
                    //structure.OldCode = structureModel.OldCode;
                    structure.StartDate = (structureModel.StartDate == null) ? null : DateTimeExtension.ShamsiToMiladiMIS(structureModel.StartDate);

                    //برای کانورت
                    structure.StructureTypeId = 2;

                };

                await _kscHrUnitOfWork.Chart_StructureRepository.AddAsync(structure);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }


        public async Task<KscResult> UpdateStructureFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var structureModel = model.InsertAndUpdateStructureViewModel;

                var structure = _kscHrUnitOfWork.Chart_StructureRepository.GetByMisStructureCode(structureModel.MisJobPositionCode).FirstOrDefault();
                if (structure == null)
                {
                    result.AddError("", "این کد موجود نمیباشد");
                    return result;
                }
                if (structureModel.ParentCode != null)
                {
                    var parentId = _kscHrUnitOfWork.Chart_StructureRepository.GetByMisStructureCode(structureModel.ParentCode).Select(x => x.Id).FirstOrDefault();
                    structure.ParentId = parentId;
                }
                else
                {
                    structure.ParentId = null;
                }
                {
                    //structure.Code = structureModel.Code;
                    //structure.CodeRelations = structureModel.CodeRelations;
                    structure.EndDate = (structureModel.EndDate == null) ? structure.EndDate : DateTimeExtension.ShamsiToMiladiMIS(structureModel.EndDate);
                    structure.UpdateDate = DateTime.Now;
                    structure.UpdateUser = model.UserName;
                    structure.IsActive = true;
                    //structure.LevelNumber = structureModel.LevelNumber;
                    structure.Title = (structureModel.Title != null) ? structureModel.Title.FixPersianChars() : structureModel.Title;
                    //structure.MaxCode = structureModel.MaxCode;
                    //structure.MinCode = structureModel.MinCode;
                    structure.MisJobPositionCode = (structureModel.MisJobPositionCode != null) ? structureModel.MisJobPositionCode.Fa2En() : structureModel.MisJobPositionCode;
                    //structure.OldCode = structureModel.OldCode;
                    structure.StartDate = (structureModel.StartDate == null) ? structure.EndDate : DateTimeExtension.ShamsiToMiladiMIS(structureModel.StartDate);

                    //برای کانورت
                    structure.StructureTypeId = 2;

                };

                _kscHrUnitOfWork.Chart_StructureRepository.Update(structure);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

        public async Task<KscResult> DeleteStructureFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var structureModel = model.InsertAndUpdateStructureViewModel;

                var structure = _kscHrUnitOfWork.Chart_StructureRepository.GetByMisStructureCode(structureModel.MisJobPositionCode).FirstOrDefault();
                if (structure == null)
                {
                    result.AddError("", "این کد موجود نمیباشد");
                    return result;
                }

                {
                    structure.UpdateDate = DateTime.Now;
                    structure.UpdateUser = model.UserName;
                    structure.IsActive = true;
                };

                _kscHrUnitOfWork.Chart_StructureRepository.Update(structure);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }


        public async Task<KscResult> InsertScoreFromMisSyncData(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var scoreList = new List<Chart_JobPositionScore>();
                var scoreModel = model.InsertAndUpdateScoreViewModel;
                if (scoreModel.StartDate == null)
                {
                    result.AddError("", "تاریخ شروع باید وارد شود");
                    return result;
                }



                var jobPositionId = _kscHrUnitOfWork.JobPositionScoreRepository.GetJobposionIdByMisCode(scoreModel.MisJobPositionCode);



                var splitedStartDate = scoreModel.StartDate.Split('|');

                var splitedEndDate = scoreModel.EndDate.Split('|');

                var listError = new List<KscError>();

                var splitedScore = scoreModel.Score.Split('|');
                var splitedType = scoreModel.TypeId.Split('|');

                for (int i = 0; i < splitedType.Length - 1; i++)
                {
                    var sss = new ScoreViewModel();
                    sss.Score = splitedScore[i];
                    var code = Convert.ToInt32(splitedType[i]);
                    sss.TypeId = _kscHrUnitOfWork.Chart_JobPositionScoreTypeRepository.GetScoreTypeIdByCode(code);
                    sss.StartDate = splitedStartDate[i];
                    sss.EndDate = splitedEndDate[i];

                    if (sss.Score == null)
                    {
                        listError.Add(new KscError("", $"امتیاز {code}وارد نشده", 0));
                        continue;
                    }
                    scoreModel.ScoreViewModel.Add(sss);

                }


                var findedDoubleActive = scoreModel.ScoreViewModel.GroupBy(a => a.TypeId)
                    .Select(a => new
                    {
                        a.Key,
                        countActiveData = a.Count(a => a.EndDate == null || a.EndDate == "0")
                    });
                if (findedDoubleActive.Any(a => a.countActiveData > 1))
                {
                    result.AddError("", "نمیتوان برای امتیازات یکسان دو مورد فعال وجود داشته باشد");
                    return result;
                }

                foreach (var item in scoreModel.ScoreViewModel)
                {
                    var score = new Chart_JobPositionScore();
                    score.EndDate = (item.EndDate == null || item.EndDate == "0") ? null : DateTimeExtension.ShamsiToMiladiMIS(item.EndDate);
                    score.InsertDate = DateTime.Now;
                    score.InsertUser = model.UserName;

                    if (score.EndDate == null)
                    {
                        score.IsActive = true;
                    }
                    else
                    {
                        score.IsActive = false;
                    }
                    score.JobPositionId = jobPositionId;

                    score.StartDate = DateTimeExtension.ShamsiToMiladiMIS(item.StartDate).Value.Date;
                    score.Score = Convert.ToInt32(item.Score);
                    score.JobPositionScoreTypeId = Convert.ToInt32(item.TypeId);

                    if (score.Score == 0 || score.Score == null)
                    {
                        continue;
                    }

                    //var findScore= _kscHrUnitOfWork.JobPositionScoreRepository
                    //    .FindRepeateJobpositionScore(scoreModel.MisJobPositionCode, score.JobPositionScoreTypeId, score.StartDate, score.EndDate.Value, score.Score.Value);

                    //if (findScore == null)
                    //{
                    //  scoreList.Add(score);
                    //}
                    //else
                    //{


                    //}




                    var isExistScore = _kscHrUnitOfWork.JobPositionScoreRepository
                         .GetRepeateJobpositionScore(scoreModel.MisJobPositionCode, score.JobPositionScoreTypeId, score.StartDate, score.EndDate, score.Score.Value);



                    var jobPositionScore = _kscHrUnitOfWork.JobPositionScoreRepository.FindRepeateJobpositionScore(scoreModel.MisJobPositionCode, score.JobPositionScoreTypeId, score.StartDate, score.EndDate, score.Score.Value);
                    var jobPositionScoreDate = DateTimeExtension.ShamsiToMiladiMIS(item.EndDate);


                    if (isExistScore == true && item.EndDate == "0")
                    {
                        continue;

                    }
                    else if (score.Score > 0 && isExistScore == true && item.EndDate != "0")
                    {

                        if (jobPositionScore != null && jobPositionScore.EndDate != jobPositionScoreDate)
                        {
                            jobPositionScore.EndDate = DateTimeExtension.ShamsiToMiladiMIS(item.EndDate);
                            jobPositionScore.IsActive = false;
                            jobPositionScore.UpdateDate = DateTime.Now;
                            jobPositionScore.UpdateUser = model.UserName;
                            try
                            {
                                _kscHrUnitOfWork.JobPositionScoreRepository.Update(jobPositionScore);
                                await _kscHrUnitOfWork.SaveAsync();
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                        //else
                        //{
                        //    scoreList.Add(score);
                        //}

                    }
                    else if (score.Score > 0 && isExistScore == false)
                    {
                        scoreList.Add(score);
                    }
                }
                if (scoreList.Count > 0)
                {
                    await _kscHrUnitOfWork.JobPositionScoreRepository.AddRangeAsync(scoreList);
                    await _kscHrUnitOfWork.SaveAsync();

                }
                else
                {

                    result.AddErrors(listError);

                }

            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }





        public async Task<KscResult> InsertAndUpdateIncreasePercent(InsertAndUpdateJobPositionFromMisViewModel model)
        {
            var result = new KscResult();
            try
            {
                var scoreList = new List<Chart_JobPositionIncreasePercent>();
                var AddUppdatescoreList = new List<Chart_JobPositionIncreasePercent>();
                var scoreModel = model.InsertAndUpdateIncreasePercentViewModel;



                var jobposition = _kscHrUnitOfWork.Chart_JobPositionRepository.GetChart_JobPositionByMisCode(scoreModel.MisJobPositionCode.Split('|')[0]);
                var JobPositionIncreasePercent = _kscHrUnitOfWork.JobPositionIncreasePercentRepository.GetJobPositionIncreasePercentByCode(scoreModel.MisJobPositionCode);



                var splitedStartDate = scoreModel.StartDate.Split('|');

                var finalEnDate = new List<string>();
                var splitedEndDate = string.IsNullOrEmpty(scoreModel.EndDate) ? null : scoreModel.EndDate.Split('|');
                for(var a=0; a< splitedEndDate.Length - 1; a++)
                {
                    finalEnDate.Add(splitedEndDate[a]);
                }
                if (finalEnDate.Any())
                {
                    if (finalEnDate.Count(a => a == null || a == "0" || a == "") > 1)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "این پست نمیتواند دو رکورد فعال داشته باشد");
                    }
                    if (finalEnDate.Count(a => a == null || a == "0" || a == "") == 0)
                    {
                        throw new HRBusinessException(Validations.RepetitiveId, "این پست باید یک  رکورد فعال داشته باشد");
                    }
                }
                var listError = new List<KscError>();

                var CoefficientYearsDaySplited = scoreModel.CoefficientYearsDay.Split('|');
                var CoefficientYearsShiftsplited = scoreModel.CoefficientYearsShift.Split('|');

                for (int i = 0; i < splitedStartDate.Length-1; i++)
                {
                    var sss = new Chart_JobPositionIncreasePercent();
                    if (CoefficientYearsDaySplited[i] == null || CoefficientYearsDaySplited[i] == "0" || CoefficientYearsDaySplited[i] == "")
                    {
                        listError.Add(new KscError("", $"امتیاز روزکار وارد نشده", 0));
                        continue;
                    }
                    if (CoefficientYearsShiftsplited[i] == null || CoefficientYearsShiftsplited[i] == "0" || CoefficientYearsShiftsplited[i] == "")
                    {
                        listError.Add(new KscError("", $"امتیاز شیفت وارد نشده", 0));
                        continue;
                    }
                    if (splitedStartDate[i] == null || splitedStartDate[i] == "0" || splitedStartDate[i] == "")
                    {
                        listError.Add(new KscError("", $"تاریخ شروع ردیف {i} درست وارد نشده است", 0));
                        continue;
                    }

                    sss.JobPositioinId = jobposition.Id;
                    sss.CoefficientYearsDay = double.Parse(CoefficientYearsDaySplited[i]);
                    sss.CoefficientYearsShift = double.Parse(CoefficientYearsShiftsplited[i]);
                    sss.StartDate = (splitedStartDate[i] == null || splitedStartDate[i] == "0" || splitedStartDate[i] == "") ? null : DateTimeExtension.ShamsiToMiladiMIS(splitedStartDate[i]);
                    sss.EndDate = (finalEnDate[i] == null || finalEnDate[i] == "0" || finalEnDate[i] == "") ? null : DateTimeExtension.ShamsiToMiladiMIS(splitedEndDate[i]);
                    sss.IsActive = (finalEnDate[i] == null || finalEnDate[i] == "0" || finalEnDate[i] == "") ? true : false;
                    sss.InsertUser = "SyncIncrease";
                    sss.InsertDate = DateTime.Now.Date;

                    if (JobPositionIncreasePercent.Any())
                    {
                        var isExistScore = _kscHrUnitOfWork.JobPositionIncreasePercentRepository
                        .GetRepeateJobpositionScore(sss.JobPositioinId,
                        sss.CoefficientYearsDay.Value,
                        sss.CoefficientYearsShift.Value,
                        sss.StartDate.Value.Date, sss.EndDate);



                        var jobPositionScore = _kscHrUnitOfWork.JobPositionIncreasePercentRepository
                            .FindRepeateJobpositionScore(sss.JobPositioinId
                                , sss.CoefficientYearsDay.Value
                                , sss.CoefficientYearsShift.Value,
                        sss.StartDate.Value.Date,
                        sss.EndDate);


                        var HasCheckScore = sss.CoefficientYearsDay > 0 || sss.CoefficientYearsShift > 0;

                        if (isExistScore == true && (splitedEndDate[i] == null || splitedEndDate[i] == "0" || splitedEndDate[i] == ""))
                        {
                            continue;

                        }
                        else if (HasCheckScore && isExistScore == true && sss.EndDate.HasValue)
                        {

                            if (jobPositionScore != null)
                            {
                                if (!jobPositionScore.EndDate.HasValue && sss.EndDate.HasValue)
                                {
                                    if (sss.StartDate.Value.Date > sss.EndDate.Value.Date)
                                    {
                                        listError.Add(new KscError("", $"تاریخ شروع ردیف {i} درست وارد نشده است", 0));
                                        continue;
                                    }

                                    jobPositionScore.EndDate = sss.EndDate;
                                    jobPositionScore.IsActive = false;
                                    jobPositionScore.UpdateDate = DateTime.Now;
                                    jobPositionScore.UpdateUser = model.UserName;
                                    try
                                    {
                                        AddUppdatescoreList.Add(jobPositionScore);
                                        //await _kscHrUnitOfWork.SaveAsync();
                                    }
                                    catch (Exception)
                                    {
                                        listError.Add(new KscError("", $"بروزرسانی ردیف {i} انجام نشد", 0));
                                        continue;
                                    }

                                }
                            }

                        }
                        else if (HasCheckScore && isExistScore == false)
                        {
                            scoreList.Add(sss);
                        }
                    }
                    else
                    {
                        scoreList.Add(sss);
                    }
                }
                if (listError.Count > 0)
                {
                    result.AddErrors(listError);
                }
                else if (scoreList.Count > 0)
                {
                    foreach (var score in scoreList)
                    {
                        _kscHrUnitOfWork.JobPositionIncreasePercentRepository.Add(score);
                    }
                    foreach (var score in AddUppdatescoreList)
                    {
                        _kscHrUnitOfWork.JobPositionIncreasePercentRepository.Update(score);
                    }

                     await _kscHrUnitOfWork.SaveAsync();

                }
                else
                {

                    throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود");

                }


            }
            catch (Exception ex)
            {
                throw new HRBusinessException(Validations.RepetitiveId, "عملیات نا موفق بود" + ex.Message);
            }
            return result;
        }

     
    }
}
