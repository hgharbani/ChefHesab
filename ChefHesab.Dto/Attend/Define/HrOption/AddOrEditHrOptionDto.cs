using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.HrOption
{
    public class AddOrEditHrOptionDto
    {
        public AddOrEditHrOptionDto()
        {
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public int? OptionTypeId { get; set; }

        public string ValueString { get; set; }

        public DateTime? ValueTime { get; set; }

        public double? ValueFloat { get; set; }

        public System.Byte? Code { get; set; }

        public bool IsClosed { get; set; }

    }
}

