using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.MIS
{
    public class UpdateTeamAndGroupInputModel
    {
        public string Domain { get; set; }
        /// <summary>
        /// MIS نام فانکشن برای استفاده از روتین در
        /// </summary>
        public string FUNCTION { get; set; }
        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public string NUM_PRSN { get; set; }
        /// <summary>
        /// تیم فعلی
        /// </summary>
        public string NUM_TEAM_EMPL { get; set; }
        /// <summary>
        /// تیم جدید
        /// </summary>
        public string NUM_TEAM_LIST { get; set; }
        /// <summary>
        /// تاریخ شروع تیم فعلی _ 8 رقمی باشد 
        /// </summary>
        public string DAT_STR_TEAM { get; set; }
        /// <summary>
        /// تاریخ شروع تیم جدید _ 8 رقمی باشد
        /// </summary>
        public string STR_TEAM_LIST { get; set; }
        /// <summary>
        /// زمان کار جدید
        /// </summary>
        public string COD_TYP_LIST { get; set; }
        /// <summary>
        /// کد گروه جدید
        /// </summary>
        public string COD_GRP_LIST { get; set; }
        /// <summary>
        /// تاریخ تغییر گروه کاری _ 8 رقمی باشد
        /// </summary>
        public string STR_WORK_LIST { get; set; }
        /// <summary>
        /// شماره گروه
        /// </summary>
        public string NUM_GRP_EMGRP { get; set; }

        /// <summary>
        /// شماره گروه
        /// </summary>
        public string DES_GRP_EMGRP { get; set; }



        /// <summary>
        /// نوع گروه
        /// 1-ستادی2-تعمیراتی3-عملیاتی4-مجازی-5-مامورین	
        /// </summary>
        public string FLG_MAN_OPE_EMGRP { get; set; }

        /// <summary>
        /// مرکز هزینه
        /// </summary>
        public string FK_CCRRX { get; set; }

        /// <summary>
        /// کد مدیریت
        /// </summary>
        public string COD_MNGT { get; set; }

        /// <summary>
        /// فلگ گروه کاری
        ///1- ایجاد گروه 2- آپدیت گروه 3- حذف گروه	
        /// </summary>
        public string FLG_GRP_CRUD { get; set; }

        /// <summary>
        /// شماره گروه
        /// </summary>
        public string NUM_GRP_EMGRP_TEAM { get; set; }

        /// <summary>
        /// نام تیم
        /// </summary>
        public string NUM_TEAM { get; set; }

        /// <summary>
        /// عنوان تیم 
        /// </summary>
        public string DES_TEAM { get; set; }

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public string DAT_STR  { get; set; }

        /// <summary>
        /// تاریخ خاتمه
        /// </summary>
        public string DAT_END { get; set; }

        /// <summary>
        /// کد میانگین و سقف اضافه کار
        /// </summary>
        public string COD_OVT { get; set; }

        /// <summary>
        /// 1-ایجاد تیم 2- ویرایش تیم 3-حذف تیم	
        /// </summary>
        public string FLG_TEAM_CRUD { get; set; }
    }
}
