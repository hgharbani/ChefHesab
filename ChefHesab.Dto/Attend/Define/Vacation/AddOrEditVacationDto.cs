using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.Vacation
{
    public class AddOrEditVacationDto
    {
        public AddOrEditVacationDto()
        {
        }
        public int Id { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }
        public System.Byte? OrderNo { get; set; }
        public bool? ShowInHistory { get; set; }
        public bool? ShowInManagement { get; set; }
        public bool? ReadonlyInManagement { get; set; }
    }
}

