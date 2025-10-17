  using System;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.View_MIS_Employee_Home
{
   public class View_MIS_Employee_HomeDto
  {
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Category { get; set; }
        public string StructureCategory { get; set; }
        public decimal? YearMonth { get; set; }
        public string MaxDate { get; set; }
        public string RemainDate { get; set; }
        public decimal? HouseScore { get; set; }
        public decimal? InHouseYear { get; set; }
        public decimal? InHouseMonth { get; set; }
        public decimal? InHouseDay { get; set; }
        public string RemainYear { get; set; }
        public string RemainMonth { get; set; }
        public string RemainDay { get; set; }

    }
  }

