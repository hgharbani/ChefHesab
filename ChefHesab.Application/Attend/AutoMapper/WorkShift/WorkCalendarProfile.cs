using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class WorkCalendarProfile : Profile
    {
        public WorkCalendarProfile()
        {
            CreateMap<WorkCalendar, EditWorkCalendarModel>().ReverseMap().AfterMap((src, dest) =>
            {
                //dest.UpdateUser = src.UpdateUser;
                //dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
       
    }
}
