using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkTime;
using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class WorkTimeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public WorkTimeProfile()
        {
            //CreateMap<WorkTimeModel, WorkTime>().ReverseMap();    
            //CreateMap<AddWorkTimeModel, WorkTime>().ReverseMap();           
            //CreateMap<EditWorkTimeModel, WorkTime>().ReverseMap();
            //CreateMap<SearchWorkTimeModel, WorkTime>().ReverseMap();

            CreateMap<SearchWorkTimeModel, WorkTime>().ReverseMap();

            CreateMap<WorkTime, WorkTimeModel>().ReverseMap(); ;
            CreateMap<WorkTime, AddWorkTimeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap<WorkTime, EditWorkTimeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}
