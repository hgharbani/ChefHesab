using Ksc.HR.DTO.OnCall.Employee;
using AutoMapper;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.Employee;

namespace Ksc.HR.Appication.Profiles.OnCall
{
    public class EmployeeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeProfile()
        {
            CreateMap<Employee, SearchEmployeeModel>().AfterMap((src, dest) =>
            {
                if (src.TeamWork != null)
                {
                    dest.TeamCode = src.TeamWork.Code;

                }
                
                //dest.NationalCode = src.NationalCode??"";
                //dest.FatherName = src.FatherName ?? "";


            });
            CreateMap<EditEmployeeModel, Employee>().ReverseMap();
            CreateMap<AddOrEditEmployeeBaseModel, Employee>()                
               .ReverseMap().AfterMap((src, dest) =>
               {
                   if (src.InsuranceList!=null)
                   {
                       dest.InsuranceTypeId =   src.InsuranceList.InsuranceTypeId;
                       dest.InsuranceTypeText = src.InsuranceList.InsuranceType.Title;
                       dest.InsuranceListText = src.InsuranceList.Title;
                   }
               });

            CreateMap<Employee, EmployeeModel>().ReverseMap();
            CreateMap<Employee, EmployeeDataForOtherSystemModel>().ReverseMap();
            CreateMap<EmployeeTeamModel, Employee>().ReverseMap().AfterMap((src, dest) =>
            {
                //dest.WorkTimeTitle = src.WorkGroup.WorkTime.Title;
                //dest.TransferChangeDate = System.DateTime.Now;
                //dest.IsActive = true;
            }).ReverseMap();

            CreateMap<EmployeeBreastfeddingModel, Employee>()
                .ForMember(x => x.MaritalStatusId, map => map.Ignore())
                .ForMember(x => x.Gender, map => map.Ignore())
                .ForMember(x => x.Id, m => m.MapFrom(src => src.EmployeeId))
                .ReverseMap()

                .AfterMap((src, dest) =>
            {
                dest.EmployeeId = src.Id;
            }).ReverseMap();

            CreateMap<EmployeeConditionModel, Employee>().ReverseMap();

            CreateMap<EmployeeSacrificeDto, Employee>().ReverseMap();

            CreateMap<EmployeeConditionModel, EmployeeWorkDayTypeModel>();
            CreateMap<EmployeeConditionModel, AddOrEditEmployeeConditionModel>();
            CreateMap<EmployeeCategoryCoefficientDto, Employee>().ReverseMap();

        }

    }
}
