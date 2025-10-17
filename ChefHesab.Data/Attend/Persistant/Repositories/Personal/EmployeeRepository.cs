using KSC.Infrastructure.Persistance;

using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.OnCall;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model;
using Ksc.HR.Share.Model.PaymentStatus;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.PersonalType;
using System.Linq.Dynamic.Core;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Share.Model.Employee;
using System.Runtime;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeRepository : EfRepository<Employee, int>, IEmployeeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        //public override void Update(Employee entity)
        //{
        //    _kscHrContext.Entry(entity).State = EntityState.Modified;
        //    //base.Update(entity);    
        //}
        //public void Update(Employee entity)
        //{
        //    _kscHrContext.Entry(entity).State = EntityState.Modified;
        //}

        public IQueryable<Employee> GetEmployeeByPaymentStatusAccess(List<string> roles)
        {
            var result = _kscHrContext.Employees.AsNoTracking();

            var paymentStatusAccess = _kscHrContext.PaymentStatusAccesses.Where(a => roles.Contains(a.AccessLevel.RoleInIdentityService));

            var paymentStatusId = paymentStatusAccess.Select(x => x.PaymentStatusId).Distinct().ToList();
            if (!paymentStatusId.Any(x => x == null)) //ادمین نباشد
            {
                result = result.Where(x => paymentStatusId.Any(p => p == x.PaymentStatusId));
            }

            return result;
        }


        public void UpdatePaymentStatusForStartMonth()
        {
            var paymentstatusfilter = new List<int> { 4, 2, 6 };
            var result = _kscHrContext.Employees
                .Where(a => a.PaymentStatusId.HasValue && paymentstatusfilter.Contains(a.PaymentStatusId.Value))
                .ToList();
            foreach (var item in result)
            {
                if (item.PaymentStatusId == 4)
                {
                    item.PaymentStatusId = 7;
                }
                else if (item.PaymentStatusId == 2 || item.PaymentStatusId == 6)
                {
                    item.PaymentStatusId = 3;
                }
                _kscHrContext.Employees.Update(item);
            }

        }

        public IQueryable<Employee> GetEmployeeWithActiveFamily(DateTime EndDate)
        {

            var result = _kscHrContext.Employees
                .Include(a => a.EmploymentType)
                .Include(a => a.GenderType)
                .Include(a => a.MaritalStatus)
                .Include(a => a.Families)
                .AsQueryable();

            result = result
                .Where(a =>
            a.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id &&
            a.PaymentStatusId.HasValue &&
            a.PaymentStatusId != EnumPaymentStatus.NewEmployeeOrganization.Id &&
            a.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id
            ).Where(a =>
            !a.Families.Any() ||
              a.Families.Any(c => !c.EndDateDependent.HasValue ||
                     (c.DependenceTypeId == EnumDependentType.ChildGirl.Id && c.IsContinuesDependent == true) ||
                     c.EndDateDependent.Value.Date >= EndDate
              )).AsNoTracking();
            return result;
        }

        public IQueryable<Employee> GetEmployeeWithActiveFamilyForTravel(DateTime EndDate)
        {

            var result = _kscHrContext.Employees
                .Include(a => a.GenderType)
                .Include(a => a.MaritalStatus)
                .Include(a => a.Families)
                .AsQueryable();

            result = result
                .Where(a =>
            a.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id &&
            a.PaymentStatusId.HasValue &&
            a.PaymentStatusId != EnumPaymentStatus.NewEmployeeOrganization.Id &&
            a.PaymentStatusId != EnumPaymentStatus.NewEmployee.Id
            ).Where(a =>
            !a.Families.Any() ||
              a.Families.Any(c => !c.EndDateDependent.HasValue ||
                     c.EndDateDependent.Value.Date >= EndDate
              )).AsNoTracking();
            return result;
        }


        public IQueryable<Employee> GetIncludedEmployeeById(int id)
        {
            var result = _kscHrContext.Employees.Where(a => a.Id == id)
                 .Include(a => a.EmployeeLongTermAbsences).ThenInclude(a => a.RollCallDefinition).ThenInclude(a => a.RollCallCategory)
                 .Include(a => a.EmployeeWorkGroups).ThenInclude(a => a.WorkGroup).ThenInclude(a => a.WorkTime)
                 .Include(a => a.EmployeeWorkGroups).ThenInclude(a => a.WorkGroup).ThenInclude(a => a.ShiftBoards).ThenInclude(a => a.WorkCalendar).AsNoTracking();
            return result;
        }

        public IQueryable<Employee> GetIncludedEmployee()
        {
            var result = _kscHrContext.Employees
                 .Include(a => a.EmployeeLongTermAbsences).ThenInclude(a => a.RollCallDefinition).ThenInclude(a => a.RollCallCategory)
                 .Include(a => a.EmployeeWorkGroups).ThenInclude(a => a.WorkGroup).ThenInclude(a => a.WorkTime)
                 .Include(a => a.EmployeeWorkGroups).ThenInclude(a => a.WorkGroup).ThenInclude(a => a.ShiftBoards).ThenInclude(a => a.WorkCalendar).AsNoTracking();
            return result;
        }

        public IQueryable<Employee> GetEmployee()
        {
            var result = _kscHrContext.Employees.AsQueryable();
            return result;
        }
        public IQueryable<Employee> GetEmployees(List<string> EmployeeNumbers)
        {
            var result = _kscHrContext.Employees
                .Include(a => a.WorkGroup)
                .Include(a => a.TeamWork)
                .AsQueryable();
            if (EmployeeNumbers.Any())
            {
                result = result.Where(x => EmployeeNumbers.Contains(x.EmployeeNumber));
            }
            return result;
        }
        public IQueryable<Employee> GetEmployeeIncludedTeamwork(List<int> ids)
        {
            var result = _kscHrContext.Employees.Where(a => ids.Contains(a.Id)
           && a.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id)
                .Include(a => a.TeamWork).AsQueryable();
            return result;
        }

        public IQueryable<Employee> GetEmployee(List<int> ids)
        {
            var result = _kscHrContext.Employees
                .Where(a => ids.Contains(a.Id)
                         && a.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id)
                .AsQueryable();
            return result;
        }

        public IQueryable<Employee> GetEmployees(List<int> ids)
        {
            List<int> invalidPaymentStatus = new List<int> { EnumPaymentStatus.DismiisalEmployee.Id, EnumPaymentStatus.NewEmployee.Id };
            var result = _kscHrContext.Employees.Include(x => x.Families).Include(x => x.MaritalStatus).Include(x => x.GenderType).Where(a => ids.Contains(a.Id)
                && a.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id && !invalidPaymentStatus.Any(p => p == a.PaymentStatusId))
                    .AsQueryable();
            return result;
        }

        public IQueryable<Employee> GetEmployeeIByTeamCodes(List<string> TeamCods)
        {
            var result = _kscHrContext.Employees.Include(a => a.TeamWork).Where(x =>
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.TeamWorkId != null
            && TeamCods.Contains(x.TeamWork.Code)).AsQueryable();
            return result;
        }
        //public CALLING_RPC GetPersonalDataMis(InputMisApiModel model)
        //{

        //    try
        //    {

        //        ParamApi<CALLING_RPC> miscall = new ParamApi<CALLING_RPC>(
        //             Enviroment: Enviroment.Load,
        //             DomainEnum: model.domain,
        //             LibraryName: LibraryName.PER,
        //             Subprogram: "S6XML025",
        //             ParamName: "CALLING_RPC",
        //             Pheader: new PHeader()
        //             {

        //             },
        //                 InputModel: new CALLING_RPC { NUM_PRSN_EMPL = model.NUM_PRSN_EMPL, USER_FK_JPOS = model.USER_FK_JPOS, FUNCTION = model.FUNCTION }
        //             );
        //        var result = miscall.GetResultDevelop<CALLING_RPC>();
        //        if (result.IsSuccess == false)
        //        {
        //            //throw new Exception(string.Join(",", result.Messages));
        //            throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
        //        }



        //        var userDataMIS = result.Data;

        //        return userDataMIS;
        //    }
        //    catch (Exception ex)
        //    {
        //        CALLING_RPC e = new CALLING_RPC();
        //        e.IsError = true;
        //        e.MsgError = ex.Message;
        //        return e;
        //        //throw new Exception(ex.Message);
        //    }
        //}
        public IQueryable<Employee> GetEmployeeByRelatedMonthTimeSheet()
        {
            var result = _kscHrContext.Employees
                .Include(a => a.PersonalType)
                .Include(a => a.TeamWork)
                .ThenInclude(x => x.OverTimeDefinition)
                .Include(x => x.PaymentStatus)
                .Include(x => x.WorkGroup)
                .ThenInclude(x => x.WorkTime)

                .AsQueryable();
            return result;
        }
        public async Task<Employee> GetEmployeeByIDMonthTimeSheet(int employeeID)
        {
            var result = await GetEmployeeByRelatedMonthTimeSheet().FirstOrDefaultAsync(x => x.Id == employeeID);
            return result;
        }
        public IQueryable<Employee> GetEmployeeWorkGroupWorkTimeByRelated()
        {
            var result = _kscHrContext.Employees.Include(x => x.WorkGroup).ThenInclude(x => x.WorkTime).AsQueryable();
            return result;
        }
        public IQueryable<Employee> GetEmployeeIncludedWorkCity()
        {
            var result = _kscHrContext.Employees
                 .Include(a => a.WorkCity).ThenInclude(a => a.City).ThenInclude(a => a.Province);
            return result;
        }

        //برای  اطلاعات پرسنلی
        public IQueryable<Employee> GetEmployeeIncludedCiteis(int id)
        {
            var result = _kscHrContext.Employees.Where(x => x.Id == id)
                 .Include(a => a.BirthCity).Include(a => a.CertificateCity).Include(a => a.HomeCity)
                 .ThenInclude(a => a.Province)
                 .Include(a => a.InsuranceList)
                 .ThenInclude(a => a.InsuranceType)
                 .AsQueryable();
            return result;
        }


        //public IQueryable<Employee> GetEmployeeIncludedInsurance()
        //{
        //    var result = _kscHrContext.Employees.Include(a => a.InsuranceList).
        //         .Include(a => a.InsuranceType);
        //    return result;
        //}
        //
        public Employee GetEmployeeByPersonalNum(string Nmp) //Nmp=شماره پرسنلی
        {
            var result = _kscHrContext.Employees.FirstOrDefault(x => x.EmployeeNumber == Nmp);
            return result;
        }
        public Employee GetEmployeeByPersonalNumIncluded(string Nmp) //Nmp=شماره پرسنلی
        {
            var result = _kscHrContext.Employees.AsQueryable().Where(x => x.EmployeeNumber == Nmp)
                .Include(a => a.TeamWork)
                .Include(a => a.EmploymentType)
                .Include(a => a.PaymentStatus)
                .Include(a => a.WorkGroup).ThenInclude(a => a.WorkTime)
                .Include(a => a.EmployeeInterdicts)
                .FirstOrDefault();
            return result;
        }

        public Employee GetEmployeeByEmployeeIdIncluded(int employeeId) //Nmp=شماره پرسنلی
        {
            var result = _kscHrContext.Employees.AsQueryable().Where(x => x.Id == employeeId)
                .Include(a => a.TeamWork)
                .Include(a => a.EmploymentType)
                .Include(a => a.PaymentStatus)
                .Include(a => a.WorkGroup).ThenInclude(a => a.WorkTime)
                .Include(a => a.EmployeeInterdicts)
                .FirstOrDefault();
            return result;
        }


        public Employee GetEmployeeByEmployeeIdIncludedByDetailed(int employeeId) //Nmp=شماره پرسنلی
        {
            var result = _kscHrContext.Employees.AsQueryable().Where(x => x.Id == employeeId)
              .Include(a => a.TeamWork)
                .Include(a => a.EmploymentType)
                .Include(a => a.PaymentStatus)
                .Include(a => a.IsarStatus)
                .Include(a => a.WorkGroup).ThenInclude(a => a.WorkTime)

                .FirstOrDefault();
            return result;
        }
        public async Task<Employee> GetEmployeeByEmployeeId(int id)
        {
            var result = await _kscHrContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public Employee GetEmployeeByEmployeeIdIncludedAbsence(int id)
        {
            var result = _kscHrContext.Employees.Where(x => x.Id == id).Include(x => x.EmployeeLongTermAbsences).Include(x => x.WorkGroup).First();
            return result;
        }

        public Employee GetEmployeeByEmployeeIdNoAsync(int id)
        {
            var result = _kscHrContext.Employees.FirstOrDefault(x => x.Id == id);
            return result;
        }
        public Employee GetEmployeeCondition(int id)
        {
            var result = _kscHrContext.Employees.Where(x => x.Id == id).Include(x => x.WorkCity).ThenInclude(x => x.Company).Include(x => x.WorkCity).ThenInclude(x => x.City).Include(a => a.Dismissal_Status).FirstOrDefault();
            return result;
        }
        public Employee GetEmployeeIncludedTeamWorkByEmployeeId(string employeeNumber)
        {
            var result = _kscHrContext.Employees.Where(x => x.EmployeeNumber == employeeNumber).Include(x => x.TeamWork).FirstOrDefault();
            return result;
        }
        public Employee GetEmployeeIncludedTeamWorkByEmployeeId(int employeeid)
        {
            var result = _kscHrContext.Employees.Include(x => x.TeamWork).FirstOrDefault(x => x.Id == employeeid);
            return result;
        }
        public Employee GetEmployeeIncludedTeamWork_WorkGroupByEmployeeId(int employeeid)
        {
            var result = _kscHrContext.Employees.Include(x => x.TeamWork).Include(x => x.WorkGroup).ThenInclude(x => x.WorkTime).FirstOrDefault(x => x.Id == employeeid);
            return result;
        }
        public IQueryable<Employee> GetEmployeesActivAndeHaveTeamAsNotracking()
        {
            return _kscHrContext.Employees.Where(x =>
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.TeamWorkId != null).AsNoTracking();
        }
        public IQueryable<Employee> GetEmployeesActivAndeHaveTeamAsNotracking(List<int> employeesid)
        {
            return _kscHrContext.Employees.Where(x => employeesid.Contains(x.Id) &&
            x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id &&
            x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.TeamWorkId != null).AsNoTracking();
        }
        //دیتای اتوکامپلیت ثبت ماموریت
        //      1- خود شخص با تیم زیرمجموعه
        //      2-خود شخص ولی تیم زیرمجموعه ندارد
        //      3-مامورین که تیم دارند ولی اسم خودشان جز لیست نمی آید
        public IQueryable<Employee> GetLeaderWithHisPersonalByUserName(string currentUserName)
        {
            var query_ViewMisEmployeeSecurity = _kscHrContext.ViewMisEmployeeSecurities.AsQueryable();

            query_ViewMisEmployeeSecurity = query_ViewMisEmployeeSecurity.Where(a => a.WindowsUser != null && a.WindowsUser.ToLower() == currentUserName.ToLower());

            var teamsByUserWindows = query_ViewMisEmployeeSecurity
                .Select(a => a.TeamWorkId).Distinct().ToList();

            var query = GetEmploymentPerson().Where(x => x.TeamWorkId != null && teamsByUserWindows.Any(t => t == x.TeamWorkId.Value)
            && x.PaymentStatusId != 7);
            var person = _kscHrContext.ViewMisEmployee.FirstOrDefault(a => a.WinUser != null && currentUserName.ToLower() == a.WinUser.ToLower());
            var personel = GetEmployee().Where(x => x.EmployeeNumber == person.EmployeeNumber);

            if (person != null)
            {
                if (query.Any())
                {

                    query = query.Concat(personel);
                }
                else
                {
                    query = personel;
                }
                return query.Distinct();
            }
            else// برای حالتی که مامور است و برای افراد تیم به جز خودش ماموریت ثبت میکند
            {
                return query.Distinct();

            }

        }

        //همکار جانشین
        public IQueryable<Employee> GetForStandbyEmployee()
        {
            return GetEmploymentPerson().Where(x => x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id && x.PaymentStatusId != 1).AsNoTracking();
        }
        public IQueryable<Employee> GetEmployeeForMissionByAccesslevel(string currentUserName, bool AcessLevel, int dismiisalEmployeeId)
        {
            IQueryable<Employee> employee;
            if (AcessLevel) //AcessLevel=True --> Admin ->نمایش تمام کاربران
            {
                employee = GetEmploymentPerson().Where(x => x.PaymentStatusId != dismiisalEmployeeId && x.PaymentStatusId != 1);
            }
            else  //AcessLevel=false --> No Admin ->نمایش افراد تحت سرپرستی و خود شخص

            {

                employee = GetLeaderWithHisPersonalByUserName(currentUserName);
            }
            return employee;
        }
        /// <summary>
        /// گرفتن اطلاعات پرسنل شرکتی
        /// </summary>
        /// <returns></returns>
        public IQueryable<Employee> GetEmploymentPerson()
        {
            return _kscHrContext.Employees.Where(x => x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id);
        }


        /// <summary>
        /// گرفتن اطلاعات پرسنل شرکتی جاری
        /// </summary>
        /// <returns></returns>
        public IQueryable<Employee> GetEmploymentPersonCurrent()
        {
            return _kscHrContext.Employees.Where(x => x.PersonalTypeId == EnumPersonalType.EmploymentPerson.Id
            && x.PaymentStatusId != EnumPaymentStatus.DismiisalEmployee.Id
            );
        }


        public IQueryable<Employee> GetByNationalCode(string nationalCode)
        {
            return GetEmployee().Where(x => x.NationalCode == nationalCode)
                .AsQueryable();

        }

        public IQueryable<Employee> GetDataEmployeeForTravel(List<string> employeeNumber)
        {
            var query = GetEmploymentPerson().Where(x => employeeNumber.Any(e => e == x.EmployeeNumber))
                .Include(a => a.WorkGroup)
               .ThenInclude(a => a.WorkTime);
            return query;
        }
        public IQueryable<Employee> GetDataEmployeeForTravelAsNoTracking(List<string> employeeNumber)
        {
            var query = GetEmploymentPerson().AsNoTracking().Include(a => a.TeamWork).Where(x => employeeNumber.Any(e => e == x.EmployeeNumber))
                .Include(a => a.WorkGroup)
               .ThenInclude(a => a.WorkTime);
            return query;
        }
        public async Task<Employee> GetEmployeeByEmployeeNumberAsync(string employeeNumber)
        {
            var result = await _kscHrContext.Employees.FirstOrDefaultAsync(x => x.EmployeeNumber == employeeNumber);
            return result;
        }
        public int? GetEmployeeIdByEmployeeNumberAsync(string employeeNumber)
        {
            var result = _kscHrContext.Employees.Where(x => x.EmployeeNumber == employeeNumber).Select(x => x.Id).FirstOrDefault();
            return result;
        }
        public async Task<List<Employee>> GetEmployeeByEmployeeNumbersAsync(List<string> employeeNumber)
        {
            var result = await _kscHrContext.Employees.Where(x => employeeNumber.Contains(x.EmployeeNumber)).ToListAsync();
            return result;
        }

        public List<Employee> GetEmployeeByEmployeeNumbersAsNoList(List<string> employeeNumber)
        {
            var result = _kscHrContext.Employees.AsNoTracking().Where(x => employeeNumber.Contains(x.EmployeeNumber)).ToList();
            return result;
        }
        public IQueryable<Employee> GetEmployeeSIncludedTeamWork()
        {
            return _kscHrContext.Employees.Include(x => x.TeamWork);
        }

        public IQueryable<Employee> GetEmployeeForTrafficCar()
        {
            List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8};
            var employeeQuery = _kscHrContext.Employees
               .AsQueryable().AsNoTracking()
               .Where(a => !paymentStatusIds.Contains(a.PaymentStatusId ?? 0) && a.TeamWorkId.HasValue && a.JobPositionId.HasValue);
            return employeeQuery;
        }//5711

        public IQueryable<Employee> GetEmployeeForManagnet(int yearMonth, List<int> jobpositionIds)
        {
            List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8};
            var employeeQuery = _kscHrContext.Employees
                               .Where(a => jobpositionIds.Contains(a.JobPositionId.Value) && !paymentStatusIds.Contains(a.PaymentStatusId ?? 0) && a.TeamWorkId.HasValue && a.JobPositionId.HasValue && a.EmployeeTimeSheets.Any(y => y.YearMonth == yearMonth));

            return employeeQuery;
        }


        public string GetEmployeeJobPositionGrade(int employeeId)
        {
            var query = (from e in _kscHrContext.Employees.ToList()
                         join jp in _kscHrContext.Chart_JobPosition.ToList()
                         on e.JobPositionId equals jp.Id
                         join ji in _kscHrContext.Chart_JobIdentity.ToList()
                         on jp.JobIdentityId equals ji.Id
                         join jc in _kscHrContext.Chart_JobCategory.ToList()
                         on ji.JobCategoryId equals jc.Id
                         join jcd in _kscHrContext.Chart_JobCategoryDefination.ToList()
                         on jc.JobCategoryDefinationId equals jcd.Id
                         select new
                         {
                             EmployeeId = e.Id,
                             JobPositionGrade = jcd.Title
                         }).FirstOrDefault(x => x.EmployeeId == employeeId).JobPositionGrade;

            return query;
        }

        public List<Employee> GetEmployeeWorkInJobposition(List<int> jobpositionIds)
        {
            var employees = GetEmployeesActivAndeHaveTeamAsNotracking();
            var workiJobPositionIds = employees.Where(a => a.JobPositionId.HasValue &&
            jobpositionIds.Contains(a.JobPositionId.Value)).ToList();
            return workiJobPositionIds;

        }

        public Employee GetEmployeeDetailsWithJobPositionAndStructureId(string employeeNumber)
        {
            return _kscHrContext
                .Employees
                .Include(x => x.JobPosition)
                .ThenInclude(x => x.Chart_Structure)
                .FirstOrDefault(x => x.EmployeeNumber == employeeNumber);
        }
    }

}
