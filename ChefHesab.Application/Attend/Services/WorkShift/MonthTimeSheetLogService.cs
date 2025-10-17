using AutoMapper;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.MonthTimeSheetLog;
using KSC.Common;
using KSC.Common.Filters.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class MonthTimeSheetLogService: IMonthTimeSheetLogService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public MonthTimeSheetLogService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public async Task<KscResult> CheckLogsByYearMonthStep(SearchLogModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var haslogs = await _kscHrUnitOfWork.MonthTimeSheetLogRepository.AnyAsync(m => m.MonthTimeShitStepperId == model.Step && m.YearMonth == model.Yearmonth);
            if (haslogs)
                result.Id = (string)$" در این تاریخ این مرحله انجام شده است.";
            return result;
        }
    }
}
