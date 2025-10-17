using AutoMapper;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.FloatTimeSetting;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class FloatTimeSettingService: IFloatTimeSettingService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public FloatTimeSettingService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler filterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = filterHandler;
        }

        public List<FloatTimeSettingModel> GetFloatTimeSetting()
        {
            var floatTimes = _kscHrUnitOfWork.FloatTimeSettingRepository.GetAllQueryable();
            return floatTimes.Select(x => new FloatTimeSettingModel()
            {
                Id = x.Id,
                Title = x.FloatTimeFromShiftStart + "-" + x.MaximumFloatTimeFromShiftStart
            }).ToList();

        }
    }
}
