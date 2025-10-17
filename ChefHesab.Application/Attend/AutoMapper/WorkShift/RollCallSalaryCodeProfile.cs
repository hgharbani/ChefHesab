using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallSalaryCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class RollCallSalaryCodeProfile: Profile
    {
        public RollCallSalaryCodeProfile()
        {
            CreateMap<RollCallSalaryCodeModel, RollCallSalaryCode>().ReverseMap(); 

        }
    }
}
