using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift;
using Ksc.HR.Share.Model.PersonalType;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Share.WorkShift;

namespace Ksc.HR.Data.Persistant.Repositories.ODSViews
{
    public class ViewOdsEmployeeWithChartDataRepository : EfRepository<ViewOdsEmployeeWithChartData>, IViewOdsEmployeeWithChartDataRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewOdsEmployeeWithChartDataRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ViewOdsEmployeeWithChartData> GetEmployeeRoozkarWithChartData()
        {
            var roozkarId = ((int)WorkTimeCodeEnums.RoozKar);
            var employeesRoozkar = _kscHrContext.Employees.Include(x => x.WorkGroup).Where(x => x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
                 x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id
                 && x.WorkGroup.WorkTimeId == roozkarId);
            var result = _kscHrContext.ViewOdsEmployeeWithChartDatas.Join(employeesRoozkar, x => x.EmployeeNumber, y => y.EmployeeNumber, (x, y) => x);
            return result;
        }
        public IQueryable<ViewOdsEmployeeWithChartData> GetAllEmployeeWithChartData()
        {
            return _kscHrContext.ViewOdsEmployeeWithChartDatas;
        }
        public ViewOdsEmployeeWithChartData GetEmployeeWithChartDataByEmployeeNumber(string employeeNumber)
        {
            return _kscHrContext.ViewOdsEmployeeWithChartDatas.FirstOrDefault(x => x.EmployeeNumber == employeeNumber);
        }

        public List<ViewOdsEmployeeWithChartData> GetEmployeeWithChartDataByEmployeeNumbers(List<string> employeeNumber)
        {
            return _kscHrContext.ViewOdsEmployeeWithChartDatas.Where(x => employeeNumber.Contains(x.EmployeeNumber)).ToList();
        }
    }
}
