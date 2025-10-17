using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.AccessLevel;
using AutoMapper;
using Ksc.HR.DTO.WorkShift.ShiftBoard;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class ViewShiftBoardProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public ViewShiftBoardProfile()
        {
            CreateMap<ViewShiftBoard, ShiftBoardByRangeDateModel>().ReverseMap(); ;
            

        }
    }
}
