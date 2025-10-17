using AutoMapper;
using DNTPersianUtils.Core;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.ShiftConcept;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class ShiftConceptDetailService : IShiftConceptDetailService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ShiftConceptDetailService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.ShiftConceptDetailRepository.Any(x => x.Id != id && x.Title == title);
            if (_kscHrUnitOfWork.ShiftConceptDetailRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.ShiftConceptDetailRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.ShiftConceptDetailRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        public void ExistsByCode(int id, string code)
        {
            //return _kscHrUnitOfWork.ShiftConceptDetailRepository.Any(x => x.Id != id && x.Code == code);
            if (_kscHrUnitOfWork.ShiftConceptDetailRepository.Any(x => x.Id != id && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddShiftConceptDetail(AddShiftConceptDetailModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;

            Exists(model.Id, model.Title);
            ExistsByCode(model.Id, model.Code);
            //
            if (string.IsNullOrEmpty(model.DurationTimeBeforeShiftStartTime))
            {
                result.AddError("رکورد نامعتبر", "مدت زمان قبل از شروع شیفت  اجباری می باشد");
                return result;
            }
            if (string.IsNullOrEmpty(model.DurationTimeAfterShiftEndTime))
            {
                result.AddError("رکورد نامعتبر", "مدت زمان بعد از پایان شیفت  اجباری می باشد");
                return result;
            }
            //model.DurationTimeAfterShiftEndTime = model.DurationTimeAfterShiftEndTime.ToEnglishNumbers();
            //model.DurationTimeBeforeShiftStartTime = model.DurationTimeBeforeShiftStartTime.ToEnglishNumbers();
            //var DurationTimeBeforeShiftStartTime = int.Parse(model.DurationTimeBeforeShiftStartTime.Replace(":", ""));
            //var DurationTimeAfterShiftEndTime = int.Parse(model.DurationTimeAfterShiftEndTime.Replace(":", ""));
            //if (DurationTimeBeforeShiftStartTime > DurationTimeAfterShiftEndTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  مدت زمان قبل از شروع شیفت نمیتواند بزرگتر از مدت زمان بعد از پایان شیفت باشد ");
            //    return result;
            //}
            //

            var ShiftConceptDetail = _mapper.Map<ShiftConceptDetail>(model);
            //ShiftConceptDetail.InsertDate = DateTime.Now;
            //ShiftConceptDetail.InsertUser = model.CurrentUserName;
            //ShiftConceptDetail.IsActive = true;
            //ShiftConceptDetail.DomainName = model.DomainName;
            ShiftConceptDetail.Code = ShiftConceptDetail.Id.ToString();
            _kscHrUnitOfWork.ShiftConceptDetailRepository.Add(ShiftConceptDetail);
            await _kscHrUnitOfWork.SaveAsync();
            var ShiftConceptDetailEdit = GetOne(ShiftConceptDetail.Id);
            ShiftConceptDetailEdit.Code = ShiftConceptDetailEdit.Id.ToString();
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public async Task<KscResult> UpdateShiftConceptDetail(EditShiftConceptDetailModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;

            Exists(model.Id, model.Title);
            ExistsByCode(model.Id, model.Code);

            //
            if (string.IsNullOrEmpty(model.DurationTimeBeforeShiftStartTime))
            {
                result.AddError("رکورد نامعتبر", "مدت زمان قبل از شروع شیفت  اجباری می باشد");
                return result;
            }
            if (string.IsNullOrEmpty(model.DurationTimeAfterShiftEndTime))
            {
                result.AddError("رکورد نامعتبر", "مدت زمان بعد از پایان شیفت  اجباری می باشد");
                return result;
            }
            //model.DurationTimeAfterShiftEndTime = model.DurationTimeAfterShiftEndTime.ToEnglishNumbers();
            //model.DurationTimeBeforeShiftStartTime = model.DurationTimeBeforeShiftStartTime.ToEnglishNumbers();
            //var DurationTimeBeforeShiftStartTime = int.Parse(model.DurationTimeBeforeShiftStartTime.Replace(":", ""));
            //var DurationTimeAfterShiftEndTime = int.Parse(model.DurationTimeAfterShiftEndTime.Replace(":", ""));
            //if (DurationTimeBeforeShiftStartTime > DurationTimeAfterShiftEndTime)
            //{
            //    result.AddError("رکورد نامعتبر", "  مدت زمان قبل از شروع شیفت نمیتواند بزرگتر از مدت زمان بعد از پایان شیفت باشد ");
            //    return result;
            //}
            //
            var oneShiftConceptDetail = GetOne(model.Id);
            if (oneShiftConceptDetail == null)
            {
                result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                return result;
            }
            oneShiftConceptDetail.Code = model.Code;
            oneShiftConceptDetail.Title = model.Title;
            oneShiftConceptDetail.ShiftConceptId = model.ShiftConceptId;

            oneShiftConceptDetail.OncallCheckBeforeShiftStart = model.OncallCheckBeforeShiftStart;
            oneShiftConceptDetail.OncallCheckAfterShiftEnd = model.OncallCheckAfterShiftEnd;
            oneShiftConceptDetail.OncallCheckAfterShiftStart = model.OncallCheckAfterShiftStart;
            oneShiftConceptDetail.OncallCheckFree = model.OncallCheckFree;
            //
            //
            oneShiftConceptDetail.DurationTimeAfterShiftEndTime = model.DurationTimeAfterShiftEndTime.ToEnglishNumbers();
            oneShiftConceptDetail.DurationTimeBeforeShiftStartTime = model.DurationTimeBeforeShiftStartTime.ToEnglishNumbers();

            //oneShiftConceptDetail.DurationTimeBeforeShiftStartTime = model.DurationTimeBeforeShiftStartTime;
            //oneShiftConceptDetail.DurationTimeAfterShiftEndTime = model.DurationTimeAfterShiftEndTime;

            oneShiftConceptDetail.UpdateDate = DateTime.Now;
            oneShiftConceptDetail.UpdateUser = model.CurrentUserName;
            oneShiftConceptDetail.IsActive = model.IsActive;
            oneShiftConceptDetail.Code = oneShiftConceptDetail.Id.ToString();
            await _kscHrUnitOfWork.SaveAsync();
            return result;

        }
        public async Task<KscResult> RemoveShiftConceptDetail(EditShiftConceptDetailModel model)
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

            //item.IsActive = false;
            item.UpdateDate = DateTime.Now;
            item.UpdateUser = model.CurrentUserName;

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<ShiftConceptDetailModel> GetShiftConceptDetail()
        {
            var cities = _kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable();
            return _mapper.Map<List<ShiftConceptDetailModel>>(cities);

        }
        public FilterResult<ShiftConceptDetailModel> GetShiftConceptDetailByFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftConceptDetail>(_kscHrUnitOfWork.ShiftConceptDetailRepository.GetAllQueryable().AsQueryable().Include(a => a.ShiftConcept).Where(a => a.IsActive).AsQueryable(), Filter, "Id");

            var ShiftConceptDetailsFiltred = result.Data.Select(a => new ShiftConceptDetailModel()
            {
                ShiftConceptId = a.ShiftConceptId,
                Id = a.Id,
                Code = a.Code,
                ShiftConceptTitle = a.ShiftConcept.Title,
                ShiftConceptCode = a.ShiftConcept.Code,
                Title = a.Title,
                OncallCheckBeforeShiftStart = a.OncallCheckBeforeShiftStart,
                OncallCheckAfterShiftEnd = a.OncallCheckAfterShiftEnd,
                OncallCheckAfterShiftStart = a.OncallCheckAfterShiftStart,
                OncallCheckFree = a.OncallCheckFree,
                DurationTimeBeforeShiftStartTime = a.DurationTimeBeforeShiftStartTime,
                DurationTimeAfterShiftEndTime = a.DurationTimeAfterShiftEndTime,
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser
            }).ToList();

            return new FilterResult<ShiftConceptDetailModel>()
            {
                Data = ShiftConceptDetailsFiltred,
                Total = result.Total

            };
        }


        public ShiftConceptDetail GetOne(int id)
        {
            return _kscHrUnitOfWork.ShiftConceptDetailRepository.FirstOrDefault(a => a.Id == id && a.IsActive);
        }

        public EditShiftConceptDetailModel GetForEdit(int id)
        {
            var model = GetOne(id);
            return _mapper.Map<EditShiftConceptDetailModel>(model);
        }

        public List<SearchShiftConceptDetailModel> GetWorkByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ShiftConceptDetail>(_kscHrUnitOfWork.ShiftConceptDetailRepository
                .WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchShiftConceptDetailModel>>(result.Data.ToList());
        }
        public async Task<SearchShiftConceptDetailModel> GetShiftConceptDetailWithWOrkGroupIdDate(int workGroupId, int workCalendarId)
        {
            ShiftConceptDetail shiftConceptDetail = new ShiftConceptDetail();
            var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupByRelations(workGroupId);
            if (workGroup.WorkTime.ShiftSettingFromShiftboard)
            {
                var ShiftBoard = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIdWorkCalendarId(workGroupId, workCalendarId);
                if (ShiftBoard != null)
                    shiftConceptDetail = ShiftBoard.ShiftConceptDetail;

            }
            else
            {
                var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                if (workTimeShiftConcept != null)
                    shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.FirstOrDefault(x => x.IsActive == true);
            }
            if (shiftConceptDetail == null)
                return new SearchShiftConceptDetailModel();
            return _mapper.Map<SearchShiftConceptDetailModel>(shiftConceptDetail);
        }

        public List<SearchShiftConceptDetailModel> GetShiftConceptDetailWithWOrkGroupIdAndWokCalendarIds(List<int> workGroupIds, List<int> workCalendarIds)
        {
            List<SearchShiftConceptDetailModel> shiftConceptDetails = new List<SearchShiftConceptDetailModel>();
            var workGroups = _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByRelations(workGroupIds).ToList();
            var shiftBoards = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIdsAndWorkCalendarIds(workGroupIds, workCalendarIds).ToList();
            foreach (var workGroup in workGroups)
            {
                ShiftConceptDetail shiftConceptDetail = new ShiftConceptDetail();
                foreach (var workCalendarId in workCalendarIds)
                {
                    if (workGroup.WorkTime.ShiftSettingFromShiftboard)
                    {
                        var ShiftBoard = shiftBoards.FirstOrDefault(a => a.WorkCalendarId == workCalendarId && a.WorkGroupId == workGroup.Id);
                        if (ShiftBoard != null)
                            shiftConceptDetail = ShiftBoard.ShiftConceptDetail;

                    }
                    else
                    {
                        var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                        if (workTimeShiftConcept != null)
                            shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.FirstOrDefault(x => x.IsActive == true);
                    }
                    if (shiftConceptDetail == null)
                        continue;

                    var newItem = new SearchShiftConceptDetailModel()
                    {
                        WorkCalendarId = workCalendarId,
                        WorkGroupId = workGroup.Id,
                        ShiftConceptId = shiftConceptDetail.ShiftConceptId,
                        Id = shiftConceptDetail.Id,
                        Title = shiftConceptDetail.Title,
                        Code = shiftConceptDetail.Code,
                    };
                    shiftConceptDetails.Add(newItem);
                }
            }
            return shiftConceptDetails;

        }

        //list
        public async Task<List<SearchShiftConceptModel>> GetShiftConceptDetailWithWOrkGroupIdDate(List<int> workGroupId, int workCalendarId)
        {
            List<SearchShiftConceptModel> listShiftConceptDetail = new List<SearchShiftConceptModel>();
            // var workGroup =await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByRelations(workGroupId).ToListAsync().ConfigureAwait(false);
            var workGroup = _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupsByRelations(workGroupId).ToList();
            //var shiftBoards =await  _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIds(workGroupId, workCalendarId).ToListAsync().ConfigureAwait(false);
            var shiftBoards = _kscHrUnitOfWork.ShiftBoardRepository.GetShiftBoardByworkGoupIds(workGroupId, workCalendarId).ToList();
            foreach (var group in workGroup)
            {
                ShiftConceptDetail shiftConceptDetail = new ShiftConceptDetail();
                var model = new SearchShiftConceptDetailModel();
                if (group.WorkTime.ShiftSettingFromShiftboard)
                {

                    var ShiftBoard = shiftBoards.FirstOrDefault(a => a.WorkGroupId == group.Id);
                    if (ShiftBoard != null)
                        shiftConceptDetail = ShiftBoard.ShiftConceptDetail;

                }
                else
                {
                    var workTimeShiftConcept = group.WorkTime.WorkTimeShiftConcepts.FirstOrDefault(x => x.IsActive == true);
                    if (workTimeShiftConcept != null)
                        shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.FirstOrDefault(x => x.IsActive == true);
                }
                if (shiftConceptDetail == null) continue;
                var item = new SearchShiftConceptModel()
                {
                    Code = shiftConceptDetail.Code,
                    Title = shiftConceptDetail.Title,
                    Id = shiftConceptDetail.Id,
                    WorkGroupCode = group.Code,
                    ShiftConceptCode = shiftConceptDetail.ShiftConcept?.Code,
                    ShiftConceptId = shiftConceptDetail.ShiftConceptId,
                    IsRest = shiftConceptDetail.ShiftConcept?.IsRest
                };
                listShiftConceptDetail.Add(item);

            }
            return listShiftConceptDetail;
        }

        public async Task<FilterResult<ShiftConceptDetailModel>> GetShiftConceptDetailListWithWOrkGroupId(SearchShiftConceptDetailModel fillter)
        {

            List<ShiftConceptDetailModel> datalist = new List<ShiftConceptDetailModel>();
            var workGroup = await _kscHrUnitOfWork.WorkGroupRepository.GetWorkGroupByRelations(fillter.WorkGroupId);
            if (workGroup.WorkTime.ShiftSettingFromShiftboard)
            {
                var shiftConceptList = _kscHrUnitOfWork.ShiftConceptDetailRepository
                    .WhereQueryable(a => a.ShiftBoards.Any(c => c.WorkGroup.IsActive && c.WorkGroup.RepetitionPeriod == workGroup.RepetitionPeriod
                    &&
                    c.WorkGroup.WorkTimeId == workGroup.WorkTimeId
                    && c.WorkCalendarId == fillter.WorkCalendarId)).ToList();

                datalist = _mapper.Map<List<ShiftConceptDetailModel>>(shiftConceptList);
            }
            else
            {
                var workTimeShiftConcept = workGroup.WorkTime.WorkTimeShiftConcepts.First(x => x.IsActive == true);
                var shiftConceptDetail = workTimeShiftConcept.ShiftConcept.ShiftConceptDetails.FirstOrDefault(x => x.IsActive == true);
                datalist.Add(_mapper.Map<ShiftConceptDetailModel>(shiftConceptDetail));
            }
            var model = new FilterResult<ShiftConceptDetailModel>()
            {
                Data = datalist,
                Total = datalist.Count()
            };
            return model;
        }


    }
}
