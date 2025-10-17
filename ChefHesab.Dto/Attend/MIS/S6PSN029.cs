using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ksc.HR.DTO.MIS;

[XmlRoot("PAR_MONT")]
public partial class PAR_MONT
{
    public PAR_MONT()
    {
       this.PER_MONT = new List<PER_MONT>();
    }
    public string OPERATION { get; set; }
    /// <summary>
    /// تاریخ کارکرد (سال و ماه)
    /// </summary>
    public string DAT_ATT_MONTP { get; set; }
    /// <summary>
    /// شمارنده (تعداد پرسنل ارسالی)
    /// </summary>
    public string COUNT_PER { get; set; }
    /// <summary>
    /// آرایه پرسنل به همراه دیتای مورد نیاز
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("PER_MONT")]
    public List<PER_MONT> PER_MONT { get; set; }

}

[XmlRoot("PER_MONT")]
public partial class PER_MONT
{
    public PER_MONT()
    {
        this.MONTHLY_SHIFT = new List<MONTHLY_SHIFT>();
        this.MONTHLY_HOURS = new List<MONTHLY_HOURS>();
    }
    /// <summary>
    /// شماره پرسنلی
    /// </summary>
    public string NUM_PRSN_EMPL { get; set; }
    /// <summary>
    /// تعداد روز مشمول عیدی
    /// </summary>
    public string NUM_BNS_DAY_MONTP { get; set; }
    /// <summary>
    /// تعداد روز مشمول بیمه
    /// </summary>
    public string NUM_PNS_DAY_MONTP { get; set; }
    /// <summary>
    /// تعداد روز  مشمول مالیات
    /// </summary>
    public string NUM_TAX_DAY_MONTP { get; set; }
    /// <summary>
    /// تعداد روز کارکرد
    /// </summary>
    public string NUM_WRK_DAY_MONTP { get; set; }
    /// <summary>
    /// مدت ساعت عدم پرداخت حقوق
    /// </summary>
    public string TOT_HOUR_NOPYM_MONTP { get; set; }
    /// <summary>
    /// عدم پرداخت پاداش
    /// </summary>
    public string TOT_HOUR_NOPYM_PREM_MONTP { get; set; }
    /// <summary>
    /// اضافه کار مازاد سقف
    /// </summary>
    public string NUM_HOUR_OVT_PAY_EMPL { get; set; }
    /// <summary>
    /// کاربر
    /// </summary>
    public string NAM_USR_DAT_UPD_MONTP { get; set; }
    /// <summary>
    /// تعداد روز عدم پرداخت 
    /// </summary>
    public string NUM_DAY_NOPYM_PREM_MONTP { get; set; }
    /// <summary>
    /// ساعت عدم پرداخت 
    /// </summary>
    public string TOT_HOUR_NOPYM_HLDY_MONTP { get; set; }
    /// <summary>
    /// اضافاه کار تایید نشده
    /// </summary>
    public string NUM_TIM_HOUR_EOVT_MONTP { get; set; }
    /// <summary>
    /// ایام بين راهي
    /// </summary>
    public string NUM_TRSP_DAY_MONTP { get; set; }
    /// <summary>
    /// شمارنده (تعداد شیفت کاری)
    /// </summary>
    public string COUNT_SHIFT { get; set; }
    /// <summary>
    /// آرایه  شیفت کاری
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("MONTHLY_SHIFT")]
    public List<MONTHLY_SHIFT> MONTHLY_SHIFT { get; set; }
    /// <summary>
    /// تعداد اطلاعات کارکردی
    /// </summary>
    public string COUNT_HOURS { get; set; }
    /// <summary>
    /// آرایه اطلاعات کارکردی 
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("MONTHLY_HOURS")]
    public List<MONTHLY_HOURS> MONTHLY_HOURS { get; set; }
}

[XmlRoot("MONTHLY_SHIFT")]
public partial class MONTHLY_SHIFT
{
    /// <summary>
    /// نوع زمان کار
    /// </summary>
    public string COD_TYP_WRK_EMPL { get; set; }
    /// <summary>
    ///  تعداد روزهای شیفت (مدت حضور در شيفت (جاري))
    /// </summary>
    public string NUM_DAY_SHIFT_MONTP { get; set; }
}

[XmlRoot("MONTHLY_HOURS")]
public partial class MONTHLY_HOURS
{
    /// <summary>
    /// کد حضور غیاب
    /// </summary>
    public string COD_ATT_ATABT { get; set; }
    /// <summary>
    /// ساعت کارکرد
    /// </summary>
    public string NUM_TIM_HOUR_MONTP { get; set; }
    /// <summary>
    /// کد حساب
    /// </summary>
    public string FK_ACNDF { get; set; }

}
