using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.General
{
    public class BaseModel
    {
        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime? InsertDate { get; set; }
        /// <summary>
        /// کاربر ثبت کننده
        /// </summary>
        [StringLength(50)]
        public string InsertUser { get; set; }
        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// کاربر ویرایش کننده
        /// </summary>
        [StringLength(50)]
        public string UpdateUser { get; set; }
        /// <summary>
        /// / دامنه
        /// </summary>
        [StringLength(50)]
        public string DomainName { get; set; }
        /// <summary>
        /// بررسی فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
