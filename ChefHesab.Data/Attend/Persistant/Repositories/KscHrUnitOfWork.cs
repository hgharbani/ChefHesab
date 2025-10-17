using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.ODSViews;
using Ksc.HR.Domain.Repositories.OnCall;
using Ksc.HR.Domain.Repositories.WorkShift;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Data.Persistant.Repositories.WorkShift;
using Ksc.HR.Data.Persistant.Repositories.ODSViews;
using Ksc.HR.Data.Persistant.Repositories.WorkFlow;
using Ksc.HR.Data.Persistant.Repositories.Personal;
using Ksc.HR.Data.Persistant.Repositories.OnCall;
using Ksc.HR.Data.Persistant.Repositories.Transfer;
using Ksc.HR.Domain.Repositories.HRSystemStatusControl;
using Ksc.HR.Data.Persistant.Repositories.HRSystemStatusControl;
using Ksc.HR.Domain.Repositories.Dismissal;
using Ksc.HR.Data.Persistant.Repositories.Dismissal;
using Ksc.HR.Domain.Repositories.ScheduledLoger;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.Hr.Data.Persistant.Repositories.Personal;
using Ksc.HR.Data.Persistant.Repositories.ScheduledLoger;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Data.Persistant.Repositories;
using Ksc.HR.Domain.Repositories.BusinessTrip;
using Ksc.HR.Data.Persistant.Repositories.BusinessTrip;
using Ksc.HR.Domain.Repositories.Chart;
using Ksc.HR.Data.Persistant.Repositories.Chart;
using Ksc.HR.Domain.Repositories.Stepper;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using Ksc.HR.Data.Persistant.Repositories.EmployeeBase;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Data.Persistant.Repositories.Emp;
using Ksc.HR.Domain.Repositories.Log;
using KSC.Identity.Abstractions.Services;
using Ksc.HR.Data.Persistant.Repositories.Stepper;
using Ksc.HR.Domain.Repositories.Salary;
using Ksc.HR.Data.Persistant.Repositories.Salary;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Data.Persistant.Repositories.Pay;
using Ksc.HR.Domain.Repositories.View;
using Ksc.HR.Data.Persistant.Repositories.View;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Data.Persistant.Repositories.Rule;
using Ksc.HR.Domain.Repositories.Reward;
using Ksc.HR.Data.Persistant.Repositories.Reward;
using Ksc.HR.Domain.Repositories.StandBy;
using Ksc.HR.Data.Persistant.Repositories.Standby;
using Ksc.HR.Domain.Repositories.Security;
using Ksc.HR.Data.Persistant.Repositories.Security;
using Ksc.HR.Domain.Repositories.Transfer;



namespace Ksc.HR.Data.Persistant.Repositories
{

    public class KscHrUnitOfWork : UnitOfWork<KscHrContext>, IKscHrUnitOfWork
    {
        private readonly IUserService _userService;

        public KscHrUnitOfWork(KscHrContext databaseContext, IUserService userService)
            : base(databaseContext: databaseContext, userService: userService)
        {
        }



        private IV_JobPositionRepository view_JobPositionRepository;
        public IV_JobPositionRepository View_JobPositionRepository
        {
            get
            {
                view_JobPositionRepository ??= new V_JobPositionRepository(DatabaseContext);
                return view_JobPositionRepository;
            }
        }


        private IView_EmployeeRepository view_EmployeeRepository;
        public IView_EmployeeRepository View_EmployeeRepository
        {
            get
            {
                view_EmployeeRepository ??= new View_EmployeeRepository(DatabaseContext);
                return view_EmployeeRepository;
            }
        }



        private IEmployeeDeductionInstallmentPutOffRepository employeeDeductionInstallmentPutOffRepository;
        public IEmployeeDeductionInstallmentPutOffRepository EmployeeDeductionInstallmentPutOffRepository
        {
            get
            {
                employeeDeductionInstallmentPutOffRepository ??= new EmployeeDeductionInstallmentPutOffRepository(DatabaseContext);
                return employeeDeductionInstallmentPutOffRepository;
            }
        }



        private IEventListenerRepository eventListenerRepository;
        public IEventListenerRepository EventListenerRepository
        {
            get
            {
                eventListenerRepository ??= new EventListenerRepository(DatabaseContext);
                return eventListenerRepository;
            }
        }

        private ILicenseTypeRepository licenseTypeRepository;
        public ILicenseTypeRepository LicenseTypeRepository
        {
            get
            {
                licenseTypeRepository ??= new LicenseTypeRepository(DatabaseContext);
                return licenseTypeRepository;
            }
        }

        private ILicenseJobPositionRepository licenseJobPositionRepository;
        public ILicenseJobPositionRepository LicenseJobPositionRepository
        {
            get
            {
                licenseJobPositionRepository ??= new LicenseJobPositionRepository(DatabaseContext);
                return licenseJobPositionRepository;
            }
        }

        private IJobCategoryDefinitionGuideRepository jobCategoryDefinitionGuideRepository;
        public IJobCategoryDefinitionGuideRepository JobCategoryDefinitionGuideRepository
        {
            get
            {
                jobCategoryDefinitionGuideRepository ??= new JobCategoryDefinitionGuideRepository(DatabaseContext);
                return jobCategoryDefinitionGuideRepository;
            }
        }

        private IRelocationTypeRepository relocationTypeRepository;
        public IRelocationTypeRepository RelocationTypeRepository
        {
            get
            {
                relocationTypeRepository ??= new RelocationTypeRepository(DatabaseContext);
                return relocationTypeRepository;
            }
        }

        private IRelocationStatusRepository relocationStatusRepository;
        public IRelocationStatusRepository RelocationStatusRepository
        {
            get
            {
                relocationStatusRepository ??= new RelocationStatusRepository(DatabaseContext);
                return relocationStatusRepository;
            }
        }

        private IRelocationRepository relocationRepository;
        public IRelocationRepository RelocationRepository
        {
            get
            {
                relocationRepository ??= new RelocationRepository(DatabaseContext);
                return relocationRepository;
            }
        }



        private ICofficientProximityProductRepository cofficientProximityProductRepository;
        public ICofficientProximityProductRepository CofficientProximityProductRepository
        {
            get
            {
                cofficientProximityProductRepository ??= new CofficientProximityProductRepository(DatabaseContext);
                return cofficientProximityProductRepository;
            }
        }

        private IPaymentStatusAccessRepository paymentStatusAccessRepository;
        public IPaymentStatusAccessRepository PaymentStatusAccessRepository
        {
            get
            {
                paymentStatusAccessRepository ??= new PaymentStatusAccessRepository(DatabaseContext);
                return paymentStatusAccessRepository;
            }
        }

        private IEmployeeEfficiencyHistoryRepository employeeEfficiencyHistoryRepository;
        public IEmployeeEfficiencyHistoryRepository EmployeeEfficiencyHistoryRepository
        {
            get
            {
                employeeEfficiencyHistoryRepository ??= new EmployeeEfficiencyHistoryRepository(DatabaseContext);
                return employeeEfficiencyHistoryRepository;
            }
        }
        private IEmployeeEfficiencyMonthRepository employeeEfficiencyMonthRepository;
        public IEmployeeEfficiencyMonthRepository EmployeeEfficiencyMonthRepository
        {
            get
            {
                employeeEfficiencyMonthRepository ??= new EmployeeEfficiencyMonthRepository(DatabaseContext);
                return employeeEfficiencyMonthRepository;
            }

        }

        private IViewOdsJobStatusCategoryRepository viewOdsJobStatusCategoryRepository;
        public IViewOdsJobStatusCategoryRepository ViewOdsJobStatusCategoryRepository
        {
            get
            {
                viewOdsJobStatusCategoryRepository ??= new ViewOdsJobStatusCategoryRepository(DatabaseContext);
                return viewOdsJobStatusCategoryRepository;
            }
        }

        private IViewMisEmployeeDisciplineRepository viewMisEmployeeDisciplineRepository;
        public IViewMisEmployeeDisciplineRepository ViewMisEmployeeDisciplineRepository
        {
            get
            {
                viewMisEmployeeDisciplineRepository ??= new ViewMisEmployeeDisciplineRepository(DatabaseContext);
                return viewMisEmployeeDisciplineRepository;
            }
        }

        private IViewOdsJobPositionTreeRepository viewOdsJobPositionTreerepository;
        public IViewOdsJobPositionTreeRepository ViewOdsJobPositionTreeRepository
        {
            get
            {
                viewOdsJobPositionTreerepository ??= new ViewOdsJobPositionTreeRepository(DatabaseContext);
                return viewOdsJobPositionTreerepository;
            }
        }


