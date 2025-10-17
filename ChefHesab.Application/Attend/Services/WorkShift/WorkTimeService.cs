using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.WorkTime;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.WorkShift;
using Ksc.HR.DTO.WorkFlow.BaseFile;
using Ksc.HR.Share.Model.Transfer_Relocation;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class WorkTimeService : IWorkTimeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public WorkTimeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public void Exists(int id, string title)
        {
            //return _kscHrUnitOfWork.WorkTimeRepository.Any(x => x.Id != id && x.IsActive == true && x.Title == title);
            if (_kscHrUnitOfWork.WorkTimeRepository.Any(x => x.Id != id == true && x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }
        public void Exists(string title)
        {
            //return _kscHrUnitOfWork.WorkTimeRepository.Any(x => x.IsActive == true && x.Title == title);
            //return _kscHrUnitOfWork.WorkTimeRepository.Any(x => x.Title == title);
            if (_kscHrUnitOfWork.WorkTimeRepository.Any(x => x.Title == title))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));

        }
        public void ExistsCode(string code)
        {
            //return _kscHrUnitOfWork.WorkTimeRepository.Any(x => x.IsActive == true && x.Code == code);
            if (_kscHrUnitOfWork.WorkTimeRepository.Any(x => x.IsActive == true && x.Code == code))
                throw new HRBusinessException(Validations.RepetitiveId,
                    String.Format(Validations.Repetitive, Resources.Workshift.WorkTimeCategoryResource.Title));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<KscResult> AddWorkTime(AddWorkTimeModel model)
        {
            var result = model.IsValid();
            try
            {


                if (!result.Success)
                    return result;


                Exists(model.Id, model.Title);
                ExistsByCode(model.Id, model.Code);

                //if (model.RepetitionPeriod == 0)
                //{
                //    //result.AddError("رکورد نامعتبر", "دوره تکرار باید مقدار داشته باشد");
                //    //return result;
                //    throw new HRBusinessException(Validations.RepetitiveId,
                //        String.Format("دوره تکرار باید مقدار داشته باشد"));
                //}
                if (model.WorkGroupTitle == null)
                {

                    //result.AddError("رکورد نامعتبر", "گروه کاری باید مقدار داشته باشد");
                    //return result;
                    throw new HRBusinessException(Validations.RepetitiveId,
                       String.Format("گروه کاری باید مقدار داشته باشد"));
                }
                if (model.ShiftConceptId == null)
                {

                    //result.AddError("رکورد نامعتبر", "گروه کاری باید مقدار داشته باشد");
                    //return result;
                    throw new HRBusinessException(Validations.RepetitiveId,
                       String.Format("شیفت باید مقدار داشته باشد"));
                }
                if (_kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().Any(x => model.WorkGroupTitle.Any(y => y == x.Code)))
                {
                    //result.AddError("رکورد نامعتبر", "کد گروه کاری نباید تکراری باشد");
                    //return result;
                    throw new HRBusinessException(Validations.RepetitiveId,
                      String.Format("کد گروه کاری نباید تکراری باشد"));
                }
                var WorkTime = _mapper.Map<WorkTime>(model);
                WorkTime.InsertDate = DateTime.Now;
                WorkTime.InsertUser = model.CurrentUserName;
                WorkTime.DomainName = model.DomainName;
                WorkTime.IsActive = true;
                WorkTime.MaximumForcedOverTime = model.MaximumForcedOverTime;
                WorkTime.ShiftSettingFromShiftboard = model.ShiftSettingFromShiftboard;
                WorkTime.OfficialUnOfficialHolidayFromWorkCalendar = model.OfficialUnOfficialHolidayFromWorkCalendar;
                WorkTime.PercentageWorkTime = model.PercentageWorkTime;
                foreach (var item in model.WorkGroupTitle)
                {
                    WorkGroup workGroup = new();
                    workGroup.Code = item;
                    workGroup.InsertDate = DateTime.Now;
                    workGroup.InsertUser = model.CurrentUserName;
                    workGroup.DomainName = model.DomainName;
                    workGroup.IsActive = true;
                    WorkTime.WorkGroups.Add(workGroup);
                }
                foreach (var item in model.ShiftConceptId)
                {
                    WorkTime.WorkTimeShiftConcepts.Add(new WorkTimeShiftConcept()
                    {
                        ShiftConceptId = item,
                        InsertUser = model.CurrentUserName,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    });
                }
                WorkTime.Code = WorkTime.Id.ToString();
                _kscHrUnitOfWork.WorkTimeRepository.Add(WorkTime);
                await _kscHrUnitOfWork.SaveAsync();
                var WorkTimeEdit = GetOne(WorkTime.Id);
                WorkTimeEdit.Code = WorkTimeEdit.Id.ToString();
                await _kscHrUnitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public async Task<KscResult> UpdateWorkTime(EditWorkTimeModel model)
        {

            var result = model.IsValid();
            if (!result.Success)
                return result;
            var oneWorkTime = GetOne(model.Id);
            if (oneWorkTime == null)
            {
                //result.AddError("رکورد حذف شده", "رکورد حذف شده است");
                //return result;
                throw new HRBusinessException(Validations.NotDelete,
                  String.Format("رکورد حذف شده است"));
            }
            //
            if (model.WorkGroupTitle == null)
            {

                //result.AddError("رکورد نامعتبر", "گروه کاری باید مقدار داشته باشد");
                //return result;
                throw new HRBusinessException(Validations.NotDelete,
                  String.Format("گروه کاری باید مقدار داشته باشد"));
            }

            if (_kscHrUnitOfWork.WorkGroupRepository.GetAllQueryable().Any(x => x.WorkTimeId != model.Id && model.WorkGroupTitle.Any(y => y == x.Code)))
            {
                //result.AddError("رکورد نامعتبر", "کد گروه کاری نباید تکراری باشد");
                //return result;
                throw new HRBusinessException(Validations.NotDelete,
                  String.Format("کد گروه کاری نباید تکراری باشد"));
            }

            ExistsByCode(model.Id, model.Code);
            Exists(model.Id, model.Title);
            //
            oneWorkTime.Code = oneWorkTime.Id.ToString();
            oneWorkTime.Title = model.Title;
            oneWorkTime.WorkTimeCategoryId = model.WorkTimeCategoryId;
            oneWorkTime.UpdateDate = DateTime.Now;
            oneWorkTime.UpdateUser = model.CurrentUserName;
            //oneWorkTime.RepetitionPeriod = model.RepetitionPeriod;
            oneWorkTime.IsActive = model.IsActive;
            oneWorkTime.MaximumForcedOverTime = model.MaximumForcedOverTime;
            oneWorkTime.ShiftSettingFromShiftboard = model.ShiftSettingFromShiftboard;
            oneWorkTime.OfficialUnOfficialHolidayFromWorkCalendar = model.OfficialUnOfficialHolidayFromWorkCalendar;
            oneWorkTime.PercentageWorkTime = model.PercentageWorkTime;
            var workGroupByWorkTimeId = _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.WorkTimeId == model.Id);
            var deletedWorkGroup = workGroupByWorkTimeId.Where(x => !model.WorkGroupTitle.Contains(x.Code));
            foreach (var item in deletedWorkGroup)
            {
                item.IsActive = false;
                _kscHrUnitOfWork.WorkGroupRepository.Update(item);
            }
            foreach (var item in model.WorkGroupTitle)
            {
                if (!workGroupByWorkTimeId.Any(x => x.Code == item))
                {
                    WorkGroup workGroup = new WorkGroup();
                    workGroup.Code = item;
                    workGroup.InsertDate = DateTime.Now;
                    workGroup.InsertUser = model.CurrentUserName;
                    workGroup.DomainName = model.DomainName;
                    workGroup.IsActive = true;
                    workGroup.WorkTimeId = oneWorkTime.Id;
                    _kscHrUnitOfWork.WorkGroupRepository.Add(workGroup);
                }
                else
                {
                    var workGroup = workGroupByWorkTimeId.FirstOrDefault(x => x.Code == item);
                    workGroup.IsActive = true;
                }
            }

            //
            var workTimeShiftConceptByWorkTimeId = _kscHrUnitOfWork.WorkTimeShiftConceptRepository.WhereQueryable(x => x.WorkTimeId == oneWorkTime.Id);
            var lasShiftConceptId = workTimeShiftConceptByWorkTimeId.Select(x => x.ShiftConceptId);
            var deletedShiftConcept = lasShiftConceptId.Where(x => !model.ShiftConceptId.Contains(x));
            foreach (var deletedShiftConceptId in deletedShiftConcept)
            {
                var workTimeShiftConcept = workTimeShiftConceptByWorkTimeId.FirstOrDefault(x => x.ShiftConceptId == deletedShiftConceptId);
                workTimeShiftConcept.IsActive = false;
                _kscHrUnitOfWork.WorkTimeShiftConceptRepository.Update(workTimeShiftConcept);
            }
            foreach (var shiftConceptId in model.ShiftConceptId)
            {
                if (!workTimeShiftConceptByWorkTimeId.Any(x => x.ShiftConceptId == shiftConceptId))
                {
                    WorkTimeShiftConcept workTimeShiftConcept = new WorkTimeShiftConcept()
                    {
                        ShiftConceptId = shiftConceptId,
                        WorkTimeId = oneWorkTime.Id,
                        InsertUser = model.CurrentUserName,
                        InsertDate = DateTime.Now,
                        DomainName = model.DomainName,
                        IsActive = true,
                    };
                    _kscHrUnitOfWork.WorkTimeShiftConceptRepository.Add(workTimeShiftConcept);
                }
                else
                {
                    var workTimeShiftConcept = workTimeShiftConceptByWorkTimeId.FirstOrDefault(x => x.ShiftConceptId == shiftConceptId);
                    workTimeShiftConcept.IsActive = true;
                    _kscHrUnitOfWork.WorkTimeShiftConceptRepository.Update(workTimeShiftConcept);
                }
            }
            //

            //

            await _kscHrUnitOfWork.SaveAsync();
            return result;
            result.AddError("رکورد نامعتبر", "رکورد نامعتبر میباشد");
            return result;

        }

        public async Task<KscResult> RemoveWorkTime(EditWorkTimeModel model)
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

            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }

        public List<WorkTimeModel> GetWorkTime()
        {
            var cities = _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable();
            return _mapper.Map<List<WorkTimeModel>>(cities);

        }
        public FilterResult<WorkTimeModel> GetWorktimeByFilter(FilterRequest Filter)
        {

            //var result = _FilterHandler.GetFilterResult<WorkTime>(_kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable()
            //   .AsQueryable().Include(a => a.WorkTimeCategory).Include(a => a.WorkGroup)
            //   .Where(a => a.IsActive).AsQueryable(), Filter, "Id");
            var worktimequery = _kscHrUnitOfWork.WorkTimeRepository.GetWorkTimes().AsQueryable().Where(x => x.WorkTimeCategory.IsActive == true);
            var result = _FilterHandler.GetFilterResult<WorkTime>(worktimequery, Filter, "Id");

            var worktimesFiltred = result.Data.Select(a => new WorkTimeModel()
            {
                WorkTimeCategoryId = a.WorkTimeCategoryId,
                Id = a.Id,
                Code = a.Code,
                WorkTimeCategoryTitle = a.WorkTimeCategory.Title,
                WorkTimeCategoryCode = a.WorkTimeCategory.Code,
                Title = a.Title,
                //RepetitionPeriod = a.RepetitionPeriod,
                WorkGroupTitle = string.Join(",", a.WorkGroups.Where(x => x.IsActive).Select(x => x.Code).ToList()),
                ShiftConceptTitle = a.WorkTimeShiftConcepts != null ? string.Join(",", a.WorkTimeShiftConcepts.Select(x => x.ShiftConcept.Title).ToList()) : "",
                InsertDate = a.InsertDate,
                InsertUser = a.InsertUser,
                IsActive = a.IsActive,
                ShiftSettingFromShiftboard = a.ShiftSettingFromShiftboard,
                OfficialUnOfficialHolidayFromWorkCalendar = a.OfficialUnOfficialHolidayFromWorkCalendar,
                MaximumForcedOverTime = a.MaximumForcedOverTime,
                PercentageWorkTime = a.PercentageWorkTime
                //HasWorkTimeCategory=a.WorkTimeCategory.IsActive

            }).ToList();

            return new FilterResult<WorkTimeModel>()
            {
                Data = worktimesFiltred,
                Total = result.Total

            };
        }


        public WorkTime GetOne(int id)
        {
            return _kscHrUnitOfWork.WorkTimeRepository.GetAllQueryable().First(a => a.Id == id);
        }

        public EditWorkTimeModel GetForEdit(int id)
        {
            var data = GetOne(id);
            EditWorkTimeModel model = new EditWorkTimeModel();
            model.Code = data.Code;
            model.Title = data.Title;
            model.IsActive = data.IsActive;
            model.WorkTimeCategoryId = data.WorkTimeCategoryId;
            //model.RepetitionPeriod = data.RepetitionPeriod;
            model.LastWorkGroupTitle = data.WorkGroups.Where(x => x.IsActive).Select(s => s.Code).ToList();
            var tt = _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.WorkTimeId == data.Id && x.IsActive);
            model.WorkGroupTitle = _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.WorkTimeId == data.Id && x.IsActive).Select(s => s.Code).ToList();//data.WorkGroups.Where(x => x.IsActive).Select(s => s.Code).ToList();
            model.ShiftConceptId = _kscHrUnitOfWork.WorkTimeShiftConceptRepository.WhereQueryable(x => x.WorkTimeId == data.Id && x.IsActive).Select(x => x.ShiftConceptId).ToList();
            var invalidCode = _kscHrUnitOfWork.WorkGroupRepository.WhereQueryable(x => x.WorkTimeId != data.Id).Select(x => x.Code);
            model.LatinAlphabetListValid = model.LatinAlphabetList.Except(invalidCode).ToList();
            model.MaximumForcedOverTime = data.MaximumForcedOverTime;
            model.ShiftSettingFromShiftboard = data.ShiftSettingFromShiftboard;
            model.OfficialUnOfficialHolidayFromWorkCalendar = data.OfficialUnOfficialHolidayFromWorkCalendar;
            model.PercentageWorkTime = data.PercentageWorkTime;
            return model;
            //return _mapper.Map<EditWorkTimeModel>(model);
        }

        public List<SearchWorkTimeModel> GetWorkByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<WorkTime>(_kscHrUnitOfWork.WorkTimeRepository.WhereQueryable(a => a.IsActive).AsQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchWorkTimeModel>>(result.Data.ToList());
        }
        public List<SearchWorkTimeModel> GetByKendoFilterForRelocation(FilterRequest Filter)
        {


            var Roozkar = (int)EnumWorkTime.Roozkar.Id;
            var ChaharShift = (int)EnumWorkTime.ChaharShift.Id;

            var result = _FilterHandler.GetFilterResult<WorkTime>(_kscHrUnitOfWork.WorkTimeRepository
                .WhereQueryable(a => a.IsActive &&
                (a.Code.Trim() == Roozkar.ToString() || a.Code.Trim() == ChaharShift.ToString())
              ).AsQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchWorkTimeModel>>(result.Data.ToList());
        }


        public bool ExistsByCode(int id, string code)
        {
            return _kscHrUnitOfWork.WorkTimeRepository.Any(x => x.Id != id && x.Code == code);
        }
    }
}
