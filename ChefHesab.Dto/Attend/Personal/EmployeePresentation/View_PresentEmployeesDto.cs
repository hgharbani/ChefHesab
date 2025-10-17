using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.View_PresentEmployees
{

    public class DetailSearchPresentationDto { 
        public DetailSearchPresentationDto()
        {
            MisJobPositionCodes = new List<string>();
        }
        public int? TeamWorkId { get; set; }
        public decimal? CostCenter { get; set; }
        public decimal? ContractorID { get; set; }

        public int? JobPositionId { get; set; }
        public List<string> MisJobPositionCodes { get; set; }

    }

    public class SearchPresentationDto:FilterRequest
    {
        public SearchPresentationDto()
        {
            MisJobPositionCodes = new List<string>();
        }
        public int? TeamWorkId { get; set; }
        public decimal? CostCenter { get; set; }
        public decimal? ContractorID { get; set; }

        public int? JobPositionId { get; set; }
        public List<string> MisJobPositionCodes { get; set; }

        public string CurrentUser { get; set; }
    }




    public class View_PresentEmployeesDto
    {
        public long? Id { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public int? TeamWorkId { get; set; }
        public int? JobPositionId { get; set; }
        public bool? WorkDay { get; set; }
        public bool? IsPresent { get; set; }
        public string TeamWorkCode { get; set; }
        public string TeamWorkTitle { get; set; }
        public string MisJobPositionCode { get; set; }
        public string JobPositionTitle { get; set; }
        public decimal? CostCenter { get; set; }
        public bool? IsKscEmployee { get; set; }
        public string ContractCode { get; set; }
        public string ContractDescription { get; set; }
        public string Contractor { get; set; }
        public decimal? ContractorID { get; set; }

    }


    public class TeamWorkPresentation
    {

        public int TeamWorkId { get; set; }
        public int TeamWrokCode { get; set; }

        public string TeamWrokTitle { get; set; }

        public int ShiftWorkEmployeeCount { get; set; }
        public int WorkDayEmployeeCount { get; set; }
        public int SumEmployeeInCompany { get { return ShiftWorkEmployeeCount + WorkDayEmployeeCount; } }
    }

    public class CostCenterPresentationDto
    {
        public decimal? CostCenter { get; set; }
        public string CostCenterTitle { get; set; }

        public int ShiftWorkEmployeeCount { get; set; }
        public int WorkDayEmployeeCount { get; set; }
        public int SumEmployeeInCompany { get { return ShiftWorkEmployeeCount + WorkDayEmployeeCount; } }
    }


    public class ContractPresentationDto
    {

        public string Contractor { get; set; }
        public decimal? ContractorID { get; set; }

        public int ShiftWorkEmployeeCount { get; set; }
        public int WorkDayEmployeeCount { get; set; }
        public int SumEmployeeInCompany { get { return ShiftWorkEmployeeCount + WorkDayEmployeeCount; } }
    }


    public class JobPositionDetailsDto
    {
        public JobPositionDetailsDto()
        {
            ChildMisJobpositionCode = new List<string>();
        }
        public int JobpositionId { get; set; }
        public string MisJobpositionCode { get; set; }
        public string JobPositionTitle { get; set; }

        public int ShiftWorkEmployeeCount { get; set; }
        public int WorkDayEmployeeCount { get; set; }
        public int SumEmployeeInCompany { get { return ShiftWorkEmployeeCount + WorkDayEmployeeCount; } }

        public List<string> ChildMisJobpositionCode { get; set; }
        public decimal? CostCenter { get; set; }
    }

    public class WorkGroupPresentation
    {
        public string Title { get; set; }
        public int TotalCount { get; set; }
        public int InCompanyCount { get; set; }
        public List<string> EmployeeNumbers { get; set; }= new List<string>() ;
    }
}

