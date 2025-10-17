
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWorkMangementCode
{
    public class AddTeamWorkMangementCodeModel
    {
        public int Id { get; set; }
        [DisplayName("عنوان")]
        public string Title { get; set; } = null!;
        [DisplayName("کد")]
        public string Code { get; set; } = null!;
        
        public DateTime? InsertDate { get; set; }
        //public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; }
        [DisplayName("فعال")]
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

    }
}
