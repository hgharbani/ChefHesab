using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.WorkShift.TeamWork;

public class TeamWorkCostCenterViewModel
{
    public string DisplayMember
    {
        get
        {
            return $"{Code} - {Title.Trim()} - {CostCenter}";
        }
    }

    [Display(Name = "شناسه")]
    public int Id { get; set; }

    [Display(Name = "عنوان تیم")]
    public string Title { get; set; }

    [Display(Name = "کد تیم")]
    public string Code { get; set; }

    [Display(Name = "مرکز هزینه")]
    public string CostCenter { get; set; }
}
