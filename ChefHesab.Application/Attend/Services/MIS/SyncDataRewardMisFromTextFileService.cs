using AutoMapper;
using DNTPersianUtils.Core;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Resources.Messages;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.Model.Vacation;
using Ksc.IndustrialAccounting.Shared.Utility;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.MIS
{
    public class SyncDataRewardMisFromTextFileService : ISyncDataRewardMisFromTextFileService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public SyncDataRewardMisFromTextFileService(IMapper mapper, IFilterHandler filterHandler, IKscHrUnitOfWork kscHrUnitOfWork)
        {
            _FilterHandler = filterHandler;
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;

        }

        /// <summary>
        /// RewardBaseHeader&RewardBaseDetail From Mis
        /// </summary>
        /// <returns></returns>
        public async Task<KscResult> SyncRewardBaseHeaderAndDetailFromMis()
        {
            var result = new KscResult<byte[]>();
            try
            {
                //مسیر لاگ خطا در سیستم خودمون
                //string path = @"D:\\Reward\RewardBaseHeaderError.txt";

                //Byte[] dataRewardBaseHeader = System.IO.File.ReadAllBytes(@"D:\\Reward\RewardBaseHeader.TXT");
                //Byte[] dataRewardBaseDetail = System.IO.File.ReadAllBytes(@"D:\\Reward\RewardBaseDetail.TXT");
                Byte[] dataRewardBaseHeader = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتا جدول مبنا-14031019\RewardBaseHeader.TXT");
                Byte[] dataRewardBaseDetail = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتا جدول مبنا-14031019\RewardBaseDetail.TXT");

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var _streamDataRewardBaseHeader = new System.IO.StreamReader(new System.IO.MemoryStream(dataRewardBaseHeader), System.Text.Encoding.GetEncoding("windows-1256"));
                var today = DateTime.Now;
                var currentUser = "MisSync";
                string lineHeader;
                string lineDetail;
                List<RewardBaseHeader> RewardBaseHeaderList = new List<RewardBaseHeader>();


                while ((lineHeader = _streamDataRewardBaseHeader.ReadLine()) != null)
                {

                    var _splitedHeader = lineHeader.Split('|').ToArray();
                    RewardBaseHeader rewardBaseHeader = new RewardBaseHeader();
                    rewardBaseHeader.Year = int.Parse(_splitedHeader[0]);
                    rewardBaseHeader.IncreasedPercent = double.Parse(_splitedHeader[1].Trim().Replace(".", "/"));
                    rewardBaseHeader.ValidStartDate = int.Parse(_splitedHeader[2].Substring(0, 6));
                    rewardBaseHeader.ValidEndDate = int.Parse(_splitedHeader[3].Substring(0, 6)) == 0 ? null : int.Parse(_splitedHeader[3].Substring(0, 6));
                    rewardBaseHeader.InsertUser = currentUser;
                    rewardBaseHeader.InsertDate = today;
                    var _streamDataRewardBasDetail = new System.IO.StreamReader(new System.IO.MemoryStream(dataRewardBaseDetail), System.Text.Encoding.GetEncoding("windows-1256"));

                    //
                    while ((lineDetail = _streamDataRewardBasDetail.ReadLine()) != null)
                    {
                        var _splitedDetail = lineDetail.Split('|').ToArray();
                        var yearDetail = _splitedDetail[0];
                        var yearHeader = _splitedHeader[0];
                        var percentDetail = _splitedDetail[1];
                        var percentHeader = _splitedHeader[1];

                        if (_splitedDetail[0] == _splitedHeader[0] && _splitedDetail[1] == _splitedHeader[1])
                        {
                            RewardBaseDetail rewardBaseDetail = new RewardBaseDetail();
                            rewardBaseDetail.RewardUnitTypeId = int.Parse(_splitedDetail[3].Trim());
                            rewardBaseDetail.MinimumOfUnit = int.Parse(_splitedDetail[4].Trim());
                            rewardBaseDetail.MaximumOfUnit = int.Parse(_splitedDetail[5].Trim());
                            rewardBaseDetail.MisKey = _splitedDetail[2];
                            rewardBaseDetail.BaseAmount = double.Parse(_splitedDetail[6].Trim().Replace(".", "/"));
                            rewardBaseDetail.IsActive = true;
                            rewardBaseDetail.IsAllBaseMonthlyProductionReward = _splitedDetail[9].Contains("PART") ? false : true;
                            rewardBaseDetail.BaseMonthlyProductionReward = string.IsNullOrWhiteSpace(_splitedDetail[7]) ? null : long.Parse(_splitedDetail[7].Trim());
                            rewardBaseDetail.FinalMonthlyProductionReward = string.IsNullOrWhiteSpace(_splitedDetail[8]) ? null : long.Parse(_splitedDetail[8].Trim());
                            rewardBaseHeader.RewardBaseDetails.Add(rewardBaseDetail);
                            RewardBaseHeaderList.Add(rewardBaseHeader);
                        }
                    }
                    //
                }

                await _kscHrUnitOfWork.RewardBaseHeaderRepository.AddRangeAsync(RewardBaseHeaderList);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }
            return result;
        }
        public async Task<KscResult> SyncRewardInFromMis()
        {
            var result = new KscResult<byte[]>();
            try
            {
                var productionEfficiencyList = _kscHrUnitOfWork.ProductionEfficiencyRepository.GetAll().ToList();
                //مسیر لاگ خطا در سیستم خودمون
                //string path = @"D:\\Reward\RewardBaseHeaderError.txt";
                Byte[] dataRewardIn = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RewardIn.TXT");
                Byte[] dataRDailySale = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RDailySale.TXT");
                Byte[] dataRDri = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RDri.TXT");
                Byte[] dataRMonthlySale = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RMonthlySale.TXT");
                Byte[] dataRPelet = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RPelet.TXT");
                Byte[] dataRProduction = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RProduction.TXT");
                Byte[] dataRSmcDaily = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RSmcDaily.TXT");
                Byte[] dataRSmcMonthly = System.IO.File.ReadAllBytes(@"\\ecln0013\HR Share\دیتا\دیتا پاداش\دیتاREWARD-IN-14031024\RSmcMonthly.TXT");


                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var _streamDataRewardIn = new System.IO.StreamReader(new System.IO.MemoryStream(dataRewardIn), System.Text.Encoding.GetEncoding("windows-1256"));
                var today = DateTime.Now;
                var currentUser = "MisSync";
                string lineRewardIn;
                string lineData;

                List<RewardIn> RewardInList = new List<RewardIn>();
                var allRewardIn = _kscHrUnitOfWork.RewardInRepository.GetAll().ToList();

                while ((lineRewardIn = _streamDataRewardIn.ReadLine()) != null)
                {

                    //rewardIn
                    var _splitedRewardIn = lineRewardIn.Split('|').ToArray();
                    RewardIn rewardIn = new RewardIn();
                    rewardIn.YearMonth = int.Parse(_splitedRewardIn[0]);
                    if (allRewardIn.Any(x => x.YearMonth == rewardIn.YearMonth))
                    {
                        continue;
                    }
                    rewardIn.ProductionExecutionUser = _splitedRewardIn[1].Trim();
                    rewardIn.ProductionExecutionDate = GetFormattedShamsiDate(_splitedRewardIn[2]).ToGregorianDateTime();
                    rewardIn.QualityControlExecutionUser = _splitedRewardIn[3].Trim();
                    rewardIn.QualityControlExecutionDate = GetFormattedShamsiDate(_splitedRewardIn[4]).ToGregorianDateTime();
                    rewardIn.ExecutionUser = _splitedRewardIn[5].Trim();
                    rewardIn.ExecutionDate = GetFormattedShamsiDate(_splitedRewardIn[6]).ToGregorianDateTime();
                    ////RewardInQualityControlMonthlyProduction
                    var _streamDataRProduction = new System.IO.StreamReader(new System.IO.MemoryStream(dataRProduction), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRProduction.ReadLine()) != null)
                    {
                        var _splitedline = lineData.Split('|').ToArray();

                        if (_splitedline[0] == _splitedRewardIn[0])
                        {
                            RewardInQualityControlMonthlyProduction rewardInQualityControlMonthlyProduction = new RewardInQualityControlMonthlyProduction();
                            rewardInQualityControlMonthlyProduction.QualityControlProductionWeghit = long.Parse(_splitedline[1]);
                            rewardInQualityControlMonthlyProduction.AooWeight = long.Parse(_splitedline[2]);
                            rewardInQualityControlMonthlyProduction.SabadWeight = long.Parse(_splitedline[3]);
                            rewardInQualityControlMonthlyProduction.OkWeight = long.Parse(_splitedline[4]);
                            rewardInQualityControlMonthlyProduction.NotOkWeight = long.Parse(_splitedline[5]);
                            rewardInQualityControlMonthlyProduction.BloomIncreasedWeight = long.Parse(_splitedline[6]);
                            rewardInQualityControlMonthlyProduction.InRewWeight = long.Parse(_splitedline[7]);
                            rewardInQualityControlMonthlyProduction.QualityFactory = double.Parse(_splitedline[8].Replace(".", "/"));
                            rewardIn.RewardInQualityControlMonthlyProductions.Add(rewardInQualityControlMonthlyProduction);
                        }
                    }
                    ////RewardInQualityControlMonthlyDri
                    var _streamDataRDri = new System.IO.StreamReader(new System.IO.MemoryStream(dataRDri), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRDri.ReadLine()) != null)
                    {
                        var _splitedline = lineData.Split('|').ToArray();

                        if (_splitedline[0] == _splitedRewardIn[0])
                        {
                            RewardInQualityControlMonthlyDri rewardInQualityControlMonthlyDri = new RewardInQualityControlMonthlyDri();
                            int? productionEfficiencyCode = int.Parse(_splitedline[1]) == 0 ? null : int.Parse(_splitedline[1]);
                            if (productionEfficiencyCode != null)
                            {
                                var productionEfficiency = productionEfficiencyList.FirstOrDefault(x => x.Code == productionEfficiencyCode);
                                rewardInQualityControlMonthlyDri.ProductionEfficiencyId = productionEfficiency.Id;
                            }
                            rewardInQualityControlMonthlyDri.QualityPercent = double.Parse(_splitedline[2].Replace(".", "/"));
                            rewardInQualityControlMonthlyDri.MetalPercent = double.Parse(_splitedline[3].Replace(".", "/"));
                            rewardInQualityControlMonthlyDri.CSuggestionFactorDri = double.Parse(_splitedline[4].Replace(".", "/"));
                            rewardInQualityControlMonthlyDri.PlantName = _splitedline[5].Trim();
                            rewardIn.RewardInQualityControlMonthlyDris.Add(rewardInQualityControlMonthlyDri);
                        }
                    }
                    ////RewardInQualityControlMonthlyPelet
                    var _streamDataRPelet = new System.IO.StreamReader(new System.IO.MemoryStream(dataRPelet), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRPelet.ReadLine()) != null)
                    {
                        var _splitedline = lineData.Split('|').ToArray();

                        if (_splitedline[0] == _splitedRewardIn[0])
                        {
                            RewardInQualityControlMonthlyPelet rewardInQualityControlMonthlyPelet = new RewardInQualityControlMonthlyPelet();
                            //
                            int? productionEfficiencyCode = int.Parse(_splitedline[1]) == 0 ? null : int.Parse(_splitedline[1]);
                            if (productionEfficiencyCode != null)
                            {
                                var productionEfficiency = productionEfficiencyList.FirstOrDefault(x => x.Code == productionEfficiencyCode);
                                rewardInQualityControlMonthlyPelet.ProductionEfficiencyId = productionEfficiency.Id;
                            }
                            //
                            rewardInQualityControlMonthlyPelet.B4Percent = double.Parse(_splitedline[2].Replace(".", "/"));
                            rewardInQualityControlMonthlyPelet.AiPercent = double.Parse(_splitedline[3].Replace(".", "/"));
                            rewardInQualityControlMonthlyPelet.PorosPercent = double.Parse(_splitedline[4].Replace(".", "/"));
                            rewardInQualityControlMonthlyPelet.CcsPercent = int.Parse(_splitedline[5]);
                            rewardInQualityControlMonthlyPelet.SizePercent = double.Parse(_splitedline[6].Replace(".", "/"));
                            rewardInQualityControlMonthlyPelet.RawPeletPercent = double.Parse(_splitedline[7].Replace(".", "/"));
                            rewardInQualityControlMonthlyPelet.CSuggestionFactorPelet = double.Parse(_splitedline[8].Replace(".", "/"));
                            rewardInQualityControlMonthlyPelet.PeletPlantName = _splitedline[9].Trim();

                            rewardIn.RewardInQualityControlMonthlyPelets.Add(rewardInQualityControlMonthlyPelet);
                        }
                    }
                    //RewardInSmcDailyProduction
                    var _streamDataRSmcDaily = new System.IO.StreamReader(new System.IO.MemoryStream(dataRSmcDaily), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRSmcDaily.ReadLine()) != null)
                    {
                        var _splitedlineSales = lineData.Split('|').ToArray();

                        if (_splitedlineSales[0] == _splitedRewardIn[0])
                        {
                            RewardInSmcDailyProduction rewardInSmcDailyProduction = new RewardInSmcDailyProduction();
                            rewardInSmcDailyProduction.SmcDay = int.Parse(_splitedlineSales[1]);
                            rewardInSmcDailyProduction.ProductionSmcWeight = long.Parse(_splitedlineSales[2]);
                            rewardInSmcDailyProduction.PlanSmcWeight = long.Parse(_splitedlineSales[3]);
                            rewardInSmcDailyProduction.HeatNumber = int.Parse(_splitedlineSales[4]);
                            rewardInSmcDailyProduction.EafNumber = int.Parse(_splitedlineSales[5]);
                            rewardIn.RewardInSmcDailyProductions.Add(rewardInSmcDailyProduction);
                        }
                    }
                    //RewardInSmcMonthlyProduction
                    var _streamDataRSmcMonthly = new System.IO.StreamReader(new System.IO.MemoryStream(dataRSmcMonthly), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRSmcMonthly.ReadLine()) != null)
                    {
                        var _splitedlineSales = lineData.Split('|').ToArray();

                        if (_splitedlineSales[0] == _splitedRewardIn[0])
                        {
                            RewardInSmcMonthlyProduction rewardInSmcMonthlyProduction = new RewardInSmcMonthlyProduction();
                            int? productionEfficiencyCode = int.Parse(_splitedlineSales[1]) == 0 ? null : int.Parse(_splitedlineSales[1]);
                            if (productionEfficiencyCode != null)
                            {
                                var productionEfficiency = productionEfficiencyList.FirstOrDefault(x => x.Code == productionEfficiencyCode);
                                rewardInSmcMonthlyProduction.ProductionEfficiencyId = productionEfficiency.Id;
                            }
                            rewardInSmcMonthlyProduction.ActualWeight = long.Parse(_splitedlineSales[2]);
                            rewardInSmcMonthlyProduction.PlanWeight = long.Parse(_splitedlineSales[3]);
                            rewardInSmcMonthlyProduction.PerformanceFactor = double.Parse(_splitedlineSales[4].Replace(".", "/"));
                            rewardInSmcMonthlyProduction.PerformanceSuggestionFactor = double.Parse(_splitedlineSales[5].Replace(".", "/"));
                            rewardInSmcMonthlyProduction.PlantName = _splitedlineSales[6].Trim();
                            rewardIn.RewardInSmcMonthlyProductions.Add(rewardInSmcMonthlyProduction);
                        }
                    }
                    //RewardInMonthlyProductionSale
                    var _streamDataRMonthlySale = new System.IO.StreamReader(new System.IO.MemoryStream(dataRMonthlySale), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRMonthlySale.ReadLine()) != null)
                    {
                        var _splitedlineSales = lineData.Split('|').ToArray();

                        if (_splitedlineSales[0] == _splitedRewardIn[0])
                        {
                            RewardInMonthlyProductionSale rewardInMonthlyProductionSale = new RewardInMonthlyProductionSale();
                            rewardInMonthlyProductionSale.IsgList = _splitedlineSales[1];
                            rewardInMonthlyProductionSale.IsgActualWeight = long.Parse(_splitedlineSales[2]);
                            rewardInMonthlyProductionSale.IsgPlanWeight = long.Parse(_splitedlineSales[3]);
                            rewardInMonthlyProductionSale.IsgPercent = double.Parse(_splitedlineSales[4].Replace(".", "/"));
                            rewardInMonthlyProductionSale.ProductionDescription = _splitedlineSales[5];
                            rewardIn.RewardInMonthlyProductionSales.Add(rewardInMonthlyProductionSale);
                        }
                    }
                    ////RewardInDailyProductionSales
                    var _streamDataRDailySale = new System.IO.StreamReader(new System.IO.MemoryStream(dataRDailySale), System.Text.Encoding.GetEncoding("windows-1256"));
                    while ((lineData = _streamDataRDailySale.ReadLine()) != null)
                    {
                        var _splitedlineSales = lineData.Split('|').ToArray();

                        if (_splitedlineSales[0] == _splitedRewardIn[0])
                        {
                            RewardInDailyProductionSale rewardInDailyProductionSale = new RewardInDailyProductionSale();
                            rewardInDailyProductionSale.DaySale = int.Parse(_splitedlineSales[1]);
                            rewardInDailyProductionSale.DailyProductionWeight = long.Parse(_splitedlineSales[2]);
                            rewardInDailyProductionSale.DailyPlanWeight = long.Parse(_splitedlineSales[3]);
                            rewardInDailyProductionSale.ProductionType = int.Parse(_splitedlineSales[4]);
                            rewardInDailyProductionSale.RewardUnitTypeId = int.Parse(_splitedlineSales[5]);

                            rewardIn.RewardInDailyProductionSales.Add(rewardInDailyProductionSale);

                        }
                    }
                    //
                    RewardInList.Add(rewardIn);

                }

                await _kscHrUnitOfWork.RewardInRepository.AddRangeAsync(RewardInList);
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                result.AddError("", ex.Message);
            }
            return result;
        }

        public string GetFormattedShamsiDate(string dateTimeShamsi)
        {
            var dateShamsiFormatted = dateTimeShamsi.Substring(0, 4) + "/" + dateTimeShamsi.Substring(4, 2) + "/" + dateTimeShamsi.Substring(6, 2);
            var timeFormatted = dateTimeShamsi.Substring(8, 2) + ":" + dateTimeShamsi.Substring(10, 2);
            return dateShamsiFormatted + " " + timeFormatted;
        }
    }
}
