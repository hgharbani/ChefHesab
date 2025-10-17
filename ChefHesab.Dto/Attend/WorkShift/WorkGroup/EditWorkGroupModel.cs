using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;

namespace Ksc.HR.DTO.WorkShift.WorkGroup
{
    public class EditWorkGroupModel
    {
        //public WorkGroup()
        //{
        //    ShiftBoard = new HashSet<ShiftBoard>();
        //}

        public int Id { get; set; }
        public string Code { get; set; }
        public int WorkTimeId { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; }
        [DisplayName("فعال")]

        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; }

        //public virtual WorkTime WorkTime { get; set; }
        //public virtual ICollection<ShiftBoard> ShiftBoard { get; set; }

    }
}
