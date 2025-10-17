using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeTimeSheetRepository : IRepository<EmployeeTimeSheet, int>
    {
        IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheet(int yearMonth);
        IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetByRelated(int yearMonth);
        IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetByYearMonthAndEmployeeId(int yearMonth, List<int> EmployeeIds);
        IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetExcessOverTime(int yearMonth);
        IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetQueryable(int yearMonth);
        //IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetTrainingOverTime(int yearMonth);
    }
}
