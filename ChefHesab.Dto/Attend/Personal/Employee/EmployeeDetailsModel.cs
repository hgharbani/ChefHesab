using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeDetailsModel
    {
        public EmployeeDetailsModel() { }

        public int Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName => Name + " " + Family;
        public string EmployeeNumberFullName => EmployeeNumber + " - " +  Name + " " + Family;

        public int EmploymentTypeId { get; set; }
        public string EmploymentTypeTitle { get; set; }
      
        public string EmploymentTypeDisplay=> EmploymentTypeId + " - " + EmploymentTypeTitle;  

        public DateTime? EmploymentDate { get; set; }
        public string EmploymentDatePersian { get; set; }

        public string EmploymeEducationDegree { get; set; }

        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamTitle { get; set; }
        public string TeamDisplay => TeamCode + " - " + TeamTitle + " / " + WorktimeTitle;

        public int JobPositionId { get; set; }
        public string JobPositionCode { get; set; }
        public string JobPositionTitle { get; set; }
        public string JobPositionDisplay => JobPositionCode + " - " + JobPositionTitle; 

        public int StructureId { get; set; }
        public string StructureCode { get; set; }
        public string StructureTitle { get; set; }
        public string StructureDisplay => StructureCode + " - " + StructureTitle;

        public string DefinitionPostTitle { get; set; }
        public string GroupNumber { get; set; }
        public string JobIdentityTitle { get; set; }
        public string JobIdentityCode { get; set; }
        public string JobIdentityDisplay => JobIdentityCode + "(" + JobIdentityTitle + ") - " +  DefinitionPostTitle;
        public string Asistance { get; set; }
        public string EducationTitle { get; set; }
        public int EducationId { get; set; }
        public string StudyFieldTitle { get; set; }
        public int StudyFieldId { get; set; }
        public string EducationDisplay => EducationTitle + " - " + StudyFieldTitle;

        public string EducationJobPositionTitle { get; set; }
        public int EducationJobPositionTitleId { get; set; }
        public string StudyFieldJobPositionTitle { get; set; }
        public int StudyFieldJobPositionId { get; set; }
        public string EducationJobPositionDisplay => EducationJobPositionTitle + " - " + StudyFieldJobPositionTitle;


        public int? PaymentStatusId { get; set; }
        public string PaymentStatusTitle { get; set; }
        public int? WorkGroupId { get; set; }
        public int WorktimeId { get; set; }
        public string WorktimeTitle { get; set; }
        public int? WorkCityId { get; set; }

        public int JobCategoryEducationId { get; set; }

        public int JobPositionFieldId { get; set; }

        public int EmploymeEducationDegreeId { get; set; }

        public int JobCategoryDefinitionId { get; set; }
        public string IsarStatusTitle { get; set; }
    }
}
