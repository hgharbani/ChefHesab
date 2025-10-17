
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallCategory
{
    public class AddRollCallCategoryModel
    {
        public int Id { get; set; }
        [DisplayName("عنوان")]
        public string Title { get; set; }
        [DisplayName("کد")]
        public string Code { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

    }
}
