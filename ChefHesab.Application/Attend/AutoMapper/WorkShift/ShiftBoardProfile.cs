using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftBoard;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class ShiftBoardProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public ShiftBoardProfile()
        {
            CreateMap<ShiftBoardModel, ShiftBoard>().ReverseMap();    
            CreateMap<AddShiftBoardModel, ShiftBoard>().ReverseMap();           
            CreateMap<EditShiftBoardModel, ShiftBoard>().ReverseMap();
            CreateMap<SearchShiftBoardModel, ShiftBoard>().ReverseMap();
        }
    }
}