        private IEmployeeEducationTimeRepository employeeEducationTimerepository;
        public IEmployeeEducationTimeRepository EmployeeEducationTimeRepository
        {
            get
            {
                employeeEducationTimerepository ??= new EmployeeEducationTimeRepository(DatabaseContext);
                return employeeEducationTimerepository;
            }
        }
        private IEmployeeBreastfeddingRepository employeeBreastfeddingRepository;
        public IEmployeeBreastfeddingRepository EmployeeBreastfeddingRepository
        {
            get
            {
                employeeBreastfeddingRepository ??= new EmployeeBreastfeddingRepository(DatabaseContext);
                return employeeBreastfeddingRepository;
            }
        }
        private IWorkTimeCategoryRepository worktimecategoryrepository;
        public IWorkTimeCategoryRepository WorkTimeCategoryRepository
        {
            get
            {
                worktimecategoryrepository ??= new WorkTimeCategoryRepository(DatabaseContext);
                return worktimecategoryrepository;
            }
        }
        private IWorkTimeRepository worktimerepository;
        public IWorkTimeRepository WorkTimeRepository
        {
            get
            {
                worktimerepository ??= new WorkTimeRepository(DatabaseContext);
                return worktimerepository;
            }
        }
        private ICityRepository cityrepository;
        public ICityRepository CityRepository
        {
            get
            {
                cityrepository ??= new CityRepository(DatabaseContext);
                return cityrepository;
            }
        }
        private IProvinceRepository provincerepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                provincerepository ??= new ProvinceRepository(DatabaseContext);
                return provincerepository;
            }
        }
        private IEntryExitTypeRepository entryExitTypeRepository;
        public IEntryExitTypeRepository EntryExitTypeRepository
        {
            get
            {
                entryExitTypeRepository ??= new EntryExitTypeRepository(DatabaseContext);
                return entryExitTypeRepository;
            }
        }
        private IShiftConceptRepository shiftconceptrepository;
        public IShiftConceptRepository ShiftConceptRepository
        {
            get
            {
                shiftconceptrepository ??= new ShiftConceptRepository(DatabaseContext);
                return shiftconceptrepository;
            }
        }
        private IShiftConceptDetailRepository shiftconceptdetailrepository;
        public IShiftConceptDetailRepository ShiftConceptDetailRepository
        {
            get
            {
                shiftconceptdetailrepository ??= new ShiftConceptDetailRepository(DatabaseContext);
                return shiftconceptdetailrepository;
            }
        }
        private IWorkDayTypeRepository workdaytyperepository;
        public IWorkDayTypeRepository WorkDayTypeRepository
        {
            get
            {
                workdaytyperepository ??= new WorkDayTypeRepository(DatabaseContext);
                return workdaytyperepository;
            }
        }
        private ITimeShiftSettingRepository timeshiftsettingrepository;
        public ITimeShiftSettingRepository TimeShiftSettingRepository
        {
            get
            {
                timeshiftsettingrepository ??= new TimeShiftSettingRepository(DatabaseContext);
                return timeshiftsettingrepository;
            }
        }
        private ICountryRepository countryrepository;
        public ICountryRepository CountryRepository
        {
            get
            {
                countryrepository ??= new CountryRepository(DatabaseContext);
                return countryrepository;

            }
        }
        private IDayNightSettingTimeRepository daynightsettingtimerepository;
        public IDayNightSettingTimeRepository DayNightSettingTimeRepository
        {
            get
            {
                daynightsettingtimerepository ??= new DayNightSettingTimeRepository(DatabaseContext);
                return daynightsettingtimerepository;

            }
        }
        private IShiftBoardRepository shiftboardrepository;
        public IShiftBoardRepository ShiftBoardRepository
        {
            get
            {
                shiftboardrepository ??= new ShiftBoardRepository(DatabaseContext);
                return shiftboardrepository;

            }
        }
        private IShiftHolidayRepository shiftholidayrepository;
        public IShiftHolidayRepository ShiftHolidayRepository
        {
            get
            {
                shiftholidayrepository ??= new ShiftHolidayRepository(DatabaseContext);
                return shiftholidayrepository;

            }
        }
        private IWorkCalendarRepository workcalendarrepository;
        public IWorkCalendarRepository WorkCalendarRepository
        {
            get
            {
                workcalendarrepository ??= new WorkCalendarRepository(DatabaseContext);
                return workcalendarrepository;

            }
        }
        private IWorkGroupRepository workgrouprepository;
        public IWorkGroupRepository WorkGroupRepository
        {
            get
            {
                workgrouprepository ??= new WorkGroupRepository(DatabaseContext);
                return workgrouprepository;

            }
        }
        private IWorkCompanySettingRepository workcompanysettingrepository;
        public IWorkCompanySettingRepository WorkCompanySettingRepository
        {
            get
            {
                workcompanysettingrepository ??= new WorkCompanySettingRepository(DatabaseContext);
                return workcompanysettingrepository;

            }
        }
        private IAccessLevelRepository accesslevelrepository;
        public IAccessLevelRepository AccessLevelRepository
        {
            get
            {
                accesslevelrepository ??= new AccessLevelRepository(DatabaseContext);
                return accesslevelrepository;

            }
        }
        private ICompanyRepository companyrepository;
        public ICompanyRepository CompanyRepository
        {
            get
            {
                companyrepository ??= new CompanyRepository(DatabaseContext);
                return companyrepository;

            }
        }
        private ICompatibleRollCallRepository compatiblerollcallrepository;
        public ICompatibleRollCallRepository CompatibleRollCallRepository
        {
            get
            {
                compatiblerollcallrepository ??= new CompatibleRollCallRepository(DatabaseContext);
                return compatiblerollcallrepository;

            }
        }
        private IIncludedDefinitionRepository includeddefinitionrepository;
        public IIncludedDefinitionRepository IncludedDefinitionRepository
        {
            get
            {
                includeddefinitionrepository ??= new IncludedDefinitionRepository(DatabaseContext);
                return includeddefinitionrepository;

            }
        }
        private IIncludedRollCallRepository includedrollcallrepository;
        public IIncludedRollCallRepository IncludedRollCallRepository
        {
            get
            {
                includedrollcallrepository ??= new IncludedRollCallRepository(DatabaseContext);
                return includedrollcallrepository;

            }
        }
        private IOverTimeDefinitionRepository overtimedefinitionrepository;
        public IOverTimeDefinitionRepository OverTimeDefinitionRepository
        {
            get
            {
                overtimedefinitionrepository ??= new OverTimeDefinitionRepository(DatabaseContext);
                return overtimedefinitionrepository;

            }
        }
        private IRollCallEmploymentTypeRepository rollCallEmploymentTyperepository;
        public IRollCallEmploymentTypeRepository RollCallEmploymentTypeRepository
        {
            get
            {
                rollCallEmploymentTyperepository ??= new RollCallEmploymentTypeRepository(DatabaseContext);
                return rollCallEmploymentTyperepository;

            }
        }
        private IRollCallCategoryRepository rollcallcategoryrepository;
        public IRollCallCategoryRepository RollCallCategoryRepository
        {
            get
            {
                rollcallcategoryrepository ??= new RollCallCategoryRepository(DatabaseContext);
                return rollcallcategoryrepository;

            }
        }
        private IRollCallConceptRepository rollcallconceptrepository;
        public IRollCallConceptRepository RollCallConceptRepository
        {
            get
            {
                rollcallconceptrepository ??= new RollCallConceptRepository(DatabaseContext);
                return rollcallconceptrepository;

            }
        }
        private IRollCallDefinitionRepository rollcalldefinitionrepository;
        public IRollCallDefinitionRepository RollCallDefinitionRepository
        {
            get
            {
                rollcalldefinitionrepository ??= new RollCallDefinitionRepository(DatabaseContext);
                return rollcalldefinitionrepository;

            }
        }
        private IRollCallJobCategoryRepository rollcalljobcategoryrepository;
        public IRollCallJobCategoryRepository RollCallJobCategoryRepository
        {
            get
            {
                rollcalljobcategoryrepository ??= new RollCallJobCategoryRepository(DatabaseContext);
                return rollcalljobcategoryrepository;

            }
        }
        private IRollCallSalaryCodeRepository rollcallsalarycoderepository;
        public IRollCallSalaryCodeRepository RollCallSalaryCodeRepository
        {
            get
            {
                rollcallsalarycoderepository ??= new RollCallSalaryCodeRepository(DatabaseContext);
                return rollcallsalarycoderepository;

            }
        }
        private IRollCallWorkTimeDayTypeRepository rollCallWorkTimeDayTypeRepository;
        public IRollCallWorkTimeDayTypeRepository RollCallWorkTimeDayTypeRepository
        {
            get
            {
                rollCallWorkTimeDayTypeRepository ??= new RollCallWorkTimeDayTypeRepository(DatabaseContext);
                return rollCallWorkTimeDayTypeRepository;

            }
        }
        private ITeamWorkRepository teamworkrepository;
        public ITeamWorkRepository TeamWorkRepository
        {
            get
            {
                teamworkrepository ??= new TeamWorkRepository(DatabaseContext);
                return teamworkrepository;

            }
        }
        private ITeamWorkCategoryRepository teamworkcategoryrepository;
        public ITeamWorkCategoryRepository TeamWorkCategoryRepository
        {
            get
            {
                teamworkcategoryrepository ??= new TeamWorkCategoryRepository(DatabaseContext);
                return teamworkcategoryrepository;

            }
        }
        private ITeamWorkCategoryTypeRepository teamworkcategorytyperepository;
        public ITeamWorkCategoryTypeRepository TeamWorkCategoryTypeRepository
        {
            get
            {
                teamworkcategorytyperepository ??= new TeamWorkCategoryTypeRepository(DatabaseContext);
                return teamworkcategorytyperepository;

            }
        }
        private ITeamWorkMangementCodeRepository teamworkmangementcoderepository;
        public ITeamWorkMangementCodeRepository TeamWorkMangementCodeRepository
        {
            get
            {
                teamworkmangementcoderepository ??= new TeamWorkMangementCodeRepository(DatabaseContext);
                return teamworkmangementcoderepository;

            }
        }
        private IWorkCityRepository workcityrepository;
        public IWorkCityRepository WorkCityRepository
        {
            get
            {
                workcityrepository ??= new WorkCityRepository(DatabaseContext);
                return workcityrepository;

            }
        }
        private IViewMisCostCenterRepository viewmiscostcenterrepository;
        public IViewMisCostCenterRepository ViewMisCostCenterRepository
        {
            get
            {
                viewmiscostcenterrepository ??= new ViewMisCostCenterRepository(DatabaseContext);
                return viewmiscostcenterrepository;

            }
        }
        private IViewMisEmploymentTypeRepository viewmisemploymenttyperepository;
        public IViewMisEmploymentTypeRepository ViewMisEmploymentTypeRepository
        {
            get
            {
                viewmisemploymenttyperepository ??= new ViewMisEmploymentTypeRepository(DatabaseContext);
                return viewmisemploymenttyperepository;

            }
        }
        private IViewMisJobCategoryRepository viewmisjobcategoryrepository;
        public IViewMisJobCategoryRepository ViewMisJobCategoryRepository
        {
            get
            {
                viewmisjobcategoryrepository ??= new ViewMisJobCategoryRepository(DatabaseContext);
                return viewmisjobcategoryrepository;

            }
        }
        private IViewMisSalaryCodeRepository viewmissalarycoderepository;
        public IViewMisSalaryCodeRepository ViewMisSalaryCodeRepository
        {
            get
            {
                viewmissalarycoderepository ??= new ViewMisSalaryCodeRepository(DatabaseContext);
                return viewmissalarycoderepository;

            }
        }
        private IOnCall_RequestRepository oncall_requestrepository;
        public IOnCall_RequestRepository OnCall_RequestRepository
        {
            get
            {
                oncall_requestrepository ??= new OnCall_RequestRepository(DatabaseContext);
                return oncall_requestrepository;

            }
        }
        private IOnCall_TypeRepository oncall_typerepository;
        public IOnCall_TypeRepository OnCall_TypeRepository
        {
            get
            {
                oncall_typerepository ??= new OnCall_TypeRepository(DatabaseContext);
                return oncall_typerepository;

            }
        }
        private IOnCall_WorkTimeRepository oncall_worktimerepository;
        public IOnCall_WorkTimeRepository OnCall_WorkTimeRepository
        {
            get
            {
                oncall_worktimerepository ??= new OnCall_WorkTimeRepository(DatabaseContext);
                return oncall_worktimerepository;

            }
        }
        private IOnCall_AccessManagmentOnCallTypeRepository managmentOnCallTypeRepository;
        public IOnCall_AccessManagmentOnCallTypeRepository OnCall_AccessManagmentOnCallTypeRepository
        {
            get
            {
                managmentOnCallTypeRepository ??= new OnCall_AccessManagmentOnCallTypeRepository(DatabaseContext);
                return managmentOnCallTypeRepository;
            }
        }
        private IEmployeeRepository employeerepository;
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                employeerepository ??= new EmployeeRepository(DatabaseContext);
                return employeerepository;

            }
        }
        private IEmployeeLongTermAbsenceRepository employeelongtermabsencerepository;
        public IEmployeeLongTermAbsenceRepository EmployeeLongTermAbsenceRepository
        {
            get
            {
                employeelongtermabsencerepository ??= new EmployeeLongTermAbsenceRepository(DatabaseContext);
                return employeelongtermabsencerepository;

            }
        }
        private IEmployeeAttendAbsenceItemRepository employeeattendabsenceitemrepository;
        public IEmployeeAttendAbsenceItemRepository EmployeeAttendAbsenceItemRepository
        {
            get
            {
                employeeattendabsenceitemrepository ??= new EmployeeAttendAbsenceItemRepository(DatabaseContext);
                return employeeattendabsenceitemrepository;

            }
        }
        private IEmployeeEntryExitRepository employeeentryexitrepository;
        public IEmployeeEntryExitRepository EmployeeEntryExitRepository
        {
            get
            {
                employeeentryexitrepository ??= new EmployeeEntryExitRepository(DatabaseContext);
                return employeeentryexitrepository;

            }
        }
        private IEmployeeWorkGroupRepository employeeworkgrouprepository;
        public IEmployeeWorkGroupRepository EmployeeWorkGroupRepository
        {
            get
            {
                employeeworkgrouprepository ??= new EmployeeWorkGroupRepository(DatabaseContext);
                return employeeworkgrouprepository;

            }
        }
        private IWF_AccessManagementRepository wf_accessmanagementrepository;
        public IWF_AccessManagementRepository WF_AccessManagementRepository
        {
            get
            {
                wf_accessmanagementrepository ??= new WF_AccessManagementRepository(DatabaseContext);
                return wf_accessmanagementrepository;
            }
        }
        private IWF_JobCategoryRangeRepository wf_jobcategoryrangerepository;
        public IWF_JobCategoryRangeRepository WF_JobCategoryRangeRepository
        {
            get
            {
                wf_jobcategoryrangerepository ??= new WF_JobCategoryRangeRepository(DatabaseContext);
                return wf_jobcategoryrangerepository;

            }
        }
        private IWF_PriorityRepository wf_priorityrepository;
        public IWF_PriorityRepository WF_PriorityRepository
        {
            get
            {
                wf_priorityrepository ??= new WF_PriorityRepository(DatabaseContext);
                return wf_priorityrepository;
            }
        }
        private IWF_ProcessRepository wf_processrepository;
        public IWF_ProcessRepository WF_ProcessRepository
        {
            get
            {
                wf_processrepository ??= new WF_ProcessRepository(DatabaseContext);
                return wf_processrepository;

            }
        }
        private IWF_RequestHistoryRepository wf_requesthistoryrepository;
        public IWF_RequestHistoryRepository WF_RequestHistoryRepository
        {
            get
            {
                wf_requesthistoryrepository ??= new WF_RequestHistoryRepository(DatabaseContext);
                return wf_requesthistoryrepository;

            }
        }
        private IWF_RequestRepository wf_requestrepository;
        public IWF_RequestRepository WF_RequestRepository
        {
            get
            {
                wf_requestrepository ??= new WF_RequestRepository(DatabaseContext);
                return wf_requestrepository;
            }
        }
        private IWF_RoleRepository wf_rolerepository;
        public IWF_RoleRepository WF_RoleRepository
        {
            get
            {
                wf_rolerepository ??= new WF_RoleRepository(DatabaseContext);
                return wf_rolerepository;

            }
        }
        private IWF_StatusProcessManagementRepository wf_statusprocessmanagementrepository;
        public IWF_StatusProcessManagementRepository WF_StatusProcessManagementRepository
        {
            get
            {
                wf_statusprocessmanagementrepository ??= new WF_StatusProcessManagementRepository(DatabaseContext);
                return wf_statusprocessmanagementrepository;

            }
        }
        private IWF_StatusReasonRepository wf_statusreasonrepository;
        public IWF_StatusReasonRepository WF_StatusReasonRepository
        {
            get
            {
                wf_statusreasonrepository ??= new WF_StatusReasonRepository(DatabaseContext);
                return wf_statusreasonrepository;

            }
        }
        private IWF_StatusRepository wf_statusrepository;
        public IWF_StatusRepository WF_StatusRepository
        {
            get
            {
                wf_statusrepository ??= new WF_StatusRepository(DatabaseContext);
                return wf_statusrepository;

            }
        }
        private IWF_ValidJobCategoryRepository wf_validjobcategoryrepository;
        public IWF_ValidJobCategoryRepository WF_ValidJobCategoryRepository
        {
            get
            {
                wf_validjobcategoryrepository ??= new WF_ValidJobCategoryRepository(DatabaseContext);
                return wf_validjobcategoryrepository;

            }
        }
        private IWF_WorkFlowManagementRepository wf_workflowmanagementrepository;
        public IWF_WorkFlowManagementRepository WF_WorkFlowManagementRepository
        {
            get
            {
                wf_workflowmanagementrepository ??= new WF_WorkFlowManagementRepository(DatabaseContext);
                return wf_workflowmanagementrepository;

            }
        }
        private IWF_WorkFlowStatusReasonRepository wf_workflowstatusreasonrepository;
        public IWF_WorkFlowStatusReasonRepository WF_WorkFlowStatusReasonRepository
        {
            get
            {
                wf_workflowstatusreasonrepository ??= new WF_WorkFlowStatusReasonRepository(DatabaseContext);
                return wf_workflowstatusreasonrepository;

            }
        }

        private IViewMisSubFunctionRepository viewmissubfunctionrepository;
        public IViewMisSubFunctionRepository ViewMisSubFunctionRepository
        {
            get
            {
                viewmissubfunctionrepository ??= new ViewMisSubFunctionRepository(DatabaseContext);
                return viewmissubfunctionrepository;

            }
        }
        private IViewMisUserDefinitionRepository viewmisuserdefinitionrepository;
        public IViewMisUserDefinitionRepository ViewMisUserDefinitionRepository
        {
            get
            {
                viewmisuserdefinitionrepository ??= new ViewMisUserDefinitionRepository(DatabaseContext);
                return viewmisuserdefinitionrepository;

            }
        }

        private IViewMisJobPositionRepository viewmisjobpositionrepository;
        public IViewMisJobPositionRepository ViewMisJobPositionRepository
        {
            get
            {
                viewmisjobpositionrepository ??= new ViewMisJobPositionRepository(DatabaseContext);
                return viewmisjobpositionrepository;
            }
        }


        private ITransfer_RequestTypeRepository transfer_RequestTypeRepository;
        public ITransfer_RequestTypeRepository Transfer_RequestTypeRepository
        {
            get
            {
                transfer_RequestTypeRepository ??= new Transfer_RequestTypeRepository(DatabaseContext);
                return transfer_RequestTypeRepository;
            }
        }

        private ITransfer_TypeRepository transfer_TypeRepository;
        public ITransfer_TypeRepository Transfer_TypeRepository
        {
            get
            {
                transfer_TypeRepository ??= new Transfer_TypeRepository(DatabaseContext);
                return transfer_TypeRepository;
            }
        }
        private ITransfer_RequestReasonJobCategoryRepository transfer_RequestReasonJobCategoryRepository;
        public ITransfer_RequestReasonJobCategoryRepository Transfer_RequestReasonJobCategoryRepository
        {
            get
            {
                transfer_RequestReasonJobCategoryRepository ??= new Transfer_RequestReasonJobCategoryRepository(DatabaseContext);
                return transfer_RequestReasonJobCategoryRepository;
            }
        }

        private ITransfer_RequestReasonRepository transfer_RequestReasonRepository;
        public ITransfer_RequestReasonRepository Transfer_RequestReasonRepository
        {
            get
            {
                transfer_RequestReasonRepository ??= new Transfer_RequestReasonRepository(DatabaseContext);
                return transfer_RequestReasonRepository;
            }
        }

        private ITransfer_RequestReasonTypeRepository transfer_RequestReasonTypeRepository;
        public ITransfer_RequestReasonTypeRepository Transfer_RequestReasonTypeRepository
        {
            get
            {
                transfer_RequestReasonTypeRepository ??= new Transfer_RequestReasonTypeRepository(DatabaseContext);
                return transfer_RequestReasonTypeRepository;
            }
        }
        private ITransfer_RequestRepository transfer_RequestRepository;
        public ITransfer_RequestRepository Transfer_RequestRepository
        {
            get
            {
                transfer_RequestRepository ??= new Transfer_RequestRepository(DatabaseContext);
                return transfer_RequestRepository;
            }
        }

        private IEmployeeTeamWorkRepository employeeTeamWorkrepository;
        public IEmployeeTeamWorkRepository EmployeeTeamWorkRepository
        {
            get
            {
                employeeTeamWorkrepository ??= new EmployeeTeamWorkRepository(DatabaseContext);
                return employeeTeamWorkrepository;

            }
        }
        private IViewMisEmployeeSecurityRepository viewMisEmployeeSecurityRepository;
        public IViewMisEmployeeSecurityRepository ViewMisEmployeeSecurityRepository
        {
            get
            {
                viewMisEmployeeSecurityRepository ??= new ViewMisEmployeeSecurityRepository(DatabaseContext);
                return viewMisEmployeeSecurityRepository;
            }
        }
        private IViewMisEmployeeRepository viewMisEmployeeRepository;

        public IViewMisEmployeeRepository ViewMisEmployeeRepository
        {
            get
            {
                viewMisEmployeeRepository ??= new ViewMisEmployeeRepository(DatabaseContext);
                return viewMisEmployeeRepository;
            }
        }

        private ISystemControlDateRepository systemControlDateRepository;
        public ISystemControlDateRepository SystemControlDateRepository
        {
            get
            {
                systemControlDateRepository ??= new SystemControlDateRepository(DatabaseContext);
                return systemControlDateRepository;
            }
        }


        private ISystemSequenceControlRepository systemSequenceControlRepository;
        public ISystemSequenceControlRepository SystemSequenceControlRepository
        {
            get
            {
                systemSequenceControlRepository ??= new SystemSequenceControlRepository(DatabaseContext);
                return systemSequenceControlRepository;
            }
        }



        private ISystemSequenceStatusRepository systemSequenceStatusRepository;
        public ISystemSequenceStatusRepository SystemSequenceStatusRepository
        {
            get
            {
                systemSequenceStatusRepository ??= new SystemSequenceStatusRepository(DatabaseContext);
                return systemSequenceStatusRepository;
            }
        }
        private ICalendarEventRepository calendarEventRepository;
        public ICalendarEventRepository CalendarEventRepository
        {
            get
            {
                calendarEventRepository ??= new CalendarEventRepository(DatabaseContext);
                return calendarEventRepository;
            }
        }

        private IWorkCompanyUnOfficialHolidaySettingRepository workCompanyUnOfficialHolidaySettingRepository;
        public IWorkCompanyUnOfficialHolidaySettingRepository WorkCompanyUnOfficialHolidaySettingRepository
        {
            get
            {
                workCompanyUnOfficialHolidaySettingRepository ??= new WorkCompanyUnOfficialHolidaySettingRepository(DatabaseContext);
                return workCompanyUnOfficialHolidaySettingRepository;
            }
        }

        private IWorkCompanyUnOfficialHolidayJobCategoryRepository workCompanyUnOfficialHolidayJobCategoryRepository;
        public IWorkCompanyUnOfficialHolidayJobCategoryRepository WorkCompanyUnOfficialHolidayJobCategoryRepository
        {
            get
            {
                workCompanyUnOfficialHolidayJobCategoryRepository ??= new WorkCompanyUnOfficialHolidayJobCategoryRepository(DatabaseContext);
                return workCompanyUnOfficialHolidayJobCategoryRepository;
            }
        }

        private IUnofficialHolidayReasonRepository unofficialHolidayReasonRepository;
        public IUnofficialHolidayReasonRepository UnofficialHolidayReasonRepository
        {
            get
            {
                unofficialHolidayReasonRepository ??= new UnofficialHolidayReasonRepository(DatabaseContext);
                return unofficialHolidayReasonRepository;
            }
        }
        private IWorkTimeShiftConceptRepository workTimeShiftConceptRepository;
        public IWorkTimeShiftConceptRepository WorkTimeShiftConceptRepository
        {
            get
            {
                workTimeShiftConceptRepository ??= new WorkTimeShiftConceptRepository(DatabaseContext);
                return workTimeShiftConceptRepository;
            }
        }
        private ITimeSheetSettingRepository timeSheetSettingRepository;
        public ITimeSheetSettingRepository TimeSheetSettingRepository
        {
            get
            {
                timeSheetSettingRepository ??= new TimeSheetSettingRepository(DatabaseContext);
                return timeSheetSettingRepository;
            }
        }
        private IDismissal_RequestRepository dismissal_RequestRepository;
        public IDismissal_RequestRepository Dismissal_RequestRepository
        {
            get
            {
                dismissal_RequestRepository ??= new Dismissal_RequestRepository(DatabaseContext);
                return dismissal_RequestRepository;
            }
        }
        private IDismissal_StatusRepository dismissal_StatusRepository;
        public IDismissal_StatusRepository Dismissal_StatusRepository
        {
            get
            {
                dismissal_StatusRepository ??= new Dismissal_StatusRepository(DatabaseContext);
                return dismissal_StatusRepository;
            }
        }
        private IEmployeeEntryExitAttendAbsenceRepository employeeEntryExitAttendAbsenceRepository;
        public IEmployeeEntryExitAttendAbsenceRepository EmployeeEntryExitAttendAbsenceRepository
        {
            get
            {
                employeeEntryExitAttendAbsenceRepository ??= new EmployeeEntryExitAttendAbsenceRepository(DatabaseContext);
                return employeeEntryExitAttendAbsenceRepository;
            }
        }

        private ISacrificeOptionSettingRepository sacrificeOptionSettingRepository;
        public ISacrificeOptionSettingRepository SacrificeOptionSettingRepository
        {
            get
            {
                sacrificeOptionSettingRepository ??= new SacrificeOptionSettingRepository(DatabaseContext);
                return sacrificeOptionSettingRepository;
            }
        }
        private ISacrificePercentageSettingRepository sacrificePercentageSettingRepository;
        public ISacrificePercentageSettingRepository SacrificePercentageSettingRepository
        {
            get
            {
                sacrificePercentageSettingRepository ??= new SacrificePercentageSettingRepository(DatabaseContext);
                return sacrificePercentageSettingRepository;
            }
        }
        private ITrainingTypeRepository trainingTypeRepository;
        public ITrainingTypeRepository TrainingTypeRepository
        {
            get
            {
                trainingTypeRepository ??= new TrainingTypeRepository(DatabaseContext);
                return trainingTypeRepository;
            }
        }
        private IPaymentStatusRepository paymentStatusRepository;
        public IPaymentStatusRepository PaymentStatusRepository
        {
            get
            {
                paymentStatusRepository ??= new PaymentStatusRepository(DatabaseContext);
                return paymentStatusRepository;
            }
        }

        private IAnalysisEmployeeAttendAbsenceItemRepository analysisEmployeeAttendAbsenceItemRepository;
        public IAnalysisEmployeeAttendAbsenceItemRepository AnalysisEmployeeAttendAbsenceItemRepository
        {
            get
            {
                analysisEmployeeAttendAbsenceItemRepository ??= new AnalysisEmployeeAttendAbsenceItemRepository(DatabaseContext);
                return analysisEmployeeAttendAbsenceItemRepository;
            }
        }


        private IScheduledLogRepository scheduledLogRepository;
        public IScheduledLogRepository ScheduledLogRepository
        {
            get
            {
                scheduledLogRepository ??= new ScheduledLogRepository(DatabaseContext);
                return scheduledLogRepository;
            }
        }

        private IOfficialMessageRepository officialMessageRepository;
        public IOfficialMessageRepository OfficialMessageRepository
        {
            get
            {
                officialMessageRepository ??= new OfficialMessageRepository(DatabaseContext);
                return officialMessageRepository;
            }
        }
        private IDayNightPercentEmplymentTypeRepository dayNightPercentEmplymentTypeRepository;
        public IDayNightPercentEmplymentTypeRepository DayNightPercentEmplymentTypeRepository
        {
            get
            {
                dayNightPercentEmplymentTypeRepository ??= new DayNightPercentEmplymentTypeRepository(DatabaseContext);
                return dayNightPercentEmplymentTypeRepository;
            }
        }

        private IDayNightRollCallRepository dayNightRollCallRepository;
        public IDayNightRollCallRepository DayNightRollCallRepository
        {
            get
            {
                dayNightRollCallRepository ??= new DayNightRollCallRepository(DatabaseContext);
                return dayNightRollCallRepository;
            }
        }

        private IMonthTimeSheetRepository monthTimeSheetRepository;
        public IMonthTimeSheetRepository MonthTimeSheetRepository
        {
            get
            {
                monthTimeSheetRepository ??= new MonthTimeSheetRepository(DatabaseContext);
                return monthTimeSheetRepository;
            }
        }

        private IMonthTimeSheetDraftRepository monthTimeSheetDraftRepository;
        public IMonthTimeSheetDraftRepository MonthTimeSheetDraftRepository
        {
            get
            {
                monthTimeSheetDraftRepository ??= new MonthTimeSheetDraftRepository(DatabaseContext);
                return monthTimeSheetDraftRepository;
            }
        }

        private IMonthTimeSheetIncludedRepository monthTimeSheetIncludedRepository;
        public IMonthTimeSheetIncludedRepository MonthTimeSheetIncludedRepository
        {
            get
            {
                monthTimeSheetIncludedRepository ??= new MonthTimeSheetIncludedRepository(DatabaseContext);
                return monthTimeSheetIncludedRepository;
            }
        }
        private IMonthTimeSheetLogRepository monthTimeSheetLogRepository;
        public IMonthTimeSheetLogRepository MonthTimeSheetLogRepository
        {
            get
            {
                monthTimeSheetLogRepository ??= new MonthTimeSheetLogRepository(DatabaseContext);
                return monthTimeSheetLogRepository;
            }
        }

        private IMonthTimeSheetRollCallRepository monthTimeSheetRollCallRepository;
        public IMonthTimeSheetRollCallRepository MonthTimeSheetRollCallRepository
        {
            get
            {
                monthTimeSheetRollCallRepository ??= new MonthTimeSheetRollCallRepository(DatabaseContext);
                return monthTimeSheetRollCallRepository;
            }
        }

        private IMonthTimeSheetWorkTimeRepository monthTimeSheetWorkTimeRepository;
        public IMonthTimeSheetWorkTimeRepository MonthTimeSheetWorkTimeRepository
        {
            get
            {
                monthTimeSheetWorkTimeRepository ??= new MonthTimeSheetWorkTimeRepository(DatabaseContext);
                return monthTimeSheetWorkTimeRepository;
            }
        }

        private IViewTimeSheetToMisRepository viewTimeSheetToMisRepository;
        public IViewTimeSheetToMisRepository ViewTimeSheetToMisRepository
        {
            get
            {
                viewTimeSheetToMisRepository ??= new ViewTimeSheetToMisRepository(DatabaseContext);
                return viewTimeSheetToMisRepository;

            }
        }

        private IOverTimeSpecialHolidayTimeSheetSettingRepository overTimeSpecialHolidayTimeSheetSettingRepository;
        public IOverTimeSpecialHolidayTimeSheetSettingRepository OverTimeSpecialHolidayTimeSheetSettingRepository
        {
            get
            {
                overTimeSpecialHolidayTimeSheetSettingRepository ??= new OverTimeSpecialHolidayTimeSheetSettingRepository(DatabaseContext);
                return overTimeSpecialHolidayTimeSheetSettingRepository;

            }
        }

        private IEmployeeTimeSheetRepository employeeTimeSheetRepository;
        public IEmployeeTimeSheetRepository EmployeeTimeSheetRepository
        {
            get
            {
                employeeTimeSheetRepository ??= new EmployeeTimeSheetRepository(DatabaseContext);
                return employeeTimeSheetRepository;

            }
        }
        private IViewTeamManagerRepository viewTeamManagerRepository;
        public IViewTeamManagerRepository ViewTeamManagerRepository
        {
            get
            {
                viewTeamManagerRepository ??= new ViewTeamManagerRepository(DatabaseContext);
                return viewTeamManagerRepository;

            }
        }

        private IMonthTimeShitStepperRepository monthTimeShitStepperRepository;
        public IMonthTimeShitStepperRepository MonthTimeShitStepperRepository
        {
            get
            {
                monthTimeShitStepperRepository ??= new MonthTimeShitStepperRepository(DatabaseContext);
                return monthTimeShitStepperRepository;

            }
        }

        private IViewAttendItemReportRepository viewAttendItemReportRepository;
        public IViewAttendItemReportRepository ViewAttendItemReportRepository
        {
            get
            {
                viewAttendItemReportRepository ??= new ViewAttendItemReportRepository(DatabaseContext);
                return viewAttendItemReportRepository;

            }
        }
        private IViewEmployeeTeamUserActiveRepository viewEmployeeTeamUserActiveRepository;
        public IViewEmployeeTeamUserActiveRepository ViewEmployeeTeamUserActiveRepository
        {
            get
            {
                viewEmployeeTeamUserActiveRepository ??= new ViewEmployeeTeamUserActiveRepository(DatabaseContext);
                return viewEmployeeTeamUserActiveRepository;

            }
        }

        private IMission_LocationRepository mission_LocationRepository;
        public IMission_LocationRepository Mission_LocationRepository
        {
            get
            {
                mission_LocationRepository ??= new Mission_LocationRepository(DatabaseContext);
                return mission_LocationRepository;

            }
        }
        private IMission_RequestRepository mission_RequestRepository;
        public IMission_RequestRepository Mission_RequestRepository
        {
            get
            {
                mission_RequestRepository ??= new Mission_RequestRepository(DatabaseContext);
                return mission_RequestRepository;

            }
        }

        private IMission_TypeRepository mission_TypeRepository;
        public IMission_TypeRepository Mission_TypeRepository
        {
            get
            {
                mission_TypeRepository ??= new Mission_TypeRepository(DatabaseContext);
                return mission_TypeRepository;

            }
        }
        private IMission_GoalRepository mission_GoalRepository;
        public IMission_GoalRepository Mission_GoalRepository
        {
            get
            {
                mission_GoalRepository ??= new Mission_GoalRepository(DatabaseContext);
                return mission_GoalRepository;

            }
        }
        private IMission_TypeAccessLevelRepository mission_TypeAccessLevelRepository;
        public IMission_TypeAccessLevelRepository Mission_TypeAccessLevelRepository
        {
            get
            {
                mission_TypeAccessLevelRepository ??= new Mission_TypeAccessLevelRepository(DatabaseContext);
                return mission_TypeAccessLevelRepository;

            }
        }
        private IDayTimeSettingRepository dayTimeSettingRepository;
        public IDayTimeSettingRepository DayTimeSettingRepository
        {
            get
            {
                dayTimeSettingRepository ??= new DayTimeSettingRepository(DatabaseContext);
                return dayTimeSettingRepository;

            }
        }
        private IFloatTimeSettingRepository floatTimeSettingRepository;
        public IFloatTimeSettingRepository FloatTimeSettingRepository
        {
            get
            {
                floatTimeSettingRepository ??= new FloatTimeSettingRepository(DatabaseContext);
                return floatTimeSettingRepository;

            }
        }

        private IViewNotConfirmedEducationRepository viewNotConfirmedEducationRepository;
        public IViewNotConfirmedEducationRepository ViewNotConfirmedEducationRepository
        {
            get
            {
                viewNotConfirmedEducationRepository ??= new ViewNotConfirmedEducationRepository(DatabaseContext);
                return viewNotConfirmedEducationRepository;

            }
        }

        private IStudyFieldRepository studyFieldRepository;

        public IStudyFieldRepository StudyFieldRepository
        {
            get
            {
                studyFieldRepository ??= new StudyFieldRepository(DatabaseContext);
                return studyFieldRepository;

            }
        }

        private IEducationRepository educationRepository;
        public IEducationRepository EducationRepository
        {
            get
            {
                educationRepository ??= new EducationRepository(DatabaseContext);
                return educationRepository;

            }
        }


        private IChart_JobPositionScoreTypeRepository JobPositionScoreTypeRepository;
        public IChart_JobPositionScoreTypeRepository Chart_JobPositionScoreTypeRepository
        {
            get
            {
                JobPositionScoreTypeRepository ??= new Chart_JobPositionScoreTypeRepository(DatabaseContext);
                return JobPositionScoreTypeRepository;

            }
        }

        private IChart_JobCertificateRepository ChartJobCertificateRepository;
        public IChart_JobCertificateRepository Chart_JobCertificateRepository
        {
            get
            {
                ChartJobCertificateRepository ??= new Chart_JobCertificateRepository(DatabaseContext);
                return ChartJobCertificateRepository;

            }
        }




        //

        private IChart_JobPositionCertificateRepository JobPositionCertificateRepository;
        public IChart_JobPositionCertificateRepository Chart_JobPositionCertificateRepository
        {
            get
            {
                JobPositionCertificateRepository ??= new Chart_JobPositionCertificateRepository(DatabaseContext);
                return JobPositionCertificateRepository;

            }
        }
        //


        private IChart_JobPositionFieldRepository JobPositionFieldRepository;
        public IChart_JobPositionFieldRepository Chart_JobPositionFieldRepository
        {
            get
            {
                JobPositionFieldRepository ??= new Chart_JobPositionFieldRepository(DatabaseContext);
                return JobPositionFieldRepository;

            }
        }


        private IChart_JobCategoryDefinationRepository JobCategoryDefinationRepository;
        public IChart_JobCategoryDefinationRepository Chart_JobCategoryDefinationRepository
        {
            get
            {
                JobCategoryDefinationRepository ??= new Chart_JobCategoryDefinationRepository(DatabaseContext);
                return JobCategoryDefinationRepository;

            }
        }

        private IChart_JobPositionRepository chart_JobPositionRepository;
        public IChart_JobPositionRepository Chart_JobPositionRepository
        {
            get
            {
                chart_JobPositionRepository ??= new Chart_JobPositionRepository(DatabaseContext);
                return chart_JobPositionRepository;

            }
        }


        private IChart_JobIdentityRepository chart_JobIdentityRepository;
        public IChart_JobIdentityRepository Chart_JobIdentityRepository
        {
            get
            {
                chart_JobIdentityRepository ??= new Chart_JobIdentityRepository(DatabaseContext);
                return chart_JobIdentityRepository;

            }
        }

        private IChart_JobPositionStatusRepository chart_JobPositionStatusRepository;
        public IChart_JobPositionStatusRepository Chart_JobPositionStatusRepository
        {
            get
            {
                chart_JobPositionStatusRepository ??= new Chart_JobPositionStatusRepository(DatabaseContext);
                return chart_JobPositionStatusRepository;

            }
        }
        private IJobGroupRepository jobGroupRepository;
        public IJobGroupRepository JobGroupRepository
        {
            get
            {
                jobGroupRepository ??= new JobGroupRepository(DatabaseContext);
                return jobGroupRepository;

            }
        }



        private IWF_WorkFlowActionRepository wF_WorkFlowActionRepository;
        public IWF_WorkFlowActionRepository WF_WorkFlowActionRepository
        {
            get
            {
                wF_WorkFlowActionRepository ??= new WF_WorkFlowActionRepository(DatabaseContext);
                return wF_WorkFlowActionRepository;

            }
        }


        private IChart_JobPositionStatusCategoryRepository chart_JobPositionStatusCategoryRepository;
        public IChart_JobPositionStatusCategoryRepository Chart_JobPositionStatusCategoryRepository
        {
            get
            {
                chart_JobPositionStatusCategoryRepository ??= new Chart_JobPositionStatusCategoryRepository(DatabaseContext);
                return chart_JobPositionStatusCategoryRepository;

            }
        }


        private IJobPositionNatureRepository jobPositionNatureRepository;
        public IJobPositionNatureRepository Chart_JobPositionNatureRepository
        {
            get
            {
                jobPositionNatureRepository ??= new JobPositionNatureRepository(DatabaseContext);
                return jobPositionNatureRepository;

            }
        }

        private IJobCategoryRepository jobCategoryRepository;
        public IJobCategoryRepository JobCategoryRepository
        {
            get
            {
                jobCategoryRepository ??= new JobCategoryRepository(DatabaseContext);
                return jobCategoryRepository;
            }
        }

        private IJobCategoryEducationRepository jobCategoryEducationRepository;
        public IJobCategoryEducationRepository JobCategoryEducationRepository
        {
            get
            {
                jobCategoryEducationRepository ??= new JobCategoryEducationRepository(DatabaseContext);
                return jobCategoryEducationRepository;
            }
        }


        private IJobPositionIncreasePercentRepository jobPositionIncreasePercentRepository;
        public IJobPositionIncreasePercentRepository JobPositionIncreasePercentRepository
        {
            get
            {
                jobPositionIncreasePercentRepository ??= new JobPositionIncreasePercentRepository(DatabaseContext);
                return jobPositionIncreasePercentRepository;
            }
        }


        private IJobPositionScoreRepository jobPositionScoreRepository;
        public IJobPositionScoreRepository JobPositionScoreRepository
        {
            get
            {
                jobPositionScoreRepository ??= new JobPositionScoreRepository(DatabaseContext);
                return jobPositionScoreRepository;
            }
        }
        private IMission_TypeLocationRepository mission_TypeLocationRepository;
        public IMission_TypeLocationRepository Mission_TypeLocationRepository
        {
            get
            {
                mission_TypeLocationRepository ??= new Mission_TypeLocationRepository(DatabaseContext);
                return mission_TypeLocationRepository;
            }
        }

        private IChart_StructureRepository chart_StructureRepository;
        public IChart_StructureRepository Chart_StructureRepository
        {
            get
            {
                chart_StructureRepository ??= new Chart_StructureRepository(DatabaseContext);
                return chart_StructureRepository;
            }
        }


        private IRewardSpecificRepository rewardSpecificRepository;
        public IRewardSpecificRepository RewardSpecificRepository
        {
            get
            {
                rewardSpecificRepository ??= new RewardSpecificRepository(DatabaseContext);
                return rewardSpecificRepository;
            }
        }
        private IStepper_ProcedureRepository stepper_ProcedureRepository;
        public IStepper_ProcedureRepository Stepper_ProcedureRepository
        {
            get
            {
                stepper_ProcedureRepository ??= new Stepper_ProcedureRepository(DatabaseContext);
                return stepper_ProcedureRepository;
            }
        }
        private IStepper_ProcedureAccessLevelRepository stepper_ProcedureAccessLevelRepository;
        public IStepper_ProcedureAccessLevelRepository Stepper_ProcedureAccessLevelRepository
        {
            get
            {
                stepper_ProcedureAccessLevelRepository ??= new Stepper_ProcedureAccessLevelRepository(DatabaseContext);
                return stepper_ProcedureAccessLevelRepository;
            }
        }
        private IStepper_ProcedureLogRepository stepper_ProcedureLogRepository;
        public IStepper_ProcedureLogRepository Stepper_ProcedureLogRepository
        {
            get
            {
                stepper_ProcedureLogRepository ??= new Stepper_ProcedureLogRepository(DatabaseContext);
                return stepper_ProcedureLogRepository;
            }
        }
        private IStepper_ProcedureStatusRepository stepper_ProcedureStatusRepository;

        public IStepper_ProcedureStatusRepository Stepper_ProcedureStatusRepository
        {
            get
            {
                stepper_ProcedureStatusRepository ??= new Stepper_ProcedureStatusRepository(DatabaseContext);
                return stepper_ProcedureStatusRepository;
            }
        }


        public IStepper_StatusSystemMonthRepository stepper_StatusSystemMonthRepository;
        public IStepper_StatusSystemMonthRepository Stepper_StatusSystemMonthRepository
        {
            get
            {
                stepper_StatusSystemMonthRepository ??= new Stepper_StatusSystemMonthRepository(DatabaseContext);
                return stepper_StatusSystemMonthRepository;
            }
        }

        private IJobPositionHistoryRepository jobPositionHistoryRepository;
        public IJobPositionHistoryRepository JobPositionHistoryRepository
        {
            get
            {
                jobPositionHistoryRepository ??= new JobPositionHistoryRepository(DatabaseContext);
                return jobPositionHistoryRepository;
            }
        }

        private IViewJobPositionRepository viewJobPositionRepository;
        public IViewJobPositionRepository ViewJobPositionRepository
        {
            get
            {
                viewJobPositionRepository ??= new ViewJobPositionRepository(DatabaseContext);
                return viewJobPositionRepository;
            }
        }
        private IStructureTypeRepository structureTypeRepository;
        public IStructureTypeRepository StructureTypeRepository
        {
            get
            {
                structureTypeRepository ??= new StructureTypeRepository(DatabaseContext);
                return structureTypeRepository;
            }
        }


        private IInsuranceTypeRepository insuranceTypeRepository;
        public IInsuranceTypeRepository InsuranceTypeRepository
        {
            get
            {
                insuranceTypeRepository ??= new InsuranceTypeRepository(DatabaseContext);
                return insuranceTypeRepository;

            }
        }
        private IInsuranceListRepository insuranceListRepository;
        public IInsuranceListRepository InsuranceListRepository
        {
            get
            {
                insuranceListRepository ??= new InsuranceListRepository(DatabaseContext);
                return insuranceListRepository;

            }
        }

        private IBloodTypeRepository bloodTypeRepository;
        public IBloodTypeRepository BloodTypeRepository
        {
            get
            {
                bloodTypeRepository ??= new BloodTypeRepository(DatabaseContext);
                return bloodTypeRepository;
            }
        }

        private IIsarStatusRepository isarStatusRepository;
        public IIsarStatusRepository IsarStatusRepository
        {
            get
            {
                isarStatusRepository ??= new IsarStatusRepository(DatabaseContext);
                return isarStatusRepository;
            }
        }


        private IEmploymentTypeRepository employmentTypeRepository;

        public IEmploymentTypeRepository EmploymentTypeRepository
        {
            get
            {
                employmentTypeRepository ??= new EmploymentTypeRepository(DatabaseContext);
                return employmentTypeRepository;
            }
        }
        private IMaritalStatusRepository maritalStatusRepository;
        public IMaritalStatusRepository MaritalStatusRepository
        {
            get
            {
                maritalStatusRepository ??= new MaritalStatusRepository(DatabaseContext);
                return maritalStatusRepository;
            }
        }



        private IEmploymentStatusRepository employmentstatusRepository;

        public IEmploymentStatusRepository EmploymentStatusRepository
        {
            get
            {
                employmentstatusRepository ??= new EmploymentStatusRepository(DatabaseContext);
                return employmentstatusRepository;
            }
        }
        private IEmployeeEducationDegreeRepository employeeEducationDegreeRepository;

        public IEmployeeEducationDegreeRepository EmployeeEducationDegreeRepository
        {
            get
            {
                employeeEducationDegreeRepository ??= new EmployeeEducationDegreeRepository(DatabaseContext);
                return employeeEducationDegreeRepository;
            }
        }
        private ISavingTypeRepository savingTypeRepository;

        public ISavingTypeRepository SavingTypeRepository
        {
            get
            {
                savingTypeRepository ??= new SavingTypeRepository(DatabaseContext);
                return savingTypeRepository;
            }
        }

        private IRegionRepository regionRepository;
        public IRegionRepository RegionRepository
        {
            get
            {
                regionRepository ??= new RegionRepository(DatabaseContext);
                return regionRepository;
            }
        }


        private INationalityRepository nationalityRepository;
        public INationalityRepository NationalityRepository
        {
            get
            {
                nationalityRepository ??= new NationalityRepository(DatabaseContext);
                return nationalityRepository;
            }
        }


        private IGenderRepository genderRepository;
        public IGenderRepository GenderRepository
        {
            get
            {
                genderRepository ??= new GenderRepository(DatabaseContext);
                return genderRepository;
            }
        }

        private IMilitaryDegreeRepository militaryDegreeRepository;
        public IMilitaryDegreeRepository MilitaryDegreeRepository
        {
            get
            {
                militaryDegreeRepository ??= new MilitaryDegreeRepository(DatabaseContext);
                return militaryDegreeRepository;

            }
        }
        private IMilitaryTypeRepository militaryTypeRepository;
        public IMilitaryTypeRepository MilitaryTypeRepository
        {
            get
            {
                militaryTypeRepository ??= new MilitaryTypeRepository(DatabaseContext);
                return militaryTypeRepository;
            }
        }


        private IDependenceJobRepository dependenceJobRepository;
        public IDependenceJobRepository DependenceJobRepository
        {
            get
            {
                dependenceJobRepository ??= new DependenceJobRepository(DatabaseContext);
                return dependenceJobRepository;
            }
        }

        private IDependenceReasonRepository dependenceReasonRepository;
        public IDependenceReasonRepository DependenceReasonRepository
        {
            get
            {
                dependenceReasonRepository ??= new DependenceReasonRepository(DatabaseContext);
                return dependenceReasonRepository;
            }
        }

        private IDependenceTypeRepository dependenceTypeRepository;
        public IDependenceTypeRepository DependenceTypeRepository
        {
            get
            {
                dependenceTypeRepository ??= new DependenceTypeRepository(DatabaseContext);
                return dependenceTypeRepository;
            }
        }

        private IDependentExitDateReasonRepository dependentExitDateReasonRepository;
        public IDependentExitDateReasonRepository DependentExitDateReasonRepository
        {
            get
            {
                dependentExitDateReasonRepository ??= new DependentExitDateReasonRepository(DatabaseContext);
                return dependentExitDateReasonRepository;
            }
        }


        private IFamilyRepository familyRepository;
        public IFamilyRepository FamilyRepository
        {
            get
            {
                familyRepository ??= new FamilyRepository(DatabaseContext);
                return familyRepository;
            }
        }

        private IMilitaryUnitRepository militaryUnitRepository;
        public IMilitaryUnitRepository MilitaryUnitRepository
        {
            get
            {
                militaryUnitRepository ??= new MilitaryUnitRepository(DatabaseContext);
                return militaryUnitRepository;

            }
        }
        private IMilitaryStatusRepository militaryStatusRepository;
        public IMilitaryStatusRepository MilitaryStatusRepository
        {
            get
            {
                militaryStatusRepository ??= new MilitaryStatusRepository(DatabaseContext);
                return militaryStatusRepository;

            }
        }
        private IMilitaryExemptionReasonRepository militaryExemptionReasonRepository;
        public IMilitaryExemptionReasonRepository MilitaryExemptionReasonRepository
        {
            get
            {
                militaryExemptionReasonRepository ??= new MilitaryExemptionReasonRepository(DatabaseContext);
                return militaryExemptionReasonRepository;

            }
        }
        private IAccountBankTypeRepository accountBankTypeRepository;
        public IAccountBankTypeRepository AccountBankTypeRepository
        {
            get
            {
                accountBankTypeRepository ??= new AccountBankTypeRepository(DatabaseContext);
                return accountBankTypeRepository;

            }
        }



        private IEmployeePaymentStatusRepository employeePaymentStatusRepository;
        public IEmployeePaymentStatusRepository EmployeePaymentStatusRepository
        {
            get
            {
                employeePaymentStatusRepository ??= new EmployeePaymentStatusRepository(DatabaseContext);
                return employeePaymentStatusRepository;

            }
        }

        private IEmployeeJobPositionRepository employeeJobPositionRepository;
        public IEmployeeJobPositionRepository EmployeeJobPositionRepository
        {
            get
            {
                employeeJobPositionRepository ??= new EmployeeJobPositionRepository(DatabaseContext);
                return employeeJobPositionRepository;

            }
        }
        private IEmployeeOtherPaymentTypeRepository employeeOtherPaymentTypeRepository;
        public IEmployeeOtherPaymentTypeRepository EmployeeOtherPaymentTypeRepository
        {
            get
            {
                employeeOtherPaymentTypeRepository ??= new EmployeeOtherPaymentTypeRepository(DatabaseContext);
                return employeeOtherPaymentTypeRepository;

            }
        }


        private IEmployeeAccountBankRepository employeeAccountBankRepository;
        public IEmployeeAccountBankRepository EmployeeAccountBankRepository
        {
            get
            {
                employeeAccountBankRepository ??= new EmployeeAccountBankRepository(DatabaseContext);
                return employeeAccountBankRepository;

            }
        }


        // TableDefinition
        private ITableDefinitionRepository tableDefinitionRepository;
        public ITableDefinitionRepository TableDefinitionRepository
        {
            get
            {
                tableDefinitionRepository ??= new TableDefinitionRepository(DatabaseContext);
                return tableDefinitionRepository;

            }
        }

        private IEmployeePictureRepository employeePictureRepository;
        public IEmployeePictureRepository EmployeePictureRepository
        {
            get
            {
                employeePictureRepository ??= new EmployeePictureRepository(DatabaseContext);
                return employeePictureRepository;

            }
        }

        public IOperationTypeRepository operationTypeRepository;
        public IOperationTypeRepository OperationTypeRepository
        {
            get
            {
                operationTypeRepository ??= new OperationTypeRepository(DatabaseContext);
                return operationTypeRepository;

            }
        }

        public IEntityTypeRepository entityTypeRepository;
        public IEntityTypeRepository EntityTypeRepository
        {
            get
            {
                entityTypeRepository ??= new EntityTypeRepository(DatabaseContext);
                return entityTypeRepository;

            }
        }

        public ILogDataRepository logDataRepository;
        public ILogDataRepository LogDataRepository
        {
            get
            {
                logDataRepository ??= new LogDataRepository(DatabaseContext);
                return logDataRepository;

            }
        }
        private IInsuranceBookletRepository insuranceBookletRepository;
        public IInsuranceBookletRepository InsuranceBookletRepository
        {
            get
            {
                insuranceBookletRepository ??= new InsuranceBookletRepository(DatabaseContext);
                return insuranceBookletRepository;

            }
        }


        private IPersonalTypeRepository personalTypeRepository;
        public IPersonalTypeRepository PersonalTypeRepository
        {
            get
            {
                personalTypeRepository ??= new PersonalTypeRepository(DatabaseContext);
                return personalTypeRepository;

            }
        }



        private IFranchiseTypeRepository franchiseTypeRepository;
        public IFranchiseTypeRepository FranchiseTypeRepository
        {
            get
            {
                franchiseTypeRepository ??= new FranchiseTypeRepository(DatabaseContext);
                return franchiseTypeRepository;

            }
        }

        private IBookletEmployeeWithFamilyAndBankRepository bookletEmployeeWithFamilyAndBankRepository;
        public IBookletEmployeeWithFamilyAndBankRepository BookletEmployeeWithFamilyAndBankRepository
        {
            get
            {
                bookletEmployeeWithFamilyAndBankRepository ??= new BookletEmployeeWithFamilyAndBankRepository(DatabaseContext);
                return bookletEmployeeWithFamilyAndBankRepository;

            }
        }
        private IViewGeneralDataEmployeeWithFamilyRepository generalDataEmployeeWithFamilyRepository;
        public IViewGeneralDataEmployeeWithFamilyRepository ViewGeneralDataEmployeeWithFamilyRepository
        {
            get
            {
                generalDataEmployeeWithFamilyRepository ??= new ViewGeneralDataEmployeeWithFamilyRepository(DatabaseContext);
                return generalDataEmployeeWithFamilyRepository;

            }
        }

        private IViewGeneralDataEmployeeWithFamilyForReportRepository generalDataEmployeeWithFamilyForReportRepository;
        public IViewGeneralDataEmployeeWithFamilyForReportRepository ViewGeneralDataEmployeeWithFamilyForReportRepository
        {
            get
            {
                generalDataEmployeeWithFamilyForReportRepository ??= new ViewGeneralDataEmployeeWithFamilyForReportRepository(DatabaseContext);
                return generalDataEmployeeWithFamilyForReportRepository;

            }
        }

        private IViewEmployeeFamilyReportRepository viewEmployeeFamilyReportRepository;
        public IViewEmployeeFamilyReportRepository ViewEmployeeFamilyReportRepository
        {
            get
            {
                viewEmployeeFamilyReportRepository ??= new ViewEmployeeFamilyReportRepository(DatabaseContext);
                return viewEmployeeFamilyReportRepository;

            }
        }
        private IViewEmployeeWithFamilyActiveRepository viewEmployeeWithFamilyActiveRepository;
        public IViewEmployeeWithFamilyActiveRepository ViewEmployeeWithFamilyActiveRepository
        {
            get
            {
                viewEmployeeWithFamilyActiveRepository ??= new ViewEmployeeWithFamilyActiveRepository(DatabaseContext);
                return viewEmployeeWithFamilyActiveRepository;

            }
        }

        private IViewOdsTherapyBookletMisRepository viewOdsTherapyBookletMisRepository;
        public IViewOdsTherapyBookletMisRepository ViewOdsTherapyBookletMisRepository
        {
            get
            {
                viewOdsTherapyBookletMisRepository ??= new ViewOdsTherapyBookletMisRepository(DatabaseContext);
                return viewOdsTherapyBookletMisRepository;

            }
        }
        private IViewShiftBoardRepository viewShiftBoardRepository;
        public IViewShiftBoardRepository ViewShiftBoardRepository
        {
            get
            {
                viewShiftBoardRepository ??= new ViewShiftBoardRepository(DatabaseContext);
                return viewShiftBoardRepository;

            }
        }
        private IHrOptionRepository hrOptionRepository;
        public IHrOptionRepository HrOptionRepository
        {
            get
            {
                hrOptionRepository ??= new HrOptionRepository(DatabaseContext);
                return hrOptionRepository;

            }
        }



        private IAccountCodeRepository accountCodeRepository;
        public IAccountCodeRepository AccountCodeRepository
        {
            get
            {
                accountCodeRepository ??= new AccountCodeRepository(DatabaseContext);
                return accountCodeRepository;

            }
        }
        private IOtherPaymentStatusFlowRepository otherPaymentStatusFlowRepository;
        public IOtherPaymentStatusFlowRepository OtherPaymentStatusFlowRepository
        {
            get
            {
                otherPaymentStatusFlowRepository ??= new OtherPaymentStatusFlowRepository(DatabaseContext);
                return otherPaymentStatusFlowRepository;

            }
        }
        private IOtherPaymentStatusRepository otherPaymentStatusRepository;
        public IOtherPaymentStatusRepository OtherPaymentStatusRepository
        {
            get
            {
                otherPaymentStatusRepository ??= new OtherPaymentStatusRepository(DatabaseContext);
                return otherPaymentStatusRepository;

            }
        }


        private IOtherPaymentTypeRepository otherPaymentTypeRepository;
        public IOtherPaymentTypeRepository OtherPaymentTypeRepository
        {
            get
            {
                otherPaymentTypeRepository ??= new OtherPaymentTypeRepository(DatabaseContext);
                return otherPaymentTypeRepository;

            }
        }



        private IOtherPaymentSettingRepository otherPaymentSettingRepository;
        public IOtherPaymentSettingRepository OtherPaymentSettingRepository
        {
            get
            {
                otherPaymentSettingRepository ??= new OtherPaymentSettingRepository(DatabaseContext);
                return otherPaymentSettingRepository;

            }
        }



        private IEmployeeOtherPaymentRepository employeeOtherPaymentRepository;
        public IEmployeeOtherPaymentRepository EmployeeOtherPaymentRepository
        {
            get
            {
                employeeOtherPaymentRepository ??= new EmployeeOtherPaymentRepository(DatabaseContext);
                return employeeOtherPaymentRepository;

            }
        }
        private IEmployeeDeductionHeaderRepository employeeDeductionHeaderRepository;
        public IEmployeeDeductionHeaderRepository EmployeeDeductionHeaderRepository
        {
            get
            {
                employeeDeductionHeaderRepository ??= new EmployeeDeductionHeaderRepository(DatabaseContext);
                return employeeDeductionHeaderRepository;

            }
        }

        private IOtherPaymentHeaderRepository otherPaymentHeaderRepository;
        public IOtherPaymentHeaderRepository OtherPaymentHeaderRepository
        {
            get
            {
                otherPaymentHeaderRepository ??= new OtherPaymentHeaderRepository(DatabaseContext);
                return otherPaymentHeaderRepository;

            }
        }
        private IOtherPaymentDetailRepository otherPaymentDetailRepository;

        public IOtherPaymentDetailRepository OtherPaymentDetailRepository
        {
            get
            {
                otherPaymentDetailRepository ??= new OtherPaymentDetailRepository(DatabaseContext);
                return otherPaymentDetailRepository;

            }
        }
        private IEmployeeOtherPaymentHistoryRepository employeeOtherPaymentHistoryRepository;

        public IEmployeeOtherPaymentHistoryRepository EmployeeOtherPaymentHistoryRepository
        {
            get
            {
                employeeOtherPaymentHistoryRepository ??= new EmployeeOtherPaymentHistoryRepository(DatabaseContext);
                return employeeOtherPaymentHistoryRepository;

            }
        }



        private IKUnitSettingRepository kUnitSettingRepository;

        public IKUnitSettingRepository KUnitSettingRepository
        {
            get
            {
                kUnitSettingRepository ??= new KUnitSettingRepository(DatabaseContext);
                return kUnitSettingRepository;

            }
        }

        private IOtherPaymentSettingParameterRepository otherPaymentSettingParameterRepository;

        public IOtherPaymentSettingParameterRepository OtherPaymentSettingParameterRepository
        {
            get
            {
                otherPaymentSettingParameterRepository ??= new OtherPaymentSettingParameterRepository(DatabaseContext);
                return otherPaymentSettingParameterRepository;

            }
        }


        private IOtherPaymentSettingParameterValueRepository otherPaymentSettingParameterValueRepository;

        public IOtherPaymentSettingParameterValueRepository OtherPaymentSettingParameterValueRepository
        {
            get
            {
                otherPaymentSettingParameterValueRepository ??= new OtherPaymentSettingParameterValueRepository(DatabaseContext);
                return otherPaymentSettingParameterValueRepository;

            }
        }
        private IOtherPaymentHeaderTypeRepository otherPaymentHeaderTypeRepository;

        public IOtherPaymentHeaderTypeRepository OtherPaymentHeaderTypeRepository
        {
            get
            {
                otherPaymentHeaderTypeRepository ??= new OtherPaymentHeaderTypeRepository(DatabaseContext);
                return otherPaymentHeaderTypeRepository;

            }
        }
        private IEmployeeBlackListRepository employeeBlackListRepository;

        public IEmployeeBlackListRepository EmployeeBlackListRepository
        {
            get
            {
                employeeBlackListRepository ??= new EmployeeBlackListRepository(DatabaseContext);
                return employeeBlackListRepository;

            }
        }

        private IOtherPaymentPercentageRepository otherPaymentPercentageRepository;

        public IOtherPaymentPercentageRepository OtherPaymentPercentageRepository
        {
            get
            {
                otherPaymentPercentageRepository ??= new OtherPaymentPercentageRepository(DatabaseContext);
                return otherPaymentPercentageRepository;

            }
        }
        private IMission_TypeLocationWorkCityRepository mission_TypeLocationWorkCityRepository;

        public IMission_TypeLocationWorkCityRepository Mission_TypeLocationWorkCityRepository
        {
            get
            {
                mission_TypeLocationWorkCityRepository ??= new Mission_TypeLocationWorkCityRepository(DatabaseContext);
                return mission_TypeLocationWorkCityRepository;

            }
        }

        private IViewOdsEmployeeWithChartDataRepository viewOdsEmployeeWithChartDataRepository;

        public IViewOdsEmployeeWithChartDataRepository ViewOdsEmployeeWithChartDataRepository
        {
            get
            {
                viewOdsEmployeeWithChartDataRepository ??= new ViewOdsEmployeeWithChartDataRepository(DatabaseContext);
                return viewOdsEmployeeWithChartDataRepository;

            }
        }

        private IViewOnCallRequestRepository viewOnCallRequestRepository;

        public IViewOnCallRequestRepository ViewOnCallRequestRepository
        {
            get
            {
                viewOnCallRequestRepository ??= new ViewOnCallRequestRepository(DatabaseContext);
                return viewOnCallRequestRepository;

            }
        }


        private IEmployeeVacationHistoryRepository employeeVacationHistoryRepository;
        public IEmployeeVacationHistoryRepository EmployeeVacationHistoryRepository
        {
            get
            {
                employeeVacationHistoryRepository ??= new EmployeeVacationHistoryRepository(DatabaseContext);
                return employeeVacationHistoryRepository;

            }
        }
        private IEmployeeOtherPaymentTemplateRepository employeeOtherPaymentTemplateRepository;
        public IEmployeeOtherPaymentTemplateRepository EmployeeOtherPaymentTemplateRepository
        {
            get
            {
                employeeOtherPaymentTemplateRepository ??= new EmployeeOtherPaymentTemplateRepository(DatabaseContext);
                return employeeOtherPaymentTemplateRepository;

            }
        }

        private IEmployeeValidOverTimeRepository employeeValidOverTimeRepository;
        public IEmployeeValidOverTimeRepository EmployeeValidOverTimeRepository
        {
            get
            {
                employeeValidOverTimeRepository ??= new EmployeeValidOverTimeRepository(DatabaseContext);
                return employeeValidOverTimeRepository;

            }
        }

        private IVacationRepository vacationRepository;
        public IVacationRepository VacationRepository
        {
            get
            {
                vacationRepository ??= new VacationRepository(DatabaseContext);
                return vacationRepository;

            }
        }

        private IEmployeeVacationManagementRepository employeeVacationManagementRepository;
        public IEmployeeVacationManagementRepository EmployeeVacationManagementRepository
        {
            get
            {
                employeeVacationManagementRepository ??= new EmployeeVacationManagementRepository(DatabaseContext);
                return employeeVacationManagementRepository;

            }
        }
        private IRewardTypeRepository rewardTypeRepository;
        public IRewardTypeRepository RewardTypeRepository
        {
            get
            {
                rewardTypeRepository ??= new RewardTypeRepository(DatabaseContext);
                return rewardTypeRepository;

            }
        }
        private IRewardLevelRepository rewardLevelRepository;
        public IRewardLevelRepository RewardLevelRepository
        {
            get
            {
                rewardLevelRepository ??= new RewardLevelRepository(DatabaseContext);
                return rewardLevelRepository;

            }
        }

        private IStudentRewardSettingRepository studentRewardSettingRepository;
        public IStudentRewardSettingRepository StudentRewardSettingRepository
        {
            get
            {
                studentRewardSettingRepository ??= new StudentRewardSettingRepository(DatabaseContext);
                return studentRewardSettingRepository;

            }
        }
        private IStudentRewardRequestRepository studentRewardRequestRepository;
        public IStudentRewardRequestRepository StudentRewardRequestRepository
        {
            get
            {
                studentRewardRequestRepository ??= new StudentRewardRequestRepository(DatabaseContext);
                return studentRewardRequestRepository;

            }
        }

        private IEmployeeVacationSoldRepository employeeVacationSoldRepository;
        public IEmployeeVacationSoldRepository EmployeeVacationSoldRepository
        {
            get
            {
                employeeVacationSoldRepository ??= new EmployeeVacationSoldRepository(DatabaseContext);
                return employeeVacationSoldRepository;

            }
        }



        private IAccountEmploymentTypeRepository accountEmploymentTypeRepository;
        public IAccountEmploymentTypeRepository AccountEmploymentTypeRepository
        {
            get
            {
                accountEmploymentTypeRepository ??= new AccountEmploymentTypeRepository(DatabaseContext);
                return accountEmploymentTypeRepository;

            }
        }

        private IAccountPaymentTypeSettingRepository accountPaymentTypeSettingRepository;
        public IAccountPaymentTypeSettingRepository AccountPaymentTypeSettingRepository
        {
            get
            {
                accountPaymentTypeSettingRepository ??= new AccountPaymentTypeSettingRepository(DatabaseContext);
                return accountPaymentTypeSettingRepository;

            }
        }



        private IBasisSalaryItemPerGroupRepository basisSalaryItemPerGroupRepository;
        public IBasisSalaryItemPerGroupRepository BasisSalaryItemPerGroupRepository
        {
            get
            {
                basisSalaryItemPerGroupRepository ??= new BasisSalaryItemPerGroupRepository(DatabaseContext);
                return basisSalaryItemPerGroupRepository;

            }
        }
        ///
        private IEmployeeInterdictRepository employeeInterdictRepository;
        public IEmployeeInterdictRepository EmployeeInterdictRepository
        {
            get
            {
                employeeInterdictRepository ??= new EmployeeInterdictRepository(DatabaseContext);
                return employeeInterdictRepository;

            }
        }
        private IEmployeeInterdictDetailRepository employeeInterdictDetailRepository;
        public IEmployeeInterdictDetailRepository EmployeeInterdictDetailRepository
        {
            get
            {
                employeeInterdictDetailRepository ??= new EmployeeInterdictDetailRepository(DatabaseContext);
                return employeeInterdictDetailRepository;

            }
        }
        private IInterdictTypeRepository interdictTypeRepository;
        public IInterdictTypeRepository InterdictTypeRepository
        {
            get
            {
                interdictTypeRepository ??= new InterdictTypeRepository(DatabaseContext);
                return interdictTypeRepository;

            }
        }
        private IInterdictStatusRepository interdictStatusRepository;
        public IInterdictStatusRepository InterdictStatusRepository
        {
            get
            {
                interdictStatusRepository ??= new InterdictStatusRepository(DatabaseContext);
                return interdictStatusRepository;

            }
        }
        private IInterdictDescriptionRepository interdictDescriptionRepository;
        public IInterdictDescriptionRepository InterdictDescriptionRepository
        {
            get
            {
                interdictDescriptionRepository ??= new InterdictDescriptionRepository(DatabaseContext);
                return interdictDescriptionRepository;

            }
        }


        private ISignatureImageRepository signatureImageRepository;
        public ISignatureImageRepository SignatureImageRepository
        {
            get
            {
                signatureImageRepository ??= new SignatureImageRepository(DatabaseContext);
                return signatureImageRepository;

            }
        }
        private IReasonJobMovingRepository reasonJobMovingRepository;
        public IReasonJobMovingRepository ReasonJobMovingRepository
        {
            get
            {
                reasonJobMovingRepository ??= new ReasonJobMovingRepository(DatabaseContext);
                return reasonJobMovingRepository;

            }
        }


        private IBasisSalaryItemRepository basisSalaryItemRepository;
        public IBasisSalaryItemRepository BasisSalaryItemRepository
        {
            get
            {
                basisSalaryItemRepository ??= new BasisSalaryItemRepository(DatabaseContext);
                return basisSalaryItemRepository;

            }
        }
        private IInterdictMaritalSettingRepository interdictMaritalSettingRepository;
        public IInterdictMaritalSettingRepository InterdictMaritalSettingRepository
        {
            get
            {
                interdictMaritalSettingRepository ??= new InterdictMaritalSettingRepository(DatabaseContext);
                return interdictMaritalSettingRepository;

            }
        }
        private IInterdictMaritalSettingDetailRepository interdictMaritalSettingDetailRepository;
        public IInterdictMaritalSettingDetailRepository InterdictMaritalSettingDetailRepository
        {
            get
            {
                interdictMaritalSettingDetailRepository ??= new InterdictMaritalSettingDetailRepository(DatabaseContext);
                return interdictMaritalSettingDetailRepository;

            }
        }


        private ICoefficientRepository coefficientRepository;
        public ICoefficientRepository CoefficientRepository
        {
            get
            {
                coefficientRepository ??= new CoefficientRepository(DatabaseContext);
                return coefficientRepository;

            }
        }


        private ICoefficientSettingRepository coefficientSettingRepository;
        public ICoefficientSettingRepository CoefficientSettingRepository
        {
            get
            {
                coefficientSettingRepository ??= new CoefficientSettingRepository(DatabaseContext);
                return coefficientSettingRepository;

            }
        }

        private IHrFormulasRepository hrFormulasRepository;
        public IHrFormulasRepository HrFormulasRepository
        {
            get
            {
                hrFormulasRepository ??= new HrFormulasRepository(DatabaseContext);
                return hrFormulasRepository;

            }
        }

        private IPropertyAccountTypeRepository propertyAccountTypeRepository;
        public IPropertyAccountTypeRepository PropertyAccountTypeRepository
        {
            get
            {
                propertyAccountTypeRepository ??= new PropertyAccountTypeRepository(DatabaseContext);
                return propertyAccountTypeRepository;

            }
        }

        private IPaymentTypeRepository paymentTypeRepository;
        public IPaymentTypeRepository PaymentTypeRepository
        {
            get
            {
                paymentTypeRepository ??= new PaymentTypeRepository(DatabaseContext);
                return paymentTypeRepository;

            }
        }
        private IInterdictCategoryRepository interdictCategoryRepository;
        public IInterdictCategoryRepository InterdictCategoryRepository
        {
            get
            {
                interdictCategoryRepository ??= new InterdictCategoryRepository(DatabaseContext);
                return interdictCategoryRepository;

            }
        }
        private IAccountCodeCompatibleTypeRepository accountCodeCompatibleTypeRepository;
        public IAccountCodeCompatibleTypeRepository AccountCodeCompatibleTypeRepository
        {
            get
            {
                accountCodeCompatibleTypeRepository ??= new AccountCodeCompatibleTypeRepository(DatabaseContext);
                return accountCodeCompatibleTypeRepository;
            }
        }

        private IEmployeeHaveTravelRepository employeeHaveTravelRepository;
        public IEmployeeHaveTravelRepository EmployeeHaveTravelRepository
        {
            get
            {
                employeeHaveTravelRepository ??= new EmployeeHaveTravelRepository(DatabaseContext);
                return employeeHaveTravelRepository;

            }
        }
        private IAccountCodeMaritalRepository accountCodeMaritalRepository;
        public IAccountCodeMaritalRepository AccountCodeMaritalRepository
        {
            get
            {
                accountCodeMaritalRepository ??= new AccountCodeMaritalRepository(DatabaseContext);
                return accountCodeMaritalRepository;

            }
        }


        private IPropertyAccountRepository propertyAccountRepository;
        public IPropertyAccountRepository PropertyAccountRepository
        {
            get
            {
                propertyAccountRepository ??= new PropertyAccountRepository(DatabaseContext);
                return propertyAccountRepository;

            }
        }

        private IAccountCodeCategoryRepository accountCodeCategoryRepository;
        public IAccountCodeCategoryRepository AccountCodeCategoryRepository
        {
            get
            {
                accountCodeCategoryRepository ??= new AccountCodeCategoryRepository(DatabaseContext);
                return accountCodeCategoryRepository;

            }
        }

        private IPropertyAccountSettingRepository propertyAccountSettingRepository;
        public IPropertyAccountSettingRepository PropertyAccountSettingRepository
        {
            get
            {
                propertyAccountSettingRepository ??= new PropertyAccountSettingRepository(DatabaseContext);
                return propertyAccountSettingRepository;

            }
        }

        private IPaymentAccountCodeRepository paymentAccountCodeRepository;
        public IPaymentAccountCodeRepository PaymentAccountCodeRepository
        {
            get
            {
                paymentAccountCodeRepository ??= new PaymentAccountCodeRepository(DatabaseContext);
                return paymentAccountCodeRepository;

            }
        }

        private IAccountCodeCompatibleRepository accountCodeCompatibleRepository;
        public IAccountCodeCompatibleRepository AccountCodeCompatibleRepository
        {
            get
            {
                accountCodeCompatibleRepository ??= new AccountCodeCompatibleRepository(DatabaseContext);
                return accountCodeCompatibleRepository;

            }
        }

        private IEmployeeVacationManagementLogRepository employeeVacationManagementLogRepository;
        public IEmployeeVacationManagementLogRepository EmployeeVacationManagementLogRepository
        {
            get
            {
                employeeVacationManagementLogRepository ??= new EmployeeVacationManagementLogRepository(DatabaseContext);
                return employeeVacationManagementLogRepository;

            }
        }

        private IInterdictWeatherRepository interdictWeatherRepository;
        public IInterdictWeatherRepository InterdictWeatherRepository
        {
            get
            {
                interdictWeatherRepository ??= new InterdictWeatherRepository(DatabaseContext);
                return interdictWeatherRepository;

            }
        }

        private IProductionEfficiencyRepository productionEfficiencyRepository;
        public IProductionEfficiencyRepository ProductionEfficiencyRepository
        {
            get
            {
                productionEfficiencyRepository ??= new ProductionEfficiencyRepository(DatabaseContext);
                return productionEfficiencyRepository;
            }
        }

        private ICategoryCoefficientRepository categoryCoefficientRepository;
        public ICategoryCoefficientRepository CategoryCoefficientRepository
        {
            get
            {
                categoryCoefficientRepository ??= new CategoryCoefficientRepository(DatabaseContext);
                return categoryCoefficientRepository;
            }
        }
        private IConditionalAbsenceSubjectRepository conditionalAbsenceSubjectRepository;
        public IConditionalAbsenceSubjectRepository ConditionalAbsenceSubjectRepository
        {
            get
            {
                conditionalAbsenceSubjectRepository ??= new ConditionalAbsenceSubjectRepository(DatabaseContext);
                return conditionalAbsenceSubjectRepository;
            }
        }
        private IEmployeeConditionalAbsenceRepository employeeConditionalAbsenceRepository;
        public IEmployeeConditionalAbsenceRepository EmployeeConditionalAbsenceRepository
        {
            get
            {
                employeeConditionalAbsenceRepository ??= new EmployeeConditionalAbsenceRepository(DatabaseContext);
                return employeeConditionalAbsenceRepository;
            }
        }
        private IConditionalAbsenceTypeRepository conditionalAbsenceTypeRepository;
        public IConditionalAbsenceTypeRepository ConditionalAbsenceTypeRepository
        {
            get
            {
                conditionalAbsenceTypeRepository ??= new ConditionalAbsenceTypeRepository(DatabaseContext);
                return conditionalAbsenceTypeRepository;
            }
        }
        private IConditionalAbsenceSubjectTypeRepository conditionalAbsenceSubjectTypeRepository;
        public IConditionalAbsenceSubjectTypeRepository ConditionalAbsenceSubjectTypeRepository
        {
            get
            {
                conditionalAbsenceSubjectTypeRepository ??= new ConditionalAbsenceSubjectTypeRepository(DatabaseContext);
                return conditionalAbsenceSubjectTypeRepository;
            }
        }
        private IEmployeeDateInformationRepository employeeDateInformationRepository;
        public IEmployeeDateInformationRepository EmployeeDateInformationRepository
        {
            get
            {
                employeeDateInformationRepository ??= new EmployeeDateInformationRepository(DatabaseContext);
                return employeeDateInformationRepository;
            }
        }

        private IMeritRatingRepository meritRatingRepository;
        public IMeritRatingRepository MeritRatingRepository
        {
            get
            {
                meritRatingRepository ??= new MeritRatingRepository(DatabaseContext);
                return meritRatingRepository;
            }
        }

        private IEmployeePercentMeritHistoryRepository employeePercentMeritHistoryRepository;
        public IEmployeePercentMeritHistoryRepository EmployeePercentMeritHistoryRepository
        {
            get
            {
                employeePercentMeritHistoryRepository ??= new EmployeePercentMeritHistoryRepository(DatabaseContext);
                return employeePercentMeritHistoryRepository;
            }
        }

        private IHistoryTypeEmployementRepository historyTypeEmployementRepository;
        public IHistoryTypeEmployementRepository HistoryTypeEmployementRepository
        {
            get
            {
                historyTypeEmployementRepository ??= new HistoryTypeEmployementRepository(DatabaseContext);
                return historyTypeEmployementRepository;
            }
        }

        private IEmployeeHistoryTypeEmployementRepository employeeHistoryTypeEmployementRepository;
        public IEmployeeHistoryTypeEmployementRepository EmployeeHistoryTypeEmployementRepository
        {
            get
            {
                employeeHistoryTypeEmployementRepository ??= new EmployeeHistoryTypeEmployementRepository(DatabaseContext);
                return employeeHistoryTypeEmployementRepository;
            }
        }


        private IDisConnectionTypeRepository disConnectionTypeRepository;
        public IDisConnectionTypeRepository DisConnectionTypeRepository
        {
            get
            {
                disConnectionTypeRepository ??= new DisConnectionTypeRepository(DatabaseContext);
                return disConnectionTypeRepository;
            }
        }

        private IEmployeeDisConnectionRepository employeeDisConnectionRepository;
        public IEmployeeDisConnectionRepository EmployeeDisConnectionRepository
        {
            get
            {
                employeeDisConnectionRepository ??= new EmployeeDisConnectionRepository(DatabaseContext);
                return employeeDisConnectionRepository;
            }
        }
        private IEmployeePromotionRepository employeePromotionRepository;
        public IEmployeePromotionRepository EmployeePromotionRepository
        {
            get
            {
                employeePromotionRepository ??= new EmployeePromotionRepository(DatabaseContext);
                return employeePromotionRepository;
            }
        }

        private IPromotionRejectReasonRepository promotionRejectReasonRepository;
        public IPromotionRejectReasonRepository PromotionRejectReasonRepository
        {
            get
            {
                promotionRejectReasonRepository ??= new PromotionRejectReasonRepository(DatabaseContext);
                return promotionRejectReasonRepository;
            }
        }
        private IPromotionStatusRepository promotionStatusRepository;
        public IPromotionStatusRepository PromotionStatusRepository
        {
            get
            {
                promotionStatusRepository ??= new PromotionStatusRepository(DatabaseContext);
                return promotionStatusRepository;
            }
        }
        private IEducationCategoryRepository educationCategoryRepository;
        public IEducationCategoryRepository EducationCategoryRepository
        {
            get
            {
                educationCategoryRepository ??= new EducationCategoryRepository(DatabaseContext);
                return educationCategoryRepository;
            }
        }

        private IPaymentDetailRepository paymentDetailRepository;
        public IPaymentDetailRepository PaymentDetailRepository
        {
            get
            {
                paymentDetailRepository ??= new PaymentDetailRepository(DatabaseContext);
                return paymentDetailRepository;
            }
        }

        private IPaymentHeaderRepository paymentHeaderRepository;
        public IPaymentHeaderRepository PaymentHeaderRepository
        {
            get
            {
                paymentHeaderRepository ??= new PaymentHeaderRepository(DatabaseContext);
                return paymentHeaderRepository;
            }
        }

        private IPaymentProcessRepository paymentProcessRepository;
        public IPaymentProcessRepository PaymentProcessRepository
        {
            get
            {
                paymentProcessRepository ??= new PaymentProcessRepository(DatabaseContext);
                return paymentProcessRepository;
            }
        }
        private IPaymentProcessCategoryRepository paymentProcessCategoryRepository;
        public IPaymentProcessCategoryRepository PaymentProcessCategoryRepository
        {
            get
            {
                paymentProcessCategoryRepository ??= new PaymentProcessCategoryRepository(DatabaseContext);
                return paymentProcessCategoryRepository;
            }
        }
        private IEmployeePromotionInterdictsRepository employeePromotionInterdictsRepository;
        public IEmployeePromotionInterdictsRepository EmployeePromotionInterdictsRepository
        {
            get
            {
                employeePromotionInterdictsRepository ??= new EmployeePromotionInterdictsRepository(DatabaseContext);
                return employeePromotionInterdictsRepository;
            }
        }


        private IEmployeePromotionSuggestionRepository employeePromotionSuggestionRepository;
        public IEmployeePromotionSuggestionRepository EmployeePromotionSuggestionRepository
        {
            get
            {
                employeePromotionSuggestionRepository ??= new EmployeePromotionSuggestionRepository(DatabaseContext);
                return employeePromotionSuggestionRepository;
            }
        }

        private IView_MIS_Employee_HomeRepository view_MIS_Employee_HomeRepository;
        public IView_MIS_Employee_HomeRepository View_MIS_Employee_HomeRepository
        {
            get
            {
                view_MIS_Employee_HomeRepository ??= new View_MIS_Employee_HomeRepository(DatabaseContext);
                return view_MIS_Employee_HomeRepository;
            }
        }
        private IViewFamilyForOtherPaymentMariedAndBirthDayRepository viewFamilyForOtherPaymentMariedAndBirthDayRepository;
        public IViewFamilyForOtherPaymentMariedAndBirthDayRepository ViewFamilyForOtherPaymentMariedAndBirthDayRepository
        {
            get
            {
                viewFamilyForOtherPaymentMariedAndBirthDayRepository ??= new ViewFamilyForOtherPaymentMariedAndBirthDayPepository(DatabaseContext);
                return viewFamilyForOtherPaymentMariedAndBirthDayRepository;
            }
        }
        private IViewEmployeeForOtherPaymentMariedAndBirthDayRepository viewEmployeeForOtherPaymentMariedAndBirthDayRepository;
        public IViewEmployeeForOtherPaymentMariedAndBirthDayRepository ViewEmployeeForOtherPaymentMariedAndBirthDayRepository
        {
            get
            {
                viewEmployeeForOtherPaymentMariedAndBirthDayRepository ??= new ViewEmployeeForOtherPaymentMariedAndBirthDayRepository(DatabaseContext);
                return viewEmployeeForOtherPaymentMariedAndBirthDayRepository;
            }
        }
        //reward
        private IRewardInRepository rewardInRepository;
        public IRewardInRepository RewardInRepository
        {
            get
            {
                rewardInRepository ??= new RewardInRepository(DatabaseContext);
                return rewardInRepository;
            }
        }

        private IRewardInSmcDailyProductionRepository rewardInSmcDailyProductionRepository;
        public IRewardInSmcDailyProductionRepository RewardInSmcDailyProductionRepository
        {
            get
            {
                rewardInSmcDailyProductionRepository ??= new RewardInSmcDailyProductionRepository(DatabaseContext);
                return rewardInSmcDailyProductionRepository;
            }
        }


        private IRewardInDailyProductionSaleRepository rewardInDailyProductionSaleRepository;
        public IRewardInDailyProductionSaleRepository RewardInDailyProductionSaleRepository
        {
            get
            {
                rewardInDailyProductionSaleRepository ??= new RewardInDailyProductionSaleRepository(DatabaseContext);
                return rewardInDailyProductionSaleRepository;
            }
        }
        private IRewardInMonthlyProductionSaleRepository rewardInMonthlyProductionSaleRepository;
        public IRewardInMonthlyProductionSaleRepository RewardInMonthlyProductionSaleRepository
        {
            get
            {
                rewardInMonthlyProductionSaleRepository ??= new RewardInMonthlyProductionSaleRepository(DatabaseContext);
                return rewardInMonthlyProductionSaleRepository;
            }
        }


        private IRewardBaseHeaderRepository rewardBaseHeaderRepository;
        public IRewardBaseHeaderRepository RewardBaseHeaderRepository
        {
            get
            {
                rewardBaseHeaderRepository ??= new RewardBaseHeaderRepository(DatabaseContext);
                return rewardBaseHeaderRepository;
            }
        }


        private IRewardBaseDetailRepository rewardBaseDetailRepository;
        public IRewardBaseDetailRepository RewardBaseDetailRepository
        {
            get
            {
                rewardBaseDetailRepository ??= new RewardBaseDetailRepository(DatabaseContext);
                return rewardBaseDetailRepository;
            }
        }
        private IRewardOutHeaderRepository rewardOutHeaderRepository;
        public IRewardOutHeaderRepository RewardOutHeaderRepository
        {
            get
            {
                rewardOutHeaderRepository ??= new RewardOutHeaderRepository(DatabaseContext);
                return rewardOutHeaderRepository;
            }
        }
        private IRewardOutDetailRepository rewardOutDetailRepository;
        public IRewardOutDetailRepository RewardOutDetailRepository
        {
            get
            {
                rewardOutDetailRepository ??= new RewardOutDetailRepository(DatabaseContext);
                return rewardOutDetailRepository;
            }
        }

        private IRewardInQualityControlMonthlyPeletRepository rewardInQualityControlMonthlyPeletRepository;
        public IRewardInQualityControlMonthlyPeletRepository RewardInQualityControlMonthlyPeletRepository
        {
            get
            {
                rewardInQualityControlMonthlyPeletRepository ??= new RewardInQualityControlMonthlyPeletRepository(DatabaseContext);
                return rewardInQualityControlMonthlyPeletRepository;
            }
        }

        private IRewardInQualityControlMonthlyDriRepository rewardInQualityControlMonthlyDriRepository;
        public IRewardInQualityControlMonthlyDriRepository RewardInQualityControlMonthlyDriRepository
        {
            get
            {
                rewardInQualityControlMonthlyDriRepository ??= new RewardInQualityControlMonthlyDriRepository(DatabaseContext);
                return rewardInQualityControlMonthlyDriRepository;
            }
        }


        private IRewardInQualityControlMonthlyProductionRepository rewardInQualityControlMonthlyProductionRepository;
        public IRewardInQualityControlMonthlyProductionRepository RewardInQualityControlMonthlyProductionRepository
        {
            get
            {
                rewardInQualityControlMonthlyProductionRepository ??= new RewardInQualityControlMonthlyProductionRepository(DatabaseContext);
                return rewardInQualityControlMonthlyProductionRepository;
            }
        }


        private IRewardInSmcMonthlyProductionRepository rewardInSmcMonthlyProductionRepository;
        public IRewardInSmcMonthlyProductionRepository RewardInSmcMonthlyProductionRepository
        {
            get
            {
                rewardInSmcMonthlyProductionRepository ??= new RewardInSmcMonthlyProductionRepository(DatabaseContext);
                return rewardInSmcMonthlyProductionRepository;
            }
        }

        private IRewardUnitTypeRepository rewardUnitTypeRepository;
        public IRewardUnitTypeRepository RewardUnitTypeRepository
        {
            get
            {
                rewardUnitTypeRepository ??= new RewardUnitTypeRepository(DatabaseContext);
                return rewardUnitTypeRepository;
            }
        }

        private IEmployeeSafetyDeductionRepository employeeSafetyDeductionRepository;
        public IEmployeeSafetyDeductionRepository EmployeeSafetyDeductionRepository
        {
            get
            {
                employeeSafetyDeductionRepository ??= new EmployeeSafetyDeductionRepository(DatabaseContext);
                return employeeSafetyDeductionRepository;
            }
        }

        private IRewardBaseSpecificRepository rewardBaseSpecificRepository;
        public IRewardBaseSpecificRepository RewardBaseSpecificRepository
        {
            get
            {
                rewardBaseSpecificRepository ??= new RewardBaseSpecificRepository(DatabaseContext);
                return rewardBaseSpecificRepository;
            }
        }

        private IRewardBaseSpecificOfUnitDetailRepository rewardBaseSpecificOfUnitDetailRepository;
        public IRewardBaseSpecificOfUnitDetailRepository RewardBaseSpecificOfUnitDetailRepository
        {
            get
            {
                rewardBaseSpecificOfUnitDetailRepository ??= new RewardBaseSpecificOfUnitDetailRepository(DatabaseContext);
                return rewardBaseSpecificOfUnitDetailRepository;
            }
        }

        private IRewardBaseSpecificOfUnitHeaderRepository rewardBaseSpecificOfUnitHeaderRepository;
        public IRewardBaseSpecificOfUnitHeaderRepository RewardBaseSpecificOfUnitHeaderRepository
        {
            get
            {
                rewardBaseSpecificOfUnitHeaderRepository ??= new RewardBaseSpecificOfUnitHeaderRepository(DatabaseContext);
                return rewardBaseSpecificOfUnitHeaderRepository;
            }
        }
        private IEmployeeRewardRepository employeeRewardRepository;
        public IEmployeeRewardRepository EmployeeRewardRepository
        {
            get
            {
                employeeRewardRepository ??= new EmployeeRewardRepository(DatabaseContext);
                return employeeRewardRepository;
            }
        }

        #region StandBy Repositories

        private IStandbyEmloyeeBoardRepository standbyEmloyeeBoardRepository;
        public IStandbyEmloyeeBoardRepository StandbyEmloyeeBoardRepository
        {
            get
            {
                standbyEmloyeeBoardRepository ??= new StandbyEmloyeeBoardRepository(DatabaseContext);
                return standbyEmloyeeBoardRepository;
            }
        }

        private IStandbyEmloyeeBoardLogRepository standbyEmloyeeBoardLogRepository;
        public IStandbyEmloyeeBoardLogRepository StandbyEmloyeeBoardLogRepository
        {
            get
            {
                standbyEmloyeeBoardLogRepository ??= new StandbyEmloyeeBoardLogRepository(DatabaseContext);
                return standbyEmloyeeBoardLogRepository;
            }
        }

        private IStandbyEmployeeRoleRepository standbyEmployeeRoleRepository;
        public IStandbyEmployeeRoleRepository StandbyEmployeeRoleRepository
        {
            get
            {
                standbyEmployeeRoleRepository ??= new StandbyEmployeeRoleRepository(DatabaseContext);
                return standbyEmployeeRoleRepository;
            }
        }

        private IStandbyHeaderRepository standbyHeaderRepository;
        public IStandbyHeaderRepository StandbyHeaderRepository
        {
            get
            {
                standbyHeaderRepository ??= new StandbyHeaderRepository(DatabaseContext);
                return standbyHeaderRepository;
            }
        }

        private IStandbyRoleRepository standbyRoleRepository;
        public IStandbyRoleRepository StandbyRoleRepository
        {
            get
            {
                standbyRoleRepository ??= new StandbyRoleRepository(DatabaseContext);
                return standbyRoleRepository;
            }
        }

        private IStandbyTurnRepository standbyTurnRepository;
        public IStandbyTurnRepository StandbyTurnRepository
        {
            get
            {
                standbyTurnRepository ??= new StandbyTurnRepository(DatabaseContext);
                return standbyTurnRepository;
            }
        }

        #endregion
        private IDeductionAdditionalRepository deductionAdditionalRepository;
        public IDeductionAdditionalRepository DeductionAdditionalRepository
        {
            get
            {
                deductionAdditionalRepository ??= new DeductionAdditionalRepository(DatabaseContext);
                return deductionAdditionalRepository;
            }
        }
        private IEmployeeDeductionAdditionalRepository employeeDeductionAdditionalRepository;
        public IEmployeeDeductionAdditionalRepository EmployeeDeductionAdditionalRepository
        {
            get
            {
                employeeDeductionAdditionalRepository ??= new EmployeeDeductionAdditionalRepository(DatabaseContext);
                return employeeDeductionAdditionalRepository;
            }
        }
        //
        private IPaymentAdditionalDetailRepository paymentAdditionalDetailRepository;
        public IPaymentAdditionalDetailRepository PaymentAdditionalDetailRepository
        {
            get
            {
                paymentAdditionalDetailRepository ??= new PaymentAdditionalDetailRepository(DatabaseContext);
                return paymentAdditionalDetailRepository;
            }
        }

        private IPaymentAdditionalHeaderRepository paymentAdditionalHeaderRepository;
        public IPaymentAdditionalHeaderRepository PaymentAdditionalHeaderRepository
        {
            get
            {
                paymentAdditionalHeaderRepository ??= new PaymentAdditionalHeaderRepository(DatabaseContext);
                return paymentAdditionalHeaderRepository;
            }
        }

        private IPaymentAdditionalSettingRepository paymentAdditionalSettingRepository;
        public IPaymentAdditionalSettingRepository PaymentAdditionalSettingRepository
        {
            get
            {
                paymentAdditionalSettingRepository ??= new PaymentAdditionalSettingRepository(DatabaseContext);
                return paymentAdditionalSettingRepository;
            }
        }

        private IPaymentAdditionalSettingJobCategoryRepository paymentAdditionalSettingJobCategoryRepository;
        public IPaymentAdditionalSettingJobCategoryRepository PaymentAdditionalSettingJobCategoryRepository
        {
            get
            {
                paymentAdditionalSettingJobCategoryRepository ??= new PaymentAdditionalSettingJobCategoryRepository(DatabaseContext);
                return paymentAdditionalSettingJobCategoryRepository;
            }
        }

        private IPaymentAdditionalSettingWorkCityRepository paymentAdditionalSettingWorkCityRepository;
        public IPaymentAdditionalSettingWorkCityRepository PaymentAdditionalSettingWorkCityRepository
        {
            get
            {
                paymentAdditionalSettingWorkCityRepository ??= new PaymentAdditionalSettingWorkCityRepository(DatabaseContext);
                return paymentAdditionalSettingWorkCityRepository;
            }
        }
        private IIncreaseSalaryHeaderRepository increaseSalaryHeaderRepository;
        public IIncreaseSalaryHeaderRepository IncreaseSalaryHeaderRepository
        {
            get
            {
                increaseSalaryHeaderRepository ??= new IncreaseSalaryHeaderRepository(DatabaseContext);
                return increaseSalaryHeaderRepository;
            }
        }
        private IIncreaseSalaryDetailRepository increaseSalaryDetailRepository;
        public IIncreaseSalaryDetailRepository IncreaseSalaryDetailRepository
        {
            get
            {
                increaseSalaryDetailRepository ??= new IncreaseSalaryDetailRepository(DatabaseContext);
                return increaseSalaryDetailRepository;
            }
        }

        private IStandbyReplacementRequestsRepository standbyReplacementRequestsRepository;

        public IStandbyReplacementRequestsRepository StandbyReplacementRequestsRepository
        {
            get
            {
                standbyReplacementRequestsRepository ??= new StandbyReplacementRequestsRepository(DatabaseContext);
                return standbyReplacementRequestsRepository;
            }
        }


        private IPaymentAdditionalSettingJobPositionRepository paymentAdditionalSettingJobPositionRepository;

        public IPaymentAdditionalSettingJobPositionRepository PaymentAdditionalSettingJobPositionRepository
        {
            get
            {
                paymentAdditionalSettingJobPositionRepository ??= new PaymentAdditionalSettingJobPositionRepository(DatabaseContext);
                return paymentAdditionalSettingJobPositionRepository;
            }
        }




        public IConfirmInterdictMessageRepository confirmInterdictMessageRepository;
        public IConfirmInterdictMessageRepository ConfirmInterdictMessageRepository
        {
            get
            {
                confirmInterdictMessageRepository ??= new ConfirmInterdictMessageRepository(DatabaseContext);
                return confirmInterdictMessageRepository;
            }
        }

        public IConfirmInterdictRepository confirmInterdictRepository;
        public IConfirmInterdictRepository ConfirmInterdictRepository
        {
            get
            {
                confirmInterdictRepository ??= new ConfirmInterdictRepository(DatabaseContext);
                return confirmInterdictRepository;
            }
        }

        public IConfirmInterdictStatusRepository confirmInterdictStatusRepository;
        public IConfirmInterdictStatusRepository ConfirmInterdictStatusRepository
        {
            get
            {
                confirmInterdictStatusRepository ??= new ConfirmInterdictStatusRepository(DatabaseContext);
                return confirmInterdictStatusRepository;
            }
        }
        //security   
        public IUserDefinitionRepository userDefinitionRepository;
        public IUserDefinitionRepository UserDefinitionRepository
        {
            get
            {
                userDefinitionRepository ??= new UserDefinitionRepository(DatabaseContext);
                return userDefinitionRepository;
            }
        }

        public IUserDefinitionSecurityPriorityStatusRepository userDefinitionSecurityPriorityStatusRepository;
        public IUserDefinitionSecurityPriorityStatusRepository UserDefinitionSecurityPriorityStatusRepository
        {
            get
            {
                userDefinitionSecurityPriorityStatusRepository ??= new UserDefinitionSecurityPriorityStatusRepository(DatabaseContext);
                return userDefinitionSecurityPriorityStatusRepository;
            }
        }

        public IUserDefinitionSecurityTeamWorkRepository userDefinitionSecurityTeamWorkRepository;
        public IUserDefinitionSecurityTeamWorkRepository UserDefinitionSecurityTeamWorkRepository
        {
            get
            {
                userDefinitionSecurityTeamWorkRepository ??= new UserDefinitionSecurityTeamWorkRepository(DatabaseContext);
                return userDefinitionSecurityTeamWorkRepository;
            }
        }


        public IJobPositionTeamWorkRepository jobPositionTeamWorkRepository;
        public IJobPositionTeamWorkRepository JobPositionTeamWorkRepository
        {

            get
            {
                jobPositionTeamWorkRepository ??= new JobPositionTeamWorkRepository(DatabaseContext);
                return jobPositionTeamWorkRepository;
            }
        }

        //public IJobPositionNatureSubGroupRepository JobPositionNatureSubGroupRepository => throw new NotImplementedException();
        public IJobPositionNatureSubGroupRepository jobPositionNatureSubGroupRepository;
        public IJobPositionNatureSubGroupRepository JobPositionNatureSubGroupRepository
        {
            get
            {
                jobPositionNatureSubGroupRepository ??= new JobPositionNatureSubGroupRepository(DatabaseContext);
                return jobPositionNatureSubGroupRepository;
            }
        }

        public IRollCallWorkTimeMonthSettingRepository rollCallWorkTimeMonthSettingRepository;
        public IRollCallWorkTimeMonthSettingRepository RollCallWorkTimeMonthSettingRepository
        {
            get
            {
                rollCallWorkTimeMonthSettingRepository ??= new RollCallWorkTimeMonthSettingRepository(DatabaseContext);
                return rollCallWorkTimeMonthSettingRepository;
            }
        }
        public IInvalidDayTypeInForcedOvertimeRepository invalidDayTypeInForcedOvertimeRepository;
        public IInvalidDayTypeInForcedOvertimeRepository InvalidDayTypeInForcedOvertimeRepository
        {
            get
            {
                invalidDayTypeInForcedOvertimeRepository ??= new InvalidDayTypeInForcedOvertimeRepository(DatabaseContext);
                return invalidDayTypeInForcedOvertimeRepository;
            }
        }

        public IRollCallWorkCityRepository rollCallWorkCityRepository;
        public IRollCallWorkCityRepository RollCallWorkCityRepository
        {
            get
            {
                rollCallWorkCityRepository ??= new RollCallWorkCityRepository(DatabaseContext);
                return rollCallWorkCityRepository;
            }
        }

        public IEmployeeDeductionTypeRepository employeeDeductionTypeRepository;
        public IEmployeeDeductionTypeRepository EmployeeDeductionTypeRepository
        {
            get
            {
                employeeDeductionTypeRepository ??= new EmployeeDeductionTypeRepository(DatabaseContext);
                return employeeDeductionTypeRepository;
            }
        }

        public IEmployeeDeductionTypeAccessRepository employeeDeductionTypeAccessRepository;
        public IEmployeeDeductionTypeAccessRepository EmployeeDeductionTypeAccessRepository
        {
            get
            {
                employeeDeductionTypeAccessRepository ??= new EmployeeDeductionTypeAccessRepository(DatabaseContext);
                return employeeDeductionTypeAccessRepository;
            }
        }


        public IView_PresentEmployeesRepository view_PresentEmployeesRepository;
        public IView_PresentEmployeesRepository View_PresentEmployeesRepository
        {
            get
            {
                view_PresentEmployeesRepository ??= new View_PresentEmployeesRepository(DatabaseContext);
                return view_PresentEmployeesRepository;
            }
        }

        public IEmployeeLoanDeductionHeaderRepository employeeLoanDeductionHeaderRepository;
        public IEmployeeLoanDeductionHeaderRepository EmployeeLoanDeductionHeaderRepository
        {
            get
            {
                employeeLoanDeductionHeaderRepository ??= new EmployeeLoanDeductionHeaderRepository(DatabaseContext);
                return employeeLoanDeductionHeaderRepository;
            }
        }
        public IEmployeeLoanDeductionDetailRepository employeeLoanDeductionDetailRepository;
        public IEmployeeLoanDeductionDetailRepository EmployeeLoanDeductionDetailRepository
        {
            get
            {
                employeeLoanDeductionDetailRepository ??= new EmployeeLoanDeductionDetailRepository(DatabaseContext);
                return employeeLoanDeductionDetailRepository;
            }
        }

        public IEmployeeDeductionDetailRepository employeeDeductionDetailRepository;
        public IEmployeeDeductionDetailRepository EmployeeDeductionDetailRepository
        {
            get
            {
                employeeDeductionDetailRepository ??= new EmployeeDeductionDetailRepository(DatabaseContext);
                return employeeDeductionDetailRepository;
            }
        }

        public IEmployeeDeductionTempRepository employeeDeductionTempRepository;
        public IEmployeeDeductionTempRepository EmployeeDeductionTempRepository
        {
            get
            {
                employeeDeductionTempRepository ??= new EmployeeDeductionTempRepository(DatabaseContext);
                return employeeDeductionTempRepository;
            }
        }

        private IAccountCodeDeductionTypeRepository accountCodeDeductionTypeRepository;
        public IAccountCodeDeductionTypeRepository AccountCodeDeductionTypeRepository
        {
            get
            {
                accountCodeDeductionTypeRepository ??= new AccountCodeDeductionTypeRepository(DatabaseContext);
                return accountCodeDeductionTypeRepository;

            }
        }

        private IViewMisPayrollRepository viewMisPayrollRepository;
        public IViewMisPayrollRepository ViewMisPayrollRepository
        {
            get
            {
                viewMisPayrollRepository ??= new ViewMisPayrollRepository(DatabaseContext);
                return viewMisPayrollRepository;
            }
        }

        private IEmployeeDeductionActivateStatusRepository employeeDeductionActivateStatusRepository;
        public IEmployeeDeductionActivateStatusRepository EmployeeDeductionActivateStatusRepository
        {
            get
            {
                employeeDeductionActivateStatusRepository ??= new EmployeeDeductionActivateStatusRepository(DatabaseContext);
                return employeeDeductionActivateStatusRepository;
            }
        }

        private IBudgetRewardEmployeeRepository budgetRewardEmployeeRepository;
        public IBudgetRewardEmployeeRepository BudgetRewardEmployeeRepository
        {
            get
            {
                budgetRewardEmployeeRepository ??= new BudgetRewardEmployeeRepository(DatabaseContext);
                return budgetRewardEmployeeRepository;
            }
        }

        private IBudgetRewardHeaderRepository budgetRewardHeaderRepository;
        public IBudgetRewardHeaderRepository BudgetRewardHeaderRepository
        {
            get
            {
                budgetRewardHeaderRepository ??= new BudgetRewardHeaderRepository(DatabaseContext);
                return budgetRewardHeaderRepository;
            }
        }

        private IBudgetRewardDetailRepository budgetRewardDetailRepository;
        public IBudgetRewardDetailRepository BudgetRewardDetailRepository
        {
            get
            {
                budgetRewardDetailRepository ??= new BudgetRewardDetailRepository(DatabaseContext);
                return budgetRewardDetailRepository;
            }
        }

        private IBudgetRewardTypeRepository budgetRewardTypeRepository;
        public IBudgetRewardTypeRepository BudgetRewardTypeRepository
        {
            get
            {
                budgetRewardTypeRepository ??= new BudgetRewardTypeRepository(DatabaseContext);
                return budgetRewardTypeRepository;
            }
        }

        private IBudgetRewardStatusRepository budgetRewardStatusRepository;
        public IBudgetRewardStatusRepository BudgetRewardStatusRepository
        {
            get
            {
                budgetRewardStatusRepository ??= new BudgetRewardStatusRepository(DatabaseContext);
                return budgetRewardStatusRepository;
            }
        }

        private IBudgetRewardEmployeeHistoryRepository budgetRewardEmployeeHistoryRepository;
        public IBudgetRewardEmployeeHistoryRepository BudgetRewardEmployeeHistoryRepository
        {
            get
            {
                budgetRewardEmployeeHistoryRepository ??= new BudgetRewardEmployeeHistoryRepository(DatabaseContext);
                return budgetRewardEmployeeHistoryRepository;
            }
        }

    }
}
