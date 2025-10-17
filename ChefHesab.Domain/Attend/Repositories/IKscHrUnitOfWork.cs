using Ksc.HR.Domain.Repositories.WorkShift;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Ksc.HR.Domain.Repositories.ODSViews;
using Ksc.HR.Domain.Repositories.OnCall;
using Ksc.HR.Domain.Repositories.Personal;
using Ksc.HR.Domain.Repositories.HRSystemStatusControl;
using Ksc.HR.Domain.Repositories.Dismissal;
using Ksc.HR.Domain.Repositories.ScheduledLoger;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Domain.Repositories.BusinessTrip;
using Ksc.HR.Domain.Repositories.Chart;
using Ksc.HR.Domain.Repositories.Stepper;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Domain.Repositories.Log;
using Ksc.HR.Domain.Repositories.Salary;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Repositories.View;
using System.Collections.Generic;
using Ksc.HR.Domain.Repositories.Rule;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Reward;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.StandBy;
using Ksc.HR.Domain.Repositories.Security;
using Ksc.HR.Domain.Repositories.Transfer;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Domain.Repositories
{
    public interface IKscHrUnitOfWork : IBaseUnitOfwork
    {
        IV_JobPositionRepository View_JobPositionRepository { get; }
        IEmployeeDeductionInstallmentPutOffRepository EmployeeDeductionInstallmentPutOffRepository { get; }
        IViewOdsJobPositionTreeRepository ViewOdsJobPositionTreeRepository { get; }

        ICofficientProximityProductRepository CofficientProximityProductRepository { get; }
        IViewOdsJobStatusCategoryRepository ViewOdsJobStatusCategoryRepository { get; }
        IEmployeeEducationTimeRepository EmployeeEducationTimeRepository { get; }
        IWorkTimeCategoryRepository WorkTimeCategoryRepository { get; }
        IEntryExitTypeRepository EntryExitTypeRepository { get; }
        IShiftConceptRepository ShiftConceptRepository { get; }
        IShiftConceptDetailRepository ShiftConceptDetailRepository { get; }
        IWorkTimeRepository WorkTimeRepository { get; }
        IWorkDayTypeRepository WorkDayTypeRepository { get; }
        ICityRepository CityRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        ICountryRepository CountryRepository { get; }
        IDayNightSettingTimeRepository DayNightSettingTimeRepository { get; }
        IShiftBoardRepository ShiftBoardRepository { get; }
        IShiftHolidayRepository ShiftHolidayRepository { get; }
        IWorkCalendarRepository WorkCalendarRepository { get; }
        IViewNotConfirmedEducationRepository ViewNotConfirmedEducationRepository { get; }
        IWorkGroupRepository WorkGroupRepository { get; }
        IWorkCompanySettingRepository WorkCompanySettingRepository { get; }
        ITimeShiftSettingRepository TimeShiftSettingRepository { get; }
        IAccessLevelRepository AccessLevelRepository { get; }
        IEmployeePaymentStatusRepository EmployeePaymentStatusRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ICompatibleRollCallRepository CompatibleRollCallRepository { get; }
        IIncludedDefinitionRepository IncludedDefinitionRepository { get; }
        IIncludedRollCallRepository IncludedRollCallRepository { get; }
        IOverTimeDefinitionRepository OverTimeDefinitionRepository { get; }
        IRollCallCategoryRepository RollCallCategoryRepository { get; }
        IRollCallConceptRepository RollCallConceptRepository { get; }
        IRollCallDefinitionRepository RollCallDefinitionRepository { get; }
        IRollCallJobCategoryRepository RollCallJobCategoryRepository { get; }
        IRollCallSalaryCodeRepository RollCallSalaryCodeRepository { get; }
        IRollCallWorkTimeDayTypeRepository RollCallWorkTimeDayTypeRepository { get; }
        ITeamWorkRepository TeamWorkRepository { get; }
        ITeamWorkCategoryRepository TeamWorkCategoryRepository { get; }
        ITeamWorkCategoryTypeRepository TeamWorkCategoryTypeRepository { get; }
        ITeamWorkMangementCodeRepository TeamWorkMangementCodeRepository { get; }
        IWorkCityRepository WorkCityRepository { get; }
        IViewMisCostCenterRepository ViewMisCostCenterRepository { get; }
        IViewMisJobCategoryRepository ViewMisJobCategoryRepository { get; }
        IViewMisSalaryCodeRepository ViewMisSalaryCodeRepository { get; }
        IViewMisEmploymentTypeRepository ViewMisEmploymentTypeRepository { get; }
        IViewMisEmployeeRepository ViewMisEmployeeRepository { get; }
        IOnCall_RequestRepository OnCall_RequestRepository { get; }
        IOnCall_TypeRepository OnCall_TypeRepository { get; }
        IOnCall_WorkTimeRepository OnCall_WorkTimeRepository { get; }
        IOnCall_AccessManagmentOnCallTypeRepository OnCall_AccessManagmentOnCallTypeRepository { get; }

        IWF_AccessManagementRepository WF_AccessManagementRepository { get; }
        IWF_JobCategoryRangeRepository WF_JobCategoryRangeRepository { get; }
        IWF_PriorityRepository WF_PriorityRepository { get; }
        IWF_ProcessRepository WF_ProcessRepository { get; }
        IWF_RequestHistoryRepository WF_RequestHistoryRepository { get; }
        IWF_RequestRepository WF_RequestRepository { get; }
        IWF_RoleRepository WF_RoleRepository { get; }
        IWF_StatusProcessManagementRepository WF_StatusProcessManagementRepository { get; }
        IWF_StatusReasonRepository WF_StatusReasonRepository { get; }
        IWF_StatusRepository WF_StatusRepository { get; }
        IWF_ValidJobCategoryRepository WF_ValidJobCategoryRepository { get; }
        IWF_WorkFlowManagementRepository WF_WorkFlowManagementRepository { get; }
        IWF_WorkFlowStatusReasonRepository WF_WorkFlowStatusReasonRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IEmployeeBreastfeddingRepository EmployeeBreastfeddingRepository { get; }
        IEmployeeLongTermAbsenceRepository EmployeeLongTermAbsenceRepository { get; }
        IViewMisSubFunctionRepository ViewMisSubFunctionRepository { get; }
        IEmployeeEntryExitRepository EmployeeEntryExitRepository { get; }
        IEmployeeAttendAbsenceItemRepository EmployeeAttendAbsenceItemRepository { get; }
        IEmployeeWorkGroupRepository EmployeeWorkGroupRepository { get; }
        IViewMisUserDefinitionRepository ViewMisUserDefinitionRepository { get; }
        IViewMisJobPositionRepository ViewMisJobPositionRepository { get; }
        ITransfer_RequestReasonRepository Transfer_RequestReasonRepository { get; }
        ITransfer_RequestReasonTypeRepository Transfer_RequestReasonTypeRepository { get; }
        ITransfer_RequestRepository Transfer_RequestRepository { get; }
        ITransfer_RequestTypeRepository Transfer_RequestTypeRepository { get; }
        ITransfer_TypeRepository Transfer_TypeRepository { get; }
        ITransfer_RequestReasonJobCategoryRepository Transfer_RequestReasonJobCategoryRepository { get; }
        IEmployeeTeamWorkRepository EmployeeTeamWorkRepository { get; }
        IViewMisEmployeeSecurityRepository ViewMisEmployeeSecurityRepository { get; }
        ISystemControlDateRepository SystemControlDateRepository { get; }
        ISystemSequenceControlRepository SystemSequenceControlRepository { get; }
        ISystemSequenceStatusRepository SystemSequenceStatusRepository { get; }
        IRollCallEmploymentTypeRepository RollCallEmploymentTypeRepository { get; }
        IUnofficialHolidayReasonRepository UnofficialHolidayReasonRepository { get; }
        IWorkCompanyUnOfficialHolidayJobCategoryRepository WorkCompanyUnOfficialHolidayJobCategoryRepository { get; }
        IWorkCompanyUnOfficialHolidaySettingRepository WorkCompanyUnOfficialHolidaySettingRepository { get; }
        IWorkTimeShiftConceptRepository WorkTimeShiftConceptRepository { get; }


        ICalendarEventRepository CalendarEventRepository { get; }
        ITimeSheetSettingRepository TimeSheetSettingRepository { get; }

        IDismissal_RequestRepository Dismissal_RequestRepository { get; }

        IDismissal_StatusRepository Dismissal_StatusRepository { get; }
        IEmployeeEntryExitAttendAbsenceRepository EmployeeEntryExitAttendAbsenceRepository { get; }
        ISacrificeOptionSettingRepository SacrificeOptionSettingRepository { get; }
        ISacrificePercentageSettingRepository SacrificePercentageSettingRepository { get; }
        ITrainingTypeRepository TrainingTypeRepository { get; }
        IPaymentStatusRepository PaymentStatusRepository { get; }
        IAnalysisEmployeeAttendAbsenceItemRepository AnalysisEmployeeAttendAbsenceItemRepository { get; }
        IScheduledLogRepository ScheduledLogRepository { get; }
        IDayNightPercentEmplymentTypeRepository DayNightPercentEmplymentTypeRepository { get; }
        IDayNightRollCallRepository DayNightRollCallRepository { get; }
        IMonthTimeSheetRepository MonthTimeSheetRepository { get; }
        IMonthTimeSheetDraftRepository MonthTimeSheetDraftRepository { get; }
        IMonthTimeSheetIncludedRepository MonthTimeSheetIncludedRepository { get; }
        IMonthTimeSheetRollCallRepository MonthTimeSheetRollCallRepository { get; }
        IMonthTimeSheetWorkTimeRepository MonthTimeSheetWorkTimeRepository { get; }
        IMonthTimeSheetLogRepository MonthTimeSheetLogRepository { get; }

        IViewTimeSheetToMisRepository ViewTimeSheetToMisRepository { get; }
        IOverTimeSpecialHolidayTimeSheetSettingRepository OverTimeSpecialHolidayTimeSheetSettingRepository { get; }
        IEmployeeTimeSheetRepository EmployeeTimeSheetRepository { get; }
        IViewTeamManagerRepository ViewTeamManagerRepository { get; }
        IMonthTimeShitStepperRepository MonthTimeShitStepperRepository { get; }
        IViewAttendItemReportRepository ViewAttendItemReportRepository { get; }
        IViewEmployeeTeamUserActiveRepository ViewEmployeeTeamUserActiveRepository { get; }
        IMission_LocationRepository Mission_LocationRepository { get; }
        IMission_RequestRepository Mission_RequestRepository { get; }
        IMission_TypeRepository Mission_TypeRepository { get; }
        IMission_TypeAccessLevelRepository Mission_TypeAccessLevelRepository { get; }
        IMission_GoalRepository Mission_GoalRepository { get; }
        IDayTimeSettingRepository DayTimeSettingRepository { get; }
        IFloatTimeSettingRepository FloatTimeSettingRepository { get; }
        IEmployeeEfficiencyHistoryRepository EmployeeEfficiencyHistoryRepository { get; }
        IEmployeeEfficiencyMonthRepository EmployeeEfficiencyMonthRepository { get; }
        IWF_WorkFlowActionRepository WF_WorkFlowActionRepository { get; }

        IStudyFieldRepository StudyFieldRepository { get; }
        IEducationRepository EducationRepository { get; }
        IChart_JobCategoryDefinationRepository Chart_JobCategoryDefinationRepository { get; }
        IChart_JobPositionRepository Chart_JobPositionRepository { get; }
        IChart_JobIdentityRepository Chart_JobIdentityRepository { get; }
        IChart_JobPositionStatusRepository Chart_JobPositionStatusRepository { get; }
        IChart_JobPositionStatusCategoryRepository Chart_JobPositionStatusCategoryRepository { get; }


        IJobPositionNatureRepository Chart_JobPositionNatureRepository { get; }
        IJobCategoryRepository JobCategoryRepository { get; }
        IJobCategoryEducationRepository JobCategoryEducationRepository { get; }
        IJobPositionIncreasePercentRepository JobPositionIncreasePercentRepository { get; }
        IChart_JobPositionScoreTypeRepository Chart_JobPositionScoreTypeRepository { get; }
        IChart_JobCertificateRepository Chart_JobCertificateRepository { get; }
        IChart_JobPositionCertificateRepository Chart_JobPositionCertificateRepository { get; }
        IChart_JobPositionFieldRepository Chart_JobPositionFieldRepository { get; }
        IChart_StructureRepository Chart_StructureRepository { get; }

        IJobPositionScoreRepository JobPositionScoreRepository { get; }
        IMission_TypeLocationRepository Mission_TypeLocationRepository { get; }
        IRewardSpecificRepository RewardSpecificRepository { get; }
        IJobPositionHistoryRepository JobPositionHistoryRepository { get; }

        IViewJobPositionRepository ViewJobPositionRepository { get; }
        IStructureTypeRepository StructureTypeRepository { get; }
        IStepper_ProcedureRepository Stepper_ProcedureRepository { get; }
        IStepper_ProcedureStatusRepository Stepper_ProcedureStatusRepository { get; }
        IStepper_ProcedureLogRepository Stepper_ProcedureLogRepository { get; }
        IStepper_ProcedureAccessLevelRepository Stepper_ProcedureAccessLevelRepository { get; }
        IStepper_StatusSystemMonthRepository Stepper_StatusSystemMonthRepository { get; }

        IInsuranceListRepository InsuranceListRepository { get; }
        IInsuranceTypeRepository InsuranceTypeRepository { get; }
        IBloodTypeRepository BloodTypeRepository { get; }
        IIsarStatusRepository IsarStatusRepository { get; }
        IMaritalStatusRepository MaritalStatusRepository { get; }
        IRegionRepository RegionRepository { get; }
        INationalityRepository NationalityRepository { get; }
        IGenderRepository GenderRepository { get; }
        IEmploymentTypeRepository EmploymentTypeRepository { get; }
        ISavingTypeRepository SavingTypeRepository { get; }
        IEmploymentStatusRepository EmploymentStatusRepository { get; }
        IMilitaryDegreeRepository MilitaryDegreeRepository { get; }
        IMilitaryTypeRepository MilitaryTypeRepository { get; }
        IMilitaryUnitRepository MilitaryUnitRepository { get; }
        IMilitaryStatusRepository MilitaryStatusRepository { get; }
        IMilitaryExemptionReasonRepository MilitaryExemptionReasonRepository { get; }
        IAccountBankTypeRepository AccountBankTypeRepository { get; }
        IPersonalTypeRepository PersonalTypeRepository { get; }
        IOtherPaymentSettingRepository OtherPaymentSettingRepository { get; }

        IDependenceJobRepository DependenceJobRepository { get; }
        IDependenceReasonRepository DependenceReasonRepository { get; }
        IDependenceTypeRepository DependenceTypeRepository { get; }
        IDependentExitDateReasonRepository DependentExitDateReasonRepository { get; }

        IFamilyRepository FamilyRepository { get; }
        IEmployeeEducationDegreeRepository EmployeeEducationDegreeRepository { get; }

        IEmployeeJobPositionRepository EmployeeJobPositionRepository { get; }
        IEmployeeAccountBankRepository EmployeeAccountBankRepository { get; }

        ITableDefinitionRepository TableDefinitionRepository { get; }
        IEmployeePictureRepository EmployeePictureRepository { get; }
        IAccountCodeRepository AccountCodeRepository { get; }


        IInsuranceBookletRepository InsuranceBookletRepository { get; }
        IFranchiseTypeRepository FranchiseTypeRepository { get; }
        IOperationTypeRepository OperationTypeRepository { get; }
        IEntityTypeRepository EntityTypeRepository { get; }
        ILogDataRepository LogDataRepository { get; }

        IBookletEmployeeWithFamilyAndBankRepository BookletEmployeeWithFamilyAndBankRepository { get; }
        IPaymentStatusAccessRepository PaymentStatusAccessRepository { get; }
        IViewGeneralDataEmployeeWithFamilyRepository ViewGeneralDataEmployeeWithFamilyRepository { get; }
        IViewGeneralDataEmployeeWithFamilyForReportRepository ViewGeneralDataEmployeeWithFamilyForReportRepository { get; }


        IViewEmployeeFamilyReportRepository ViewEmployeeFamilyReportRepository { get; }
        IViewOdsTherapyBookletMisRepository ViewOdsTherapyBookletMisRepository { get; }

        IHrOptionRepository HrOptionRepository { get; }
        IViewShiftBoardRepository ViewShiftBoardRepository { get; }
        IOtherPaymentStatusRepository OtherPaymentStatusRepository { get; }
        IOtherPaymentStatusFlowRepository OtherPaymentStatusFlowRepository { get; }
        IOtherPaymentTypeRepository OtherPaymentTypeRepository { get; }
        IEmployeeOtherPaymentRepository EmployeeOtherPaymentRepository { get; }
        IOtherPaymentHeaderRepository OtherPaymentHeaderRepository { get; }
        IEmployeeOtherPaymentHistoryRepository EmployeeOtherPaymentHistoryRepository { get; }
        IOtherPaymentDetailRepository OtherPaymentDetailRepository { get; }
        IKUnitSettingRepository KUnitSettingRepository { get; }
        IOtherPaymentSettingParameterRepository OtherPaymentSettingParameterRepository { get; }
        IOtherPaymentSettingParameterValueRepository OtherPaymentSettingParameterValueRepository { get; }
        IOtherPaymentHeaderTypeRepository OtherPaymentHeaderTypeRepository { get; }
        IEmployeeBlackListRepository EmployeeBlackListRepository { get; }
        IOtherPaymentPercentageRepository OtherPaymentPercentageRepository { get; }
        IEmployeeVacationHistoryRepository EmployeeVacationHistoryRepository { get; }

        IMission_TypeLocationWorkCityRepository Mission_TypeLocationWorkCityRepository { get; }
        IViewOdsEmployeeWithChartDataRepository ViewOdsEmployeeWithChartDataRepository { get; }
        IViewOnCallRequestRepository ViewOnCallRequestRepository { get; }
        IEmployeeOtherPaymentTemplateRepository EmployeeOtherPaymentTemplateRepository { get; }
        IEmployeeValidOverTimeRepository EmployeeValidOverTimeRepository { get; }

        IVacationRepository VacationRepository { get; }
        IEmployeeVacationManagementRepository EmployeeVacationManagementRepository { get; }
        IEmployeeOtherPaymentTypeRepository EmployeeOtherPaymentTypeRepository { get; }
        IRewardTypeRepository RewardTypeRepository { get; }
        IRewardLevelRepository RewardLevelRepository { get; }
        IStudentRewardSettingRepository StudentRewardSettingRepository { get; }
        IStudentRewardRequestRepository StudentRewardRequestRepository { get; }
        IViewEmployeeWithFamilyActiveRepository ViewEmployeeWithFamilyActiveRepository { get; }
        IEmployeeVacationSoldRepository EmployeeVacationSoldRepository { get; }
        IReasonJobMovingRepository ReasonJobMovingRepository { get; }
        ISignatureImageRepository SignatureImageRepository { get; }
        IInterdictDescriptionRepository InterdictDescriptionRepository { get; }
        IInterdictStatusRepository InterdictStatusRepository { get; }
        IInterdictTypeRepository InterdictTypeRepository { get; }
        IEmployeeInterdictRepository EmployeeInterdictRepository { get; }
        IJobGroupRepository JobGroupRepository { get; }


        IAccountEmploymentTypeRepository AccountEmploymentTypeRepository { get; }
        IAccountPaymentTypeSettingRepository AccountPaymentTypeSettingRepository { get; }
        IBasisSalaryItemPerGroupRepository BasisSalaryItemPerGroupRepository { get; }
        IBasisSalaryItemRepository BasisSalaryItemRepository { get; }
        ICoefficientRepository CoefficientRepository { get; }
        ICoefficientSettingRepository CoefficientSettingRepository { get; }
        IHrFormulasRepository HrFormulasRepository { get; }
        IInterdictMaritalSettingRepository InterdictMaritalSettingRepository { get; }
        IInterdictMaritalSettingDetailRepository InterdictMaritalSettingDetailRepository { get; }
        IPaymentTypeRepository PaymentTypeRepository { get; }
        IInterdictCategoryRepository InterdictCategoryRepository { get; }
        IPropertyAccountTypeRepository PropertyAccountTypeRepository { get; }
        IAccountCodeCompatibleTypeRepository AccountCodeCompatibleTypeRepository { get; }
        IAccountCodeMaritalRepository AccountCodeMaritalRepository { get; }

        IEmployeeHaveTravelRepository EmployeeHaveTravelRepository { get; }
        IPropertyAccountRepository PropertyAccountRepository { get; }
        IAccountCodeCategoryRepository AccountCodeCategoryRepository { get; }
        IPropertyAccountSettingRepository PropertyAccountSettingRepository { get; }
        IPaymentAccountCodeRepository PaymentAccountCodeRepository { get; }
        IAccountCodeCompatibleRepository AccountCodeCompatibleRepository { get; }
        IEmployeeInterdictDetailRepository EmployeeInterdictDetailRepository { get; }
        IEmployeeVacationManagementLogRepository EmployeeVacationManagementLogRepository { get; }
        IInterdictWeatherRepository InterdictWeatherRepository { get; }
        IProductionEfficiencyRepository ProductionEfficiencyRepository { get; }
        ICategoryCoefficientRepository CategoryCoefficientRepository { get; }
        IConditionalAbsenceTypeRepository ConditionalAbsenceTypeRepository { get; }
        IEmployeeConditionalAbsenceRepository EmployeeConditionalAbsenceRepository { get; }
        IConditionalAbsenceSubjectRepository ConditionalAbsenceSubjectRepository { get; }
        IConditionalAbsenceSubjectTypeRepository ConditionalAbsenceSubjectTypeRepository { get; }
        IMeritRatingRepository MeritRatingRepository { get; }
        IEmployeePercentMeritHistoryRepository EmployeePercentMeritHistoryRepository { get; }
        IHistoryTypeEmployementRepository HistoryTypeEmployementRepository { get; }
        IEmployeeHistoryTypeEmployementRepository EmployeeHistoryTypeEmployementRepository { get; }
        IDisConnectionTypeRepository DisConnectionTypeRepository { get; }
        IEmployeeDisConnectionRepository EmployeeDisConnectionRepository { get; }
        IEmployeePromotionRepository EmployeePromotionRepository { get; }
        IPromotionRejectReasonRepository PromotionRejectReasonRepository { get; }
        IEmployeeDateInformationRepository EmployeeDateInformationRepository { get; }
        IPromotionStatusRepository PromotionStatusRepository { get; }
        IEducationCategoryRepository EducationCategoryRepository { get; }
        IEventListenerRepository EventListenerRepository { get; }
        IPaymentDetailRepository PaymentDetailRepository { get; }
        IPaymentHeaderRepository PaymentHeaderRepository { get; }
        IPaymentProcessRepository PaymentProcessRepository { get; }
        IPaymentProcessCategoryRepository PaymentProcessCategoryRepository { get; }
        IEmployeePromotionInterdictsRepository EmployeePromotionInterdictsRepository { get; }

        IEmployeePromotionSuggestionRepository EmployeePromotionSuggestionRepository { get; }

        IView_MIS_Employee_HomeRepository View_MIS_Employee_HomeRepository { get; }
        IViewFamilyForOtherPaymentMariedAndBirthDayRepository ViewFamilyForOtherPaymentMariedAndBirthDayRepository { get; }
        IViewEmployeeForOtherPaymentMariedAndBirthDayRepository ViewEmployeeForOtherPaymentMariedAndBirthDayRepository { get; }
        //reward
        IRewardInSmcDailyProductionRepository RewardInSmcDailyProductionRepository { get; }
        IRewardInDailyProductionSaleRepository RewardInDailyProductionSaleRepository { get; }
        IRewardInRepository RewardInRepository { get; }
        IRewardBaseDetailRepository RewardBaseDetailRepository { get; }
        IRewardBaseHeaderRepository RewardBaseHeaderRepository { get; }
        IRewardOutHeaderRepository RewardOutHeaderRepository { get; }
        IRewardOutDetailRepository RewardOutDetailRepository { get; }
        IRewardInQualityControlMonthlyPeletRepository RewardInQualityControlMonthlyPeletRepository { get; }
        IRewardInQualityControlMonthlyDriRepository RewardInQualityControlMonthlyDriRepository { get; }
        IRewardInMonthlyProductionSaleRepository RewardInMonthlyProductionSaleRepository { get; }

        IRewardInQualityControlMonthlyProductionRepository RewardInQualityControlMonthlyProductionRepository { get; }

        IRewardInSmcMonthlyProductionRepository RewardInSmcMonthlyProductionRepository { get; }
        IEmployeeSafetyDeductionRepository EmployeeSafetyDeductionRepository { get; }
        IRewardBaseSpecificRepository RewardBaseSpecificRepository { get; }
        IRewardUnitTypeRepository RewardUnitTypeRepository { get; }
        IRewardBaseSpecificOfUnitDetailRepository RewardBaseSpecificOfUnitDetailRepository { get; }
        IRewardBaseSpecificOfUnitHeaderRepository RewardBaseSpecificOfUnitHeaderRepository { get; }
        IEmployeeRewardRepository EmployeeRewardRepository { get; }

        #region StandBy Repositories Interface

        IStandbyEmloyeeBoardRepository StandbyEmloyeeBoardRepository { get; }
        IStandbyEmloyeeBoardLogRepository StandbyEmloyeeBoardLogRepository { get; }
        IStandbyEmployeeRoleRepository StandbyEmployeeRoleRepository { get; }
        IStandbyHeaderRepository StandbyHeaderRepository { get; }
        IStandbyRoleRepository StandbyRoleRepository { get; }
        IStandbyTurnRepository StandbyTurnRepository { get; }
        IStandbyReplacementRequestsRepository StandbyReplacementRequestsRepository { get; }

        #endregion
        IDeductionAdditionalRepository DeductionAdditionalRepository { get; }
        IEmployeeDeductionAdditionalRepository EmployeeDeductionAdditionalRepository { get; }



        IEmployeeDeductionHeaderRepository EmployeeDeductionHeaderRepository { get; }

        IEmployeeDeductionDetailRepository EmployeeDeductionDetailRepository { get; }

        //
        IPaymentAdditionalDetailRepository PaymentAdditionalDetailRepository { get; }
        IPaymentAdditionalHeaderRepository PaymentAdditionalHeaderRepository { get; }
        IPaymentAdditionalSettingRepository PaymentAdditionalSettingRepository { get; }
        IPaymentAdditionalSettingJobCategoryRepository PaymentAdditionalSettingJobCategoryRepository { get; }
        IPaymentAdditionalSettingWorkCityRepository PaymentAdditionalSettingWorkCityRepository { get; }
        IIncreaseSalaryHeaderRepository IncreaseSalaryHeaderRepository { get; }
        IIncreaseSalaryDetailRepository IncreaseSalaryDetailRepository { get; }
        IPaymentAdditionalSettingJobPositionRepository PaymentAdditionalSettingJobPositionRepository { get; }

        IConfirmInterdictMessageRepository ConfirmInterdictMessageRepository { get; }
        IConfirmInterdictRepository ConfirmInterdictRepository { get; }
        IConfirmInterdictStatusRepository ConfirmInterdictStatusRepository { get; }

        //security
        IUserDefinitionRepository UserDefinitionRepository { get; }
        IUserDefinitionSecurityPriorityStatusRepository UserDefinitionSecurityPriorityStatusRepository { get; }
        IUserDefinitionSecurityTeamWorkRepository UserDefinitionSecurityTeamWorkRepository { get; }


        IJobPositionTeamWorkRepository JobPositionTeamWorkRepository { get; }

        IJobPositionNatureSubGroupRepository JobPositionNatureSubGroupRepository { get; }
        //relocation
        IRelocationRepository RelocationRepository { get; }
        IRelocationStatusRepository RelocationStatusRepository { get; }
        IRelocationTypeRepository RelocationTypeRepository { get; }
        IViewMisEmployeeDisciplineRepository ViewMisEmployeeDisciplineRepository { get; }
        IView_PresentEmployeesRepository View_PresentEmployeesRepository { get; }

        IRollCallWorkTimeMonthSettingRepository RollCallWorkTimeMonthSettingRepository { get; }
        IInvalidDayTypeInForcedOvertimeRepository InvalidDayTypeInForcedOvertimeRepository { get; }
        IRollCallWorkCityRepository RollCallWorkCityRepository { get; }
        IJobCategoryDefinitionGuideRepository JobCategoryDefinitionGuideRepository { get; }

        IEmployeeDeductionTypeRepository EmployeeDeductionTypeRepository { get; }
        IEmployeeDeductionTypeAccessRepository EmployeeDeductionTypeAccessRepository { get; }



        IEmployeeLoanDeductionHeaderRepository EmployeeLoanDeductionHeaderRepository { get; }
        IEmployeeLoanDeductionDetailRepository EmployeeLoanDeductionDetailRepository { get; }

        IEmployeeDeductionTempRepository EmployeeDeductionTempRepository { get; }

        IAccountCodeDeductionTypeRepository AccountCodeDeductionTypeRepository { get; }

        IViewMisPayrollRepository ViewMisPayrollRepository { get; }
        IView_EmployeeRepository View_EmployeeRepository { get; }

        IEmployeeDeductionActivateStatusRepository EmployeeDeductionActivateStatusRepository { get; }

        IBudgetRewardHeaderRepository BudgetRewardHeaderRepository { get; }
        IBudgetRewardDetailRepository BudgetRewardDetailRepository { get; }
        IBudgetRewardTypeRepository BudgetRewardTypeRepository { get; }
        IBudgetRewardStatusRepository BudgetRewardStatusRepository { get; }

        IBudgetRewardEmployeeRepository BudgetRewardEmployeeRepository { get; }
        IBudgetRewardEmployeeHistoryRepository BudgetRewardEmployeeHistoryRepository { get; }


        ILicenseJobPositionRepository LicenseJobPositionRepository { get; }
        ILicenseTypeRepository LicenseTypeRepository { get; }

    }

}