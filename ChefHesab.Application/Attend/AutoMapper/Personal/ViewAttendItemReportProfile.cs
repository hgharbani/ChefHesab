   using Ksc.Hr.Domain.Entities;
 
  using AutoMapper;
using Ksc.Hr.DTO.View_AttendItemReport;

namespace Ksc.Hr.Application.Profiles
 {
  public class ViewAttendItemReportProfile : Profile
 {
  public ViewAttendItemReportProfile()
 {
  this.CreateMap<ViewAttendItemReport, ViewAttendItemReportDto>();
  // .ForMember(d => d.ViewAttendItemReport, s => s.MapFrom(a => a.ViewAttendItemReport));
   this.CreateMap<ViewAttendItemReportDto, ViewAttendItemReport>();

    this.CreateMap<ViewAttendItemReport, ViewAttendItemReport>();
   this.CreateMap<ViewAttendItemReport, ViewAttendItemReportPairDto>();
    this.CreateMap<ViewAttendItemReportPairDto, ViewAttendItemReport>();
   }
  }
 }

