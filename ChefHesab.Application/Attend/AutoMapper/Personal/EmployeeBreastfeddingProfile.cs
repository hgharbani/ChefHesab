using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeBreastfeddingProfile : Profile
    {
        public EmployeeBreastfeddingProfile()
        {
            CreateMap<EmployeeBreastfeddingModel, EmployeeBreastfedding>()
             .ReverseMap()

             .AfterMap((src, dest) =>
             {
             }).ReverseMap();

        }
    }
}
